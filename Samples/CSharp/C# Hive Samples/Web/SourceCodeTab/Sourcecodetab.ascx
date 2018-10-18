<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Sourcecodetab.ascx.cs"
    Inherits="WebSampleBrowser.Sourcecodetab" %>
<%@ Register Assembly="WebSampleBrowser" Namespace="AspSamplebrowser" TagPrefix="Sync" %>
<Sync:SourceCodeTab ID="codetabview" runat="server" />

<script type="text/javascript">
    $(document).ready(function () {
        $("#SourceTabDiv ul li a").each(function () {
            var hrefvalue = $(this).attr("href");
            hrefvalue = hrefvalue.replace("SourceCodeTab/", "");
            hrefvalue = hrefvalue.replace("../", "");
            $(this).attr("href", hrefvalue);
        });
        $("#SourceTabDiv").ejTab();
        $(".cols-source.sourcecodeTab ").addClass("selectable");
        $("<span>").attr("id", "newcodewindow").addClass("newwindow codewindow").appendTo($("#SourceTabDiv .e-header"));
       
        $("#newcodewindow").click(function () {
            var popupWin = window.open();
            var headcontent = $(document.head).html();
            var script = "<" + "/" + "script" + ">";
            if (headcontent.indexOf('sampleBrowserSite') > 0) headcontent = headcontent.replace('<script src="../Scripts/sampleBrowserSite.js" type="text/javascript">' + script)
            headcontent = headcontent.replace('undefined', '');
            popupWin.document.writeln('<html>' + headcontent + '<body><div id="windowOpen"><div class="cols-source sourcecodeTab">' + $(".cols-source.sourcecodeTab").html() + '</div></div><script type="text/Javascript">$(function(){ $("#SourceTabDiv").ejTab(); $(".newwindow").each(function(){$(this).remove();});});' + script + '</body></html>');
            popupWin.document.close();
        
        });
    });
    

</script>

