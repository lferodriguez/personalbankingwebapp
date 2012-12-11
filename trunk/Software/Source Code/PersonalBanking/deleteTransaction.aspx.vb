Public Class deleteTransaction
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Dim enc As New Encrypt
            Dim acttran As clsAccountTransaction
            If IsNumeric(enc.Decrypt_param(Request.QueryString("ref"))) Then
                acttran = New clsAccountTransaction
                If (acttran.deleteTransaction(Session("sWebUserId"), enc.Decrypt_param(Request.QueryString("ref")), clsCatalogs.events.Warning)) Then
                    lblMensajes.Text = "Trasanction dropped successfully. Please remeber if you made a compound transaction " & _
                                       "Such as: Transfer, Loan Payment or a Credit Card payment, delete the associated transaction also."
                Else
                    '' Unable to drop transaction
                    lblMensajes.Text = "Sorry, It's impossible to delete the transaction. More information: " + acttran.informacionAdicional
                End If

            Else
                ''Url(mismatch)
                lblMensajes.Text = "Sorry, the URL you've entered is no longer avaible."
            End If
        End If
    End Sub

End Class