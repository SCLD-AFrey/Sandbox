using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using VikingEnterprise.Client.Models;
using VikingEnterprise.Server.Global.FileStructure;

namespace VikingEnterprise.Client.Global.FileStructure
{
    public class CommonFiles
    {
        private readonly CommonDirectories m_commonDirectories;
    
        public CommonFiles(CommonDirectories p_commonDirectories)
        {
            m_commonDirectories = p_commonDirectories;

            LogsPath   = Path.Combine(m_commonDirectories.ServerDataPath, "logs", "client.log");
            SettingsPath   = Path.Combine(m_commonDirectories.ServerDataPath, "settings", "settings.ini");
            DatabasePath   = Path.Combine(m_commonDirectories.ServerDataPath, "db", "data.db");
            ConfigPath   = Path.Combine(m_commonDirectories.ServerDataPath, "settings.config");
        
            CreateNecessaryDirectories();
        }
    
        public string LogsPath      { get; }
        public string SettingsPath      { get; }
        public string DatabasePath      { get; }
        public string ConfigPath      { get; }

        private void CreateNecessaryDirectories()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(LogsPath)    ?? string.Empty);
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)      ?? string.Empty);
            Directory.CreateDirectory(Path.GetDirectoryName(DatabasePath)      ?? string.Empty);
        }
        
        public async Task SaveClientConfig(string? p_serverAddress, string? p_port)
        {
            try
            {
                if ( !File.Exists(ConfigPath) )
                {
                    var serializer = JsonSerializer.Serialize(new ClientConfiguration
                        { ServerAddress = p_serverAddress, Port = p_port});
                    await File.WriteAllTextAsync(ConfigPath, serializer);
                }
                else
                {
                    var configJson = JsonSerializer.Deserialize<ClientConfiguration>(await File.ReadAllTextAsync(ConfigPath));
                    if ( configJson is null )
                    {
                        throw new Exception($"Trouble deserializing the file at '{ConfigPath}'");
                    }

                    configJson.ServerAddress = p_serverAddress;
                    configJson.Port          = p_port;

                    await File.WriteAllTextAsync(ConfigPath, JsonSerializer.Serialize(configJson));
                }
            }
            catch
            {
                // Do Nothing, because if we can't store the valid server than we can't store it. - Comment by Jamie McCoard on 02/12/2022 @ 14:57:08
            }
        }
        
        public ClientConfiguration? GetClientConfig()
        {
            if ( !File.Exists(ConfigPath) ) return null;

            try
            {
                var configJson = JsonSerializer.Deserialize<ClientConfiguration>(File.ReadAllText(ConfigPath));
                if ( configJson is null )
                {
                    throw new Exception($"Trouble deserializing the file at '{ConfigPath}'");
                }

                return configJson;
            }
            catch
            {
                // Couldn't read file. Return null. - Comment by Matt Heimlich on 02/12/2022 @ 14:54:16
                return null;
            }
        }
        
        
    }
}