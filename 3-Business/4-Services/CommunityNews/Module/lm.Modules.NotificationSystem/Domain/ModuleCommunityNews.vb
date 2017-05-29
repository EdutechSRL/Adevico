Imports COL_BusinessLogic_v2.UCServices

Namespace Domain
    <Serializable()> _
       <CLSCompliant(True)> _
       Public Class ModuleCommunityNews
        Private adapter As ModuleCommunityNewsAdapter
        Private _ViewMyNews As Boolean
        Private _DeleteMyNews As Boolean
        Private _ManagementPermission As Boolean
        Private _ViewOtherNews As Boolean
        Private _DeleteOtherNews As Boolean
        Public Property ViewMyNews() As Boolean
            Get
                Return _ViewMyNews
            End Get
            Set(ByVal value As Boolean)
                _ViewMyNews = value
            End Set
        End Property
        Public Property DeleteMyNews() As Boolean
            Get
                Return _DeleteMyNews
            End Get
            Set(ByVal value As Boolean)
                _DeleteMyNews = value
            End Set
        End Property
        Public Property ManagementPermission() As Boolean
            Get
                Return _ManagementPermission
            End Get
            Set(ByVal value As Boolean)
                _ManagementPermission = value
            End Set
        End Property
        Public Property ViewOtherNews() As Boolean
            Get
                Return _ViewOtherNews
            End Get
            Set(ByVal value As Boolean)
                _ViewOtherNews = value
            End Set
        End Property
        Public Property DeleteOtherNews() As Boolean
            Get
                Return _DeleteOtherNews
            End Get
            Set(ByVal value As Boolean)
                _DeleteOtherNews = value
            End Set
        End Property


        Public Sub New()
            Me.ViewMyNews = False
            Me.DeleteMyNews = False
            Me.ViewOtherNews = False
            Me.ManagementPermission = False
            Me.DeleteOtherNews = False
        End Sub
        Public Sub New(ByVal s As Services_CommunityNews)
            adapter = New ModuleCommunityNewsAdapter(s)
            adapter.Initialize(Me)
        End Sub

        Private Class ModuleCommunityNewsAdapter
            Private _service As COL_BusinessLogic_v2.UCServices.Services_CommunityNews

            Public Sub New(ByVal s As Services_CommunityNews)
                Me._service = s
            End Sub

            Public Sub Initialize(ByVal m As ModuleCommunityNews)
                With m
                    .ViewMyNews = _service.ViewMyNews
                    .DeleteMyNews = _service.DeleteMyNews
                    .ManagementPermission = _service.ManagementPermission
                    .ViewOtherNews = _service.ViewOtherNews
                    .DeleteOtherNews = _service.DeleteOtherNews
                End With
            End Sub
        End Class
    End Class

End Namespace