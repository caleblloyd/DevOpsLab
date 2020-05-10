using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DevOpsLab.Server.Models.BaseModels;
using DevOpsLab.Server.Models.Interfaces;
using DevOpsLab.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Server.Models
{
    public class TrainingCodeAppUser : BaseModel, IHasViewModel<TrainingCodeAppUserVM>
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            BaseModel.OnModelCreating<TrainingCodeAppUser>(modelBuilder);
            modelBuilder.Entity<TrainingCodeAppUser>(entity =>
            {
                entity.HasOne(m => m.TrainingCode)
                    .WithMany(m => m.TrainingCodeAppUsers)
                    .HasForeignKey(m => m.TrainingCodeId)
                    .HasPrincipalKey(m => m.Id)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(m => m.AppUser)
                    .WithMany(m => m.TrainingCodeAppUsers)
                    .HasForeignKey(m => m.AppUserId)
                    .HasPrincipalKey(m => m.Id)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private TrainingCodeAppUserVM _viewModel;

        [NotMapped]
        public TrainingCodeAppUserVM ViewModel
        {
            get
            {
                return _viewModel ??= new TrainingCodeAppUserVM
                {
                    Id = Id,
                    TrainingCode = TrainingCode.ViewModel,
                    AppUser = AppUser.ViewModel,
                    Expires = Expires
                };
            }
        }

        public Guid TrainingCodeId { get; set; }
        public virtual TrainingCode TrainingCode { get; set; }

        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }

        public bool Active { get; set; } = true;

        [NotMapped]
        public DateTime? Expires
        {
            get
            {
                if (TrainingCode == default)
                {
                    return default;
                }

                if (TrainingCode.ExpiresDate.HasValue)
                {
                    return TrainingCode.ExpiresDate.Value;
                }

                if (TrainingCode.ExpiresAfter.HasValue)
                {
                    return Created + TrainingCode.ExpiresAfter.Value;
                }

                return default;
            }
        }
    }
}
