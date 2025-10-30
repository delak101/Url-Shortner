using API.Data;
using API.DTOs;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class ShortenService : IShortenService
{
    private readonly URLShortenerContext _context;
    private const string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public ShortenService(URLShortenerContext context)
    {
        _context = context;
    }

    public async Task<UrlResponse?> ShortenUrlAsync(CreateUrlRequest request)
    {
        var shortCode = await GenerateUniqueShortCodeAsync();
        var now = DateTime.UtcNow;
        var urlMapping = new URLMap
        {
            Url = request.Url,
            ShortCode = shortCode,
            CreatedAt = now,
            UpdatedAt = now,
            AccessCount = 0
        };
        _context.URLMaps.Add(urlMapping);
        await _context.SaveChangesAsync();
        
        return MapToUrlResponse(urlMapping);
    }

    public async Task<UrlStatsResponse?> GetAccessCountAsync(string shortCode)
    {
        var urlMapping = await _context.URLMaps
            .FirstOrDefaultAsync(u => u.ShortCode == shortCode);
        if (urlMapping == null) return null;

        return new UrlStatsResponse
        {
            Id = urlMapping.Id,
            Url = urlMapping.Url,
            ShortCode = urlMapping.ShortCode,
            CreatedAt = urlMapping.CreatedAt,
            UpdatedAt = urlMapping.UpdatedAt,
            AccessCount = urlMapping.AccessCount
        };
    }

    public async Task<bool> DeleteShortUrlAsync(string shortCode)
    {
        var urlMapping = await _context.URLMaps
            .FirstOrDefaultAsync(u => u.ShortCode == shortCode);

        if (urlMapping == null) return false;

        _context.URLMaps.Remove(urlMapping);
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<IEnumerable<URLMap>> GetAllUrlsAsync()
    {
        return await _context.URLMaps.ToListAsync();
    }

    public async Task<UrlResponse?> GetUrlByShortcodeAsync(string shortCode)
    {
        var urlMapping = await _context.URLMaps
            .FirstOrDefaultAsync(u => u.ShortCode == shortCode);
        if (urlMapping == null) return null;

        // Increment access count
        urlMapping.AccessCount++;
        await _context.SaveChangesAsync();
        
        return MapToUrlResponse(urlMapping);
    }

    public async Task<UrlResponse?> UpdateUrlAsync(string shortCode, UpdateUrlRequest request)
    {
        var urlMapping = await _context.URLMaps
            .FirstOrDefaultAsync(u => u.ShortCode == shortCode);
        if (urlMapping == null) return null;

        urlMapping.Url = request.NewUrl;
        urlMapping.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return MapToUrlResponse(urlMapping);
    }

    private async Task<string> GenerateUniqueShortCodeAsync()
    {
        string shortCode;
        bool exists;

        do
        {
            shortCode = GenerateShortCode();
            exists = await _context.URLMaps.AnyAsync(u => u.ShortCode == shortCode);
        } while (exists);

        return shortCode;
    }

    private static string GenerateShortCode()
    {
        var random = new Random();
        var shortCode = new char[6];
        for (int i = 0; i < 6; i++)
        {
            shortCode[i] = Characters[random.Next(Characters.Length)];
        }
        return new string(shortCode);
    }

    private static UrlResponse MapToUrlResponse(URLMap urlMapping)
    {
        return new UrlResponse
        {
            Id = urlMapping.Id,
            Url = urlMapping.Url,
            ShortCode = urlMapping.ShortCode,
            CreatedAt = urlMapping.CreatedAt,
            UpdatedAt = urlMapping.UpdatedAt
        };
    }

}
