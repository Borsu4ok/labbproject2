using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace labbproject2
{
    public class DomSearchStrategy : ISearchStrategy
    {
        public List<Book> Search(string filePath, string authorCriteria)
        {
            var result = new List<Book>();
            var doc = new XmlDocument();
            doc.Load(filePath);

            XmlNodeList nodes = doc.SelectNodes("//Book");
            if (nodes == null) return result;

            foreach (XmlNode node in nodes)
            {
                var book = new Book();
                book.Author = node["Author"]?.InnerText ?? "";
                book.Title = node["Title"]?.InnerText ?? "";
                book.Category = node["Category"]?.InnerText ?? "";

                var readerNode = node["Reader"];
                if (readerNode != null)
                {
                    book.ReaderName = readerNode["Name"]?.InnerText ?? "";
                }

                if (string.IsNullOrEmpty(authorCriteria) ||
                    book.Author.Contains(authorCriteria))
                {
                    result.Add(book);
                }
            }
            return result;
        }
    }
}