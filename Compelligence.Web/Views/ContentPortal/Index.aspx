<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/FrontEndSite.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>
<asp:Content ID="indexHead" ContentPlaceHolderID="Head" runat="server">
    <title>Compelligence - Content Portal</title>
</asp:Content>
<asp:Content ID="indexStyles" ContentPlaceHolderID="Styles" runat="server">
    <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>"
        rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/Discussion.css") %>" rel="stylesheet"
        type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jquery.multiselect.filter.css") %>"
        rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/Sticky/stickytooltip.css") %>" rel="stylesheet"
        type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jquery.multiselect.css") %>" rel="stylesheet"
        type="text/css" />
    <link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />
</asp:Content>
<asp:Content ID="indexScripts" ContentPlaceHolderID="Scripts" runat="server">

    <script src="<%= Url.Content("~/Scripts/MicrosoftAjax.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.js") %>" type="text/javascript"></script>
    
    <%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>--%>
    <%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.8.12.custom.min.js") %>" type="text/javascript"></script>--%>
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/Sticky/stickytooltip.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"
        type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-validate/jquery.validate.min.js") %>"
        type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Rating.js") %>" type="text/javascript">    </script>

    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Feedback.js") %>" type="text/javascript">  </script>

    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Comments.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Messages.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Utility.js") %>" type="text/javascript"></script>

    <!-- agregando appis js-->

    <script src="<%= Url.Content("~/Scripts/jquery.multiselect.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery.multiselect.filter.js") %>" type="text/javascript"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/Question.2.js") %>"></script>
    
    <script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Functions.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/FunctionsFrontEnd.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/EditAddSW.js") %>" type="text/javascript"></script>
    
    <script type="text/javascript">
        var urlc_deleteS = '<%=Url.Action("deleteStrength","ContentPortal") %>';
        var urlc_deleteW = '<%=Url.Action("deleteWeakness","ContentPortal") %>';
    </script>
    <script type="text/javascript">

        function ChangeAction(url) {
            showLoadingDialog();
            $('#FrontEndForm').prop("action", url);
            $('#FrontEndForm').submit();
        }
        $(document).ready(function() {
            $('.AddTargetBlankToLin a').attr('target', '_blank');
        });
        function FeedBackFormDlg6(urlAction, dlgTitle, entityId, u, c) {
            var pcnDlg = $("#PrivateCommentNewDlg");
            pcnDlg.empty();
            pcnDlg.dialog({
                autoOpen: false,
                title: dlgTitle,
                width: 470,
                height: 'auto',
                autoResize: true,
                close: function(event, ui) { $(this).dialog("destroy"); hideLoadingDialog(); },
                buttons:
                {
                    "Close": function() { $(this).dialog("destroy"); hideLoadingDialog(); }
                     ,
                    "Send": function() {
                    $("#frameFeedBack").contents().find("#FeeBackResponseForm").submit();
                    $(".ui-dialog-buttonpane button:contains('Send')").button().hide();
                    }
                }
            });
            pcnDlg.html("<iframe id='frameFeedBack' src='" + urlAction + "' width=450 height=200 frameBorder=0></iframe>");
            pcnDlg.dialog("open");
            return false;
        };
        function ExternalFeedBackWithAttachedDlg(url, title) {
            var urlValidate = '<%= Url.Action("ValidateFile", "Forum")%>';
            ExternalFeedBackWithAttachedVDlg(url, title, urlValidate);
        };
        function ExternalResponseNewDlg(url) {
            var urlValidate = '<%= Url.Action("ValidateFile", "Forum")%>';
            ExternalResponseNewVDlg(url, urlValidate);
        };
        //var firebug = document.createElement('script'); firebug.setAttribute('src', 'http://getfirebug.com/releases/lite/1.2/firebug-lite-compressed.js'); document.body.appendChild(firebug); (function() { if (window.firebug.version) { firebug.init(); } else { setTimeout(arguments.callee); } })(); void (firebug);

       
    </script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <% Html.RenderPartial("LeftContent"); %>
    <div id="FormComments"></div>
    <div id="FormDiscussions"></div>
    <div id="FormConfirm" title="Confirm deletion?" class="displayNone">
        <p>
            <span class="ui-icon ui-icon-alert confirmDialog"></span>
            <%--These comment will be deleted. Are you sure?--%>
            <label for="LblMessage1">
                <asp:Literal ID="LtMessage1" runat="server" Text="<%$ Resources:LabelResource, ContentPortalMessage1%>" />
            </label>
        </p>
    </div>
    <div id="FormFeedBack">
    </div>
    <div id="FormQuiz">
    </div>
    <div id="FormMessage">
    </div>
    <%--<div id="FileNotFound">
        <br />
    </div>--%>
   <div id="ExternalResponse" title="Comment Form">   </div>
    <div id="ExternalResponseNew" >   </div>
    <div id="DiscussionsResponse">   </div>
    <div id="DiscussionsResponseNew">   </div>
    <div id="PrivateCommentNewDlg"></div>
    <div id="PositioningBox"></div>
    <div id="StrengthWeaknessBox"></div>
    <div id="HomeFE">
    </div>
    <div id="mystickytooltip" class="stickytooltip" style="max-width:35%;max-height:190px;">
        <div style="padding: 5px">
            <%if ((string)ViewData["IndustryImageUrl"] == null || (string)ViewData["IndustryImageUrl"] == "")
              { %>
            <div id="sticky1" class="atip">
                <img src="/Content/Images/Icons/none.png" />
            </div>
            <%}
              else
              {
                  string IndustryImageUrl = (string)ViewData["IndustryImageUrl"];
                  if (IndustryImageUrl.Substring(0, 2).Equals("./"))
                      IndustryImageUrl = "." + IndustryImageUrl;%>
            <div id="sticky1" class="atip" >
                <img src="<%=IndustryImageUrl%>" style="max-width:100%;max-height:168px" />
            </div>
            <%} %>
            <%if ((string)ViewData["CompetitorImageUrl"] == null || (string)ViewData["CompetitorImageUrl"] == "")
              { %>
            <div id="sticky2" class="atip">
                <img src="/Content/Images/Icons/none.png" />
            </div>
            <%}
              else
              {
                  string CompetitorImageUrl = (string)ViewData["CompetitorImageUrl"];
                  if (CompetitorImageUrl.Substring(0, 2).Equals("./"))
                      CompetitorImageUrl = "." + CompetitorImageUrl;%>
            <div id="sticky2" class="atip">
                <img src="<%=CompetitorImageUrl%>" style="max-width:100%;max-height:168px" />
            </div>
            <%} %>
            <%if ((string)ViewData["ProductImageUrl"] == null || (string)ViewData["ProductImageUrl"] == "")
              { %>
            <div id="sticky3" class="atip">
                <img src="/Content/Images/Icons/none.png" />
            </div>
            <%}
              else
              {
                  string ProductImageUrl = (string)ViewData["ProductImageUrl"];
                  if (ProductImageUrl.Substring(0, 2).Equals("./"))
                      ProductImageUrl = "." + ProductImageUrl;%>
            <div id="sticky3" class="atip">
                <img src="<%=ProductImageUrl%>"  style="max-width:100%;max-height:168px" />
            </div>
            <%} %>
        </div>
        <div class="stickystatus">
        </div>       
    </div>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="RightMainContent" runat="server">
<!--here we have to put the condition-->

    <%=Html.RenderPanels((IList<WebsitePanel>)ViewData["Panels"],Convert.ToBoolean(ViewData["DefaultsSocialLog"]))%>
    <br />   
    <% Html.RenderPartial("~/Views/Shared/FrontEndDownloadSection.ascx"); %>
</asp:Content>
