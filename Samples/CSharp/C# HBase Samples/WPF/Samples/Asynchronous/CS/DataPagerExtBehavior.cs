#region Copyright Syncfusion Inc. 2001 - 2016

// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws.

#endregion Copyright Syncfusion Inc. 2001 - 2016

using Syncfusion.ThriftHBase.Base;
using Syncfusion.Windows.Controls.Grid;
using System.Windows.Interactivity;

namespace Asynchronous
{
    public class SummaryCalculationBehavior : Behavior<MainWindow>
    {
        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
        protected override void OnAttached()
        {
            this.AssociatedObject.dataPager.OnDemandDataSourceLoad += new GridDataOnDemandPageLoadingEventHandler(dataPager_OnDemandDataSourceLoad);
        }

        private int PreviousPageIndex;

        /// <summary>
        /// Handles the OnDemandDataSourceLoad event of the dataPager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Syncfusion.Windows.Controls.Grid.GridDataOnDemandPageLoadingEventArgs"/> instance containing the event data.</param>
        private async void dataPager_OnDemandDataSourceLoad(object sender, GridDataOnDemandPageLoadingEventArgs e)
        {
            DataPagerExt ex = e.Source as DataPagerExt;
            ViewModel dataContext = this.AssociatedObject.DataContext as ViewModel;
            bool IsCache = false;

            //Assigning number of rows to be fetched from HBase
            HBaseOperation.FetchSize = 5000;

            if (!dataContext.ResultCollection.ContainsKey(ex.PageIndex) && HBaseOperation.HasRows)
            {
                dataContext.OrderDetails = await HBaseOperation.ScanTableAsync(dataContext.TableName, dataContext.Connection);
                dataContext.ResultCollection.Add(ex.PageIndex == -1 ? 1 : ex.PageIndex, dataContext.OrderDetails);
                IsCache = false;
            }
            else
            {
                dataContext.OrderDetails = dataContext.ResultCollection[ex.PageIndex];
                IsCache = true;
            }

            bool IsPrevious = PreviousPageIndex > ex.PageIndex;

            if (!IsPrevious && HBaseOperation.HasRows && !IsCache)
            {
                if (ex.PageIndex != -1)
                    ex.PageCount = ex.PageCount + 1;
            }

            PreviousPageIndex = ex.PageIndex == -1 ? 1 : ex.PageIndex;
        }

        /// <summary>
        /// Called when [detaching].
        /// </summary>
        protected override void OnDetaching()
        {
            this.AssociatedObject.dataPager.OnDemandDataSourceLoad -= new GridDataOnDemandPageLoadingEventHandler(dataPager_OnDemandDataSourceLoad);
        }
    }
}