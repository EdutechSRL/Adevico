Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Web
Imports NHibernate
'Imports ProductModel.Session


'See http://www.codeproject.com/KB/architecture/NHibernateBestPractices.aspx#WEB
Namespace lm.Comol.UI.Presentation
	Public Class SessionModule
		Implements IHttpModule

		Private _session As ISession

		Public Sub Init(ByVal context As HttpApplication) Implements System.Web.IHttpModule.Init
			AddHandler context.BeginRequest, AddressOf BeginTransaction
			AddHandler context.EndRequest, AddressOf CloseSession
		End Sub

		Private Sub CloseSession(ByVal sender As Object, ByVal e As EventArgs)
			SessionManager.CloseSession()
		End Sub

		'Create our session
		Private Sub BeginTransaction(ByVal sender As Object, ByVal e As EventArgs)
			_session = SessionManager.GetCurrentSession()
		End Sub

        Public Sub Dispose() Implements System.Web.IHttpModule.Dispose
            If _session.IsOpen Then
                _session.Close()
            End If
            _session.Dispose()
            _session = Nothing
        End Sub

	End Class
End Namespace