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
		[Required(ErrorMessage = "Это поле обязательно для заполнения")]
		[StringLength(40, ErrorMessage = "Имя не должно превышать 40 символов")]
		public string Name { get; set; }

		[UIHint("MultilineText")]
		public string Description { get; set; }

		[Required(ErrorMessage = "Это поле обязательно для заполнения")]
		[Range(0, int.MaxValue, ErrorMessage = "Цена указана не верно")]
		//79228162514264337593543950335
		public decimal Price { get; set; }

		[Display(Name = "Category")]
		public string Category { get; set; }

		[Display(Name = "Product image")]
		[UIHint("ImageTemplate")]
		public string imgName { get; set; }
	}

	//public class Category
	//{
	//	[Key]
	//	[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
	//	[Required(ErrorMessage = "Это поле обязательно для заполнения")]
	//	[StringLength(20, ErrorMessage = "Имя категории не должно превышать 20 символов")]
	//	public string CategoryName { get; set; }
	//}
}