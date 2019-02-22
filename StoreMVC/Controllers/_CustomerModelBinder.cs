using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using StoreMVC.Models;

namespace StoreMVC.Controllers
{
	public class CustomerModelBinder : IModelBinder
	{
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			Customer model = (Customer)bindingContext.Model ?? new Customer(); // если != null, то первое, иначе второе

			bool hasPrefix = bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName);
			string prefix = hasPrefix ? bindingContext.ModelName + "." : "";

			//string valueEmail = GetValue(bindingContext, prefix, "Email");
			//model.Email = ifValidEmail(valueEmail) ? valueEmail : null;
			model.Email = GetValue(bindingContext, prefix, "Email");
			model.Name = GetValue(bindingContext, prefix, "Name");

			return model;
		}

		private string GetValue(ModelBindingContext bindingContext, string prefix, string key)
		{
			ValueProviderResult vpr = bindingContext.ValueProvider.GetValue(prefix + key);

			return vpr == null ? null : vpr.AttemptedValue;
			//return ifValidEmail(vpr.AttemptedValue) ? vpr.AttemptedValue : null;
		}
	}
}