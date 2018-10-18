<%@ Page Title="" Language="C#" MasterPageFile="~/Samplebrowser.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="WebSampleBrowser.Default" %>

<%@ Register Assembly="Syncfusion.EJ" Namespace="Syncfusion.JavaScript.Models" TagPrefix="ej" %>
<asp:Content ID="contentHeader" ContentPlaceHolderID="SampleHeading" runat="server">
    <span class="sampletitle">
        Data Binding
    </span>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ControlsSection" runat="server">
    <p><strong>This sample demonstrates,  how the result of the Hive query can be bound to a grid for viewing.
</strong></p>
    
    <div id="ErrorMessage" runat="server" style="color:red;padding-top:3px;padding-bottom:10px"></div>
    <ej:Grid ID="FlatGrid" runat="server" AllowScrolling="true">
        <Columns>
            <ej:Column Field="0" HeaderText="Contact ID" TextAlign="Left" Width="70" />
            <ej:Column Field="1" HeaderText="Full Name" TextAlign="Left" Width="90" />
            <ej:Column Field="2" HeaderText="Age" TextAlign="Left" Width="30" />
            <ej:Column Field="3" HeaderText="Email Address" TextAlign="Left" Width="160" />
            <ej:Column Field="4" HeaderText="Phone No" TextAlign="Left" Width="120" />
            <ej:Column Field="5" HeaderText="Modified Date" TextAlign="Left" Width="120" />
        </Columns>
        <ScrollSettings Height="300" Width="880"></ScrollSettings>
    </ej:Grid>

</asp:Content>
