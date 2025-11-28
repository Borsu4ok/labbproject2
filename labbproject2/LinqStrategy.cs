using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace labbproject2
{
    public class LinqSearchStrategy : ISearchStrategy
    {
        public List<Book> Search(string filePath, string authorCriteria)
        {
            var doc = XDocument.Load(filePath);

            var query = from b in doc.Descendants("Book")
                        where string.IsNullOrEmpty(authorCriteria) ||
                              b.Element("Author").Value.Contains(authorCriteria)
                        select new Book
                        {
                            Author = b.Element("Author")?.Value ?? "",
                            Title = b.Element("Title")?.Value ?? "",
                            Category = b.Element("Category")?.Value ?? "",
                            ReaderName = b.Element("Reader")?.Element("Name")?.Value ?? ""
                        };

            return query.ToList();
        }
    }
}