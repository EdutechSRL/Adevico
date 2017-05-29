Imports COL_BusinessLogic_v2.Localizzazione

Public Class UC_ListSessionInfo
    Inherits System.Web.UI.UserControl
    Public Event SendAction(ByVal Action As String)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Property ActiveControl As Boolean
        Get
            Dim Active As Boolean = False
            If Not String.IsNullOrEmpty(Me.ViewState("ActiveControl")) Then
                Try
                    Active = System.Convert.ToBoolean(Me.ViewState("ActiveControl"))
                Catch ex As Exception
                End Try
            End If
            Return Active
        End Get
        Set(value As Boolean)
            Me.ViewState("ActiveControl") = value
        End Set
    End Property


    Public Function InitControl( _
                               ByVal oResource As ResourceManager, _
                               ByVal value As IList(Of InfoItemSession), _
                               Optional ByVal useLink As Boolean = False, _
                               Optional ByVal hideZero As Boolean = False)

        If Not value.Any() Then
            RPTitem.Visible = False
            Return False
        End If

        _oResource = oResource
        _hideZero = hideZero
        _useLink = useLink

        Me.RPTitem.DataSource = value
        Me.RPTitem.DataBind()

        Return True
    End Function

    Private _oResource As ResourceManager = Nothing
    Private _hideZero As Boolean = False
    Private _useLink As Boolean = False

    Private Sub RPTitem_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTitem.ItemCommand
        Dim argument As String = e.CommandArgument

        RaiseEvent SendAction(argument)
    End Sub



    Private Sub RPTitem_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTitem.ItemDataBound

        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim iItm As InfoItemSession = e.Item.DataItem


            If IsNothing(iItm) Then
                Return
            End If

            Dim LTstaticItem As Literal = e.Item.FindControl("LTstaticItem")
            Dim LKBsendAction As LinkButton = e.Item.FindControl("LKBsendAction")

            Dim spanMainCssClass As String = Me.LTmainCss.Text

            If IsNothing(LTstaticItem) OrElse IsNothing(LKBsendAction) Then
                Return
            End If

            'InnerText
            Dim InnerText As String = Me.LTcontentTemplate.Text

            If _hideZero AndAlso iItm.CountValue <= 0 Then
                InnerText = InnerText.Replace("{hideCss}", LThideCss.Text)
            Else
                InnerText = InnerText.Replace("{hideCss}", "")
            End If

            Dim title As String = _oResource.getValue("LInfo.title." & iItm.TextValue)

            If _useLink Then
                InnerText = InnerText.Replace("{innerTitle}", "")
            Else
                InnerText = InnerText.Replace("{innerTitle}", "title=""" & title & """")
            End If

            InnerText = InnerText.Replace("{itemCss}", iItm.TextValue)

            'Text
            Dim displayText As String = _oResource.getValue("LInfo.value." & iItm.TextValue)
            If String.IsNullOrEmpty(displayText) Then
                displayText = iItm.TextValue
            End If

            InnerText = InnerText.Replace("{innerText}", displayText)
            InnerText = InnerText.Replace("{innerValue}", iItm.CountValue.ToString())

            If _useLink Then

                LTstaticItem.Visible = False
                LKBsendAction.Visible = True

                LKBsendAction.Text = InnerText
                LKBsendAction.ToolTip = title

                LKBsendAction.CommandName = ""
                LKBsendAction.CommandArgument = iItm.TextValue
            Else
                LKBsendAction.Visible = False
                LTstaticItem.Visible = True

                LTstaticItem.Text = InnerText
            End If

            LKBsendAction.Enabled = (iItm.CountValue > 0 AndAlso iItm.Unselected)

            If Not LKBsendAction.Enabled Then
                LKBsendAction.CssClass &= " disabled"
            End If

            If Not iItm.Unselected Then
                LKBsendAction.CssClass &= " selected"
            End If

        End If

    End Sub

    Public Property CssMain As String
        Get
            Return Me.LTmainCss.Text
        End Get
        Set(value As String)
            Me.LTmainCss.Text = value
        End Set
    End Property

    Public Property CssEmpty As String
        Get
            Return Me.LThideCss.Text
        End Get
        Set(value As String)
            Me.LThideCss.Text = value
        End Set
    End Property

End Class

''' <summary>
''' Item for render List Info Items
''' </summary>
''' <remarks></remarks>
Public Class InfoItemSession

    Public Sub New()
        CountValue = 0
        TextValue = ""
        Unselected = True
    End Sub

    ''' <summary>
    ''' Testo (css, internaz. e commanvalue) + valore
    ''' </summary>
    ''' <param name="Text"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal text As String, ByVal value As Integer, Optional ByVal isEnabled As Boolean = True)
        CountValue = value
        TextValue = text
        Unselected = isEnabled
    End Sub

    ''' <summary>
    ''' Number
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CountValue As Integer = 0

    ''' <summary>
    ''' Usato come:
    ''' - Classe CSS
    ''' - Argmoento evento generato (se LINK)
    ''' - Internazionalizzazione:
    '''     - LInfo.value. + TextValue
    '''     - LInfo.title. + TextValue
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TextValue As String = ""

    ''' <summary>
    ''' SE a true, il link è attivo, altrimenti no.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Unselected As Boolean = True

End Class

'Public Class InfoItem
'    Inherits InfoItemSimple

'    Public Property CommandArguemnt As String = ""
'End Class