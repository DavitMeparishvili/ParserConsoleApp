using Microsoft.EntityFrameworkCore;
using Parser;

namespace ExchangeRateExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var result = await GetExchangerates("2018");

            using (var db = new CurrencyExchangeContext())
            {
                await db.ExchangeRates.AddRangeAsync(result);
                await db.SaveChangesAsync();
            }
        }

        static async Task<List<ExchangeRate>> GetExchangerates(string year)
        {
            var client = new HttpClient();
            var response = await client.GetAsync($"https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/year.txt?year={year}");
            var content = await response.Content.ReadAsStringAsync();

            // Split the content into lines
            var lines = content.Split('\n');

            // Initialize a list to hold the exchange rates
            var exchangeRates = new List<ExchangeRate>();

            // Initialize a list to hold exchange rate names
            List<string> currensynames = null;

            for (int i = 0; i < lines.Length; i++)
            {
                var fields = lines[i].Split('|');

                if (fields[0] == String.Empty)
                {
                    continue;
                }

                if (i == 0)
                {
                    //First line of parsed string contains currency names,so store temporary for later use
                    currensynames = new List<string>(fields);
                    continue;
                }

                var date = DateTime.ParseExact(fields[0], "dd.MM.yyyy", null);

                for (int a = 0; a < fields.Length; a++)
                {
                    if (a == 0)
                    {
                        continue;
                    }

                    exchangeRates.Add(new ExchangeRate()
                    {
                        Date = date,
                        Currency = currensynames[a],
                        Rate = decimal.Parse(fields[a])
                    });
                }
            }

            return exchangeRates;
        }
    }
}
