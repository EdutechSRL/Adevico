Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Presentation.Call

Public Class UC_PrintSettings
    Inherits BaseControl
    Implements IViewCallPrintSettings

#Region "Context"
    Private _Presenter As CallPrintSettingsPresenter
    Private ReadOnly Property Presenter() As CallPrintSettingsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CallPrintSettingsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("uc_PrintSettings", "Modules", "CallForPapers")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBtemplate_t)
            .setLabel(LBunselected_t)
            .setLabel(LBlayout_t)
            .setLabel(LBhighlightMandatory)
            .setLabel(LBLSectionTitle_t)
            .setLabel(LBLsectionDesc_t)
            .setLabel(LBLFieldTitle_t)
            .setLabel(LBLFieldDesc_t)
            .setLabel(LBLFieldEntry_t)

            .setCheckBox(CBXmandatory)

            
            .setDropDownList(DDLlayout, "0")
            .setDropDownList(DDLlayout, "1")
            .setDropDownList(DDLlayout, "19")
            .setDropDownList(DDLlayout, "28")
            .setDropDownList(DDLlayout, "37")
            .setDropDownList(DDLlayout, "46")
            .setDropDownList(DDLlayout, "55")
            .setDropDownList(DDLlayout, "64")
            .setDropDownList(DDLlayout, "73")
            .setDropDownList(DDLlayout, "82")
            .setDropDownList(DDLlayout, "91")

            .setRadioButtonList(RBLhiddenFields, "0")
            .setRadioButtonList(RBLhiddenFields, "1")

        End With

        CTRL_SectionTitleFont.SetInternazionalizzazione("SectionTitle", Resource)
        CTRL_SectionDescFont.SetInternazionalizzazione("SectionDesc", Resource)
        CTRL_FieldTitle.SetInternazionalizzazione("FieldTitle", Resource)
        CTRL_FieldDesc.SetInternazionalizzazione("FieldDesc", Resource)
        CTRL_FieldEntry.SetInternazionalizzazione("FieldEntry", Resource)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get

        End Get
    End Property

#Region "Inizializzazione"


    Public Sub InitControl(ByVal callForPeaperId As Int64)
        CallId = callForPeaperId
        Presenter.InitView(callForPeaperId)
    End Sub



    Private Property CallId
        Get
            Try
                Return System.Convert.ToInt64(Me.HDFcallId.Value)
            Catch ex As Exception

            End Try
            Return -1
        End Get
        Set(value)
            Me.HDFcallId.Value = value
        End Set
    End Property



    Public Sub Initialize(ByVal settings As CallPrintSettings, ByVal moduleId As Long) Implements lm.Comol.Modules.CallForPapers.Presentation.IViewCallPrintSettings.Initialize

        With settings
            'Selettore Template
            Me.CTRLtemplate.isInAjaxPanel = False
            Me.CTRLtemplate.EnabledSelectedIndexChanged = False

            '            Me.CTRLtemplate.AllowSelect = Not isReadOnly
            Me.CTRLtemplate.InitializeControl(.TemplateId, .VersionId, moduleId) 'ServiceEP.ServiceModuleID(), oOwner)

            RBLhiddenFields.SelectedValue = .UnselectFields
            DDLlayout.SelectedValue = .Layout
            CBXmandatory.Checked = .ShowMandatory

            CTRL_SectionTitleFont.FontSettings = .SectionTitle
            CTRL_SectionDescFont.FontSettings = .SectionDescription

            CTRL_FieldTitle.FontSettings = .FieldTitle
            CTRL_FieldDesc.FontSettings = .FieldDescription
            CTRL_FieldEntry.FontSettings = .FieldContent
        End With
    End Sub


    Public Sub UpdateSettings(ByRef settings As CallPrintSettings) Implements IViewCallPrintSettings.UpdateSettings
        'Implements lm.Comol.Modules.CallForPapers.Presentation.IViewCallPrintSettings.GetSettings
        If IsNothing(settings) Then
            settings = New CallPrintSettings()
        End If

        Dim version As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_sTemplateVersion = CTRLtemplate.SelectedItem

        With settings
            .TemplateId = version.IdTemplate
            .VersionId = version.Id
            .CallId = Me.CallId
            .UnselectFields = RBLhiddenFields.SelectedValue
            .Layout = DDLlayout.SelectedValue
            .ShowMandatory = CBXmandatory.Checked
            .SectionTitle = CTRL_SectionTitleFont.FontSettings
            .SectionDescription = CTRL_SectionDescFont.FontSettings

            .FieldTitle = CTRL_FieldTitle.FontSettings
            .FieldDescription = CTRL_FieldDesc.FontSettings
            .FieldContent = CTRL_FieldEntry.FontSettings

        End With
    End Sub

    Public WriteOnly Property IsReadOnly As Boolean
        Set(value As Boolean)

            Me.CTRLtemplate.AllowSelect = Not value

            Me.RBLhiddenFields.Enabled = Not value
            Me.DDLlayout.Enabled = Not value
            
            Me.CTRL_SectionTitleFont.IsReadonly = value
            Me.CTRL_SectionDescFont.IsReadonly = value

            Me.CTRL_FieldTitle.IsReadonly = value
            Me.CTRL_FieldDesc.IsReadonly = value
            Me.CTRL_FieldEntry.IsReadonly = value
            
        End Set
    End Property
#End Region

    Public Sub SaveSettings()
        Me.Presenter.SaveSetting()
    End Sub

End Class