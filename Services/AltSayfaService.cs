using dafsem.Context;
using dafsem.Models;
using dafsem.Services.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Services
{
    public class AltSayfaService : IAltSayfaService
    {
        private readonly AplicationDbContext _context;
        private readonly IDilService _dilService;

        public AltSayfaService(AplicationDbContext context, IDilService dilService)
        {
            _context = context;
            _dilService = dilService;
        }
        public async Task<SelectList?> SoftGetAllAsSelectListAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();

            var model = await _context.AltSayfa.AsNoTracking().Where(p => p.State && p.DilId == dilId).Select(k => new { k.Id, k.AltSayfaBaslik }).ToListAsync();
            return new SelectList(model, "Id", "AltSayfaBaslik");
        }

        public async Task<AltSayfa?> SoftFirstOrDefaultAsync(int id)
        {
            return await _context.AltSayfa
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id & a.State);
        }
    }
}
