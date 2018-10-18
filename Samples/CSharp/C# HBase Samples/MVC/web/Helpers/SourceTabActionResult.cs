#region Copyright Syncfusion Inc. 2001 - 2016

// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws.

#endregion Copyright Syncfusion Inc. 2001 - 2016

using System.Web.Mvc;

namespace MVCSampleBrowser.Helpers
{
    public class SourceTabActionResult : ActionResult
    {
        private string FileName
        {
            get;
            set;
        }

        public SourceTabActionResult(string fileName)
        {
            this.FileName = fileName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            ProductXmlDataEngine xmlEngine = new ProductXmlDataEngine();
            TabType tabType = xmlEngine.GetTabType(this.FileName);

            string content = xmlEngine.GetTabContent(tabType, this.FileName);

            context.HttpContext.Response.Output.Write(content);
        }
    }
}