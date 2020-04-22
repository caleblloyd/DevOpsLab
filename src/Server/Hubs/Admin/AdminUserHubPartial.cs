using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using DevOpsLab.Server.Helpers.Data;
using DevOpsLab.Server.Helpers.Hub;
using DevOpsLab.Server.Models;
using DevOpsLab.Shared.Collections;
using DevOpsLab.Shared.Filter;
using DevOpsLab.Shared.Sort;
using DevOpsLab.Shared.ViewModels;
using IdentityModel;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Server.Hubs.Admin
{
    public partial class AdminHub
    {
        public async IAsyncEnumerable<ListResponse<AppUserVM>> UserList(
            AppUserFilter filter,
            AppUserSort sort,
            Paginate paginate)
        {
            yield return await HubHelper.WrapAsync(_logger, async () =>
            {
                IQueryable<AppUser> query = _db.Users;

                if (!string.IsNullOrEmpty(filter.Email))
                {
                    query = query.Where(m => EF.Functions.ILike(m.Email, $"%{filter.Email}%"));
                }

                if (!string.IsNullOrEmpty(filter.Name))
                {
                    query = query.Include(m => m.UserClaims.Where(
                        uc => uc.ClaimType == JwtClaimTypes.Name
                              && EF.Functions.ILike(uc.ClaimValue, $"%{filter.Name}%")));
                }
                else
                {
                    query = query.Include(m => m.UserClaims.Where(
                        uc => uc.ClaimType == JwtClaimTypes.Name));
                }

                if (!string.IsNullOrEmpty(filter.Role))
                {
                    query = query
                        .Include(m => m.UserRoles.Where(
                            ur => EF.Functions.ILike(ur.Role.Name, $"%{filter.Role}%")))
                        .ThenInclude(m => m.Role);
                }
                else
                {
                    query = query
                        .Include(m => m.UserRoles)
                        .ThenInclude(m => m.Role);
                }

                switch (sort)
                {
                    case AppUserSort.EmailAsc:
                        query = query.OrderBy(m => m.Email);
                        break;
                    case AppUserSort.EmailDesc:
                        query = query.OrderByDescending(m => m.Email);
                        break;
                    case AppUserSort.NameAsc:
                        query = query.OrderBy(m => m.UserClaims.OrderBy(uc => uc.ClaimValue));
                        break;
                    case AppUserSort.NameDesc:
                        query = query.OrderByDescending(m => m.UserClaims.OrderByDescending(uc => uc.ClaimValue));
                        break;
                    case AppUserSort.RoleAsc:
                        query = query.OrderBy(m => m.UserRoles.OrderBy(ur => ur.Role.Name));
                        break;
                    case AppUserSort.RoleDesc:
                        query = query.OrderByDescending(m => m.UserRoles.OrderByDescending(ur => ur.Role.Name));
                        break;
                }

                return await PaginationHelper.FromQueryAsync<AppUser, AppUserVM>(paginate, query);
            });
        }

        public async IAsyncEnumerable<bool> UserEdit(AppUserVM appUserVM)
        {
            yield return await HubHelper.WrapAsync(_logger, async () =>
            {
                var user = await _db.Users.FirstOrDefaultAsync(m => m.Id == appUserVM.Id);
                if (user == default)
                {
                    throw new HubException("User does not exist");
                }

                var nameClaim = (await _userManager.GetClaimsAsync(user))
                    .FirstOrDefault(m => m.Type == JwtClaimTypes.Name);

                if (nameClaim == null || appUserVM.Name != nameClaim.Value)
                {
                    var newNameClaim = new Claim(JwtClaimTypes.Name, appUserVM.Name);
                    if (nameClaim == null)
                    {
                        await _userManager.AddClaimAsync(user, newNameClaim);
                    }
                    else
                    {
                        await _userManager.ReplaceClaimAsync(user, nameClaim, newNameClaim);
                    }
                }

                return true;
            });
        }

        public async IAsyncEnumerable<bool> UserDelete(AppUserVM appUserVM)
        {
            yield return await HubHelper.WrapAsync(_logger, async () =>
            {
                var user = await _db.Users.FirstOrDefaultAsync(m => m.Id == appUserVM.Id);
                if (user == default)
                {
                    throw new HubException("User does not exist");
                }

                if (Context.UserIdentifier == user.Id)
                {
                    throw new HubException("You cannot delete yourself");
                }

                await _userManager.DeleteAsync(user);
                return true;
            });
        }
    }
}
