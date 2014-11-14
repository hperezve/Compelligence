<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>



<script type="text/javascript">
    $(function() {
        $('.whitePaperFiles').click(function() {
            $("#Message").show();
        });
    });
</script>

<style type="text/css">

.ContentWhitePaper {        
    color: #666666;
    cursor: pointer;
    font-size: 14px;
    font-weight: bold;   
    width: 700px;
    margin:40px;
}
/*CSS LINKS*/
a.whitePaperFiles 
{
    color: #FC6703;
    font-weight: bold;
    text-decoration: underline;
}
a.whitePaperFiles:link 
{
    text-decoration: underline;
}
a.whitePaperFiles:visited {
    color: #FC6703;
    font-weight: bold;
    text-decoration: underline;
}
a.whitePaperFiles:hover 
{
    text-decoration: none;
    color:#FF9622
}
a.whitePaperFiles:active 
{
    text-decoration: underline;
    color: #000000;
} 
</style>
<script type="text/javascript">
    $(document).ready(function() {
        $("#FormDatos").dialog({
        autoOpen: false,
            _height: 750,            
            width: 847,
            modal: true,
            buttons:
			{
			    'Close Statistics': function() {
			        $(this).dialog('close');
			    }
			}
        });
    });
</script>
<script type="text/javascript">
    function LoadDownloads(url) {
        $('#FormDatos').load(url, function() {
            $("#FormDatos").dialog("open");

        });
    }
</script>

<style type="text/css">
    .HeaderWhitePaperName
    {
        background-color: #EEEEEE;
        border: 1px solid #AAAAAA;
        font: bold 11px tahoma,arial,helvetica;
        height: 18px;        
        width: 280px;
        _width: 300px;
    }
    
    .HeaderSummary
    {
        background-color: #EEEEEE;
        border: 1px solid #AAAAAA;
        font: bold 11px tahoma,arial,helvetica;
        height: 18px;        
        width: 380px;
        _width: 387px;
    }
    .Actions
    {
        background-color: #EEEEEE;
        border: 1px solid #AAAAAA;
        font: bold 11px tahoma,arial,helvetica;
        height: 18px;        
        width: 70px;
        _width: 90px;
    }
    #DownloadPaper
    {                        
        height: auto;
        _height: auto;
        margin-left: 31px;
        margin-top: 4px;
        width: 257px;
        _margin-left: 252px;
        font-size:12px;
        font-family:Arial;
    }
    #SummaryData
    {                        
        height: auto;
        margin-left: 280px;
        _margin-left: 544px;
        margin-top: -18px;
        width: 310px;
        font-size:12px;
        font-family:Arial;
    }
    .List_Action
    {
        height: 18px;
        margin-left: 10px;
    }
    
    #ImageDownloaderPaper
    {
        float: left;
        height: 27px;
        margin-left: -1px;
        _margin-left: 226px;       
        width: 24px;
    }
    #Message
    {
    	text-align:center;
    	font-family:arial;
    	font-size:12px;
    	display:none;
    	width:596px;
    	margin-left:4px;
    	font-weight: bold;
    }
    
    #ContentInformation
    {    	
    	font-family:arial;
    	font-size:12px;    	    	
    	margin-left:auto;
    	margin-right:auto;
    	
    }
    
</style>
<%IList<WhitePaper> whitepapers=(IList<WhitePaper>)ViewData["WhitePapers"] ; %>        

 <div id="ContentInformation">
  <br/> 
  <div id="Message" style="margin-left: auto; margin-right: auto;">
        Thank you for downloading our white paper. Please feel free to leave us feedback via the <a class="whitePaperFiles" href="http://www.compelligence.com/contact.html">contact page.</a> 
  </div>  
  <br/>
  
  <div id="PleaseBrowse" style="text-align: center;">
    Please browse through our collection of white papers that will provide insight and tips on a variety of Competitive Intelligence topics.
  </div>
  <br/>	
          <table style="  margin-left: auto; margin-right: auto;">
                <thead>                
                    <tr>
                        <th class="HeaderWhitePaperName">
                          White Paper Name
                        </th>
                        <th class="HeaderSummary">
                         Summary
                        </th>
                        <th class="Actions">
                          Actions
                        </th>                        
                    </tr>                    
                </thead>             
                         
        <%int x = 0;
        foreach (WhitePaper WhiteP in whitepapers)     
        {  x = x + 1;
        %>                
                <tr style=" font-size:12px"> 
                               
                    <td style="border:0px">
                      <div id="Div1">
                      <img alt="ok" id="OK" src="<%= Url.Content("~/Content/Images/Styles/ok.png") %>" style="border:solid 1px #FFF;" />           	          
                      <a class="whitePaperFiles" href="<%=Url.Action("Download", "WhitePaper") + "/" + WhiteP.Id%>"> <%=WhiteP.Label%></a>	
           	          </div>             
           	        </td>
                    
                    <td style="border:0px"> <%=WhiteP.Summary%></td>
                    <td style="border:0px">
                    <div id="List_Action" class="List_Action">
    
                     <a href="<%= Url.Action("Update", "WhitePaper")+ "/" +  WhiteP.Id%>"/>
                     <img alt="Edit" src="<%= Url.Content("~/Content/Images/Styles/write.png") %>" style="border:solid 1px #FFF;" /> </a>
                	 
                     <a href="#" onclick="ConfirmDeleteWhitePaper('<%= Url.Action("DeleteWhitePaper", "WhitePaper") + "/" + WhiteP.Id%>');return false;">
                     <img alt="Delete" src="<%= Url.Content("~/Content/Images/Styles/list_remove.png") %>" style="border:solid 1px #FFF;" /> </a>
               
	                 <a href="#" onclick="javascript:LoadDownloads('<%= Url.Action("WhiteDownloads", "WhitePaper") + "/" + WhiteP.Id%>');return false"> 
	                 <img alt="statistics" width="17px" src="<%= Url.Content("~/Content/Images/Styles/statistics.png") %>" style="border:solid 1px #FFF;"/> </a>                
	                </div>	
                    </td>
                    </tr>               
                    <%} %>                   
            </table>                    
    
    <div id="DownloadPaper">                
    </div>  
         
<div id="FormDatos" title="White Papers Downloads">	   
</div>
</div>	
<div id="FormMessages">
</div>
