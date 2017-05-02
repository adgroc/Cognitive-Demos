using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.Diagnostics;

namespace VisionSdkDemo
{
    class Program
    {
        private static string apiKey = "";

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Analyze Image:");
                AnalyzeImage().Wait();
                Console.WriteLine();

                Console.WriteLine("Describing image:");
                DescribeImage().Wait();
                Console.WriteLine();

                Console.WriteLine("Run OCR:");
                RunOcr().Wait();
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

        private static async Task AnalyzeImage()
        {
            var imageUrl = "";

            VisionServiceClient VisionServiceClient = new VisionServiceClient(apiKey);

            VisualFeature[] visualFeatures = new VisualFeature[]
{
                VisualFeature.Adult, VisualFeature.Categories, VisualFeature.Color,
                VisualFeature.Description, VisualFeature.Faces, VisualFeature.ImageType, VisualFeature.Tags
};

            AnalysisResult analysisResult = await VisionServiceClient.AnalyzeImageAsync(imageUrl, visualFeatures);
            LogAnalysisResult(analysisResult);
        }

        private static async Task DescribeImage()
        {
            var imageUrl = "";

            var VisionServiceClient = new VisionServiceClient(apiKey);

            var analysisResult = await VisionServiceClient.DescribeAsync(imageUrl, 3);
            LogAnalysisResult(analysisResult);
        }

        private static async Task RunOcr()
        {
            var imageUrl = "";

            var VisionServiceClient = new VisionServiceClient(apiKey);

            var ocrResult = await VisionServiceClient.RecognizeTextAsync(imageUrl);
            LogOcrResults(ocrResult);
        }

        #region Helpers
        private static void Log(string message)
        {
            Console.WriteLine(message);
        }

        private static void LogAnalysisResult(AnalysisResult result)
        {
            if (result == null)
            {
                Log("null");
                return;
            }

            if (result.Metadata != null)
            {
                Log("Image Format : " + result.Metadata.Format);
                Log("Image Dimensions : " + result.Metadata.Width + " x " + result.Metadata.Height);
            }

            if (result.ImageType != null)
            {
                string clipArtType;
                switch (result.ImageType.ClipArtType)
                {
                    case 0:
                        clipArtType = "0 Non-clipart";
                        break;
                    case 1:
                        clipArtType = "1 ambiguous";
                        break;
                    case 2:
                        clipArtType = "2 normal-clipart";
                        break;
                    case 3:
                        clipArtType = "3 good-clipart";
                        break;
                    default:
                        clipArtType = "Unknown";
                        break;
                }
                Log("Clip Art Type : " + clipArtType);

                string lineDrawingType;
                switch (result.ImageType.LineDrawingType)
                {
                    case 0:
                        lineDrawingType = "0 Non-LineDrawing";
                        break;
                    case 1:
                        lineDrawingType = "1 LineDrawing";
                        break;
                    default:
                        lineDrawingType = "Unknown";
                        break;
                }
                Log("Line Drawing Type : " + lineDrawingType);
            }


            if (result.Adult != null)
            {
                Log("Is Adult Content : " + result.Adult.IsAdultContent);
                Log("Adult Score : " + result.Adult.AdultScore);
                Log("Is Racy Content : " + result.Adult.IsRacyContent);
                Log("Racy Score : " + result.Adult.RacyScore);
            }

            if (result.Categories != null && result.Categories.Length > 0)
            {
                Log("Categories : ");
                foreach (var category in result.Categories)
                {
                    Log("   Name : " + category.Name + "; Score : " + category.Score);
                }
            }

            if (result.Faces != null && result.Faces.Length > 0)
            {
                Log("Faces : ");
                foreach (var face in result.Faces)
                {
                    Log("   Age : " + face.Age + "; Gender : " + face.Gender);
                }
            }

            if (result.Color != null)
            {
                Log("AccentColor : " + result.Color.AccentColor);
                Log("Dominant Color Background : " + result.Color.DominantColorBackground);
                Log("Dominant Color Foreground : " + result.Color.DominantColorForeground);

                if (result.Color.DominantColors != null && result.Color.DominantColors.Length > 0)
                {
                    string colors = "Dominant Colors : ";
                    foreach (var color in result.Color.DominantColors)
                    {
                        colors += color + " ";
                    }
                    Log(colors);
                }
            }

            if (result.Description != null)
            {
                Log("Description : ");
                foreach (var caption in result.Description.Captions)
                {
                    Log("   Caption : " + caption.Text + "; Confidence : " + caption.Confidence);
                }
                string tags = "   Tags : ";
                foreach (var tag in result.Description.Tags)
                {
                    tags += tag + ", ";
                }
                Log(tags);

            }

            if (result.Tags != null)
            {
                Log("Tags : ");
                foreach (var tag in result.Tags)
                {
                    Log("   Name : " + tag.Name + "; Confidence : " + tag.Confidence + "; Hint : " + tag.Hint);
                }
            }

        }

        private static void LogOcrResults(OcrResults results)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (results != null && results.Regions != null)
            {
                stringBuilder.Append("Text: ");
                stringBuilder.AppendLine();
                foreach (var item in results.Regions)
                {
                    foreach (var line in item.Lines)
                    {
                        foreach (var word in line.Words)
                        {
                            stringBuilder.Append(word.Text);
                            stringBuilder.Append(" ");
                        }

                        stringBuilder.AppendLine();
                    }

                    stringBuilder.AppendLine();
                }
            }

            Log(stringBuilder.ToString());
        }

        #endregion
    }
}
