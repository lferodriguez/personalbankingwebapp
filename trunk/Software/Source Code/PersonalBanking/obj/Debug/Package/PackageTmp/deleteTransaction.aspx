<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/PersonalBanking.Master" CodeBehind="deleteTransaction.aspx.vb" Inherits="PersonalBanking.deleteTransaction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pbmaster_main" runat="server">
     <div id="frmContainer">
        <div id="frmContainer_tittle">AccountHistory</div>
        <div id="frmContainer_spacer">                        
        </div>
        <div id="frmContainer_toolBar">                        
        | <a href="main.aspx">Return</a>
        </div>
        <div id="frmcontainer_finalmessages">
            <asp:Label ID="lblMensajes" runat="server"></asp:Label>            
        </div>
    </div>
</asp:Content>
