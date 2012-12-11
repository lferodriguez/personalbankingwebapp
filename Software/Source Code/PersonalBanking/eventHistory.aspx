<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/PersonalBanking.Master" CodeBehind="eventHistory.aspx.vb" Inherits="PersonalBanking.eventHistory" %>
<%@ Register src="DeveloperWebTools/jquerydatepicker.ascx" tagname="jquerydatepicker" tagprefix="uc1" %>
<%@ Register src="DeveloperWebTools/jqueryvalidateplugin.ascx" tagname="jqueryvalidateplugin" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pbmaster_main" runat="server">
<uc1:jquerydatepicker ID="jquerydatepicker1" runat="server" />
<uc2:jqueryvalidateplugin ID="jqueryvalidateplugin1" runat="server" />
<asp:Label ID="hddStartDate" runat="server" Text="" Visible=false></asp:Label>
<asp:Label ID="hddEndDate" runat="server" Text="" Visible=false></asp:Label>
<asp:Label ID="hddSearchText" runat="server" Text="" Visible=false></asp:Label>
<div id="frmContainer">
        <div id="frmContainer_tittle">Event History Search</div>
        <div id="frmContainer_spacer"></div>
        <div>
            <div class="frmcontainer_field">Enter a valid start date</div>
            <div class="frmcontainer_data"><asp:TextBox ID="txtStartDate" runat="server" CssClass="controlTextBox"></asp:TextBox></div>
            <div class="frmcontainer_field">Enter a valid final date</div>
            <div class="frmcontainer_data"><asp:TextBox ID="txtEndDate" runat="server" CssClass="controlTextBox"></asp:TextBox></div>
            <div class="frmcontainer_field">Enter a search string</div>
            <div class="frmcontainer_data"><asp:TextBox ID="txtSearchString" runat="server" CssClass="controlTextBox" MaxLength="25"></asp:TextBox>
            <div class="frmcontainer_confirm"><asp:Button ID="btnAdd" runat="server" Text="Search ..." CssClass="controlSubmit"/></div>
            </div>
        </div>
        <div>
            <div class="frmcontainer_field"><strong><asp:Label ID="lblResults" runat="server" Text=""></asp:Label></strong></div>
        </div>
        <div>
            <asp:GridView ID="grdEvent" runat="server" AutoGenerateColumns="False" 
                CssClass="homeAnalysis" Width="100%" AllowPaging="True" PageSize="10">
                <Columns>
                    <asp:BoundField HeaderText="Level" DataField="EventLevelCatalog"  />
                    <asp:BoundField HeaderText="Date" DataField="EventDate"  DataFormatString="{0:dd/MM/yyyy}"/>
                    <asp:BoundField HeaderText="User" DataField="WebUserName" />                    
                    <asp:BoundField HeaderText="Event Description" DataField="eventMessage" ItemStyle-Width="70%" />                    
                </Columns>
            </asp:GridView>
        </div>
</div>
</asp:Content>
