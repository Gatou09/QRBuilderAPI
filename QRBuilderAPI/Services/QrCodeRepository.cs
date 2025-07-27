using QRBuilderAPI.Models;

namespace QRBuilderAPI.Services;

public class QrCodeRepository
{
    private readonly List<QrCodeEntry> _qrs = new();

    public void Add(QrCodeEntry entry)
    {
        _qrs.Add(entry);
    }

    public IEnumerable<QrCodeEntry> GetByUser(Guid userId)
    {
        return _qrs.Where(q => q.UserId == userId);
    }

    public bool Delete(Guid id, Guid userId)
    {
        var qr = _qrs.FirstOrDefault(q => q.Id == id && q.UserId == userId);
        if (qr == null) return false;

        _qrs.Remove(qr);
        return true;
    }
}