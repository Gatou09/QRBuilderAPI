using Microsoft.AspNetCore.Mvc;
using QRBuilderAPI.Models;
using QRBuilderAPI.Services;

namespace QRBuilderAPI.Controllers;

[ApiController]
[Route("qr")]
public class QrController : ControllerBase
{
    private readonly QrCodeService _qrService;

    public QrController(QrCodeService qrService)
    {
        _qrService = qrService;
    }

    [HttpPost("generate")]
    public IActionResult GenerateQr([FromBody] QrCodeRequestDto dto)
    {
        var path = _qrService.GenerateQrImage(dto);
        var fullUrl = $"{Request.Scheme}://{Request.Host}/qrcodes/{Path.GetFileName(path)}";
        return Ok(new { url = fullUrl });
    }
}

