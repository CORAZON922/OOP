using System;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            string searchQuery = GetSearchQueryFromUser();
            await ExecuteSearchAndDisplayResults(searchQuery);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }

        Console.WriteLine("Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }

    private static string GetSearchQueryFromUser()
    {
        Console.Write("Введите ваш поисковый запрос: ");
        string searchQuery = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(searchQuery))
        {
            throw new ArgumentException("Запрос не может быть пустым.");
        }

        return searchQuery;
    }

    private static async Task ExecuteSearchAndDisplayResults(string searchQuery)
    {
        using (var searchService = new WikiSearchService())
        {
            var searchResult = await searchService.SearchAsync(searchQuery);

            if (!searchResult.HasResults)
            {
                Console.WriteLine("По вашему запросу ничего не найдено.");
                return;
            }

            DisplaySearchResults(searchResult);
            await ProcessUserSelection(searchResult);
        }
    }

    private static void DisplaySearchResults(WikiSearchResult searchResult)
    {
        Console.WriteLine("\nРезультаты поиска:");
        Console.WriteLine("==================");

        for (int i = 0; i < searchResult.ResultsCount; i++)
        {
            var article = searchResult.GetArticle(i);
           

            Console.WriteLine(article.GetFormattedInfo(i + 1));
            Console.WriteLine();
        }
    }

    private static async Task ProcessUserSelection(WikiSearchResult searchResult)
    {
        Console.Write("Введите номер статьи для открытия в браузере (или 0 для выхода): ");
        string userInput = Console.ReadLine();

        if (int.TryParse(userInput, out int choice) && choice > 0 && choice <= searchResult.ResultsCount)
        {
            await OpenArticleInBrowser(searchResult.GetArticle(choice - 1));
        }
        else if (choice != 0)
        {
            Console.WriteLine("Некорректный ввод. Выход из программы.");
        }
    }

    private static async Task OpenArticleInBrowser(SearchItem article)
    {
        string articleUrl = article.GetArticleUrl();
        Console.WriteLine($"Открываю: {articleUrl}");

        try
        {
            Process.Start(new ProcessStartInfo { FileName = articleUrl, UseShellExecute = true });
            Console.WriteLine("Браузер запущен!");
            await Task.Delay(1000); 
        }
        catch (Exception e)
        {
            Console.WriteLine($"Не удалось открыть браузер: {e.Message}");
        }
    }
}
