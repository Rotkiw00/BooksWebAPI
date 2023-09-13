using BooksClient.Enums;
using BooksClient.ModelsDto;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BooksClient
{
    public class Application
    {
        private readonly HttpClient _httpClient;
        private HttpMethodType _methodType;

        public Application()
        {
            _httpClient = new()
            {
                BaseAddress = new Uri("https://localhost:7213")
            };

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        //TODO: Complete below method and the rest of http client method

        public async Task RunClientAsync()
        {
            PrintAvaiableOptions();
            int httpMethod = int.Parse(Console.ReadLine());

            switch ((HttpMethodType)httpMethod)
            {
                case HttpMethodType.GET:
                    await GetAllBooksAsync();
                    break;
                case HttpMethodType.POST:
                    await CreateBook();
                    break;
                case HttpMethodType.PUT:
                    break;
                case HttpMethodType.DELETE:
                    break;
                default:
                    break;
            }
        }

        private static void PrintAvaiableOptions()
        {
            HttpMethodType[] httpMethods = (HttpMethodType[])Enum.GetValues(typeof(HttpMethodType));
            Console.WriteLine("Avaiable HTTP Client methods:");
            for (int i = 0; i < httpMethods.Length; i++)
            {
                Console.WriteLine($"{i + 1} - {httpMethods[i]}");
            }
        }

        private async Task GetAllBooksAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/Books");
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<IEnumerable<BooksDto>>() ?? Enumerable.Empty<BooksDto>();

                if (!content.Any())
                {
                    await Console.Out.WriteLineAsync("There is no books in database..");
                }
                else
                {
                    foreach (var item in content)
                    {
                        await Console.Out.WriteLineAsync(item.ToString());
                    }
                }
            }
            else
            {
                await Console.Out.WriteLineAsync(response.StatusCode.ToString());
            }
        }

        private async Task CreateBook()
        {
            // TODO: Fix it
            var book = HandleBookCreation();
            var json = JsonSerializer.Serialize(book);

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Books", json);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode) { await Console.Out.WriteLineAsync("Successfully added new book."); }
            else { await Console.Out.WriteLineAsync(response.StatusCode.ToString()); }
        }

        private static BooksDto HandleBookCreation()
        {
            Console.WriteLine("New book adding...");
            BooksDto book = new();

            foreach (var prop in book.GetType().GetProperties())
            {
                if (prop.Name is "Id") { continue; }

                if (prop.Name is "PublishDate")
                {
                    prop.SetValue(book, DateTime.Now);
                    continue;
                }

                Console.WriteLine($"{prop.Name}: ");
                prop.SetValue(book, Console.ReadLine());                
            }
            return book;
        }        
    }
}
