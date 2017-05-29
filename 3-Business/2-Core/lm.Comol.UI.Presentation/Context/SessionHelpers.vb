Imports System.Web
Imports lm.Comol.Core.Data
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.UI.Presentation.Utility

Namespace lm.Comol.UI.Presentation
Public Class SessionHelpers

		Public Shared Function CurrentApplicationContext() As iApplicationContext
			Return New ApplicationContext With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
		End Function

		Public Shared Function CurrentUserContext() As iUserContext
			Dim oUserContext As New UserContext

            oUserContext.Language = GetUserLanguage()
			oUserContext.CurrentCommunityID = GetCommunityID()
			oUserContext.CurrentUser = GetCurrentUser()
			oUserContext.RolesID = GetCurrentRoles
			oUserContext.WorkSessionID = GetCurrentWorkSession()
			oUserContext.WorkingCommunityID = GetWorkingCommunity()
			oUserContext.isAnonymous = GetIsAnonymousUser()
            oUserContext.UserTypeID = GetUserTypeID()
            oUserContext.IpAddress = ClientIPadress()
            oUserContext.ProxyIpAddress = ProxyIPadress()
            oUserContext.CurrentCommunityOrganizationID = GetCurrentCommunityOrganizationID()
            oUserContext.UserDefaultOrganizationId = GetUserDefaultOrganizationId()
            Return oUserContext
        End Function

        Private Shared Function ClientIPadress() As String
            If Not TypeOf HttpContext.Current.Session("ClientIPadress") Is String Then
                HttpContext.Current.Session("ClientIPadress") = lm.Comol.UI.Utility.RemoteClientUtility.ClientAddress
            ElseIf String.IsNullOrEmpty(HttpContext.Current.Session("ClientIPadress")) Then
                HttpContext.Current.Session("ClientIPadress") = lm.Comol.UI.Utility.RemoteClientUtility.ClientAddress
            End If
            Return HttpContext.Current.Session("ClientIPadress")
        End Function
        Private Shared Function ProxyIPadress() As String
            If Not TypeOf HttpContext.Current.Session("ProxyIPadress") Is String Then
                HttpContext.Current.Session("ProxyIPadress") = lm.Comol.UI.Utility.RemoteClientUtility.ProxyAddress
            ElseIf String.IsNullOrEmpty(HttpContext.Current.Session("ClientIPadress")) Then
                HttpContext.Current.Session("ProxyIPadress") = lm.Comol.UI.Utility.RemoteClientUtility.ProxyAddress
            End If
            Return HttpContext.Current.Session("ProxyIPadress")
        End Function

        Public Shared Function CurrentDataContext() As iDataContext

            Dim currentSession As NHibernate.ISession = TryCast(HttpContext.Current.Items("nhibernate.current_session"), NHibernate.ISession)

            If IsNothing(currentSession) Then

                currentSession = lm.Comol.Core.Data.SessionHelper.GetNewSession()

            End If



            Return New DataContext(currentSession)

        End Function

        Private Shared Function GetCurrentUser() As iPerson
			If Not IsNothing(HttpContext.Current) AndAlso Not IsNothing(HttpContext.Current.Session) Then
				If TypeOf (HttpContext.Current.Session("objPersona")) Is COL_BusinessLogic_v2.CL_persona.COL_Persona Then
					Dim oPersona As COL_BusinessLogic_v2.CL_persona.COL_Persona = HttpContext.Current.Session("objPersona")
					Dim oPerson As New Person With {.Id = oPersona.ID, .Login = oPersona.Login, .Name = oPersona.Nome, .Surname = oPersona.Cognome, .Mail = oPersona.Mail}

					Return oPerson

				End If
				'If TypeOf (HttpContext.Current.Session("CurrentUser")) Is iPerson Then
				'	Return HttpContext.Current.Session("CurrentUser")

				'End If
			End If
			Return New Person With {.Id = 0}
        End Function
        Private Shared Function GetUserDefaultOrganizationId() As Integer
            Dim OrganizationId As Integer = 0
            Try
                If IsNumeric(HttpContext.Current.Session("ORGN_id")) Then
                    OrganizationId = HttpContext.Current.Session("ORGN_id")
                End If
            Catch ex As Exception
            End Try
            Return OrganizationId
        End Function
        Private Shared Function GetCurrentCommunityOrganizationID() As Integer
            Dim CurrentCommunityOrganizationID As Integer = 0
            Try
                If IsNumeric(HttpContext.Current.Session("CurrentCommunityOrganizationID")) Then
                    CurrentCommunityOrganizationID = HttpContext.Current.Session("CurrentCommunityOrganizationID")
                End If
            Catch ex As Exception
            End Try
            Return CurrentCommunityOrganizationID
        End Function
		Private Shared Function GetCommunityID() As Integer
			If Not IsNothing(HttpContext.Current) AndAlso Not IsNothing(HttpContext.Current.Session) Then
				If TypeOf (HttpContext.Current.Session("idComunita")) Is Integer Then
					Return HttpContext.Current.Session("idComunita")
				End If
			End If
		End Function
		Private Shared Function GetUserLanguage() As iLanguage
			If Not IsNothing(HttpContext.Current) AndAlso Not IsNothing(HttpContext.Current.Session) Then
				If TypeOf (HttpContext.Current.Session("UserLanguage")) Is lm.Comol.Core.DomainModel.iLanguage Then
					Return HttpContext.Current.Session("UserLanguage")
				End If
			End If
			' RITORNARE LINGUA DEFAULT !!!
			Return New Language With {.Id = 1, .Code = "it-IT", .isDefault = True, .Name = "Italiano"}
		End Function
		Private Shared Function GetCurrentRoles() As IList(Of Integer)
			Dim oRoleList As New List(Of Integer)
			If Not IsNothing(HttpContext.Current) AndAlso Not IsNothing(HttpContext.Current.Session) Then
				If TypeOf (HttpContext.Current.Session("IdRuolo")) Is Integer Then
					oRoleList.Add(CInt(HttpContext.Current.Session("IdRuolo")))
				End If
			End If
			Return oRoleList
		End Function
		Private Shared Function GetCurrentWorkSession() As System.Guid
			Dim iUniqueGuidSession As Guid = Guid.Empty

			If Not IsNothing(HttpContext.Current) AndAlso Not IsNothing(HttpContext.Current.Session) Then
                If TypeOf (HttpContext.Current.Session("UniqueGuidSession")) Is System.Guid Then
                    iUniqueGuidSession = HttpContext.Current.Session("UniqueGuidSession")
                End If
				If iUniqueGuidSession = Guid.Empty Then
					iUniqueGuidSession = Guid.NewGuid
					HttpContext.Current.Session("UniqueGuidSession") = iUniqueGuidSession
				End If
			End If
			Return iUniqueGuidSession
		End Function
		Private Shared Function GetWorkingCommunity() As System.Int32
            Dim idContainerCommunity As Integer = 0
			Dim UrlID As String = UrlEncryptHelper.DecryptCommonUrl("ContainerID")
			If Not UrlID = "" Then
				Try
                    idContainerCommunity = CInt(UrlID)
				Catch ex As Exception

				End Try
			Else
				Return GetCommunityID()
			End If
            Return idContainerCommunity
		End Function
		Private Shared Function GetIsAnonymousUser() As Boolean
			Dim oUserTypeID As Integer = 18	' Main.TipoPersonaStandard.Guest
            If Not IsNothing(HttpContext.Current.Session("objPersona")) AndAlso TypeOf (HttpContext.Current.Session("objPersona")) Is COL_BusinessLogic_v2.CL_persona.COL_Persona Then
                Dim oPersona As COL_BusinessLogic_v2.CL_persona.COL_Persona = HttpContext.Current.Session("objPersona")
                Try
                    If oPersona.TipoPersona.ID <> oUserTypeID Then
                        Return False
                    End If
                Catch ex As Exception
                End Try
            End If
			Return True
		End Function
		Private Shared Function GetUserTypeID() As Integer
			If Not IsNothing(HttpContext.Current) AndAlso Not IsNothing(HttpContext.Current.Session) Then
				If TypeOf (HttpContext.Current.Session("objPersona")) Is COL_BusinessLogic_v2.CL_persona.COL_Persona Then
					Dim oPersona As COL_BusinessLogic_v2.CL_persona.COL_Persona = HttpContext.Current.Session("objPersona")
					Dim oPerson As New Person With {.Id = oPersona.ID, .Login = oPersona.Login, .Name = oPersona.Nome, .Surname = oPersona.Cognome, .Mail = oPersona.Mail}

					Return oPersona.TipoPersona.ID
				Else
					Return COL_BusinessLogic_v2.Main.TipoPersonaStandard.Guest
				End If
			End If
			Return COL_BusinessLogic_v2.Main.TipoPersonaStandard.Guest
		End Function
	End Class
End Namespace