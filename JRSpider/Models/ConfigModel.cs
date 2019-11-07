using System.Configuration;

namespace JRSpider.Models
{
    public class ConfigModel
    {
        private static string savePath = ConfigurationManager.AppSettings["saveDirectory"];
        private static string downloadUrl = ConfigurationManager.AppSettings["downLoadUrl"];

        public static string Url
        {
            get { return downloadUrl; }
            private set
            {
                downloadUrl = value;
            }
        }

        public static string SavePath
        {
            get
            {
                return savePath;
            }
            set
            {
                savePath = value;
            }
        }
    }
}