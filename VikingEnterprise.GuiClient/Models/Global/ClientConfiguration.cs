namespace VikingEnterprise.GuiClient.Models.Global;

public class ClientConfiguration
{
    public string? ServerAddress { get; set; } = "localhost";
    public string? Port { get; set; } = "50002";
    public UserCredential LastLogin { get; set; } = new ();
    public bool SkipLogin { get; set; } = false;
    
}