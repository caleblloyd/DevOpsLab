using System;
using System.ComponentModel.DataAnnotations;
using DevOpsLab.Shared.Helpers.Data;
using DevOpsLab.Shared.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Shared.Models.BaseModels
{
    public abstract class BaseModel : IModel
    {
        protected static void OnModelCreating<T>(ModelBuilder modelBuilder) where T:BaseModel
        {
            modelBuilder.Entity<T>(entity =>
            {
                entity.Property(m => m.Id)
                    .HasValueGenerator(typeof(SequentialGuidValueGenerator));
                entity.Property(m => m.Created)
                    .HasColumnType("TIMESTAMP WITH TIME ZONE")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
        
        [Key] public Guid Id { get; set; }
        
        public DateTime Created { get; set; }
    }
}