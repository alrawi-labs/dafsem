using dafsem.Context;
using dafsem.Models;
using dafsem.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Services
{
    public class BasvuruService : IBasvuruService
    {
        private readonly AplicationDbContext _context;
        private readonly IFileService _fileService;
        private readonly IDilService _dilService;

        public BasvuruService(AplicationDbContext context, IFileService fileService, IDilService dilService)
        {
            _context = context;
            _fileService = fileService;
            _dilService = dilService;
        }

        public async Task<bool> SoftDeleteForm()
        {
            Basvuru? basvuru = await SoftGetLastAsync();
            if (basvuru == null)
                return false;

            basvuru.Form = null;
            _context.Basvuru.Update(basvuru);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Basvuru?> SoftGetLastAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            var basvuru = await _context.Basvuru.AsNoTracking()
                .Where(b => b.State && b.DilId == dilId)
                .OrderByDescending(b => b.Id)
                .FirstOrDefaultAsync();

            if (basvuru != null)
                return basvuru;

            if (!await _context.Basvuru.Where(b => b.DilId == dilId && b.State).AnyAsync())
            {
                basvuru = new Basvuru();
                basvuru.DilId = dilId;
                await _context.Basvuru.AddAsync(basvuru);
                await _context.SaveChangesAsync();
                return basvuru;
            }

            return await _context.Basvuru.AsNoTracking()
                .Where(b => b.DilId == dilId)
                .OrderByDescending(b => b.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SoftUpdateAsync(Basvuru basvuru, IFormFile? basvuruForm)
        {
            if (basvuru == null)
                return false;

            Basvuru? model = await SoftGetLastAsync();

            if (model == null)
            {
                try
                {
                    int dilId = await _dilService.SoftGetDilIdFromCookie();
                    basvuru.DilId = dilId;
                    // daha önce hiçbir kayıt olmadığı için yeni gelen kaydı eklemek
                    await _context.Basvuru.AddAsync(basvuru);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                    throw;
                }
            }

            Basvuru yeniKayit = new Basvuru()
            {
                AltMetin = model.AltMetin,
                Form = model.Form,
                UstMetin = model.UstMetin,
                DilId = model.DilId,
                State = false
            };

            await _context.Basvuru.AddAsync(yeniKayit);


            if (basvuruForm != null)
            {
                // Yeni dosyayı yükle
                var yeniDosyaYolu = await _fileService.UploadFileAsync(basvuruForm, "/Uploads/Site/Docs", FileService.DosyaTuru.Doc);
                basvuru.Form = yeniDosyaYolu;
            }
            else
            {
                basvuru.Form = model.Form; // Eğer gönderilmezse olduğu gibi kalsın diye yapıyoruz
            }

            basvuru.DilId = model.DilId;
            _context.Update(basvuru);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
