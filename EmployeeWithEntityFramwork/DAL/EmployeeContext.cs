namespace EmployeeWithEntityFramwork
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EmployeeContext : DbContext
    {
        public EmployeeContext()
            : base("name=EmployeeContext")
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Employee_Table> Employee_Table { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .Property(e => e.Id)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Department)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Image)
                .IsUnicode(false);

            //modelBuilder.Entity<Employee_Table>()
            //    .Property(e => e.Id)
            //    .HasPrecision(18, 0);

            //modelBuilder.Entity<Employee_Table>()
            //    .Property(e => e.Name)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Employee_Table>()
            //    .Property(e => e.Department)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Employee_Table>()
            //    .Property(e => e.Image)
            //    .IsUnicode(false);
        }
    }
}
