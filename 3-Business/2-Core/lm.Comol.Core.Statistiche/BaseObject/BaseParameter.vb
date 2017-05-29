Public Class BaseParameter
    Implements I_BaseParameter

    Private _CommunityID As Integer
    Private _UserID As Integer

    Public Property CommunityID() As Integer
        Get
            Return _CommunityID
        End Get
        Set(ByVal value As Integer)
            _CommunityID = value
        End Set
    End Property

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property
End Class