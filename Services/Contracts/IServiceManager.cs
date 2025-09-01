namespace dafsem.Services.Contracts
{
    public interface IServiceManager
    {
        IBaslikService BaslikService { get; }
        ITarihlerService TarihlerService { get; }
        IBankaBilgileriService BankaBilgileriService { get; }
        IUnvanlarService UnvanlarService { get; }
        IParaBirimiService ParaBirimiService { get; }
        IUcretlerService UcretlerService { get; }
        IKonaklamaService KonaklamaService { get; }
        IOdaTipleriService OdaTipleriService { get; }
        IHizmetTuruService HizmetTuruService { get; }
        IHizmetlerService HizmetlerService { get; }
        IAltSayfaService AltSayfaService { get; }
        IKuralTuruService KuralTuruService { get; }
        IKurallarService KurallarService { get; }
        IBasvuruService BasvuruService { get; }
        IIletisimService IletisimService { get; }
        ITelefonlarService TelefonlarService { get; }
        IAyarlarService AyarlarService { get; }
        IFileService FileService { get; }
        ISayfalarService SayfalarService { get; }
        IKurulKategorileriService KurulKategorileriService { get; }
        IKurulUyeleriService KurulUyeleriService { get; }
        IAnaSayfaService AnaSayfaService { get; }
        IDilService DilService { get; }
        IEkSayfalarService EkSayfalarService { get; }
    }
}
