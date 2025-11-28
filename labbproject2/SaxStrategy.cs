using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace labbproject2
{ 
    public class SaxSearchStrategy : ISearchStrategy
    {
        public List<Book> Search(string filePath, string authorCriteria)
        {
            var result = new List<Book>();
            var currentBook = new Book();
            string currentElement = "";

            using (var reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        currentElement = reader.Name;
                        if (currentElement == "Book")
                        {
                            currentBook = new Book();
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.Text)
                    {
                        switch (currentElement)
                        {
                            case "Author":
                                currentBook.Author = reader.Value;
                                break;
                            case "Title":
                                currentBook.Title = reader.Value;
                                break;
                            case "Category":
                                currentBook.Category = reader.Value;
                                break;
                            case "Name":
                                currentBook.ReaderName = reader.Value;
                                break;
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        if (reader.Name == "Book")
                        {
                            if (string.IsNullOrEmpty(authorCriteria) ||
                                currentBook.Author.Contains(authorCriteria))
                            {
                                result.Add(currentBook);
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}