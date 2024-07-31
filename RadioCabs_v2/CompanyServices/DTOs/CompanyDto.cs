﻿namespace CompanyServices.DTOs;

public class CompanyDto
{
    public string CompanyName { get; set; }
    public string? CompanyTaxCode { get; set; }
    public string? CompanyEmail { get; set; }
    public string? CompanyPassword { get; set; }
    public string? MembershipType { get; set; } // Add this line
    // Company have many Driver 
    public ICollection<int>? DriversId { get; set; }
    public string? Role { get; set; }
}