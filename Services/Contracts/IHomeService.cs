using dafsem.Models;
using dafsem.Models.ViewModels;
using dafsem.Models.ViewModels.UserSite;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace dafsem.Services.Contracts
{
    public interface IHomeService
    {
        Task<AnaSayfaViewModel?> GetAnaSayfa(int dilId);
        Task<string> GetTitleOfSayfa(string SayfaUrl, int dilId);
        Task<string> GetTitleOfAltSayfa(string SayfaUrl, int dilId);
        Task<List<TarihlerViewModel>> GetTarihler(int dilId);
        Task<List<KategoriUyelerViewModel>> GetKategoriUyeler(string SayfaUrl, int dilId);
        Task<List<KategoriUyelerViewModel>> GetBasliklar(int dilId);
        Task<string?> GetProgram(int dilId);
        Task<string[]> GetKurallar(string SayfaUrl, int dilId);
        Task<UcretHizmetBankaViewModel?> GetUcretHizmetBanka(int dilId);
        Task<List<KonaklamaViewModel>> GetKonaklama(int dilId);
        Task<BasvuruViewModel?> GetBasvuru(int dilId);
        Task<IletisimViewModel?> GetIletisim(int dilId);
        Task<int> GetIdbyDilKodu(string dilKodu);
        Task<SayfaDetayViewModel> GetEkSayfa(string url);
    }
}
