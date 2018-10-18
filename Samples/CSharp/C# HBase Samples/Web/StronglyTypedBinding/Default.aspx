<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Samplebrowser.Master" CodeBehind="Default.aspx.cs" Inherits="WebSampleBrowser.StronglyTypedView.Default" %>

<asp:Content ID="Content5" ContentPlaceHolderID="ControlsSection" runat="server">
    <p><strong>This sample demonstrates, how to convert the Hive results to a strongly typed data and bind it to a grid for viewing</strong></p>
     <div id="ErrorMessage" runat="server" style="color:red;padding-top:3px;padding-bottom:10px"></div>
    <ej:Grid ID="FlatGrid" runat="server" AllowScrolling="true" >
        <Columns>
             <ej:Column Field="ContactId" HeaderText="ContactId" TextAlign="Left" Width="150" />
            <ej:Column Field="EmailId" HeaderText="EmailId" TextAlign="Left" Width="220" />
            <ej:Column Field="PhoneNumber" HeaderText="PhoneNumber" TextAlign="Left" Width="150" />
            <ej:Column Field="Age" HeaderText="Age" TextAlign="Left" Width="150" />
            <ej:Column Field="FullName" HeaderText="FullName" TextAlign="Left" Width="150" />
            <ej:Column Field="ModifiedDate" HeaderText="ModifiedDate" TextAlign="Left" Width="150" />
        </Columns>
        <ScrollSettings Height="300" Width="880" ></ScrollSettings>
    </ej:Grid>
</asp:Content>
