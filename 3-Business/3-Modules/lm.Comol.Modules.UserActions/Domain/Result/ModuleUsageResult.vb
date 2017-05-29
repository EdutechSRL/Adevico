Namespace lm.Comol.Modules.UsageResults.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class ModuleUsageResult
        Public Const UniqueId As String = "SRVUSAGER"

#Region "Private Property"
        Private _Administration As Boolean
        Private _SelfReport As Boolean
        Private _CommunityReports As Boolean
        Private _PortalReports As Boolean
        Private _ManagementPermission As Boolean
        Private _Export As Boolean
        Private _Print As Boolean
#End Region

#Region "Public Property"
        Public Property ViewMyReport() As Boolean
            Get
                ViewMyReport = _SelfReport
            End Get
            Set(ByVal Value As Boolean)
                _SelfReport = Value
            End Set
        End Property
        Public Property ViewCommunityReports() As Boolean
            Get
                ViewCommunityReports = _CommunityReports
            End Get
            Set(ByVal Value As Boolean)
                _CommunityReports = Value
            End Set
        End Property
        Public Property PortalReports() As Boolean
            Get
                PortalReports = _PortalReports
            End Get
            Set(ByVal Value As Boolean)
                _PortalReports = Value
            End Set
        End Property
        Public Property ManagementPermission() As Boolean
            Get
                ManagementPermission = _ManagementPermission
            End Get
            Set(ByVal Value As Boolean)
                _ManagementPermission = Value
            End Set
        End Property
        Public Property Administration() As Boolean
            Get
                Administration = _Administration
            End Get
            Set(ByVal Value As Boolean)
                _Administration = Value
            End Set
        End Property
        Public Property Export() As Boolean
            Get
                Export = _Export
            End Get
            Set(ByVal Value As Boolean)
                _Export = Value
            End Set
        End Property
        Public Property Print() As Boolean
            Get
                Print = _Print
            End Get
            Set(ByVal Value As Boolean)
                _Print = Value
            End Set
        End Property
#End Region

        Sub New()
            Me._Administration = False
            Me._Export = False
            Me._SelfReport = True
            Me._CommunityReports = False
            Me._PortalReports = False
            Me._ManagementPermission = False
            Me._Print = True
        End Sub

        Sub New(permission As Long)
            Administration = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService, permission)
            Print = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService Or Base2Permission.Print, permission)
            Export = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService Or Base2Permission.Export, permission)
            ManagementPermission = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService Or Base2Permission.GrantPermission, permission)
            ViewCommunityReports = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService Or Base2Permission.ViewOtherReports, permission)
            ViewMyReport = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService Or Base2Permission.ViewMyReport, permission)
            PortalReports = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(Base2Permission.ViewPortalReports, permission)
        End Sub

        Public Shared Function CreatePortalmodule(userType As Integer) As ModuleUsageResult
            Dim moduleP As New ModuleUsageResult

            moduleP.Administration = (userType = lm.Comol.Core.DomainModel.UserTypeStandard.Administrator OrElse userType = lm.Comol.Core.DomainModel.UserTypeStandard.SysAdmin)
            moduleP.Export = (userType <> lm.Comol.Core.DomainModel.UserTypeStandard.Guest)
            moduleP.Print = (userType <> lm.Comol.Core.DomainModel.UserTypeStandard.Guest)
            moduleP.ViewMyReport = (userType <> lm.Comol.Core.DomainModel.UserTypeStandard.Guest)
            moduleP.ViewCommunityReports = False
            moduleP.PortalReports = (userType = lm.Comol.Core.DomainModel.UserTypeStandard.Administrator OrElse userType = lm.Comol.Core.DomainModel.UserTypeStandard.SysAdmin)
            moduleP.ManagementPermission = (userType = lm.Comol.Core.DomainModel.UserTypeStandard.Administrator OrElse userType = lm.Comol.Core.DomainModel.UserTypeStandard.SysAdmin)

            Return moduleP
        End Function
      
        <Flags()> _
        Public Enum Base2Permission
            ViewMyReport = 1
            ViewPortalReports = 16
            ViewOtherReports = 1024

            GrantPermission = 32
            AdminService = 64
            Export = 256
            Print = 2048
        End Enum
    End Class
End Namespace