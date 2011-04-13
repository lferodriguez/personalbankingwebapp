Public Class clsWebUser
    Private _dbCon As clsDbWrapper
    Private _dsConsulta As DataSet
    Private _informacionAdicional As String

    Sub New()
        _dbCon = New clsDbWrapper
    End Sub
    ReadOnly Property informacionAdicional() As String
        Get
            Return _informacionAdicional
        End Get
    End Property
    ReadOnly Property resultadoConsulta() As DataSet
        Get
            Return _dsConsulta
        End Get
    End Property
    Function findUser(ByVal webuser As Integer) As Boolean
        Dim booldev As Boolean = False
        Dim strQry As String = ""
        strQry = "select * from webuser where webuser=" & webuser
        booldev = _dbCon.ejecutarInstruccionSQL(strQry, False)
        _dsConsulta = _dbCon.resultadoConsulta
        _informacionAdicional = _dbCon.informacionAdicional
        Return booldev
    End Function
    Function userAunthentication(ByVal usuario As String, ByVal contrasenia As String, ByRef idUsuario As Integer) As Boolean
        Dim booldev As Boolean = False
        Dim strQry As String = ""
        strQry = "select * from webuser where email='" & usuario.Replace("'", "''") & _
                 "' and password='" & contrasenia.Replace("'", "''") & "' and enabled='Y'"
        If (_dbCon.ejecutarInstruccionSQL(strQry, False)) Then
            If (_dbCon.resultadoConsulta.Tables(0).Rows.Count > 0) Then
                idUsuario = _dbCon.resultadoConsulta.Tables(0).Rows(0).Item("webuser")
                booldev = True
            Else
                _informacionAdicional = "Invalid user credentials."
            End If
        Else
            _informacionAdicional = _dbCon.informacionAdicional
        End If

        Return booldev
    End Function

End Class
