namespace Ecommerce.Models.Specifications
{
    public class PagedList<T> : List<T>
    {
        public MetaData MetaData { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize), //for example 1.5 round 2
            };
            AddRange(items);   //Aregate items end list
        }

        public static PagedList<T> ToPagedList(IEnumerable<T> entity, int pageNumber, int pageSize)
        {
            var count = entity.Count();   
            var items = entity.Skip((pageNumber-1)*pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
