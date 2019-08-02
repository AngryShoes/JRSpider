using System;
using System.IO;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using System.Configuration;

namespace JRSpider
{
    class Program
    {
        private static readonly string savePath = ConfigurationManager.AppSettings["saveDirectory"];
        private static readonly string downloadUrl = ConfigurationManager.AppSettings["downLoadUrl"];
        static void Main(string[] args)
        {
            WebClient webClient = new WebClient();
            JRSpiderHelper.DownloadImages(downloadUrl, savePath, webClient);
            webClient.Dispose();
            Console.ReadLine();
        }
    }
}
