<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>

<style>
    .HeaderNumber
    {
        background-color: #EEEEEE;
        border: 1px solid #AAAAAA;
        font: bold 11px tahoma,arial,helvetica;
        height: 18px;       
        margin-top: 10px;       
        padding-top: 2px;
        width: 135px;
        _width:137px;
        
       
    }
    .HeaderClientIp
    {
        border: 1px solid #AAAAAA;
        height: 18px;    
        margin-top: 10px;
        _margin-top: 18px;      
        padding-top: 2px;
        width: 180px;
        _width: 174px;
        background-color:#EEEEEE;
        font: bold 11px tahoma,arial,helvetica;  
             
    }
    .HeaderClientDns
    {
        border: 1px solid #AAAAAA;
        height: 18px;
        margin-top: 10px;
        _margin-top: 18px;        
        padding-top: 2px;
        width: 269px;
        _width: 281px;
        background-color:#EEEEEE;
        font: bold 11px tahoma,arial,helvetica;
      
    }
    .HeaderDownloadedData
    {
        background-color: #EEEEEE;
        border: 1px solid #AAAAAA;
        font: bold 11px tahoma,arial,helvetica;
        height: 18px;
        margin-top: 10px;
        _margin-top: 18px;        
        padding-top: 2px;
        width: 210px;
        _width: 220px;
    }
   
    #WhiteNameLabel
    {
        margin-left: 8px;
        margin-top: -7px;
        width: 139px;
    }
    #WhiteName
    {
        font: bold 12px tahoma,arial,helvetica;
        margin-left: 150px;
        margin-top: -16px;
        width: 428px;
    }
    #DownloadLabel
    {
        margin-left: 8px;
        margin-top: 7px;
        width: 139px;
    }
    #DownloadNumber
    {
        font: bold 12px tahoma,arial,helvetica;
        margin-left: 150px;
        margin-top: -15px;
        width: 100px;
    }
    #ContentData
    {
        height: 250px;
        overflow: auto;
        width: 812px;
        _width: 820px;
        margin-left:8px;
    }
    #ContentAllWhitePaper
    {
        overflow:hidden;
        height:320px;
        _width:auto;
        width: auto;
    }  
</style>
<div id="ContentAllWhitePaper">
    <br />
    <%IList<WhitePaperDetail> WhitePaperDetalDownload = (List<WhitePaperDetail>)ViewData["WhitePaperDetalDownload"];
      int Nro = WhitePaperDetalDownload.Count; %>		
    <div id="WhiteNameLabel">
       White Paper Name 
    </div>
    <div id="WhiteName">
        : <%=ViewData["NameWhite"]%>
    </div>

    <div id="DownloadLabel">
       Download Number 
    </div>	
    <div id="DownloadNumber">
        : <%=Nro%>
    </div>
   
    <br/>
     <div id="ContentData">
     <table>
                <thead>
                
                    <tr>
                        <th class="HeaderNumber">
                             Nro.
                        </th>
                        <th class="HeaderClientIp">
                             Client Ip
                        </th>
                        <th class="HeaderClientDns">
                            Client Dns
                        </th>
                        <th class="HeaderDownloadedData">
                            Downloaded Data
                        </th> 
                    </tr>
                    
                </thead>
               
                
          <%  int x = 0;
              foreach (WhitePaperDetail WhiteDownload in WhitePaperDetalDownload)
              {
                  x = x + 1;
                  %>  
               
                <tr style=" font-size:12px">                
                    <td style="border:0px">
                      <div id="Div1">
           	          <img id="Img1" src="<%= Url.Content("~/Content/Images/Styles/ok.png") %>" style="border:none;"> <%=x%>
           	         </div>               
           	     
                    </td>
                    
                    <td style="border:0px">   <%=WhiteDownload.ClientIp %> </td>
                    
                    <td style="border:0px"> <%=WhiteDownload.ClientDns%> </td>
                 
                    <td style="border:0px"> <%=WhiteDownload.DownLoadedDate%> </td>                                                       
                </tr>               
                
                <%} %>  
                   
            </table>
            </div>
    
    
    
    
    
    
    
</div>
	   
   
