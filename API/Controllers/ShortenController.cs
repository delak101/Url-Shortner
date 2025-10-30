using API.DTOs;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ShortenController : ControllerBase
{
    private readonly IShortenService _shortenService;

    public ShortenController(IShortenService shortenService)
    {
        _shortenService = shortenService;
    }

    [HttpPost]
    public async Task<IActionResult> ShortenUrl([FromBody] CreateUrlRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _shortenService.ShortenUrlAsync(request);
            return CreatedAtAction(nameof(GetOriginalUrl), new { shortCode = result.ShortCode }, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{shortCode}")]
    public async Task<ActionResult<UrlResponse>> GetOriginalUrl(string shortCode)
    {
        if (string.IsNullOrEmpty(shortCode))
        {
            return BadRequest("Short code is required");
        }

        try
        {
            var result = await _shortenService.GetUrlByShortcodeAsync(shortCode);
            if (result == null)
            {
                return NotFound(new { message = "Short URL not found" });
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{shortCode}")]
    public async Task<IActionResult> UpdateUrl(string shortCode, [FromBody] UpdateUrlRequest request)
    {
        if (string.IsNullOrEmpty(shortCode))
        {
            return BadRequest("Short code is required");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _shortenService.UpdateUrlAsync(shortCode, request);
            if (result == null)
            {
                return NotFound(new { message = "Short URL not found" });
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{shortCode}")]
    public async Task<IActionResult> DeleteUrl(string shortCode)
    {
        if (string.IsNullOrEmpty(shortCode))
        {
            return BadRequest("Short code is required");
        }

        try
        {
            var success = await _shortenService.DeleteShortUrlAsync(shortCode);
            if (!success)
            {
                return NotFound(new { message = "Short URL not found" });
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    [HttpGet("{shortCode}/stats")]
    public async Task<IActionResult> GetUrlStats(string shortCode)
    {
        if (string.IsNullOrEmpty(shortCode))
        {
            return BadRequest("Short code is required");
        }

        try
        {
            var result = await _shortenService.GetAccessCountAsync(shortCode);
            if (result == null)
            {
                return NotFound(new { message = "Short URL not found" });
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
