﻿using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VikingEnterprise.WpfClient.Models.Global;

namespace VikingEnterprise.WpfClient.Services;

public class SettingsService
{
    private readonly ILogger<NavigationService> m_logger;
    private readonly CommonFiles m_commonFiles;

    public SettingsService(CommonFiles p_commonFiles, ILogger<NavigationService> p_logger)
    {
        m_commonFiles = p_commonFiles;
        m_logger = p_logger;
    }

    public ClientConfiguration ClientConfig { get; set; }
    
    public async Task LoadSettings()
    {
        try
        {
            if ( !File.Exists(m_commonFiles.ConfigPath) )
            {
                await SaveSettings();
            }
        
            var configJson = JsonSerializer.Deserialize<ClientConfiguration>(await File.ReadAllTextAsync(m_commonFiles.ConfigPath));

            ClientConfig = configJson 
                           ?? throw new Exception($"Trouble deserializing the file at '{m_commonFiles.ConfigPath}'");
        } catch ( Exception ex )
        {
            ClientConfig = new ClientConfiguration();
            m_logger.LogError(ex, "Error loading settings");
        }

    }
    public async Task SaveSettings()
    {
        try
        {
            if ( !File.Exists(m_commonFiles.ConfigPath) )
            {
                var serializer = JsonSerializer.Serialize(new ClientConfiguration());
                await File.WriteAllTextAsync(m_commonFiles.ConfigPath, serializer);
            }
            else
            {
                ClientConfig.UserCredential.Password = "";
                await File.WriteAllTextAsync(m_commonFiles.ConfigPath, JsonSerializer.Serialize(ClientConfig));
            }
        } catch ( Exception ex )
        {
            ClientConfig = new ClientConfiguration();
            m_logger.LogError(ex, "Error saving settings");
        }

    }
}