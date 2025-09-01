using dafsem.Services.Contracts;

namespace dafsem.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly IBaslikService _baslikService;
        private readonly ITarihlerService _tarihlerService;
        private readonly IBankaBilgileriService _bankaBilgileriService;
        private readonly IUnvanlarService _unvanlarService;
        private readonly IParaBirimiService _paraBirimiService;
        private readonly IUcretlerService _ucretlerService;
        private readonly IKonaklamaService _konaklamaService;
        private readonly IOdaTipleriService _odaTipleriService;
        private readonly IHizmetTuruService _hizmetTuruService;
        private readonly IHizmetlerService _hizmetlerService;
        private readonly IAltSayfaService _altSayfaService;
        private readonly IKuralTuruService _kuralTuruService;
        private readonly IKurallarService _kurallarService;
        private readonly IBasvuruService _basvuruService;
        private readonly IIletisimService _iletisimService;
        private readonly ITelefonlarService _telefonlarService;
        private readonly IAyarlarService _ayarlarService;
        private readonly IFileService _fileService;
        private readonly ISayfalarService _sayfalarService;
        private readonly IKurulKategorileriService _kurulKategorileriService;
        private readonly IKurulUyeleriService _kurulUyeleriService;
        private readonly IAnaSayfaService _anaSayfaService;
        private readonly IDilService _dilService;
        private readonly IEkSayfalarService _ekSayfalarService;
        public ServiceManager(IBaslikService baslikService, ITarihlerService tarihlerService,
            IBankaBilgileriService bankaBilgileriService, IUnvanlarService unvanlarService,
            IParaBirimiService paraBirimiService, IUcretlerService ucretlerService,
            IKonaklamaService konaklamaService, IOdaTipleriService odaTipleriService,
            IHizmetTuruService hizmetTuruService, IHizmetlerService hizmetlerService,
            IAltSayfaService altSayfaService, IKuralTuruService kuralTuruService,
            IKurallarService kurallarService, IBasvuruService basvuruService,
            IIletisimService iletisimService, ITelefonlarService telefonlarService,
            IAyarlarService ayarlarService, IFileService fileService,
            ISayfalarService sayfalarService, IKurulKategorileriService kurulKategorileriService,
            IKurulUyeleriService kurulUyeleriService, IAnaSayfaService anaSayfaService,
            IDilService dilService, IEkSayfalarService ekSayfalarService)
        {
            _baslikService = baslikService;
            _tarihlerService = tarihlerService;
            _bankaBilgileriService = bankaBilgileriService;
            _unvanlarService = unvanlarService;
            _paraBirimiService = paraBirimiService;
            _ucretlerService = ucretlerService;
            _konaklamaService = konaklamaService;
            _odaTipleriService = odaTipleriService;
            _hizmetTuruService = hizmetTuruService;
            _hizmetlerService = hizmetlerService;
            _altSayfaService = altSayfaService;
            _kuralTuruService = kuralTuruService;
            _kurallarService = kurallarService;
            _basvuruService = basvuruService;
            _iletisimService = iletisimService;
            _telefonlarService = telefonlarService;
            _ayarlarService = ayarlarService;
            _fileService = fileService;
            _sayfalarService = sayfalarService;
            _kurulKategorileriService = kurulKategorileriService;
            _kurulUyeleriService = kurulUyeleriService;
            _anaSayfaService = anaSayfaService;
            _dilService = dilService;
            _ekSayfalarService = ekSayfalarService;
        }

        public IBaslikService BaslikService => _baslikService;
        public ITarihlerService TarihlerService => _tarihlerService;
        public IBankaBilgileriService BankaBilgileriService => _bankaBilgileriService;
        public IUnvanlarService UnvanlarService => _unvanlarService;
        public IParaBirimiService ParaBirimiService => _paraBirimiService;
        public IUcretlerService UcretlerService => _ucretlerService;
        public IKonaklamaService KonaklamaService => _konaklamaService;
        public IOdaTipleriService OdaTipleriService => _odaTipleriService;
        public IHizmetTuruService HizmetTuruService => _hizmetTuruService;
        public IHizmetlerService HizmetlerService => _hizmetlerService;
        public IAltSayfaService AltSayfaService => _altSayfaService;
        public IKuralTuruService KuralTuruService => _kuralTuruService;
        public IKurallarService KurallarService => _kurallarService;
        public IBasvuruService BasvuruService => _basvuruService;
        public IIletisimService IletisimService => _iletisimService;
        public ITelefonlarService TelefonlarService => _telefonlarService;
        public IAyarlarService AyarlarService => _ayarlarService;
        public IFileService FileService => _fileService;
        public ISayfalarService SayfalarService => _sayfalarService;
        public IKurulKategorileriService KurulKategorileriService => _kurulKategorileriService;
        public IKurulUyeleriService KurulUyeleriService => _kurulUyeleriService;
        public IAnaSayfaService AnaSayfaService => _anaSayfaService;
        public IDilService DilService => _dilService;
        public IEkSayfalarService EkSayfalarService => _ekSayfalarService;
    }
}
