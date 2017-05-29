Namespace Presentation
    <CLSCompliant(True), Serializable()> Public Class dtoModuleMessage
        Public UniqueID As System.Guid
        Public SentDate As DateTime
        Public Day As DateTime
        Public Message As String

        Public Sub New()

        End Sub
        Public Sub New(ByVal ID As System.Guid, ByVal oDate As DateTime, ByVal oDay As DateTime, ByVal oMessage As String)
            UniqueID = ID
            SentDate = oDate
            Day = oDay
            Message = oMessage
        End Sub
    End Class
End Namespace