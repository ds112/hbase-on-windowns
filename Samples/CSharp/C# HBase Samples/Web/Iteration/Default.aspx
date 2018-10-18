<%@ Page Title="" Language="C#" MasterPageFile="~/Samplebrowser.Master"  ValidateRequest="false" CodeBehind="Default.aspx.cs" Inherits="WebSampleBrowser.RichTextBox.Default" %>

<asp:Content ID="Content5" ContentPlaceHolderID="ControlsSection" runat="server">
     <div >   
         <p><strong> This sample demonstrates, how to fetch the records from the HBase and iterate each tuple from the result and display the records.</strong></p>   
          <div id="ErrorMessage" runat="server" style="color:red;padding-top:3px;padding-bottom:10px"></div>      
              <ej:RTE ID="rteControl" runat="server" ShowToolBar="false">
                  <RTEContent>
                      
                  </RTEContent>
              </ej:RTE>
        </div>
</asp:Content>
