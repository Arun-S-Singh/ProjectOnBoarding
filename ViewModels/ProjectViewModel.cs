namespace ProjectOnBoarding.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? CompanyId { get; set; }
        public string? Company { get; set; }
        public int? DivisionId { get; set; }
        public string? Division { get; set; }
        public int? BrandId { get; set; }
        public string? Brand { get; set; }
        public string? Brief { get; set; }
        public string? References { get; set; }
        public string Code { get; set; }
        public bool IsCompleted { get; set; } 
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }

    }
}
