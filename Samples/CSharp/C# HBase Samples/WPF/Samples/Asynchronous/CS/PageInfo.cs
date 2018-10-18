#region Copyright Syncfusion Inc. 2001 - 2016

// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws.

#endregion Copyright Syncfusion Inc. 2001 - 2016

namespace Asynchronous
{
    public class PageInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageInfo"/> class.
        /// </summary>
        /// <param name="PagedRows">The paged rows.</param>
        /// <param name="MaximumRows">The maximum rows.</param>
        public PageInfo(int PagedRows, int MaximumRows)
        {
            this.StartIndex = PagedRows;
            this.EndIndex = PagedRows + MaximumRows;
        }

        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
    }
}