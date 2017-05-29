Public Interface I_Uc_FileCom
    Inherits IcommunityContainer

    Sub BindListUtenti(ByVal oList As List(Of StatFileUsr))

    Property RecordTotali() As Integer
    'Property PageIndex() As Integer
    Property PageSize() As Integer
    Property OrderDirection() As Boolean
    Property OrderField() As Presenter_Uc_FileCom.OrderFields
    Property Pager() As lm.Comol.Core.DomainModel.PagerBase

End Interface