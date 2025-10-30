using API.DTOs;
using API.Models;

namespace API.Services;

public interface IShortenService
{
    Task<UrlResponse?> ShortenUrlAsync(CreateUrlRequest request);
    Task<UrlResponse?> GetUrlByShortcodeAsync(string shortCode);
    Task<UrlResponse?> UpdateUrlAsync(string shortCode, UpdateUrlRequest request);
    Task<bool> DeleteShortUrlAsync(string shortCode);
    Task<UrlStatsResponse?> GetAccessCountAsync(string shortCode);
    Task<IEnumerable<URLMap>> GetAllUrlsAsync();
}
