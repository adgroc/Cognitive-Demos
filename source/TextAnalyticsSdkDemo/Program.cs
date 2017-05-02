using Microsoft.ProjectOxford.Text.Core;
using Microsoft.ProjectOxford.Text.KeyPhrase;
using Microsoft.ProjectOxford.Text.Language;
using Microsoft.ProjectOxford.Text.Sentiment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAnalyticsSdkDemo
{
    class Program
    {
        private static string apiKey = "";
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Getting languages");
                GetLanguages();
                Console.WriteLine();

                Console.WriteLine("Getting sentiment");
                GetSentiment();
                Console.WriteLine();

                Console.WriteLine("Getting key phrases");
                GetKeyPhrases();
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        private static void GetLanguages()
        {
            var client = new LanguageClient(apiKey);
            var request = new LanguageRequest();

            var document = new Document()
            {
                Id = "YOUR-UNIQUE-ID",
                Text = "YOUR-TEXT"
            };

            var response = client.GetLanguages(request);

            foreach (var doc in response.Documents)
            {
                Console.WriteLine("Document Id: {0}", doc.Id);

                foreach (var lang in doc.DetectedLanguages)
                {
                    Console.WriteLine("--Language: {0}({1})", lang.Name, lang.Iso639Name);
                    Console.WriteLine("--Confidence: {0}%", (lang.Score * 100));
                }
            }

            request.Documents.Add(document);
        }

        private static void GetSentiment()
        {
            var client = new SentimentClient(apiKey);
            var request = new SentimentRequest();

            var document = new SentimentDocument()
            {
                Id = "YOUR-UNIQUE-ID",
                Text = "YOUR-TEXT",
                Language = "en"
            };

            request.Documents.Add(document);

            var response = client.GetSentiment(request);

            foreach (var doc in response.Documents)
            {
                Console.WriteLine("   Document Id: {0}", doc.Id);
                Console.WriteLine("   Score: {0}%", (doc.Score * 100));
            }
        }

        private static void GetKeyPhrases()
        {
            var client = new KeyPhraseClient(apiKey);
            var request = new KeyPhraseRequest();

            var document = new KeyPhraseDocument()
            {
                Id = "YOUR-UNIQUE-ID",
                Text = "YOUR-TEXT",
                Language = "en"
            };

            request.Documents.Add(document);

            var response = client.GetKeyPhrases(request);

            foreach (var doc in response.Documents)
            {
                Console.WriteLine("Document Id: {0}", doc.Id);

                foreach (var keyPhrase in doc.KeyPhrases)
                {
                    Console.WriteLine("   Key Phrase: {0}", keyPhrase);
                }
                Console.WriteLine();
            }
        }
    }
}
