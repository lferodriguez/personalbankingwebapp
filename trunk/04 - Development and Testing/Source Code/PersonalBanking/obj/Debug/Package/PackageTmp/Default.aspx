<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="PersonalBanking._Default" %>

<%@ Register src="DeveloperWebTools/jqueryvalidateplugin.ascx" tagname="jqueryvalidateplugin" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>PersonalBanking</title>
    <style type="text/css">@import "css/layout.css";@import "css/styles.css";  </style>
    <script src="js/jquery-1.3.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="cntdr_frm_login">
        <div class="cntdr_frm_login_tittle">Personal Banking</div>
        <div class="cntdr_frm_login_field">E-Mail:</div>
        <div class="cntdr_frm_login_data"><asp:TextBox id="txtEmail" runat="server" CssClass="controlTextBox"></asp:TextBox></div>
        <div class="cntdr_frm_login_field">Password:</div>
        <div class="cntdr_frm_login_data"><asp:TextBox id="txtPassword" runat="server" TextMode="Password" CssClass="controlTextBox"></asp:TextBox></div>
        <div class="cntdr_frm_login_conf"><asp:Button ID="btnIngresar" runat="server" Text="Log in" CssClass="controlSubmit" /></div>        
        <div class="cntdr_frm_login_msgs"><asp:Label ID="lblMensajes" runat="server" Text=""></asp:Label></div>
    </div>
    <uc1:jqueryvalidateplugin ID="jqueryvalidateplugin1" runat="server" />
    </form>
</body>
</html>
