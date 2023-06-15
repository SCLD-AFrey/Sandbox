using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace VikingEnterprise.WpfClient.Models.Global;

public class CommonFiles
{
    private readonly CommonDirectories m_commonDirectories;

    public CommonFiles(CommonDirectories p_commonDirectories)
    {
        m_commonDirectories = p_commonDirectories;

        LogsPath   = Path.Combine(m_commonDirectories.ServerDataPath, "logs", "client.log");
        SettingsPath   = Path.Combine(m_commonDirectories.ServerDataPath, "settings", "settings.ini");
        DatabasePath   = Path.Combine(m_commonDirectories.ServerDataPath, "db", "data.db");
        ConfigPath   = Path.Combine(m_commonDirectories.ServerDataPath, "settings", "client.config");
    
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
    
}