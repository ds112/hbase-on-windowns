<%@ Page Title="" Language="C#" MasterPageFile="~/Samplebrowser.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="WebSampleBrowser.Default" %>

<%@ Register Assembly="Syncfusion.EJ" Namespace="Syncfusion.JavaScript.Models" TagPrefix="ej" %>
<asp:Content ID="contentHeader" ContentPlaceHolderID="SampleHeading" runat="server">
    <span class="sampletitle">
        Data Binding
    </span>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ControlsSection" runat="server">
    <p><strong>This sample demonstrates,  how the result of the HBase can be bound to a grid for viewing.
</strong></p>

    <div id="ErrorMessage" runat="server" style="color:red;padding-top:3px;padding-bottom:10px"></div>
    <ej:Grid ID="FlatGrid" runat="server" AllowScrolling="true">
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

</asp:Content>
