using System;
using System.Linq;
using System.Threading.Tasks;
using DevOpsLab.Server.Models.Interfaces;
using DevOpsLab.Shared.Collections;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Server.Helpers.Data
{
    public static class PaginationHelper
    {
        public static async Task<ListResponse<TViewModel>> FromQueryAsync<TModel, TViewModel>
        (
            Paginate paginate,
            IQueryable<TModel> query)
            where TModel : IHasViewModel<TViewModel>
        {
            var count = await query.CountAsync();

            if (paginate.Limit > 0)
            {
                query = query.Skip(paginate.Offset).Take(paginate.Limit);
            }

            var items = (await query.ToListAsync()).Select(m => m.ViewModel);
            return new ListResponse<TViewModel>
            {
                Total = count,
                Items = items
            };
        }
    }
}
