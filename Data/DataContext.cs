using Microsoft.EntityFrameworkCore;
using pvo_dictionary.Model;
using System.Reflection.Emit;

namespace pvo_dictionary.Data
{
    public class DataContext : DbContext
    {
        static readonly string connectionString = "Server=localhost; User ID=root; Password=123456; Database=dictionary";
        public DbSet<User> user { get; set; }
        public DbSet<Dictionary> dictionary { get; set; }
        public DbSet<AuditLog> audit_log { get; set; }
        public DbSet<Concept> concept { get; set; }
        public DbSet<Example> example { get; set; }
        public DbSet<ExampleRelationship> example_relationship { get; set; }
        public DbSet<ConceptLink> concept_link { get; set; }
        public DbSet<ConceptRelationship> concept_relationship { get; set; }
        public DbSet<ConceptSearchHistory> concept_search_history { get; set; }
        public DbSet<ExampleLink> example_link { get; set; }
        public DbSet<Tone> tone { get; set; }
        public DbSet<Mode> mode { get; set; }
        public DbSet<Register> register { get; set; }
        public DbSet<Nuance> nuance { get; set; }
        public DbSet<Dialect> dialect { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(c => c.user_id);
            modelBuilder.Entity<Dictionary>().HasKey(d => d.dictionary_id);
            modelBuilder.Entity<AuditLog>().HasKey(a => a.audit_log_id);
            modelBuilder.Entity<Concept>().HasKey(c => c.concept_id);
            modelBuilder.Entity<Example>().HasKey(c => c.example_id);
            modelBuilder.Entity<ExampleRelationship>().HasKey(e => e.example_relationship_id);
            modelBuilder.Entity<ConceptLink>().HasKey(c => c.concept_link_id);
            modelBuilder.Entity<ConceptRelationship>().HasKey(c => c.concept_link_id);
            modelBuilder.Entity<ConceptSearchHistory>().HasKey(c => c.id);
            modelBuilder.Entity<ExampleLink>().HasKey(c => c.example_link_id);
            modelBuilder.Entity<Tone>().HasKey(t => t.tone_id);
            modelBuilder.Entity<Mode>().HasKey(m => m.mode_id );
            modelBuilder.Entity<Register>().HasKey(r => r.register_id);
            modelBuilder.Entity<Nuance>().HasKey(n => n.nuance_id);
            modelBuilder.Entity<Dialect>().HasKey(d => d.dialect_id);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
        
    }
}
