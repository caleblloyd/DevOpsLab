using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevOpsLab.Server.Db;
using DevOpsLab.Server.Helpers.Data;
using DevOpsLab.Server.Models;
using DevOpsLab.Shared;
using DevOpsLab.Shared.Collections;
using DevOpsLab.Shared.Sort;
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

        public async IAsyncEnumerable<ListResponse<AppUserVM>> UserList(
            AppUserVM filter,
            AppUserSort sort,
            Paginate paginate)
        {
            IQueryable<AppUser> query = _db.Users
                .Include(m => m.UserClaims)
                .Include(m => m.UserRoles)
                .ThenInclude(m => m.Role);

            switch (sort)
            {
                case AppUserSort.EmailAsc:
                    query = query.OrderBy(m => m.Email);
                    break;
                case AppUserSort.EmailDesc:
                    query = query.OrderByDescending(m => m.Email);
                    break;
            }
            
            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(m => m.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(filter.Email))
            {
                query = query.Where(m => m.Email.Contains(filter.Email, StringComparison.OrdinalIgnoreCase));
            }

            yield return await PaginationHelper.FromQueryAsync<AppUser, AppUserVM>(paginate, query);
        }
    }
}
