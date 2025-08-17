namespace Infrastructure.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<ApplicationUser> User { get; set; }
    public DbSet<Chore> Chore { get; set; }
    public DbSet<ChoreCategory> ChoreCategory { get; set; }
    public DbSet<History> History { get; set; }
    public DbSet<Room> Room { get; set; }
    public DbSet<RoomCategory> RoomCategory { get; set; }
    public DbSet<Theme> Theme { get; set; }
    public DbSet<TextTemplate> TextTemplate { get; set; }
    public DbSet<TextFragment> TextFragment { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Chore>()
            .Navigation(e => e.Room)
            .AutoInclude();

        modelBuilder.Entity<Chore>()
            .Navigation(e => e.Category)
            .AutoInclude();

        modelBuilder.Entity<Chore>()
            .HasOne(c => c.Category)
            .WithMany()
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Room>()
            .Navigation(e => e.Chores);

        modelBuilder.Entity<Room>()
            .Navigation(e => e.Category)
            .AutoInclude();

        modelBuilder.Entity<Room>()
            .HasOne(c => c.Category)
            .WithMany()
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
