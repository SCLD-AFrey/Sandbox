using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VikingEnterprise.GuiClient.Models.Global;

namespace VikingEnterprise.GuiClient.Services;

public class SettingsService
{
    private readonly ILogger<SettingsService> m_logger;
    private readonly CommonFiles m_commonFiles;

    public SettingsService(CommonFiles p_commonFiles, ILogger<SettingsService> p_logger)
    {
        m_commonFiles = p_commonFiles;
        m_logger = p_logger;
        m_logger.LogInformation("SettingsService created");
    }
    public ClientConfiguration ClientConfiguration { get; set; }
    
    public async Task SetCurrentUser(UserCredential p_userCredential)
    {
        ClientConfiguration.LastLogin = p_userCredential;
        await SaveSettings();
    }
    
    public async Task LoadSettings()
    {
        try
        {
            if ( !File.Exists(m_commonFiles.ConfigPath) )
            {
                await SaveSettings();
            }
        
            var configJson = JsonSerializer.Deserialize<ClientConfiguration>(await File.ReadAllTextAsync(m_commonFiles.ConfigPath));

            ClientConfiguration = configJson 
                                  ?? throw new Exception($"Trouble deserializing the file at '{m_commonFiles.ConfigPath}'");
        } catch ( Exception ex )
        {
            ClientConfiguration = new ClientConfiguration();
            m_logger.LogError(ex, "Error loading settings");
        }

    }
    public async Task SaveSettings()
    {
        try
        {
            if ( !File.Exists(m_commonFiles.ConfigPath) )
            {
                var serializer = JsonSerializer.Serialize(new ClientConfiguration(), new JsonSerializerOptions()
                {
                    WriteIndented = true
                });
                await File.WriteAllTextAsync(m_commonFiles.ConfigPath, serializer);
            }
            else
            {
                await File.WriteAllTextAsync(m_commonFiles.ConfigPath, JsonSerializer.Serialize(ClientConfiguration));
            }
        } catch ( Exception ex )
        {
            ClientConfiguration = new ClientConfiguration();
            m_logger.LogError(ex, "Error saving settings");
        }

    }
}