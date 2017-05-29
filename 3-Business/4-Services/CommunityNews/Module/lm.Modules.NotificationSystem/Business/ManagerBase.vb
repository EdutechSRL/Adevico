Imports lm.Comol.Core.DomainModel.Common
Imports NHibernate
Imports NHibernate.Linq
Imports lm.Comol.Core.DomainModel

Namespace Business
    Public Class ManagerBase
        Inherits COL_BusinessLogic_v2.ObjectBase
        Implements iDomainManager



#Region "Private property"

        Private _UserContext As iUserContext
        Private _Datacontext As iDataContext
#End Region
      
        Protected ReadOnly Property DC() As iDataContext
            Get
                Return _Datacontext
            End Get
        End Property
        Protected ReadOnly Property CurrentUserContext() As iUserContext
            Get
                Return _UserContext
            End Get
        End Property
        Public Sub New()
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext)
            Me._UserContext = oContext.UserContext
            Me._Datacontext = oContext.DataContext
        End Sub
        Public Sub New(ByVal oUserContext As iUserContext, ByVal oDatacontext As iDataContext)
            Me._UserContext = oUserContext
            Me._Datacontext = oDatacontext
        End Sub


        Protected Function GetGenericModuleList() As List(Of COL_BusinessLogic_v2.PlainService)
            Dim oList As New List(Of COL_BusinessLogic_v2.PlainService)

            Try
                oList = COL_BusinessLogic_v2.Comol.Manager.ManagerService.ListSystemTranslated(CurrentUserContext.Language.Id)
                Return oList
            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try

            Return oList
        End Function
        Public Function GetCommunity(ByVal CommunityID As Integer) As Community
            Dim oCommunity As Community = Nothing

            Try
                oCommunity = _Datacontext.GetById(Of Community)(CommunityID)
            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            If IsNothing(oCommunity) Then
                oCommunity = New Community
            End If
            Return oCommunity
        End Function
        Public Function GetCommunitiesList(ByVal oListID As List(Of Integer)) As List(Of Community)
            Dim oList As List(Of Community) = New List(Of Community)

            Try
                For Each CommunityID As Integer In oListID
                    Dim oCommunity As iCommunity = Me.GetCommunity(CommunityID)
                    If Not IsNothing(oCommunity) Then : oList.Add(oCommunity)

                    End If
                Next
            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            Return oList
        End Function
        Public Function GetCommunitiesSubscribed(ByVal PersonID As Integer, ByVal oListID As List(Of Integer), ByVal orderBy As OrderSubscriptionBy) As List(Of Community)
            Dim oList As List(Of Community) = New List(Of Community)

            If oListID.Count = 0 Then
                Return oList
            End If
            Try
                Dim oSubscriptions As List(Of Subscription)
                Dim oPerson As Person = _Datacontext.GetById(Of Person)(PersonID)

                Select Case orderBy
                    Case OrderSubscriptionBy.Name
                        oSubscriptions = (From subs In DC.GetCurrentSession.Linq(Of Subscription)() Where subs.Person Is oPerson Order By subs.Community.Name Select subs).ToList
                    Case OrderSubscriptionBy.LastAccess
                        oSubscriptions = (From subs In DC.GetCurrentSession.Linq(Of Subscription)() Where subs.Person Is oPerson Order By subs.LastAccessOn Descending Select subs).ToList
                    Case OrderSubscriptionBy.SubscriptedOn
                        oSubscriptions = (From subs In DC.GetCurrentSession.Linq(Of Subscription)() Where subs.Person Is oPerson Order By subs.SubscriptedOn Descending Select subs).ToList
                    Case Else
                        oSubscriptions = (From subs In DC.GetCurrentSession.Linq(Of Subscription)() Where subs.Person Is oPerson Order By subs.SubscriptedOn Descending Select subs).ToList
                End Select

                oList = (From s In oSubscriptions Join remoteID In oListID On s.Community.Id Equals remoteID Where s.Accepted AndAlso s.Enabled Select DirectCast(s.Community, Community)).ToList

               


            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            Return oList
        End Function
        Public Function GetSubscription(ByVal CommunityID As Integer, ByVal PersonID As Integer) As Subscription
            Dim oCommunity As Community = Nothing
            Dim oPerson As Person = Nothing
            Dim oSubscription As Subscription = Nothing

            Try
                oCommunity = _Datacontext.GetById(Of Community)(CommunityID)
                oPerson = _Datacontext.GetById(Of Person)(PersonID)
                oSubscription = (From subs In DC.GetCurrentSession.Linq(Of Subscription)() Where subs.Person Is oPerson AndAlso subs.Community Is oCommunity Select subs).FirstOrDefault

            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            Return oSubscription
        End Function
        Public Function isCommunityMember(ByVal PersonID As Integer, ByVal CommunityID As Integer) As Boolean
            Dim isMember As Boolean = False
            Try
                Dim oPerson As Person = _Datacontext.GetById(Of Person)(PersonID)
                Dim oCommunity As Community = _Datacontext.GetById(Of Community)(CommunityID)

                isMember = ((From subs In DC.GetCurrentSession.Linq(Of Subscription)() Where subs.Accepted AndAlso subs.Enabled AndAlso subs.Person Is oPerson AndAlso subs.Community Is oCommunity Select subs.Id).Count() = 1)

            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            Return isMember
        End Function
        Enum OrderSubscriptionBy
            Name
            LastAccess
            SubscriptedOn
        End Enum
    End Class
End Namespace