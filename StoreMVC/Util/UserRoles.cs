//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace StoreMVC
//{
//	public static class ProductsCategories
//	{
//		//string[] categories = new string[] { "","mobile", "notebook", "tabletpc", "console", "video", "tv", "other" };
//		private static Dictionary<string, string> categoriesDictionary = new Dictionary<string, string>() {
//			{"all", "All"},
//			{"mobile", "Mobile phone"},
//			{"notebook", "Notebook" },
//			{"desktoppc", "Desktop PC"},
//			{"tabletpc", "Tablet PC"},
//			{"console", "Game console"},
//			{"video", "Video camera"},
//			{"tv", "TV"},
//			{"other", "Other"}
//		};

//		public static Dictionary<string, string> CategoriesDictionary { get => categoriesDictionary; }

//		public static List<SelectListItem> CategoriesSelectList
//		{
//			get
//			{
//				return categoriesDictionary.Select((name, index) =>
//				 {
//					 return new SelectListItem
//					 {
//						 Value = name.Key,
//						 Text = name.Value
//					 };
//				 }).ToList();
//			}
//		}
//	}
//}