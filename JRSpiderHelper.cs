using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using System.Threading;

namespace JRSpider
{
    class JRSpiderHelper
    {
        public static void DownloadImages(string pageUrl, string savePath, WebClient webClient)
        {
            Random random = new Random();
            string title = string.Empty;
            int count = 0;
            string imageUrl = string.Empty;
            WebHeaderCollection webHeaderCollection = webClient.Headers;
            SetHeader(webHeaderCollection);
            DownloadSourceHtml(webClient, pageUrl, ref title, ref imageUrl, ref count);
            var directory = CreateDirectory(savePath, title);

            Console.WriteLine("Start download image: " + title);
            for (int i = 1; i <= count; i++)
            {
                string currentWebUrl;
                if (i == 1)
                {
                    currentWebUrl = pageUrl;
                }
                else
                {
                    currentWebUrl = pageUrl + "/" + i;
                }
            label: Console.WriteLine(currentWebUrl);
                try
                {
                    SetHeader(webHeaderCollection);
                    DownloadSourceHtml(webClient, currentWebUrl, ref title, ref imageUrl, ref count);
                    webHeaderCollection.Set(HttpRequestHeader.Referer, imageUrl);
                    webClient.DownloadFile(imageUrl, directory + "\\" + i + ".jpg");
                    Thread.Sleep(random.Next(800, 1200));
                }
                catch (Exception)
                {
                    Console.WriteLine($"Sleeping, current fail page:{i}");
                    Thread.Sleep(random.Next(1000, 1800));
                    goto label;
                }
            }
            Console.WriteLine("Download completed");
        }

        private static void DownloadSourceHtml(WebClient webClient, string sourceUrl, ref string tile, ref string imageUrl, ref int count)
        {
            var imgXPath = "/html/body/div[2]/div[1]/div[3]/p/a/img";
            var countXPath = "/html/body/div[2]/div[1]/div[4]/a[5]/span";
            var htmlDocument = GetHtmlDocument(webClient, sourceUrl);
            var imageNodes = GetHtmlNodes(htmlDocument, imgXPath);
            GetImageTitleAndUrl(imageNodes, ref tile, ref imageUrl);
            var countNodes = GetHtmlNodes(htmlDocument, countXPath);
            GetImageCount(countNodes, ref count);
        }
        private static string CreateDirectory(string savePath, string title)
        {
            var directory = Path.Combine(savePath, title);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            return directory;
        }
        private static HtmlDocument GetHtmlDocument(WebClient webClient, string url)
        {
            using (Stream stream = webClient.OpenRead(url))
            using (StreamReader streamReader = new StreamReader(stream))
            {
                var sourceHtml = streamReader.ReadToEnd();
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(sourceHtml);
                return htmlDocument;
            }
        }

        private static HtmlNodeCollection GetHtmlNodes(HtmlDocument htmlDocument, string xPath)
        {
            return htmlDocument.DocumentNode.SelectNodes(xPath);
        }

        private static void GetImageTitleAndUrl(HtmlNodeCollection htmlNodes, ref string title, ref string imageUrl)
        {
            foreach (var node in htmlNodes)
            {
                imageUrl = node.Attributes["src"].Value;
                if (string.IsNullOrEmpty(title))
                {
                    title = node.Attributes["alt"].Value;
                }
            }
        }

        private static void GetImageCount(HtmlNodeCollection htmlNodes, ref int count)
        {
            if (count == 0)
            {
                foreach (var node in htmlNodes)
                {
                    count = Convert.ToInt32(node.InnerText);
                }
            }
        }

        private static void SetHeader(WebHeaderCollection webHeaderCollection)
        {
            webHeaderCollection.Set(HttpRequestHeader.Pragma, "no-cache");
            webHeaderCollection.Set(HttpRequestHeader.ContentType, "image/gif");
            webHeaderCollection.Set(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64)" +
                " AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.142 Safari/537.36");
            webHeaderCollection.Set(HttpRequestHeader.CacheControl, "no-cache");
        }
    }
}
