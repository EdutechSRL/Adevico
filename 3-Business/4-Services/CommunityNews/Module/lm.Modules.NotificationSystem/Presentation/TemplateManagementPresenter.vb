Imports lm.Comol.Core.DomainModel.Common
Imports lm.Modules.NotificationSystem.Domain
Imports lm.Comol.Core.DomainModel
Imports lm.Modules.NotificationSystem.Business
Imports lm.Modules.NotificationSystem.WSremoteManagement
Namespace Presentation
    Public Class TemplateManagementPresenter
        Inherits DomainPresenter

#Region "PERMESSI"
        Private _Permission As ModuleNotificationManagement
        Private _CommunitiesPermission As IList(Of ModuleCommunityPermission(Of ModuleNotificationManagement))
        Private ReadOnly Property Permission(Optional ByVal CommunityID As Integer = 0) As ModuleNotificationManagement
            Get
                If IsNothing(_Permission) AndAlso CommunityID <= 0 Then
                    _Permission = Me.View.ModulePermission
                    Return _Permission
                ElseIf CommunityID > 0 Then
                    _Permission = (From o In CommunitiesPermission Where o.ID = CommunityID Select o.Permissions).FirstOrDefault
                    If IsNothing(_Permission) Then
                        _Permission = New ModuleNotificationManagement
                    End If
                    Return _Permission
                Else
                    Return _Permission
                End If
            End Get
        End Property
        Private ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleNotificationManagement))
            Get
                If IsNothing(_CommunitiesPermission) Then
                    _CommunitiesPermission = Me.View.CommunitiesPermission()
                End If
                Return _CommunitiesPermission
            End Get
        End Property
#End Region

#Region "Standard"
        Public Overloads Property CurrentManager() As ManagerTemplates
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerTemplates)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As ITemplateManagementView
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerTemplates(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As ITemplateManagementView)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerTemplates(MyBase.AppContext)
        End Sub
#End Region

        Public Sub InitView()
            If Not Me.UserContext.isAnonymous AndAlso HasPermissionToSee() Then
                Me.LoadModules()
                Me.LoadActions()
            Else
                Me.View.AddActionNoPermission(Me.UserContext.CurrentCommunityID, 0)
                Me.View.NoPermissionToAccess()
            End If
        End Sub

        Private Sub LoadModules()
            Dim oList As List(Of dtoModule) = Me.CurrentManager.ListModules
            If oList.Count = 0 Then
                Me.View.LoadModules(New List(Of Element(Of Integer)))
            Else
                Me.View.LoadModules((From nm In oList Select New Element(Of Integer) With {.ID = nm.ID, .Name = nm.Name}).ToList)
            End If

        End Sub
        Public Sub LoadActions()
            Dim ModuleID As Integer = Me.View.ModuleID
            Dim oActions As New List(Of Element(Of Integer))

            oActions = (From a In Me.CurrentManager.ListModuleAction(ModuleID) Select New Element(Of Integer) With {.ID = a.TypeID, .Name = a.Name}).ToList()
            Me.View.LoadAction(oActions)
            Me.LoadMessages()
        End Sub
        Public Sub LoadMessages()
            Dim ModuleID As Integer = Me.View.ModuleID
            Dim ActionID As Integer = Me.View.ActionID
            Dim TemplateTypeID As dtoTemplateType = Me.View.TemplateTypeID

            If Me.View.ModuleID > 0 AndAlso Me.View.ActionID <> 0 Then
                Dim oList As List(Of TranslatedMessage)
                oList = Me.CurrentManager.LoadMessages(ModuleID, ActionID, TemplateTypeID)
                If oList.Count = 0 Then
                    Me.View.AllowUpdate = Me.Permission.AddTemplate OrElse Me.Permission.Administration
                Else
                    Me.View.AllowUpdate = Me.Permission.EditTemplate OrElse Me.Permission.Administration
                End If
                Me.View.LoadMessages(oList)
            Else
                Me.View.AllowUpdate = False
                Me.View.LoadMessages(New List(Of TranslatedMessage))
            End If
        End Sub
        Public Sub SaveMessage()
            Dim oTranslatedList As List(Of TranslatedMessage) = Me.View.GetTranslatedMessages
            If oTranslatedList.Count > 0 Then
                Dim oTemplates As New List(Of dtoTemplateMessage)
                Dim ModuleID As Integer = Me.View.ModuleID
                Dim ActionID As Integer = Me.View.ActionID
                Dim TemplateTypeID As dtoTemplateType = Me.View.TemplateTypeID
                Dim TemplateID As Long = 0
                Dim tm As TranslatedMessage

                For i As Integer = 0 To oTranslatedList.Count - 1
                    tm = oTranslatedList(i)
                    TemplateID = Me.CurrentManager.SaveTemplate(New dtoTemplateMessage() With {.ActionID = ActionID, .ID = tm.ID, .LanguageID = tm.LanguageID, .Message = tm.Message, .ModuleID = ModuleID, .Name = tm.TemplateName, .Type = TemplateTypeID})
                    If TemplateID > 0 Then
                        Me.View.UpdateMessageID(TemplateID, i)
                    End If
                Next
            End If
        End Sub

        Private Function HasPermissionToSee() As Boolean
            Return Me.Permission.AddTemplate OrElse Me.Permission.Administration OrElse Me.Permission.EditTemplate
        End Function



    End Class
End Namespace