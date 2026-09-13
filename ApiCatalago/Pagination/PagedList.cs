namespace ApiCatalago.Pagination;

public class PagedList<T> : List<T> where T : class
{
     public int CurrentPage { get; private set; }
     public int TotalPages { get; private set; }
     public int PageSize { get; private set; }
     public int TotalCount { get; private set; }

     public bool HasPrevius => CurrentPage > 1;
     public bool HasNext => CurrentPage < TotalPages;

     public PagedList(List<T> items, int totalCount, int pageNumber, int pageSize)
     {
          TotalCount = totalCount;
          PageSize = pageSize;
          CurrentPage = pageNumber;
          TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
          
          AddRange(items);
     }

     // IQueryable para consulta de formas eficientes (consultas diferidas)
     // IQueryable mais eficinte do que IEnumerable
     public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
     {
         var count = source.Count(); // source = fonte de dados
         var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

         return new PagedList<T>(items, count, pageNumber, pageSize);
     }
     
}