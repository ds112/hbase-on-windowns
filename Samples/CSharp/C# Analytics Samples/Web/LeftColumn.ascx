<div class="accordion-panel cols-fixed-sidebar">
    <div class="search">
        <ej:Autocomplete runat="server" ID="auto" CssClass="autotext" ClientIDMode="Static" />
        <span class="navigation"></span>
    </div>
    <div id="scrollcontainer">
        <div>
           
            <div class="accordion" id="accordion2">
            </div>
            <script id="accordionTmpl" type="text/x-jsrender">
                                <div id="{{>id}}" class="anchorclass mainlevels" hashbang="/{{>controller}}/{{>action}}" >
                                 <a href={{:~location}}{{>id}}/{{>action}}.aspx>
                                    <div class="dashboard">
                                        <span class="anchor">{{>name}}</span>
                                        {{if type}} <span class="samplestatus {{>type}}"></span> {{/if}}
                                        <span class="arrow"></span>
                                    </div>
                                </a>
                                </div>
            </script>
            <div class="accordion samplesCollection" id="samplesDiv">
            </div>
            <script type="text/x-jsrender" id="accordionTmplchild">
                                <div id="{{>id}}" class="samples">
                                 <div id="{{>id}}_back" class="firstlevelback">
                                   <a href={{:~location}}Default.aspx class="dashboard">
                                        <span class="arrowback"></span><span class="anchor sbheading">Spark PMML Samples</span>
                                   </a>
                                 </div>
                              
                                    {{for samples ~pId=id}} 
                                        <div class="anchorclass subsamples samples">
                                            <div id="{{>id}}" class="firstlevelload" childcount="{{>childcount}}" controller="{{>~pId}}" action="{{>action}}">
                                                <a href=  {{:~location}}{{:#parent.parent.data.name}}/{{>action}}.aspx class="dashboard" >
                                                  <span class="anchor">{{>name}}</span>
                                                  {{if type}} <span class="samplestatus {{>type}}"></span> {{/if}}
                                                  {{if arrowclass}} <span class="{{>arrowclass}}"></span>{{/if}}
                                                </a>
                                               
                                                <div id="subControls">
                                                    <div class="firstlevelback  dashboardheader">                               
                                                        <a href={{:~location}}Default.aspx/{{:~sufurl}} class="dashboard">
                                                        <span class="arrowback"></span><span class="anchor sbheading">All Controls</span>
                                                        </a>                              
                                                    </div>
                                                    <div id="{{>id}}">
                                                        <div id="{{>id}}_back" class="secondlevelback dashboardheader">
                            
                                                        <a id="sec_back" href={{:~location}}{{>~pId}}/{{:#parent.parent.data.samples[0].action}}.aspx class="dashboard">
                                                        <span class="arrowback"></span><span class="anchor sbheading">{{>name}}</span>
                                                        </a>
                              
                                                        </div>
                                                       
                                                        {{for samples ~rId=~pId ~sId=name}}                                                                                                 
                                                            <div id="{{>id}}" hashbang="/{{>~sId}}/{{>action}}"  class="secondlevelload" childcount="{{>childcount}}" action="{{>action}}">
                                                                <a href={{:~location}}{{:#parent.parent.parent.parent.data.name}}/{{>action}}.aspx >
                                                                <div class="dashboard">
                                                                    <span class="anchor">{{>name}}</span>
                                                                    {{if type}} <span class="samplestatus {{>type}}"></span> {{/if}}
                                                                </div>
                                                                </a>
                                                            </div>
                                                        {{/for}}
                                                  </div>
                                               </div>
                                          </div>
                                       </div>
                                    {{/for}}
                                </div>
            </script>
            <div id="subsamplesDiv" style="display: none">
            </div>
        </div>
    </div>
</div>
