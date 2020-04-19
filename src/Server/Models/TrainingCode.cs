using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DevOpsLab.Server.Models.BaseModels;
using DevOpsLab.Server.Models.Collections;
using DevOpsLab.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Server.Models
{
    public class TrainingCode : BaseModel
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
        
        private TrainingCodeVM ViewModel { get; set; }

        public static implicit operator TrainingCodeVM(TrainingCode model)
        {
            return model.ViewModel ??= new TrainingCodeVM
            {
                Id = model.Id,
                Code = model.Code,
                MaxUsers = model.MaxUsers,
                ExpiresAfter = model.ExpiresAfter,
                TrainingCodeAppUsers = model.TrainingCodeAppUsers
                    .Select<TrainingCodeAppUser, TrainingCodeAppUserVM>(m => m),
                Tracks = model.TrainingCodeTracks
                    .Select<TrainingCodeTrack, TrackVM>(m => m.Track)
            };
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
