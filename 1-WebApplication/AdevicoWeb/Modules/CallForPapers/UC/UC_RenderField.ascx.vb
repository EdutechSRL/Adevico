Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Public Class UC_RenderField
    Inherits BaseControl
    Implements IViewRenderField

#Region "Context"
    Private _Presenter As RenderFieldPresenter
    Private ReadOnly Property CurrentPresenter() As RenderFieldPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New RenderFieldPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"

    Private Property Disabled As Boolean Implements IViewRenderField.Disabled
        Get
            Return ViewStateOrDefault("Disabled", False)
        End Get
        Set(value As Boolean)
            ViewState("Disabled") = value
        End Set
    End Property
    Private Property MaxChars As Integer Implements IViewRenderField.MaxChars
        Get
            Return ViewStateOrDefault("MaxChars", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("MaxChars") = value
        End Set
    End Property
    Private Property MaxOptions As Integer Implements IViewRenderField.MaxOptions
        Get
            Return ViewStateOrDefault("MaxOptions", CInt(1))
        End Get
        Set(value As Integer)
            ViewState("MaxOptions") = value
        End Set
    End Property

    Private Property MinOptions As Integer Implements IViewRenderField.MinOptions
        Get
            Return ViewStateOrDefault("MinOptions", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("MinOptions") = value
        End Set
    End Property
    Public Property CurrentError As FieldError Implements IViewRenderField.CurrentError
        Get
            Return ViewStateOrDefault("CurrentError", FieldError.None)
        End Get
        Set(value As FieldError)
            ViewState("CurrentError") = value
        End Set
    End Property
    Public Property FieldType As FieldType Implements IViewRenderField.FieldType
        Get
            Return ViewStateOrDefault("FieldType", FieldType.None)
        End Get
        Set(value As FieldType)
            ViewState("FieldType") = value
        End Set
    End Property
    Public Property IdField As Long Implements IViewRenderField.IdField
        Get
            Return ViewStateOrDefault("IdField", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdField") = value
        End Set
    End Property
    Public Property Selected As Boolean Implements IViewRenderField.Selected
        Get
            Select Case FieldType
                Case lm.Comol.Modules.CallForPapers.Domain.FieldType.CheckboxList, lm.Comol.Modules.CallForPapers.Domain.FieldType.DropDownList, _
                    lm.Comol.Modules.CallForPapers.Domain.FieldType.RadioButtonList, lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer, _
                    lm.Comol.Modules.CallForPapers.Domain.FieldType.FileInput, lm.Comol.Modules.CallForPapers.Domain.FieldType.Date, lm.Comol.Modules.CallForPapers.Domain.FieldType.DateTime, lm.Comol.Modules.CallForPapers.Domain.FieldType.Time, FieldType.MultiLine

                    Dim oChecBox As CheckBox = FindControl("CBX" & FieldType.ToString & "RevisionField")
                    Return oChecBox.Checked
                Case FieldType.CompanyCode, FieldType.CompanyTaxCode, FieldType.SingleLine, FieldType.Name, FieldType.Surname, _
                    FieldType.TaxCode, FieldType.TelephoneNumber, FieldType.VatCode, FieldType.ZipCode
                    Return CBXsinglelineRevisionField.Checked
                Case Else
                    Return False
            End Select
        End Get
        Set(value As Boolean)
            Select Case FieldType
                Case lm.Comol.Modules.CallForPapers.Domain.FieldType.CheckboxList, lm.Comol.Modules.CallForPapers.Domain.FieldType.DropDownList, _
                    lm.Comol.Modules.CallForPapers.Domain.FieldType.RadioButtonList, lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer, _
                    lm.Comol.Modules.CallForPapers.Domain.FieldType.FileInput, lm.Comol.Modules.CallForPapers.Domain.FieldType.Date, lm.Comol.Modules.CallForPapers.Domain.FieldType.DateTime, lm.Comol.Modules.CallForPapers.Domain.FieldType.Time, FieldType.MultiLine

                    Dim oChecBox As CheckBox = FindControl("CBX" & FieldType.ToString & "RevisionField")
                    oChecBox.Checked = value
                Case FieldType.CompanyCode, FieldType.CompanyTaxCode, FieldType.SingleLine, FieldType.Name, FieldType.Surname, _
                    FieldType.TaxCode, FieldType.TelephoneNumber, FieldType.VatCode, FieldType.ZipCode
                    CBXsinglelineRevisionField.Checked = value

            End Select
        End Set
    End Property
    Private Property AllowRevisionCheck As Boolean Implements IViewRenderField.AllowRevisionCheck
        Get
            Return ViewStateOrDefault("AllowRevisionCheck", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowRevisionCheck") = value
        End Set
    End Property
    Private Property RevisionCount As Long Implements IViewRenderField.RevisionCount
        Get
            Return ViewStateOrDefault("RevisionCount", CLng(0))
        End Get
        Set(value As Long)
            ViewState("RevisionCount") = value
        End Set
    End Property
    Public Property ShowFieldChecked As Boolean Implements IViewRenderField.ShowFieldChecked
        Get
            Return ViewStateOrDefault("ShowFieldChecked", False)
        End Get
        Set(value As Boolean)
            ViewState("ShowFieldChecked") = value
        End Set
    End Property

#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected ReadOnly Property CssError As String
        Get
            Return IIf(CurrentError = FieldError.None, "", "error")
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCall", "Modules", "CallForPapers")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            LBDisclaimerAccept.Text = .getValue("RBLdisclaimer.True")
            LBDisclaimerRefuse.Text = .getValue("RBLdisclaimer.")
        End With
    End Sub
#End Region
    Public Sub InitializeControl(item As dtoSubmissionValueField, disabled As Boolean, isPublic As Boolean, revisionCheck As Boolean) Implements IViewRenderField.InitializeControl
        CurrentError = FieldError.None
        allowRevisionCheck = revisionCheck
        CurrentPresenter.InitView(item, disabled, isPublic)
    End Sub
    Public Sub InitializeControl(item As dtoSubmissionValueField, disabled As Boolean, isPublic As Boolean, err As FieldError, revisionCheck As Boolean) Implements IViewRenderField.InitializeControl
        CurrentError = err
        allowRevisionCheck = revisionCheck
        CurrentPresenter.InitView(item, disabled, isPublic)

        If Not IsNothing(item) AndAlso Not IsNothing(item.Field) Then
            Dim oLabel As Label = FindControl("LBerrorMessage" & item.Field.Type.ToString.ToLower)

            If Not IsNothing(oLabel) Then
                oLabel.Visible = Not (err = FieldError.None)

                If Not (err = FieldError.None) Then
                    oLabel.Text = Resource.getValue("FieldError." & err.ToString())
                End If
            End If
        End If
    End Sub
    Private Sub SetupView(item As dtoSubmissionValueField, isPublic As Boolean) Implements IViewRenderField.SetupView


        IdField = item.Field.Id
        RevisionCount = item.RevisionsCount
        FieldType = item.Field.Type


        Dim oLiteral As Literal = FindControl("LTmaxChars" & item.Field.Type.ToString.ToLower)
        If Not IsNothing(oLiteral) Then
            oLiteral.Text = Resource.getValue("MaxCharsInfo")
        End If

        Dim oGeneric As HtmlGenericControl = FindControl("SPNmaxChar" & item.Field.Type.ToString.ToLower)
        If Not IsNothing(oGeneric) AndAlso item.Field.MaxLength > 0 AndAlso (item.Field.Type <> FieldType.CheckboxList AndAlso item.Field.Type <> FieldType.RadioButtonList AndAlso item.Field.Type <> FieldType.DropDownList AndAlso item.Field.Type <> FieldType.Mail) Then
            oGeneric.Visible = True
        End If

        oGeneric = FindControl("DV" & item.Field.Type.ToString.ToLower)
        Dim oLabelDescription As Label = FindControl("LB" & item.Field.Type.ToString.ToLower & "Description")
        Dim oLabelText As Label = FindControl("LB" & item.Field.Type.ToString.ToLower & "Text")

        Dim view As System.Web.UI.WebControls.View = FindControl("VIW" & item.Field.Type.ToString.ToLower)
        Dim oValidator As RequiredFieldValidator
        Select Case item.Field.Type
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.CheckboxList
                Dim values As String = ""
                If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) Then
                    values = item.Value.Text
                ElseIf item.IdValueField = 0 AndAlso Not IsNothing(item.Field.Options.Where(Function(o) o.IsDefault).FirstOrDefault()) Then
                    values = item.Field.Options.Where(Function(o) o.IsDefault).FirstOrDefault().Id
                End If

                Me.RPTcheckboxlist.DataSource = GetDisplayOptions(item.Field.Options, Values, False)
                Me.RPTcheckboxlist.DataBind()

                Dim opt As dtoFieldOption = item.Field.Options.Where(Function(o) o.IsFreeValue).FirstOrDefault()
                If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) Then
                    If Not IsNothing(opt) Then
                        Me.TXBcheckboxlist.Text = item.Value.FreeText
                    End If
                End If

                Me.MinOptions = item.Field.MinOption
                Me.MaxOptions = item.Field.MaxOption

                Me.SPNminMaxcheckboxlist.Visible = (item.Field.MinOption > 0 OrElse item.Field.MaxOption > 0)

                If item.Field.MinOption > 0 AndAlso Not SPNcheckboxlist.Attributes("class").Contains("min-") Then
                    SPNcheckboxlist.Attributes("class") &= " min-" & item.Field.MinOption.ToString
                    Me.Resource.setLiteral(LTminOptionscheckboxlist)
                    LBminOptioncheckboxlist.Text = item.Field.MinOption.ToString
                Else
                    LTminOptionscheckboxlist.Visible = False
                    LBminOptioncheckboxlist.Visible = False
                End If
                If item.Field.MaxOption > 0 AndAlso Not SPNcheckboxlist.Attributes("class").Contains("max-") Then
                    SPNcheckboxlist.Attributes("class") &= " max-" & item.Field.MaxOption.ToString
                    Me.Resource.setLiteral(LTmaxOptionscheckboxlist)
                    LBmaxOptioncheckboxlist.Text = item.Field.MaxOption.ToString
                Else
                    LTmaxOptionscheckboxlist.Visible = False
                    LBmaxOptioncheckboxlist.Visible = False
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.DropDownList
                DDLitems.DataSource = item.Field.Options
                DDLitems.DataTextField = "Name"
                DDLitems.DataValueField = "Id"
                DDLitems.DataBind()
                If Not String.IsNullOrEmpty(item.Value.Text) Then
                    Try
                        DDLitems.SelectedValue = item.Value.Text
                    Catch ex As Exception

                    End Try
                End If

                MinOptions = item.Field.MinOption
                MaxOptions = item.Field.MaxOption
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.RadioButtonList
                RBLitems.DataSource = item.Field.Options
                RBLitems.DataTextField = "Name"
                RBLitems.DataValueField = "Id"
                RBLitems.DataBind()

                Dim opt As dtoFieldOption = item.Field.Options.Where(Function(o) o.IsFreeValue).FirstOrDefault()
                If Not IsNothing(opt) Then
                    SPNtextOptionRadioButtonList.Visible = True
                    Dim oItem As ListItem = RBLitems.Items.FindByValue(opt.Id)
                    If Not IsNothing(oItem) Then
                        oItem.Attributes.Add("class", "extraoption")
                    End If
                    TXBradiobuttonlist.Text = item.Value.FreeText
                End If

                If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) Then
                    Try
                        RBLitems.SelectedValue = item.Value.Text
                    Catch ex As Exception

                    End Try
                End If
                MinOptions = item.Field.MinOption
                MaxOptions = item.Field.MaxOption

            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer

                Dim dView As System.Web.UI.WebControls.View = FindControl("VIW" & item.Field.DisclaimerType.ToString)
                If Not IsNothing(dView) Then
                    MLVdisclaimer.SetActiveView(dView)
                End If
                Select Case item.Field.DisclaimerType
                    Case DisclaimerType.Standard
                        RBDisclaimerAccept.Checked = Not String.IsNullOrEmpty(item.Value.Text) AndAlso item.Value.Text.ToLower = "true"
                        RBDisclaimerRefuse.Checked = Not RBDisclaimerAccept.Checked
                    Case DisclaimerType.CustomSingleOption
                        RPTsingleOption.DataSource = GetDisplayOptions(item.Field.Options, item.Value.Text, True)
                        RPTsingleOption.DataBind()
                    Case DisclaimerType.CustomMultiOptions
                        RPTmultiOption.DataSource = GetDisplayOptions(item.Field.Options, item.Value.Text, False)
                        RPTmultiOption.DataBind()

                        SPNminMaxCustomMultiOptions.Visible = (item.Field.MinOption > 0 OrElse item.Field.MaxOption > 0)

                        If item.Field.MinOption > 0 AndAlso Not SPNdisclaimerCheckboxlist.Attributes("class").Contains("min-") Then
                            SPNdisclaimerCheckboxlist.Attributes("class") &= " min-" & item.Field.MinOption.ToString
                            Resource.setLiteral(LTminOptionsCustomMultiOptions)
                            LBminOptionCustomMultiOptions.Text = item.Field.MinOption.ToString
                        Else
                            LTminOptionsCustomMultiOptions.Visible = False
                            LBminOptionCustomMultiOptions.Visible = False
                        End If
                        If item.Field.MaxOption > 0 AndAlso Not SPNdisclaimerCheckboxlist.Attributes("class").Contains("max-") Then
                            SPNdisclaimerCheckboxlist.Attributes("class") &= " max-" & item.Field.MaxOption.ToString
                            Resource.setLiteral(LTmaxOptionsCustomMultiOptions)
                            LBmaxOptionCustomMultiOptions.Text = item.Field.MaxOption.ToString
                        Else
                            LTmaxOptionsCustomMultiOptions.Visible = False
                            LBmaxOptionCustomMultiOptions.Visible = False
                        End If
                End Select

            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.FileInput
                Dim toUpload As Boolean = IsNothing(item.Value.Link)
                CTRLdisplayItem.Visible = Not toUpload
                If Not toUpload Then
                    Dim initializer As New lm.Comol.Core.ModuleLinks.dtoObjectRenderInitializer
                    initializer.RefreshContainerPage = False
                    initializer.SaveObjectStatistics = True
                    initializer.Link = item.Value.Link
                    initializer.SetOnModalPageByItem = True
                    initializer.SetPreviousPage = False
                    Dim actions As List(Of dtoModuleActionControl)
                    If Disabled Then
                        CTRLdisplayItem.InitializeControl(initializer, lm.Comol.Core.ModuleLinks.DisplayActionMode.text)
                    Else
                        CTRLdisplayItem.InitializeControl(initializer, lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction)
                    End If


                    'Dim initializer As New lm.Comol.Core.ModuleLinks.dtoModuleDisplayActionInitializer

                    '' DIMENSIONI IMMAGINI
                    'initializer.IconSize = Helpers.IconSize.Small
                    'CTRLdisplayFile.EnableAnchor = True
                    'If Disabled Then
                    '    initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.text
                    'Else
                    '    initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction Or lm.Comol.Core.ModuleLinks.DisplayActionMode.actions
                    'End If

                    'initializer.Link = item.Value.Link
                    'CTRLdisplayFile.InsideOtherModule = True
                    'Dim actions As List(Of dtoModuleActionControl)
                    'actions = CTRLdisplayFile.InitializeRemoteControl(initializer, initializer.Display)
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Date
                LBdateValue.CssClass = "readonlyinput"
                If Not String.IsNullOrEmpty(item.Value.Text) AndAlso IsDate(item.Value) Then
                    LBdateValue.Text = FormatDateTime(CDate(item.Value.Text), vbShortDate)
                Else
                    LBdateValue.Text = ""
                    LBdateValue.CssClass &= " empty"
                End If

            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.DateTime
                LBdatetimeValueData.CssClass = "readonlyinput"
                LBdatetimeValueHour.CssClass = "readonlyinput"
                LBdatetimeValueMinutes.CssClass = "readonlyinput"

                If Not String.IsNullOrEmpty(item.Value.Text) AndAlso IsDate(item.Value.Text) Then
                    LBdatetimeValueData.Text = FormatDateTime(CDate(item.Value.Text), vbShortDate)
                    LBdatetimeValueHour.Text = CDate(item.Value.Text).Hour
                    LBdatetimeValueMinutes.Text = CDate(item.Value.Text).Minute
                Else
                    LBdatetimeValueData.Text = ""
                    LBdatetimeValueHour.Text = ""
                    LBdatetimeValueMinutes.Text = ""
                    LBdatetimeValueData.CssClass &= " empty"
                    LBdatetimeValueHour.CssClass &= " empty"
                    LBdatetimeValueMinutes.CssClass &= " empty"
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Time
                LBtimeValueHour.CssClass = "readonlyinput"
                LBtimeValueMinutes.CssClass = "readonlyinput"
                If Not String.IsNullOrEmpty(item.Value.Text) AndAlso IsDate(item.Value.Text) Then
                    LBtimeValueHour.Text = CDate(item.Value.Text).Hour
                    LBtimeValueMinutes.Text = CDate(item.Value.Text).Minute
                Else
                    LBtimeValueHour.CssClass &= " empty"
                    LBtimeValueMinutes.CssClass &= " empty"
                End If
            Case FieldType.MultiLine
                LBmultilineValue.CssClass = "readonlytextarea"
                LBmultilineValue.Text = item.Value.Text
                If item.Field.MaxLength > 0 Then
                    'LBmultilineValue.Attributes.Add("maxlength", item.Field.MaxLength)
                    LTmultilineTotal.Text = item.Field.MaxLength
                    Dim used As Integer = item.Field.MaxLength
                    If Not IsNothing(item.Value) Then
                        used = item.Field.MaxLength - Len(item.Value.Text)
                    End If
                    LTmultilineUsed.Text = IIf(used < 0, 0, used)
                End If
                If IsNothing(item.Value) OrElse String.IsNullOrEmpty(item.Value.Text) Then
                    LBmultilineValue.CssClass &= " empty"
                End If
            Case FieldType.CompanyCode, FieldType.CompanyTaxCode, FieldType.SingleLine, FieldType.Name, FieldType.Surname, _
                FieldType.TaxCode, FieldType.TelephoneNumber, FieldType.VatCode, FieldType.ZipCode, FieldType.Mail
                    If item.Field.MaxLength > 0 Then
                        Dim used As Integer = item.Field.MaxLength
                        If Not IsNothing(item.Value) Then
                            used -= Len(item.Value.Text)
                        End If
                        LTsinglelineUsed.Text = IIf(used < 0, 0, used)
                        LTsinglelineTotal.Text = item.Field.MaxLength
                    End If
                    LBsinglelineValue.Text = item.Value.Text
                    view = VIWsingleline
                    LBsinglelineValue.CssClass = "readonlyinput"
                    If IsNothing(item.Value) OrElse String.IsNullOrEmpty(item.Value.Text) Then
                        LBsinglelineValue.CssClass &= " empty"
                    End If
                    oGeneric = DVsingleline
                    oLabelDescription = LBsinglelineDescription
                    oLabelText = LBsinglelineText
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Note
                    Exit Select
            Case Else

        End Select

        If item.Field.Type <> lm.Comol.Modules.CallForPapers.Domain.FieldType.Note AndAlso Not IsNothing(oLabelText) Then
            oLabelText.Text = IIf(item.Field.Mandatory, "(*)", "") & item.Field.Name
        End If
        If item.Field.Type = lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer Then
            LTdisclaimerDescription.Text = item.Field.Description
        ElseIf Not IsNothing(oLabelDescription) Then
            oLabelDescription.Text = item.Field.Description
        End If
        If Not IsNothing(oGeneric) Then
            'If Not IsNothing(oTextBox) Then
            Dim cssClass = oGeneric.Attributes("class")
            If (item.FieldError = FieldError.None) Then
                cssClass = Replace(cssClass, " error", "")
            Else
                If Not cssClass.Contains(" error") Then
                    cssClass &= " error"
                End If
            End If
            oGeneric.Attributes("class") = cssClass
        End If
        SetupForRevisions(item.Field.Type, item.RevisionsCount)
        If Not IsNothing(view) Then
            MLVfield.SetActiveView(view)
        End If
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        MLVfield.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        MLVfield.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplayEmptyField() Implements IViewRenderField.DisplayEmptyField
        MLVfield.SetActiveView(VIWempty)
    End Sub

    Private Sub SetupForRevisions(type As FieldType, count As Long)
        Select Case type
            Case FieldType.CheckboxList, FieldType.DropDownList, _
                FieldType.RadioButtonList, FieldType.Disclaimer, _
                FieldType.FileInput, FieldType.Date, FieldType.DateTime, FieldType.Time, FieldType.MultiLine

                Dim oGeneric As HtmlGenericControl = FindControl("SPN" & FieldType.ToString & "RevisionField")
                If Not IsNothing(oGeneric) Then
                    oGeneric.Visible = (count > 0 OrElse AllowRevisionCheck OrElse ShowFieldChecked)
                    Dim oChecBox As CheckBox = FindControl("CBX" & FieldType.ToString & "RevisionField")
                    Dim oLabel As Label = FindControl("LB" & FieldType.ToString & "RevisionField")
                    oLabel.Visible = Not AllowRevisionCheck AndAlso (count > 0)
                    oChecBox.Visible = AllowRevisionCheck OrElse ShowFieldChecked
                    oChecBox.Enabled = Not ShowFieldChecked

                    oLabel.Text = Resource.getValue("RevisionField.Label")
                    oChecBox.Text = Resource.getValue("RevisionField.Check")

                    If ShowFieldChecked Then
                        oChecBox.Checked = True
                    Else
                        oGeneric.Attributes.Add("class", LTrevisionedCssClass.Text)
                    End If
                End If

            Case FieldType.CompanyCode, FieldType.CompanyTaxCode, FieldType.SingleLine, FieldType.Name, FieldType.Surname, _
                FieldType.TaxCode, FieldType.TelephoneNumber, FieldType.VatCode, FieldType.ZipCode, FieldType.Mail
                SPNsinglelineRevisionField.Visible = (count > 0 OrElse AllowRevisionCheck OrElse ShowFieldChecked)
                LBsinglelineRevisionField.Visible = Not AllowRevisionCheck AndAlso (count > 0)
                CBXsinglelineRevisionField.Visible = AllowRevisionCheck OrElse ShowFieldChecked
                CBXsinglelineRevisionField.Enabled = Not ShowFieldChecked
                If ShowFieldChecked Then
                    CBXsinglelineRevisionField.Checked = True
                End If
                LBsinglelineRevisionField.Text = Resource.getValue("RevisionField.Label")
                CBXsinglelineRevisionField.Text = Resource.getValue("RevisionField.Check")
        End Select


    End Sub

    Public Sub HideSelection()
        AllowRevisionCheck = False
        SetupForRevisions(FieldType, RevisionCount)
    End Sub
    Private Sub RPTcheckboxlist_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcheckboxlist.ItemDataBound
        Dim item As dtoOptionDisplay = DirectCast(e.Item.DataItem, dtoOptionDisplay)
        Dim oCheck As HtmlInputCheckBox = e.Item.FindControl("CBoption")
        oCheck.Checked = item.Selected
        If item.IsFreeValue Then
            Me.SPNtextOptionCheckBoxList.Visible = True
            oCheck.Attributes("class") = oCheck.Attributes("class") & " extraoption"
        End If

    End Sub
    Private Sub RPTmultiOption_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTmultiOption.ItemDataBound
        Dim item As dtoOptionDisplay = DirectCast(e.Item.DataItem, dtoOptionDisplay)
        Dim oCheck As HtmlInputCheckBox = e.Item.FindControl("CBoption")
        oCheck.Checked = item.Selected
        If item.IsFreeValue Then
            SPNtextOptionCheckBoxList.Visible = True
            oCheck.Attributes("class") = oCheck.Attributes("class") & " extraoption"
        End If

    End Sub

    Private Sub RPTsingleOption_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsingleOption.ItemDataBound
        Dim item As dtoOptionDisplay = DirectCast(e.Item.DataItem, dtoOptionDisplay)
        Dim oRadio As HtmlInputRadioButton = e.Item.FindControl("RBoption")
        oRadio.Checked = item.Selected
        If item.IsFreeValue Then
            SPNtextOptionRadioButtonList.Visible = True
            oRadio.Attributes("class") = oRadio.Attributes("class") & " extraoption"
        End If
    End Sub

    Private Function GetDisplayOptions(options As List(Of dtoFieldOption), value As String, isSingleSelection As Boolean) As List(Of dtoOptionDisplay)
        Dim results As List(Of dtoOptionDisplay) = options.Select(Function(o) New dtoOptionDisplay(o)).ToList
        Dim values As List(Of String) = value.Split("|").ToList
        For Each item As dtoOptionDisplay In results.Where(Function(v) values.Contains(v.Id)).ToList
            item.Selected = True
        Next
        If (isSingleSelection AndAlso results.Where(Function(r) r.Selected).Count > 1) Then
            For Each item As dtoOptionDisplay In results.Where(Function(v) v.Selected).Skip(1).ToList
                item.Selected = False
            Next
        End If
        Return results
    End Function

    Private Class dtoOptionDisplay
        Public Id As String
        Public Name As String
        Public Selected As Boolean
        Public IsFreeValue As Boolean
        Sub New()

        End Sub
        Sub New(opt As dtoFieldOption)
            Id = opt.Id
            Name = opt.Name
            Selected = False
            IsFreeValue = opt.IsFreeValue
        End Sub
    End Class
End Class