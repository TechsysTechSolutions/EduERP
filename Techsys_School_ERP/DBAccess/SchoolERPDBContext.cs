using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Techsys_School_ERP.Model;



namespace Techsys_School_ERP.DBAccess
{
	public class SchoolERPDBContext : DbContext
	{
		public SchoolERPDBContext() : base("SchoolERPDbContext")
        {
		}

		public DbSet<User> Users { get; set; }
		public DbSet<User_Role> User_Roles { get; set; }
		public DbSet<Exam> Exam { get; set; }
		public DbSet<Fee> Fee { get; set; }
		public DbSet<Class> Class { get; set; }
		public DbSet<Section> Section { get; set; }
        public DbSet<Fee_Configuration> Fee_Configuration { get; set; }
		public DbSet<Holiday> Holiday { get; set; }
		public DbSet<Term> Term { get; set; }
		public DbSet<Student> Student { get; set; }
		public DbSet<Attendance> Attendance { get; set; }
		public DbSet<Subject> Subject { get; set; }
		public DbSet<Subject_Class_Detail> Subject_Class_Detail { get; set; }
		public DbSet<Mark> Mark { get; set; }
		public DbSet<Image> Image { get; set; }

		public DbSet<Gender> Gender { get; set; }
		public DbSet<Blood_Group> Blood_Group { get; set; }
		public DbSet<City> City { get; set; }
		public DbSet<State> State { get; set; }
		public DbSet<Country> Country { get; set; }
		public DbSet<Employee> Employee { get; set; }


		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			modelBuilder.Entity<User>().HasKey(s => s.Id);
			modelBuilder.Entity<User_Role>().HasKey(s => s.Id);
			modelBuilder.Entity<Exam>().HasKey(s => s.Id);
			modelBuilder.Entity<Fee>().HasKey(s => s.Id);
			modelBuilder.Entity<Class>().HasKey(s => s.Id);
			modelBuilder.Entity<Section>().HasKey(s => s.Id);
			modelBuilder.Entity<Fee_Configuration>().HasKey(s => s.Id);
			modelBuilder.Entity<Holiday>().HasKey(s => s.Id);
			modelBuilder.Entity<Term>().HasKey(s => s.Id);
			modelBuilder.Entity<Attendance>().HasKey(s => s.Id);
			modelBuilder.Entity<Subject>().HasKey(s => s.Id);
			modelBuilder.Entity<Subject_Class_Detail>().HasKey(s => s.Id);
			modelBuilder.Entity<Mark>().HasKey(s => s.Id);
			modelBuilder.Entity<Image>().HasKey(s => s.Id);
			modelBuilder.Entity<Employee>().HasKey(s => s.EmployeeID);

			//modelBuilder.Entity<User>().ToTable("User");
			//modelBuilder.Entity<Billing>().ToTable("Billing");
			//modelBuilder.Entity<ProductViewModel>().ToTable("Product");
			//modelBuilder.Entity<CustomerViewModel>().ToTable("Customer");
			//modelBuilder.Entity<User_Registration>().ToTable("UserRegistration");
			//modelBuilder.Entity<Billing_Cancelled>().ToTable("Billing_Cancelled");
			//modelBuilder.Entity<Unit>().ToTable("Unit");
			//modelBuilder.Entity<GSM>().ToTable("GSM");
			//modelBuilder.Entity<Status>().ToTable("Status");
			//modelBuilder.Entity<PaymentType>().ToTable("PaymentType");
			//modelBuilder.Entity<PaymentDetail>().ToTable("PaymentDetail");
			//modelBuilder.Entity<CreditNoteDetail>().ToTable("CreditNote_Detail");
			//modelBuilder.Entity<OpeningBalance>().ToTable("OpeningBalance");

			base.OnModelCreating(modelBuilder);
		}
	}


}