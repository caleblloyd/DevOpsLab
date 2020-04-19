using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevOpsLab.Server.Db;
using DevOpsLab.Server.Models;
using DevOpsLab.Shared;
using DevOpsLab.Shared.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Server.Hubs
{
    [Authorize(Policy = PolicyTypes.RequireAdmin)]
    public class AdminHub : Hub
    {
        private readonly AppDb _db;

        public AdminHub(AppDb db)
        {
            _db = db;
        }

        public async IAsyncEnumerable<IEnumerable<AppUserVM>> UserList()
        {
            yield return (await _db.Users
                    .Include(m => m.UserClaims)
                    .Include(m => m.UserRoles)
                    .ThenInclude(m => m.Role)
                    .OrderBy(m => m.UserName)
                    .ToListAsync())
                .Select<AppUser, AppUserVM>(m => m);
        }
    }
}
