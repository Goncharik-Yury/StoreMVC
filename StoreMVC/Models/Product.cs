using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StoreMVC.Models
{
	public class Product
	{
		public int ProductId { get; set; }

		[Display(Name = "Product name")]
		[Required(ErrorMessage = "Enter the name of product")]
		[StringLength(100, ErrorMessage = "Name should not be longer than 100 simbols")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Chose product category")]
		//[Display(Name = "Category")]
		public String Category { get; set; }

		[Required(ErrorMessage = "Enter the price")]
		[Range(0, int.MaxValue, ErrorMessage = "Incorrect price")]
		//79228162514264337593543950335
		public decimal Price { get; set; }

		[Required(ErrorMessage = "Enter number of products")]
		[Range(0, int.MaxValue, ErrorMessage = "Incorrect count")]
		public int Count { get; set; }

		[Display(Name = "Product image")]
		[UIHint("ImageTemplate")]
		public string imgName { get; set; }

		[UIHint("MultilineText")]
		public string Description { get; set; }
	}

	//public class Category
	//{
	//	[Key]
	//	[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
	//	public int CategoryId { get; set; }
	//	[Required(ErrorMessage = "Это поле обязательно для заполнения")]
	//	//[StringLength(20, ErrorMessage = "Имя категории не должно превышать 20 символов")]
	//	//[UIHint("CategoryTemplate")]
	//	//[Display(Name = "")]
	//	public string CategoryName { get; set; }
	//}
}