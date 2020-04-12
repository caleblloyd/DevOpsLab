using DevOpsLab.Shared.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DevOpsLab.Server.Db
{
    public class AppDb : ApiAuthorizationDbContext<AppUser>
    {
        public AppDb(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Scenario> Scenarios { get; set; }

        public DbSet<Track> Tracks { get; set; }

        public DbSet<TrackCourse> TrackCourses { get; set; }

        public DbSet<TrainingCode> TrainingCodes { get; set; }

        public DbSet<TrainingCodeAppUser> TrainingCodeAppUsers { get; set; }

        public DbSet<TrainingCodeTrack> TrainingCodeTracks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Course.OnModelCreating(modelBuilder);
            Scenario.OnModelCreating(modelBuilder);
            Track.OnModelCreating(modelBuilder);
            TrackCourse.OnModelCreating(modelBuilder);
            TrainingCode.OnModelCreating(modelBuilder);
            TrainingCodeAppUser.OnModelCreating(modelBuilder);
            TrainingCodeTrack.OnModelCreating(modelBuilder);
        }
    }
}
