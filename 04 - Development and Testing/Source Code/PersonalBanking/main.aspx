<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/PersonalBanking.Master" CodeBehind="main.aspx.vb" Inherits="PersonalBanking.main" 
    title="PersonalBanking" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="pbmaster_main" runat="server">
    <table cellpadding="0" cellspacing="0" class="homeResume">    
    <tr><td style="height:20px;"><a href="accountBalances.aspx">View all accounts</a></td></tr>
    <tr>
        <td>
            <asp:GridView ID="accountAnalysis" runat="server" AutoGenerateColumns="False" CssClass="homeAnalysis" Width="400px">
                <Columns>
                    <asp:BoundField HeaderText="Account Type" DataField="AccountTypeDescription" />
                    <asp:BoundField HeaderText="" DataField="currencySymbol" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField HeaderText="Total" DataField="total" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Center"/>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
    
    <tr>
        <td style="padding-top:10px">
        <div id ="lblIncomeAnalysis" runat="server"></div>            
        </td>
    </tr>
    <tr>
        <td style="padding-top:10px">
        <asp:GridView ID="grdAccountAnalysis" runat="server" AutoGenerateColumns="False" CssClass="homeAnalysis" Width="75%">
                <Columns>
                    <asp:BoundField HeaderText="Currency" DataField="CurrencySymbol" />
                    <asp:BoundField HeaderText="Credit Card" DataField="number" />                    
                    <asp:BoundField HeaderText="Bank" DataField="BankName" ItemStyle-HorizontalAlign="Center"/>                    
                    <asp:BoundField HeaderText="Balance" DataField="balance" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Center"/>
                    <asp:BoundField HeaderText="Max Credit" DataField="MaxCredit" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Center"/>
                    <asp:BoundField HeaderText="Available" DataField="available" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Center"/>
                    <asp:BoundField HeaderText="Next Pay Day" DataField="PayDay" ItemStyle-HorizontalAlign="Center" />                    
                    <asp:BoundField HeaderText="Next Cut Day" DataField="CutDay" ItemStyle-HorizontalAlign="Center" />                    
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>

</asp:Content>
