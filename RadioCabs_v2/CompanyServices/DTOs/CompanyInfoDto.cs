namespace CompanyServices.DTO;

public class CompanyInfoDto
{
    public string CompanyName { get; set; }
    public string CompanyEmail { get; set; }
    public string? CompanyPassword { get; set; }
    public string ContactPersonName { get; set; }
    public string Designation { get; set; }
    public string ContactPersonMobile { get; set; }
    public string CompanyTelephone { get; set; }
    public string CompanyAddress { get; set; }
    public string CompanyWard { get; set; }
    public string CompanyDistrict { get; set; }
    public string CompanyCity { get; set; }
    public IFormFile? CompanyImage { get; set; }
}