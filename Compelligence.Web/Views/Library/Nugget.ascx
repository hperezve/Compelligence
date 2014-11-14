<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<%IList<Pair<string, string>> questionAnswers = (IList<Pair<string, string>>)ViewData["QuestionAnswers"]; %>
<br />
<h1><%=ViewData["Title"] %></h1>
<br />
<fieldset>

<% foreach( Pair<string,string> q in questionAnswers)
   { %>
   
        <div class="line">
                <div class="field">
                   <label><%=q.First %></label>
                   <textarea><%=q.Second %></textarea>
                
                </div>
         </div>
    
   
  <%} %>

</fieldset>