using System.Net.Http.Headers;

HttpClient httpClient = new()
{
    BaseAddress = new Uri("https://localhost:7213")
};

httpClient.DefaultRequestHeaders.Accept.Clear();
httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

// TODO: Write some client methods according to API endpoint and print its response to the console
// ...