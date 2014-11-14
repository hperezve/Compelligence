<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>
<%@ Import Namespace="Compelligence.Common.Utility.Web" %>
<%@ Import Namespace="Compelligence.DataTransfer.FrontEnd" %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>
<link href="<%= Url.Content("~/Content/Styles/FrontEndSite.css") %>" rel="stylesheet"    type="text/css" />
<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
<link href="<%= Url.Content("~/Content/Styles/rating.css") %>" rel="stylesheet" type="text/css" />
<link href="<%= Url.Content("~/Content/Styles/jquery.multiselect.css") %>" rel="stylesheet" type="text/css" />
<link href="<%= Url.Content("~/Content/Styles/jquery.multiselect.filter.css") %>" rel="stylesheet" type="text/css" />
<link href="<%= Url.Content("~/Content/Styles/comparinator.css") %>" rel="stylesheet" type="text/css" />
<link href="<%= Url.Content("~/Content/Styles/Comparisons.css") %>" rel="stylesheet" type="text/css" />
<link href="<%= Url.Content("~/Content/Styles/jquery.autocomplete.css") %>" rel="stylesheet" type="text/css" />
<link href="<%= Url.Content("~/Scripts/cluetip/jquery.cluetip.css") %>" rel="stylesheet" type="text/css" />

<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Download.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Rating.js") %>" type="text/javascript">    </script>
<script src="<%= Url.Content("~/Scripts/System/Retrieve.js") %>" type="text/javascript">    </script>
<script src="<%= Url.Content("~/Scripts/Chart.js") %>" type="text/javascript"></script>
<style type="text/css">
    body
    {
        font-size: .75em;
    font-family: verdana;/*Arial, Helvetica, Sans-Serif;*/
    margin:0px;
    min-width: 1025px;
    width: 100%;
    }
    td
    {
        font-size:0.75em;
    }

    .ui-multiselect-filter input
    {
        height:20px;
    }
    h1
    {
        font-size: 3em;
        margin: 20px 0;
    }
    input:focus
    {
        border: 1px solid #aaaaaa; /*height:23px;   */
    }
    .container
    {
        width: 100%;
        margin: 10px auto;
    }
   
     .sizeProductAdd
    {
        width:100%;
		float:left;
        min-height:305px;
    }
</style>
<script type="text/javascript">

    function hov(loc, cls) { if (loc.className) loc.className = cls; }
 
</script>
<script src="<%= Url.Content("~/Scripts/MicrosoftAjax.js") %>" type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"    type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/jquery-validate/jquery.validate.min.js") %>"    type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/System/FrontEnd/FeedBack.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Comments.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Messages.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Toggle.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Comparinator.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/System/FrontEnd/ComparinatorFilter.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/System/FrontEnd/ComparinatorTools.js") %>" type="text/javascript"></script>


<script src="<%= Url.Content("~/Scripts/jquery.multiselect.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jquery.multiselect.filter.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jquery.autocomplete.min.js") %>" type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/cluetip/jquery.cluetip-1.2.10.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>" type="text/javascript"></script>

<link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />
<script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>
<script type="text/javascript" charset="utf-8" src="<%= Url.Content("~/Scripts/browserselector.js") %>" ></script>
<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Functions.js") %>" type="text/javascript"></script>

<script type="text/javascript">
    var removeRecommendedProduct = function() {
    $('#DivRecommendProducts').empty();
    saved_comparison(1);
    };
    var loadContent = function(urlAction, target) {
        showLoadingDialog();
        $('#' + target).load(urlAction, {}, function() { hideLoadingDialog(); });
        removeRecommendedProduct();
        saved_comparison(1);
    };
    var loadSocialContent = function(urlAction, target) {
        var MyCompanyId = '<%= ViewData["C"] %>';
        var parameters = { C: MyCompanyId };
        $('#' + target).load(urlAction, parameters, function() { });
    };
    function RemoveProduct(urlAction, target) {
        var MyCompanyId = '<%= ViewData["C"] %>';
        var parameters = { C: MyCompanyId };
        $.get(urlAction, parameters, function(data) {
            $('#' + target).html(data);
            UpdateElements();
            resizeHeightOfTdImage();
            resizeHeightOfTdRecommendImage();
        });

        $('#FormResults').empty();
    }
</script>

<script type="text/javascript" src="<%= Url.Content("~/Scripts/System/CascadingDropDown.js") %>">  </script>
<script type="text/javascript">
    var LoadAutoComplete = function(Industryid, CriteriaId) {
        $.ajax({
            type: "POST",
            url: '<%= Url.Action("GetValues_Industry", "ProductCriteria")%>',
            dataType: "json",
            data: { IndustryId: Industryid, CriteriaId: CriteriaId },
            success: function(data) {
                $("#txtValue").autocomplete(data, {
                    matchContains: true,
                    minChars: 0,
                    max: 200,
                    formatItem: function(row, i, max) {
                        return row.Text;
                    }
                });
            }
        });

    };
    var LoadAutoCost = function(CriteriaId) {
        $.ajax({
            type: "POST",
            url: '<%= Url.Action("GetCost", "Criteria")%>',
            dataType: "json",
            data: { CriteriaId: CriteriaId },
            success: function(data) {
                $("#txtValueCost").autocomplete(data, {
                    matchContains: true,
                    minChars: 0,
                    max: 200,
                    formatItem: function(row, i, max) {
                        return row.Text;
                    }
                });
            }
        });
    }
    var LoadAutoBenefit = function(CriteriaId) {
        $.ajax({
            type: "POST",
            url: '<%= Url.Action("GetBenefit", "Criteria")%>',
            dataType: "json",
            data: { CriteriaId: CriteriaId },
            success: function(data) {
                $("#txtValue").autocomplete(data, {
                    matchContains: true,
                    minChars: 0,
                    max: 200,
                    formatItem: function(row, i, max) {
                        return row.Text;
                    }
                });
            }
        });

    };
</script>
<script type="text/javascript">
    //I Forced copy/past it need remove to external file
    function updCompetitorDropDown() {
        //for competitor, is possible assign event only after refresh competitors list
        $('input[name="multiselect_CompetitorId"]').click(function(Comp) {
            var var_name = $("input[name=multiselect_CompetitorId]:checked").val();
            var select_industry = $("input[name=multiselect_IndustryId]:checked").val();
            var urlComparinator = '<%= Url.Action("GetProductsByCompetitor", "Comparinator") %>/' + var_name + "?IndustryId=" + select_industry;
            $.ajax({
                type: "POST",
                url: '<%= Url.Action("GetProductsByCompetitor", "Comparinator") %>/' + var_name + "?IndustryId=" + select_industry,
                data: { U: '<%=(string)ViewData["U"] %>', C: '<%=(string)ViewData["C"] %>' },
                dataType: 'json',
                beforeSend: function() {
                    //$("#ProductIdLoader").show();
                    showLoadingDialog();
                },

                success: function(json) {
                    var items = "";
                    $.each(json, function(i, item) {
                        items += "<option value='" + item.Value + "' " + item.Disabled + " >" + item.Text + "</option>";
                    })
                    $("#ProductId").html(items);
                }, complete: function() {
                    hideLoadingDialog();
                    $("#ProductId").multiselect('refresh');
                    $("#ProductIdLoader").hide();
                }
            })

        });
    }
    function updIndustryDropDown() {


        $('input[name="multiselect_IndustryId"]').click(function(Ind) {
            //alert('hit industry');
            var var_name = $("input[name=multiselect_IndustryId]:checked").val();
            $('#FormProducts').empty();
            $('#FormResults').empty();
            $('#IndustryId').val(var_name);
            //verify parameter U
            var urls = '<%= Url.Action("GetCompetitorsByIndustry", "Comparinator") %>/' + var_name ;
            $.ajax({
                type: "POST",
                url: urls,
                data: { U: '<%=(string)ViewData["U"] %>', C: '<%=(string)ViewData["C"] %>' },
                dataType: 'json',
                beforeSend: function() {

                    showLoadingDialog();
                },
                success: function(json) {
                    var items = "";
                    var items1 = "";
                    var lista = "";
                    $.each(json, function(i, item) {
                        items += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                    })

                    $("#CompetitorId").html(items);
                    $("#ProductId").html(items1);
                }, complete: function() {
                    hideLoadingDialog();
                    $("#CompetitorId").multiselect('refresh');
                    $("#ProductId").multiselect('refresh');

                    updCompetitorDropDown();



                } //complete
            })
        });

    }
    
    
    //    


    $(function() {
        var sizeProducts = '<%= ViewData["DefaultsSocialLog"] %>';
        loadSocialContent('<%= Url.Action("SocialLogComparinatorSales", "ContentPortal") %>', 'FormSocialContent');
        if (sizeProducts == 'true') {
            $('#FormProducts').addClass('sizeProductAdd');
        }
        $('#IndustryId').attr("selectedIndex", 0);

        //
        //savecomparison
        $("#UserProfileComparisonId").multiselect({
            multiple: false,
            header: 'Select a Comparison',
            noneSelectedText: 'Select a Comparison',
            maxWidth: 700,
            selectedList: 1,
            clas_ajust: "comparison_titlec",
            classes: "w100Pc"
        }).multiselectfilter();
        $("#multiselect_button").bind('contextmenu', function(e) {
            e.preventDefault();
            //code
            renameComparisonList('<%=ViewData["U"]%>');  //Salesforce
            return false;
        }).prop('title', 'Right click to re-name saved comparison.');



        /*add*/
        $("#IndustryId").multiselect({
            multiple: false,
            header: 'Select an <%=ViewData["IndustryLabel"]%>',
            noneSelectedText: 'Select ',
            selectedList: 1,
            clas_ajust: "adjust-textc",
            classes: "auto w100P"
        }).multiselectfilter();

        $("#CompetitorId").multiselect({
            multiple: false,
            header: 'Select a <%=ViewData["CompetitorLabel"]%>',
            noneSelectedText: 'Select ',
            selectedList: 1,
            clas_ajust: "adjust-textc",
            classes: "auto w100P"
        }).multiselectfilter();

        $("#ProductId").multiselect({
            multiple: true,
            header: 'Select a <%=ViewData["ProductLabel"]%>',
            noneSelectedText: 'Select ',
            selectedList: 1,
            clas_ajust: "adjust-textc",
            classes: "auto w100P"
        }).multiselectfilter();

        updIndustryDropDown(); //important for redefine multiselect dropdown & refresh data
       
    });

    $(function() {
        $('#MainContent').css("width", "100%");
        $('#IndustryId').addClass("cmbStandard");
        $('#CompetitorId').addClass("cmbStandard");
        $('#ProductId').addClass("cmbStandard");
    });
    function UpdateElements() {
        $(".tab_content").hide(); //Hide all content
        $("ul.tabs li:last").addClass("active").show(); //Activate first tab
        $(".tab_content:last").show(); //Show first tab content
        //On Click Event
        $("ul.tabs li").click(function() {
            $("ul.tabs li").removeClass("active"); //Remove any "active" class
            $(this).addClass("active"); //Add "active" class to selected tab
            $(".tab_content").hide(); //Hide all tab content
            var activeTab = $(this).find("a").prop("href"); //Find the rel attribute value to identify the active tab + content
            $(activeTab).fadeIn(); //Fade in the active content
            return false;
        });
    };
    function ExternalFeedBackWithAttachedDlg(url, title) {
        var urlValidate = '<%= Url.Action("ValidateFile", "Forum")%>';
        ExternalFeedBackWithAttachedVDlg(url, title, urlValidate);
    };
    function ExternalResponseNewDlg(url) {
        var urlValidate = '<%= Url.Action("ValidateFile", "Forum")%>';
        ExternalResponseNewVDlg(url, urlValidate);
    };
</script>

<!--for Comparisons-->
<script type="text/javascript">
    var urlc_remove = '<%=Url.Action("RemoveComparisonList","Comparinator") %>';
    var urlc_load = '<%=Url.Action("GetComparisonList","Comparinator") %>';
    var urlc_info = '<%=Url.Action("GetComparisonListDropDownInfo","Comparinator") %>';
    var urlc_save = '<%=Url.Action("SaveComparisonList","Comparinator") %>';
    var urlc_rename = '<%=Url.Action("RenameComparisonList","Comparinator") %>';    
</script>
<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Comparisons.js") %>" type="text/javascript"></script>
<% string btnWidth = "width:100%;min-width:100%";
   string marginLeftToFiel = "";
   string marginRightDiv = "";
   string compeleftWidth = "";
   if ((Request.Browser.Browser == "IE") && (Request.Browser.MajorVersion <= 8))
   {
       //btnWidth = "width:73px;min-width:73px;";
       marginLeftToFiel = "margin-right: 3px;";
       btnWidth = "width:100%;min-width:100%;padding-right:0; padding-left:0;";
       marginRightDiv = "margin-right: 1%; width: 7%;padding-right:0;";
       compeleftWidth = "width:74%;";
   }
 %>
<div id="content">
<div class="comp_pleft" style="<%=compeleftWidth%>">
   <div id="headTitleComparison" class ="subtitleToComparison">
	Load a saved comparison
	</div> 
   <div id ="bodysavedComparison" class="lineHeaderComparison">
        <div class="labelField">
            <label for="" >
                <asp:Literal ID="Literal7" runat="server" Text="" />Saved Comparisons:
            </label>
        </div>
        <div class="longSelectFieldSF" style="<%=marginLeftToFiel%>">
            <%= Html.DropDownList("UserProfileComparisonId", (SelectList)ViewData["UserProfileComparisonId"],string.Empty)%>
        </div>
        <div class="buttonComparisonSFField" style="<%=marginRightDiv%>">
            <input type="button" class="button" value="Load"  style="<%=btnWidth%>" onclick="loadComparisonList('<%=(string)ViewData["U"] %>','<%=(string)ViewData["C"] %>')" title="Selected 'Saved Comparisons' will display comparison results below."/>
        </div>
        <div class="buttonComparisonSFField" style="<%=marginRightDiv%>">
            <input type="button" class="button" value="Remove"  style="<%=btnWidth%>"  onclick="removeComparisonList('<%=ViewData["U"]%>')" title="Selected 'Saved Comparisons' will be permanently deleted from your 'Saved Comparisons' list."/>
        </div>
    </div>
 
<% bool DefaultsSocialLog = Convert.ToBoolean(ViewData["DefaultsSocialLog"].ToString());%>
<% using (Ajax.BeginForm("AddSalesForceProduct", "Comparinator", new AjaxOptions
     {
         HttpMethod = "POST",
         UpdateTargetId = "FormProducts",
         LoadingElementId = "Messages",
         OnSuccess = "function(){UpdateElements();resizeHeightOfTdImage();setTimeout(function() {resizeHeightOfTdRecommendImage();},1550);}",
     }, new { id = "ProductForm" }))
   { %>
<div class="lineHeader">
       <div id="headTitleCreateComparison" class="labelInLineHeader">
	Create a new Comparison
	</div>
</div>  
<div id ="bodyCreateComparison" class="lineHeader">
        <%=Html.Hidden("U",(string)ViewData["U"]) %>
        <%=Html.Hidden("C",(string)ViewData["C"]) %>
        <div class="selectFieldSF" style="<%=marginLeftToFiel%>">
            <label for="Industry" class="comboBoxHeader">
                <asp:Literal ID="LblIndustry" runat="server" Text="" /><%=ViewData["IndustryLabel"]%><img src="<%= Url.Content("~/Content/Images/Icons/next.png") %>" alt=":" align="top"/>
            </label>
            <%=Html.DropDownList("IndustryId", (SelectList)ViewData["cmbIndustry"],string.Empty) %>
        </div>
        <div class="selectFieldSF" style="<%=marginLeftToFiel%>">
            <label for="Competitor" class="comboBoxHeader">
                <asp:Literal ID="LblCompetitor" runat="server" Text=""/><%=ViewData["CompetitorLabel"]%><img src="<%= Url.Content("~/Content/Images/Icons/next.png") %>" alt=":" align="top"/>
            </label>
            <%=Html.DropDownList("CompetitorId", (SelectList)ViewData["cmbCompetitor"], string.Empty)%>
        </div>
        <div class="selectFieldSF" style="<%=marginLeftToFiel%>">
            <label for="Product" class="comboBoxHeader">
                 <asp:Literal ID="LblProduct" runat="server" Text=""/><%=ViewData["ProductLabel"]%><img src="<%= Url.Content("~/Content/Images/Icons/next.png") %>" alt=":" align="top"/>
            </label>
            <%=Html.DropDownList("ProductId", (SelectList)ViewData["cmbProduct"], string.Empty)%>
        </div>
        <div class="buttonFieldSF" style="<%=marginRightDiv%>">
             <input id="BtnSubmit" type="submit" class="button" value="Add" onmouseover="hov(this,'btnhov')" onmouseout="hov(this,'button')"  style="<%=btnWidth%>"/>
        </div>
        <div class="buttonFieldSF" style="<%=marginRightDiv%>">
            <input id="btnCompare" type="button" value="Compare" class="button" onmouseover="hov(this,'btnhov')" onmouseout="hov(this,'button')" onclick="javascript:loadSocialContent('<%= Url.Action("SocialLogComparinatorSales", "ContentPortal") %>', 'FormSocialContent'); loadContent('<%= Url.Action("CompareSalesForceProducts", "Comparinator", new {U=(string)ViewData["U"],C=(string)ViewData["C"]}) %>','FormResults');removeRecommendedProduct();"  style="padding-left: 5px;padding-right: 5px;<%=btnWidth%>" />
        </div>
        <div class="buttonFieldSF" style="<%=marginRightDiv%>">
             <input type="button" class="button" value="Save" onclick="saveComparisonList('<%=(string)ViewData["U"] %>')" title="Comparison criteria will be saved to your 'Saved Comparisons' selections"   style="<%=btnWidth%>" />
        </div>
</div> 
        <div id="saved_comp_button" class="savedComparison">
            <div id="saved_comp_button_close"><img src="<%=Url.Content("~/Content/Images/Icons/tools_close.png") %>"
                onclick="saved_comparison(1)" /> Hide comparison tools</div>
            <div id="saved_comp_button_show" style="display: none"><img src="<%=Url.Content("~/Content/Images/Icons/tools_show.png") %>"
                onclick="saved_comparison(0)"/> Show comparison tools</div>
        </div> 
<% } %>
	
<div id="FormProducts">
</div>

<div id="FormConfirm" title="Confirm deletion?" class="displayNone">
    <p>
        <span class="ui-icon ui-icon-alert confirmDialog"></span>These comment will be deleted.
        Are you sure?</p>
</div>
<div id="FormComments">
</div>
<div id="DiscussionsResponse">   </div>
<div id="DiscussionsResponseNew">   </div>
<div id="FormFeedBack">
</div>
<div id="FormMessage">
</div>
<div id="MessageBox">
</div>
<div id="MessageStatus">
</div>
<div id="ExternalResponse" title="Comment Form">
</div>
<div id="ExternalResponseNew">
</div>
<div id="ProductInfo">
</div>
<div id="PositioningBox">
</div>
<div id="ConfirmBox">
</div>

<div id="ComparisonBox">
</div>
<div id="PrivateCommentNewDlg"></div>
</div> <!-- comp_left-->
<%if (DefaultsSocialLog)
 { %>
<div id="SocialContent" class="comp_pright" >
 <div id="FormSocialContent">
 </div>
</div>
<%} %>  
</div> <!-- content -->
<div id="FormResults" style="display: block;float: left;width:100%">
</div>

