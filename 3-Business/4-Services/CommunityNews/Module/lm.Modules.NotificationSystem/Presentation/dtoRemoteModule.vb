Namespace Presentation
    <CLSCompliant(True), Serializable()> Public Class dtoRemoteModule
        Private _ID As Integer
        Private _Name As String

        Public Property ModuleID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal value As Integer)
                _ID = value
            End Set
        End Property
        Public Property ModuleName() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
            End Set
        End Property
        Public Sub New()

        End Sub
        Public Sub New(ByVal oId As Integer, ByVal oName As String)
            _ID = oId
            _Name = oName
        End Sub
        Public Sub New(ByVal m As COL_BusinessLogic_v2.PlainService)
            _ID = m.ID
            _Name = m.Name
        End Sub
    End Class
End Namespace