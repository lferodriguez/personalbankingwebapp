<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/PersonalBanking.Master" CodeBehind="statistics.aspx.vb" Inherits="PersonalBanking.statistics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">@import "css/ps_statistics.css";</style>
    <script type="text/javascript" src="js/pf_statistics.js"></script>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pbmaster_main" runat="server">
<table cellpadding="0" cellspacing="0" class="homeResume">    
<tr><td style="height:20px;"></td></tr>
<tr><td style="height:20px;">Minimal Statistics, press <a href="minimalStatistics.aspx">Here</a><td></tr>
<tr><td>
    <div id="statistics_container">
        <div class="optionsMenu">
            <ul class="tabs">  
                <li><a href="#tab1">Summary</a></li>  
                <li><a href="#tab2">Detailed Statistics Q.</a></li>  
                <li><a href="#tab3">Detailed Statistics US$.</a></li>  
            </ul>
        </div>
            <div class="tab_container">
                <div id="tab1" class="tab_content">                                                      
                 <div><p style="font-weight:bold;">(Quetzales Q.)</p><asp:Label ID="lblResumeQ" runat="server" Text=""></asp:Label></div> <br />
                 <div><p style="font-weight:bold;">(US $.)</p><asp:Label ID="lblResumeUS" runat="server" Text=""></asp:Label></div> <br />
                </div>
                <div id="tab2" class="tab_content">                    
                    <div><asp:Label ID="lblincomesQue" runat="server" Text=""></asp:Label></div> <br />
                    <div><asp:Label ID="lblexpensesQue" runat="server" Text=""></asp:Label></div> <br />
                </div>
                <div id="tab3" class="tab_content">
                    <div><asp:Label ID="lblincomesDol" runat="server" Text=""></asp:Label></div><br /> 
                    <div><asp:Label ID="lblexpensesdol" runat="server" Text=""></asp:Label></div><br />                
                </div>
            </div>
    </div>
</td></tr></table>
</asp:Content>
