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

    /// <summary>
    /// Génère un QR code personnalisé.
    /// </summary>
    /// <remarks>
    /// Nécessite un token JWT. Génère un QR code à partir du contenu (texte ou URL) fourni dans le corps de la requête.
    /// </remarks>
    /// <param name="dto">Les données du QR code à générer.</param>
    /// <returns>L'URL de l'image PNG générée.</returns>
    [HttpPost("generate")]
    [Authorize]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

    /// <summary>
    /// Récupère tous les QR codes générés par l'utilisateur connecté.
    /// </summary>
    /// <remarks>
    /// Retourne une liste des QR codes générés avec leur URL et date.
    /// Nécessite un token JWT.
    /// </remarks>
    /// <returns>Liste des objets `QrCodeEntry` (Id, Url, Date...)</returns>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(List<QrCodeEntry>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetMyQrs()
    {
        var userId = GetUserId();
        var qrs = _repo.GetByUser(userId);
        return Ok(qrs);
    }

    /// <summary>
    /// Supprime un QR code par son identifiant de fichier.
    /// </summary>
    /// <remarks>
    /// Supprime physiquement l’image PNG correspondante.
    /// </remarks>
    /// <param name="id">Le nom de fichier (sans extension).</param>
    /// <returns>204 si succès, 404 si le fichier est introuvable.</returns>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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