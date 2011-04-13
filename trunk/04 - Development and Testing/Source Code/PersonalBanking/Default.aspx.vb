Partial Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        validacionesEnCliente()
    End Sub

    Protected Sub btnIngresar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnIngresar.Click
        Dim user As New clsWebUser
        Dim idWebUser As Integer = 0
        If (user.userAunthentication(txtEmail.Text, txtPassword.Text, idWebUser)) Then
            Session("sWebUserId") = idWebUser
            Response.Redirect(ResolveUrl("~/main.aspx"), False)
        Else
            lblMensajes.Text = "Invalid user or password."
        End If
    End Sub
    Sub validacionesEnCliente()

        jqueryvalidateplugin1.cargarJQueryValidatePlugin(True, True)
        Dim _jquery_actions As New HtmlGenericControl("script")
        _jquery_actions.Attributes.Add("type", "text/javascript")
        _jquery_actions.Attributes.Add("language", "javascript")

        Dim strHtml As New StringBuilder
        strHtml.AppendLine("$(document).ready(function(){")
        strHtml.AppendLine("$.metadata.setType(""attr"", ""validate"");")
        strHtml.AppendLine("$('#" & Form.ClientID & "').validate({")
        strHtml.AppendLine("rules:{")
        strHtml.AppendLine(txtEmail.UniqueID & ": {required:true, email:true},")        
        strHtml.AppendLine(txtPassword.UniqueID & ": {required:true}")
        strHtml.AppendLine("},")
        strHtml.AppendLine("messages:{")
        strHtml.AppendLine(txtEmail.UniqueID & ": {required:""*"", email:""please enter a valid e-mail""},")
        strHtml.AppendLine(txtPassword.UniqueID & ": {required:""*""}")
        strHtml.AppendLine("}")
        strHtml.AppendLine("});")
        strHtml.AppendLine("});")
        _jquery_actions.InnerHtml = strHtml.ToString
        Page.Header.Controls.Add(_jquery_actions)
    End Sub
End Class