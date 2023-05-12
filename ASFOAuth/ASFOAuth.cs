using ArchiSteamFarm.Core;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using ASFOAuth.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Composition;
using System.Text;

namespace ASFOAuth;

[Export(typeof(IPlugin))]
internal sealed class ASFOAuth : IASF, IBotCommand2
{
    public string Name => nameof(ASFOAuth);
    public Version Version => Utils.MyVersion;

    [JsonProperty]
    public static PluginConfig Config => Utils.Config;

    private static Timer? StatisticTimer;

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
                        Utils.Logger.LogGenericException(ex);
                    }
                }
            }
        }

        Utils.Config = config ?? new();

        StringBuilder warning = new();

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

        if (warning.Length > 0)
        {
            warning.Insert(0, Environment.NewLine);
            Utils.Logger.LogGenericWarning(warning.ToString());
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// 插件加载事件
    /// </summary>
    /// <returns></returns>
    public Task OnLoaded()
    {
        StringBuilder message = new("\n");
        message.AppendLine(Static.Line);
        message.AppendLine(Static.Logo);
        message.AppendLine(Static.Line);
        message.AppendLine(string.Format(Langs.PluginVer, nameof(ASFOAuth), Utils.MyVersion.ToString()));
        message.AppendLine(Langs.PluginContact);
        message.AppendLine(Langs.PluginInfo);
        message.AppendLine(Static.Line);

        string pluginFolder = Path.GetDirectoryName(Utils.MyLocation) ?? ".";
        string backupPath = Path.Combine(pluginFolder, $"{nameof(ASFOAuth)}.bak");
        bool existsBackup = File.Exists(backupPath);
        if (existsBackup)
        {
            try
            {
                File.Delete(backupPath);
                message.AppendLine(Langs.CleanUpOldBackup);
            }
            catch (Exception e)
            {
                Utils.Logger.LogGenericException(e);
                message.AppendLine(Langs.CleanUpOldBackupFailed);
            }
        }
        else
        {
            message.AppendLine(Langs.ASFEVersionTips);
            message.AppendLine(Langs.ASFEUpdateTips);
        }

        message.AppendLine(Static.Line);

        Utils.Logger.LogGenericInfo(message.ToString());

        return Task.CompletedTask;
    }

    /// <summary>
    /// 处理命令
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="access"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <param name="steamId"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static async Task<string?> ResponseCommand(Bot bot, EAccess access, string message, string[] args)
    {
        string cmd = args[0].ToUpperInvariant();

        if (cmd.StartsWith("ABB."))
        {
            cmd = cmd[4..];
        }
        else
        {
            //跳过禁用命令
            if (Config.DisabledCmds?.Contains(cmd) == true)
            {
                Utils.Logger.LogGenericInfo("Command {0} is disabled!");
                return null;
            }
        }

        int argLength = args.Length;
        switch (argLength)
        {
            case 0:
                throw new InvalidOperationException(nameof(args));
            case 1: //不带参数
                switch (cmd)
                {
                    //Core
                    //case "TEST" when access >= EAccess.Master:
                    //case "T" when access >= EAccess.Master:
                    //    return await Core.Command.Test(bot).ConfigureAwait(false);

                    //Update
                    case "ASFBUFFBOT" when access >= EAccess.FamilySharing:
                    case "ABB" when access >= EAccess.FamilySharing:
                        return Update.Command.ResponseASFBuffBotVersion();

                    case "ABBVERSION" when access >= EAccess.Operator:
                    case "ABBV" when access >= EAccess.Operator:
                        return await Update.Command.ResponseCheckLatestVersion().ConfigureAwait(false);

                    case "ABBUPDATE" when access >= EAccess.Owner:
                    case "ABBU" when access >= EAccess.Owner:
                        return await Update.Command.ResponseUpdatePlugin().ConfigureAwait(false);

                    default:
                        return null;
                }
            default: //带参数
                switch (cmd)
                {
                    //Core
                    case "TEST" when argLength == 3 && access >= EAccess.Master:
                    case "T" when argLength == 3 && access >= EAccess.Master:
                        return await Core.Command.Test(args[1], args[2]).ConfigureAwait(false);

                    case "TEST" when argLength == 2 && access >= EAccess.Master:
                    case "T" when argLength == 2 && access >= EAccess.Master:
                        return await Core.Command.Test(bot, args[1]).ConfigureAwait(false);

                    default:
                        return null;
                }
        }
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
        if (!Enum.IsDefined(access))
        {
            throw new InvalidEnumArgumentException(nameof(access), (int)access, typeof(EAccess));
        }

        try
        {
            return await ResponseCommand(bot, access, message, args).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            string version = await bot.Commands.Response(EAccess.Owner, "VERSION").ConfigureAwait(false) ?? "Unknown";
            var i = version.LastIndexOf('V');
            if (i >= 0)
            {
                version = version[++i..];
            }
            string cfg = JsonConvert.SerializeObject(Config, Formatting.Indented);

            StringBuilder sb = new();
            sb.AppendLine(Langs.ErrorLogTitle);
            sb.AppendLine(Static.Line);
            sb.AppendLine(string.Format(Langs.ErrorLogOriginMessage, message));
            sb.AppendLine(string.Format(Langs.ErrorLogAccess, access.ToString()));
            sb.AppendLine(string.Format(Langs.ErrorLogASFVersion, version));
            sb.AppendLine(string.Format(Langs.ErrorLogPluginVersion, Utils.MyVersion));
            sb.AppendLine(Static.Line);
            sb.AppendLine(cfg);
            sb.AppendLine(Static.Line);
            sb.AppendLine(string.Format(Langs.ErrorLogErrorName, ex.GetType()));
            sb.AppendLine(string.Format(Langs.ErrorLogErrorMessage, ex.Message));
            sb.AppendLine(ex.StackTrace);

            _ = Task.Run(async () =>
            {
                await Task.Delay(500).ConfigureAwait(false);
                sb.Insert(0, '\n');
                Utils.Logger.LogGenericError(sb.ToString());
            }).ConfigureAwait(false);

            return sb.ToString();
        }
    }
}
