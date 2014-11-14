<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>
<%@ Import Namespace="Compelligence.DataTransfer.Comparinator" %>
<script type='text/javascript'>
    // initialize tooltips in a seperate thread
    $(document).ready(function() {
        window.setTimeout(hovertipInit, 1);
        $("#txtFilter").keyup(function() {
            filterRows();
        });
        CollapseCriteriaSet();
    });
    
</script>


<%IList<Competitor> Competitors = (IList<Competitor>)ViewData["Competitors"]; %> 
 
<div style="width: 100%">
    <div style="text-align:center;width:100%">&nbsp;</div>
    <div style="text-align:center;width:100%"><h2>COMPARINATOR RESULT</h2></div>
    <div class="comp_rounding">
        <table style="width: 100%">
            <tr>
                <td>
                    <label>
                        Filter :
                    </label>
                    <input id="txtFilter" type="text" value="" />
                </td>
                <td style="width:60%"></td>
                <td>            <a href="<%= Url.Action("Export", "Comparinator", new { Type=2}) %>"><img alt="Export to CSV format" src="<%= Url.Content("~/Content/Images/Icons/icon_csv.png")%>" width='22px'/>Export to CSV format</a>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <%UserProfile user = (UserProfile)ViewData["User"];%>
    <%string tblWidth = "width:" + (Competitors.Count * 330 + 500).ToString() + "px";%>
    <%foreach (var oRow in (IEnumerable<ComparinatorGroup>)ViewData["ComparinatorGroups"])
      { %>
      <div class="comp_grouphead" style="<%=tblWidth%>">
        <p class="comp_groupheadname"><%=oRow.Name%></p>
      </div>
      <% 
      foreach (var oRowTwo in oRow.ComparinatorSets)
      {%>
      <div class="comp_sethead" style="<%=tblWidth%>">
         <div class='comp_criteriacollapse'></div>
         <div class="comp_setheadname"><%=oRowTwo.Name%></div> 
      </div>
    <table class="comp_table filtered" style="<%=tblWidth%>"> 
        <colgroup>
          <col width='300px'/>
          <%foreach (var competitor in Competitors)
            {%>
              <col width='200px'/>
              <col width='105px'/>
          <%} %>  
          
        </colgroup>
            
        <thead class="comp_head">
            <tr>
                <th> Criteria </th>
                <%int iColumn = 3; %>
                <%foreach (var oCompetitor in Competitors)
                  {%>
                <th>
                    <div style="width: 176px; float: left">
                        <%=oCompetitor.Name %>
                    </div>
                    <div class="comp_expand comp_cehidden" onclick="CollapseColumn(this,<%=iColumn %>,false)" title='Click for expand column'> </div>
                 </th>
                <th>
                  <div style='width:86px; float: left'>Actions</div>
                  <div class='comp_collapse' onclick="CollapseColumn(this,<%=iColumn %>,true)" title='Click for collapse column'></div>
                </th>
                <%iColumn += 2;  
                   }%>
            </tr>
        </thead>
            <tbody>
             <% foreach (var oRowThree in oRowTwo.ComparinatorCriterias)
                {%>                
               <tr>
                    <td>
                        <%=oRowThree.Criteria.Name%>
                    </td>
                    <% int diff = 0;

                    foreach (var value in oRowThree.Values)
                    {
                           diff++;
                    %>
                    <td id="val<%=oRowThree.Criteria.Id+diff.ToString() %>">
                        <%=oRowThree.ToValue(value)%>
                    </td>
                    <td>
                        <%
                           string noteIconClass = "";
                           string noteNull = "";
                                                     
                           if (oRowThree.ToNote(value) == noteNull)
                           {noteIconClass = "notehidden";                                                        
                           }                                                       
                           else 
                           {noteIconClass = "noteshow";}                                          
                        %>
                        <div class=<%=noteIconClass%> id="Note<%=oRowThree.Criteria.Id+diff.ToString() %>"></div>
                        
                        
                        <div class="hovertip" target="Note<%=oRowThree.Criteria.Id+diff.ToString()%>">
                            <h1>
                                Notes Entered</h1>
                            <div><%=oRowThree.ToNote(value)%></div>
                        </div>
                        
                        <%
                            string linkIconClass = "";
                            string iconNull = "";

                            string linkcontents = oRowThree.ToLink(value);
                        %>  

                        <%
                            if (oRowThree.ToLink(value).Trim().Length == 0)
                            {
                                linkIconClass = "linkhidden";
                            }
                            else {
                                linkIconClass = "linkshow";
                            }
                           
                        %>
                        
                        <div class=<%=linkIconClass%> id="Links<%=oRowThree.Criteria.Id+diff.ToString() %>"></div>
                        <div class="hovertip" target="Links<%=oRowThree.Criteria.Id+diff.ToString()%>">
                            <h1>
                                Links</h1>
                            <div><%=oRowThree.ToLink(value)%></div>
                        </div>
                        
                          
                        <a href="javascript:void(0)" onclick="CommentFormDlg('<%=Url.Action("GetComments", "Forum",new {EntityId=oRowThree.ToId(value),ObjectType=DomainObjectType.CompetitorCriteria},null)%>','Comment Form','<%=Url.Action("CommentsResponse", "Forum",new {EntityId=oRowThree.ToId(value),ForumResponseId=0,ObjectType=DomainObjectType.CompetitorCriteria},null)%>')">
                           <img src="<%=Url.Content("~/Content/Images/Icons/testforum.gif") %>" width="22px" alt="Comments"/> 
                        </a>
                        <a href="javascript:void(0)" onclick="FeedBackFormDlg('<%=Url.Action("SendFeedBack", "Comparinator",new {Id=oRowThree.ToId(value),objt=DomainObjectType.CompetitorCriteria},null)%>');">
                            <img src="<%=Url.Content("~/Content/Images/Icons/testfeedback.gif") %>" width="22px"
                                alt="FeedBack" />
                        </a>
                        
                        <%if (user.SecurityGroupId != "ENDUSER")
                          { %>
                        <img src="<%=Url.Content("~/Content/Images/Icons/properties.png") %>" width="22px" alt="Properties" onclick="AddDialog('<%=oRowThree.Criteria.Description+"/"+Competitors[diff-1].Name %>','<%=Url.Action("SaveValue","Comparinator") %>','<%=ViewData["IndustryId"] %>','<%=oRowThree.ToType(value) %>','<%=oRowThree.Criteria.Id %>','<%=oRowThree.ToEntityId(value) %>','<%=oRowThree.Criteria.Id+diff.ToString() %>')" />
                        <%} %>
                    </td>
                    <%} %>
                </tr>
                <% } %>
            </tbody>
        </table>
    
       <% } %>
    <% } %>

</div>
