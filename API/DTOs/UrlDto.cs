using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class CreateUrlRequest
{
    [Required(ErrorMessage = "URL is required")]
    [Url(ErrorMessage = "Please provide a valid URL")]
    public string Url { get; set; } = string.Empty;
}

public class UpdateUrlRequest
{
    [Required(ErrorMessage = "New URL is required")]
    [Url(ErrorMessage = "Please provide a valid URL")]
    public string NewUrl { get; set; } = string.Empty;
}

public class UrlResponse
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UrlStatsResponse : UrlResponse
{
    public int AccessCount { get; set; }
}