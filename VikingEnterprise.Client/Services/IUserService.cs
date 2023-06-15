using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VikingEnterprise.Client.Models;
using VikingEntity.Server.Protos.UserManager;

namespace VikingEnterprise.Client.Services;

public interface IUserService
{
    public Task LoginUser(out string p_message);

    public Task<UserCredential> GetUser(int p_oid, out string p_message);

    public Task<List<UserCredential>> GetUsers(out string p_message);

    public Task<UserCredential> CreateUser(UserCredential p_userCredential, out string p_message);

    public Task<UserCredential> ModifyUser(UserCredential p_userCredential, out string p_message);
    public List<UserCredential> UserCredentialRepo { get; }
}