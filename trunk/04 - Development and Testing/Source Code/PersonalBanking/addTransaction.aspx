<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/PersonalBanking.Master" CodeBehind="addTransaction.aspx.vb" Inherits="PersonalBanking.addTransaction" 
    title="Personal Banking" %>
<%@ Register src="DeveloperWebTools/jqueryvalidateplugin.ascx" tagname="jqueryvalidateplugin" tagprefix="uc1" %>
<%@ Register src="DeveloperWebTools/jquerydatepicker.ascx" tagname="jquerydatepicker" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pbmaster_main" runat="server">
<uc1:jqueryvalidateplugin ID="jqueryvalidateplugin1" runat="server" />
    <uc2:jquerydatepicker ID="jquerydatepicker1" runat="server" />
    <div id="frmContainer">
        <div id="frmContainer_tittle">Add a Transaction</div>
        <div id="frmContainer_spacer">            
        </div>
        <asp:MultiView ID="mview" runat="server">            
            <asp:View ID="vStep001" runat="server">
            <div>
            <div class="frmcontainer_field">What would you like to do?:</div>
            <div class="frmcontainer_data">            
                <asp:RadioButtonList ID="rblAction" runat="server" RepeatLayout="OrderedList" CssClass="radioLabels">
                    <asp:ListItem Selected="True" Value="2">Report an Expense</asp:ListItem>
                    <asp:ListItem Value="1">Add an Income</asp:ListItem>
                    <asp:ListItem Value="3">Pay a Credit Card</asp:ListItem>
                    <asp:ListItem Value="4">Pay a Loan</asp:ListItem>
                    <asp:ListItem Value="5">Make a Transfer</asp:ListItem>
                </asp:RadioButtonList>            
            </div>
            <div class="frmcontainer_confirm">
                <asp:Button ID="btnNext" runat="server" Text="Continue" CssClass="controlSubmit"/>
                </div>
            </div>
            </asp:View>            
            
            <asp:View ID="vStep002" runat="server">
            <div>            
            <div class="frmcontainer_field">
                <asp:Label ID="lblFirstMessage" runat="server" Text=""></asp:Label></div>            
            <div class="frmcontainer_field">When was the transaction made?:</div>
            <div class="frmcontainer_data"><asp:TextBox ID="txtDate" runat="server" CssClass="controlTextBox"></asp:TextBox></div>
            
            <div class="frmcontainer_field">Please enter the amount:</div>
            <div class="frmcontainer_data"><asp:TextBox ID="txtAmount" runat="server" CssClass="controlTextBox"></asp:TextBox></div>
            
            <div class="frmcontainer_field">Please choose the concept:</div>
            <div class="frmcontainer_data"><asp:DropDownList ID="ddlConcept" runat="server" CssClass="controlListBox"></asp:DropDownList></div>
            
            <div class="frmcontainer_field">Please choose one of your accounts:</div>
            <div class="frmcontainer_data"><asp:DropDownList ID="ddlAccounts" runat="server" CssClass="controlListBox"></asp:DropDownList></div>            
            
            <div class="frmcontainer_field">Do you have a comment about it?</div>
            <div class="frmcontainer_data"><asp:TextBox ID="txtComment" runat="server" 
                    CssClass="controlTextBox" TextMode="MultiLine" MaxLength="200"></asp:TextBox></div>            
            <div class="frmcontainer_confirm"><asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="controlSubmit"/></div>              
            </div>
            </asp:View>                        
            
            <asp:View ID="vstep003" runat="server">
            <div>
            <div class="frmcontainer_field">When was the transaction made?:</div>
            <div class="frmcontainer_data">
                <asp:TextBox ID="txtDate01" runat="server" CssClass="controlTextBox"></asp:TextBox>
                </div>
            <div class="frmcontainer_field">How much did you pay?:</div>
            <div class="frmcontainer_data">
                <asp:TextBox ID="txtAmount01" runat="server" CssClass="controlTextBox"></asp:TextBox>
                </div>
            <div class="frmcontainer_field">Choose your payment account:</div>
            <div class="frmcontainer_data">
                <asp:DropDownList ID="ddlDestinationAccount" runat="server" CssClass="controlListBox" AutoPostBack="true">
                </asp:DropDownList>
                </div>
            <div class="frmcontainer_field">Choose the account where you take the amount to pay:</div>
            <div class="frmcontainer_data">
                <asp:DropDownList ID="ddlSourceAccount" runat="server" CssClass="controlListBox" AutoPostBack="true">
                </asp:DropDownList>
                </div>
            <div class="frmcontainer_field">
                <asp:Label ID="lblExchangeRate" runat="server"
                visible="false" 
                Text="It seems that you have made a transaction with a currency exchange rate. How much was it?"></asp:Label>
                </div>
            <div class="frmcontainer_data">
                <asp:TextBox ID="txtexchangeRate" 
                visible="false"
                runat="server" CssClass="controlTextBox"></asp:TextBox>
                </div>
            <div class="frmcontainer_field">Do you have a comment?</div>
            <div class="frmcontainer_data">
                <asp:TextBox ID="txtComment01" runat="server" CssClass="controlTextBox" 
                    MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                </div>
            <div class="frmcontainer_confirm">
                <asp:Button ID="btnPayment" runat="server" Text="Continue" CssClass="controlSubmit"/>
                </div>
            </div>
            </asp:View>
            <asp:View ID="vstep004" runat="server">
            <div id="frmcontainer_finalmessages">
                <asp:Label ID="lblResultado" runat="server" Text=""></asp:Label>
             </div>
            </asp:View>
        </asp:MultiView>
        
    </div>
</asp:Content>
