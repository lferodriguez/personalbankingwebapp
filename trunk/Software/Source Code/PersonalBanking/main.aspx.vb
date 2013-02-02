Public Partial Class main
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            AccountsAnalisis()
            CreditCardAnaysis()
            CertificateOfDepositsAnalysis()
            lastEventsOnSystem()
        End If
    End Sub
    Private Sub AccountsAnalisis()
        Dim st As New clsStatistics
        If (st.AccountAnalysis(Session("sWebUserId"), clsAccount.accountStates.Enabled)) Then
            If (st.resultadoConsulta.Tables(0).Rows.Count > 0) Then
                accountAnalysis.DataSource = st.resultadoConsulta
                accountAnalysis.DataBind()
            End If
        End If
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
    Private Sub CertificateOfDepositsAnalysis()
        Dim st As New clsStatistics
        If (st.CertificateOfDepositsAnalysis(Session("sWebUserId"))) Then
            If (st.resultadoConsulta.Tables(0).Rows.Count > 0) Then
                grdCdsAccountAnalysis.DataSource = st.resultadoConsulta
                grdCdsAccountAnalysis.DataBind()
            End If
        End If
    End Sub
    Private Sub lastEventsOnSystem()
        Dim evts As New clsEvents

        grdWebUserEvents.DataSource = Nothing
        grdWebUserEvents.DataBind()

        If (evts.lastEvents) Then
            If (evts.resultadoConsulta.Tables(0).Rows.Count > 0) Then
                grdWebUserEvents.DataSource = evts.resultadoConsulta
                grdWebUserEvents.DataBind()
            End If
        End If
    End Sub
    Private Sub grdAccountAnalysis_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAccountAnalysis.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Payday As Integer = 0
            Dim cutDay As Integer = 0
            Dim addition As Integer = 0
            Dim expirationDate As Date
            Dim daysDifference As Integer
            Dim monthsDifference As Integer
            Dim yearsDifference As Integer
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
            If (Not (e.Row.Cells(8).Text = "")) Then
                Date.TryParse(e.Row.Cells(8).Text, expirationDate)
                daysDifference = DateDiff(DateInterval.Day, Now.Date, expirationDate)
                monthsDifference = DateDiff(DateInterval.Month, Now.Date, expirationDate)
                yearsDifference = DateDiff(DateInterval.Year, Now.Date, expirationDate)
                If (yearsDifference < 0) Then
                    e.Row.Cells(8).CssClass = "reviewCell"
                ElseIf (monthsDifference <= 0) And (daysDifference <= 15) Then
                    e.Row.Cells(8).CssClass = "reviewCell"
                End If
            End If
        End If
    End Sub

    
    Private Sub grdCdsAccountAnalysis_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdCdsAccountAnalysis.RowDataBound

        Dim expirationDate As Date
        Dim daysDifference As Integer
        Dim monthsDifference As Integer
        Dim yearsDifference As Integer

        If (Not (e.Row.Cells(4).Text = "")) Then
            Date.TryParse(e.Row.Cells(4).Text, expirationDate)
            daysDifference = DateDiff(DateInterval.Day, Now.Date, expirationDate)
            monthsDifference = DateDiff(DateInterval.Month, Now.Date, expirationDate)
            yearsDifference = DateDiff(DateInterval.Year, Now.Date, expirationDate)
            If (yearsDifference < 0) Then
                e.Row.Cells(4).CssClass = "reviewCell"
            ElseIf (monthsDifference <= 0) And (daysDifference <= 15) Then
                e.Row.Cells(4).CssClass = "reviewCell"
            End If
        End If
    End Sub
End Class