using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Aprenda.Api.Utils
{
	public static class HateoasHelper
	{
		// Gera links simples HATEOAS (self, update, delete) para um recurso.
		public static IDictionary<string, string> GenerateLinks(ControllerBase controller, string routeName, object routeValues)
		{
			var links = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

			try
			{
				var self = controller.Url.Action(null, routeName, routeValues);
				links["self"] = self ?? string.Empty;

				// update & delete often point to same action with different verbs (PUT/DELETE) but same URL
				links["update"] = self ?? string.Empty;
				links["delete"] = self ?? string.Empty;
			}
			catch
			{
				// fallback — não lançar exceção no helper
			}

			return links;
		}
	}
}
