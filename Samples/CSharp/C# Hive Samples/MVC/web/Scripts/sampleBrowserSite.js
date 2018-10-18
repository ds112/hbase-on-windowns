var slider1 = null, slider2 = null, oldthemepath = null, SamplesList = null, editcontrolpath = null, selthemecolor = null, cssheight = null; window.theme = "saffron", minScrollerHeight = 550;
window.themecolor = ""; window.themestyle = ""; window.themevarient = "";

$(function () {
    var sbModel = new ViewModel(), isloadLeft = false;
    this.PathLoc = null, this.themeonchange=false;
    window.theme = "saffron";
    //debugger
    if ((readCookie('themeName') != null) && (readCookie('themeName') != ""))
        window.theme = readCookie('themeName');
    if ((readCookie('themeColor') != null) && (readCookie('themeColor') != ""))
        window.themecolor = readCookie('themeColor');
    $(".htmljssamplebrowser").attr("class", "htmljssamplebrowser " + window.themecolor);
    if ((readCookie('themeStyle') != null) && (readCookie('themeStyle') != ""))
        window.themestyle = readCookie('themeStyle');
    if ((readCookie('themeVarient') != null) && (readCookie('themeVarient') != ""))
        window.themevarient = readCookie('themeVarient');
    if (window.themevarient == 'dark') $("body").attr("class", window.themevarient + "theme"); else $("body").attr("class", "");
    $(function () {
        var control = "", categories = [], currentSample, theme = "";
        var urlread = window.location.toString(), cur_sample = "";

        for (i = 0; i < window.SampleControls.length; i++) {
            var controlName = window.SampleControls[i].name;
            if (((urlread.indexOf("/" + controlName) == urlread.length - controlName.length - 1) && (urlread.indexOf("/" + controlName) >= 0)) || (urlread.indexOf("/" + controlName + "/") >= 0)) {
                control = controlName;
                cur_sample = urlread.substr(urlread.lastIndexOf("/") + 1, urlread.length);
                if (cur_sample == control)
                    cur_sample = window.SamplesList.filter(function (e) { if (e.id == control) return e })[0].action
                break;
            }
        }

        categories.push(cur_sample);
        isloadLeft = true;
        currentSample = categories.pop();
        if (control !== "")
            loadSamplePage(control, currentSample, categories);
        updateSBTheme();
        updateTheme(window.theme);
        //replaceTheme();
    });
    var updateSBTheme = function () {
        if (window.theme.indexOf("lime") != -1) {
            $(".htmljssamplebrowser").attr("class", "htmljssamplebrowser " + "lime");
            $("#themebutton").ejButton('option', 'prefixIcon', 'e-icons ' + "lime");
        }
        else if (window.theme.indexOf("saffron") != -1) {
            $(".htmljssamplebrowser").attr("class", "htmljssamplebrowser " + "saffron");
            $("#themebutton").ejButton('option', 'prefixIcon', 'e-icons ' + "saffron");
        }
    };
    var loadSamplePage = function (control, currentSample, categories) {
        if (control == null) {
            $("#samplesDiv").css("display", "none");
            $(".samples").css("display", "none");
            self.loadSBMainPage(null);
            return;
        }

        var sample = findSample(control, currentSample, categories), name = sample, parentId = null;

        while (name.samples) {
            if (name.samples) {
                parentId = parseInt(name.id) ? name.id : null;
                name = name.samples[0];
                currentSample = name.action;
            } else
                break;
        }


        if ($("#sbtooltipbox").data("ejDialog"))
            $("#sbtooltipbox").ejDialog("close");

        if (isloadLeft || Number(name.id) <= 1)
            self.loadLeftTab(control, currentSample);
        var sampleurl = "/" + control + "/" + currentSample;
        self.loadSamplePage(sampleurl, control, currentSample, parentId, name.id);
    };

    var findSample = function (control, currentSample, categories) {
        var currentcontrollist = null, subcontrollist = null;
        $(SamplesList).each(function () {
            if ($(this)[0].id == control) {
                currentcontrollist = $(this);
            }

        });

        $($(currentcontrollist)[0].samples).each(function () {
            if ($(this)[0].samples) {
                for (var i = 0; i < $(this)[0].samples.length; i++) {
                    if ($(this)[0].samples[i].action == currentSample) {
                        categories.push($(this)[0].name);
                        break;
                    }
                }
            }

        });

        var sample = ej.Query().using(ej.DataManager(SamplesList)).where("id", "==", control), current = sample, res;

        for (var k = 0; k < categories.length; k++) {
            current.hierarchy((current = ej.Query("samples").where("name", "==", categories[k])));
        }

        if (currentSample)
            current.hierarchy(ej.Query("samples").where("action", "==", currentSample));

        res = sample.executeLocal();

        if (res.length) res = res[0];
        return res;
    };

    $("#scrollcontainer").on("click", ".secondlevelload, .anchorclass", function (e) {
        var hashvalue = $(e.currentTarget).attr('hashbang');
        var categorieslist = hashvalue.slice(1, hashvalue.lastIndexOf("/"));
        eraseCookie("catalog");
        createCookie('catalog', categorieslist, 1);
        var viewportWidth = $(window).width();
        if (viewportWidth < 981) {
            $('.content-container-fluid .row > .navigation').addClass('collapsePanel');
            $('.accordion-panel').removeClass("expandPanel");
        }
    });

   
    $("#themebutton").ejButton({
        size: "normal",
        width: "60px",
        height: "55px",
        cssClass: "metroblue",
        click: "themebtnClick",
        contentType: "imageonly",
        prefixIcon: "e-uiLight e-icon-handup"
    });
    $("#buybutton").ejButton({
        size: "normal",
        width: "66px",
        height: "55px",
        cssClass: "metroblue",
        contentType: "imageonly",
        prefixIcon: "e-uiLight e-icon-handup"
    });
    $("#sbtooltipbox").ejDialog({ height: "84px", width: "176px", enableResize: false, showOnInit: false, showHeader: false, cssClass: "metroblue" });
    $("#themeDialog").ejDialog({ height: "160px", enableResize: false, showOnInit: false, showHeader: false, cssClass: "metroblue" });
    $("#metrotext").ejRadioButton({ text: "Flat", value: "flat", size: "medium", name: "themestyle", cssClass: "metroblue", change: 'themeonchange' });
    $("#gradienttext").ejRadioButton({ cssClass: "metroblue", size: "medium", name: "themestyle", text: "Gradient", change: 'themeonchange' });
    $("#lighttext").ejRadioButton({ text: "Light", size: "medium", name: "themevarient", cssClass: "metroblue", change: 'themeonchange' });
    $("#darktext").ejRadioButton({ cssClass: "metroblue", text: "Dark", name: "themevarient", size: "medium", change: 'themeonchange' });
    $("#bootstrapcheck").ejCheckBox({ size: "small", change:"bootstraponselect" });
    themeButtonSelect();
    var metroradio = $("#JobSearch").data("ejMenu");
    var firstlevelsamples = [];

    var isContentPageLoaded = null;
    var index = 0;

    function ViewModel() {
        this.Controls = SamplesList;
        this.controlname = "";
        this.controlName = null;
        this.sampleName = null;

        self.editSubItem = function (id, back) {
            var divid = id;
            removeSelectedItemcss();
            $("#subsamplesDiv").html('');
            $("#" + divid).css("margin-top", "0px");
            $("#subsamplesDiv").hide().css("left", "250px");
            $("#samplesDiv").css("left", "0px").show();
            $("#" + divid).show().css("visibility", "visible");
            $("#" + divid + "_back").css("display", "block");
            $("#" + divid + "_back").addClass("dashboarddiv");
            $("#" + divid).children(".subsamples").find("a >div").removeClass("dashboardhover");
            $("#samplesDiv").find("#" + divid).css("left", "-250px");
            $("#samplesDiv").find("#" + divid).children(".subsamples").show();
            $("#samplesDiv").find("#" + divid).animate({ left: '0px' }, 200);
            var samplename = null, controlname = null;
       
        };

        self.loadLeftTab = function (divid) {
            if ($("#" + divid).prev().length > 0) {
                $("#" + divid).css("margin-top", "0px");
            }
            $("#accordion2").css("left", "-250px");
            $("#accordion2").prev().css("display", "none");
            $("#accordion2").css("display", "none");
            $("#subsamplesDiv").css("display", "none");
            $(".samples").hide();
            $("#samplesDiv").children("#" + divid).css("display", "block");
            $("#samplesDiv").children("#" + divid).children(".subsamples").show();
            $("#samplesDiv").children("#" + divid).children(".subsamples").find("#subControls").hide();
            $("#" + divid + "_back").css("display", "block");
            $("#" + divid + "_back").addClass("dashboardheader");
            $("#samplesDiv").css("display", "block");
            $("#samplesDiv").css("margin-top", "0px");
            $("#samplesDiv").animate({ left: '0px' }, 0);
            $('html, body').animate({
                scrollTop: 0
            }, 0);
            $(".accordion-panel").height($(".accordion-panel")[0].scrollHeight);
           
        };


        self.findSample = function (ctrlname, samplename, subchild, currentsampleid) {
            var query = ej.Query().using(ej.DataManager(SamplesList))
                .where("id", "==", ctrlname), curr = query, res;

            if (subchild)
                query.hierarchy(
                    curr = ej.Query("samples")
                        .where("id", "==", subchild));

            curr.hierarchy(
                ej.Query("samples")
                    .where("id", "==", currentsampleid)
            );

            res = query.executeLocal();

            if (res.length) res = res[0];
            return res;

        },
        self.loadSamplePage = function (url, ctrlname, samplename, subchild, currentsampleid) {
            currentSamplepage = url;
            window.currentSamplepage = url;
            var sample = self.findSample(ctrlname, samplename, subchild, currentsampleid), sampleTitle = "",samplePath="";
            if ($("#auto").data("ejAutocomplete"))
                $("#auto").ejAutocomplete("clearText");
            $(".sampleheadingtext").empty();
            
            while (sample) {
                if (sampleTitle)
                    sampleTitle += " / ";

                sampleTitle += sample.displayName;

                sample = sample.samples && sample.samples[0];
            }

            var names = sampleTitle.split("/"), _samplename = names.pop();
            var _halfTitle = names.join("/") + "/ ";
            var links = $(document.head || document.getElementsByTagName('head')[0]).find("link");
            var serverconfig = links[0].href.substr(0, links[0].href.indexOf("Content") - 1);
            var sampletitleobj = ej.buildTag("div.samplename sbsamplename");
            //ej.buildTag("span", _halfTitle).appendTo(sampletitleobj);
            ej.buildTag("span.sampletitle", _samplename).appendTo(sampletitleobj);
            var navigation = ej.buildTag("span.navigation-btn");
            var paclk = prevnext(ctrlname, samplename, 'prev');
            var ancprev = ej.buildTag("a.navprev", "", {}, { href:serverconfig+ paclk }).appendTo(navigation);
            var naclk = prevnext(ctrlname, samplename, 'next');
            var ancnext = ej.buildTag("a.navprev", "", {}, { href: serverconfig+naclk }).appendTo(navigation);
            var prevState = ej.buildTag("span.prev", "Prev").appendTo(ancprev);
            var nextstate = ej.buildTag("span.next", "Next ").appendTo(ancnext);
            navigation.appendTo(sampletitleobj);
            $(".sampleheadingtext:first").html(sampletitleobj);
            editcontrolpath = "/" + samplename;
            document.title = "BigData Management Studio for ASP.NET MVC : " + sampleTitle.replace(/\//g, "-") + " Demo";
            index = 0;
            
            if (subchild != null) {
                $("#samplesDiv").children("#" + ctrlname).children(".subsamples").children("#" + subchild).children("#subControls").children()[0].id = ctrlname;
                var divtoappend = $("#samplesDiv").children("#" + ctrlname).children(".subsamples").children("#" + subchild).children("#subControls").children().clone(true);
                $("#subsamplesDiv").html('');
                $(divtoappend).appendTo($("#subsamplesDiv"));
                $("#" + ctrlname).children(".dashboard").removeClass("dashboardhover");
                $("#samplesDiv").hide().css("left", "-250px");
                $("#subsamplesDiv").css("margin-top", "0px");
                if ($("#subsamplesDiv").find(".itemselected").length > 0) {
                    $("#subsamplesDiv").find(".itemselected").removeClass("itemselected");
                    $("#subsamplesDiv").find(".itemselected").parent().removeClass("highlighttextbg");
                }
                $("#subsamplesDiv").show();
                $("#subsamplesDiv").children(".firstlevelback").children(".anchor").addClass("sbheading").text("All Controls");
                $("#samplesDiv").children("#" + ctrlname).children(".subsamples").hide();
                $("#subsamplesDiv").find(".highlighttextbg").removeClass("highlighttextbg");
                $("#subsamplesDiv").children(".anchor").show();
                $("#subsamplesDiv").children(".anchor").children().removeClass("hideParent").addClass("dashboard");
                $("#subsamplesDiv").children("#" + subchild).show();
                $("#subsamplesDiv").find("#" + subchild + "_back").find('span').last().text(ctrlname.charAt(0).toUpperCase() + ctrlname.slice(1));
                $("#subsamplesDiv").children(".anchor").children().addClass("dashboardheader")
                $("#subsamplesDiv").children().last().children("#" + currentsampleid).find(".dashboard").find(".anchor").addClass("itemselected");
                $("#subsamplesDiv").children().last().children("#" + currentsampleid).find(".dashboard").addClass("highlighttextbg");
                $("#subsamplesDiv").animate({ left: '0px' }, 0);
                $('div.secondlevelback').bind('click', function (evt, args) {
                    editSubItem(ctrlname, 'back');
                });
            }

            $(".prev").ejButton({
                size: "mini",
                cssClass: "metroblue",
                contentType: "imageonly",
                prefixIcon: "e-rarrowleft-2x"
            });
            $(".next").ejButton({
                size: "mini",
                cssClass: "metroblue",
                contentType: "imageonly",
                prefixIcon: "e-rarrowright-2x"
            });

            var curr;
            if (curr = ($("#samplesDiv").children("#" + ctrlname).children('.subsamples').find("div[action=" + samplename + "]"))) {
                if ($(curr).attr("childcount") == 0 && !$(curr).hasClass('secondlevelload')) {
                    $("#samplesDiv").children("#" + ctrlname).children('.subsamples').find("div[action=" + samplename + "]").find('span.anchor').addClass("itemselected");
                    $("#samplesDiv").children("#" + ctrlname).children('.subsamples').find("div[action=" + samplename + "]").find('span.anchor').parent('.dashboard').addClass("highlighttextbg");
                }
                else {
                    $("#samplesDiv").children("#" + ctrlname).children('.subsamples').find("div[action=" + samplename + "]" + '.secondlevelload').find('span.anchor').addClass('itemselected');
                    $("#samplesDiv").children("#" + ctrlname).children('.subsamples').find("div[action=" + samplename + "]" + '.secondlevelload').find('span.anchor').parent('.dashboard').addClass("highlighttextbg");
                }
            }
          
        };


        self.loadSBMainPage = function (divid) {
            $(".samplesection").hide();
            if (divid != null) {
                $("#" + divid).css("visibility", "hidden");
                $("#" + divid + "_back").css("display", "none");
            }
            self.loadSBPage(divid);
            $(".accordion-panel").height($(".accordion-panel")[0].scrollHeight);
        };
        self.loadSBPage = function (divid) {
            $("#accordion2").prev().css("display", "block");
            $("#accordion2>a>.dashboardhover").removeClass("dashboardhover");
            $(".sourcecodetab").hide();
            if (divid != null)
                $("#" + divid).hide();
            $("#accordion2").show();
            $("#samplesDiv").css("left", "250px");
            $("#sbdashboard").addClass("highlighttextbg itemselected");
            $("#accordion2").animate({ left: 0 }, 200, function () {
                //self.setVisibility("cols-iframe", "productpage");
            });
            $(".accordion-panel").height($(".content-container-fluid").height() - ($(".htmljssamplebrowser>.header").height() + 5));
            $('html, body').animate({
                scrollTop: 0
            }, 0);
            $(".accordion-panel").height($(".accordion-panel")[0].scruollHeight);
        };

        self.setVisibility = function (oldpage, newpage) {
            $("." + newpage).show();
            $("." + oldpage).hide();
        };
        this.getCssClass = function (item) {
            var value = Number(item.childcount);

            if (value >= 1) {
                return 'arrow';
            }
            return;
        };
        this.getCssVisibility = function (item) {
            var value = Number(item.id);

            if (value != 1) {
                return 'hideParent';
            }
            return 'dashboard';
        };
        self.removeSelectedItemcss = function (ctrlname, controlid) {
            var obj = $(".itemselected");
            obj.each(function () {
                $(obj).removeClass('itemselected');
                $(obj).parent().removeClass('highlighttextbg');
            });
        };
    }

    $("#accordion2").html($("#accordionTmpl").render(SamplesList));

    var samplesdetails = SamplesList;
    $(samplesdetails).each(function (index1, sampleslist) {
        var smpls = {}, secsamples = [];
        smpls["id"] = sampleslist.id;
        smpls["name"] = sampleslist.name;
        smpls["type"] = sampleslist.type;
        $(sampleslist.samples).each(function (ind, subsampleslist) {
            var subsmpls = {};
            subsmpls["id"] = subsampleslist.id;
            subsmpls["name"] = subsampleslist.name;
            subsmpls["controller"] = subsampleslist.controller;
            subsmpls["action"] = subsampleslist.action;
            subsmpls["childcount"] = subsampleslist.childcount;
            subsmpls["type"] = subsampleslist.type;
            if (subsampleslist.childcount == 1)
                subsmpls["arrowclass"] = "arrow";
            else
                subsmpls["arrowclass"] = "";
            subsmpls["samples"] = subsampleslist.samples;
            secsamples.push(subsmpls);
        });
        smpls["samples"] = secsamples;
        firstlevelsamples.push(smpls);
    });
    $("#samplesDiv").html($("#accordionTmplchild").render(firstlevelsamples));
    $('div.firstlevelback').bind('click', function (evt, args) {
        $("#subsamplesDiv").hide().css("left", "250px");
    });

    $(".dashboard").mouseover(function () {
        if (!$(".vHandle").is(":visible")) {
            if ($('input[name=themevarient]:checked').length > 0 && $('input[name=themevarient]:checked')[0].id == "darktext" && !$(this).hasClass("dark-highlighttextbg"))
                $(this).addClass("dark-dashboardhover");
            else
                $(this).addClass("dashboardhover");
        }
    });
    $(".dashboard").click(function () {
        $(".anchor").removeClass("itemselected");
        $(this).find(".anchor").addClass("itemselected");
        $(".dark-dashboard").removeClass("dark-highlighttextbg");
        $(".dashboard").removeClass("highlighttextbg");
        $(".dashboard").removeClass("itemselected");
        $(this).find(".itemselected").parent().addClass("highlighttextbg");
    });

    $(".dashboard").mouseout(function () {
        if ($(this).hasClass("dashboardhover"))
            $(this).removeClass("dashboardhover");
        else if ($(this).hasClass("dark-dashboardhover"))
            $(this).removeClass("dark-dashboardhover");
    });

    //autocomplete selection list
    var collection = SamplesList;
    var subdata = [], i, j, k;
    for (i = 0; i < collection.length; i++) {
        for (j = 0; j < collection[i].samples.length; j++) {
            var properties = {}, len;
            var controlName = collection[i].controller;
            properties["id"] = controlName + "/" + collection[i].samples[j].action;
            properties["text"] = collection[i].name + "-" + collection[i].samples[j].name;
            if (collection[i].samples[j].childcount != "0") {
                if (/msie 8.0/.test(navigator.userAgent.toLowerCase()))
                    len = collection[i].samples[j].samples.length - 1;
                else
                    len = collection[i].samples[j].samples.length;
                if (collection[i].samples[j].name == "Selection")
                    var s = true;
                for (k = 0; k < len; k++) {
                    var subprops = {};
                    var subcontrolName = collection[i].samples[j].name;
                    subprops["id"] = controlName + "/" + collection[i].samples[j].samples[k].action;
                    subprops["text"] = collection[i].name + "-" + collection[i].samples[j].name + "-" + collection[i].samples[j].samples[k].name;
                    subdata.push(subprops);
                }
            }
            subdata.push(properties);
        }
    }
    $('#auto').ejAutocomplete({
        dataSource: subdata,
        fields: { key: "id", text: "text" },
    });
    //

    $('.navigation').bind('click', function () {
        var move, proxy = this;
        if ($(this).hasClass("expandPanel")) {
            $('.navigation').removeClass("expandPanel").addClass("collapsePanel"), move = -$('.accordion-panel').outerWidth();
            $('.control-panel.cols-content-fluid').addClass('center-flow');
            $('content-container-fluid > row > .navigation').addClass("expandPanel");
        }
        else {
            $(proxy).removeClass("expandPanel");
            $('.accordion-panel').removeClass('accordionHide').addClass("expandPanel");
            $('.control-panel.cols-content-fluid').removeClass('accordionHide').removeClass('center-flow');
            $('.accordion-panel > .navigation').addClass("expandPanel");
            $('.navigation').removeClass("collapsePanel"), move = 0;
            if (parseInt($('.accordion-panel').css('left')) >= 0) {
                $('.accordion-panel').css('left', '-' + $('.accordion-panel').outerWidth() + 'px');
            }
            $('.accordion-panel .navigation').addClass("expandPanel")
        }
        $('.accordion-panel').animate({
            "left": move + "px",
        }, 500, function () {
            if ($('.navigation').hasClass("collapsePanel")) {
                $('.accordion-panel').addClass('accordionHide');
                $('.navigation').show();

            }
            if ($(proxy).hasClass("expandPanel")) {
                $('.accordion-panel').addClass("expandPanel");
            }
        });
    });
    $(window).bind("resize", function () {
        var viewportWidth = $(window).width();
        if (viewportWidth < 981) {
            $('.content-container-fluid .row > .navigation').addClass('collapsePanel');
            $('.accordion-panel').removeClass("expandPanel");
        }
    });
   

    $(document).click(function (evt) {
        if ($("#sbtooltipbox").ejDialog("isOpened"))
            $("#sbtooltipbox").ejDialog("close");
        var ele = false;
        var chromebrowser = /chrome/.test(navigator.userAgent.toLowerCase());
        var safaribrowser = /safari/.test(navigator.userAgent.toLowerCase());
        if (chromebrowser || safaribrowser)
            if(evt.target && evt.target.parentElement != null && evt.target.parentElement.parentElement != null && evt.target.parentElement.parentElement.id != null && evt.target.parentElement.parentElement.id == "themebutton")
            
            if (evt.target && evt.target.parentElement!=null && evt.target.parentElement.parentElement!=null && evt.target.parentElement.parentElement.id != null && evt.target.parentElement.parentElement.id == "themebutton")
                ele = true;
        if (!ele && $(evt.target).parents("#themeDialog_wrapper").length <= 0 && evt.target.id != "themebutton") {
            if ($("#themeDialog").ejDialog("isOpened"))
                $("#themeDialog").ejDialog("close");
        }
    });
   
    $(".circlebaseouter").mouseover(function (evt) {
        $(".hoverselcolor").removeClass("hoverselcolor");
        $(this).addClass("hoverselcolor");
    });
    $(".circlebaseouter").mouseout(function (evt) {
        $(".hoverselcolor").removeClass("hoverselcolor");
    });
    $(".circlebaseouter .azure,.circlebaseouter .lime,.circlebaseouter .saffron").click(function (evt) {
        $(".colorsel").removeClass("colorsel");
        $(this).parent().addClass("colorsel");
        var selcolor = $(this).parent().attr("id");
        window.themecolor = selcolor.replace("theme", "");
        updateTheme();
    });
      
});


function preparetheme() {
    var themename = "";
    if (window.theme.indexOf('dark') != -1 && window.themevarient != 'light')
        window.themevarient = "dark";
    if (window.theme.indexOf('gradient') != -1 && window.themestyle != 'flat')
        window.themestyle = "gradient";
    if (window.themevarient == "dark") {
        if (window.themestyle == "gradient")
            themename = window.themestyle + window.themecolor + window.themevarient;
        else
            themename = (window.themecolor != "") ? window.themecolor + window.themevarient : window.themestyle + window.themevarient;

    }
    else {
        if (window.themestyle == "gradient")
            themename = window.themestyle + window.themecolor;
        else
            themename = (window.themecolor != "") ? window.themecolor : window.themestyle;
    }
    if (window.theme.indexOf( "bootstrap")>=0)
        themename = window.theme;
    window.theme = themename;
}

function updateTheme(selcolor) {
    preparetheme();
    replacebg(window.themevarient == "dark");
    replaceTheme();
}
var themes = {
    "flat": "/Content/ej/default-theme/ej.theme.min.css",
    "flatdark": "/Content/ej/flat-azure-dark/ej.theme.min.css",
    "azure": "/Content/ej/default-theme/ej.theme.min.css",
    "azuredark": "/Content/ej/flat-azure-dark/ej.theme.min.css",
    "lime": "/Content/ej/flat-lime/ej.theme.min.css",
    "limedark": "/Content/ej/flat-lime-dark/ej.theme.min.css",
    "saffron": "/Content/ej/flat-saffron/ej.theme.min.css",
    "saffrondark": "/Content/ej/flat-saffron-dark/ej.theme.min.css",
    "gradient": "/Content/ej/gradient-azure/ej.theme.min.css",
    "gradientdark": "/Content/ej/gradient-azure-dark/ej.theme.min.css",
    "gradientazure": "/Content/ej/gradient-azure/ej.theme.min.css",
    "gradientazuredark": "/Content/ej/gradient-azure-dark/ej.theme.min.css",
    "gradientlime": "/Content/ej/gradient-lime/ej.theme.min.css",
    "gradientlimedark": "/Content/ej/gradient-lime-dark/ej.theme.min.css",
    "gradientsaffron": "/Content/ej/gradient-saffron/ej.theme.min.css",
    "gradientsaffrondark": "/Content/ej/gradient-saffron-dark/ej.theme.min.css",
    "bootstrap": "/Content/ej/bootstrap-theme/ej.theme.min.css"
};

function replaceTheme() {
    if ((window.theme.indexOf("lime") != -1) || (window.theme.indexOf('lime') != -1 && window.themecolor != 'azure' && window.themecolor != "saffron"))
        window.themecolor = 'lime';
    else if ((window.theme.indexOf('saffron') != -1) || (window.theme.indexOf('saffron') != -1 && window.themecolor != 'azure' && window.themecolor != 'lime'))
        window.themecolor = 'saffron';
    else
        window.themecolor = "azure";
    var selcolor = (window.themecolor == "") ? "saffron" : window.themecolor;
    $("#themebutton").ejButton('option', 'prefixIcon', 'e-icons ' + selcolor);
    $("#themebutton .e-uiLight").addClass(selcolor);
    $(".htmljssamplebrowser").attr("class", "htmljssamplebrowser " + selcolor);
    var urlread = window.location.toString();
    var links = $(document.head || document.getElementsByTagName('head')[0]).find("link");
    for (var i = 0; i < links.length; i++) {
        if (links[i].href.indexOf("ej.theme.min.css") != -1) {
            var cssref = links[i].href,serverconfig = cssref.substr(0, cssref.indexOf("Content")-1),
			fileref = $('<link rel="stylesheet" type="text/css" href="' + serverconfig + themes[theme] + '" />');
            $(fileref).insertAfter(links[i]);
            $(links[i]).remove();
            break;
        }
    }
    //gauge chart and others
    if (urlread.indexOf("Gauge") >= 0) {
        loadGaugeTheme();
    }
    else if ((urlread.indexOf("Chart") >= 0) || (urlread.indexOf("Client") >= 0) || (urlread.indexOf("RangeNavigator") >= 0)) {
        loadTheme(undefined);
    }
    else if (urlread.indexOf("Graph") >= 0) {
        loadBulletTheme();
    }
    eraseCookie("themeName");
    createCookie('themeName', window.theme, 1);
    eraseCookie("themeColor");
    createCookie('themeColor', window.themecolor, 1);
    eraseCookie("themeStyle");
    createCookie('themeStyle', window.themestyle, 1);
    eraseCookie("themeVarient");
    createCookie('themeVarient', window.themevarient, 1);
}
function replacebg(enable) {
    if (enable)
        $("body").addClass("darktheme");
    else
        $("body").removeClass("darktheme");
}

function themebtnClick(args) {
    showThemeDialog();
}
function showTooltipbox() {
    var $btnelement = $("#themebutton");
    var pos = $btnelement.offset();
    var left = $("#sbtooltipbox_wrapper").width() + pos.left;
    if (left > $(".samplecontainerparent").width())
        left = (pos.left + ($btnelement.width() / 2)) - $("#sbtooltipbox_wrapper").width();
    else
        left = pos.left;
    $("#sbtooltipbox").ejDialog("option", "position", { X: left + 10 });
    $("#sbtooltipbox").ejDialog("open");
}
function showThemeDialog() {
    var $btnelement = $("#themebutton");
    var pos = $btnelement.offset();
    var left = $("#themeDialog_wrapper").width() + pos.left;
    if (left > $(".samplecontainerparent").width())
        left = (pos.left + $btnelement.width()) - $("#themeDialog_wrapper").width();
    else
        left = pos.left;
    $("#themeDialog").ejDialog("option", "position", { X: left });
    $("#themeDialog").ejDialog("open");
   
}
function themeonchange(args) {
    switch (args.model.name) {
        case "themestyle":
            window.themestyle = args.model.value;
            break;
        case "themevarient":
            window.themevarient = args.model.value;
            break;
    }
    updateTheme(window.theme);
        //radioButtonSelect();
}
function bootstraponselect(args) {
    if (args.isChecked) {
        window.theme = "bootstrap";
        updateTheme(window.theme);
    }
    else {
        window.theme = "saffron";
        window.themecolor = "saffron";
        eraseCookie('themeName');
        createCookie('themeName', window.theme, 1);
        eraseCookie('themeColor');
        createCookie('themeColor', window.themecolor, 1);
        themeButtonSelect();
        updateTheme(window.theme);
    }
    
    
}
function radioButtonSelect() {
    $('#ejmetrotext,#ejgradienttext,#ejlighttext,#ejdarktext').children('span').children('span').removeClass('e-rad-active');
    if (window.themestyle.indexOf("gradient") != -1) {
        $('#ejgradienttext').children('span').children('span').addClass('e-rad-active');
        window.themestyle = 'gradient';
    }
    else {
        $('#ejmetrotext').children('span').children('span').addClass('e-rad-active');
        window.themestyle = 'flat';
    }
    if (window.themevarient.indexOf("dark") != -1) {
        $('#ejdarktext').children('span').children('span').addClass('e-rad-active');
        window.themevarient = 'dark';
    }
    else  {
        $('#ejlighttext').children('span').children('span').addClass('e-rad-active');
        window.themevarient = 'light';
    }
    if (window.theme == "bootstrap") {
        $("#bootstrapcheck").ejCheckBox("option", "checked", true);
    }

}
function themeButtonSelect() {
    //
    if ((readCookie('themeName') != null) && (readCookie('themeName') != ""))
        window.theme = readCookie('themeName');
    if ((readCookie('themeColor') != null) && (readCookie('themeColor') != ""))
        window.themecolor = readCookie('themeColor');
    if ((readCookie('themeStyle') != null) && (readCookie('themeStyle') != ""))
        window.themestyle = readCookie('themeStyle');
    if ((readCookie('themeVarient') != null) && (readCookie('themeVarient') != ""))
        window.themevarient = readCookie('themeVarient');
    //
    $('#themesaffron,#themeazure,#themelime').removeClass('colorsel');
    if ((window.theme.indexOf("lime") != -1) || (window.theme.indexOf('lime') != -1)) {
        $('#themelime').addClass('colorsel');
        window.themecolor = 'lime';
    }
    else if ((window.theme.indexOf("saffron") != -1) || (window.theme.indexOf('saffron') != -1)) {
        $('#themesaffron').addClass('colorsel');
        window.themecolor = 'saffron';
    }
    else {
        $('#themeazure').addClass('colorsel');
        window.themecolor = 'azure';
    }
    radioButtonSelect()
}

//next previous location
function prevnext(name, cursample, navloc) {
    var cursampitem, prevsampitem, nextsampitem, cursamplist, prevsamplist, nextsamplist, chx, tempsample, cursampquery, anchref;
    for (i = 0; i < SamplesList.length; i++) {
        chx = SamplesList[i].id;
        if (chx == name) {
            cursampitem = SamplesList[i];
            prevsampitem = SamplesList[i - 1];
            nextsampitem = SamplesList[i + 1];
            break;
        }
    }
    cursamplist = retrivesamplkist(cursampitem);
    prevsamplist = retrivesamplkist(prevsampitem);
    nextsamplist = retrivesamplkist(nextsampitem);
    if (navloc == "next") {
        for (i = 0; i < cursamplist.length; i++) {
            if (cursamplist[i].action == cursample) {
                if (i == cursamplist.length - 1) {
                    if (nextsamplist.length == 0) {
                        anchref = "/Introduction/IntroductionDefault";
                    }
                    else {
                        anchref = "/" + nextsamplist[0].controller + "/" + nextsamplist[0].action;
                    }
                }
                else if (i < cursamplist.length - 1) {
                    anchref = "/" + cursamplist[i + 1].controller + "/" + cursamplist[i + 1].action;
                }
                break;
            }
        }
    }
    else if (navloc == "prev") {
        for (i = 0; i < cursamplist.length; i++) {
            if (cursamplist[i].action == cursample) {
                if (i == 0) {
                    if (prevsamplist.length == 0) {
                        anchref = "/Introduction/IntroductionDefault";
                    }
                    else {
                        anchref = "/" + prevsamplist[prevsamplist.length - 1].controller + "/" + prevsamplist[prevsamplist.length - 1].action;
                    }
                }
                else if (i > 0 && i < cursamplist.length) {
                    anchref = "/" + cursamplist[i - 1].controller + "/" + cursamplist[i - 1].action;
                }
                break;
            }
        }
    }
    return anchref;
}
function retrivesamplkist(cursampitem) {
    var cursamplist = [];
    if (cursampitem) {
        for (i = 0; i < cursampitem.samples.length; i++) {
            if (cursampitem.samples[i].childcount == "0") {
                cursamplist.push(cursampitem.samples[i]);
            }
            else if (cursampitem.samples[i].samples.length != 0) {
                for (j = 0; j < cursampitem.samples[i].samples.length; j++) {
                    if (cursampitem.samples[i].samples[j].childcount == "0")
                        cursamplist.push(cursampitem.samples[i].samples[j]);
                }
            }
        }
    }
    return cursamplist;
}
//cookies
function createCookie(name, value, days) {
    var expires;

    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (3 * 60 * 1000));
        expires = "; expires=" + date.toGMTString();
    } else {
        expires = "";
    }
    document.cookie = escape(name) + "=" + escape(value) + expires + "; path=/";
}

function readCookie(name) {
    var nameEQ = escape(name) + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) === 0) return unescape(c.substring(nameEQ.length, c.length));
    }
    return null;
}

function eraseCookie(name) {
    createCookie(name, "", -1);
}
//calulate samplefile height
function autoResize(id) {
    var newheight;
    var newwidth;

    if (document.getElementById) {
        newheight = document.getElementById(id).contentWindow.document.body.scrollHeight;
        newwidth = document.getElementById(id).contentWindow.document.body.scrollWidth;
    }

    $("#" + id).height(newheight + 20);
}


/* Launching Other Products*/
function viewdemo(args) {
    var text = 'Handler/StartDevServer.ashx', product = args.text, prot = window.location.protocol, hostname = window.location.host;

    if (typeof (iisPrefixLink) != "undefined" && iisPrefixLink != null) {
        if (product == "Mobile") window.location = 'http://localhost/sfejmobilemvcsamplebrowser'; else return false;
        return false;
    }

    if (product == "Mobile") product = "mobile"; else return false;
    $.ajax({
        url: prot + "//" + hostname + "/" + text + "?product=" + product,
        success: function (data) {
            window.location = data;
        }
    });
}
//autocomplete
function onSelectSearchItem(args) {
    var links = $(document.head || document.getElementsByTagName('head')[0]).find("link");
    var serverconfig = links[0].href.substr(0, links[0].href.indexOf("Content") - 1);
    var url = serverconfig+ "/" + args.key;
    window.location = url;
}