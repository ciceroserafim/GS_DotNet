namespace Aprenda.Infrastructure.DependencyInjection
{
	public class PaginatedResponse<T>
	{
		public IEnumerable<T> Items { get; set; }
		public int Page { get; set; }
		public int PageSize { get; set; }
		public int TotalItems { get; set; }
		public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

		public PaginatedResponse(IEnumerable<T> items, int page, int pageSize, int totalItems)
		{
			Items = items;
			Page = page;
			PageSize = pageSize;
			TotalItems = totalItems;
		}
	}
}
