namespace ProjectOnBoarding.ViewModels
{
    public class AddProjectViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brief { get; set; }
        public string References { get; set; }
        public int CompanyId { get; set; }
        public string Company { get; set; }
        public int DivisionId { get; set; }
        public string Division { get; set; }
        public int BrandId { get; set; }
        public string Brand { get; set; }
        public bool IsCompleted { get; set; }
        //public string? Category { get; set; }
        //public string? Size { get; set; }
        //public IFormFile? BriefFile { get; set; }
        //public IFormFile? ReferenceFiles { get; set; }

    }
}
