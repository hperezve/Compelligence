<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import namespace="Compelligence.Domain.Entity"  %>

<%ProductCriteria pc = (ProductCriteria)ViewData["ProductCriteria"];%>
<%string C = (string)ViewData["C"];%>
<%string U = (string)ViewData["U"];%>
<script type="text/javascript">
    function clearTextBox(element, type) {
        if (element.value == 'N/A' && type == '<%= Compelligence.Domain.Entity.Resource.CriteriaType.Numeric %>') {
            $('#' + element.id).val('');
        }
    };
    function isNumbersAndPointKeyOrValue(event, element, _float) {
        event = event || window.event;
        var charCode = event.which || event.keyCode;
        var patronA = /[\^(\d|.)]/; //THIS PATRON IS TO NUMERIC AND POINT
        var patronB = /([nN])|([nN]+\/)/; //TO SEARCH /N/A
        var patronC = /[a-zA-Z\/]/; //TO ALL CHARACTER AND /
        var result = false;
        // TO FUTURE
        // shift + 5 = % = 37 , event.shiftKey = TRUE if key press shift
        // shift + 4 = $ = 36
        //             / = 47
        // left arrow    = 37
        // apostrophe ( ' )  = 39 , event.keyCode= 0 & event.which=39 [FireFox]
        // apostrophe ( ' )  = 39 ,  event.keyCode= 39 & event.which=undefined [IE]
        // right arrow  = 39, event.keyCode= 39 & event.which=0
        // backspace=8, delete = 46
        // left arrow, right arrow no working to IE and Safary    
        if (charCode == 8 || charCode == 13 || (charCode == 39 && (event.keyCode == 39 && event.which == 0) && !patronB.test(element.value)) || (!event.shiftKey && charCode == 37 && !patronB.test(element.value)) || (_float ? (element.value.indexOf('.') == -1 ? charCode == 46 : false) : false))
            result= true;
        else if ((((_float ? ((element.value.indexOf('A') == -1 && element.value.indexOf('a') == -1) ? charCode == 65 : false) : false) && (element.value != '' && (element.value == 'n/' || element.value == 'N/'))) ||
                  ((_float ? ((element.value.indexOf('A') == -1 && element.value.indexOf('a') == -1) ? charCode == 97 : false) : false) && (element.value != '' && (element.value == 'n/' || element.value == 'N/'))) ||
                  (_float ? ((element.value.indexOf('N') == -1 && element.value.indexOf('n') == -1) ? charCode == 110 : false) : false) ||
                  (_float ? ((element.value.indexOf('N') == -1 && element.value.indexOf('n') == -1) ? charCode == 78 : false) : false) ||
                  ((_float ? (element.value.indexOf('/') == -1 ? charCode == 47 : false) : false) && element.value != ''))
                  && !patronA.test(element.value) && (element.value == '' || patronB.test(element.value)))
            result= true;
        else if ((charCode < 48) || (charCode > 57))
            result= false;
        else
            result = !patronC.test(element.value);
        ShowMessage(result);   
        return result;
    };
    function ShowMessage(value) {
        if(value) $('#divMessageAlert').hide();
        else $('#divMessageAlert').show();
    };
</script>
<form id="ComparinatorFormEdit" action="">
  
  <input type="hidden" value="<%=pc.IndustryId %>" name="IndustryId" />
  <input type="hidden" value="<%=pc.CriteriaId %>" name="CriteriaId" />
  <input type="hidden" value="<%=pc.ProductId %>" name="ProductId" />
  <input type="hidden" value="<%=pc.CriteriaType %>" name="txtCriteriaType" />
  <input type="hidden" value="<%=C %>" name="C" />
  <input type="hidden" value="<%=U %>" name="U" />
  <label>Value</label> 
  <% if (pc.CriteriaType.Equals("NUM") )
    {%>
        <input type="text" onkeypress="return isNumbersAndPointKeyOrValue(event, this, true)" value="<%=pc.Value %>" onfocus="LoadAutoComplete('<%=pc.IndustryId %>', '<%=pc.CriteriaId %>')" name="txtValue" id="txtValue" /><br>
 <% }
    else if (pc.CriteriaType.Equals("BOL") ) 
    {
        %>
        <select id="txtValue" name='txtValue'>
          <option value=''> </option>
          <option value='N/A'>N/A</option>
          <option value='No'>No</option>
          <option value='Yes'>Yes</option>
        </select>
   <% }
    else if (pc.CriteriaType.Equals("LIS") ) { %>
        <input type='text' name='txtValue' onfocus="LoadAutoComplete('<%=pc.IndustryId %>', '<%=pc.CriteriaId %>')" value="<%=Server.HtmlEncode(pc.Value) %>" />
   <%} %>
   
 <div id="divMessageAlert" style="display:none;">
    <label id="lblMessageAlert" style="color: rgb(255, 0, 0); font-size: x-small;">System will only accept a number or the value N/A</label><br />
 </div>
 <br />
  <label>Notes:</label><textarea onclick="setClassOnFocus(this)" class="textareadialog" rows="4" cols="50" wrap="SOFT" name="txtNotes"><%=pc.Notes %></textarea><br>
  <label>Links:(include http://)</label><textarea onclick="setClassOnFocus(this)" class="textareadialog" rows="4" cols="50" wrap="SOFT" name="txtLinks"><%=pc.Links %></textarea><br>
  
  
  <% if (!pc.CriteriaType.Equals("NUM") && !pc.CriteriaType.Equals("BOL"))
     { 
         %>
        <label>Ranking</label>
        <select id="CellPropertyFeature" name='Feature'>
        <option value=''></option>
        <option value='BC' >Best in Class</option>
        <option value='MA' >Market Advantage</option>
        <option value='MP' >Market Parity</option>
        <option value='MD' >Market Disadvantage</option>
        <option value='LM' >Lagging Market</option>
        </select>
  <% }%>
  
  <input type="hidden" id="pcValue" value="<%= Server.HtmlEncode(pc.Value)%>" />
  <input type="hidden" id="pcFeature" value="<%= pc.Feature%>" />
</form>

<script type="text/javascript">
    $(function() {
        var curFeatureType = '<%=pc.CriteriaType%>';
        console.log(curFeatureType);
        if (curFeatureType == "BOL")
            $("#txtValue").val($('#pcValue').val());
        if (curFeatureType == "LIS")
            $('#CellPropertyFeature').val('<%=pc.Feature%>');
    });    
</script>
    
    