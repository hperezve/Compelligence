<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Views" %>
<%@ Import Namespace="Compelligence.BusinessLogic.Implementation" %>

<% bool DefaultsDisabPublComm = Convert.ToBoolean(ViewData["DefaultsDisabPublComm"].ToString());  %>


<style type="text/css">
    #linkSocialLog a
    {
        color: #0000FF;
    }
    #linkSocialLog a:hover
    {
        color: black;
    }
    #linkSocialLog a:active
    {
        color: gray;
    }
    
</style>
<% string userId = (string)Session["UserId"];
     IList<ActionHistoryAllView> actionList = (IList<ActionHistoryAllView>)ViewData["Social"]; %>
<% if (actionList != null && actionList.Count > 0)
    {%>
    <div class="rightTitle">Activity Log</div>
    <div class="shadow" style="height: 200px; overflow-y: auto;">
        <ul>
            <% foreach (ActionHistoryAllView actionAllView in actionList)
                   {
            %>
          <li> On  <%=actionAllView.CreatedDate.Value.ToShortDateString()%> at <%=actionAllView.CreatedDate.Value.TimeOfDay%>
                <b><%= actionAllView.CreatedByName%></b> 
                
                
                <% if (!string.IsNullOrEmpty(actionAllView.Controller))
                   { 
                           
                                       
                       if (actionAllView.Controller.Equals("Product"))
                           { %>                                      
                         
                          <a href="<%= Url.Action("Comments",actionAllView.Controller) + "/"+ actionAllView.EntityId %>">
                                    <%= actionAllView.EntityAction.Replace("product",(String)ViewData["ProductLabel"])%></a>
                                    <% if (actionAllView.EntityAction.Equals("Commented"))
                                       { %>
                                    on
                                     <%} %>
                                     the   
                         
                           <% string[] entities3 = actionAllView.EntitiesOfAction.Split('Å'); 
                        %>
                                       
                        
                        <% if (actionAllView.EntitiesOfAction.Equals(""))
                           { %>
                           
                       <%= actionAllView.Description%> "<%= actionAllView.Controller%>"
                        
                         <% }
                                             
                           else if (entities3.Length >= 2)
                           { %>
                        <% string[] industry = entities3[0].Split('Ä');
                           string industryId = industry[1];%>
                         
                        <% for (int i = 1; i < entities3.Count(); i++)
                               { %>
                            <% string[] properties = entities3[i].Split('Ä');
                               if (properties.Length <= 5) continue;
                            %>
                            <a href="<%= Url.Action("ChangeProduct", "ContentPortal")+ "?Industry=" +industryId + "&Competitor=" +properties[1]+ "&Product=" +properties[4]%>">
                                <%--<a href="#" onclick='selectIndustry();'>--%>
                                <%= properties[5]%></a>
                            <% if (i < (entities3.Count()) - 2)
                               { %>
                            ,
                            <% }
                               else if (i == (entities3.Count()) - 2)
                               { %>
                            and
                            <%} %>
                            <%} %>
                                           
                        <% }
                                            
                           }                                                
                       else if (actionAllView.Controller.Equals("DealSupport"))
                           { %>
                                <% if (actionAllView.CreatedBy.Equals(userId))
                                   { %>
                                   
                                <% if (actionAllView.EntityAction.Equals("Commented"))
                                   {%>
                                <% if (!DefaultsDisabPublComm)
                                   { %>                                
                                <a href="<%= Url.Action("Comments",actionAllView.Controller) + "/"+ actionAllView.EntityId %>">
                                    <%= actionAllView.EntityAction%></a>
                                    <%} else { %>
                                    <%= actionAllView.EntityAction%>
                                    <% } %>
                                <% }
                                   else if (actionAllView.EntityAction.Equals("Opened") || actionAllView.EntityAction.Equals("Updated"))
                                   { %>
                                <a href="<%= Url.Action("Edit",actionAllView.Controller) + "/"+ actionAllView.EntityId %>">
                                    <%= actionAllView.EntityAction %></a>
                                <% }
                                   else
                                   { %>
                                <%= actionAllView.EntityAction%>
                                <% } %>
                                the <a href="<%= Url.Action("Edit",actionAllView.Controller) + "/"+ actionAllView.EntityId %>">
                                    <%= actionAllView.TypeName%></a>
                                <%= actionAllView.Type%>
                                <% }
                                   else
                                   { %>
                                <%= actionAllView.Description%>
                                <% } %>
                    <% }
                    else if (actionAllView.Controller.Equals("Comparinator"))
                    {%>
                        <% if ((actionAllView.EntityAction.Equals("Compared products")) && (actionAllView.EntitiesOfAction.IndexOf('Ä') != -1))
                           { %>
                        <% string[] entities3 = actionAllView.EntitiesOfAction.Split('Å'); 
                        %>
                        <% if (entities3.Length >= 2)
                           { %>
                        <% string[] industry = entities3[0].Split('Ä');
                           string industryId = industry[1];%>
                        <a href="<%= Url.Action(actionAllView.Action,actionAllView.Controller) %>">
                            <%--<a href="<%= Url.Action("Index", "Comparinator") %>">--%>
                            <%= actionAllView.EntityAction.Replace("products", (String)ViewData["ProductLabel"])%></a> the 
                            <% for (int i = 1; i < entities3.Count(); i++)
                               { %>
                            <% string[] properties = entities3[i].Split('Ä');
                               if (properties.Length <= 5) continue;
                            %>
                            <a href="<%= Url.Action("ChangeProduct", "ContentPortal")+ "?Industry=" +industryId + "&Competitor=" +properties[1]+ "&Product=" +properties[4]%>">
                                <%--<a href="#" onclick='selectIndustry();'>--%>
                                <%= properties[5]%></a>
                            <% if (i < (entities3.Count()) - 2)
                               { %>
                            ,
                            <% }
                               else if (i == (entities3.Count()) - 2)
                               { %>
                            and
                            <%} %>
                            <%} %>
                        for <a href="<%= Url.Action("ChangeIndustry", "ContentPortal")+ "?Industry=" +industryId %>">
                            <%= industry[2]%></a> Industry
                        <% }
                                           }
                           else if (actionAllView.EntityAction.Equals("Compared competitors") && (actionAllView.EntitiesOfAction.IndexOf('Å') != -1))
                           { %>
                        <% string[] entities2 = actionAllView.EntitiesOfAction.Split('Å'); %>
                        <% string[] industry = entities2[0].Split('Ä');
                           string industryId = industry[1];%>
                        <a href="<%= Url.Action(actionAllView.Action,actionAllView.Controller, new { ComparisonType = ComparisonType.Competitors}) %>">
                            <%= actionAllView.EntityAction%></a> the
                        <% for (int i = 1; i < entities2.Count(); i++)
                           { %>
                        <% string[] properties = entities2[i].Split('Ä');
                        %>
                        <a href="<%= Url.Action("ChangeCompetitor", "ContentPortal")+ "?Industry=" +industryId + "&Competitor=" +properties[1]%>">
                            <%= properties[2]%></a>
                        <% if (i < (entities2.Count()) - 2)
                           { %>
                        ,
                        <% }
                           else if (i == (entities2.Count()) - 2)
                           { %>
                        and
                        <%} %>
                        <%} %>
                        for <a href="<%= Url.Action("ChangeIndustry", "ContentPortal")+ "?Industry=" +industryId %>">
                            <%= industry[2]%></a> Industry
                        <% } %>
                        <% 
                            else if (actionAllView.EntityAction.Equals("exported"))
                            {%>      
                            
                            <% string[] entities3 = actionAllView.EntitiesOfAction.Split('Å'); 
                        %>
                        <% if (entities3.Length >= 2)
                           { %>
                        <% string[] industry = entities3[0].Split('Ä');
                           string industryId = industry[1];%>
                        
                            <%= actionAllView.EntityAction%>                             
                             
                            <a href="<%= Url.Action("Index", actionAllView.Controller) %>">                                                        
                            <%=actionAllView.Controller%> </a> output for the
                            
                        <div id="Div1">
                            <% for (int i = 1; i < entities3.Count(); i++)
                               { %>
                            <% string[] properties = entities3[i].Split('Ä');
                               if (properties.Length <= 5) continue;
                            %>
                            <a href="<%= Url.Action("ChangeProduct", "ContentPortal")+ "?Industry=" +industryId + "&Competitor=" +properties[1]+ "&Product=" +properties[4]%>">
                                
                                <%= properties[5]%></a>
                            <% if (i < (entities3.Count()) - 2)
                               { %>
                            ,
                            <% }
                               else if (i == (entities3.Count()) - 2)
                               { %>
                            and
                            <%} %>
                            <%} %>
                        </div>
                        for <a href="<%= Url.Action("ChangeIndustry", "ContentPortal")+ "?Industry=" +industryId %>">
                            <%= industry[2]%></a> Industry
                        <% }%>
                                                
                        <% } %>
                        <% } //errror
                            else if (actionAllView.EntityAction.Equals("Commented") && actionAllView.Controller.Equals("Forum"))
                            { %>
                        <% if (!DefaultsDisabPublComm)
                           { %>  
                        <a href="javascript:void(0)" onclick="CommentFormDlg('<%=Url.Action("GetComments", "Forum",new {EntityId=actionAllView.EntityId,ObjectType=DomainObjectType.Project},null)%>','Comment Form','<%=Url.Action("CommentsResponse", "Forum",new {EntityId=actionAllView.EntityId,ForumResponseId=0,ObjectType=DomainObjectType.Project},null)%>')">
                            <%= actionAllView.EntityAction%></a>
                            <% }
                           else
                           { %>
                            <%= actionAllView.EntityAction%>
                            <% } %>
                             on the
                        <%if (!string.IsNullOrEmpty(actionAllView.EntitiesOfAction))
                          { %>
                        <% string[] entities = actionAllView.EntitiesOfAction.Split('Å');
                           string[] industry;
                           string[] competitor;
                           string[] product;
                           if (entities.Length == 1)
                           { %>
                        <% industry = entities[0].Split('Ä');   %>
                        <a href="<%= Url.Action("ChangeIndustry", "ContentPortal")+ "?Industry=" +industry[1] %>">
                            <%= actionAllView.TypeName%></a>
                        <% }
                                                        if (entities.Length == 2)
                                                        { %>
                        <%  industry = entities[0].Split('Ä');
                            competitor = entities[1].Split('Ä');  %>
                        <a href="<%= Url.Action("ChangeCompetitor", "ContentPortal")+ "?Industry=" +industry[1] + "&Competitor=" +competitor[1]%>">
                            <%= actionAllView.TypeName%></a>
                        <% }
                                                        if (entities.Length == 3)
                                                        { %>
                        <% industry = entities[0].Split('Ä');
                           competitor = entities[1].Split('Ä');
                           product = entities[2].Split('Ä'); %>
                        <a href="<%= Url.Action("ChangeProduct", "ContentPortal")+ "?Industry=" +industry[1] + "&Competitor=" +competitor[1]+ "&Product=" +product[1]%>">
                            <%= actionAllView.TypeName%></a>
                        <% } %>
                        <% }
                          else
                          { %>
                        <%= actionAllView.TypeName%>
                        <% } %>
                        <%= actionAllView.Type%>
                        <% }
                           //error                                                          
                    else if (actionAllView.EntityAction.Equals("answered"))
                       { %> answered 
                    <a href="<%= Url.Action(actionAllView.Action ,actionAllView.Controller) + "/"+ actionAllView.EntityId %>">
                        <%= actionAllView.TypeName %></a> <%= actionAllView.Type%>
                        
                    <% }
                       else if (actionAllView.Controller.Equals("Competitor"))
                       {
                           string[] entities = actionAllView.EntitiesOfAction.Split('Å');
                           string industryId = string.Empty;
                           string industryName = string.Empty;
                           bool existComment = false;
                           if (entities.Length >= 1)
                           {
                               if (!string.IsNullOrEmpty(entities[0]))
                               {
                                   existComment = true;
                                   string[] industryObject = entities[0].Split('Ä');
                                   if (industryObject.Length >= 3)
                                   {
                                       industryId = industryObject[1];
                                       industryName = industryObject[2];
                                   }
                               }    
                           }
                         %>
                        <% if (actionAllView.EntityAction.Equals(EntityAction.Commented))
                           {  %>
                         <% if (!DefaultsDisabPublComm)
                            { %>  
                         <a href="javascript:void(0)" onclick="GetExternalDiscussionsDlg('<%=Url.Action("GetDiscussions", "Forum",new {EntityId=actionAllView.EntityId,ObjectType=DomainObjectType.Competitor},null)%>','Discussions Form','<%=Url.Action("DiscussionsResponse", "Forum",new {EntityId=actionAllView.EntityId,ForumResponseId=0,ObjectType=DomainObjectType.Competitor},null)%>','<%=  actionAllView.EntityId %>')">
                         <%= actionAllView.EntityAction%></a>
                         <% }
                            else
                            { %>
                         <%= actionAllView.EntityAction%>
                         <% } %>
                         <% }
                           else if (actionAllView.EntityAction.Equals(EntityAction.FeedBack))
                           { %>
                         <a href="javascript: void(0);"  onclick="ExternalFeedBackWithAttachedDlg('<%= Url.Action("FeedBackMessage", "Forum", new {EntityId=actionAllView.EntityId, EntityType=DomainObjectType.Competitor, IndustryId=actionAllView.EntityId, SubmittedVia=FeedBackSubmitedVia.ActivityLog })%>','FeedBack Dialog' );">
                         <%= actionAllView.EntityAction %></a>
                         <% } else { %>
                        <%= actionAllView.EntityAction %>
                        <% } %>
                        <% if (existComment)
                           { %>
                        on Strengths / Weaknesses for 
                        <a href="<%= Url.Action("ChangeCompetitor", "ContentPortal")+ "?Industry=" + industryId + "&Competitor="+ actionAllView.EntityId %>">
                        <%= actionAllView.TypeName%>
                        </a>
                                <%= actionAllView.Type%> in 
                         <a href="<%= Url.Action("ChangeIndustry", "ContentPortal")+ "?Industry=" + industryId %>">
                         <%= industryName%> 
                         </a>
                         Industry
                         <% }
                           else
                           { %>
                           the 
                           <%if (!string.IsNullOrEmpty(industryId))
                             { %>
                           <a href="<%= Url.Action("ChangeCompetitor", "ContentPortal")+ "?Industry=" + industryId + "&Competitor="+ actionAllView.EntityId %>">
                                    <%= actionAllView.TypeName%></a>
                                    <%}
                             else
                             { %>
                                    <%= actionAllView.TypeName%>
                                    <% } %>
                                <%= actionAllView.Type%>
                           
                         <%  } %>
                         <%  
                       } 
                        else if (!actionAllView.Controller.Equals("DealSupport") && !actionAllView.Controller.Equals("ContentPortal") && !actionAllView.Controller.Equals("Forum"))
                            { %>
                    <% if (actionAllView.EntityAction.Equals("Updated"))
                       {%>
                    <a href="<%= Url.Action("Edit",actionAllView.Controller) + "/"+ actionAllView.EntityId %>">
                        <%= actionAllView.EntityAction%></a>
                    <% }
                          
                       else {%> 
                    <a href="<%= Url.Action(actionAllView.Action,actionAllView.Controller) + "/"+ actionAllView.EntityId %>">
                        <%= actionAllView.EntityAction%></a>
                    <%} %>
                    the <a href="<%= Url.Action("Edit",actionAllView.Controller) + "/"+ actionAllView.EntityId %>">
                        <%= actionAllView.TypeName%></a>
                    <%= actionAllView.Type%>
                    <% }
                       else if (actionAllView.Controller.Equals("ContentPortal"))
                       {%>
                            <% if (actionAllView.EntityAction.Equals(EntityAction.Downloaded) && !string.IsNullOrEmpty(actionAllView.EntitiesOfAction))
                               {
                                   string projectName = string.Empty;
                                   decimal projectId = 0;
                                   int posFlag = actionAllView.EntitiesOfAction.IndexOf('Ä');
                                   if (posFlag != -1)
                                   {
                                       string[] actionsOptions = actionAllView.EntitiesOfAction.Split('Ä');
                                       if (actionsOptions.Length == 3)
                                       {
                                           projectName = actionsOptions[2];
                                           if (!string.IsNullOrEmpty(actionsOptions[1]))
                                           {
                                               projectId = Decimal.Parse(actionsOptions[1]);
                                           }
                                       }
                                   }
                                   %>
                               viewed <a onclick="return downloadFile('<%=Url.Action("Download", "ContentPortal") + "/" + projectId %>');" href="javascript:void(0)"> <%= projectName%> </a> File
                    <%}
                               else if (actionAllView.EntityAction.Equals(EntityAction.FeedBack))
                               {%>
                                   <%= EntityAction.FeedBack%>  
                                  <% if (actionAllView.EntityType.Equals(DomainObjectType.Project))
                                     { %>
                                        <a href="javascript: void(0);" onclick="ExternalFeedBackWithAttachedDlg('<%= Url.Action("FeedBackMessage", "Forum", new {EntityId=actionAllView.EntityId, EntityType=DomainObjectType.Project, SubmittedVia=FeedBackSubmitedVia.ActivityLog }) %>','FeedBack Dialog');"><%= actionAllView.TypeName %></a>
                                        Project
                                  <%} %>
                      <% }} %>
                    <% } %>
                <% else
                   {%>
                        <% if (actionAllView.EntityAction.Equals(EntityAction.SetedRating) && !string.IsNullOrEmpty(actionAllView.EntitiesOfAction) && (actionAllView.EntitiesOfAction.IndexOf('Å') != -1))
                   {
                       int posRated = actionAllView.Description.IndexOf("with");
                       int descriptionLength = actionAllView.Description.Length;
                       int length = descriptionLength - posRated;%>
                rated the
                <%if (!string.IsNullOrEmpty(actionAllView.EntitiesOfAction))
                  { %>
                <% string[] entities = actionAllView.EntitiesOfAction.Split('Å');
                   string[] industry;
                   string[] competitor;
                   string[] product;
                   if (entities.Length == 1)
                   { %>
                <% industry = entities[0].Split('Ä');   %>
                <a href="<%= Url.Action("ChangeIndustry", "ContentPortal")+ "?Industry=" +industry[1] %>">
                    <%= actionAllView.TypeName%></a>
                <% }
                                    if (entities.Length == 2)
                                    { %>
                <%  industry = entities[0].Split('Ä');
                    competitor = entities[1].Split('Ä');  %>
                <a href="<%= Url.Action("ChangeCompetitor", "ContentPortal")+ "?Industry=" +industry[1] + "&Competitor=" +competitor[1]%>">
                    <%= actionAllView.TypeName%></a>
                <% }
                                    if (entities.Length == 3)
                                    { %>
                <% industry = entities[0].Split('Ä');
                   competitor = entities[1].Split('Ä');
                   product = entities[2].Split('Ä'); %>
                <a href="<%= Url.Action("ChangeProduct", "ContentPortal")+ "?Industry=" +industry[1] + "&Competitor=" +competitor[1]+ "&Product=" +product[1]%>">
                    <%= actionAllView.TypeName%></a>
                <% } %>
                <% } %>
                <% if (posRated != -1)
                   { %>
                <% string ratedstring = actionAllView.Description.Substring(posRated, length); %>
                <%= ratedstring %>
                <% } %>
                <% }else if (actionAllView.EntityAction.Equals(EntityAction.SetedRating) && string.IsNullOrEmpty(actionAllView.EntitiesOfAction) && actionAllView.EntityType.Equals("PROJT")){
                       int posTypeName = actionAllView.Description.IndexOf(actionAllView.TypeName);
                       string subDescription = actionAllView.Description.Substring(posTypeName + actionAllView.TypeName.Length);
                       %>
                        rated the 
                        <a href="#ap<%= actionAllView.EntityId%>"> <%= actionAllView.TypeName%></a>
                        <%= subDescription%>
                   
                    <% }else{%>
                       
                        <%= actionAllView.Description%>
                    <% } %>        
                <% } %>
                <% } %>
                <br />
            </li>
        </ul>
     </div>
 <% } %>

      
      

