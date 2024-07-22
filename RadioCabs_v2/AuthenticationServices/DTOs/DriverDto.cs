﻿using AuthenticationServices.Database;

namespace AuthenticationServices.DTOs;

public class DriverDto
{
    public string DriverFullName { get; set; }
    public string DriverMobile { get; set; }
    public string Password { get; set; }
    public string? Role { get; set; }
}