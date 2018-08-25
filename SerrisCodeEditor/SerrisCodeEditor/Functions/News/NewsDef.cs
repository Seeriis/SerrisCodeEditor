using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerrisCodeEditor.Functions.News
{
    public class News
    {
        public int ArticleID { get; set; }
        public string Title { get; set; }
        public string HeaderImage { get; set; }
        public string Date { get; set; }
        public string Author { get; set; }
    }

    public class NewsContent
    {
        public int ArticleID { get; set; }
        public string Title { get; set; }
        public string HeaderImage { get; set; }
        public string Date { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
    }
}
