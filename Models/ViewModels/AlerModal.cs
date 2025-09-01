namespace dafsem.Models.ViewModels
{
    public class AlerModal
    {
        public string ModalId { get; set; } = "";
        public string Title { get; set; } = "Sistem Kaynaklı Bir Sorun Meydana Geldi";
        public string Message { get; set; } = "Yazılımda bir sorun oluştu. Lütfen bir süre sonra tekrar deneyin. Eğer bu hata sürekli karşılaşıyorsa, sistem yöneticileriyle iletişime geçmeyi deneyin";
        private string ModalType { get; set; } = "";
        public string Icon { get; private set; } = "bi bi-bug";
        public string Color { get; private set; } = "#871414";
        public string ButtonLabel { get; private set; } = "Anladım";


        public AlerModal(string modelId = "", string title = "", string message = "", string modalType="")
        {
            ModalId = modelId;
            Title = title;
            Message = message;
            ModalType = modalType;
            string modalTuru = ModalType.ToLower();
            if (modalTuru == "success")
            {
                Icon = "bi-check-circle";
                Color = "#82ce34";
                ButtonLabel = "Tamam";
            }
            else if (modalTuru == "danger")
            {
                Icon = "bi-x-circle";
                Color = "#ce3434";
                ButtonLabel = "Kapat";

            }
            else if (modalTuru == "warning")
            {
                Icon = "bi bi-exclamation-circle";
                Color = "#ce9234";
                ButtonLabel = "Anladım";

            }
            else if (modalTuru == "info")
            {
                Icon = "bi bi-info-circle";
                Color = "#34a9ce";
                ButtonLabel = "Anladım";
            }
            else
            {
                Icon = "bi bi-bug";
                Color = "#871414";
                ButtonLabel = "Anladım";
            }

        }


    }
}
