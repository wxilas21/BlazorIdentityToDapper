using BlazorApp1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using System.Security.Claims;

namespace BlazorApp1.Stores
{
    public class UserStore : IUserStore<ApplicationUser>, IUserEmailStore<ApplicationUser>, IUserPhoneNumberStore<ApplicationUser>,
        IUserTwoFactorStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserRoleStore<ApplicationUser>, IQueryableUserStore<ApplicationUser>,
        IUserClaimStore<ApplicationUser>
    {
        private readonly string _connectionString;

        public UserStore(IConfiguration configuration)
        {
            _connectionString = new MySqlConnectionStringBuilder
            {
                Database = "gm",
                Server = "127.0.0.1",
                Port = 6380,
                UserID = "root",
                Password = "gmdb!!"

            }.ConnectionString;
        }

        public void Dispose()
        {
            // Nothing to dispose.
        }

        #region User
        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                user.Id = await connection.QuerySingleAsync<int>($@"INSERT INTO gmuser (UserName, NormalizedUserName, Email,
                    NormalizedEmail, EmailConfirmed, PasswordHash, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled)
                    VALUES (@{nameof(ApplicationUser.UserName)}, @{nameof(ApplicationUser.NormalizedUserName)}, @{nameof(ApplicationUser.Email)},
                    @{nameof(ApplicationUser.NormalizedEmail)}, @{nameof(ApplicationUser.EmailConfirmed)}, @{nameof(ApplicationUser.PasswordHash)},
                    @{nameof(ApplicationUser.PhoneNumber)}, @{nameof(ApplicationUser.PhoneNumberConfirmed)}, @{nameof(ApplicationUser.TwoFactorEnabled)});
                    SELECT LAST_INSERT_ID()", user);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($"DELETE FROM gmuser WHERE Id = @{nameof(ApplicationUser.Id)}", user);
            }

            return IdentityResult.Success;
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ApplicationUser>($@"SELECT * FROM gmuser
                    WHERE Id = @{nameof(userId)}", new { userId });
            }
        }

        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ApplicationUser>($@"SELECT * FROM gmuser
                    WHERE NormalizedUserName = @{nameof(normalizedUserName)}", new { normalizedUserName });
            }
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($@"UPDATE gmuser SET
                    UserName = @{nameof(ApplicationUser.UserName)},
                    NormalizedUserName = @{nameof(ApplicationUser.NormalizedUserName)},
                    Email = @{nameof(ApplicationUser.Email)},
                    NormalizedEmail = @{nameof(ApplicationUser.NormalizedEmail)},
                    EmailConfirmed = @{nameof(ApplicationUser.EmailConfirmed)},
                    PasswordHash = @{nameof(ApplicationUser.PasswordHash)},
                    PhoneNumber = @{nameof(ApplicationUser.PhoneNumber)},
                    PhoneNumberConfirmed = @{nameof(ApplicationUser.PhoneNumberConfirmed)},
                    TwoFactorEnabled = @{nameof(ApplicationUser.TwoFactorEnabled)}
                    WHERE Id = @{nameof(ApplicationUser.Id)}", user);
            }

            return IdentityResult.Success;
        }
        #endregion

        #region Email
        public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ApplicationUser>($@"SELECT * FROM gmuser
                    WHERE NormalizedEmail = @{nameof(normalizedEmail)}", new { normalizedEmail });
            }
        }

        public Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }
        #endregion

        #region PhoneNumber
        public Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }
        #endregion

        #region TwoFactor
        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }
        #endregion

        #region Password
        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }
        #endregion

        #region Role
        public async Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                var normalizedName = roleName.ToUpper();
                var roleId = await connection.ExecuteScalarAsync<int?>($"SELECT RoleId FROM gmrole WHERE NormalizedName = @{nameof(normalizedName)}", new { normalizedName });
                if (!roleId.HasValue)
                    roleId = await connection.ExecuteAsync($"INSERT INTO gmrole(Name, NormalizedName) VALUES(@{nameof(roleName)}, @{nameof(normalizedName)})",
                        new { roleName, normalizedName });

                await connection.ExecuteAsync($"INSERT INTO gmuserrole(gmId, RoleId) VALUES(@userId, @{nameof(roleId)}) ON DUPLICATE KEY UPDATE gmId = @userId",
                    new { userId = user.Id, roleId });
            }
        }

        public async Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                var roleId = await connection.ExecuteScalarAsync<int?>("SELECT RoleId FROM gmrole WHERE NormalizedName = @normalizedName", new { normalizedName = roleName.ToUpper() });
                if (true == roleId.HasValue)
                {
                    var gmId = await connection.ExecuteScalarAsync<int?>($"SELECT gmId FROM gmuserrole WHERE gmId = @userId AND RoleId = @{nameof(roleId)}", new { userId = user.Id, roleId });
                    if (true == gmId.HasValue)
                    {
                        await connection.ExecuteAsync($"DELETE FROM gmuserrole WHERE gmId = @userId AND RoleId = @{nameof(roleId)}", new { userId = user.Id, roleId });
                    }
                }
            }
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                var queryResults = await connection.QueryAsync<string>("SELECT r.Name FROM gmrole r INNER JOIN gmuserrole ur ON ur.RoleId = r.RoleId " +
                    "WHERE ur.gmId = @userId", new { userId = user.Id });

                return queryResults.ToList();
            }
        }

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new MySqlConnection(_connectionString))
            {
                var roleId = await connection.ExecuteScalarAsync<int?>("SELECT RoleId FROM gmrole WHERE NormalizedName = @normalizedName", new { normalizedName = roleName.ToUpper() });
                if (roleId == default(int)) return false;
                var matchingRoles = await connection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM gmuserrole WHERE gmId = @userId AND RoleId = @{nameof(roleId)}",
                    new { userId = user.Id, roleId });

                return matchingRoles > 0;
            }
        }

        public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new MySqlConnection(_connectionString))
            {
                var queryResults = await connection.QueryAsync<ApplicationUser>(@"SELECT u.* FROM gmuser u 
                    INNER JOIN gmuserrole ur ON ur.gmId = u.Id INNER JOIN gmrole r ON r.RoleId = ur.RoleId WHERE r.NormalizedName = @normalizedName",
                    new { normalizedName = roleName.ToUpper() });

                return queryResults.ToList();
            }
        }
        #endregion

        #region IQueryableUserStore
        public IQueryable<ApplicationUser> Users
        {
            get
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    return connection.Query<ApplicationUser>("SELECT * FROM gmuser").AsQueryable();
                }
            }
        }
        #endregion

        #region Claim
        public async Task AddClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                foreach(Claim claim in claims)
                {
                    await connection.ExecuteAsync($"INSERT INTO gmclaims(gmId, ClaimType, ClaimValue) VALUES(@userId, @{nameof(claim.Type)}, @{nameof(claim.Value)}) ON DUPLICATE KET UPDATE gmId = @userId",
                        new { userId = user.Id, claim.Type, claim.Value });
                }
            }
        }

        public async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            IEnumerable<ApplicationUserClaim> userClaims;
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                userClaims = await connection.QueryAsync<ApplicationUserClaim>($"SELECT gmId, ClaimType, ClaimValue FROM gmclaims WHERE gmId = @userId",
                        new { userId = user.Id });
            }

            IList<Claim> ret = null;
            if(0 != userClaims.Count())
            {
                ret = userClaims.Select(s => s.ToClaim()).ToList();
            }

            return ret ?? new System.Collections.Generic.List<Claim>();
        }

        public async Task<IList<ApplicationUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            IEnumerable<ApplicationUser> enumerableUsers;
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                enumerableUsers = await connection.QueryAsync<ApplicationUser>($@"SELECT u.* FROM gmuser u INNER JOIN gmclaims gr ON u.Id = gr.gmId 
                        WHERE gr.ClaimType = @{nameof(claim.Type)} AND gr.ClaimValue = @{nameof(claim.Value)}", new { claim.Type, claim.Value });
            }

            IList<ApplicationUser> ret = null;
            if(0 != enumerableUsers.Count())
            {
                ret = enumerableUsers.ToList();
            }

            return ret;
        }

        public async Task RemoveClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                foreach(Claim claim in claims)
                {
                    await connection.ExecuteAsync($"DELETE FROM gmclaims WHERE gmId = @userId AND ClaimType = @{nameof(claim.Type)} AND ClaimValue = @{nameof(claim.Value)}",
                        new { userId = user.Id, claim.Type, claim.Value });
                }
            }
        }

        public async Task ReplaceClaimAsync(ApplicationUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
            }
        }
        #endregion
    }
}
