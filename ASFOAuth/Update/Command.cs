namespace ASFOAuth.Update;

internal static class Command
{
    /// <summary>
    /// 查看插件版本
    /// </summary>
    /// <returns></returns>
    internal static string? ResponseASFBuffBotVersion()
    {
        return Utils.FormatStaticResponse(string.Format(Langs.PluginVer, nameof(ASFOAuth), Utils.MyVersion.ToString()));
    }
}
