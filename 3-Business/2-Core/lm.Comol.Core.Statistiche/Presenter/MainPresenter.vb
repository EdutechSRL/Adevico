Public Class MainPresenter
    Private _view As IViewMainStat

    Public Sub New(ByVal view As IViewMainStat)
        Me._view = view
    End Sub

    Public Sub init(Optional ByVal service As StatGeneric.EnumUCService = StatGeneric.EnumUCService.ScormUser)

    End Sub

End Class
