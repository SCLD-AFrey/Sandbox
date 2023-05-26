using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using VikingEnterprise.Server.Global.FileStructure;

namespace VikingEnterprise.Server.Services.Host.Database;

public class DatabaseUtilities
{
    private readonly CommonFiles m_fileService;

    public DatabaseUtilities(CommonFiles p_fileService)
    {
        m_fileService = p_fileService;
    }

    public IDataLayer GetDataLayer()
    {
        var connectionString = SQLiteConnectionProvider.GetConnectionString(m_fileService.DatabasePath);
        return new SimpleDataLayer(XpoDefault.GetConnectionProvider(connectionString, AutoCreateOption.DatabaseAndSchema));
    }
}