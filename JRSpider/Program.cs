using JRSpider.Models;
using System;
using System.IO;
using System.Net;

namespace JRSpider
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            WebClient webClient = new WebClient();
            if (!Directory.Exists(ConfigModel.SavePath))
            {
                Console.WriteLine("Please Enter a valid save path...");
                ConfigModel.SavePath = Console.ReadLine();
            }
            JRSpiderHelper.DownloadImages(ConfigModel.Url, ConfigModel.SavePath, webClient);
            webClient.Dispose();
            Console.ReadLine();
        }
    }
}