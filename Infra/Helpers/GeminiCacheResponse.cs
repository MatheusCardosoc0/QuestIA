using System.Text.Json.Serialization;

public class GeminiCacheResponse
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; }

    [JsonPropertyName("contents")]
    public ContentDto[] Contents { get; set; }

    [JsonPropertyName("systemInstruction")]
    public ContentDto SystemInstruction { get; set; }

    [JsonPropertyName("ttl")]
    public string Ttl { get; set; }

    [JsonPropertyName("expireTime")]
    public string ExpireTime { get; set; }

    [JsonPropertyName("createTime")]
    public string CreateTime { get; set; }

    [JsonPropertyName("updateTime")]
    public string UpdateTime { get; set; }

    [JsonPropertyName("usageMetadata")]
    public UsageMetadataDto UsageMetadata { get; set; }

    [JsonPropertyName("tools")]
    public ToolDto[] Tools { get; set; }

    [JsonPropertyName("toolConfig")]
    public ToolConfigDto ToolConfig { get; set; }
}

public class ContentDto
{
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("parts")]
    public PartDto[] Parts { get; set; }
}

public class PartDto
{
    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("inlineData")]
    public InlineDataDto InlineData { get; set; }

    [JsonPropertyName("fileData")]
    public FileDataDto FileData { get; set; }
}

public class InlineDataDto
{
    [JsonPropertyName("mimeType")]
    public string MimeType { get; set; }

    [JsonPropertyName("data")]
    public string Data { get; set; }
}

public class FileDataDto
{
    [JsonPropertyName("mimeType")]
    public string MimeType { get; set; }

    [JsonPropertyName("fileUri")]
    public string FileUri { get; set; }
}

public class UsageMetadataDto
{
    [JsonPropertyName("totalTokenCount")]
    public int TotalTokenCount { get; set; }
}

// Exemplos simplificados para ToolDto e ToolConfigDto — ajuste conforme a spec completa:
public class ToolDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("toolUri")]
    public string ToolUri { get; set; }

    // adicione outras propriedades conforme necessário...
}

public class ToolConfigDto
{
    [JsonPropertyName("someConfigField")]
    public string SomeConfigField { get; set; }

    // adicione outras propriedades conforme necessário...
}
