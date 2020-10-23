using LearningPlan.DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningPlan.DataAccess.Implementation
{
    public class EfContext : DbContext
    {
        public EfContext(DbContextOptions<EfContext> options) : base(options)
        {
            //
        }

        public DbSet<AreaTopic> AreaTopics { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<PlanArea> PlanAreas { get; set; }
        public DbSet<BotSubscription> BotSubscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<AreaTopic> areaTopicBuilder = modelBuilder.Entity<AreaTopic>().ToContainer("AreaTopics");
            areaTopicBuilder.Property(areaTopic => areaTopic.Name).IsRequired();
            areaTopicBuilder.HasOne(areaTopic => areaTopic.PlanArea);
            areaTopicBuilder.HasKey(at => at.Id);
            areaTopicBuilder.HasPartitionKey(at => at.PlanAreaId);

            EntityTypeBuilder<Plan> planBuilder = modelBuilder.Entity<Plan>().ToContainer("Plans");
            planBuilder.Property(plan => plan.Name).IsRequired();
            planBuilder.HasKey(x => x.Id);
            planBuilder.HasPartitionKey(x => x.Id);

            EntityTypeBuilder<PlanArea> planAreaBuilder = modelBuilder.Entity<PlanArea>().ToContainer("PlanAreas");
            planAreaBuilder.Property(planArea => planArea.Name).IsRequired();
            planAreaBuilder.HasOne(area => area.Plan);
            planAreaBuilder.HasMany(planArea => planArea.AreaTopics).WithOne(x => x.PlanArea);
            planAreaBuilder.HasKey(x => x.Id);
            planAreaBuilder.HasPartitionKey(x => x.PlanId);

            EntityTypeBuilder<User> userBuilder = modelBuilder.Entity<User>().ToContainer("Users");
            userBuilder.HasPartitionKey(x => x.Id);
            userBuilder.Property(x => x.Username).IsRequired();
            userBuilder.HasIndex(user => user.Username).IsUnique();
            
            EntityTypeBuilder<BotSubscription> botSubscriptionBuilder = modelBuilder.Entity<BotSubscription>().ToContainer("BotSubscriptions");
            botSubscriptionBuilder.HasPartitionKey(x => x.Id);
            botSubscriptionBuilder.Property(x => x.ChatId).IsRequired();
            botSubscriptionBuilder.Property(x => x.PlanId).IsRequired();
            botSubscriptionBuilder.HasOne(x => x.Plan);
            botSubscriptionBuilder.HasPartitionKey(x => x.ChatId);

            base.OnModelCreating(modelBuilder);
        }
    }
}