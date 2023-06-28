using System;
using System.IO;

namespace VikingEnterprise.GuiClient.Models.Global;

public class CommonDirectories
{
    public CommonDirectories()
    {
        Directory.CreateDirectory(ServerDataPath);
    }
    public string ServerDataPath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), ".VikingEnterprise");

}