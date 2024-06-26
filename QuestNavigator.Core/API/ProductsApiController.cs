using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Controllers;

namespace QuestNavigator.Core.API;

public class ProductsApiController : UmbracoApiController
{
	//~/Umbraco/Api/Products/GetAllProducts
	public IEnumerable<string> GetAllProducts()
	{
		return new[] { "Table", "Chair", "Desk", "Computer" };
	}
}

public class ProductsAuthorizedApiController : UmbracoAuthorizedApiController
{
	//~/Umbraco/backoffice/Api/Products/GetAllProducts
	public IEnumerable<string> GetAllProducts()
	{
		return new[] { "Table", "Chair", "Desk", "Computer" };
	}
}