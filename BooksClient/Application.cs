using BooksClient.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

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
                    break;
                case HttpMethodType.PUT:
                    break;
                case HttpMethodType.DELETE:
                    break;
                default:
                    break;
            }            
        }

        private async Task GetAllBooksAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/Books");
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<IEnumerable<BooksDto>>();
                foreach (var item in content)
                {
                    Console.WriteLine(item);
                }
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
    }

    public enum HttpMethodType
    {
        GET = 1,
        POST = 2,
        PUT = 3,
        DELETE = 4,
    }
}
