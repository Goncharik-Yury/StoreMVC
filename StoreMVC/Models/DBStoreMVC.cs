namespace StoreMVC.Models
{
	using System;
	using System.Data.Entity;
	using System.Linq;

	public class DBStoreMVC : DbContext
	{
		public DBStoreMVC()
			: base("name=DBStoreMVC")
		{
		}

		// Add a DbSet for each entity type that you want to include in your model. For more information 
		// on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

		public DbSet<Product> Products { get; set; }

		public DbSet<Customer> Customers { get; set; }

		public DbSet<Order> Orders { get; set; }
	}
}