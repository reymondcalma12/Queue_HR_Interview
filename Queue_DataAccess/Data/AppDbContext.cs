using Microsoft.EntityFrameworkCore;
using Queue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Queue.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Applicant_Form> Applicant_Form { get; set; }
        public DbSet<Applicant_Status> Applicant_Status { get; set; }
        //public DbSet<Applicant_Reject> Applicant_Reject { get; set; }
        public DbSet<Queue_Status> Queue_Status { get; set; }

        public DbSet<Serving> Serving { get; set; }
        public DbSet<Queue_Stage_1> Queue_Stage_1 { get; set; }
        public DbSet<Queue_Stage_2> Queue_Stage_2 { get; set; }
        public DbSet<Queue_Stage_3> Queue_Stage_3 { get; set; }

        public DbSet<Table> Table { get; set; }
        public DbSet<Stage> Stage { get; set; }

        public DbSet<Table_Serve> Table_Serve { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Seeding
            modelBuilder.Entity<Table>().HasData(
                new Table { TableId = 1, Username = "Table 1", StageId = 1 },
                new Table { TableId = 2, Username = "Table 2", StageId = 1 },
                new Table { TableId = 3, Username = "Table 3", StageId = 1 },
                new Table { TableId = 4, Username = "Table 4", StageId = 1 },
                new Table { TableId = 5, Username = "Table 5", StageId = 1 },
                
                new Table { TableId = 6, Username = "Table 1", StageId = 2 },
                new Table { TableId = 7, Username = "Table 2", StageId = 2 },
                new Table { TableId = 8, Username = "Table 3", StageId = 2 },
                new Table { TableId = 9, Username = "Table 4", StageId = 2 },
                new Table { TableId = 10, Username = "Table 5", StageId = 2 },
                new Table { TableId = 11, Username = "Table 6", StageId = 2 },
                new Table { TableId = 12, Username = "Table 7", StageId = 2 },
                new Table { TableId = 13, Username = "Table 8", StageId = 2 },
                new Table { TableId = 14, Username = "Table 9", StageId = 2 },
                new Table { TableId = 15, Username = "Table 10", StageId = 2 },
                new Table { TableId = 16, Username = "Table 11", StageId = 2 },
                new Table { TableId = 17, Username = "Table 12", StageId = 2 },

                new Table { TableId = 18, Username = "Room - HUMILITY", StageId = 3 },
                new Table { TableId = 19, Username = "Room - OPENNESS", StageId = 3 },
                new Table { TableId = 20, Username = "Room - OWNER'S MINDSET", StageId = 3 },
                new Table { TableId = 21, Username = "Room - TRANSPARENCY", StageId = 3 },
                new Table { TableId = 22, Username = "Room - UNITY AND LUCIA", StageId = 3 }
                );

            modelBuilder.Entity<Queue_Status>().HasData(
                 new Queue_Status { StatusId = 1, StatusName = "waiting" },
                 new Queue_Status { StatusId = 2, StatusName = "temp_reject" },
                 new Queue_Status { StatusId = 3, StatusName = "rejected" }
                 );


            modelBuilder.Entity<Stage>().HasData(
               new Stage { StageId = 1, StageName = "Pre-Screening - (Ground Floor)" },
               new Stage { StageId = 2, StageName = "Initial Interviewing - (Third Floor)" },
               new Stage { StageId = 3, StageName = "Final Interviewing - (Second Floor)" }
               );


            // For Junction
            modelBuilder.Entity<Serving>()
               .HasKey(uc => new { uc.TableId, uc.ApplicantId, uc.StageId });

            modelBuilder.Entity<Serving>()
                .HasOne(s => s.Table)
                .WithMany()
                .HasForeignKey(s => s.TableId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Serving>()
                .HasOne(s => s.Applicant)
                .WithMany()
                .HasForeignKey(s => s.ApplicantId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Serving>()
                .HasOne(s => s.Stage)
                .WithMany()
                .HasForeignKey(s => s.StageId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Queue_Stage_1>()
                 .HasKey(uc => new { uc.ApplicantId, uc.StatusId });
          
            modelBuilder.Entity<Queue_Stage_2>()
                 .HasKey(uc => new { uc.ApplicantId, uc.StatusId });
           
            modelBuilder.Entity<Queue_Stage_3>()
                 .HasKey(uc => new { uc.ApplicantId, uc.StatusId });
          
            modelBuilder.Entity<Applicant_Status>()
               .HasKey(uc => new { uc.ApplicantId});

            modelBuilder.Entity<Table_Serve>()
                .HasKey(uc => new { uc.TableId });

            // Default Date
            modelBuilder.Entity<Table_Serve>()
                .Property(u => u.Served_At)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Applicant_Form>()
                .Property(u => u.Created_At)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Queue_Stage_1>()
                .Property(u => u.Generated_At)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Queue_Stage_2>()
                .Property(u => u.Generated_At)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Queue_Stage_3>()
                .Property(u => u.Generated_At)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Serving>()
             .Property(u => u.Served_At)
             .HasDefaultValueSql("GETDATE()");

        }


    }
}
