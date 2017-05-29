Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Public Class UC_InputField
    Inherits BaseControl
    Implements IViewInputField

#Region "Context"
    Private _Presenter As InputFieldPresenter
    Private ReadOnly Property CurrentPresenter() As InputFieldPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New InputFieldPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property Disabled As Boolean Implements IViewInputField.Disabled
        Get
            Return ViewStateOrDefault("Disabled", False)
        End Get
        Set(value As Boolean)
            ViewState("Disabled") = value
            Me.TXBcompanycode.Enabled = Not value
            Me.TXBcompanytaxcode.Enabled = Not value
            Me.TXBmail.Enabled = Not value
            Me.TXBmultiline.Enabled = Not value
            Me.TXBname.Enabled = Not value
            Me.TXBsingleline.Enabled = Not value
            Me.TXBsurname.Enabled = Not value
            Me.TXBtaxcode.Enabled = Not value
            Me.TXBtelephonenumber.Enabled = Not value
            Me.TXBvatcode.Enabled = Not value
            Me.TXBzipcode.Enabled = Not value
            Me.BTNremoveFile.Enabled = Not value
            Me.DDLitems.Enabled = Not value
            Me.RBLdisclaimer.Enabled = Not value
            Me.RBLitems.Enabled = Not value
            Me.RDPdate.Enabled = Not value
            Me.RDPdatetime.Enabled = Not value
            Me.RDPtime.Enabled = Not value
            Me.RBLsingleOption.Enabled = Not value
            Me.CBLmultiOptions.Enabled = Not value

            For Each row As RepeaterItem In RPTcheckboxlist.Items
                Dim oCheck As HtmlInputCheckBox = row.FindControl("CBoption")
                oCheck.Disabled = value
            Next
            If value Then
                TXBcheckboxlist.Enabled = False
                TXBradiobuttonlist.Enabled = False
                'TXBcheckboxlist.ReadOnly = True
                'TXBradiobuttonlist.ReadOnly = True
            End If
        End Set
    End Property
    Public Property FieldType As FieldType Implements IViewInputField.FieldType
        Get
            Return ViewStateOrDefault("FieldType", FieldType.None)
        End Get
        Set(value As FieldType)
            ViewState("FieldType") = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewInputField.IdCall
        Get
            Return ViewStateOrDefault("IdCall", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdCall") = value
        End Set
    End Property
    Public Property IdField As Long Implements IViewInputField.IdField
        Get
            Return ViewStateOrDefault("IdField", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdField") = value
        End Set
    End Property
    Private Property IdSubmittedField As Long Implements IViewInputField.IdSubmittedField
        Get
            Return ViewStateOrDefault("IdSubmittedField", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdSubmittedField") = value
        End Set
    End Property
    Private Property IdSubmission As Long Implements IViewInputField.IdSubmission
        Get
            Return ViewStateOrDefault("IdSubmission", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdSubmission") = value
        End Set
    End Property
    Private Property IdLink As Long Implements IViewInputField.IdLink
        Get
            Return ViewStateOrDefault("IdLink", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdLink") = value
        End Set
    End Property

    Private Property Mandatory As Boolean Implements IViewInputField.Mandatory
        Get
            Return ViewStateOrDefault("Mandatory", False)
        End Get
        Set(value As Boolean)
            ViewState("Mandatory") = value
        End Set
    End Property

    Public ReadOnly Property isValid As Boolean Implements IViewInputField.isValid
        Get
            Dim isMandatory As Boolean = Me.Mandatory
            Select Case FieldType
                Case lm.Comol.Modules.CallForPapers.Domain.FieldType.CheckboxList
                    Dim selected As Integer = GetSelectedItems(False).Count
                    Return (selected >= MinOptions AndAlso selected <= MaxOptions)
                Case lm.Comol.Modules.CallForPapers.Domain.FieldType.RadioButtonList
                    Dim selected As Integer = (From i As ListItem In Me.RBLitems.Items Where i.Selected).Count
                    Return (selected >= MinOptions AndAlso selected <= MaxOptions) AndAlso (Not isMandatory OrElse (isMandatory AndAlso Me.RBLitems.SelectedIndex <> -1))

                Case lm.Comol.Modules.CallForPapers.Domain.FieldType.CompanyCode, lm.Comol.Modules.CallForPapers.Domain.FieldType.CompanyTaxCode _
                    , lm.Comol.Modules.CallForPapers.Domain.FieldType.MultiLine, lm.Comol.Modules.CallForPapers.Domain.FieldType.SingleLine _
                    , lm.Comol.Modules.CallForPapers.Domain.FieldType.Surname, lm.Comol.Modules.CallForPapers.Domain.FieldType.Name _
                    , FieldType.TaxCode, FieldType.VatCode, FieldType.ZipCode
                    Dim oTextBox As TextBox = Me.FindControl("TXB" & FieldType.ToString.ToLower)

                    Return (Not isMandatory OrElse (isMandatory AndAlso Not String.IsNullOrEmpty(oTextBox.Text)))
                Case FieldType.TelephoneNumber
                    Return Not isMandatory OrElse (isMandatory AndAlso Not String.IsNullOrEmpty(Me.TXBtelephonenumber.Text))
                Case FieldType.Time
                    Return Not isMandatory OrElse (isMandatory AndAlso Me.RDPtime.SelectedDate.HasValue)
                Case FieldType.Date
                    Return Not isMandatory OrElse (isMandatory AndAlso Me.RDPdate.SelectedDate.HasValue)
                Case FieldType.DateTime
                    Return Not isMandatory OrElse (isMandatory AndAlso Me.RDPdatetime.SelectedDate.HasValue)
                Case FieldType.Disclaimer
                    Select Case DisclaimerType
                        Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.None, lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomDisplayOnly
                            Return True
                        Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.Standard
                            Return (Me.RBLdisclaimer.SelectedIndex = 0)
                        Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomSingleOption
                            Dim selected As Integer = (From i As ListItem In Me.RBLsingleOption.Items Where i.Selected).Count
                            Return (selected >= MinOptions AndAlso selected <= MaxOptions) AndAlso (Not isMandatory OrElse (isMandatory AndAlso Me.RBLitems.SelectedIndex <> -1))
                        Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomMultiOptions
                            Dim selected As Integer = (From i As ListItem In Me.CBLmultiOptions.Items Where i.Selected).Count
                            Return (selected >= MinOptions AndAlso selected <= MaxOptions)
                    End Select

                Case FieldType.DropDownList
                    Return Me.DDLitems.SelectedIndex <> -1
                Case lm.Comol.Modules.CallForPapers.Domain.FieldType.FileInput
                    Return Not isMandatory OrElse (isMandatory AndAlso Me.IdLink >= 0)
                Case FieldType.Mail
                    Dim mail As String = TXBmail.Text
                    If Not String.IsNullOrEmpty(mail) Then
                        mail = mail.Trim
                        TXBmail.Text = mail
                    End If
                    Return (Not isMandatory AndAlso String.IsNullOrEmpty(mail)) OrElse (lm.Comol.Core.Authentication.Helpers.ValidationHelpers.Mail(mail, REVmail.ValidationExpression) AndAlso (Not isMandatory OrElse (isMandatory AndAlso Not String.IsNullOrEmpty(mail))))
                Case FieldType.Note
                    Return True
            End Select
        End Get
    End Property

    Private Property MaxChars As Integer Implements IViewInputField.MaxChars
        Get
            Return ViewStateOrDefault("MaxChars", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("MaxChars") = value
        End Set
    End Property
    Private Property MaxOptions As Integer Implements IViewInputField.MaxOptions
        Get
            Return ViewStateOrDefault("MaxOptions", CInt(1))
        End Get
        Set(value As Integer)
            ViewState("MaxOptions") = value
        End Set
    End Property
    Private Property MinOptions As Integer Implements IViewInputField.MinOptions
        Get
            Return ViewStateOrDefault("MinOptions", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("MinOptions") = value
        End Set
    End Property
    Private Property Options As List(Of dtoFieldOption) Implements IViewInputField.Options
        Get
            Return ViewStateOrDefault("Options", New List(Of dtoFieldOption))
        End Get
        Set(value As List(Of dtoFieldOption))
            ViewState("Options") = value
        End Set
    End Property
    Public Property CurrentError As FieldError Implements IViewInputField.CurrentError
        Get
            Return ViewStateOrDefault("CurrentError", FieldError.None)
        End Get
        Set(value As FieldError)
            ViewState("CurrentError") = value
        End Set
    End Property
    Private Property DisclaimerType As DisclaimerType Implements IViewInputField.DisclaimerType
        Get
            Return ViewStateOrDefault("DisclaimerType", DisclaimerType.None)
        End Get
        Set(value As DisclaimerType)
            ViewState("DisclaimerType") = value
        End Set
    End Property
    Private Property IdCallCommunity As Integer Implements IViewInputField.IdCallCommunity
        Get
            Return ViewStateOrDefault("IdCallCommunity", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdCallCommunity") = value
        End Set
    End Property
    Public Property ReviewMode As Boolean Implements IViewInputField.ReviewMode
        Get
            Return ViewStateOrDefault("ReviewMode", False)
        End Get
        Set(value As Boolean)
            ViewState("ReviewMode") = value
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
#Region "internal"
    Private _PostBackTriggers As String
    Public Property PostBackTriggers As String
        Get
            Return _PostBackTriggers
        End Get
        Set(value As String)
            _PostBackTriggers = value
        End Set
    End Property
    Public Event RemoveFile(ByVal idSubmittedField As Long)
    Protected ReadOnly Property CssError As String
        Get
            Return IIf(CurrentError = FieldError.None, "", "error")
        End Get
    End Property
#End Region
  
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCall", "Modules", "CallForPapers")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setButton(BTNremoveFile, True, , , True)
            .setRadioButtonList(RBLdisclaimer, "True")
            .setRadioButtonList(RBLdisclaimer, "")
        End With
    End Sub
#End Region
    Public Sub InitializeControl(idCall As Long, idSubmission As Long, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier, field As dtoSubmissionValueField, disabled As Boolean, isPublic As Boolean) Implements IViewInputField.InitializeControl
        CurrentError = FieldError.None
        Me.CurrentPresenter.InitView(idCall, idSubmission, identifier, field, disabled, isPublic)
    End Sub
    Public Sub InitializeControl(idCall As Long, idSubmission As Long, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier, item As dtoSubmissionValueField, disabled As Boolean, isPublic As Boolean, err As FieldError) Implements IViewInputField.InitializeControl
        CurrentError = err
        Me.CurrentPresenter.InitView(idCall, idSubmission, identifier, item, disabled, isPublic)

        If Not IsNothing(item) AndAlso Not IsNothing(item.Field) Then
            Dim oLabel As Label = Me.FindControl("LBerrorMessage" & item.Field.Type.ToString.ToLower)

            If Not IsNothing(oLabel) Then
                oLabel.Visible = Not (err = FieldError.None)

                If Not (err = FieldError.None) Then
                    oLabel.Text = Resource.getValue("FieldError." & err.ToString())
                End If
            End If
        End If
    End Sub
    Private Sub SetupView(item As dtoSubmissionValueField, idUploader As Integer, identifier As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier, isPublic As Boolean) Implements IViewInputField.SetupView

        IdSubmittedField = item.IdValueField
        IdField = item.IdField
        FieldType = item.Field.Type

        Dim oLabel As Label = Nothing
        Dim oGeneric As HtmlGenericControl = Nothing
        If ReviewMode Then
            oGeneric = FindControl("SPN" & FieldType.ToString & "RevisionField")
            If Not IsNothing(oGeneric) Then
                oGeneric.Visible = True
                oLabel = FindControl("LB" & FieldType.ToString & "RevisionField")
                oLabel.Text = Resource.getValue("RevisionField.LabelInfo")
            End If
        End If



        If item.Field.Type <> lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer Then
            oLabel = Me.FindControl("LB" & item.Field.Type.ToString.ToLower & "Description")
        Else
            oLabel = Me.FindControl("LB" & item.Field.Type.ToString.ToLower & item.Field.DisclaimerType.ToString & "Description")
        End If
        If Not IsNothing(oLabel) Then
            oLabel.Text = item.Field.Description
        End If

        If item.Field.Type <> lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer AndAlso item.Field.Type <> lm.Comol.Modules.CallForPapers.Domain.FieldType.Note Then
            oLabel = Me.FindControl("LB" & item.Field.Type.ToString.ToLower & "Help")
            oLabel.Visible = Not String.IsNullOrEmpty(item.Field.ToolTip)
            oLabel.Text = item.Field.ToolTip
        End If

        If item.Field.Type <> lm.Comol.Modules.CallForPapers.Domain.FieldType.Note Then
            If item.Field.Type <> lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer Then
                oLabel = Me.FindControl("LB" & item.Field.Type.ToString.ToLower & "Text")
            Else
                oLabel = Me.FindControl("LB" & item.Field.Type.ToString.ToLower & DisclaimerType.ToString & "Text")
            End If

            oLabel.Text = IIf(item.Field.Mandatory, "(*)", "") & item.Field.Name
        End If

        Dim oLiteral As Literal = Me.FindControl("LTmaxChars" & item.Field.Type.ToString.ToLower)
        If Not IsNothing(oLiteral) Then
            oLiteral.Text = Me.Resource.getValue("MaxCharsInfo")
        End If

        oGeneric = Me.FindControl("SPNmaxChar" & item.Field.Type.ToString.ToLower)
        If Not IsNothing(oGeneric) AndAlso item.Field.MaxLength > 0 AndAlso (item.Field.Type <> FieldType.CheckboxList AndAlso item.Field.Type <> FieldType.RadioButtonList AndAlso item.Field.Type <> FieldType.DropDownList AndAlso item.Field.Type <> FieldType.Mail) Then
            oGeneric.Visible = True
        End If

        If item.Field.Type <> lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer Then
            oGeneric = Me.FindControl("DV" & item.Field.Type.ToString.ToLower)
        Else
            oGeneric = Me.FindControl("DV" & item.Field.Type.ToString.ToLower & item.Field.DisclaimerType.ToString)
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

        Dim oValidator As RequiredFieldValidator
        Select Case item.Field.Type
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.CheckboxList
                Dim values As String = ""
                If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) Then
                    values = item.Value.Text
                ElseIf item.IdValueField = 0 AndAlso Not IsNothing(item.Field.Options.Where(Function(o) o.IsDefault).FirstOrDefault()) Then
                    values = item.Field.Options.Where(Function(o) o.IsDefault).FirstOrDefault().Id
                End If
                Me.RPTcheckboxlist.DataSource = GetDisplayOptions(item.Field.Options, values, False)
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
                Me.DDLitems.DataSource = item.Field.Options
                Me.DDLitems.DataTextField = "Name"
                Me.DDLitems.DataValueField = "Id"
                Me.DDLitems.DataBind()
                If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) Then
                    Try
                        Me.DDLitems.SelectedValue = item.Value.Text
                    Catch ex As Exception

                    End Try
                ElseIf item.IdValueField = 0 AndAlso Not IsNothing(item.Field.Options.Where(Function(o) o.IsDefault).FirstOrDefault()) Then
                    Try
                        Me.DDLitems.SelectedValue = item.Field.Options.Where(Function(o) o.IsDefault).FirstOrDefault().Id
                    Catch ex As Exception

                    End Try
                End If

                Me.MinOptions = item.Field.MinOption
                Me.MaxOptions = item.Field.MaxOption
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.RadioButtonList
                Me.RBLitems.DataSource = item.Field.Options
                Me.RBLitems.DataTextField = "Name"
                Me.RBLitems.DataValueField = "Id"
                Me.RBLitems.DataBind()

                Dim opt As dtoFieldOption = item.Field.Options.Where(Function(o) o.IsFreeValue).FirstOrDefault()
                If Not IsNothing(opt) Then
                    Me.SPNtextOptionRadioButtonList.Visible = True
                    Dim oItem As ListItem = Me.RBLitems.Items.FindByValue(opt.Id)
                    If Not IsNothing(oItem) Then
                        oItem.Attributes.Add("class", "extraoption")
                    End If
                    Me.TXBradiobuttonlist.Text = item.Value.FreeText
                End If

                If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) Then
                    Try
                        Me.RBLitems.SelectedValue = item.Value.Text
                    Catch ex As Exception

                    End Try
                ElseIf item.IdValueField = 0 AndAlso Not IsNothing(item.Field.Options.Where(Function(o) o.IsDefault).FirstOrDefault()) Then
                    Try
                        RBLitems.SelectedIndex = -1
                        Dim oItem As ListItem = Me.RBLitems.Items.FindByValue(item.Field.Options.Where(Function(o) o.IsDefault).FirstOrDefault().Id)
                        If Not IsNothing(oItem) Then
                            oItem.Selected = True
                        End If
                    Catch ex As Exception

                    End Try
                End If


                Me.MinOptions = item.Field.MinOption
                Me.MaxOptions = item.Field.MaxOption
                '               Me.SPNminMaxradioButtonlist.Visible = False
                '(item.Field.MinOption > 0 OrElse item.Field.MaxOption > 0)
                '               If item.Field.MinOption > 0 AndAlso Not Me.RBLitems.CssClass.Contains("min-") Then
                '                   Me.RBLitems.CssClass &= "min-" & item.Field.MinOption.ToString
                '                   Me.Resource.setLiteral(LTminOptionsradioButtonlist)
                '                   LBminOptionradioButtonlist.Text = item.Field.MinOption.ToString
                '               End If
                '               If item.Field.MaxOption > 0 AndAlso Not Me.RBLitems.CssClass.Contains("max-") Then
                '                   Me.RBLitems.CssClass &= "max-" & item.Field.MaxOption.ToString
                '                   Me.Resource.setLiteral(LTmaxOptionsradioButtonlist)
                '                   LBmaxOptionradioButtonlist.Text = item.Field.MaxOption.ToString
                '               End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer

                Select Case item.Field.DisclaimerType
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomDisplayOnly
                        Exit Select
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.Standard
                        If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) AndAlso item.Value.Text.ToLower = "true" Then
                            Me.RBLdisclaimer.SelectedIndex = 0
                        Else
                            Me.RBLdisclaimer.SelectedIndex = 1
                        End If
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomMultiOptions
                        Me.CBLmultiOptions.DataSource = item.Field.Options
                        Me.CBLmultiOptions.DataTextField = "Name"
                        Me.CBLmultiOptions.DataValueField = "Id"
                        Me.CBLmultiOptions.DataBind()
                        If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) Then
                            Dim values As List(Of String) = item.Value.Text.Split("|").ToList
                            For Each value As String In values.Where(Function(v) Not String.IsNullOrEmpty(v)).ToList
                                Dim oItem As ListItem = Me.CBLmultiOptions.Items.FindByValue(value)
                                If Not IsNothing(oItem) Then
                                    oItem.Selected = True
                                End If
                            Next
                        ElseIf item.IdValueField = 0 AndAlso item.Field.Options.Where(Function(o) o.IsDefault).Any Then
                            Me.CBLmultiOptions.SelectedValue = item.Field.Options.Where(Function(o) o.IsDefault).FirstOrDefault.Id
                        End If

                        Me.MinOptions = item.Field.MinOption
                        Me.MaxOptions = item.Field.MaxOption

                        Me.SPNminMaxCustomMultiOptions.Visible = (item.Field.MinOption > 0 OrElse item.Field.MaxOption > 0)

                        If item.Field.MinOption > 0 AndAlso Not CBLmultiOptions.CssClass.Contains("min-") Then
                            CBLmultiOptions.CssClass &= " min-" & item.Field.MinOption.ToString
                            Me.Resource.setLiteral(LTminOptionsCustomMultiOptions)
                            LBminOptionCustomMultiOptions.Text = item.Field.MinOption.ToString
                        Else
                            LTminOptionsCustomMultiOptions.Visible = False
                            LBminOptionCustomMultiOptions.Visible = False
                        End If
                        If item.Field.MaxOption > 0 AndAlso Not SPNcheckboxlist.Attributes("class").Contains("max-") Then
                            CBLmultiOptions.CssClass &= " max-" & item.Field.MaxOption.ToString
                            Me.Resource.setLiteral(LTmaxOptionsCustomMultiOptions)
                            LBmaxOptionCustomMultiOptions.Text = item.Field.MaxOption.ToString
                        Else
                            LTmaxOptionsCustomMultiOptions.Visible = False
                            LBmaxOptionCustomMultiOptions.Visible = False
                        End If
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomSingleOption
                        Me.RBLsingleOption.DataSource = item.Field.Options
                        Me.RBLsingleOption.DataTextField = "Name"
                        Me.RBLsingleOption.DataValueField = "Id"
                        Me.RBLsingleOption.DataBind()
                        If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) Then
                            Try
                                Me.RBLsingleOption.SelectedValue = item.Value.Text
                            Catch ex As Exception

                            End Try
                        ElseIf item.IdValueField = 0 AndAlso item.Field.Options.Where(Function(o) o.IsDefault).Any Then
                            Me.RBLsingleOption.SelectedValue = item.Field.Options.Where(Function(o) o.IsDefault).FirstOrDefault.Id
                        End If
                        Me.MinOptions = item.Field.MinOption
                        Me.MaxOptions = item.Field.MaxOption
                    Case Else

                End Select
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.FileInput
                Dim toUpload As Boolean = (IsNothing(item.Value) OrElse IsNothing(item.Value.Link))
                'Me.CTRLfileUploader.AjaxEnabled = Not Disabled
                CTRLinternalUploader.Enabled = Not Disabled
                CTRLinternalUploader.Visible = toUpload

                Me.BTNremoveFile.Visible = Not toUpload
                Me.CTRLdisplayItem.Visible = Not toUpload
                SetInternazionalizzazione()
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
                    'initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction Or lm.Comol.Core.ModuleLinks.DisplayActionMode.actions
                    'initializer.Link = item.Value.Link
                    'CTRLdisplayFile.InsideOtherModule = True
                    'Dim actions As List(Of dtoModuleActionControl)
                    'actions = CTRLdisplayFile.InitializeRemoteControl(initializer, StandardActionType.Play)
                    Me.IdLink = item.Value.Link.Id
                    Me.LBfileinputHelp.Visible = False
                Else
                    CTRLinternalUploader.PostbackTriggers = PostBackTriggers
                    CTRLinternalUploader.AllowAnonymousUpload = isPublic
                    CTRLinternalUploader.InitializeControl(idUploader, identifier)
                    Me.IdLink = 0
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Date
                RDPdate.MinDate = New DateTime(1900, 1, 1)
                If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) AndAlso IsDate(item.Value.Text) Then
                    Me.RDPdate.SelectedDate = CDate(item.Value.Text)
                Else
                    Me.RDPdate.SelectedDate = Nothing
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.DateTime
                RDPdatetime.MinDate = New DateTime(1900, 1, 1)
                If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) AndAlso IsDate(item.Value.Text) Then
                    Me.RDPdatetime.SelectedDate = CDate(item.Value.Text)
                Else
                    Me.RDPdatetime.SelectedDate = Nothing
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Time
                If Not IsNothing(item.Value) AndAlso Not String.IsNullOrEmpty(item.Value.Text) AndAlso IsDate(item.Value.Text) Then
                    Me.RDPtime.SelectedDate = CDate(item.Value.Text)
                Else
                    Me.RDPtime.SelectedDate = Nothing
                End If
            Case FieldType.CompanyCode, FieldType.CompanyTaxCode, FieldType.MultiLine, FieldType.SingleLine, FieldType.Name, FieldType.Surname, _
                FieldType.TaxCode, FieldType.TelephoneNumber, FieldType.VatCode, FieldType.ZipCode, FieldType.Mail
                Dim oTextBox As TextBox = Me.FindControl("TXB" & item.Field.Type.ToString.ToLower)
                If item.Field.MaxLength > 0 Then
                    If item.Field.Type = FieldType.MultiLine Then
                        oTextBox.Attributes.Add("maxlength", item.Field.MaxLength)
                    Else
                        oTextBox.MaxLength = item.Field.MaxLength
                    End If
                End If
                If IsNothing(item.Value) Then
                    oTextBox.Text = ""
                Else
                    oTextBox.Text = item.Value.Text
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Note
                Exit Select
            Case Else

        End Select
        Dim name As String = ""
        If DisclaimerType <> lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.None Then
            name = "VIW" & item.Field.Type.ToString.ToLower & DisclaimerType.ToString
        Else
            name = "VIW" & item.Field.Type.ToString.ToLower
        End If
        Dim view As System.Web.UI.WebControls.View = Me.FindControl(name)
        If Not IsNothing(view) Then
            Me.MLVfield.SetActiveView(view)
        End If
    End Sub
    Public Function GetField() As dtoSubmissionValueField Implements IViewInputField.GetField
        Dim dto As New dtoSubmissionValueField
        dto.Field = New dtoCallField

        dto.IdValueField = IdSubmittedField
        dto.Value = New dtoValueField("")
        dto.Value.IdLink = IdLink
        dto.Field.Id = IdField
        dto.Field.Type = FieldType
        dto.Field.DisclaimerType = DisclaimerType
        Dim oLabel As Label = Nothing
        If dto.Field.Type <> lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer Then
            oLabel = Me.FindControl("LB" & dto.Field.Type.ToString.ToLower & "Text")
        Else
            oLabel = Me.FindControl("LB" & dto.Field.Type.ToString.ToLower & dto.Field.DisclaimerType.ToString & "Text")
        End If
        If Not IsNothing(oLabel) Then
            dto.Field.Name = oLabel.Text
        End If


        Select Case FieldType
            Case FieldType.CheckboxList
                Dim items As List(Of String) = GetSelectedItems(False)
                If (items.Count = 0) Then
                    dto.Value.Text = ""
                ElseIf (items.Count = 1) Then
                    dto.Value.Text = items(0)
                Else
                    dto.Value.Text = String.Join("|", items.ToArray)
                End If
                Dim opt As dtoFieldOption = Options.Where(Function(o) o.IsFreeValue).FirstOrDefault()
                If Not IsNothing(opt) AndAlso items.Contains(opt.Id.ToString) Then
                    dto.Value.FreeText = Me.TXBcheckboxlist.Text
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.DropDownList
                Try
                    dto.Value.Text = Me.DDLitems.SelectedValue
                Catch ex As Exception

                End Try
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.RadioButtonList
                If Me.RBLitems.SelectedIndex <> -1 Then
                    dto.Value.Text = Me.RBLitems.SelectedValue
                    Dim opt As dtoFieldOption = Options.Where(Function(o) o.IsFreeValue).FirstOrDefault()
                    If Not IsNothing(opt) AndAlso Me.RBLitems.SelectedValue = opt.Id.ToString Then
                        dto.Value.FreeText = Me.TXBradiobuttonlist.Text
                    End If
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Disclaimer
                Select Case dto.Field.DisclaimerType
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomDisplayOnly
                        dto.Value.Text = "True"
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.Standard
                        dto.Value.Text = (Me.RBLdisclaimer.SelectedIndex = 0)
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomMultiOptions
                        Dim items As List(Of String) = (From i As ListItem In Me.CBLmultiOptions.Items Where i.Selected Select i.Value).ToList
                        If (items.Count = 0) Then
                            dto.Value = New dtoValueField("")
                        ElseIf (items.Count = 1) Then
                            dto.Value = New dtoValueField(items(0))
                        Else
                            dto.Value = New dtoValueField(String.Join("|", items.ToArray))
                        End If
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomSingleOption
                        dto.Value = New dtoValueField(Me.RBLsingleOption.SelectedValue)
                    Case Else
                        dto.Value = New dtoValueField("")
                End Select

            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.FileInput
                Exit Select
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Date
                If Me.RDPdate.SelectedDate.HasValue Then
                    dto.Value = New dtoValueField(Me.RDPdate.SelectedDate.Value)
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.DateTime
                If Me.RDPdatetime.SelectedDate.HasValue Then
                    dto.Value = New dtoValueField(Me.RDPdatetime.SelectedDate.Value)
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Time
                If Me.RDPtime.SelectedDate.HasValue Then
                    dto.Value = New dtoValueField(Me.RDPtime.SelectedDate.Value)
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.Note
                Exit Select
            Case FieldType.CompanyCode, FieldType.CompanyTaxCode, FieldType.MultiLine, FieldType.SingleLine, FieldType.Name, FieldType.Surname, _
                FieldType.TaxCode, FieldType.TelephoneNumber, FieldType.VatCode, FieldType.ZipCode, lm.Comol.Modules.CallForPapers.Domain.FieldType.Mail
                Dim oTextBox As TextBox = Me.FindControl("TXB" & FieldType.ToString.ToLower)
                dto.Value.Text = oTextBox.Text

            Case Else
                dto.Value.Text = ""
        End Select
        dto.FieldError = GetFieldError()

        Return (dto)
    End Function

    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        Me.MLVfield.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        Me.MLVfield.SetActiveView(VIWempty)
    End Sub
    Private Sub DisplayEmptyField() Implements IViewInputField.DisplayEmptyField
        Me.MLVfield.SetActiveView(VIWempty)
    End Sub

    Public Sub DisplayInputError() Implements IViewInputField.DisplayInputError

    End Sub
    Private Sub HideInputError() Implements IViewInputField.HideInputError

    End Sub

    Private Sub RefreshFileField(link As lm.Comol.Core.DomainModel.ModuleLink) Implements IViewInputField.RefreshFileField
        Dim uploadFile As Boolean = IsNothing(link)
        '  CTRLinternalUploader.AjaxEnabled = uploadFile
        CTRLinternalUploader.Visible = uploadFile
        Me.BTNremoveFile.Visible = Not uploadFile
        Me.CTRLdisplayItem.Visible = Not uploadFile

        If Not uploadFile Then
            Dim initializer As New lm.Comol.Core.ModuleLinks.dtoObjectRenderInitializer
            initializer.RefreshContainerPage = False
            initializer.SaveObjectStatistics = True
            initializer.Link = New liteModuleLink()
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
            'initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction Or lm.Comol.Core.ModuleLinks.DisplayActionMode.actions
            'initializer.Link = link
            'CTRLdisplayFile.InsideOtherModule = True
            'Dim actions As List(Of dtoModuleActionControl)
            'actions = CTRLdisplayFile.InitializeRemoteControl(initializer, StandardActionType.Play)
            Me.IdLink = link.Id
        Else
            Me.IdLink = 0
        End If
    End Sub


    Public Function AddInternalFile(submission As UserSubmission, moduleCode As String, idModule As Integer, moduleAction As Integer, objectType As Integer) As dtoSubmissionFileValueField 'Implements IViewInputField.AddInternalFile
        If IdLink > 0 Then
            Return Nothing
        Else
            Dim items As List(Of lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem) = CTRLinternalUploader.AddModuleInternalFiles(submission, submission.Id, objectType, moduleCode, moduleAction)

            If IsNothing(items) OrElse Not items.Where(Function(i) i.IsAdded).Any Then
                Return Nothing
            Else
                Dim uploadedFile As New dtoSubmissionFileValueField
                uploadedFile.IdField = IdField
                uploadedFile.ActionLink = items.Where(Function(i) i.IsAdded).Select(Function(i) i.Link).FirstOrDefault()
                Return uploadedFile
            End If

            'Dim uploadedFile As New dtoSubmissionFileValueField
            'uploadedFile.ActionLink = CTRLfileUploader.UploadAndLinkInternalFile(FileRepositoryType.InternalLong, submission, moduleCode, moduleAction, objectType)

            'If uploadedFile.ActionLink Is Nothing Then
            '    uploadedFile = Nothing
            'Else
            '    uploadedFile.IdField = IdField
            'End If
            'Return uploadedFile
        End If
    End Function

    Private Sub CTRLinternalUploader_IsValidOperation(ByRef isvalid As Boolean) Handles CTRLinternalUploader.IsValidOperation
        isvalid = True
    End Sub
#Region "Control"
    Private Sub BTNremoveFile_Click(sender As Object, e As System.EventArgs) Handles BTNremoveFile.Click
        RaiseEvent RemoveFile(IdSubmittedField)
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
    Private Function GetSelectedItems(isSingleSelection As Boolean) As List(Of String)
        Dim results As New List(Of String)
        If isSingleSelection Then
        Else
            For Each row As RepeaterItem In RPTcheckboxlist.Items
                Dim oCheck As HtmlInputCheckBox = row.FindControl("CBoption")
                If oCheck.Checked Then
                    results.Add(oCheck.Value)
                End If
            Next
        End If
        Return results
    End Function
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

    Public Function GetFieldError() As FieldError
        Dim isMandatory As Boolean = Me.Mandatory
        Select Case FieldType
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.CheckboxList
                Dim selected As Integer = GetSelectedItems(False).Count
                Return GetOptionsError(isMandatory, selected, True)
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.RadioButtonList
                Dim selected As Integer = (From i As ListItem In Me.RBLitems.Items Where i.Selected).Count
                Return GetOptionsError(isMandatory, selected, False)
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.CompanyCode, lm.Comol.Modules.CallForPapers.Domain.FieldType.CompanyTaxCode _
                , lm.Comol.Modules.CallForPapers.Domain.FieldType.MultiLine, lm.Comol.Modules.CallForPapers.Domain.FieldType.SingleLine _
                , lm.Comol.Modules.CallForPapers.Domain.FieldType.Surname, lm.Comol.Modules.CallForPapers.Domain.FieldType.Name _
                , FieldType.TaxCode, FieldType.VatCode, FieldType.ZipCode
                Dim oTextBox As TextBox = Me.FindControl("TXB" & FieldType.ToString.ToLower)

                If (isMandatory AndAlso String.IsNullOrEmpty(TrimValue(oTextBox.Text))) Then
                    Return FieldError.Mandatory
                End If
            Case FieldType.TelephoneNumber
                If (isMandatory AndAlso String.IsNullOrEmpty(TrimValue(TXBtelephonenumber.Text))) Then
                    Return FieldError.Mandatory
                End If
            Case FieldType.Time
                If (isMandatory AndAlso Not Me.RDPtime.SelectedDate.HasValue) Then
                    Return FieldError.Mandatory
                End If
            Case FieldType.Date
                If (isMandatory AndAlso Not Me.RDPdate.SelectedDate.HasValue) Then
                    Return FieldError.Mandatory
                End If
            Case FieldType.DateTime
                If (isMandatory AndAlso Not Me.RDPdatetime.SelectedDate.HasValue) Then
                    Return FieldError.Mandatory
                End If
            Case FieldType.Disclaimer
                Select Case DisclaimerType
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.None, lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomDisplayOnly
                        Return FieldError.None
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.Standard
                        If Not (Me.RBLdisclaimer.SelectedIndex = 0) Then
                            Return FieldError.Invalid
                        End If
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomSingleOption
                        Dim selected As Integer = (From i As ListItem In Me.RBLsingleOption.Items Where i.Selected).Count
                        Return GetOptionsError(isMandatory, selected, False)
                    Case lm.Comol.Modules.CallForPapers.Domain.DisclaimerType.CustomMultiOptions
                        Dim selected As Integer = (From i As ListItem In Me.CBLmultiOptions.Items Where i.Selected).Count
                        Return GetOptionsError(isMandatory, selected, True)
                End Select

            Case FieldType.DropDownList
                If isMandatory AndAlso DDLitems.SelectedIndex = -1 Then
                    Return FieldError.Invalid
                End If
            Case lm.Comol.Modules.CallForPapers.Domain.FieldType.FileInput
                If (isMandatory AndAlso Me.IdLink <= 0) Then
                    Return FieldError.Mandatory
                End If
            Case FieldType.Mail
                Dim mail As String = TrimValue(TXBmail.Text)
                If isMandatory AndAlso String.IsNullOrEmpty(mail) Then
                    Return FieldError.Mandatory
                ElseIf Not String.IsNullOrEmpty(mail) AndAlso Not lm.Comol.Core.Authentication.Helpers.ValidationHelpers.Mail(mail, REVmail.ValidationExpression) Then
                    Return FieldError.InvalidFormat
                End If
        End Select
        Return FieldError.None
    End Function

    Private Function TrimValue(value As String) As String
        If Not String.IsNullOrEmpty(value) Then
            value = value.Trim
        End If
        Return value
    End Function

    Private Function GetOptionsError(ByVal isMandatory As Boolean, ByVal selected As Integer, isMultiOption As Boolean) As FieldError
        If (selected >= MinOptions AndAlso selected <= MaxOptions) Then
            If Not isMultiOption AndAlso isMandatory AndAlso selected = 0 Then
                Return FieldError.Mandatory
            Else
                Return FieldError.None
            End If
        ElseIf selected > MaxOptions Then
            Return FieldError.MoreOptions
        ElseIf selected < MinOptions Then
            Return FieldError.LessOptions
        End If
        Return FieldError.None
    End Function
#End Region

   
End Class