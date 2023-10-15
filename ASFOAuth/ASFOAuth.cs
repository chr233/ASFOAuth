using ArchiSteamFarm.Core;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using ASFOAuth.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Composition;
using System.Reflection;
using System.Text;

namespace ASFOAuth;

[Export(typeof(IPlugin))]
internal sealed class ASFOAuth : IASF, IBotCommand2
{
    public string Name => nameof(ASFOAuth);
    public Version Version => Utils.MyVersion;

    private AdapterBtidge? ASFEBridge = null;

    [JsonProperty]
    public static PluginConfig Config => Utils.Config;

    private Timer? StatisticTimer;

    /// <summary>
    /// ASF启动事件
    /// </summary>
    /// <param name="additionalConfigProperties"></param>
    /// <returns></returns>
    public Task OnASFInit(IReadOnlyDictionary<string, JToken>? additionalConfigProperties = null)
    {
        PluginConfig? config = null;

        if (additionalConfigProperties != null)
        {
            foreach ((string configProperty, JToken configValue) in additionalConfigProperties)
            {
                if (configProperty == nameof(ASFOAuth) && configValue.Type == JTokenType.Object)
                {
                    try
                    {
                        config = configValue.ToObject<PluginConfig>();
                        if (config != null)
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Utils.ASFLogger.LogGenericException(ex);
                    }
                }
            }
        }

        Utils.Config = config ?? new();

        //统计
        if (Config.Statistic)
        {
            Uri request = new("https://asfe.chrxw.com/");
            StatisticTimer = new Timer(
                async (_) =>
                {
                    await ASF.WebBrowser!.UrlGetToHtmlDocument(request).ConfigureAwait(false);
                },
                null,
                TimeSpan.FromSeconds(30),
                TimeSpan.FromHours(24)
            );
        }

        //禁用命令
        if (Config.DisabledCmds == null)
        {
            Config.DisabledCmds = new();
        }
        else
        {
            for (int i = 0; i < Config.DisabledCmds.Count; i++)
            {
                Config.DisabledCmds[i] = Config.DisabledCmds[i].ToUpperInvariant();
            }
        }

        Utils.ASFLogger.LogGenericWarning(Static.Line);
        Utils.ASFLogger.LogGenericWarning(Langs.RiskWarning);
        Utils.ASFLogger.LogGenericWarning(Static.Line);

        return Task.CompletedTask;
    }

    /// <summary>
    /// 插件加载事件
    /// </summary>
    /// <returns></returns>
    public Task OnLoaded()
    {
        try
        {
            var flag = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            var handler = typeof(ASFOAuth).GetMethod(nameof(ResponseCommand), flag);

            const string pluginName = nameof(ASFOAuth);
            const string cmdPrefix = "ASFO";
            const string repoName = "ASFOAuth";

            ASFEBridge = AdapterBtidge.InitAdapter(pluginName, cmdPrefix, repoName, handler);
            ASF.ArchiLogger.LogGenericDebug(ASFEBridge != null ? "ASFEBridge 注册成功" : "ASFEBridge 注册失败");
        }
        catch (Exception ex)
        {
            ASF.ArchiLogger.LogGenericDebug("ASFEBridge 注册出错");
            ASF.ArchiLogger.LogGenericException(ex);
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// 处理命令
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="access"></param>
    /// <param name="cmd"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static Task<string?>? ResponseCommand(Bot bot, EAccess access, string cmd, string[] args)
    {
        int argLength = args.Length;
        return argLength switch
        {
            0 => throw new InvalidOperationException(nameof(args)),
            1 => cmd switch  //不带参数
            {
                //Update
                "ASFOAUTH" or
                "ASFO" when access >= EAccess.FamilySharing =>
                   Task.FromResult(Update.Command.ResponseASFBuffBotVersion()),

                _ => null,
            },
            _ => cmd switch //带参数
            {
                //Core
                "OAUTH" or
                "O" when argLength == 3 && access >= EAccess.Master =>
                     Core.Command.OAuth(args[1], args[2]),

                "OAUTH" or
                "O" when argLength == 2 && access >= EAccess.Master =>
                     Core.Command.OAuth(bot, args[1]),

                _ => null,
            },
        };
    }

    /// <summary>
    /// 处理命令事件
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="access"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <param name="steamId"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<string?> OnBotCommand(Bot bot, EAccess access, string message, string[] args, ulong steamId = 0)
    {
        if (ASFEBridge != null)
        {
            return null;
        }

        if (!Enum.IsDefined(access))
        {
            throw new InvalidEnumArgumentException(nameof(access), (int)access, typeof(EAccess));
        }

        try
        {
            var cmd = args[0].ToUpperInvariant();

            if (cmd.StartsWith("ASFO."))
            {
                cmd = cmd[5..];
            }

            var task = ResponseCommand(bot, access, cmd, args);
            if (task != null)
            {
                return await task.ConfigureAwait(false);
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            _ = Task.Run(async () =>
            {
                await Task.Delay(500).ConfigureAwait(false);
                Utils.ASFLogger.LogGenericException(ex);
            }).ConfigureAwait(false);

            return ex.StackTrace;
        }
    }
}
