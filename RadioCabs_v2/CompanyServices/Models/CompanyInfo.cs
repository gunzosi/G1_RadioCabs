using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CompanyServices.Models;

public class CompanyInfo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [ForeignKey("Company")]
    public int CompanyId { get; set; }
    public string? ContactPerson { get; set; }
    public string? Designation { get; set; }
    public string? ContactPersonMobile { get; set; }
    public string? CompanyTelephone { get; set; }
    public string? CompanyImageUrl { get; set; }
    public string? CompanyAddress { get; set; }
    public string? CompanyWard { get; set; }
    public string? CompanyDistrict { get; set; }
    public string? CompanyCity { get; set; }

    [JsonIgnore]
    public Company? Company { get; set; }
}