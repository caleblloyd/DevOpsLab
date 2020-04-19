using System;
using System.Collections.Generic;
using System.Linq;
using DevOpsLab.Server.Db;
using DevOpsLab.Server.Helpers.Data;
using DevOpsLab.Server.Models;
using DevOpsLab.Shared;
using DevOpsLab.Shared.Collections;
using DevOpsLab.Shared.Filter;
using DevOpsLab.Shared.Sort;
using DevOpsLab.Shared.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DevOpsLab.Server.Hubs
{
    [Authorize(Policy = PolicyTypes.RequireAdmin)]
    public class AdminHub : Hub
    {
        private readonly AppDb _db;
        private readonly ILogger<AdminHub> _logger;

        public AdminHub(AppDb db, ILogger<AdminHub> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async IAsyncEnumerable<ListResponse<AppUserVM>> UserList(
            AppUserFilter filter,
            AppUserSort sort,
            Paginate paginate)
        {
            ListResponse<AppUserVM> result;
            try
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

                if (!string.IsNullOrEmpty(filter.Email))
                {
                    query = query.Where(m => EF.Functions.ILike(m.Email, $"%{filter.Email}%"));
                }

                result = await PaginationHelper.FromQueryAsync<AppUser, AppUserVM>(paginate, query);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "\n" + e.StackTrace, e);
                throw;
            }

            yield return result;
        }
    }
}
