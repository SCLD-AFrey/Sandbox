namespace VikingEnterprise.WpfClient.Models.Global;

public class ClientConfiguration
{
    public string? ServerAddress { get; set; } = "localhost";
    public string? Port { get; set; } = "50002";
    public UserCredential UserCredential { get; set; } = new ();
    public bool RequireLogin { get; set; } = true;
    
}