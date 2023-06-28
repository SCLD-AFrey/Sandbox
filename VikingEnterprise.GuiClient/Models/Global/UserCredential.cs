using System;

namespace VikingEnterprise.GuiClient.Models.Global;

public class UserCredential
{
    public int Oid { get; set; } = 0;
    public string Username { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime LastLogin { get; set; } = DateTime.MinValue;

    public void Reset()
    {
        this.Username = string.Empty;
        this.Oid = 0;
        this.IsActive = true;
    }
}