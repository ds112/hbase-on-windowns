<%@ Page Title="" Language="C#" MasterPageFile="~/Samplebrowser.Master"  ValidateRequest="false" CodeBehind="Default.aspx.cs" Inherits="WebSampleBrowser.LinearRegressionModel.Default" %>
<asp:Content ID="contentHeader" ContentPlaceHolderID="SampleHeading" runat="server">
    <span class="sampletitle">
        lpsa
    </span>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ControlsSection" runat="server">
            <div class="resultHeading"><%= Session["childHeading"]%></div>
                  <p>
    <strong>This sample demonstrate the usage of PMML execution engine for prediction based on the existing dataset and PMML file using LinearRegression Model.</strong>
</p>
     <div class="samplecontainer" runat="server" id="Sample">
            <div class="inner">
            <div id="outputresult" style="width:100%">
            <div>
                <ej:Tab ID="output" runat="server">
                    <Items>

                        <%--Grid Initialization--%>
                        <ej:TabItem Id="result" Text="Predicted Result">
                            <ContentSection>
                                <div class="tab_height" id="gridpadding">
                                     <ej:Grid ID="FlatGrid" runat="server" Height="300" Width="0" AllowScrolling="true" >
        <Columns>
            
            <ej:Column Field="1" HeaderText="target" TextAlign="Center" Width="140" />
            <ej:Column Field="2" HeaderText="field_0" TextAlign="Center" Width="140" />
            <ej:Column Field="3" HeaderText="field_1" TextAlign="Center" Width="140" />
            <ej:Column Field="4" HeaderText="field_2" TextAlign="Center" Width="140" />
            <ej:Column Field="5" HeaderText="field_3" TextAlign="Center" Width="140" />
            <ej:Column Field="6" HeaderText="field_4" TextAlign="Center" Width="140" />
            <ej:Column Field="7" HeaderText="field_5" TextAlign="Center" Width="140" />
            <ej:Column Field="8" HeaderText="field_6" TextAlign="Center" Width="140" />
            <ej:Column Field="9" HeaderText="field_7" TextAlign="Center" Width="140" />
            <ej:Column Field="0" HeaderText="Predicted" TextAlign="Center" Width="140" CssClass="predictedColumnColor"/> 
        </Columns>
        <ScrollSettings Height="300" Width="930"></ScrollSettings>
    </ej:Grid>
                                </div>
                            </ContentSection>
                        </ej:TabItem>
                        
                        <%--C# source code display--%>
                        <ej:TabItem Id="source" Text="C#">
                            <ContentSection>
                                <div class="tab_height">
                                    <pre id="run" class="brush: csharp;"/>                                
                                </div>
                            </ContentSection>
                        </ej:TabItem>

                        <%--RScript code display--%>
                        <ej:TabItem Id="spark" Text="Spark">
                             <ContentSection>
                                <div class="tab_height">
                                    <pre id="sparkCode" class="brush: perl;"/>                            
                                </div>
                            </ContentSection>
                        </ej:TabItem>

                        <%--PMML display--%>
                        <ej:TabItem Id="pmml" Text="PMML">
                            <ContentSection>
                                <div class="tab_height">
                                    <pre class="brush: perl;">
                                        <asp:Literal ID="Literal3" runat="server"/>
                                     </pre>
                                </div>
                            </ContentSection>
                        </ej:TabItem>

                    </Items>
                </ej:Tab>
            </div>
            </div>
        </div>
    </div>
        </div>
       <script type="text/javascript">

      
                $(document).ready(function () {
                    var browserDetails = getBrowserDetails();
                    var source = "<%= Session["source"]%>";
                    for (var i = 0; i < source.length; i++)
                        source = source.replace('^', '');
                    var spark = "<%= Session["spark"]%>";                   
                    for (var i = 0; i < spark.length; i++)
                        spark = spark.replace('^', '"');
                    if (browserDetails.browser === "msie" && browserDetails.version === "8.0") {
                        source = document.createTextNode(source);
                        rscript = document.createTextNode(rscript);
                    }
                    $("#run").html(source);
                    $("#sparkCode").html(spark);
                    
                    SyntaxHighlighter.highlight($("#sparkCode"));
                    SyntaxHighlighter.highlight($("#run"));
                    SyntaxHighlighter.highlight($("#Literal3"));
                });



    </script>

</asp:Content>
