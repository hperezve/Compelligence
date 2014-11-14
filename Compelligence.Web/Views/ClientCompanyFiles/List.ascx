<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<style>
  .HeaderName
    {
        background-color: #EEEEEE;
        border: 1px solid #AAAAAA;
        font: bold 11px tahoma,arial,helvetica;
        height: 18px;        
        width: 250px;
        _width: 300px;
    }
    
    .HeaderDescription
    {
        background-color: #EEEEEE;
        border: 1px solid #AAAAAA;
        font: bold 11px tahoma,arial,helvetica;
        height: 18px;        
        width: 350px;
        _width: 387px;
    }
    .HeaderActions
    {
        background-color: #EEEEEE;
        border: 1px solid #AAAAAA;
        font: bold 11px tahoma,arial,helvetica;
        height: 18px;        
        width: 40px;
        _width: 90px;
    }
     .HeaderDate
    {
        background-color: #EEEEEE;
        border: 1px solid #AAAAAA;
        font: bold 11px tahoma,arial,helvetica;
        height: 18px;        
        width: 140px;
        _width: 90px;
    }
    
</style>  
<div>
<%IList<ClientCompanyFiles> ClienCompanyFileList = (IList<ClientCompanyFiles>)ViewData["ClientCompanyFile"]; %>        
    <table style="  margin-left: auto; margin-right: auto;">
                <thead>                
                    <tr>
                        <th class="HeaderName">
                         File Name
                        </th>
                        <th  class="HeaderDescription">
                         Description
                        </th>
                        <th class="HeaderDate">
                         Date Upload
                        </th>
                        <th class="HeaderActions">
                         Actions
                        </th>                         
                    </tr>                    
                </thead>             
                         
        <%int x = 0;
          foreach (ClientCompanyFiles clientCompanyFile in ClienCompanyFileList)     
        {  x = x + 1;
        %>                
                <tr style=" font-size:12px"> 
                               
                    <td style="border:0px">
                      <div id="Div1">
                      <img alt="ok" id="OK" src="<%= Url.Content("~/Content/Images/Styles/ok.png") %>" style="border:solid 1px #FFF;" />           	          
                      <a href="<%=Url.Action("Download", "ClientCompanyFiles") + "/" + clientCompanyFile.Id%>"> <%=clientCompanyFile.Name%></a>	
           	          </div>             
           	        </td>
                    
                    <td style="border:0px"> <%=clientCompanyFile.Description%></td>
                    <td style="border:0px"> <%=clientCompanyFile.CreatedDate%></td>
                    <td style="border:0px">
                    <div id="List_Action" class="List_Action">
    
                     <a href="<%= Url.Action("Edit", "ClientCompanyFiles")+ "/" +  clientCompanyFile.Id%>"/>
                     <img alt="Edit" src="<%= Url.Content("~/Content/Images/Styles/write.png") %>" style="border:solid 1px #FFF;" /> </a>
                	 
                     <a href="#" onclick="ConfirmDeleteClientCompanyFile('<%= Url.Action("Delete", "ClientCompanyFiles") + "/" + clientCompanyFile.Id%>');return false;">
                     <img alt="Delete" src="<%= Url.Content("~/Content/Images/Styles/list_remove.png") %>" style="border:solid 1px #FFF;" /> </a>               
	                
	                </div>	
                    </td>
                    </tr>               
                    <%} %>                   
           </table> 

 </div>
 <div id="FormMessages">
</div>
