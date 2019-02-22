using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace StoreMVC.Models
{
	public class UserProfile
	{
		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int UserId { get; set; }

		[Display(Name = "Login")]
		[Index(IsUnique = true)]
		[Required(ErrorMessage = "Enter your login")]
		[StringLength(20, ErrorMessage = "Login can't be more than 20 simbols")]
		public string UserName { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Patronymic { get; set; }

		//[Required(ErrorMessage = "Enter your email")]
		[RegularExpression(@"(?i)\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b", ErrorMessage = "Wrong email address")]
		public string Email { get; set; }
	}

	[NotMapped]
	public class UserProfileFull : UserProfile
	{
		public string[] Roles { get; set; }
		public UserProfileFull(UserProfile userProfile) : base()
		{
			UserId = userProfile.UserId;
			UserName = userProfile.UserName;
			FirstName = userProfile.FirstName;
			LastName = userProfile.LastName;
			Patronymic = userProfile.Patronymic;
			Email = userProfile.Email;
		}

		//public List<SelectListItem> RolesSelectList
		//{
		//	get
		//	{
		//		return Roles.Select((name, index) =>
		//		{
		//			return new SelectListItem
		//			{

		//				Value = name,
		//				Text = name
		//			};
		//		}).ToList();
		//	}
		//}
	}

	public class LocalPasswordModel
	{
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string OldPassword { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "New password")]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

	public class LoginModel
	{
		[Required]
		[Display(Name = "User name")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }
	}

	public class RegisterModel
	{
		[Required]
		[Display(Name = "User name")]
		public string UserName { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Patronymic { get; set; }

		[RegularExpression(@"(?i)\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b", ErrorMessage = "Wrong email address")]
		public string Email { get; set; }
		[Required]
		[Display(Name = "Enter symbols from the picture")]
		public string Captcha { get; set; }
	}
}
