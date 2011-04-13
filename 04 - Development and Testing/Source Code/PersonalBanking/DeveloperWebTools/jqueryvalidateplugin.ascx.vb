Public Partial Class jqueryvalidateplugin
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub
    Sub cargarJQueryValidatePlugin(ByVal cargarMetadata As Boolean, ByVal cargarMetodosAdicionalesDePuguin As Boolean)

        Dim _jquery As New HtmlGenericControl("script")
        _jquery.Attributes.Add("type", "text/javascript")
        _jquery.Attributes.Add("language", "javascript")
        _jquery.Attributes.Add("src", ResolveUrl("~/js/jquery.validate.min.js"))
        Page.Header.Controls.Add(_jquery)

        If (cargarMetadata) Then
            _jquery = New HtmlGenericControl("script")
            _jquery.Attributes.Add("type", "text/javascript")
            _jquery.Attributes.Add("language", "javascript")
            _jquery.Attributes.Add("src", ResolveUrl("~/js/jquery.metadata.js"))
            Page.Header.Controls.Add(_jquery)
        End If
        If (cargarMetodosAdicionalesDePuguin) Then
            _jquery = New HtmlGenericControl("script")
            _jquery.Attributes.Add("type", "text/javascript")
            _jquery.Attributes.Add("language", "javascript")
            _jquery.Attributes.Add("src", ResolveUrl("~/js/additional-methods.js"))
            Page.Header.Controls.Add(_jquery)
        End If
    End Sub    
End Class