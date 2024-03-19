namespace ASFOAuth.Data;

/// <summary>
/// 插件配置
/// </summary>
public sealed record PluginConfig
{
    /// <summary>
    /// 是否同意使用协议
    /// </summary>
    public bool EULA { get; set; }
    /// <summary>
    /// 启用统计信息
    /// </summary>
    public bool Statistic { get; set; } = true;
}
