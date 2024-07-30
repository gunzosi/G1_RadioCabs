﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyServices.Models;

public class Payment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int? Amount { get; set; }
    public string? ContentPayment { get; set; }
    public DateTime? PaymentAt { get; set; }
    public bool? IsPayment { get; set; }
    [ForeignKey("Company")]
    public int? CompanyId { get; set; }
    public virtual Company? Company { get; set; }
}