Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Web
Imports NHibernate
Imports NHibernate.Cfg


Namespace lm.Comol.UI.Presentation
	Public NotInheritable Class SessionManager
		Private Const CurrentSessionKey As String = "nhibernate.current_session"
		Private Shared ReadOnly sessionFactory As ISessionFactory

        Shared Sub New()
            Try
                sessionFactory = lm.Comol.Core.Data.SessionHelper.GetCurrentFactory  ' New Configuration().Configure().BuildSessionFactory()
            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try

        End Sub

		Public Shared Function GetCurrentSession() As ISession
			Dim context As HttpContext = HttpContext.Current
			Dim currentSession As ISession = TryCast(context.Items(CurrentSessionKey), ISession)

            If currentSession Is Nothing Then
                Try
                    currentSession = sessionFactory.OpenSession()
                    context.Items(CurrentSessionKey) = currentSession
                Catch ex As Exception
                    Debug.Write(ex.ToString)
                End Try
            End If

			Return currentSession
		End Function

		Public Shared Sub CloseSession()
			Dim context As HttpContext = HttpContext.Current
			Dim currentSession As ISession = TryCast(context.Items(CurrentSessionKey), ISession)

			If currentSession Is Nothing Then
				Exit Sub
				' No current session
            End If

            If currentSession.IsOpen Then
                currentSession.Close()
            End If
			context.Items.Remove(CurrentSessionKey)
		End Sub

		Public Shared Sub CloseSessionFactory()
			If sessionFactory IsNot Nothing Then
				sessionFactory.Close()
			End If
		End Sub



	End Class
End Namespace