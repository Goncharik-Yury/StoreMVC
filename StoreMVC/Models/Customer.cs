using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StoreMVC.Models
{
	public class Customer
	{
		public int CustomerId { get; set; }

		[Display(Name = "Customer name")]
		[Index(IsUnique = true)]
		[Required(ErrorMessage = "Вы не ввели имя")]
		[StringLength(20, ErrorMessage = "Имя не может привышать 20 символов")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Поле Email обязательно для заполнения")]
		[RegularExpression(@"(?i)\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b", ErrorMessage = "Email адрес указан не правильно")]
		public string Email { get; set; }

		public List<Order> Orders { get; set; }
	}
}