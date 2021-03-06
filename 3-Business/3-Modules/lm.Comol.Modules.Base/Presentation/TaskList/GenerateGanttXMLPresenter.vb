﻿Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.Modules.TaskList.Business
Imports lm.Comol.Modules.TaskList.Domain


Namespace lm.Comol.Modules.Base.Presentation.TaskList
    Public Class GenerateGanttXMLPresenter
        Inherits DomainPresenter

        Private _BaseManager As ManagerCommon
        Private _BaseTaskManager As TaskManager

#Region "Standard"

        Public Property BaseManager() As ManagerCommon
            Get
                Return _BaseManager
            End Get
            Set(ByVal value As ManagerCommon)
                _BaseManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IViewGenerateGanttXML
            Get
                Return MyBase.View
            End Get
        End Property
        Public Property CurrentTaskManager() As TaskManager
            Get
                Return _BaseTaskManager
            End Get
            Set(ByVal value As TaskManager)
                _BaseTaskManager = value
            End Set
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            Me.CurrentTaskManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
            Me.BaseManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewGenerateGanttXML)
            MyBase.New(oContext, view)
            Me.CurrentTaskManager = New TaskManager(MyBase.AppContext.DataContext.GetCurrentSession)
            Me.BaseManager = New ManagerCommon(MyBase.AppContext)
        End Sub
#End Region

        Public Sub InitView(ByVal BaseUrl As String)
            Dim oProject As ProjectForGanttXML
            If IsNothing(Me.UserContext.CurrentUser) OrElse Me.UserContext.isAnonymous OrElse Me.UserContext.CurrentUser.Id <= 0 Then
                oProject = New ProjectForGanttXML
            Else
                Dim TaskPermission As TaskPermissionEnum = Me.CurrentTaskManager.GetPermissionsOverTask(Me.View.ProjectID, Me.AppContext.UserContext.CurrentUserID)

                TaskPermission = Me.CurrentTaskManager.GetPermissionOverAllProject(Me.View.ProjectID, Me.AppContext.UserContext.CurrentUserID)
                If (TaskPermission And TaskPermissionEnum.TaskView) = TaskPermissionEnum.TaskView Then
                    oProject = CurrentTaskManager.GetProjectForGantXML(Me.View.ProjectID, BaseUrl)
                Else
                    oProject = New ProjectForGanttXML

                End If
            End If
            Me.View.GenerateGanttXML(oProject)
        End Sub

    End Class
End Namespace