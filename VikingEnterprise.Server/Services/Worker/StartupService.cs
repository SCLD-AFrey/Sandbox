using DevExpress.Xpo;
using Sandbox.UserData;
using VikingEnterprise.Server.Services.Host;
using VikingEnterprise.Server.Services.Host.Database;

namespace VikingEnterprise.Server.Services.Worker;

public class StartupService
{
    private readonly ILogger<StartupService>            m_logger;
    private readonly DatabaseInterface                m_databaseInterface;
    private readonly EncryptionService               m_encryptionService;

    public StartupService(ILogger<StartupService>          p_logger, DatabaseInterface p_databaseInterface, EncryptionService p_encryptionService)
    {
        m_logger                            = p_logger;
        m_databaseInterface = p_databaseInterface;
        m_encryptionService = p_encryptionService;
    }

    public async Task InitDatabase()
    {
        await InitAdminUser();
    }

    private async Task InitAdminUser()
    {
        using var uow = m_databaseInterface.ProvisionUnitOfWork();

        var queryable = uow.Query<User>().Where(p_x => p_x.Username == "admin");
        if (queryable.Count() == 1) return;
        var adminUser = new User(uow)
        {
            Username = "admin",
            IsActive = true,
            UserCredentials = { new UserCredential(uow)
            {
                DateAdded = DateTime.UtcNow,
                Password = m_encryptionService.GeneratePasswordHash("password", out var salt),
                Salt = salt
            } }
        };
        await uow.CommitChangesAsync();
    }
}