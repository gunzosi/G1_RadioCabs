using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AuthenticationServices.Models;

public class Driver
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? DriverFullName { get; set; }
    public string? DriverCode { get; set; }
    public string? DriverMobile { get; set; }
    public string? DriverEmail { get; set; }
    public string? Password { get; set; }
    public bool? Status { get; set; }
    public string? Role { get; set; }
    
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public string? CompanyId { get; set; }
    [JsonIgnore]
    public virtual DriverInfo? DriverInfo { get; set; }
}