using LearningPlan.DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningPlan.DataAccess.Implementation
{
    public class EfContext : DbContext
    {
        public EfContext(DbContextOptions<EfContext> options) : base(options)
        {

        }

        public DbSet<AreaTopic> AreaTopics { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<PlanArea> PlanAreas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<AreaTopic> areaTopicBuilder = modelBuilder.Entity<AreaTopic>();
            areaTopicBuilder.Property(areaTopic => areaTopic.Name).IsRequired();
            areaTopicBuilder.HasOne(areaTopic => areaTopic.PlanArea);

            EntityTypeBuilder<Plan> planBuilder = modelBuilder.Entity<Plan>();
            planBuilder.Property(plan => plan.Name).IsRequired();

            EntityTypeBuilder<PlanArea> planAreaBuilder = modelBuilder.Entity<PlanArea>();
            planAreaBuilder.Property(plan => plan.Name).IsRequired();
            planAreaBuilder.HasOne(area => area.Plan);
        }

    }
}