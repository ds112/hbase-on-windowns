using System.Collections;

namespace MVCSampleBrowser.Controllers.Grid
{
    internal class DataResult
    {
        public IEnumerable result { get; set; }
        public int count { get; set; }
        public string hasRows { get; set; }
    }
}