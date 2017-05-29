Imports lm.Comol.Core.DomainModel
Imports lm.Modules.NotificationSystem.Presentation
Imports NHibernate
Imports NHibernate.Linq
Imports lm.Modules.NotificationSystem.WSremoteManagement

Namespace Business
    Public Class ManagerTemplates
        Inherits ManagerBase

        Private _ServiceManagement As WSremoteManagement.NotificationManagementSoapClient
        Private ReadOnly Property ServiceManagement() As WSremoteManagement.NotificationManagementSoapClient
            Get
                If IsNothing(_ServiceManagement) Then
                    _ServiceManagement = New WSremoteManagement.NotificationManagementSoapClient
                End If
                Return _ServiceManagement
            End Get
        End Property

        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
        End Sub
        Public Sub New(ByVal oUserContext As iUserContext, ByVal oDatacontext As iDataContext)
            MyBase.New(oUserContext, oDatacontext)
        End Sub

        Public Function ListModules() As List(Of dtoModule)
            Dim oList As List(Of dtoModule) = AvailableModules()
            Return oList
        End Function
        Public Function ListModuleAction(ByVal ModuleID As Integer) As List(Of dtoModuleAction)
            Dim oList As List(Of dtoModule) = AvailableModules()
            Dim iResponse As New List(Of dtoModuleAction)

            If oList.Count > 0 Then
                Dim oModule As dtoModule = (From nm In oList Where nm.ID = ModuleID Select nm).FirstOrDefault()
                If Not IsNothing(oModule) Then
                    iResponse = oModule.Actions
                End If
            End If
            Return iResponse
        End Function
        Public Function LoadMessages(ByVal ModuleID As Integer, ByVal ActionID As Integer, ByVal tTemplate As dtoTemplateType) As List(Of TranslatedMessage)
            Dim mList As List(Of TranslatedMessage)

            Try
                Dim oRemoteTemplates As List(Of dtoTemplateMessage)
                Dim oLanguages As List(Of Language) = (Me.DC.GetCurrentSession.Linq(Of Language)()).ToList()
                oRemoteTemplates = Me.ServiceManagement.AvailableTemplates(ModuleID, ActionID, tTemplate)
                mList = (From lang In oLanguages Group Join t In oRemoteTemplates On lang.Id Equals t.LanguageID Into children = Group _
                         From child In children.DefaultIfEmpty(New dtoTemplateMessage()) Select New TranslatedMessage() With {.LanguageID = lang.Id, .LanguageName = lang.Name, .Message = child.Message, .ID = child.ID, .TemplateName = child.Name}).tolist

            Catch ex As Exception
                mList = New List(Of TranslatedMessage)
            End Try

            Return mList
        End Function


        Public Function SaveTemplate(ByVal oTemplate As dtoTemplateMessage) As Long
            Dim TemplateID As Long = 0
            Try
                Dim ModuleCode As String = (From m In MyBase.GetGenericModuleList Where m.ID = oTemplate.ModuleID Select m.Code).FirstOrDefault
                If ModuleCode <> "" Then
                    oTemplate.ModuleCode = ModuleCode
                    TemplateID = ServiceManagement.SaveTemplate(oTemplate)
                End If
            Catch ex As Exception

            End Try
            Return TemplateID
        End Function

        Private Function AvailableModules() As List(Of dtoModule)
            Dim oRemoteModules As List(Of dtoModule)
            Try
                Dim cacheKey As String = CachePolicy.AvailableModules(Me.CurrentUserContext.Language.Id)

                If COL_BusinessLogic_v2.ObjectBase.Cache(cacheKey) Is Nothing Then
                    Dim oLocalModules As List(Of COL_BusinessLogic_v2.PlainService) = MyBase.GetGenericModuleList
                    oRemoteModules = Me.ServiceManagement.AvailableModules()

                    'var(LeftJoin = From emp In ListOfEmployees)  join dept in ListOfDepartment on emp.DeptID equals dept.ID into JoinedEmpDept From dept In JoinedEmpDept.DefaultIfEmpty()()select new                          {EmployeeName = emp.Name,DepartmentName = dept != null ? dept.Name : null                         };


                    Dim oTranslatedModules As List(Of dtoModule)
                    oTranslatedModules = (From o In oRemoteModules Group Join m In oLocalModules On o.ID Equals m.ID Into RightLocalModules = Group _
                                              From m In RightLocalModules.DefaultIfEmpty() _
                    Select New dtoModule() With {.ID = o.ID, .Actions = o.Actions, .Name = If(m Is Nothing, o.Name & "_", m.Name), .Code = o.Code}).ToList()


                    'Select New dtoModule() With {.ID = o.ID, .Actions = o.Actions, .Name = m.Name, .Code = o.Code}).ToList()
                    COL_BusinessLogic_v2.ObjectBase.Cache.Insert(cacheKey, oTranslatedModules, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza60minuti)
                Else
                    oRemoteModules = CType(COL_BusinessLogic_v2.ObjectBase.Cache(cacheKey), List(Of dtoModule))
                End If
            Catch ex As Exception
                oRemoteModules = New List(Of dtoModule)
            End Try
            Return oRemoteModules
        End Function
    End Class
End Namespace