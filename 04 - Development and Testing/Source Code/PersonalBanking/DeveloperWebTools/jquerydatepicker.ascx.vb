Public Partial Class jquerydatepicker
    Inherits System.Web.UI.UserControl

    Sub cargarJqueryDatePicker()
        Dim _jquery As New HtmlGenericControl("link")
        _jquery.Attributes.Add("type", "text/css")
        _jquery.Attributes.Add("rel", "Stylesheet")
        _jquery.Attributes.Add("href", ResolveUrl("~/js/jquery-datepicker-1.7.2/themes/cupertino/jquery-ui-1.7.2.custom.css"))
        Page.Header.Controls.Add(_jquery)
        _jquery = New HtmlGenericControl("script")
        _jquery.Attributes.Add("type", "text/javascript")
        _jquery.Attributes.Add("language", "javascript")
        _jquery.Attributes.Add("src", ResolveUrl("~/js/jquery-datepicker-1.7.2/js/jquery-ui-1.7.2.custom.min.js"))
        Page.Header.Controls.Add(_jquery)
    
    End Sub
    Sub mostrarDatePickerEnTextBox(ByVal texto As System.Web.UI.WebControls.TextBox)
        Dim _jquery_actions As New HtmlGenericControl("script")
        _jquery_actions.Attributes.Add("type", "text/javascript")
        _jquery_actions.Attributes.Add("language", "javascript")
        Dim strHtml As New StringBuilder
        strHtml.AppendLine("$(function(){")
        strHtml.AppendLine("$(""#" & texto.ClientID & _
                         """).datepicker({showOn: 'button', buttonImage: '" & _
                         ResolveUrl("~/js/jquery-datepicker-1.7.2/images/calendar.gif") & _
                         "', buttonImageOnly: true})")
        strHtml.AppendLine("});")
        _jquery_actions.InnerHtml = strHtml.ToString
        Page.Header.Controls.Add(_jquery_actions)
    End Sub
End Class