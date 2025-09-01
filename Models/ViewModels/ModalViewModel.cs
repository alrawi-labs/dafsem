namespace dafsem.Models.ViewModels
{
    public class ModalViewModel
    {
        public string ModalId { get; set; } = "customModal";
        public string Title { get; set; } = "Başlık";
        public string Body { get; set; } = "İçerik";
        public string ConfirmButtonId { get; set; } = "confirmButton";
        public string? ButtonLabel { get; set;}
        public string? ButtonIcon { get; set; }
        public string? ButtonColor { get; set; }
        public string? TitleColor { get; set; }
        public string? TitleBackground { get; set; }

    }

}
