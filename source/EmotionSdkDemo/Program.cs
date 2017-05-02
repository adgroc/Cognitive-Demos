using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Emotion;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionSdkDemo
{
    class Program
    {
        private static string apiKey = "";
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Detecting emotion:");
                DetectEmotion().Wait();
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

        private static async Task DetectEmotion()
        {
            var imageUrl = "";

            var emotionServiceClient = new EmotionServiceClient(apiKey);

            Emotion[] emotionResult = await emotionServiceClient.RecognizeAsync(imageUrl);
            LogEmotionResult(emotionResult);
        }

        #region Helpers

        private static void Log(string message)
        {
            Console.WriteLine(message);
        }

        private static void LogEmotionResult(Emotion[] emotionResult)
        {
            int emotionResultCount = 0;
            if (emotionResult != null && emotionResult.Length > 0)
            {
                foreach (Emotion emotion in emotionResult)
                {
                    Log("Emotion[" + emotionResultCount + "]");
                    Log("  .FaceRectangle = left: " + emotion.FaceRectangle.Left
                             + ", top: " + emotion.FaceRectangle.Top
                             + ", width: " + emotion.FaceRectangle.Width
                             + ", height: " + emotion.FaceRectangle.Height);

                    Log("  Anger    : " + emotion.Scores.Anger.ToString());
                    Log("  Contempt : " + emotion.Scores.Contempt.ToString());
                    Log("  Disgust  : " + emotion.Scores.Disgust.ToString());
                    Log("  Fear     : " + emotion.Scores.Fear.ToString());
                    Log("  Happiness: " + emotion.Scores.Happiness.ToString());
                    Log("  Neutral  : " + emotion.Scores.Neutral.ToString());
                    Log("  Sadness  : " + emotion.Scores.Sadness.ToString());
                    Log("  Surprise  : " + emotion.Scores.Surprise.ToString());
                    Log("");
                    emotionResultCount++;
                }
            }
            else
            {
                Log("No emotion is detected. This might be due to:\n" +
                    "    image is too small to detect faces\n" +
                    "    no faces are in the images\n" +
                    "    faces poses make it difficult to detect emotions\n" +
                    "    or other factors");
            }
        }

        #endregion
    }
}
