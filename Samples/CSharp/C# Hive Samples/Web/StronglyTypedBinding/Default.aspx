<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Samplebrowser.Master" CodeBehind="Default.aspx.cs" Inherits="WebSampleBrowser.StronglyTypedView.Default" %>

<asp:Content ID="Content5" ContentPlaceHolderID="ControlsSection" runat="server">
    <p><strong>This sample demonstrates, how to convert the Hive results to a strongly typed data and bind it to a grid for viewing</strong></p>
     <div id="ErrorMessage" runat="server" style="color:red;padding-top:3px;padding-bottom:10px"></div>
    <ej:Grid ID="FlatGrid" runat="server" AllowScrolling="true" >
        <Columns>
            <ej:Column Field="ContactId" HeaderText="Contact ID" TextAlign="Left" Width="70" />
            <ej:Column Field="FullName" HeaderText="Full Name" TextAlign="Left" Width="90" />
            <ej:Column Field="Age" HeaderText="Age" TextAlign="Left" Width="30" />
            <ej:Column Field="EmailAddress" HeaderText="Email Address" TextAlign="Left" Width="160" />
            <ej:Column Field="PhoneNo" HeaderText="Phone No" TextAlign="Left" Width="120" />
            <ej:Column Field="ModifiedDate" HeaderText="Modified Date" TextAlign="Left" Width="120" />
        </Columns>
        <ScrollSettings Height="300" Width="880" ></ScrollSettings>
    </ej:Grid>
</asp:Content>
