using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCSampleBrowser.Controllers.Grid
{
    class DataResult
    {
        public IEnumerable result { get; set; }
        public int count { get; set; }
        public string hasRows { get; set; }
    }
}