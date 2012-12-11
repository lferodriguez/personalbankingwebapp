Public Partial Class PersonalBanking
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim user As clsWebUser

        If (Session("sWebUserId") > 0) Then
            user = New clsWebUser
            user.findUser(Session("sWebUserId"))
            lblUserName.Text = user.resultadoConsulta.Tables(0).Rows(0).Item("firstName")
        Else
            Response.Redirect(ResolveUrl("~/default.aspx"), False)
        End If
    End Sub

    Protected Sub hlkbOut_Click(ByVal sender As Object, ByVal e As EventArgs) Handles hlkbOut.Click
        Session("sWebUserId") = 0
        Response.Redirect(ResolveUrl("~/default.aspx"), False)
    End Sub
End Class