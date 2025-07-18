using QRBuilderAPI.Models;
using QRCoder;
using SkiaSharp;

namespace QRBuilderAPI.Services
{
    public class QrCodeService
    {
        private readonly IWebHostEnvironment _env;

        public QrCodeService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string GenerateQrImage(QrCodeRequestDto dto)
        {
            var generator = new QRCodeGenerator();
            var data = generator.CreateQrCode(dto.Value, QRCodeGenerator.ECCLevel.Q);

            // Color
            var fgColor = SKColor.Parse(dto.Color);
            var bgColor = SKColor.Parse(dto.BackgroundColor);

            // Create QR with SkiaSharp
            int size = 512;
            using var surface = SKSurface.Create(new SKImageInfo(size, size));
            var canvas = surface.Canvas;
            canvas.Clear(bgColor);

            var moduleCount = data.ModuleMatrix.Count;
            float cellSize = (float)size / moduleCount;

            for (int y = 0; y < moduleCount; y++)
            {
                for (int x = 0; x < moduleCount; x++)
                {
                    if (data.ModuleMatrix[y][x])
                    {
                        var paint = new SKPaint { Color = fgColor, IsAntialias = false };
                        canvas.DrawRect(new SKRect(x * cellSize, y * cellSize, (x + 1) * cellSize, (y + 1) * cellSize), paint);
                    }
                }
            }

            using var image = surface.Snapshot();
            using var dataEncoded = image.Encode(SKEncodedImageFormat.Png, 100);

            var folderPath = Path.Combine(_env.WebRootPath, "qrcodes");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}.png";
            var filePath = Path.Combine(folderPath, fileName);

            using var fs = File.OpenWrite(filePath);
            dataEncoded.SaveTo(fs);

            return filePath;
        }
    }
}
