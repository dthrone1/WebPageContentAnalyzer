using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebPageContentAnalyzer.Models;

namespace WebPageContentAnalyzer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Analyze(string url)
        {
            // Download the HTML content of the webpage
            WebClient webClient = new WebClient();
            string htmlContent = webClient.DownloadString(url);

            // Load the HTML content into an HtmlDocument using HtmlAgilityPack
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            // Get the images from the HTML document
            var imageUrls = htmlDoc.DocumentNode.Descendants("img").Select(e => e.GetAttributeValue("src", null)).Where(src => !string.IsNullOrEmpty(src));

            // Extract the text from the HTML document
            string text = htmlDoc.DocumentNode.InnerText;

            // Split the text into words and count the number of occurrences of each word
            Dictionary<string, int> wordCountDict = new Dictionary<string, int>();
            string[] words = text.Split(new[] { ' ', '\n', '\r', '&' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in words)
            {
                if (wordCountDict.ContainsKey(word))
                {
                    wordCountDict[word]++;
                }
                else
                {
                    wordCountDict.Add(word, 1);
                }
            }

            // Sort the dictionary by value and take the top 10
            var top10Words = wordCountDict.OrderByDescending(x => x.Value).Take(10);

            // Create a ViewModel to pass the data to the View
            var viewModel = new AnalyzeViewModel
            {
                Url = url,
                WordCount = words.Length,
                Top10Words = top10Words,
                ImageUrls = imageUrls
            };
            return View(viewModel);
        }
    }
}