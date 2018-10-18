<%@ Page Title="" Language="C#" MasterPageFile="~/Samplebrowser.Master"  ValidateRequest="false" CodeBehind="Default.aspx.cs" Inherits="WebSampleBrowser.Filter.Default" %>

<%@ Register assembly="Syncfusion.EJ" namespace="Syncfusion.JavaScript.Models" tagprefix="ej" %>

<%@ Register assembly="Syncfusion.EJ" namespace="Syncfusion.JavaScript.Models" tagprefix="ej" %>

<%@ Register assembly="Syncfusion.EJ" namespace="Syncfusion.JavaScript.Models" tagprefix="ej" %>

<%@ Register assembly="Syncfusion.EJ" namespace="Syncfusion.JavaScript.Models" tagprefix="ej" %>

<%@ Register assembly="Syncfusion.EJ" namespace="Syncfusion.JavaScript.Models" tagprefix="ej" %>

<%@ Register assembly="Syncfusion.EJ" namespace="Syncfusion.JavaScript.Models" tagprefix="ej" %>

<%@ Register assembly="Syncfusion.EJ" namespace="Syncfusion.JavaScript.Models" tagprefix="ej" %>

<%@ Register assembly="Syncfusion.EJ" namespace="Syncfusion.JavaScript.Models" tagprefix="ej" %>

<%@ Register assembly="Syncfusion.EJ" namespace="Syncfusion.JavaScript.Models" tagprefix="ej" %>

<%@ Register assembly="Syncfusion.EJ" namespace="Syncfusion.JavaScript.Models" tagprefix="ej" %>

<%@ Register assembly="Syncfusion.EJ" namespace="Syncfusion.JavaScript.Models" tagprefix="ej" %>

<%@ Register assembly="Syncfusion.EJ" namespace="Syncfusion.JavaScript.Models" tagprefix="ej" %>

<%@ Register assembly="Syncfusion.EJ" namespace="Syncfusion.JavaScript.Models" tagprefix="ej" %>

<asp:Content ID="Content5" ContentPlaceHolderID="ControlsSection" runat="server">
     <strong>
<asp:RadioButton GroupName="Filter" ID="RowFilters"  runat="server" OnClick="RowFilter_Checked()" TabIndex="1" Text="Row Filter &nbsp &nbsp" Checked="True"   />
<asp:RadioButton GroupName="Filter" ID="SingleColumnValueFilters" OnClick="SingleColumnValueFilter_Checked()" runat="server"  Text="SingleColumnValue Filter &nbsp &nbsp"   />
</strong>
     <p><strong>This sample demonstrates,  how the result of the HBase can be bound to a grid for viewing.
</strong></p>

    <div id="ErrorMessage" runat="server" style="color:red;padding-top:3px;padding-bottom:10px"></div>
    <div class="RowFilter">
    <ej:Grid ID="FlatGrid1" runat="server" AllowScrolling="true">
        <Columns>
            <ej:Column Field="0" HeaderText="ContactId" TextAlign="Left" Width="150" />
            <ej:Column Field="1" HeaderText="contact:EmailId" TextAlign="Left" Width="220" />
            <ej:Column Field="2" HeaderText="contact:PhoneNo" TextAlign="Left" Width="150" />
            <ej:Column Field="3" HeaderText="info:Age" TextAlign="Left" Width="150" />
            <ej:Column Field="4" HeaderText="info:FullName" TextAlign="Left" Width="150" />
            <ej:Column Field="5" HeaderText="others:ModifiedDate" TextAlign="Left" Width="150" />
            
        </Columns>
        <ScrollSettings Height="300" Width="880"></ScrollSettings>
    </ej:Grid>
        </div>
    <div class="SingleColumnValueFilter">
     <ej:Grid ID="FlatGrid2" runat="server" AllowScrolling="true">
        <Columns>
            <ej:Column Field="0" HeaderText="ContactId" TextAlign="Left" Width="150" />
            <ej:Column Field="1" HeaderText="contact:EmailId" TextAlign="Left" Width="220" />
            <ej:Column Field="2" HeaderText="contact:PhoneNo" TextAlign="Left" Width="150" />
            <ej:Column Field="3" HeaderText="info:Age" TextAlign="Left" Width="150" />
            <ej:Column Field="4" HeaderText="info:FullName" TextAlign="Left" Width="150" />
            <ej:Column Field="5" HeaderText="others:ModifiedDate" TextAlign="Left" Width="150" />
            
        </Columns>
        <ScrollSettings Height="300" Width="880"></ScrollSettings>
    </ej:Grid>
        </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".RowFilter").css("display", "block");
            $(".SingleColumnValueFilter").css("display", "none");
        });

        function RowFilter_Checked() {
           debugger
           $(".SingleColumnValueFilter").css("display", "none");
           $(".RowFilter").css("display", "block");
               
           
        }
        function SingleColumnValueFilter_Checked()
        {
            debugger
            $(".RowFilter").css("display", "none");
            $(".SingleColumnValueFilter").css("display", "block");
        }
</script>
</asp:Content>
