using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;

namespace ASFOAuth.Core;

internal static class Command
{
    internal static async Task<string?> Test(Bot bot, string url)
    {
        var response = await WebRequest.LoginViaSteamOAuth(bot, url).ConfigureAwait(false);
        return response;
    }

    internal static async Task<string?> Test(string botName, string url)
    {
        var bot = Bot.GetBot(botName);
        if (bot == null)
        {
            return Utils.FormatStaticResponse(string.Format(Strings.BotNotFound, botName));
        }

        var response = await WebRequest.LoginViaSteamOAuth(bot, url).ConfigureAwait(false);
        return response;

    }
}
