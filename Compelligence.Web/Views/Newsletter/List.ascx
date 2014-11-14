<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Newsletter>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'NewsletterAll');
    }).trigger('resize');
</script>
<asp:Panel ID="NewsletterToolbarContent" runat="server" CssClass="buttonLink">

    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Newsletter") %>', '<%= ViewData["Scope"] %>', 'Newsletter', 'NewsletterAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Newsletter") %>', '<%= ViewData["Scope"] %>', 'Newsletter', 'NewsletterAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Newsletter','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Newsletter") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('Newsletter','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Newsletter") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Newsletter', 'NewsletterAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Newsletter', 'NewsletterAll');" />
    <%--<input class="button" type="button" value="Preview" onclick="javascript: NewsPreview('<%=Url.Action("Preview","Newsletter") %>');" />--%>
    <input class="button" type="button" value="Send" onclick="javascript: NewsSend('<%=Url.Action("SendMail2","Newsletter") %>');" />
    <input class="button" type="button" value="Show Calendar" onclick="CreateCalendar('<%= Url.Action("CreateCalendar", "Newsletter") %>');" />
    
    <input id="<%= ViewData["Scope"] %>NewsletterCheckbox" class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>', 'NewsletterAll', 'NewsletterAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');" />
    <label for="<%= ViewData["Scope"] %>NewsletterCheckbox" style="font-size: 12px; color: black;">
    
    
    <asp:Literal ID="NewsletterCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label>
    <input class="button" type="button" value="Help" onclick="javascript: SetValuesToShowHelp('<%= ViewData["Scope"] %>', 'Newsletter','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','<%= Compelligence.Domain.Entity.Resource.Pages.Newsletter %>');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>NewsletterAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="NewsletterDataListContent" runat="server" CssClass="contentDetailData">


    <div id="gridOverflow" class="gridOverflow">
        <%= Html.DataGrid("NewsletterAll", "Newsletter", ViewData["Container"].ToString())%>
    </div>
</asp:Panel>
<asp:Panel ID="NewsletterSearchContent" runat="server">
    <%= Html.GridSearch("NewsletterAll")%>
</asp:Panel>
<asp:Panel ID="NewsletterFilterContent" runat="server">
    <%= Html.GridFilter("NewsletterAll")%>
</asp:Panel>

