<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Samplebrowser.Master" CodeBehind="Default.aspx.cs" Inherits="WebSampleBrowser.Asynchronous_Grid_Paging.Default" %>

<%@ Register assembly="Syncfusion.EJ" namespace="Syncfusion.JavaScript.Models" tagprefix="ej" %>
<asp:Content runat="server" ID="conHead" ContentPlaceHolderID="HeadSection">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ControlsSection" runat="server">
    <p><strong> This sample demonstrates the asynchronous access of records from the HBase</strong></p>
     <div id="ErrorMessage" runat="server" style="color:red;padding-top:3px;padding-bottom:10px"></div>
    <ej:Grid ID="Paging" AllowScrolling="true" AllowPaging="true" runat="server">
        <DataManager URL="Default.aspx/DataSource" Adaptor="UrlAdaptor" />
        <Columns>
            <ej:Column Field="ContactId" HeaderText="ContactID" TextAlign="Left" Width="80" />
            <ej:Column Field="FullName" HeaderText="FullName" TextAlign="Left" Width="80" />
            <ej:Column Field="Age" HeaderText="Age" TextAlign="Left" Width="80" />
            <ej:Column Field="EmailId" HeaderText="EmailAddress" TextAlign="Left" Width="150" />
            <ej:Column Field="PhoneNumber" HeaderText="PhoneNo" TextAlign="Left" Width="120" />
            <ej:Column Field="ModifiedDate" HeaderText="ModifiedDate" TextAlign="Left" Width="120" />
        </Columns>
        <PageSettings PageSize="12" EnableTemplates="true" Template="#pagerTemplate" />
        <ClientSideEvents Load="load" ActionComplete="actionComplete" />
        
    </ej:Grid>
    </asp:Content>
    <asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptSection">
    <script type="text/x-jsrender" id="pagerTemplate">
    <a id="prev" class="e-disable" onclick="gotoPage(this)" style="border: none; height: 25px; text-decoration: none; cursor: pointer"><img src=<%=ResolveUrl("~/Content/images/arrow.png") %> style="vertical-align:text-bottom" alt="Previous" width="14" height="16" /></a>
    <a onclick="gotoPage(this)" id="next" style="border: none; text-decoration: none; cursor: pointer"><img src=<%=ResolveUrl("~/Content/images/rightarrow.png") %> style="vertical-align:text-bottom" alt="Next" width="14" height="16" /></a>
    </script>
    <script type="text/javascript">
        function gotoPage(sender) {

            var gridObj = $('#<%= Paging.ClientID %>').data("ejGrid");
            var gridObj1 = $('#<%= Paging.ClientID %>').data("ejGrid");
            var val = $("#txtPageNumber").val();
            var currentPage = gridObj.model.pageSettings.currentPage
            if (sender.id == "prev") {
                if ($("#prev").hasClass('e-disable'))
                    return false;
                gridObj.gotoPage(parseInt(currentPage) - 1);
                $("#next").removeClass('e-disable');
            }
            else {
                if ($("#next").hasClass('e-disable'))
                    return false;
                gridObj.gotoPage(parseInt(currentPage) + 1);
            }
            if (gridObj.model.pageSettings.currentPage == 1)
                $("#prev").addClass('e-disable');
            else
                $("#prev").removeClass('e-disable');
            $("#txtPageNumber").val(gridObj.model.pageSettings.currentPage)
            
        }

        var customs = new ej.UrlAdaptor().extend({
            processResponse: function (data, ds, query, xhr, request, changes) {
                var pvt = request.ejPvtData || {};
                if (xhr && xhr.getResponseHeader("Content-Type").indexOf("xml") != -1 && data.nodeType == 9)
                    return query._requiresCount ? { result: [], count: 0 } : [];
                var d = JSON.parse(request.data);
                if (d && d.action === "batch" && data.added) {
                    changes.added = data.added;
                    return changes;
                }
                if (data.d)
                    data = data.d;
                if (data.hasRows != undefined)
                    window.disablePage = data.hasRows;
                if (pvt.groups && pvt.groups.length) {
                    var groups = pvt.groups, args = {};
                    if (data["count"]) args.count = data.count;
                    if (data["result"]) data = data.result;
                    for (var i = 0; i < groups.length; i++)
                        data = ej.group(data, groups[i]);
                    if (args.count)
                        args.result = data;
                    else
                        args = data;
                    return args;
                }
                return data;
            }
        });

        function load(args) {
            this.model.dataSource.adaptor = new customs();
        }

        function actionComplete(args) {
            if (args.requestType == "paging" && window.disablePage != undefined) {
                $("#next").addClass('e-disable');
                delete window.disablePage;
            }
        }

        function callajax() {
            $.ajax({
                type: "POST",
                url: "Default.aspx/Data",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                async: false,
                dataType: "json",
                success: OnSuccess
            });

            function OnSuccess(response) {
                window.flag = "true";
            }

            if (window.flag == "true") {
                __doPostBack('', '');
                window.flag = "false";
            }
        }
    </script>
</asp:Content>
