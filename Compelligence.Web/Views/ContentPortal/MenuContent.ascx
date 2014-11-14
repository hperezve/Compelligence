<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<link href="<%= Url.Content("~/Content/Styles/rhm1.css") %>" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    $(function() {
        var pathname = window.location.pathname;
        if ((pathname.indexOf('ContentPortal') != -1) || (pathname.indexOf('Answer') != -1)) {
            $('#mnuWinLoss').show();
        } else {
            $('#mnuWinLoss').hide();
        }

    });
    function showOptionsWinloss(nameMenu, nameSubMenu, ppos) {
        var newTop = 0;
        var disabled = '<%= ViewData["DefaultsDisabPublComm"] %>';
        if (disabled != null && disabled != undefined) {
            if (disabled == 'true' || disabled == 'True' || disabled == 'TRUE') {
                newTop = 37;
            }
        }
        var pos = $('#' + nameMenu).position();
        if ((arguments.length == 3) && (arguments[2] != null))
            pos = ppos;


        $("#" + nameSubMenu).show();
        if (nameSubMenu == "optionsWinLoss") {
            $("#" + nameSubMenu).css("left", pos.left + 201 + "px");
            $("#" + nameSubMenu).css("top", (pos.top + 158 - newTop) + "px");

        }
        else {
            $("#" + nameSubMenu).css("left", pos.left + "px");
            $("#" + nameSubMenu).css("top", (pos.top + 37 - newTop) + "px");
        }

        console.log("l:" + pos.left + "t:" + pos.top);

    };


</script>

<div id="topbar">
<div id="topbarmenu">
    <div  id="topbarsearch">
            <% using (Html.BeginForm("Index", "ContentSearch", FormMethod.Get))
               { %>
            <%= Html.TextBox("q",null, new { Class="textbox"})%>
            <input type="image" class="search" value="Search" src="<%= Url.Content("~/Content/Images/Menu/search-icon.png") %>"/>
            <% } %>
    </div>
  <div class="rhm1-bg">
   <ul>
      <li id="mnuComparinator">
        <a href="javascript:void(0)" onmouseover="showOptions('mnuComparinator','optionsComparinator');" onmouseout="hideOptions('optionsComparinator');">
        <span>
      <%--Comparinator--%>
	    <label for="LblComparinator">
		    <%= ViewData["Comparinator"]%>
	    </label>
	    </span>
      </a>
      </li>
      <li id="mnuContent">
        <a href="<%=Url.Action("Index","ContentPortal") %>">
            <span>
            <label>          
            <%= ViewData["Content"]%></label></span>
        </a>
      </li>
      <li id="mnuTools"><a href="javascript:void(0)" onmouseover="showOptions('mnuTools','optionsTools');" onmouseout="hideOptions('optionsTools');" >
            <span><label>Tools</label></span>
        </a>
      </li>
  </ul>
  </div>
  
</div>
</div>
<div  id="optionsComparinator"  class="femenu_sub"  onmouseover="showOptions('mnuComparinator','optionsComparinator');" onmouseout="hideOptions('optionsComparinator');">
   <%--<%=Html.ActionLink("Comparison for " + ViewData["ProductLabel"], "Index", "Comparinator", null, new { Class = "fesubmenu"})%>--%>
   <a class="fesubmenu" href="<%=Url.Action("Index","Comparinator") %>">Comparison for <%=ViewData["ProductLabel"]%>&nbsp;&nbsp;</a>
</div>
<div id="optionsTools" class="femenu_sub" onmouseover="showOptions('mnuTools','optionsTools');" onmouseout="hideOptions('optionsTools')"> <!--;hideOptions('optionsWinLoss')-->
    <%--<%=Html.ActionLink("Content", "Index", "ContentPortal", null, new { Class = "fesubmenu"})%>--%>
    <%=Html.ActionLink("Deal Support", "Index", "DealSupport", null, new { Class="fesubmenu"})%>
    <%=Html.ActionLink("Events", "Index", "Events", null, new { Class = "fesubmenu" })%>
     <% bool DefaultsDisabPublComm = false;
        if (!string.IsNullOrEmpty((string)ViewData["DefaultsDisabPublComm"])) 
        { 
            DefaultsDisabPublComm = Convert.ToBoolean(ViewData["DefaultsDisabPublComm"].ToString()); 
        }
       if (!DefaultsDisabPublComm)
       {  %>
    <%=Html.ActionLink("Forums", "Index", "Forum", null, new { Class = "fesubmenu" })%>
    <% } %>
    <!--<a  id="mnuWinLoss" class="fesubmenu" href="javascript:void(0)"  onmouseover="showOptions('mnuTools','optionsWinLoss');"   >
      <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:LabelResource, ContentPortalMenuContentWinLoss%>" /> <span style="float:right;padding-right:5px">&#187;</span></a>-->

</div>

<!--<div id="optionsWinLoss"  class="femenu_sub"   onmouseover="showOptionsWinloss('mnuTools','optionsWinLoss');"   onmouseout="hideOptions('optionsWinLoss');"  >
    <%Html.RenderPartial(Url.Content("~/Views/ContentPortal/WinLossContent.ascx")); %>
</div>-->