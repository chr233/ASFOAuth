using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;

namespace ASFOAuth.Core;

internal static class Command
{
    internal static async Task<string?> OAuth(Bot bot, string url)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }
        var response = await WebRequest.LoginViaSteamOAuth(bot, url).ConfigureAwait(false);
        return bot.FormatBotResponse(response);
    }

    internal static async Task<string?> OAuth(string botName, string url)
    {
        var bot = Bot.GetBot(botName);
        if (bot == null)
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, botName));
        }
        return await OAuth(bot, url).ConfigureAwait(false);
    }

    internal static async Task<string?> OpenId(Bot bot, string url)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }
        var response = await WebRequest.LoginViaSteamOpenId(bot, url).ConfigureAwait(false);
        return bot.FormatBotResponse(response);
    }

    internal static async Task<string?> OpenId(string botName, string url)
    {
        var bot = Bot.GetBot(botName);
        if (bot == null)
        {
            return FormatStaticResponse(string.Format(Strings.BotNotFound, botName));
        }
        return await OpenId(bot, url).ConfigureAwait(false);
    }
}
