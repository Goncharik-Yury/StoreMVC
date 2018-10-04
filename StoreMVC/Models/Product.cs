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
		[StringLength(100, ErrorMessage = "Имя не должно превышать 100 символов")]
		public string Name { get; set; }

		[UIHint("MultilineText")]
		public string Description { get; set; }

		[Required(ErrorMessage = "Это поле обязательно для заполнения")]
		[Range(0, int.MaxValue, ErrorMessage = "Цена указана не верно")]
		//79228162514264337593543950335
		public decimal Price { get; set; }

		[Required(ErrorMessage = "Выберите категорию продукта")]
		//[Display(Name = "Category")]
		public String Category { get; set; }

		[Display(Name = "Product image")]
		[UIHint("ImageTemplate")]
		public string imgName { get; set; }
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