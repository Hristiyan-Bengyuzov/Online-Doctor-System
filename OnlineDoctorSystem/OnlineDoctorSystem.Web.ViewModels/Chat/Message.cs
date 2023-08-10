namespace OnlineDoctorSystem.Web.ViewModels.Chat
{
    public class Message
    {
        public string User { get; set; }

        public string Text { get; set; }

        public string CreatedOn { get; set; }

        public bool IsDoctor { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsPatient { get; set; }

        public string ImageUrl { get; set; }
    }
}
