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
		public DbSet<School> School { get; set; }
		public DbSet<Gender> Gender { get; set; }
		public DbSet<Blood_Group> Blood_Group { get; set; }
		public DbSet<City> City { get; set; }
		public DbSet<State> State { get; set; }
		public DbSet<Country> Country { get; set; }
		public DbSet<Employee> Employee { get; set; }
		public DbSet<Student_Prev_School_Details> Student_Prev_School_Detail { get; set; }
		public DbSet<Student_Sibling_Detail> Student_Sibling_Detail { get; set; }
		public DbSet<Student_Other_Details> Student_Other_Details { get; set; }
		public DbSet<Category> Category { get; set; }
		public DbSet<Staff_Type> Staff_Type { get; set; }
		public DbSet<Staff> Staff { get; set; }
		public DbSet<Staff_Educational_Details> Staff_Educational_Details { get; set; }
		public DbSet<Staff_Exp_Details> Staff_Exp_Details { get; set; }
		public DbSet<Staff_Salary_Detail> Staff_Salary_Detail { get; set; }
		public DbSet<Institution> Institution { get; set; }
		public DbSet<Qualification> Qualification { get; set; }
		public DbSet<Specialization> Specialization { get; set; }
		public DbSet<Occupation> Occupation { get; set; }
		public DbSet<Fee_Payment> Fee_Payment { get; set; }
		public DbSet<Frequency> Frequency { get; set; }
		public DbSet<Designation> Designation { get; set; }
		public DbSet<Staff_Attendance> Staff_Attendance { get; set; }
		public DbSet<Assignment> Assignment { get; set; }
		public DbSet<Term_Fee_Date> Term_Fee_Date { get; set; }


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
			modelBuilder.Entity<School>().HasKey(s => s.Id);
			modelBuilder.Entity<Student_Prev_School_Details>().HasKey(s => s.Student_PrevSchool_Id);
			modelBuilder.Entity<Student_Sibling_Detail>().HasKey(s => s.Sibling_Detail_Id);
			modelBuilder.Entity<Student_Other_Details>().HasKey(s => s.StudentDetail_Id);
			modelBuilder.Entity<Category>().HasKey(s => s.Id);
			modelBuilder.Entity<Staff_Type>().HasKey(s => s.Id);
			modelBuilder.Entity<Staff>().HasKey(s => s.Staff_Id);
			modelBuilder.Entity<Staff_Educational_Details>().HasKey(s => s.Id);
			modelBuilder.Entity<Staff_Exp_Details>().HasKey(s => s.Id);
			modelBuilder.Entity<Staff_Salary_Detail>().HasKey(s => s.Id);
			modelBuilder.Entity<Institution>().HasKey(s => s.Id);
			modelBuilder.Entity<Qualification>().HasKey(s => s.Id);
			modelBuilder.Entity<Occupation>().HasKey(s => s.Id);
			modelBuilder.Entity<Fee_Payment>().HasKey(s => s.Id);
			modelBuilder.Entity<Frequency>().HasKey(s => s.Id);
			modelBuilder.Entity<Designation>().HasKey(s => s.Id);
			modelBuilder.Entity<Staff_Attendance>().HasKey(s => s.Id);
			modelBuilder.Entity<Assignment>().HasKey(s => s.Id);
			modelBuilder.Entity<Term_Fee_Date>().HasKey(s => s.Id);

			base.OnModelCreating(modelBuilder);
		}
	}


}