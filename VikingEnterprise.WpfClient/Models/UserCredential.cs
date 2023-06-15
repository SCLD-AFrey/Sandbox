using System;

namespace VikingEnterprise.WpfClient.Models;

public class UserCredential
{
    public int Oid { get; set; } = 0;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsLoggedIn { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public DateTime LastLogin { get; set; } = DateTime.MinValue;

    public void Reset()
    {
        this.Username = string.Empty;
        this.Password = string.Empty;
        this.Oid = 0;
        this.IsLoggedIn = false;
        this.IsActive = true;
    }
}