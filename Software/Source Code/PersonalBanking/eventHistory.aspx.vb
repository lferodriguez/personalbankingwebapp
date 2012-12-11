Public Class eventHistory
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        jquerydatepicker1.cargarJqueryDatePicker()
        jquerydatepicker1.mostrarDatePickerEnTextBox(txtStartDate)
        jquerydatepicker1.mostrarDatePickerEnTextBox(txtEndDate)
        dataVerification()

    End Sub
    Private Sub dataVerification()

        jqueryvalidateplugin1.cargarJQueryValidatePlugin(True, True)
        Dim _jquery_actions As New HtmlGenericControl("script")
        _jquery_actions.Attributes.Add("type", "text/javascript")
        _jquery_actions.Attributes.Add("language", "javascript")

        Dim strHtml As New StringBuilder
        strHtml.AppendLine("$(document).ready(function(){")
        strHtml.AppendLine("$.metadata.setType(""attr"", ""validate"");")
        strHtml.AppendLine("$('#" & Form.ClientID & "').validate({")

        strHtml.AppendLine("rules:{")
        strHtml.AppendLine(txtStartDate.UniqueID & ": {required:true,date:true},")
        strHtml.AppendLine(txtEndDate.UniqueID & ": {required:true,date:true}")
        strHtml.AppendLine("},")
        strHtml.AppendLine("messages:{")
        strHtml.AppendLine(txtStartDate.UniqueID & ": {required:""Please enter a date."", date:""Please enter a valid date""},")
        strHtml.AppendLine(txtEndDate.UniqueID & ": {required:""Please enter a date."", date:""Please enter a valid date""}")
        strHtml.AppendLine("}")
        strHtml.AppendLine("});")
        strHtml.AppendLine("});")        
        _jquery_actions.InnerHtml = strHtml.ToString

        Page.Header.Controls.Add(_jquery_actions)
    End Sub

    Function searchData(ByVal startDate As String, ByVal endDate As String, ByVal searchCriteria As String) As DataSet
        Dim evts As New clsEvents
        evts.searchForEvents(startDate, endDate, searchCriteria)
        Return evts.resultadoConsulta
    End Function

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click

        Dim resultData As DataSet

        grdEvent.DataSource = Nothing
        grdEvent.DataBind()
        lblResults.Text = ""

        resultData = searchData(txtStartDate.Text, txtEndDate.Text, txtSearchString.Text)


        If (resultData.Tables(0).Rows.Count > 0) Then
            hddStartDate.Text = txtStartDate.Text
            hddEndDate.Text = txtEndDate.Text
            hddSearchText.Text = txtSearchString.Text

            lblResults.Text = "Query returned " & resultData.Tables(0).Rows.Count & " results. "
            grdEvent.DataSource = resultData
            grdEvent.DataBind()
        Else
            lblResults.Text = "Query returned 0 results."
        End If
        
    End Sub

   
    Private Sub grdEvent_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdEvent.PageIndexChanging
        grdEvent.DataSource = searchData(hddStartDate.Text, hddEndDate.Text, hddSearchText.Text)
        grdEvent.PageIndex = e.NewPageIndex
        grdEvent.DataBind()
    End Sub
End Class