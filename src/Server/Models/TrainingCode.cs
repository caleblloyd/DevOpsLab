using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DevOpsLab.Server.Models.BaseModels;
using DevOpsLab.Server.Models.Collections;
using DevOpsLab.Server.Models.Interfaces;
using DevOpsLab.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Server.Models
{
    public class TrainingCode : BaseModel, IHasViewModel<TrainingCodeVM>
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            BaseModel.OnModelCreating<TrainingCode>(modelBuilder);
            modelBuilder.Entity<TrainingCode>(entity =>
            {
                entity.HasMany(m => m.TrainingCodeAppUsers)
                    .WithOne(m => m.TrainingCode)
                    .HasForeignKey(m => m.TrainingCodeId)
                    .HasPrincipalKey(m => m.Id)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(m => m.TrainingCodeTracks)
                    .WithOne(m => m.TrainingCode)
                    .HasForeignKey(m => m.TrainingCodeId)
                    .HasPrincipalKey(m => m.Id)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(m => m.Code);
            });
        }

        private TrainingCodeVM _viewModel;

        [NotMapped]
        public TrainingCodeVM ViewModel
        {
            get
            {
                return _viewModel ??= new TrainingCodeVM
                {
                    Id = Id,
                    Code = Code,
                    MaxUsers = MaxUsers,
                    ExpiresAfter = ExpiresAfter,
                    TrainingCodeAppUsers = TrainingCodeAppUsers.Select(m => m.ViewModel),
                    Tracks = TrainingCodeTracks.Select(m => m.Track.ViewModel)
                };
            }
            
        }

        [Required] public string Code { get; set; }

        public int MaxUsers { get; set; }

        public TimeSpan? ExpiresAfter { get; set; }

        public virtual List<TrainingCodeAppUser> TrainingCodeAppUsers { get; set; } =
            new List<TrainingCodeAppUser>();

        public virtual RankedList<TrainingCodeTrack> TrainingCodeTracks { get; set; } =
            new RankedList<TrainingCodeTrack>();
    }
}
