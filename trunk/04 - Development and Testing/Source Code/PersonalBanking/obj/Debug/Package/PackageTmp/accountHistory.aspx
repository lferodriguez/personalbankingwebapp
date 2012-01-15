<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/PersonalBanking.Master" CodeBehind="accountHistory.aspx.vb" Inherits="PersonalBanking.accountHistory" 
    title="PersonalBanking" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="js/scripts.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pbmaster_main" runat="server">
     <div id="frmContainer">
        <div id="frmContainer_tittle">AccountHistory</div>
        <div id="frmContainer_spacer">                        
        </div>
        <div id="frmContainer_toolBar">                        
        | <a href="main.aspx">Return</a>
        </div>
        <div id="frmContainer_generalInformation">
        <asp:Label ID="lblAccountInformation" runat="server"></asp:Label>
        </div>
        <div id="frmContainer_fieldSet">
            <div class="frmcontainer_field">Please choose a period:</div>
            <div class="frmcontainer_data"><asp:DropDownList ID="ddlPeriods" runat="server" CssClass="controlListBox" AutoPostBack="true">
                <asp:ListItem Value="1">This month</asp:ListItem>
                <asp:ListItem Value="2">Last month</asp:ListItem>
                <asp:ListItem Value="3">Two months Ago</asp:ListItem>
                </asp:DropDownList></div>            
            <div class="frmcontainer_confirm"></div>              
        </div>
        <div id="frmcontainer_finalmessages">
            <asp:Label ID="lblMensajes" runat="server"></asp:Label>            
        </div>
        <div id="frmContainer_gridResults">
            <asp:GridView ID="gdResults" runat="server" AutoGenerateColumns="False" 
                GridLines="None" DataKeyNames="AccountTransaction">
                <Columns>
                    <asp:BoundField/>
                    <asp:BoundField DataField="transactionDate" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center"> 
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="accountTransaction" HeaderText="Reference" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="name" HeaderText="Concept" />
                    <asp:BoundField DataField="Deposits" HeaderText="Deposits" DataFormatString="{0:F2}" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Withdrawls" HeaderText="Withdrawls" DataFormatString="{0:F2}" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                </Columns>
            </asp:GridView>                        
        </div>
        <div id="frmContainer_resumes">
            <div class="resumesBox ">
            <div class="resumesBox_tittle">Totals by Concept Type</div>
            <div class="resumesBox_content">
                <asp:GridView ID="gdtctresume" runat="server" AutoGenerateColumns="False" 
                GridLines="None" ShowHeader="false">
                <Columns>
                    <asp:BoundField DataField="concept" HeaderText="Concept"/>
                    <asp:BoundField DataField="total" HeaderText="Total" DataFormatString="{0:F2}" ItemStyle-HorizontalAlign="Right"/>                    
                </Columns>
                </asp:GridView>
            </div>
            </div>
            <div class="resumesBox ">
                <div class="resumesBox_tittle">Totals by Concept</div>
                <div class="resumesBox_content">
                <asp:GridView ID="gdtcresume" runat="server" AutoGenerateColumns="False" 
                GridLines="None" ShowHeader="false">
                <Columns>
                    <asp:BoundField DataField="concept" HeaderText="Concept"/>
                    <asp:BoundField DataField="total" HeaderText="Total" DataFormatString="{0:F2}" ItemStyle-HorizontalAlign="Right"/>                    
                </Columns>
                </asp:GridView>
                </div>
            </div>
        </div>
     </div>
</asp:Content>