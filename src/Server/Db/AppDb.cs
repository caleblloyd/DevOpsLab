using DevOpsLab.Server.Models;
using DevOpsLab.Server.Services;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DevOpsLab.Server.Db
{
    public class AppDb : ApiAuthorizationDbContext<AppUser, AppRole, AppUserRole>
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

            AppRole.OnModelCreating(modelBuilder);
            AppUser.OnModelCreating(modelBuilder);
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
