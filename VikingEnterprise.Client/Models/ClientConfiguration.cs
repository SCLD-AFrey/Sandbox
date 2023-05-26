namespace VikingEnterprise.Client.Models;

public class ClientConfiguration
{
    public string? ServerAddress { get; set; } = "localhost";
    public string? Port { get; set; } = "50002";
    public bool IsConnected { get; set; } = false;
}