Imports COL_BusinessLogic_v2.UCServices

Namespace Domain
    <Serializable()> _
       <CLSCompliant(True)> _
       Public Class ModuleNotificationManagement
        Private adapter As ModuleNotificationManagementAdapter
        Private _EditTemplate As Boolean
        Private _AddTemplate As Boolean
        Private _ManagementPermission As Boolean
        Private _Administration As Boolean
        Public Property EditTemplate() As Boolean
            Get
                Return _EditTemplate
            End Get
            Set(ByVal value As Boolean)
                _EditTemplate = value
            End Set
        End Property
        Public Property AddTemplate() As Boolean
            Get
                Return _AddTemplate
            End Get
            Set(ByVal value As Boolean)
                _AddTemplate = value
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
        Public Property Administration() As Boolean
            Get
                Return _Administration
            End Get
            Set(ByVal value As Boolean)
                _Administration = value
            End Set
        End Property


        Public Sub New()
            Me.EditTemplate = False
            Me.AddTemplate = False
            Me.Administration = False
            Me.ManagementPermission = False
        End Sub
        Public Sub New(ByVal s As Services_NotificationManagement)
            adapter = New ModuleNotificationManagementAdapter(s)
            adapter.Initialize(Me)
        End Sub

        Private Class ModuleNotificationManagementAdapter
            Private _service As COL_BusinessLogic_v2.UCServices.Services_NotificationManagement

            Public Sub New(ByVal s As Services_NotificationManagement)
                Me._service = s
            End Sub

            Public Sub Initialize(ByVal m As ModuleNotificationManagement)
                With m
                    .Administration = _service.Administration
                    .AddTemplate = _service.AddTemplate
                    .ManagementPermission = _service.ManagementPermission
                    .EditTemplate = _service.EditTemplate
                End With
            End Sub
        End Class
    End Class

End Namespace