using labbproject2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labbproject2
{
    public interface ISearchStrategy
    {
        List<Book> Search(string filePath, string authorCriteria);
    }
}