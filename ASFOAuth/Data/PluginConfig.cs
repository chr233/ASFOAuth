using Newtonsoft.Json;

namespace ASFOAuth.Data;

public sealed record PluginConfig
{
    /// <summary>
    /// 是否同意使用协议
    /// </summary>
    [JsonProperty(Required = Required.DisallowNull)]
    public bool EULA { get; set; }
    /// <summary>
    /// 启用统计信息
    /// </summary>
    [JsonProperty(Required = Required.DisallowNull)]
    public bool Statistic { get; set; } = true;
}
