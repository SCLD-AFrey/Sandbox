using System.Text.Json;
using System.Text.Json.Serialization;
using VikingEnterprise.Server.Global.FileStructure;

namespace VikingEnterprise.Server.Services.Host;

public class Settings
{
    private readonly ILogger<Settings> m_logger;
    private readonly CommonFiles       m_commonFilesService;
    

    [JsonConstructor]
    // Don't rename these variables, they have to match for the json serialization. - Comment by Matt Heimlich on 12/09/2022@11:35:08
    public Settings(string p_port)
    {
        Port        = p_port;
    }
    public Settings(ILogger<Settings> p_logger, CommonFiles p_commonFilesService)
    {
        m_logger             = p_logger;
        m_commonFilesService = p_commonFilesService;
    }
    
    public string Port        { get; set; } = "5001";
    public async Task WriteSettings()
    {
        await using var fs = new FileStream(m_commonFilesService.SettingsPath, FileMode.Create);
        await JsonSerializer.SerializeAsync(fs, this, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        await fs.FlushAsync();
        fs.Close();
    }

    public async Task ReadSettings()
    {
        if (!File.Exists(m_commonFilesService.SettingsPath))
        {
            await WriteSettings();
        }

        await using var storedSettings = File.OpenRead(m_commonFilesService.SettingsPath);
        var             settings       = await JsonSerializer.DeserializeAsync<Settings>(storedSettings);

        if (settings is null) return;
        
        Port = settings.Port;
    }
    
}