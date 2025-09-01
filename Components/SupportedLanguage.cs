using System.ComponentModel.DataAnnotations;

namespace dafsem.Components
{
    public enum SupportedLanguage
    {
        [Display(Name = "İngilizce")]
        EN,

        [Display(Name = "Arapça")]
        AR,

        [Display(Name = "Fransızca")]
        FR,

        [Display(Name = "Almanca")]
        DE,

        [Display(Name = "İspanyolca")]
        ES,

        [Display(Name = "İtalyanca")]
        IT,

        [Display(Name = "Portekizce")]
        PT,

        [Display(Name = "Rusça")]
        RU,

        [Display(Name = "Çince (Basitleştirilmiş)")]
        ZH,

        [Display(Name = "Japonca")]
        JA,

        [Display(Name = "Korece")]
        KO
    }
}
