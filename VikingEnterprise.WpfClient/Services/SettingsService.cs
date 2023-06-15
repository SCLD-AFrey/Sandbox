using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using VikingEnterprise.WpfClient.Models.Global;

namespace VikingEnterprise.WpfClient.Services;

public class SettingsService
{
    private readonly CommonFiles m_commonFiles;

    public SettingsService(CommonFiles p_commonFiles)
    {
        m_commonFiles = p_commonFiles;
    }

    public ClientConfiguration ClientConfig { get; set; }
    
    public async void LoadSettings()
    {
        if ( !File.Exists(m_commonFiles.ConfigPath) )
        {
            await SaveSettings();
        }
        
        var configJson = JsonSerializer.Deserialize<ClientConfiguration>(File.ReadAllText(m_commonFiles.ConfigPath));
        if ( configJson is null )
        {
            throw new Exception($"Trouble deserializing the file at '{m_commonFiles.ConfigPath}'");
        }

        ClientConfig = configJson;
    }
    public async Task SaveSettings()
    {
        if ( !File.Exists(m_commonFiles.ConfigPath) )
        {
            var serializer = JsonSerializer.Serialize(new ClientConfiguration());
            await File.WriteAllTextAsync(m_commonFiles.ConfigPath, serializer);
        }
        else
        {
            ClientConfig.UserCredential.Password = "XXXXXXXXXXXXXXXXXX";
            await File.WriteAllTextAsync(m_commonFiles.ConfigPath, JsonSerializer.Serialize(ClientConfig));
        }
    }
}