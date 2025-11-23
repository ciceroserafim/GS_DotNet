using System.Collections.Generic;

namespace SkillUp.Api.Utils
{
	public class PaginatedResponse<T>
	{
		public IEnumerable<T> Items { get; set; } = new List<T>();
		public int Page { get; set; }
		public int PageSize { get; set; }
		public int Total { get; set; }

		public PaginatedResponse() { }

		public PaginatedResponse(IEnumerable<T> items, int page, int pageSize, int total)
		{
			Items = items;
			Page = page;
			PageSize = pageSize;
			Total = total;
		}
	}
}
