using dafsem.Context;
using dafsem.Models;
using dafsem.Services;
using dafsem.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public class AyarService
{
    private readonly AplicationDbContext _context;

    public AyarService(AplicationDbContext context)
    {
        _context = context;
    }

    public string GetSiteAdi(int dilId = 0)
    {
        int id = Convert.ToInt32(dilId);
        return _context.Ayarlar.Where(a => a.State && a.DilId == id).Select(a => a.SiteAdi).FirstOrDefault() ?? "";
    }

    public string GetKurumAdi(int dilId = 0)
    {
        int id = Convert.ToInt32(dilId);
        return _context.Ayarlar.Where(a => a.State && a.DilId == id).Select(a => a.KurumAdi).FirstOrDefault() ?? GetSiteAdi(dilId);
    }

    public string GetSiteLogo(int dilId = 0)
    {
        int id = Convert.ToInt32(dilId);
        return _context.Ayarlar.Where(a => a.State && a.DilId == id).Select(a => a.SiteLogo).Select(s => s.Yol).FirstOrDefault()?.Replace('\\', '/') ?? "";
    }

    public string GetFiligran(int dilId = 0)
    {
        int id = Convert.ToInt32(dilId);
        return _context.Ayarlar.Where(a => a.State && a.DilId == id).Select(a => a.Filigran).Select(s => s.Yol).FirstOrDefault()?.Replace('\\', '/') ?? "";

    }
}
