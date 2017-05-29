Namespace lm.Comol.Modules.UserActions.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class ModuleOnLineUser
        Public Const UniqueId As String = "SRVOLUSR"

        Public Administration As Boolean
        Public Export As Boolean
        Public ManagementPermission As Boolean
        Public Print As Boolean
        Public ViewUsersAndModuleOnLine As Boolean
        Public ViewUsersOnLine As Boolean
        Public ViewUsersWithAction As Boolean
        Public ViewUsersOnLineLowDetails As Boolean

        Sub New()
            Me.ViewUsersWithAction = False
            Me.Export = False
            Me.ManagementPermission = False
            Me.Print = False
            Me.ViewUsersAndModuleOnLine = False
            Me.ViewUsersOnLine = False
            Me.ViewUsersWithAction = False
            Me.ViewUsersOnLineLowDetails = True
        End Sub

        Sub New(permission As Long)
            Administration = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService, permission)
            Print = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService Or Base2Permission.Print, permission)
            Export = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService Or Base2Permission.Export, permission)
            ManagementPermission = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService Or Base2Permission.GrantPermission, permission)
            ViewUsersOnLine = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService Or Base2Permission.ViewUsersOnLine, permission)
            ViewUsersWithAction = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService Or Base2Permission.ViewUsersWithAction, permission)
            ViewUsersAndModuleOnLine = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(Base2Permission.ViewUsersAndModuleOnLine, permission)
            ViewUsersOnLineLowDetails = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(Base2Permission.ViewUsersOnLineLowDetails, permission)
        End Sub

        Public Shared Function CreatePortalmodule(userType As Integer) As ModuleOnLineUser
            Dim moduleP As New ModuleOnLineUser

            moduleP.Administration = (userType = lm.Comol.Core.DomainModel.UserTypeStandard.Administrator OrElse userType = lm.Comol.Core.DomainModel.UserTypeStandard.SysAdmin)
            moduleP.Export = (userType = lm.Comol.Core.DomainModel.UserTypeStandard.Administrator OrElse userType = lm.Comol.Core.DomainModel.UserTypeStandard.SysAdmin OrElse userType = lm.Comol.Core.DomainModel.UserTypeStandard.Administrative)
            moduleP.Print = (userType = lm.Comol.Core.DomainModel.UserTypeStandard.Administrator OrElse userType = lm.Comol.Core.DomainModel.UserTypeStandard.SysAdmin OrElse userType = lm.Comol.Core.DomainModel.UserTypeStandard.Administrative)
            moduleP.ManagementPermission = (userType = lm.Comol.Core.DomainModel.UserTypeStandard.Administrator OrElse userType = lm.Comol.Core.DomainModel.UserTypeStandard.SysAdmin)

            moduleP.ViewUsersOnLineLowDetails = (userType <> lm.Comol.Core.DomainModel.UserTypeStandard.Guest)
            moduleP.ViewUsersOnLine = (userType = lm.Comol.Core.DomainModel.UserTypeStandard.Administrator OrElse userType = lm.Comol.Core.DomainModel.UserTypeStandard.SysAdmin OrElse userType = lm.Comol.Core.DomainModel.UserTypeStandard.Administrative)
            moduleP.ViewUsersAndModuleOnLine = (userType = lm.Comol.Core.DomainModel.UserTypeStandard.Administrator OrElse userType = lm.Comol.Core.DomainModel.UserTypeStandard.SysAdmin OrElse userType = lm.Comol.Core.DomainModel.UserTypeStandard.Administrative)
            moduleP.ViewUsersWithAction = (userType = lm.Comol.Core.DomainModel.UserTypeStandard.Administrator OrElse userType = lm.Comol.Core.DomainModel.UserTypeStandard.SysAdmin)

            Return moduleP
        End Function

        <Flags()> _
        Public Enum Base2Permission
            ViewUsersOnLine = 1
            ViewUsersWithAction = 16
            ViewUsersAndModuleOnLine = 1024

            GrantPermission = 32
            AdminService = 64
            ViewUsersOnLineLowDetails = 256
            Export = 128
            Print = 2048
        End Enum

        Public Enum ActionType
            None = 0
            NoPermission = 1
            GenericError = 2
            ViewUsersOnLine = 75003
            ViewUsersOnLineModule = 75004
            ViewUsersOnLineAction = 75005
            ViewAllUsersOnLine = 75006
            ViewCommunityUsersOnLine = 75007
            FindUsersOnLine = 75008
        End Enum
        Public Enum ObjectType
            None = 0
            Community = 1
        End Enum

    End Class
End Namespace