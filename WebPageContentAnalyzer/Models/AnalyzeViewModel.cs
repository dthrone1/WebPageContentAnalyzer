using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPageContentAnalyzer.Models
{
    public class AnalyzeViewModel
    {
        public string Url { get; set; }
        public int WordCount { get; set; }
        public IEnumerable<KeyValuePair<string, int>> Top10Words { get; set; }
        public IEnumerable<string> ImageUrls { get; set; }
    }
}