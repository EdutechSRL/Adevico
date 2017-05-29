Imports System.Text
Imports PresentationLayer
Imports lm.Comol.Modules.Standard.Faq

Public Class FAQList
    Inherits PageBase
    Implements Adevico.Modules.Faq.Presentation.IVIewFaqList


    Private CatList As List(Of DTO_Category) = New List(Of DTO_Category)
    Protected Shared strBuildScriptPage As StringBuilder = New StringBuilder
    Protected Shared oModule As lm.Comol.Modules.Base.DomainModel.ModuleFaq

#Region "Context"

    Private _Presenter As Adevico.Modules.Faq.Presentation.FaqListPresenter

    Private ReadOnly Property CurrentPresenter() As Adevico.Modules.Faq.Presentation.FaqListPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New Adevico.Modules.Faq.Presentation.FaqListPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strBuildScriptPage.Clear()


    End Sub

    'ServiceTitle

#Region "Page Base"


    ''' <summary>
    ''' Se false, esegue BindDati() solo alla prima richiesta, quando Page.IsPostBack = false.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property



    Public Overrides Sub BindDati()
        CTRLeditorText.InitializeControl(lm.Comol.Modules.Standard.GlossaryNew.Domain.ModuleGlossaryNew.UniqueCode)
        CurrentPresenter.InitView()
    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

#Region "Internazionalizzazione"
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_FAQList", "FAQ")

    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource

            Me.Master.ServiceTitle = .getValue("Service.Title")

            .setLiteral(LTpageTitle_t)
            .setLinkButton(BTNaddCat, True, True, False, True)
            .setLinkButton(BTaddFaq, True, True, False, True)
            .setLinkButton(BTNShowAddCat, True, True, False, False)
            .setLinkButton(BTNShowAddFaq, True, True, False, False)
            .setLinkButton(BTNannullaAddCat, True, True, False, False)
            .setLinkButton(BTNAnnullaAddFaq, True, True, False, False)
            .setLinkButton(BTNAnnullaAddFaq, True, True, False, False)
            .setLabel(LBnomeCat)
            .setLabel(LBcategoria)
            .setLabel(LBdomanda)
            .setLabel(LBrisposta)
            .setLiteral(LTtitolettoCategorie)
            '.setLinkButton(asd, True, True, False, True)

        End With
    End Sub



#End Region
    Public Overrides Sub ShowMessageToPage(errorMessage As String)
        Dim test As String = Resource.getValue("My.Value")

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get

        End Get
    End Property

#End Region

#Region "Implementazione View"
    Public Property CurrentCategoryId As Long Implements Adevico.Modules.Faq.Presentation.IVIewFaqList.CurrentCategoryId
        Get

            If Request.QueryString("catID") & "" <> "" Then
                Try
                    Return Long.Parse(Request.QueryString("catID"))
                Catch
                End Try
            End If

            Return -1
        End Get
        Set(value As Long)

        End Set
    End Property

    Public Sub ShowNoData() Implements Adevico.Modules.Faq.Presentation.IVIewFaqList.ShowNoData

    End Sub
    Public Sub LoadFaq(faqs As IList(Of lm.Comol.Modules.Standard.Faq.DTO_Faq), cats As IList(Of lm.Comol.Modules.Standard.Faq.DTO_Category), _oModule As lm.Comol.Modules.Base.DomainModel.ModuleFaq) Implements Adevico.Modules.Faq.Presentation.IVIewFaqList.LoadFaq

        oModule = _oModule
        PnNoFaqs.Visible = faqs.Count < 1
        BTNShowAddFaq.Visible = oModule.Admin Or oModule.CreateFaq
        BTNAnnullaAddFaq.Visible = oModule.Admin Or oModule.CreateFaq
        BTaddFaq.Visible = oModule.Admin Or oModule.CreateFaq
        BTNShowAddCat.Visible = oModule.Admin Or oModule.ManageCategory
        BTNannullaAddCat.Visible = oModule.Admin Or oModule.ManageCategory
        BTNaddCat.Visible = oModule.Admin Or oModule.ManageCategory
        ''Mostro la lista nella pagina.
        RPTFaq.DataSource = faqs
        RPTFaq.DataBind()

        RPTCats.DataSource = cats
        RPTCats.DataBind()

        CBLcategorie.Items.Clear()
        For Each cat As DTO_Category In cats
            CBLcategorie.Items.Add(New ListItem With {.Text = cat.Name, .Value = cat.ID, .Selected = False})
        Next

    End Sub
    Private Sub RPTFaq_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTFaq.ItemDataBound

        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim faqitem As DTO_Faq = e.Item.DataItem

            Dim LTquestion As Literal = e.Item.FindControl("LTquestion")
            Dim LTanswer As Literal = e.Item.FindControl("LTanswer")
            Dim LTcategories As Literal = e.Item.FindControl("LTcategories")
            Dim BTNdeleteFaq As LinkButton = e.Item.FindControl("BTNdeleteFaq")
            Dim BTmodifyFaq As LinkButton = e.Item.FindControl("BTmodifyFaq")
            Dim BTNUpOrderFaq As LinkButton = e.Item.FindControl("BTNUpOrderFaq")
            Dim BTNDownOrderFaq As LinkButton = e.Item.FindControl("BTNDownOrderFaq")

            If Not IsNothing(LTquestion) Then
                LTquestion.Text = faqitem.Question
            End If
            If Not IsNothing(LTanswer) Then
                LTanswer.Text = faqitem.Answer
            End If
            If Not IsNothing(LTcategories) Then
                LTcategories.Text = String.Join(", ", faqitem.Categories.Select(Function(c) c.Name).ToArray())
            End If
            If Not IsNothing(BTNdeleteFaq) Then
                Resource.setLinkButton(BTNdeleteFaq, True, True, False, True)
            End If
            If Not IsNothing(BTmodifyFaq) Then
                Resource.setLinkButton(BTmodifyFaq, True, True, False, False)
            End If

            BTmodifyFaq.Visible = oModule.Admin Or oModule.ModifyFaq
            BTNdeleteFaq.Visible = oModule.Admin Or oModule.DeleteFaq
            BTNUpOrderFaq.Visible = (oModule.Admin Or oModule.ModifyFaq Or oModule.CreateFaq) And (Request("catID") & "" = "-1" Or Request("catID") & "" = "")
            BTNDownOrderFaq.Visible = (oModule.Admin Or oModule.ModifyFaq Or oModule.CreateFaq) And (Request("catID") & "" = "-1" Or Request("catID") & "" = "")

        End If

    End Sub
#End Region

    Protected Sub RPTCats_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTCats.ItemDataBound

        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim isCatsVisible As Boolean = oModule.Admin Or oModule.ManageCategory

            Dim catItem As DTO_Category = e.Item.DataItem

            Dim HLcatName As HyperLink = e.Item.FindControl("HLcatName")
            Dim BTNdeleteCat As LinkButton = e.Item.FindControl("BTNdeleteCat")
            Dim BTNmodifyCat As LinkButton = e.Item.FindControl("BTNmodifyCat")

            If Not IsNothing(HLcatName) Then
                HLcatName.Text = catItem.Name
                HLcatName.NavigateUrl = "?catID=" & catItem.ID
            End If

            If Not IsNothing(BTNdeleteCat) Then
                Resource.setLinkButton(BTNdeleteCat, True, True, False, True)
                BTNdeleteCat.Visible = isCatsVisible
            End If
            If Not IsNothing(BTNmodifyCat) Then
                Resource.setLinkButton(BTNmodifyCat, True, True, False, False)
                BTNmodifyCat.Visible = isCatsVisible
            End If
        End If

    End Sub


    Protected Sub BTaddFaq_Click(sender As Object, e As EventArgs) Handles BTaddFaq.Click
        Dim newFaq As DTO_Faq = New DTO_Faq With {.Question = TXTquestion.Text, .Answer = CTRLeditorText.HTML, .Categories = getCategoriesFromView()}

        If newFaq.Answer = "" Or newFaq.Question = "" Then
            Return
        End If

        If HFfaqId.Value = "" Then
            Me.CurrentPresenter.InsertFaq(newFaq)
        Else
            Try
                newFaq.ID = Int64.Parse(HFfaqId.Value)
            Catch
                strBuildScriptPage.AppendLine(" alert('Errore: La Faq non ha un Id corretto.'); ")
                Return
            End Try
            Me.CurrentPresenter.ModifyFaq(newFaq)
        End If


    End Sub
    Protected Sub BTNaddCat_Click(sender As Object, e As EventArgs) Handles BTNaddCat.Click
        Dim newCat As DTO_Category = New DTO_Category With {.Name = TXTcatName.Text}

        If HFCatId.Value = "" Then
            Me.CurrentPresenter.InsertFaqCategory(newCat)
        Else
            Try
                newCat.ID = Int64.Parse(HFCatId.Value)
            Catch
                strBuildScriptPage.AppendLine(" alert('Errore: La Cat non ha un Id corretto.'); ")
                Return
            End Try


            Me.CurrentPresenter.ModifyCat(newCat)
        End If
    End Sub

    Protected Sub BTNShowAddFaq_Click(sender As Object, e As EventArgs) Handles BTNShowAddFaq.Click
        BlankView()

        PNaddFaq.Visible = True
        BTNShowAddFaq.Visible = False
    End Sub

    Protected Sub BTNShowAddCat_Click(sender As Object, e As EventArgs) Handles BTNShowAddCat.Click
        BlankView()

        BTNShowAddCat.Visible = False
        PNaddCat.Visible = True
    End Sub

    Protected Sub BTNAnnullaAddFaq_Click(sender As Object, e As EventArgs) Handles BTNAnnullaAddFaq.Click
        BlankView()

        BTNShowAddFaq.Visible = True
        PNaddFaq.Visible = False
    End Sub

    Protected Sub BTNannullaAddCat_Click(sender As Object, e As EventArgs) Handles BTNannullaAddCat.Click
        BlankView()

        BTNShowAddCat.Visible = True
        PNaddCat.Visible = False
    End Sub

    Protected Sub RPTFaq_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTFaq.ItemCommand
        Dim faqId As Int64 = e.CommandArgument

        If e.CommandName = "ClickModify" Then
            Me.CurrentPresenter.SetFaqModify(faqId)
        End If
        If e.CommandName = "ClickDelete" Then
            Me.CurrentPresenter.DeleteFaq(faqId)
        End If
        If e.CommandName = "ClickOrderUp" Then
            Dim faqList As List(Of DTO_Faq) = Me.CurrentPresenter.GetAllFaqs()

            Dim faqTemp As DTO_Faq = Nothing
            For Index As Int32 = 0 To faqList.Count - 1 Step 1
                faqTemp = faqList(Index)
                If (faqTemp.ID = faqId) Then
                    If (Index > 0) Then
                        faqTemp.Order = Index - 1

                        Dim faqTemp2 As DTO_Faq = faqList(Index - 1)
                        faqTemp2.Order = Index
                    End If
                Else
                    faqTemp.Order = Index
                End If
            Next

            Me.CurrentPresenter.UpdateOrderFaqs(faqList)
        End If
        If e.CommandName = "ClickOrderDwon" Then
            Dim faqList As List(Of DTO_Faq) = Me.CurrentPresenter.GetAllFaqs()

            Dim faqTemp As DTO_Faq = Nothing
            For Index As Int32 = 0 To faqList.Count - 1 Step 1
                faqTemp = faqList(Index)
                If (faqTemp.ID = faqId) Then
                    If (Index < faqList.Count - 1) Then
                        Index = Index + 1
                        faqTemp.Order = Index

                        Dim faqTemp2 As DTO_Faq = faqList(Index)
                        faqTemp2.Order = Index - 1
                    End If
                Else
                    faqTemp.Order = Index
                End If
            Next

            Me.CurrentPresenter.UpdateOrderFaqs(faqList)
        End If
    End Sub
    Protected Sub RPTCats_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTCats.ItemCommand
        Dim catId As Int64 = e.CommandArgument

        If e.CommandName = "ClickModify" Then
            Me.CurrentPresenter.SetCatModify(catId)
        End If
        If e.CommandName = "ClickDelete" Then
            Me.CurrentPresenter.DeleteCat(catId)
        End If
    End Sub

    ' show faq CRUD

    Public Sub ShowFaqInserted(faq As DTO_Faq, hasBeenInserted As Boolean) Implements Adevico.Modules.Faq.Presentation.IVIewFaqList.ShowFaqInserted
        If hasBeenInserted Then
            strBuildScriptpage.AppendLine(" alert('La FAQ è stata inserita.'); ")
        Else
            strBuildScriptpage.AppendLine(" alert('Errore: La FAQ non è stata inserita.'); ")
        End If

        ResetView()
    End Sub
    Public Sub ShowCatInserted(cat As DTO_Category, hasBeenInserted As Boolean) Implements Adevico.Modules.Faq.Presentation.IVIewFaqList.ShowCatInserted
        If hasBeenInserted Then
            strBuildScriptPage.AppendLine(" alert('La categoria è stata inserita.'); ")
        Else
            strBuildScriptPage.AppendLine(" alert('Errore: La categoria non è stata inserita.'); ")
        End If

        ResetView()
    End Sub
    Public Sub ShowFaqModified(faq As DTO_Faq, hasBeenModified As Boolean) Implements Adevico.Modules.Faq.Presentation.IVIewFaqList.ShowFaqModified
        If hasBeenModified Then
            strBuildScriptPage.AppendLine(" alert('La FAQ è stata modificata.'); ")
        Else
            strBuildScriptPage.AppendLine(" alert('Errore: La FAQ non è stata modificata.'); ")
        End If

        ResetView()
    End Sub
    'show cat CRUD
    Public Sub ShowCatModified(cat As DTO_Category, hasBeenModified As Boolean) Implements Adevico.Modules.Faq.Presentation.IVIewFaqList.ShowCatModified
        If hasBeenModified Then
            strBuildScriptPage.AppendLine(" alert('La categoria è stata modificata.'); ")
        Else
            strBuildScriptPage.AppendLine(" alert('Errore: La categoria non è stata modificata.'); ")
        End If

        ResetView()
    End Sub
    Public Sub ShowFaqDeleted(hasBeenDeleted As Boolean) Implements Adevico.Modules.Faq.Presentation.IVIewFaqList.ShowFaqDeleted
        If hasBeenDeleted Then
            strBuildScriptPage.AppendLine(" alert('La FAQ è stata cancellata.'); ")
        Else
            strBuildScriptPage.AppendLine(" alert('Errore: La FAQ non è stata cancellata.'); ")
        End If

        ResetView()
    End Sub
    Public Sub ShowCatDeleted(hasBeenDeleted As Boolean) Implements Adevico.Modules.Faq.Presentation.IVIewFaqList.ShowCatDeleted
        If hasBeenDeleted Then
            strBuildScriptPage.AppendLine(" alert('La categoria è stata cancellata.'); ")
        Else
            strBuildScriptPage.AppendLine(" alert('Errore: La categoria non è stata cancellata.'); ")
        End If
        ResetView()
    End Sub

    Public Sub ShowFaqModify(faq As DTO_Faq) Implements Adevico.Modules.Faq.Presentation.IVIewFaqList.ShowFaqModify
        If IsNothing(faq) Then
            strBuildScriptPage.AppendLine(" alert('Errore: La faq non è stata trovata.'); ")
            Return
        End If

        PNaddFaq.Visible = True
        BTNShowAddFaq.Visible = False
        PNaddCat.Visible = False
        BTNShowAddCat.Visible = True

        CTRLeditorText.HTML = faq.Answer
        TXTquestion.Text = faq.Question
        HFfaqId.Value = faq.ID & ""
        Me.setCategoriesView(faq)
    End Sub
    Public Sub ShowCatModify(cat As DTO_Category) Implements Adevico.Modules.Faq.Presentation.IVIewFaqList.ShowCatModify
        If IsNothing(cat) Then
            strBuildScriptPage.AppendLine(" alert('Errore: La categoria non è stata trovata.'); ")
            Return
        End If

        PNaddFaq.Visible = False
        BTNShowAddFaq.Visible = True
        PNaddCat.Visible = True
        BTNShowAddCat.Visible = False

        TXTcatName.Text = cat.Name
        HFCatId.Value = cat.ID & ""
    End Sub
    Public Sub ShowNoPermission() Implements Adevico.Modules.Faq.Presentation.IVIewFaqList.ShowNoPermission
        BindNoPermessi()
    End Sub

    Private Sub ResetView()
        BlankView()
        Me.CurrentPresenter.InitView()
    End Sub

    Private Sub BlankView()
        PNaddFaq.Visible = False
        PNaddCat.Visible = False
        BTNShowAddFaq.Visible = True
        BTNShowAddCat.Visible = True

        HFfaqId.Value = ""
        HFCatId.Value = ""
        CTRLeditorText.HTML = ""
        TXTcatName.Text = ""
        TXTquestion.Text = ""
        setCategoriesView(New DTO_Faq()) ' set false le checkbox

    End Sub

    Private Function getCategoriesFromView() As List(Of DTO_Category)
        Dim listCats As List(Of DTO_Category) = New List(Of DTO_Category)

        For Each li As ListItem In CBLcategorie.Items
            If li.Selected Then
                listCats.Add(New DTO_Category With {.ID = Int64.Parse(li.Value), .Name = li.Text})
            End If
        Next

        Return listCats
    End Function

    Private Sub setCategoriesView(faq As DTO_Faq)
        For Each li As ListItem In CBLcategorie.Items
            li.Selected = False
            If Not IsNothing(faq.Categories) And faq.Categories.Where(Function(c) c.ID & "" = li.Value).Count() > 0 Then
                li.Selected = True
            End If
        Next
    End Sub

End Class