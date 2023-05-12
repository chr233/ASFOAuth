using Newtonsoft.Json;

namespace ASFOAuth.Data;

public sealed record PluginConfig
{
    /// <summary>
    /// 启用统计信息
    /// </summary>
    [JsonProperty(Required = Required.DisallowNull)]
    public bool Statistic { get; set; } = true;

    /// <summary>
    /// 禁用命令表
    /// </summary>
    [JsonProperty(Required = Required.Default)]
    public List<string>? DisabledCmds { get; set; }
}
