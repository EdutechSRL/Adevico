Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports COL_BusinessLogic_v2
Imports lm.Modules.NotificationSystem.Domain

Namespace Presentation
    <CLSCompliant(True)> Public Interface ITemplateManagementView
        Inherits iDomainView

        ReadOnly Property CommunitiesPermission() As IList(Of ModuleNotificationManagement)
        ReadOnly Property ModulePermission() As ModuleNotificationManagement
        ReadOnly Property TemplateTypeID() As Integer
        ReadOnly Property ModuleID() As Integer
        ReadOnly Property ActionID() As Integer
        ReadOnly Property ActionName() As String
        WriteOnly Property AllowUpdate() As Boolean

        Sub LoadModules(ByVal modules As List(Of Element(Of Integer)))
        Sub LoadAction(ByVal actions As List(Of Element(Of Integer)))
        Sub LoadMessages(ByVal messages As List(Of TranslatedMessage))
        Function GetTranslatedMessages() As List(Of TranslatedMessage)
        Sub NoPermissionToAccess()
        Sub AddActionNoPermission(ByVal CommunityID As Integer, ByVal PersonID As Integer)
        Sub UpdateMessageID(ByVal TemplateID As Long, ByVal RowNumber As Integer)
    End Interface
End Namespace