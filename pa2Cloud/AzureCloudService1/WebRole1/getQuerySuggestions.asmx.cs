using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure;
using System.IO;
using System.Diagnostics;

namespace WebRole1
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {

        private static Trie trie;
        private string filePath = System.IO.Path.GetTempPath() + "\\wikiTitles.txt";

        /// <summary>
        /// Downloads the wiki data from blob
        /// </summary>
        [WebMethod]
        public string downloadWiki()
        {
            //File.Delete(this.filePath);

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("container123");
            {
                CloudBlockBlob blob = container.GetBlockBlobReference("wikiTitles.txt");

                using (var fileStream = System.IO.File.OpenWrite(this.filePath))
                {
                    blob.DownloadToStream(fileStream);
                }
            }
            return "download successful";

        }

        /// <summary>
        /// Reads each line of wiki data and builds trie structure
        /// </summary>
        [WebMethod]
        public string buildTrie()
        {
            int check = 0;
            string lineCheck = "";

            trie = new Trie();

            using (StreamReader reader = new StreamReader(this.filePath))
            {
                PerformanceCounter counter = new PerformanceCounter("Memory", "Available MBytes");
                while (!reader.EndOfStream)
                {
                    if (check % 1000 == 0)
                    {
                        if (counter.NextValue() < 50)
                        {
                            break;
                        }
                    }
                    string line = reader.ReadLine();
                    trie.AddTitle(line.ToLower());
                    check++;
                    lineCheck = line;
                }
            }
            return "building trie successful " + " " + lineCheck + " " + check;
        }

        /// <summary>
        /// Searches through the Trie data structure 
        /// </summary>
        /// <param name="input"></param>
        /// <returns>returns string in JSON format</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)] 
        public string searchTrie(string input)
        {
            List<string> results = trie.SearchForPrefix(input.ToLower().Trim().Replace(' ', '_'));
            return new JavaScriptSerializer().Serialize(results);
        }

        /// <summary>
        /// Returns memory avalibility in cloud service
        /// </summary>
        [WebMethod]
        public string getMemory()
        {
            float mem = (new PerformanceCounter("Memory", "Available MBytes")).NextValue();
            return mem.ToString();
        }
    }
}
