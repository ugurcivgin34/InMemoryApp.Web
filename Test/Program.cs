using System.Text.Json;
using System.Text.Json.Serialization;

class Program
{
    static async Task Main(string[] args)
    {
        string url = "https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&ids=bitcoin,ethereum,binancecoin,dogecoin";
        var client = new HttpClient();

        // User-Agent başlığını ekle
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");

        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var coinData = JsonSerializer.Deserialize<CoinData[]>(jsonString);

            foreach (var coin in coinData)
            {
                Console.WriteLine($"ID: {coin.Id}, Symbol: {coin.Symbol}, Name: {coin.Name}, Current Price: {coin.CurrentPrice}");
            }
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }
    }
}

public class CoinData
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("current_price")]
    public decimal CurrentPrice { get; set; }
}

