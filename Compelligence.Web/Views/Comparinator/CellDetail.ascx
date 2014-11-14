<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
  <%
      ProductCriteria pc=(ProductCriteria)ViewData["ProductCriteria"];
      bool hascomments = (bool)ViewData["HasComments"];
      bool ddp = Convert.ToBoolean(ViewData["spc"]);
   %>
  <div style="width:190px; overflow:hidden;" >  <%=pc.Notes %>  </div><br>
  <div style="width:100%; overflow:hidden;" >  <%=pc.Links %>    </div><br>
  <div class="comp_cdp" style="width:190px;text-align:center; overflow:hidden;">    
     <%if (!ddp)
     { %>
        <div onclick="ExternalCommentsDlg('<%=Url.Action("GetComments", "Forum", new { EntityId = pc.ExternalId, ObjectType = ViewData["ObjectType"] })%>','<%=Url.Action("ExternalResponse", "Forum", new {EntityId=pc.ExternalId,ObjectType=ViewData["ObjectType"],ForumResponseId=0,IndustryId=pc.IndustryId,CriteriaId=pc.CriteriaId, ProductId=pc.ProductId ,U=(string)ViewData["U"],C=(string)ViewData["C"]})%>');EnqueueUpdateCorner('<%=pc.CriteriaId %>','<%=pc.ProductId%>')" title="Add public comment" class="comp_ficon <%=(hascomments? "comp_ficong":"") %>"  />  
      <%} %>
     <div onclick="ExternalFeedBackWithAttachedDlg('<%= Url.Action("FeedBackMessage", "Forum", new {EntityId=(string)ViewData["pid"],EntityType=DomainObjectType.Product, U=(string)ViewData["U"], SubmittedVia=FeedBackSubmitedVia.Positioning  }) %>','FeedBack Dialog');" title="Add private feedback" class="comp_fbicon" />    
     <div onclick="CellPropertyDlg('<%=ViewData["CellPropertyTitle"]%>','<%=Url.Action("CellProperty", "Comparinator", new { iid=pc.IndustryId,cid=pc.CriteriaId,pid=pc.ProductId, U=(string)ViewData["U"], C=(string)ViewData["C"]})%>', '<%=Url.Action("CellPropertySave","Comparinator") %>')"  title="Edit properties" class="comp_picon" /> 
  </div>    
