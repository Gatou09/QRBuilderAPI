using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QRBuilderAPI.Models;
using QRBuilderAPI.Services;
using System.Security.Claims;

namespace QRBuilderAPI.Controllers;

[ApiController]
[Route("qr")]
[Authorize]
public class QrController : ControllerBase
{
    private readonly QrCodeService _qrService;
    private readonly QrCodeRepository _repo;

    public QrController(QrCodeService qrService, QrCodeRepository repo)
    {
        _qrService = qrService;
        _repo = repo;
    }

    [HttpPost("generate")]
    public IActionResult GenerateQr([FromBody] QrCodeRequestDto dto)
    {
        var userId = GetUserId();
        var path = _qrService.GenerateQrImage(dto);
        var url = $"{Request.Scheme}://{Request.Host}/qrcodes/{Path.GetFileName(path)}";

        var entry = new QrCodeEntry
        {
            UserId = userId,
            Url = url
        };

        _repo.Add(entry);

        return Ok(new { url, entry.Id });
    }

    [HttpGet]
    public IActionResult GetMyQrs()
    {
        var userId = GetUserId();
        var qrs = _repo.GetByUser(userId);
        return Ok(qrs);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var userId = GetUserId();
        var success = _repo.Delete(id, userId);

        if (!success)
            return NotFound();

        return NoContent();
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return Guid.Parse(claim!.Value);
    }
}