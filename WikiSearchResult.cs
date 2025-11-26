using System.Text.RegularExpressions;

public class WikiSearchResult
{
    public Query query { get; set; }

    /// <summary>
    /// Проверяет, есть ли результаты поиска
    /// </summary>
    public bool HasResults => query?.search != null && query.search.Length > 0;

    /// <summary>
    /// Возвращает количество найденных статей
    /// </summary>
    public int ResultsCount => query?.search?.Length ?? 0;

    /// <summary>
    /// Возвращает статью по индексу
    /// </summary>
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
    public string snippet { get; set; }

    /// <summary>
    /// Возвращает URL статьи в Википедии
    /// </summary>
    public string GetArticleUrl() => $"https://ru.wikipedia.org/w/index.php?curid={pageid}";

    /// <summary>
    /// Возвращает очищенный от HTML-тегов текст сниппета
    /// </summary>
    public string GetCleanSnippet() => Regex.Replace(snippet ?? "", "<.*?>", string.Empty);

    /// <summary>
    /// Возвращает форматированную информацию о статье
    /// </summary>
    public string GetFormattedInfo(int index) => $"{index}. {title} (ID: {pageid})";
}