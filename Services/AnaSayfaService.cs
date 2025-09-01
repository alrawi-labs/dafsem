using dafsem.Context;
using dafsem.Models;
using dafsem.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Services
{
    public class AnaSayfaService : IAnaSayfaService
    {
        private readonly AplicationDbContext _context;
        private readonly IFileService _fileService;
        private readonly IDilService _dilService;

        public AnaSayfaService(AplicationDbContext context, IFileService fileService, IDilService dilService)
        {
            _context = context;
            _fileService = fileService;
            _dilService = dilService;
        }
        public async Task<AnaSayfa?> SoftGetAnaSayfaAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();

            var anaSayfaQuery = _context.AnaSayfa
                .OrderByDescending(i => i.Id)
                .Include(a => a.AfiseId)
                .Include(a => a.Sliderler.Where(s => s.State && s.DilId == dilId));

            var anaSayfa = await anaSayfaQuery.Where(s => s.State && s.DilId == dilId).FirstOrDefaultAsync();
            if (anaSayfa == null)
                anaSayfa = await anaSayfaQuery.Where(a=>a.DilId == dilId).FirstOrDefaultAsync();

            if (anaSayfa != null)
            {
                anaSayfa.State = true;
                await _context.SaveChangesAsync();
                return anaSayfa;
            }

            anaSayfa = new AnaSayfa();
            anaSayfa.DilId = dilId;
            await _context.AnaSayfa.AddAsync(anaSayfa);
            await _context.SaveChangesAsync();
            return anaSayfa;
        }

        public async Task<Fotolar> SoftAddSliderAsync(IFormFile SliderPhoto)
        {
            if (SliderPhoto == null || SliderPhoto.Length <= 0)
                return null;

            string uploadPath = await _fileService.UploadFileAsync(SliderPhoto, "/Uploads/Sliders", FileService.DosyaTuru.Photo);
            int dilId = await _dilService.SoftGetDilIdFromCookie();

            // Önce fotoğrafı veritabana ekliyorum
            var anaSayfa = await SoftGetAnaSayfaAsync();
            var yeniSlider = new Fotolar() { Yol = uploadPath, DilId = dilId };

            anaSayfa?.Sliderler!.Add(yeniSlider);
            await _context.SaveChangesAsync();

            return yeniSlider; // Eklenen slider bilgilerini döndür
        }

        public async Task<bool> SoftDeleteFotoAsync(int id)
        {
            // Veritabanından ilgili slider kaydını bul
            var foto = _context.Fotolar.FirstOrDefault(s => s.Id == id && s.State);

            if (foto == null)
                return false;

            foto.State = false;
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<bool> SoftUpdateAsync(AnaSayfa model, IFormFile? Afise)
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            var anaSayfa = await SoftGetAnaSayfaAsync();
            if (anaSayfa == null)
            {
                model.DilId = dilId;
                await _context.AnaSayfa.AddAsync(model);
                await _context.SaveChangesAsync();
                anaSayfa = await SoftGetAnaSayfaAsync();
            }
            else
            {

                AnaSayfa yeniKayit = new AnaSayfa()
                {
                    AfiseId = anaSayfa.AfiseId,
                    Mektup = anaSayfa.Mektup,
                    DilId = anaSayfa.DilId,
                    State = false,
                };

                await _context.AnaSayfa.AddAsync(yeniKayit);
            }


            if (Afise != null && Afise.Length > 0)
            {
                string uploadPath = await _fileService.UploadFileAsync(Afise, "/Uploads/Fotograflar", FileService.DosyaTuru.Photo);
                anaSayfa.AfiseId = new Fotolar() { Yol = uploadPath, DilId = dilId };
            }

            anaSayfa.Mektup = model.Mektup;
            anaSayfa.DilId = (model.DilId != 0) ? model.DilId : dilId;
            // Modeli güncelle ve veritabanına kaydet
            _context.AnaSayfa.Update(anaSayfa);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> SoftDeleteAfisAsync()
        {
            var anaSayfa = await SoftGetAnaSayfaAsync();
            if (anaSayfa == null || anaSayfa.AfiseId == null)
                return false;

            bool result = await SoftDeleteFotoAsync(anaSayfa!.AfiseId!.Id);
            if (!result)
                return false;

            anaSayfa!.AfiseId = null;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
