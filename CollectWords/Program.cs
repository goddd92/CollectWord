using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectWords
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> words = new List<string> { };
            var text = new WebClient().DownloadString("http://book-online.com.ua/read.php?book=9628");

            byte[] bytes = Encoding.Default.GetBytes(text);
            text = Encoding.UTF8.GetString(bytes);

            bool tagOpened = false;
            var sb = new StringBuilder();
            foreach (var c in text)
            {
                switch (c)
                {
                    case '<':
                        tagOpened = true;
                        break;
                    case '>':
                        tagOpened = false;
                        break;
                    default:
                        if (tagOpened == false)
                            sb.Append(c);
                        break;
                }
            }
            text = sb.ToString();

            string[] separators = new string[] { ".", ",", "!", "\'", " ", "\'s", "", "?", "\n", "-", ":", "=", "–" };

            foreach (string word in text.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                words.Add(word);

            var dictionary1 = new Dictionary<string, int>();
            var dictionary2 = new Dictionary<string, int>();
            foreach (string word in words)
            {
                if (dictionary1.ContainsKey(word))
                {
                    dictionary1[word]++;
                }
                else
                {
                    dictionary1.Add(word, 1);
                }
            }

            foreach (var item in dictionary1)
            {
                if (item.Value > 5)
                    dictionary2.Add(item.Key, item.Value);
            }
            words = dictionary2.Select(kvp => kvp.Key + " " + kvp.Value + " \n").ToList();

            System.IO.File.WriteAllText("C:\\Users\\koshkowskyii\\Documents\\TestCopy.txt", text);
            System.IO.File.WriteAllLines("C:\\Users\\koshkowskyii\\Documents\\List.txt", words);


        }
    }
}


//namespace ConsoleApplication1
//{
//    using System;
//    using System.IO;
//    using System.Linq;
//    using System.Net;
//    using System.Text;

//    public class Program
//    {
//        private static void Main(string[] args)
//        {
//            var str = new WebClient { Encoding = Encoding.UTF8 }.DownloadString("http://book-online.com.ua/read.php?book=9628");
//            bool tagOpened = false;

//            var sb = new StringBuilder();
//            foreach (var c in str)
//            {
//                switch (c)
//                {
//                    case '<':
//                        tagOpened = true;
//                        break;
//                    case '>':
//                        tagOpened = false;
//                        break;
//                    default:
//                        if (tagOpened == false)
//                            sb.Append(c);
//                        break;
//                }
//            }

//            var strings = sb.ToString().Split(new[] { ' ', '.', ',', '\t', '\r', '\n', '–', '=' }, StringSplitOptions.RemoveEmptyEntries)
//                .Select(x => x.Trim(' '))
//                .GroupBy(x => x)
//                .Where(x => x.Count() > 5)
//                .OrderByDescending(x => x.Count())
//                .Select(x => x.Key + ' ' + x.Count());

//            File.WriteAllLines("123.txt", strings);
//        }
//    }
//}