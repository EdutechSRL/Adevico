Namespace Presentation
    <CLSCompliant(True), Serializable()> Public Class dtoModuleNews
        Public CommunityID As Integer
        Public ModuleID As Integer
        Public ModuleName As String
        Public ModuleDefaultUrl As String
        Public News As List(Of dtoModuleMessage)
        Public Sub New()

        End Sub
        Public Sub New(ByVal m As COL_BusinessLogic_v2.PlainService, ByVal CommunityID As Integer)
            CommunityID = CommunityID
            ModuleID = m.ID
            ModuleName = m.Name
        End Sub
    End Class
End Namespace