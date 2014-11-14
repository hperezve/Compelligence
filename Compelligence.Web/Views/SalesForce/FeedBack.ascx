<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<strong id="validateTips"></strong>
<form id="frmFeedBack" action="">
    <input type="hidden" id="EntityId" name="EntityId" value='<%=ViewData["EntityId"] %>' />
    <input type="hidden" id="ObjectType" name="ObjectType" value='<%=ViewData["ObjectType"] %>' />
    <input type="hidden" id="IndustryId" name="IndustryId" value='<%=ViewData["IndustryId"] %>' />
    <input type="hidden" id="U" name="U" value='<%=ViewData["U"] %>' />
  <label for="txtComment">Comment</label>
  <textarea name="txtComment" id="txtComment" WRAP=SOFT class="textareadialog text ui-widget-content ui-corner-all" COLS=50 ROWS=4></textarea>
</form>

