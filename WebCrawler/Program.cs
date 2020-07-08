using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.CrawlerRepository;

namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            var count = 10;
            var exclusions = new List<string>();

            Console.WriteLine("Enter number of most frequent words to return: ");

            if (int.TryParse(Console.ReadLine(), out var result))
            {
                count = result;
            }

            Console.WriteLine("Enter comma separated list of words to exclude: ");

            var line = Console.ReadLine();

            if (!string.IsNullOrEmpty(line))
            {
                exclusions = line.ToString().Replace(" ", string.Empty).Split(',').ToList();
            }

            var crawler = new Crawler(count, exclusions);

            var words = crawler.GetWords();

            Console.WriteLine("Word\t\t# of occurrences");

            if (words.Any())
            {
                foreach (var word in words)
                {
                    Console.WriteLine($"{word.Key}\t\t{word.Value}");
                }
            }
        }
    }
}
