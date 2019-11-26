using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dapper;
using Microsoft.AspNetCore.Identity;
using BlazorApp1.Models;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace BlazorApp1.Stores
{
    public class RoleStore : IRoleStore<ApplicationRole>
    {
        private readonly string _connectionString;

        public RoleStore(IConfiguration configuration)
        {
            _connectionString = new MySqlConnectionStringBuilder
            {
                Database = "gm",
                Server="127.0.0.1",
                Port = 6380,
                UserID = "root",
                Password = "gmdb!!"

            }.ConnectionString;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                role.RoleId = await connection.QuerySingleAsync<int>($@"INSERT INTO gmrole (Name, NormalizedName)
                    VALUES (@{nameof(ApplicationRole.Name)}, @{nameof(ApplicationRole.NormalizedName)});
                    SELECT LAST_INSERT_ID()", role);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($@"UPDATE gmrole SET
                    Name = @{nameof(ApplicationRole.Name)},
                    NormalizedName = @{nameof(ApplicationRole.NormalizedName)}
                    WHERE RoleId = @{nameof(ApplicationRole.RoleId)}", role);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($"DELETE FROM gmrole WHERE RoleId = @{nameof(ApplicationRole.RoleId)}", role);
            }

            return IdentityResult.Success;
        }

        public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.RoleId.ToString());
        }

        public Task<string> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(ApplicationRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.FromResult(0);
        }

        public Task<string> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(ApplicationRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.FromResult(0);
        }

        public async Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ApplicationRole>($@"SELECT * FROM gmrole
                    WHERE RoleId = @{nameof(roleId)}", new { roleId });
            }
        }

        public async Task<ApplicationRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ApplicationRole>($@"SELECT * FROM gmrole
                    WHERE NormalizedName = @{nameof(normalizedRoleName)}", new { normalizedRoleName });
            }
        }

        public async Task<IEnumerable<ApplicationRole>> FetchRolesAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QueryAsync<ApplicationRole>("SELECT * FROM gmrole");
            }
        }

        public void Dispose()
        {
            // Nothing to dispose.
        }
    }
}
