<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MasterPages/FrontEndSite.Master"
    Inherits="System.Web.Mvc.ViewPage" ValidateRequest="false" %>

<%@ Import Namespace="Compelligence.Domain.Entity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= Url.Content("~/Content/Styles/FrontEndSite.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jquery.multiselect.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jquery.multiselect.filter.css") %>"  rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/comparinator.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/Comparisons.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jquery.autocomplete.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Scripts/cluetip/jquery.cluetip.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Download.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/Retrieve.js") %>" type="text/javascript"></script>
    <title>Compelligence - Comparinator</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Scripts" runat="server">
       
    <script src="<%= Url.Content("~/Scripts/MicrosoftAjax.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-validate/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Rating.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/FeedBack.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Comments.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Messages.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Toggle.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Comparinator.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/ComparinatorFilter.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/ComparinatorTools.js") %>" type="text/javascript"></script>

    <!-- agregando appis js-->

    <script src="<%= Url.Content("~/Scripts/jquery.multiselect.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.multiselect.filter.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.autocomplete.min.js") %>" type="text/javascript"></script>

    <!--Comparinator Result-->
    <script src="<%= Url.Content("~/Scripts/cluetip/jquery.cluetip-1.2.10.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>" type="text/javascript"></script>
    
    <link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />
    <script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>
    <script type="text/javascript" charset="utf-8" src="<%= Url.Content("~/Scripts/browserselector.js") %>" ></script>
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Functions.js") %>" type="text/javascript"></script>
    <!--End - Comparinator Result-->
    <style type="text/css">
     .sizeProductAdd
    {
        width:70%;
    }
    </style>
    
    <script type="text/javascript">

        var removeRecommendedProduct = function() {
            $('#DivRecommendProducts').empty();
            $('#DivRecommendProducts').removeClass("comp_block");
            saved_comparison(1); 
        };

        var loadContent = function(urlAction, target) {
            showLoadingDialog();
            $('#' + target).load(urlAction, {}, function() { hideLoadingDialog(); });
            removeRecommendedProduct();
            saved_comparison(1);            
        };

        var loadSocialContent = function(urlAction, target) {
            $('#' + target).load(urlAction, {}, function() { });
        };
               
        function RemoveProduct(urlAction, target) {
            removeRecommendedProduct();
            $.get(urlAction, {}, function(data) {
                $('#' + target).html(data);
                resizeHeightOfTdImage();
                resizeHeightOfTdRecommendImage();
            });
            $('#FormResults').empty();
        }

        function toggle_visibility(container, enabled) {
            var e = container.getElementsByTagName('span')[0];
            //if (e.style.display == 'block')
            if (!enabled)
                e.style.display = 'none';
            else {
                e.style.display = 'block';
                e.setAttribute("className", "tooltipspanhover"); //For IE; harmless to other browsers.
                e.setAttribute("class", "tooltipspanhover"); //For IE; harmless to other browsers.
            }


        }
        function resetOnFeaturesTab() {//to reload when the user reset click
            var urlAction = '<%= Url.Action("CompareProducts", "Comparinator") %>';
            showLoadingDialog();
            $('#FormResults').load(urlAction, {}, function() {
                hideLoadingDialog();
                $(".comp_tab_content").hide(); //after reload the tabs hosuls be hiiden
                $("ul.comp_tabs li").removeClass("comp_active"); //remove the class to no active tab
                $("#featuresTab").addClass("comp_active").show();
                $("#tab4").attr("style", "display:inline");
                update_tools(0); //                
            });            
        };
    </script>
  <script type="text/javascript">
      function tgr_dlg(pEntityId, pObjectType, pIndustryId, pCriteriaId, pForumResponseId, pEntityIdT,U,C) 
      {
          var urlbase = '<%= Url.Action("GetComments", "Forum")%>';
          var urlbase2 = '<%= Url.Action("ExternalResponse", "Forum")%>';
          ExternalCommentsDlg(urlbase + "?EntityId=" + pEntityId
	                           + "&ObjectType=" + '<%=DomainObjectType.ProductCriteria %>' //pObjectType
							   + "&IndustryId=" + pIndustryId
							   + "&CriteriaId=" + pCriteriaId
							   + "&EntityIdT=" + pEntityIdT
							   , 'Comment Form',
          urlbase2 + '?EntityId=' + pEntityId +
          "&ForumResponseId=" + 0 +
          "&ObjectType=" + pObjectType
          + "&IndustryId=" + pIndustryId
          + "&CriteriaId=" + pCriteriaId
          + "&EntityIdT=" + pEntityIdT, pEntityId);
      }
      function ExternalFeedBackWithAttachedDlg(url, title) {
          var urlValidate = '<%= Url.Action("ValidateFile", "Forum")%>';
          ExternalFeedBackWithAttachedVDlg(url, title, urlValidate);
      };
      function ExternalResponseNewDlg(url) {
          var urlValidate = '<%= Url.Action("ValidateFile", "Forum")%>';
          ExternalResponseNewVDlg(url, urlValidate);
      };
  </script>
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

    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/CascadingDropDown.js") %>">  </script>

    <script type="text/javascript">

        function updCompetitorDropDown() 
        {
            //for competitor, is possible assign event only after refresh competitors list
            $('input[name="multiselect_CompetitorId"]').click(function(Comp) {
                var var_name = $("input[name=multiselect_CompetitorId]:checked").val();
                var select_industry = $("input[name=multiselect_IndustryId]:checked").val();
                var urlComparinator = '<%= Url.Action("GetProductsByCompetitor", "Comparinator") %>/' + var_name + "?IndustryId=" + select_industry;
                $.ajax({
                    type: "POST",
                    url: '<%= Url.Action("GetProductsByCompetitor", "Comparinator") %>/' + var_name + "?IndustryId=" + select_industry,
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
                var urls = '<%= Url.Action("GetCompetitorsByIndustry", "Comparinator") %>/' + var_name;
                $.ajax({
                    type: "POST",
                    url: urls,
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

        $(function() {
            var sizeProducts = '<%= ViewData["DefaultsSocialLog"] %>';
            loadSocialContent('<%= Url.Action("SocialLogComparinatorIndex", "ContentPortal") %>', 'FormSocialContent');
            if (sizeProducts == 'true') {
                //$('#FormProducts').addClass('sizeProductAdd');
            }
            $('#IndustryId').prop("selectedIndex", 0);

            //savecomparison
            $("#UserProfileComparisonId").multiselect({
                multiple: false,
                header: 'Select a Comparison',
                noneSelectedText: 'Select a Comparison',
                maxWidth: 700,
                selectedList: 1,
                clas_ajust: "adjust-textc",
                classes: "w100Pc"
            }).multiselectfilter();
            $("#multiselect_button").bind('contextmenu', function(e) {
                e.preventDefault();
                //code
                renameComparisonList();
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

            updIndustryDropDown();
        });     //document
    </script>

    <script type="text/javascript">
        var urlBrowsePopup = '<%= Url.Action("GetContentBrowsePopup", "Browse") %>';
    </script>

    <script type="text/javascript">
        var cont = 0;
        var browseContent = function(id, competitor, product) {
            var title = "Content Portal";
            var browseId = 'Content' + id;
            var industry = $('#IndustryId');
            $('#CompetitorId').value = competitor;
            $('#ProductId').value = product;

            browsePopup = window.open('<%= Url.Action("ContentPopup", "ContentPortal") %>?Title=' + title + '&BrowseId=' + browseId + '&Industry=' + industry[0].value + '&Competitor=' + competitor + '&Product=' + product, "ContentBrowsePopup" + id, "width=800,height=730");
            if (window.focus) {
                browsePopup.focus();
            }
            cont = cont + 1;
        };
    </script>

    <script type="text/javascript">
        function resize(which) {
            var elem = document.getElementById(which);
            var percentajeRectWidth;
            var percentajeRectHeight;
            if (elem == undefined || elem == null) return false;
            var height = elem.height;
            var width = elem.width;
            if (elem.width > 250) {
                percentajeRectWidth = (25000 / width);
                elem.style.height = ((height * percentajeRectWidth) / 100).toString() + 'px';
                height = (height * percentajeRectWidth) / 100;
                elem.style.width = 250 + 'px';
                width = 250;
            }
            if (elem.height > 84) {
                percentajeRectHeight = (8400 / height);
                elem.style.width = ((width * percentajeRectHeight) / 100).toString() + 'px';
                elem.style.height = 84 + 'px';
            }
        };
    </script>
    <script type="text/javascript">
        var urlc_remove = '<%=Url.Action("RemoveComparisonList","Comparinator") %>';
        var urlc_load = '<%=Url.Action("GetComparisonList","Comparinator") %>';
        var urlc_info = '<%=Url.Action("GetComparisonListDropDownInfo","Comparinator") %>';
        var urlc_save = '<%=Url.Action("SaveComparisonList","Comparinator") %>';
        var urlc_rename = '<%=Url.Action("RenameComparisonList","Comparinator") %>';        
    </script>
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Comparisons.js") %>" type="text/javascript"></script>

</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
<% string btnWidth = "width:100%;min-width:100%"; string marginRightDiv = "";
   if ((Request.Browser.Browser == "IE") && (Request.Browser.MajorVersion <= 8))
   {
       btnWidth = "width:87%;min-width:87%;";
       marginRightDiv = "margin-right: 1%; width: 7%;";
   }
 %>
<div id="content">
   <div class="comp_pleft">
      <!--<div class="savedComparison">My code-->  
        <div id="headTitleComparison" class="subtitleToComparison">
	    Load a saved comparison
	    </div>
	    <div id ="bodysavedComparison" class="lineHeaderComparison">
	        <div class="labelField">
                <label for="" >
                    <asp:Literal ID="Literal7" runat="server" Text="" />Saved Comparisons:
                </label>
            </div>
            <div class="longSelectField">
                <%= Html.DropDownList("UserProfileComparisonId", (SelectList)ViewData["UserProfileComparisonId"],string.Empty)%>
            </div>
            <div class="buttonComparisonField" style="<%=marginRightDiv%>">
                <input type="button" class="button" value="Load" style="<%=btnWidth%>" onclick="loadComparisonList()" title="Selected 'Saved Comparisons' will display comparison results below."/>
            </div>
            <div class="buttonComparisonField" style="<%=marginRightDiv%>">
                <input type="button" class="button" value="Remove"  style="<%=btnWidth%>" onclick="removeComparisonList()" title="Selected 'Saved Comparisons' will be permanently deleted from your 'Saved Comparisons' list."/>
            </div>
	    </div>

	  <%--</div>--%>
	
  <%--  <div class="comp_psave"> Saved Comparisons :
    </div> --%> 
    <div class="lineHeader">
        <div id="headTitleCreateComparison" class="labelInLineHeader">
	        Create a new comparison
	    </div>
    </div>
    <% bool DefaultsSocialLog = Convert.ToBoolean(ViewData["DefaultsSocialLog"].ToString());%>
    <% using (Ajax.BeginForm("AddProduct", "Comparinator", new AjaxOptions
     {
         HttpMethod = "POST",
         UpdateTargetId = "FormProducts",
         LoadingElementId = "Messages",
         OnSuccess = "function(){resizeHeightOfTdImage();setTimeout(function() {resizeHeightOfTdRecommendImage();},300);}",

     }, new { id = "ProductForm" }))
       { %>
    <div id ="bodyCreateComparison" class="lineHeader">
        <div class="selectField">
            <label for="Industry" class="comboBoxHeader">
                <asp:Literal ID="Literal3" runat="server" Text="" /><%=ViewData["IndustryLabel"]%><img src="<%= Url.Content("~/Content/Images/Icons/next.png") %>" alt=":" align="top"/>
            </label>
            <%=Html.DropDownList("IndustryId", (SelectList)ViewData["cmbIndustry"], string.Empty)%>
        </div>
        <div class="selectField">
            <label for="Competitor" class="comboBoxHeader">
                <asp:Literal ID="Literal4" runat="server" Text=""/><%=ViewData["CompetitorLabel"]%><img src="<%= Url.Content("~/Content/Images/Icons/next.png") %>" alt=":" align="top"/>
            </label>
            <%=Html.DropDownList("CompetitorId", (SelectList)ViewData["cmbCompetitor"], string.Empty)%>
        </div>
        <div class="selectField">
            <label for="Product" class="comboBoxHeader">
                <%=ViewData["ProductLabel"]%><img src="<%= Url.Content("~/Content/Images/Icons/next.png") %>" alt=":" align="top"/>
            </label>
            <%=Html.DropDownList("ProductId", (SelectList)ViewData["cmbProduct"], string.Empty)%>
        </div>
        <div class="buttonField" style="<%=marginRightDiv%>">
            <input id="btnSubmit" type="submit" value="Add" class="button" onclick="javascript: resize('imgDetail')"  style="<%=btnWidth%>"/>
        </div>
        <div class="buttonField" style="<%=marginRightDiv%>">
            <input id="btnCompare" type="button" value="Compare" class="button" onclick="javascript:loadSocialContent('<%= Url.Action("SocialLogComparinatorIndex", "ContentPortal") %>', 'FormSocialContent');loadContent('<%= Url.Action("CompareProducts", "Comparinator") %>','FormResults');removeRecommendedProduct()"  style="padding-left: 5px;padding-right: 5px;<%=btnWidth%>"/>
        </div>
        <div class="buttonField" style="<%=marginRightDiv%>">
            <input id="btnSave" type="button" class="button" value="Save" onclick="saveComparisonList()" title="Comparison criteria will be saved to your 'Saved Comparisons' selections"  style="<%=btnWidth%>"/>
        </div>
    </div> 
    	<div id="saved_comp_button" class="savedComparison">
            <div id="saved_comp_button_close"><img src="<%=Url.Content("~/Content/Images/Icons/tools_close.png") %>"
                onclick="saved_comparison(1)" /> Hide comparison tools</div>
            <div id="saved_comp_button_show" style="display: none"><img src="<%=Url.Content("~/Content/Images/Icons/tools_show.png") %>"
                onclick="saved_comparison(0)"/> Show comparison tools</div>
        </div>     	
    <br />
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
    <% Html.RenderPartial("~/Views/Shared/FrontEndDownloadSection.ascx"); %>    
  </div>
   <%if (DefaultsSocialLog)
     { %>
   <div id="SocialContent" class="comp_pright" >
     <div id="FormSocialContent">
     </div>
   </div>
   <%} %>  
</div>
<div id="FormResults" style="display: block;float: left;width:100%">
</div>
   
</asp:Content>