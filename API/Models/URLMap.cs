namespace API.Models;

public class URLMap
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int AccessCount { get; set; } = 0;
}
