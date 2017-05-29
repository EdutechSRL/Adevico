Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoInfo

        Public Day As Date
        Public SentDate As DateTime
        Public UniqueID As System.Guid
        Public CommunityID As Integer
        Public CommunityName As String
        Public ModuleID As Integer
        Public ModuleName As String
        Sub New()

        End Sub
        Sub New(ByVal oDay As Date, ByVal oUniqueID As Guid, ByVal oCommunityID As Integer, ByVal oCommunityName As String, ByVal oSentdate As DateTime)
            Me.Day = oDay
            Me.UniqueID = oUniqueID
            Me.CommunityID = oCommunityID
            Me.CommunityName = oCommunityName
            Me.SentDate = oSentdate
        End Sub
        Sub New(ByVal oDay As Date, ByVal oUniqueID As Guid, ByVal oCommunityID As Integer, ByVal oCommunityName As String, ByVal oModuleID As Integer, ByVal oSentdate As DateTime)
            Me.Day = oDay
            Me.UniqueID = oUniqueID
            Me.CommunityID = oCommunityID
            Me.CommunityName = oCommunityName
            Me.ModuleID = oModuleID
            Me.SentDate = oSentdate
        End Sub
        Sub New(ByVal oDay As Date, ByVal oUniqueID As Guid, ByVal oCommunityID As Integer, ByVal oCommunityName As String, ByVal oModuleID As Integer, ByVal oModuleName As String, ByVal oSentdate As DateTime)
            Me.Day = oDay
            Me.UniqueID = oUniqueID
            Me.CommunityID = oCommunityID
            Me.CommunityName = oCommunityName
            Me.ModuleID = oModuleID
            Me.ModuleName = oModuleName
            Me.SentDate = oSentdate
        End Sub
    End Class
End Namespace
