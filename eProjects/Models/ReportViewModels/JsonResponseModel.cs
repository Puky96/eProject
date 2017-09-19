using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eProjects.Models.ReportViewModels
{
    public class Col
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Pattern { get; set; }
        public string Role { get; set; }
        public string Type { get; set; }
    }

    public class C
    {
        public object V { get; set; }
        public object F { get; set; }
    }

    public class Row
    {
        public IList<C> C { get; set; }
    }

    public class JsonResponseModel
    {
        public IList<Col> Cols { get; set; }
        public IList<Row> Rows { get; set; }
    }
}
