
Public Class clsDbWrapper
    Private _dsConsulta As DataSet
    Private _dbCon As SQLServerConnection
    Private _informacionAdicional As String

    Sub New()
        _dbCon = New SQLServerConnection
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
    Public Function ejecutarInstruccionSQL(ByVal strSQL As String, ByVal modificaData As Boolean) As Boolean
        Dim booldev As Boolean = False
        Try
            If _dbCon.Open() Then
                If (_dbCon.ExecuteSqlReturnDataset(strSQL, modificaData)) Then
                    _dsConsulta = _dbCon.ResultDataset
                    booldev = True
                Else
                    _informacionAdicional = "Unable to execute query on database. More: " & _dbCon.ErrorMessage
                End If
            Else
                _informacionAdicional = "Unable to open connection. More: " & _dbCon.ErrorMessage
            End If
            _dbCon.Close()
        Catch ex As Exception
            _informacionAdicional = ex.ToString
        End Try
        
        Return booldev
    End Function
    Public Function ejecutarProcedimientoAlmacenado(ByVal procedimiento As String, ByVal sppParameters As SpParameters) As Boolean
        Dim booldev As Boolean = False
        Try
            If _dbCon.Open() Then
                If (_dbCon.RunSpReturnDataset(procedimiento, sppParameters)) Then
                    _dsConsulta = _dbCon.ResultDataset
                    booldev = True
                Else
                    _informacionAdicional = "Unable to execute procedure on database. Razón: " & _dbCon.ErrorMessage
                End If
            Else
                _informacionAdicional = "Unable to open connection. More: " & _dbCon.ErrorMessage
            End If
            _dbCon.Close()
        Catch ex As Exception
            _informacionAdicional = ex.ToString
        End Try
        
        Return booldev
    End Function
End Class
