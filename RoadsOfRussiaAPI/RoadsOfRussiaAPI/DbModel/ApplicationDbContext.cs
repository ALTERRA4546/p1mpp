using Microsoft.EntityFrameworkCore;

namespace RoadsOfRussiaAPI.DbModel
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option) { }

        public DbSet<Comment> Comment { get; set; }
        public DbSet<Document> Document { get; set; }
        public DbSet<DocumentCategory> DocumentCategory { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Division> Division { get; set; }
        public DbSet<TemporaryAbsenceCalendar> TemporaryAbsenceCalendar { get; set; }
        public DbSet<TraningCalendar> TraningCalendar { get; set; }
        public DbSet<VacationCalendar> VacationCalendar { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<News> News { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Division>()
                .HasMany(d => d.Division1)
                .WithOne()
                .HasForeignKey(d => d.IDMainDivision);
        }
    }
}
