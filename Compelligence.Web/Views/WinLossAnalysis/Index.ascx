<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="WinLossForm">
    <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>','<%= Compelligence.Domain.Entity.Resource.Pages.WinLossAnalysis %>','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','Tools:Win/Loss Analysis');" style="float: right;margin-right:5px;margin-top:5px;"/>
<div class="line">
    <div class="field">
        <label class="required">
            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:LabelResource, WinLossAnalysisCompetitor %>" />:</label>
        <select id="competitor"></select>
    </div>  
 </div>
<div class="line">
    <div class="field">
        <label> Start date:</label>
        <input type="text" id="startdate"/>
    </div>  
    <div class="field">
        <label> End date:</label>
        <input type="text" id="enddate"/>
    </div>  
 </div>
<div class="line">
    <div class="field">
        <input type="button" value="Execute" class="button" onclick="WinLossExecute()"/>
    </div>  
 </div> 
<div class="line">
    <div class="field">
        <label> WIN / LOSS %:</label>
        <input type="text" id="percent"/>
    </div>  
    <div class="field">
        <label> WIN / LOSS Total Dollar Amount:</label>
        <input type="text" id="amount"/>
    </div>  
 </div>
 <div class="line">
   <br />
 </div>
</div>
<script type="text/javascript">

function WinLossExecute() {
    var winlossform = $("#WinLossForm")
    var competitor = winlossform.find("#competitor").val();
    var startdate = winlossform.find("#startdate").val();
    var enddate = winlossform.find("#enddate").val();
    var urlAction='<%= Url.Action("Execute", "WinLossAnalysis") %>';
    showLoadingDialog();
    $.getJSON(urlAction, { competitor: competitor, startdate: startdate, enddate: enddate }, function(data) {
        winlossform.find("#percent").val(data.percent);
        winlossform.find("#amount").val(data.amount);
        hideLoadingDialog();
    });


}

$(function() {
    var winlossform = $("#WinLossForm")
    var competitor = winlossform.find("#competitor");
    $(winlossform.find("#startdate")).datepicker(
                                  {
                                      changeMonth: true,
                                      changeYear: true
                                  });
    $(winlossform.find("#enddate")).datepicker(
                                  {
                                      changeMonth: true,
                                      changeYear: true
                                  });

    var urlAction = '<%= Url.Action("GetAllCompetitors", "WinLossAnalysis") %>';
    $.getJSON(urlAction, {}, function(data) {
        $.each(data, function(i, item) {
            competitor.append("<option value=" + item.Value + " >" + item.Text + "</option>");
        });
    });

});
</script>