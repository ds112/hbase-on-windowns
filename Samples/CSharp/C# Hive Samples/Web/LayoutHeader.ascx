<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LayoutHeader.ascx.cs" Inherits="WebSampleBrowser.LayoutHeader" %>
<div class="header">
    <div class="sbheader" style="visibility: visible">
        <div class="title">
            <a title="BigData Management Studio for JavaScript" class="anchorclass site-title">
                <div title="Big Data Studio for ASP.NET HTML5" class="jslogo"></div>
            </a>
        </div>
       
        <div style="float: right;">
            <div class="wrapper-demo" title="Download Trial">
                <a href="http://www.syncfusion.com/downloads/aspnet" target="_blank" class="anchorclass">
                    <ej:Button ContentType="ImageOnly" ID="buybutton" Type="Button" PrefixIcon="e-uiLight e-icon-handup" Size="Normal" CssClass="float-right ejbutton downbutton metroblue" ClientIDMode="Static" runat="server" Width="66px" Height="55px" />
                </a>
            </div>
            <div class="themegallery" title="Select Theme">
                <ej:Dialog ID="sbtooltipbox" runat="server" CssClass="metroblue" Height="84" Width="176" EnableResize="false" ShowOnInit="false" ShowHeader="false" ClientIDMode="Static">
                    <DialogContent>
                        <div class="sbtooltip"></div>
                    </DialogContent>
                </ej:Dialog>

                <ej:Button ContentType="ImageOnly" ID="themebutton" Type="Button" PrefixIcon="e-uiLight e-icon-handup" Size="Normal" CssClass="ejbutton metroblue" ClientIDMode="Static" runat="server" Width="60px" Height="55px" ClientSideOnClick="themebtnClick" />

                <ej:Dialog ID="themeDialog" ClientIDMode="Static" runat="server" ShowHeader="false" ShowOnInit="false" EnableResize="false" Height="160px" CssClass="metroblue">
                    <DialogContent>
                        <div class="themestyle">
                            <ej:RadioButton ID="metrotext" Name="themestyle" CssClass="themestyleradio metroblue" Text="Flat" Size="Medium" runat="server" ClientSideOnChange="themeonchange" ClientIDMode="Static" Value="flat" />
                            <label for="metrotext" class="skin-name">Flat</label>
                            <ej:RadioButton ID="gradienttext" Name="themestyle" CssClass="themestyleradio metroblue" Text="Gradient" Size="Medium" runat="server" ClientSideOnChange="themeonchange" ClientIDMode="Static" Value="gradient" />
                            <label for="gradienttext" class="skin-name">Gradient</label>
                        </div>
                        <div class="themevarient">
                            <ej:RadioButton ID="lighttext" Name="themevarient" CssClass="themestyleradio metroblue" Text="Flat" Size="Medium" runat="server" ClientSideOnChange="themeonchange" ClientIDMode="Static" Value="light" />
                            <label for="lighttext" class="skin-name">Light</label>
                            <ej:RadioButton ID="darktext" Name="themevarient" CssClass="themestyleradio metroblue" Text="Flat" Size="Medium" runat="server" ClientSideOnChange="themeonchange" ClientIDMode="Static" Value="dark" />
                            <label for="darktext" class="skin-name">Dark</label>
                        </div>
                        <div class="themecolors">
                            <div id="themeazure" class="circlebaseouter colorsel">
                                <div class="circlebaseinner azure"></div>
                            </div>
                            <div id="themelime" class="circlebaseouter">
                                <div class="circlebaseinner lime"></div>
                            </div>
                            <div id="themesaffron" class="circlebaseouter">
                                <div class="circlebaseinner saffron"></div>
                            </div>
                        </div>
                        <div class="bootstraptheme">
                            <ej:CheckBox runat="server" ID="bootstrapcheck" Size="Small" ClientSideOnChange="bootstraponselect" />
                           
                            <label for="bootstrapcheck" class="skin-name">Bootstrap</label>
                        </div>
                    </DialogContent>
                </ej:Dialog>

            </div>
        </div>
    </div>
</div>

