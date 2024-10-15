
namespace ProjectOnBoarding.ViewModels
{
    public class DocumentViewModel
    {
        public int Id { get; set; }

        public string Type { get; set; }
        public string FileName { get; set; }

        public string UniqueFileName { get; set; }
        public int ProjectId { get; set; }

        public IFormFile? File { get; set; }
    }
}
