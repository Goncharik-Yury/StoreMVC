using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreMVC.Models
{
	public class Order
	{
		public int OrderId { get; set; }
		public int UserId { get; set; }
		public int ProductId { get; set; }

		[Required(ErrorMessage = "Enter number of products")]
		[Range(0, int.MaxValue, ErrorMessage = "Incorrect count")]
		public int Count { get; set; }

		[DataType(DataType.Date)]
		public DateTime Date { get; set; }
		public Product Product { get; set; }
		public UserProfile UserProfile { get; set; }

	}
}