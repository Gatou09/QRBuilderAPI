namespace QRBuilderAPI.Models;

public class QrCodeRequestDto
{
    public string Type { get; set; } // "url", "text", etc.
    public string Value { get; set; }
    public string Color { get; set; } = "#000000";
    public string BackgroundColor { get; set; } = "#ffffff";
    public string? LogoUrl { get; set; }
}