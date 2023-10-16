using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Web;
using System.Text.RegularExpressions;

namespace ASFOAuth.Core;

internal static partial class WebRequest
{
    [GeneratedRegex(@"(?:(openid\.[a-z_.]+)=([^&]+))")]
    private static partial Regex MatchQueries();

    /// <summary>
    /// Steam OAuth登录
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static async Task<string> LoginViaSteamOAuth(Bot bot, string url)
    {
        var regex = MatchQueries();
        var matches = regex.Matches(url);

        var queries = new List<string>();
        foreach (var match in matches.ToList())
        {
            queries.Add(match.Value);
        }

        if (queries.Count == 0)
        {
            return "OAuth链接无效";
        }

        var request = new Uri(SteamCommunityURL, "/openid/login?" + string.Join('&', queries));
        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request).ConfigureAwait(false);

        var eles = response?.Content?.QuerySelectorAll("#openidForm>input[name][value]");
        if (eles == null)
        {
            return "网络错误或OAuth链接无效";
        }

        var formData = new Dictionary<string, string>();
        foreach (var ele in eles)
        {
            var name = ele.GetAttribute("name");
            var value = ele.GetAttribute("value");
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
            {
                formData.Add(name, value);
            }
        }

        request = new Uri(SteamCommunityURL, "/openid/login");
        var response2 = await bot.ArchiWebHandler.UrlPostToHtmlDocumentWithSession(request, data: formData, requestOptions: WebBrowser.ERequestOptions.ReturnRedirections).ConfigureAwait(false);

        return response2?.FinalUri.ToString() ?? "登录失败";
    }
}
