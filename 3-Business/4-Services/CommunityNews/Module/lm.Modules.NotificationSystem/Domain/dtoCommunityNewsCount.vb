Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoCommunityNewsCount
        Inherits DomainObject(Of Integer)

        Public LastUpdate As DateTime
        Public Count As Integer

        Sub New()
            Count = 0
        End Sub
    End Class
End Namespace