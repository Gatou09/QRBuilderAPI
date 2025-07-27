namespace QRBuilderAPI.Models;

public class QrCodeEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Url { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}