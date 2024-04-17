namespace Restaurants.Application.Common;

public class PageResult<T>
{
    public PageResult(IEnumerable<T> items, int totalItemCount, int pageSize, int pageNumber)
    {
        Items= items;
        TotalItemCount = totalItemCount;
        TotalPages = (int)Math.Ceiling(totalItemCount / (double)pageSize);
        ItemsFrom = pageSize * (pageNumber - 1) + 1;
        ItemsTo = ItemsFrom + items.Count() - 1;
    }
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int TotalPages { get; set; }
    public int TotalItemCount { get; set; }
    public int ItemsFrom { get; set; }
    public int ItemsTo { get; set; }

}
