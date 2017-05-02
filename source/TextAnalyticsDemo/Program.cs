using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TextAnalyticsDemo
{
    class Program
    {
        private static string url = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/languages";
        private static string apiKey = "";

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Input text to identify:");
                var text = Console.ReadLine();
                GetLanguages(text).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        private static async Task GetLanguages(string text)
        {
            var json = "{ 'documents': [ { 'id': '001', 'text': '" + text + "' } ] }";

            byte[] reqData = Encoding.UTF8.GetBytes(json);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.ContentLength = reqData.Length;
            request.Accept = "application/json";
            request.Method = "POST";

            request.Headers.Add("Ocp-Apim-Subscription-Key", apiKey);

            var reqStream = await request.GetRequestStreamAsync();
            reqStream.Write(reqData, 0, reqData.Length);
            reqStream.Close();

            var response = await request.GetResponseAsync();
            var responseStream = response.GetResponseStream();
            var reader = new StreamReader(responseStream);
            var responseData = reader.ReadToEnd();
            reader.Close();
            response.Close();

            Console.WriteLine(responseData);
        }
    }
}
