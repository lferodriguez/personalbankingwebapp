Public Partial Class main
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            AccountsAnalisis()
            StateOfIncomeAnalysis()
            CreditCardAnaysis()
        End If
    End Sub
    Private Sub AccountsAnalisis()
        Dim st As New clsStatistics
        If (st.AccountAnalysis(Session("sWebUserId"))) Then
            If (st.resultadoConsulta.Tables(0).Rows.Count > 0) Then
                accountAnalysis.DataSource = st.resultadoConsulta
                accountAnalysis.DataBind()
            End If
        End If
    End Sub
    
    Private Sub StateOfIncomeAnalysis()
        Dim st As New clsStatistics
        Dim sb As StringBuilder
        sb = New StringBuilder
        st.stateOfIncomeAnalysis(Session("sWebUserId"))


        sb.Append("<table cellpadding=""0"" cellspacing=""0"" border=""0"" id=""main-generatedAnalysis01"">")
        sb.Append("<tr>")
        sb.Append("<th></th>")
        sb.Append("<th colspan=""2"" align=""center"">Six Months ago</th>")
        sb.Append("<th colspan=""2"" align=""center"">Five Months ago</th>")
        sb.Append("<th colspan=""2"" align=""center"">Four Months ago</th>")
        sb.Append("<th colspan=""2"" align=""center"">Three Months ago</th>")
        sb.Append("<th colspan=""2"" align=""center"">Two Months ago</th>")
        sb.Append("<th colspan=""2"" align=""center"">Last Month</th>")
        sb.Append("<th colspan=""2"" align=""center"">This Month</th>")
        sb.Append("</tr>")
        sb.Append("<tr>")
        sb.Append("<td>Concept</td>")
        sb.Append("<td>Q</td>")
        sb.Append("<td>US$</td>")
        sb.Append("<td>Q</td>")
        sb.Append("<td>US$</td>")
        sb.Append("<td>Q</td>")
        sb.Append("<td>US$</td>")
        sb.Append("<td>Q</td>")
        sb.Append("<td>US$</td>")
        sb.Append("<td>Q</td>")
        sb.Append("<td>US$</td>")
        sb.Append("<td>Q</td>")
        sb.Append("<td>US$</td>")
        sb.Append("<td>Q</td>")
        sb.Append("<td>US$</td>")
        sb.Append("</tr>")
        Dim i As Integer
        For Each dr As DataRow In st.resultadoConsulta.Tables(0).Rows
            sb.Append("<tr>")
            i = 0
            For i = 0 To dr.ItemArray.Count - 1
                sb.Append("<td>" & String.Format("{0:N2}", dr.Item(i)).ToString() & "</td>")
            Next
            sb.Append("</tr>")

        Next
        sb.Append("</table>")
        lblIncomeAnalysis.InnerHtml = sb.ToString()
    End Sub

    Private Sub CreditCardAnaysis()
        Dim st As New clsStatistics
        If (st.CreditCardAnalysis(Session("sWebUserId"))) Then
            If (st.resultadoConsulta.Tables(0).Rows.Count > 0) Then
                grdAccountAnalysis.DataSource = st.resultadoConsulta
                grdAccountAnalysis.DataBind()
            End If
        End If
    End Sub

    Private Sub grdAccountAnalysis_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAccountAnalysis.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Payday As Integer = 0
            Dim cutDay As Integer = 0
            Dim addition As Integer = 0
            If Not (e.Row.Cells(6).Text.Trim() = "") Then
                Integer.TryParse(e.Row.Cells(6).Text, Payday)
                If (Payday > 0) Then
                    addition = IIf(Payday < Day(Now.Date), 1, 0)
                    e.Row.Cells(6).Text = DateSerial(Year(Now.Date), Month(Now.Date) + addition, Payday)
                    e.Row.Cells(6).CssClass = IIf(addition = 0 And (Payday - Day(Now.Date)) < 3, "reviewCell", "")
                End If
            End If
            If Not (e.Row.Cells(7).Text = "") Then
                Integer.TryParse(e.Row.Cells(7).Text, cutDay)
                If (cutDay > 0) Then
                    addition = IIf(Payday < Day(Now.Date), 1, 0)
                    e.Row.Cells(7).Text = DateSerial(Year(Now.Date), Month(Now.Date) + addition, cutDay)                    
                End If
            End If
        End If
    End Sub
End Class