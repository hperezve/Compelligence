<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>

<% var ProductCollection = (IList<Product>)ViewData["Products"];
    string c = (string)ViewData["C"];
    string DefaultsSocialLog = (string)ViewData["DefaultsSocialLog"];%>
<% var RecomentProductCollection = (IList<Compelligence.DataTransfer.Comparinator.ComparinatorRecommendedProducts>)ViewData["RecommendProducts"]; %>

<% if( ProductCollection!=null && ProductCollection.Count>0) 
   {%>

   <div class="comp_block">
    <div class="comp_blockt">
	  <label class="comp_row_title"><%=ViewData["ProductLabel"].ToString().ToUpper()%></label>      
    </div> 
    <div style="float:left;">
     <%       
  foreach (Product oProduct in ProductCollection)
  {
      %>      
          <div class="comp_elements" style="text-align:center;float:left;">
                <div class="comp_top_title">
                    <div><%=oProduct.Competitor.Name%><img src="<%=Url.Content("~/Content/Images/Icons/close-gray.gif") %>" onclick="RemoveProduct('<%=Url.Action("RemoveProduct", "Comparinator",new { ProductId=oProduct.Id}) %>','FormProducts');return false" />&nbsp;</div>
                </div>
                <div>
                     <div class="tdText2"><%=oProduct.Name%></div>
                </div>
                <div class="default_image_area">
                    <div class="tdImage">
                        <img src="<%=Html.ImageUrl(oProduct.ImageUrl)%>" alt = "" class="imgProducts"/>
                    </div>
                </div>
          </div>    
        <% } %>  
  </div>
</div>
<%} %>
 

<% if (RecomentProductCollection != null && RecomentProductCollection.Count>0)
   { %>

    <div id="DivRecommendProducts" class="comp_block">
     <div class="comp_blockt">
   	    <label class="comp_row_title">RECOMMENDED <%=ViewData["CompetitorLabel"].ToString().ToUpper()%> <%=ViewData["ProductLabel"].ToString().ToUpper()%> BASED ON SELECTED <%=ViewData["ProductLabel"].ToString().ToUpper()%></label>
   	</div>
    <div style="float:left;">
    <% foreach (Compelligence.DataTransfer.Comparinator.ComparinatorRecommendedProducts objt in RecomentProductCollection)
  { %>   
          <div class="comp_elements" style="text-align:center;float:left;">
                <div class="comp_top_title">
                    <div><%=objt.CompetitorName%></div>
                </div>
                <div>
                     <div class="tdText"><%=objt.ProductName%></div>
                </div>
                <div class="default_image_area">
                    <div class="tdImage">
                        <img src="<%=Html.ImageUrl( objt.ProductImageUrl )%>" class="imgProducts"/>
                    </div>
                </div>
                <div class="removebutton" style="margin-top: 13px;">
                    <input id="Button1" type="button" value="Add" class="button" onclick="javascript:AddProductAdvance('<%=objt.CompetitorName.Replace("'", @"\'")%>','<%=objt.ProductId %>','<%= Url.Action("AddProductOfRecommended","Comparinator") %>','<%=ViewData["C"]%>', '<%= Url.Action("GetProductsByCompetitor", "Comparinator") %>');"/>
                </div>
          </div> 
        <% } %>   
    </div>
</div>
<%} %>
