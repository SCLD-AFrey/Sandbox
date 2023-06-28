using DevExpress.Xpo;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Sandbox.UserData;
using VikingEnterprise.Server.Services.Host;
using VikingEnterprise.Server.Services.Host.Database;
using VikingEntity.Server.Protos.UserManager;

namespace VikingEnterprise.Server.Services.Grpc;

public class UserService : UserManagerRpc.UserManagerRpcBase
{
     private readonly DatabaseInterface m_databaseInterface;
     private readonly EncryptionService m_encryptionService;

     public UserService(DatabaseInterface p_databaseInterface, EncryptionService p_encryptionService)
     {
         m_databaseInterface = p_databaseInterface;
         m_encryptionService = p_encryptionService;
     }

     public override async Task<G_GetUsersResponse> GetUsers(G_GetUsersRequest p_request, ServerCallContext p_context)
     {
         var reply = new G_GetUsersResponse();

         try
         {
             reply.Success = new G_GetUsersResponse_Success()
             {
                 Users =
                 {
                     await m_databaseInterface.ProvisionUnitOfWork().Query<User>().Select(p_user => new G_User()
                     {
                         Oid = p_user.Oid,
                         Username = p_user.Username,
                         IsActive = p_user.IsActive
                     }).ToListAsync()
                 }
             };
         } catch (RpcException e)
         {
             reply.Failure = new G_Response_Failure()
             {
                 Message = e.Message
             };
         } catch (Exception e)
         {
             reply.Failure = new G_Response_Failure()
             {
                 Message = e.Message
             };
         }

         return await Task.FromResult(reply);
     }
     public override async Task<G_ModifyUserResponse> ModifyUser(G_ModifyUserRequest p_request, ServerCallContext p_context)
     {
         var reply = new G_ModifyUserResponse();
         try
         {
             var uow = m_databaseInterface.ProvisionUnitOfWork();
             var user = await uow.GetObjectByKeyAsync<User>(p_request.User.Oid);
             if (user == null)
             {
                 reply.Failure = new G_Response_Failure()
                 {
                     Message = "User not found"
                 };
             }
             user!.IsActive = p_request.User.IsActive;
             user.Username = p_request.User.Username;
             user.UserCredentials.Add(new UserCredential(uow)
             {
                 Password = m_encryptionService.GeneratePasswordHash(p_request.User.Password, out var salt),
                 Salt = salt
             });
             await uow.CommitChangesAsync();
         }
         catch (Exception e)
         {
             reply.Failure = new G_Response_Failure()
             {
                 Message = e.Message
             };
         }

         return await Task.FromResult(reply);
     }
     public override async Task<G_CreateUserResponse> CreateUser(G_CreateUserRequest p_request, ServerCallContext p_context)
     {
        var reply = new G_CreateUserResponse();
        try
        {
            var uow = m_databaseInterface.ProvisionUnitOfWork();
            var user = new User(uow)
            {
                Username = p_request.Username,
                IsActive = true
            };
            user.UserCredentials.Add(new UserCredential(uow)
            {
                Password = m_encryptionService.GeneratePasswordHash(p_request.Password, out var salt),
                Salt = salt
            });
            await uow.CommitChangesAsync();
            reply.Success = new G_CreateUserResponse_Success()
            {
                User = new G_User()
                {
                    Oid = user.Oid,
                    Username = user.Username,
                    IsActive = user.IsActive
                }
            };
        } catch (Exception e)
        {
            reply.Failure = new G_Response_Failure()
            {
                Message = e.Message
            };
            return await Task.FromResult(reply);
        }

        return await Task.FromResult(reply);
     }
     public override async Task<G_LoginResponse> Login(G_LoginRequest p_request, ServerCallContext p_context)
     {
         var uow = m_databaseInterface.ProvisionUnitOfWork();
         G_LoginResponse reply = new G_LoginResponse()
         {
             ServerTime = DateTime.UtcNow.ToTimestamp()
         };
         
         User user = uow.Query<User>()
             .FirstOrDefaultAsync(p_x => p_x.Username == p_request.Username)
             .Result;
         
         if (user == null)
         {
             reply.Failure = new G_Response_Failure()
             {
                 Message = "User not found"
             };
             return await Task.FromResult(reply);
         }

         if (user.IsActive == false)
         {
             reply.Failure = new G_Response_Failure()
             {
                 Message = "User is not active"
             };
             return await Task.FromResult(reply);
         }
         if (user.UserCredentials.Count == 0)
         {
             reply.Failure = new G_Response_Failure()
             {
                 Message = "No credentials found"
             };
             return await Task.FromResult(reply);
         }
         
         var cred = user.UserCredentials
             .LastOrDefault(p_x => ReferenceEquals(p_x.User, user));

         if (m_encryptionService.VerifyPassword(p_request.Password, cred!.Password, cred.Salt))
         {
             reply.Success = new G_LoginResponse_Success()
             {
                 User = new G_User()
                 {
                     Oid = user.Oid,
                     Username = user.Username,
                     IsActive = user.IsActive
                 }
             };
         }
         else
         {
             reply.Failure = new G_Response_Failure()
             {
                 Message = "Unable to verify password"
             };
         }
         return await Task.FromResult(reply);
     }
     public override async Task<G_GetUserResponse> GetUser(G_GetUserRequest p_request, ServerCallContext p_context)
     {
         var reply = new G_GetUserResponse();
         var uow = m_databaseInterface.ProvisionUnitOfWork();
         var user = new User(uow);
         switch (p_request.RequestCase)
         {
             case G_GetUserRequest.RequestOneofCase.ByUsername:
                 if (string.IsNullOrEmpty(p_request.ByUsername.Username))
                 {
                     reply.Failure = new G_Response_Failure()
                     {
                         Message = $"User '{p_request.ByUsername.Username}' is invalid"
                     };
                 }
                 user = uow.Query<User>()
                     .FirstOrDefaultAsync(p_x => string.Equals(p_x.Username, p_request.ByUsername.Username, StringComparison.CurrentCultureIgnoreCase))
                     .Result;
                 break;
             case G_GetUserRequest.RequestOneofCase.ByOid:
                 if (p_request.ByOid.Oid <= 0)
                 {
                     reply.Failure = new G_Response_Failure()
                     {
                         Message = $"User ID '{p_request.ByOid.Oid}' is invalid"
                     };
                 }
                 user = uow.Query<User>()
                     .FirstOrDefaultAsync(p_x => p_x.Oid == p_request.ByOid.Oid)
                     .Result;
                 break;
             case G_GetUserRequest.RequestOneofCase.None:
                 reply.Failure = new G_Response_Failure()
                 {
                     Message = "Unknown request type"
                 };
                 break;
         }
         
         

         
         if (user == null)
         {
             reply.Failure = new G_Response_Failure()
             {
                 Message = $"User not found"
             };
         }
         
         else
         {
             reply.Success = new G_GetUserResponse_Success()
             {
                 User = new G_User()
                 {
                     Oid = user.Oid,
                     Username = user.Username,
                     IsActive = user.IsActive
                 }
             };
         }

         return await Task.FromResult(reply);
     }
}