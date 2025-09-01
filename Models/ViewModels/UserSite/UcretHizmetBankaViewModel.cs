namespace dafsem.Models.ViewModels.UserSite
{
    public class UcretHizmetBankaViewModel
    {
        public List<UcretViewModel> Ucretler { get; set; }
        public List<HizmetTuruViewModel> HizmetTurler { get; set; }
        public List<BankaViewModel> Bankalar { get; set; }
    }

    public class UcretViewModel
    {
        public string Baslik { get; set; }
        public string Ucret { get; set; }
    }

    public class HizmetTuruViewModel
    {
        public string Tur { get; set; }
        public List<string> Hizmetler { get; set; }
    }

    public class BankaViewModel
    {
        public string BankaAdi { get; set; }
        public string HesapSahibiAdi { get; set; }
        public string IBAN { get; set; }
    }
}