Public Class dataAnalysis01
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then

            StateOfIncomeAnalysis()

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
End Class