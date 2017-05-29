Namespace Presentation
    <CLSCompliant(True)> Public Class Element(Of T)
        Private _ID As T
        Private _Name As String

        Public Property ID() As T
            Get
                Return _ID
            End Get
            Set(ByVal value As T)
                _ID = value
            End Set
        End Property
        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
            End Set
        End Property

        Public Sub New()

        End Sub

    End Class
End Namespace