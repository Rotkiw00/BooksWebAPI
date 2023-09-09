using BooksClient.ModelsDto;
using System.Net.Http.Headers;
using System.Net.Http.Json;

HttpClient httpClient = new()
{
    BaseAddress = new Uri("https://localhost:7213")
};

httpClient.DefaultRequestHeaders.Accept.Clear();
httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

// TODO: Write some client methods according to API endpoint and print its response to the console
// ...

HttpResponseMessage response = await httpClient.GetAsync("api/Books");
response.EnsureSuccessStatusCode();

if (response.IsSuccessStatusCode)
{
    var content = await response.Content.ReadFromJsonAsync<IEnumerable<BooksDto>>();
    foreach (var item in content)
    {
        Console.WriteLine(item);
    }
}

Console.ReadLine();