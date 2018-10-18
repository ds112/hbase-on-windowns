#region Copyright Syncfusion Inc. 2001 - 2016

// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws.

#endregion Copyright Syncfusion Inc. 2001 - 2016

using MVCSampleBrowser.Helpers;
using System.Web.Mvc;

namespace MVCSampleBrowser.Controllers
{
    public class SourceCodeTabController : Controller
    {
        public ActionResult Index(string file)
        {
            return new SourceTabActionResult(file);
        }
    }
}