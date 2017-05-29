Namespace lm.Comol.Modules.UserActions.DomainModel
	<Serializable(), CLSCompliant(True)> Public Class ModuleStatistics
        Public Const UniqueId As String = "SRVSTAT"

#Region "Private Property"
		Private _ViewDetails As Boolean
		Private _ListSelfStatistic As Boolean
		Private _ListOtherStatistic As Boolean
		Private _ViewGenericOtherStatistic As Boolean
		Private _ManagementPermission As Boolean
		Private _Administration As Boolean
        Private _Export As Boolean
        Private _IsForHistory As Boolean
#End Region

#Region "Public Property"
        Public Property IsForHistory() As Boolean
            Get
                IsForHistory = _IsForHistory
            End Get
            Set(ByVal Value As Boolean)
                _IsForHistory = Value
            End Set
        End Property
        Public Property ViewDetails() As Boolean
            Get
                ViewDetails = _ViewDetails
            End Get
            Set(ByVal Value As Boolean)
                _ViewDetails = Value
            End Set
        End Property
        Public Property ListSelfStatistic() As Boolean
            Get
                ListSelfStatistic = _ListSelfStatistic
            End Get
            Set(ByVal Value As Boolean)
                _ListSelfStatistic = Value
            End Set
        End Property
        Public Property ListOtherStatistic() As Boolean
            Get
                ListOtherStatistic = _ListOtherStatistic
            End Get
            Set(ByVal Value As Boolean)
                _ListOtherStatistic = Value
            End Set
        End Property
        Public Property ViewGenericOtherStatistic() As Boolean
            Get
                ViewGenericOtherStatistic = _ViewGenericOtherStatistic
            End Get
            Set(ByVal Value As Boolean)
                _ViewGenericOtherStatistic = Value
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
#End Region

		Sub New()
			Me._Administration = False
			Me._Export = False
			Me._ListOtherStatistic = False
			Me._ListSelfStatistic = True
			Me._ManagementPermission = False
			Me._ViewDetails = False
			Me._ViewGenericOtherStatistic = False
		End Sub
        Sub New(permission As Long)
            Administration = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService, permission)
            Export = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService Or Base2Permission.Export Or Base2Permission.Management, permission)
            ListOtherStatistic = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService Or Base2Permission.Management, permission)
            ViewDetails = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService Or Base2Permission.Management, permission)
            ListSelfStatistic = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService Or Base2Permission.Management Or Base2Permission.View, permission)
            ViewGenericOtherStatistic = ListSelfStatistic
            ManagementPermission = lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(Base2Permission.AdminService Or Base2Permission.GrantPermission, permission)

        End Sub
        ' //public ModuleProfileManagement(long permission)
        '//{
        '//    ViewDiaryItems = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ViewLessons | (long)Base2Permission.EditLesson | (long)Base2Permission.AdminService , permission);
        '//    Edit = PermissionHelper.CheckPermissionSoft((long)Base2Permission.EditLesson | (long)Base2Permission.AdminService , permission);
        '//    PrintList = PermissionHelper.CheckPermissionSoft((long)Base2Permission.ViewLessons | (long)Base2Permission.EditLesson | (long)Base2Permission.AdminService , permission);
        '//    ManagementPermission = PermissionHelper.CheckPermissionSoft((long)Base2Permission.GrantPermission , permission);
        '//    UploadFile = PermissionHelper.CheckPermissionSoft((long)Base2Permission.UploadFile | (long)Base2Permission.AddLesson | (long)Base2Permission.EditLesson | (long)Base2Permission.AdminService , permission);
        '//    AddItem = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AddLesson | (long)Base2Permission.AdminService, permission);
        '//    Administration = PermissionHelper.CheckPermissionSoft((long)Base2Permission.AdminService, permission);
        '//    DeleteItem  = PermissionHelper.CheckPermissionSoft((long)Base2Permission.DeleteLesson | (long) Base2Permission.AdminService , permission); 
        '//}

        Public Shared Function CreatePortalmodule(userType As Integer) As ModuleStatistics
            Dim moduleP As New ModuleStatistics

            moduleP.Administration = (userType = lm.Comol.Core.DomainModel.UserTypeStandard.Administrator OrElse userType = lm.Comol.Core.DomainModel.UserTypeStandard.SysAdmin)
            moduleP.Export = (moduleP.Administration OrElse userType = lm.Comol.Core.DomainModel.UserTypeStandard.Administrative)
            moduleP.ListOtherStatistic = moduleP.Administration
            moduleP.ViewDetails = moduleP.Administration
            moduleP.ListSelfStatistic = (userType <> lm.Comol.Core.DomainModel.UserTypeStandard.Guest)
            moduleP.ViewGenericOtherStatistic = moduleP.ListSelfStatistic
            moduleP.ManagementPermission = (userType = lm.Comol.Core.DomainModel.UserTypeStandard.Administrator OrElse userType = lm.Comol.Core.DomainModel.UserTypeStandard.SysAdmin)

            Return moduleP
        End Function

        <Flags()> _
        Public Enum Base2Permission
            View = 1
            Management = 16
            GrantPermission = 32
            AdminService = 64
            Export = 256
        End Enum

        Public Enum ActionType
            None = 0
            NoPermission = 71000
            GenericError = 70002
            'ListGlobalPortal = 71003
            'ListGlobalCommunity = 71004
            'ViewGenericDetails = 71005
            'ViewModuleDetails = 71006
            LoadPortalMyPersonalStatistics = 71007
            LoadPortalUsersStatistics = 71008
            LoadPortalStatistics = 71009
            LoadPortalUserStatistics = 71010
            LoadCommunityMyPersonalStatistics = 71011
            LoadCommunityUsersStatistics = 71012
            LoadCommunityUserStatistics = 71013
            LoadCommunityStatistics = 71014
            LoadModuleCommunityStatistics = 71015
            LoadModuleTimeStatistics = 71016
            UserNotFound = 71017
            LoadPortalUserCommunityStatistics = 71018
            LoadPortalCommunitiesStatistics = 71019
            LoadPortalAccessInfo = 71020
            LoadCommunityAccessInfo = 71020
        End Enum
        Public Enum ObjectType
            None = 0
            User = 1
            Community = 2
        End Enum
    End Class
End Namespace