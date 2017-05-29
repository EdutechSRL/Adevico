Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports System.Collections.Generic
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.UCServices.Services_Coordinamento
Imports lm.ActionDataContract
Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.EduPath.BusinessLogic

Public MustInherit Class EPlitePageBaseMoocs
    Inherits EPliteCommonPageBaseEduPath

#Region "Must Inherits"
    Public MustOverride Overrides ReadOnly Property AlwaysBind() As Boolean
    Public MustOverride Overrides ReadOnly Property VerifyAuthentication() As Boolean
    Public MustOverride Overrides Function HasPermessi() As Boolean
    Public MustOverride Overrides Sub RegistraAccessoPagina()
    Public MustOverride Overrides Sub BindNoPermessi()
    Public MustOverride Overrides Sub BindDati()
    Public MustOverride Overrides Sub SetCultureSettings()
    Public MustOverride Overrides Sub SetInternazionalizzazione()
    Public MustOverride Overrides Sub ShowMessageToPage(errorMessage As String)
#End Region

    Protected Overrides ReadOnly Property IsMoocPath As Boolean
        Get
            Return True
        End Get
    End Property

End Class