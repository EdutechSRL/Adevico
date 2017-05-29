Namespace WikiNew
    Namespace Services
        <Serializable()> Public MustInherit Class MyServices
            'Inherits DomainObject

            '<CLSCompliant(False)> _
            'Private n_StringaPermessi(31) As Byte

            Private Const Codice As String = "SRVABSTRACT"

            Public PermessoDiag As Permesso

            'Public Property PermessiAssociati() As String
            '    Get
            '        Dim i As Integer
            '        Dim oListaPermessi(n_StringaPermessi.Length - 1) As Char

            '        For i = 0 To n_StringaPermessi.Length - 1
            '            oListaPermessi(i) = n_StringaPermessi(i).ToString
            '        Next
            '        PermessiAssociati = oListaPermessi
            '    End Get
            '    Set(ByVal Value As String)
            '        Dim i As Integer
            '        Dim oListaPermessi() As Char
            '        oListaPermessi = Value.ToCharArray()

            '        For i = 0 To oListaPermessi.Length - 1
            '            n_StringaPermessi(i) = CByte(oListaPermessi(i).ToString)
            '        Next
            '    End Set
            'End Property




            'Public Enum TipoPermesso
            '    Lettura = 1
            '    Scrittura = 2
            '    Modifica = 4
            '    Cancellazione = 8
            '    Moderazione = 16
            '    AssegnarePermesso = 32
            '    Amministrazione = 64
            '    Invia = 128
            '    Ricevi = 256
            '    Sincronizza = 512
            '    Esplora = 1024
            '    Stampa = 2048
            '    CambiaProprietario = 4096
            '    TornaRuolo = 8192
            '    Impersonifica = 16384

            '    'None = -1
            '    'Read = 0
            '    'Write = 1
            '    'Change = 2
            '    'Delete = 3
            '    'Moderate = 4
            '    'Grant = 5
            '    'Admin = 6
            '    'Send = 7
            '    'Receive = 8
            '    'Synchronize = 9
            '    'Browse = 10
            '    'Print = 11
            '    'ChangeOwner = 12
            'End Enum
            Sub New()
                MyBase.New()
                'Dim i As Integer
                'For i = 0 To n_StringaPermessi.Length - 1
                '    n_StringaPermessi(i) = 0
                'Next
            End Sub
            'Protected Overridable Function GetPermissionByPosition(ByVal oPosizione As Integer) As Boolean
            '    If oPosizione > (n_StringaPermessi.Length - 1) Then
            '        Return False
            '    Else
            '        Return CBool(n_StringaPermessi(oPosizione))
            '    End If
            '    Return False
            'End Function
            'Protected Overridable Function GetPermissionValue(ByVal oType As PermissionType) As Boolean
            '    Dim iPosizione As Integer
            '    iPosizione = CType(oType, PermissionType)
            '    Return GetPermissionByPosition(iPosizione)
            'End Function
            'Protected Overridable Function SetPermissionByPosition(ByVal oPosizione As Integer, ByVal oValue As Byte) As Boolean
            '    If oPosizione > (n_StringaPermessi.Length - 1) Then
            '        Return False
            '    Else
            '        n_StringaPermessi(oPosizione) = oValue
            '    End If
            '    Return False
            'End Function

        End Class

    End Namespace
End Namespace