<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<form id='ComparinatorFormEdit'>
 <input type='hidden' name='IndustryId' value="<%=ViewData["IndustryId"]%>" />
 <input type='hidden' name='txtType' value="<%=ViewData["Type"]%>" />
 <input type='hidden' name='txtCriteriaId' value="<%=ViewData["CriteriaId"]%>" />
 <input type='hidden' name='txtEntityId' value="<%=ViewData["EntityId"]%>" />
 <label>Value</label><input type='text' name='txtValue' value="<%=ViewData["Value"]%>" /><br />
 <label>Notes</label><textarea name='txtNotes' WRAP=SOFT COLS=50 ROWS=4><%=ViewData["Notes"]%></textarea><br />
 <label>Links</label><textarea name='txtLinks' WRAP=SOFT COLS=50 ROWS=4><%=ViewData["Links"]%></textarea><br />
</form>

    