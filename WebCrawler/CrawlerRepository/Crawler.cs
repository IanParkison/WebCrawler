using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace WebCrawler.CrawlerRepository
{
    public class Crawler
    {
        private int _wordCount = 10;
        private const string _url = "https://en.wikipedia.org/wiki/Microsoft";
        private const string _startingFilter = "History";
        private const string _endingFilter = "Corporate_affairs";
        private const string _text = "#text";
        private Dictionary<string, int> _wordTable;
        private HashSet<string> _exclusionSet;

        public Crawler(int wordCount, List<string> exclusionlist)
        {
            _wordCount = wordCount;
            _exclusionSet = new HashSet<string>(exclusionlist);
            _wordTable = new Dictionary<string, int>();
        }

        public List<KeyValuePair<string, int>> GetWords()
        {
            var parser = new HtmlAgilityPack.HtmlWeb();
            var document = parser.Load(_url);
            var readText = false;

            foreach (var node in document.DocumentNode.Descendants())
            {
                if (node.Id == _startingFilter)
                {
                    readText = true;
                }

                if (node.Id == _endingFilter)
                {
                    readText = false;
                }

                if (readText)
                {
                    if (node.Name == _text)
                    {
                        var strippedString = _formatWords(node.InnerText);

                        if (!string.IsNullOrEmpty(strippedString))
                        {
                            _addWords(strippedString);
                        }                      
                    }
                }
            }

            var sorted = _wordTable.ToList().OrderByDescending(entry => entry.Value).Take(_wordCount).ToList();

            return sorted;
        }

        private string _formatWords(string innerText)
        {
            var strippedString = Regex.Replace(innerText, @"\p{P}|\t|\n|\r", "");
            return strippedString.Trim();
        }
        
        private void _addWords(string words)
        {
            var wordslist = words.Split(' ').ToList();

            foreach (var word in wordslist)
            {
                if (!_exclusionSet.Contains(word))
                {
                    if (_wordTable.ContainsKey(word))
                    {
                        _wordTable[word]++;
                    }
                    else
                    {
                        _wordTable.Add(word, 1);
                    }
                }
            }
        }
    }
}
