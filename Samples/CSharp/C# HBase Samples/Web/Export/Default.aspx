<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Samplebrowser.Master" CodeBehind="Default.aspx.cs" Inherits="WebSampleBrowser.Export.Default" %>

<asp:Content ID="Content5" ContentPlaceHolderID="ControlsSection" runat="server">
    
    <script type="text/javascript">
        function getFormat(obj) {
            var frmt;
            frmt = $(obj).attr("value");
            frmt = frmt.substring(2, frmt.length);
            if (frmt == "Excel") {
                $("#divExcel").css("display", "block");
                $("#divWord").css("display", "none");
                $("#LayoutSection_ControlsSection_hdnGroup").val("Excel");
            }
            else if (frmt == "Word") {
                $("#divExcel").css("display", "none");
                $("#divWord").css("display", "block");
                $("#LayoutSection_ControlsSection_hdnGroup").val("Word");
            }
        }
    </script>
    <p><strong>This sample demonstrates the export of HBase data as Excel and Word documents</strong></p>
     <div id="ErrorMessage" runat="server" style="color:red;padding-top:3px;padding-bottom:10px"></div>
    <div>
        <asp:RadioButton runat="server" Text="&nbsp;Excel" Width="60" ID="rdExcel" GroupName="frmtgen" Checked="true" onclick="return getFormat(this)" />
        <asp:RadioButton runat="server" Text="&nbsp;Word" ID="rdWord" GroupName="frmtgen" onclick="return getFormat(this)" />
    </div>    
    <div id="divExcel" style="display: block">
        <asp:Label ID="label2" runat="server" Text="SaveAs:"></asp:Label><br />
        <table cellpadding="10">
            <tr>
                <td>
                   <asp:RadioButton runat="server" Text="&nbsp;Excel 2003"  ID="rBtn2003" GroupName="Excelgrp" /><br />
                </td>
                <td>
                    <asp:RadioButton runat="server" Text="&nbsp;Excel 2007" ID="rBtn2007" GroupName="Excelgrp" />
                 </td>
                <td>
                    <asp:RadioButton runat="server" Text="&nbsp;Excel 2010" ID="rbtn2010" GroupName="Excelgrp" />
                 </td>
                <td>
                   <asp:RadioButton runat="server" Text="&nbsp;Excel 2013" Checked="true" ID="rbtn2013" GroupName="Excelgrp" />
                </td>
                </tr>
            </table>
    </div>
    <div id="divWord" style="display: none">
        <asp:Label ID="label3" runat="server" Text="SaveAs:"></asp:Label><br />
        <table cellpadding="10">
            <tr>
                <td>
                    <asp:RadioButton runat="server" Text="&nbsp;Word 97-2003" ID="rBtnWord2003" GroupName="formatgrp" />
                 </td>
                <td>
                    <asp:RadioButton runat="server" Text="&nbsp;Word 2007" ID="rBtnWord2007" GroupName="formatgrp" />
                 </td>
                <td>
                    <asp:RadioButton runat="server" Text="&nbsp;Word 2010" ID="rbtnWord2010" GroupName="formatgrp" />
                </td>
                <td>
                     <asp:RadioButton runat="server" Text="&nbsp;Word 2013" Checked="true" ID="rbtnWord2013" GroupName="formatgrp" />
                 </td>
            </tr>
        </table>
    </div>
    <br />
    <asp:Button ID="Button1" runat="server" Text="Export" OnClick="Button1_Click" />
    
        <asp:HiddenField runat="server" ID="hdnGroup" Value="Excel" />
    
</asp:Content>

