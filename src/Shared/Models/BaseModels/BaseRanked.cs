using DevOpsLab.Shared.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Shared.Models.BaseModels
{
    public abstract class BaseRankedModel : BaseModel, IRanked
    {
        protected new static void OnModelCreating<T>(ModelBuilder modelBuilder) where T : BaseRankedModel
        {
            BaseModel.OnModelCreating<T>(modelBuilder);
            modelBuilder.Entity<T>(entity => { entity.HasIndex(m => m.Rank); });
        }

        public double? Rank { get; set; }
    }
}
