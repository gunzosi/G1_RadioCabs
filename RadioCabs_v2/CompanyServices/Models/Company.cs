using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CompanyServices.Models;

public class Company
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public string? CompanyName { get; set; }
    [Required]
    public string? CompanyTaxCode { get; set; }
    [Required]
    public string? CompanyEmail { get; set; }
    [Required]
    public string? CompanyPassword { get; set; }
    
    // Service Reference
    public int? CompanyServiceLocationId { get; set; }
    public int? MembershipId { get; set; }
    
    public virtual CompanyInfo? CompanyInfo { get; set; }
    public virtual ICollection<CompanyService>? CompanyServices { get; set; }
    public virtual ICollection<CompanyLocationService>? CompanyLocationServices { get; set; }
    public virtual ICollection<AdvertisementImage>? Advertisements { get; set; }
}