<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    ViewData["oHeadProduct_Id"] = string.Empty;
 %>
<div style="height:110px; width:200px;">
<div class="comp_chf">
    <div class="redcomp comp_fm bgg" ></div>
       <%=Html.FeatureInput((string[])ViewData["features"], "BC",(string)ViewData["BC"], "javascript:FeatureFilter('" + ViewData["productid"] + "',this)")%>
       <label for="chkbestfeature" class="comp_labels_feature_ranking">Best in Class</label><br />
</div>
<div class="comp_chf">
    <div class="redcomp comp_fm bgl" ></div>
       <%=Html.FeatureInput((string[])ViewData["features"], "MA", (string)ViewData["MA"], "javascript:FeatureFilter('" + ViewData["productid"] + "',this)")%>
       <label for="chkmarketafeature" class="comp_labels_feature_ranking">Market Advantage</label><br />
</div>
<div class="comp_chf">
    <div class="redcomp comp_fm bgw" ></div>
       <%=Html.FeatureInput((string[])ViewData["features"], "MP", (string)ViewData["MP"], "javascript:FeatureFilter('" + ViewData["productid"] + "',this)")%>
       <label for="chkmarketpfeature" class="comp_labels_feature_ranking">Market Parity</label><br />
</div>      
<div class="comp_chf">
    <div class="redcomp comp_fm bgp" ></div>
      <%=Html.FeatureInput((string[])ViewData["features"], "MD", (string)ViewData["MD"], "javascript:FeatureFilter('" + ViewData["productid"] + "',this)")%>
      <label for="chkmarketdfeature" class="comp_labels_feature_ranking">Market Disadvantage</label><br />
</div>      
<div class="comp_chf">
    <div class="redcomp comp_fm bgr" ></div>
      <%=Html.FeatureInput((string[])ViewData["features"], "LM", (string)ViewData["LM"], "javascript:FeatureFilter('" + ViewData["productid"] + "',this)")%>
      <label for="chklaggingfeature" class="comp_labels_feature_ranking">Lagging Market</label>
</div>            
</div>
