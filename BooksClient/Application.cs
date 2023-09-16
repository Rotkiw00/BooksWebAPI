using BooksClient.Enums;
using BooksClient.ModelsDto;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BooksClient
{
    public class Application
    {
        private readonly HttpClient _httpClient;

        public Application()
        {
            _httpClient = new() { BaseAddress = new Uri("https://localhost:7213") };

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

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
                    await PostBookAsync();
                    break;
                case HttpMethodType.PUT:
                    await UpdateBookAsync();
                    break;
                case HttpMethodType.DELETE:
                    await DeleteBookAsync();
                    break;
                default:
                    await Console.Out.WriteLineAsync("Wrong option selected. Try again");
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

        #region Client GET        
        private async Task GetAllBooksAsync()
        {
            HttpResponseMessage getResponse = await _httpClient.GetAsync("api/Books");
            getResponse.EnsureSuccessStatusCode();

            if (getResponse.IsSuccessStatusCode)
            {
                var content = await getResponse.Content.ReadFromJsonAsync<IEnumerable<BooksDto>>() ?? Enumerable.Empty<BooksDto>();

                if (!content.Any())
                    await Console.Out.WriteLineAsync("There is no books in database..");

                else
                    foreach (var item in content)
                        await Console.Out.WriteLineAsync(item.ToString());
            }
            else
                await Console.Out.WriteLineAsync(getResponse.StatusCode.ToString());

        }
        #endregion

        #region Client POST        
        private async Task PostBookAsync()
        {
            var book = CreateBook();

            HttpResponseMessage postResponse = await _httpClient.PostAsJsonAsync("api/Books", book);
            postResponse.EnsureSuccessStatusCode();
            if (postResponse.IsSuccessStatusCode) { Console.Clear(); await Console.Out.WriteLineAsync("Successfully added new book."); }
            else await Console.Out.WriteLineAsync(postResponse.StatusCode.ToString());
        }

        private static BooksDto CreateBook()
        {
            Console.WriteLine("New book adding...");
            BooksDto book = new();

            foreach (var prop in book.GetType().GetProperties())
            {
                if (prop.Name is "Id") continue;

                if (prop.Name is "PublishDate")
                {
                    prop.SetValue(book, DateTime.Now.ToUniversalTime());
                    continue;
                }

                Console.WriteLine($"{prop.Name}: ");
                prop.SetValue(book, Console.ReadLine());
            }
            return book;
        }
        #endregion

        #region Client PUT        
        private async Task UpdateBookAsync()
        {
            Console.WriteLine("Enter ID of Book that you want to edit:");
            int id = int.Parse(Console.ReadLine());

            var bookToEdit = await GetBookToUpdate(id);
            EditBook(bookToEdit);

            HttpResponseMessage putResponse = await _httpClient.PutAsJsonAsync($"api/Books/{id}", bookToEdit);
            putResponse.EnsureSuccessStatusCode();
            if(putResponse.IsSuccessStatusCode) { Console.Clear(); await Console.Out.WriteLineAsync("Book successfully updated."); }
        }

        private async Task<BooksDto> GetBookToUpdate(int id)
        {
            HttpResponseMessage getByIdResponse = await _httpClient.GetAsync($"api/Books/{id}");
            var book = await getByIdResponse.Content.ReadAsAsync<BooksDto>();
            return book;
        }

        private static void EditBook(BooksDto bookToEdit)
        {
            Console.WriteLine("Title:");
            bookToEdit.Title = Console.ReadLine();
            Console.WriteLine("Author:");
            bookToEdit.Author = Console.ReadLine();
            Console.WriteLine("Description");
            bookToEdit.Description = Console.ReadLine();
            Console.WriteLine("Genre:");
            bookToEdit.Genre = Console.ReadLine();
        }
        #endregion

        #region Client DELETE
        private async Task DeleteBookAsync()
        {
            Console.WriteLine("Enter ID of Book that you want to delete:");
            int id = int.Parse(Console.ReadLine());

            HttpResponseMessage deleteResponse = await _httpClient.DeleteAsync($"api/Books/{id}");
            deleteResponse.EnsureSuccessStatusCode();
            if(deleteResponse.IsSuccessStatusCode) { Console.Clear(); await Console.Out.WriteLineAsync("Book succesfully deleted."); }
        }
        #endregion
    }
}
