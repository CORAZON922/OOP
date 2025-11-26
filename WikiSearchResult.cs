using System.Text.RegularExpressions;

public class WikiSearchResult
{
    public Query query { get; set; }

    public bool HasResults => query?.search != null && query.search.Length > 0;


    public int ResultsCount => query?.search?.Length ?? 0;

   
    public SearchItem GetArticle(int index)
    {
        if (query?.search == null || index < 0 || index >= query.search.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        return query.search[index];
    }
}

public class Query
{
    public SearchItem[] search { get; set; }
}

public class SearchItem
{
    public string title { get; set; }
    public int pageid { get; set; }


   
    public string GetArticleUrl() => $"https://ru.wikipedia.org/w/index.php?curid={pageid}";

    public string GetFormattedInfo(int index) => $"{index}. {title} (ID: {pageid})";
}
