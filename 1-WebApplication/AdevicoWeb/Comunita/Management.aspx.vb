Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.UCServices
Imports Telerik.WebControls
Imports COL_BusinessLogic_v2.UCServices.Services_AmministraComunita
Imports lm.ActionDataContract
Imports lm.Comol.UI.Presentation


Public Class Management
    Inherits System.Web.UI.Page
    Protected oResource As ResourceManager
    Private _PageUtility As OLDpageUtility

    Public ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            PageUtility = _PageUtility
        End Get
    End Property

#Region "Management File"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As lm.Comol.Modules.Base.Presentation.ManagementCommunityPresenter

    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As lm.Comol.Modules.Base.Presentation.ManagementCommunityPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New lm.Comol.Modules.Base.Presentation.ManagementCommunityPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region
    


    Private Enum Iscrizioni_code
        IscrizioniAperteIl = 0
        IscrizioniChiuse = 1
        IscrizioniComplete = 2
        IscrizioniEntro = 3
    End Enum
    Private Enum StringaOrdinamento
        Crescente = 0
        Decrescente = 1
        Corrente = 2
    End Enum

    Protected Enum AzioneTree
        Aggiorna = 1
        Dettagli = 2
        Entra = 3
        Iscrivi = 4
        Modifica = 5
        Cancella = 6
        GestioneServizi = 7
        GestioneUtenti = 8
        LogonAs = 9
        LogonAsRuolo = 10
        AssociaA = 11
        Coordina = 12

        Archivia = 13
        DeArchivia = 14
        Blocca = 15
        Sblocca = 16
    End Enum

    Protected WithEvents HDNcmnt_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNreturnTo As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_isDiretto As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_idPadre_Link As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_nodoSel As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_nodoPadreSel As System.Web.UI.HtmlControls.HtmlInputHidden

#Region "FORM PERMESSI"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    'Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLmenuPrincipale As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBtreeView As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBcrea As System.Web.UI.WebControls.LinkButton

    Protected WithEvents PNLmenuTree As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBespandi As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBcomprimi As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBelenco As System.Web.UI.WebControls.LinkButton

    Protected WithEvents PNLmenuDettagli As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBnascondiDettagli As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBentraDettagli As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBiscriviDettagli As System.Web.UI.WebControls.LinkButton

    Protected WithEvents PNLmenuCancella As System.Web.UI.WebControls.Panel
    Protected WithEvents LNBindietro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBelimina As System.Web.UI.WebControls.LinkButton

#Region "FORM Filtri"
    Protected WithEvents PNLgriglia As System.Web.UI.WebControls.Panel
    Protected WithEvents DDLTipo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLNumeroRecord As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLTipoRicerca As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TXBValore As System.Web.UI.WebControls.TextBox

    Protected WithEvents DGComunita As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LBmsgDG As System.Web.UI.WebControls.Label

    Protected WithEvents LBorganizzazione_c As System.Web.UI.WebControls.Label
    Protected WithEvents DDLorganizzazione As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TBLcorsi As System.Web.UI.WebControls.Table
    Protected WithEvents LBannoAccademico_c As System.Web.UI.WebControls.Label
    Protected WithEvents DDLannoAccademico As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LBperiodo_c As System.Web.UI.WebControls.Label
    Protected WithEvents DDLperiodo As System.Web.UI.WebControls.DropDownList

    Protected WithEvents TBLcorsiDiStudio As System.Web.UI.WebControls.Table
    Protected WithEvents LBcorsoDiStudi_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLtipoCorsoDiStudi As System.Web.UI.WebControls.DropDownList

    Protected WithEvents TBRtipiSpeciali As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBnoCorsi As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoComunita_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBnumeroRecord_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoRicerca_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBvalore_c As System.Web.UI.WebControls.Label
    Protected WithEvents LBstatoComunita_t As System.Web.UI.WebControls.Label
    Protected WithEvents RBLstatoComunita As System.Web.UI.WebControls.RadioButtonList

    Protected WithEvents DDLresponsabile As System.Web.UI.WebControls.DropDownList
    Protected WithEvents CBXautoUpdate As System.Web.UI.WebControls.CheckBox
    Protected WithEvents TBLfiltroNew As System.Web.UI.WebControls.Table
    Protected WithEvents TBRchiudiFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents LNBchiudiFiltro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TBRapriFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents LNBapriFiltro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TBRfiltri As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBLfiltro As System.Web.UI.WebControls.Table
#Region "Lettere"
    Protected WithEvents LKBtutti As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBaltro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBa As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBb As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBc As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBd As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBe As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBf As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBg As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBh As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBi As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBj As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBk As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBl As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBn As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBp As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBq As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBr As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBs As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBt As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBu As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBv As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBw As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBx As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBy As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBz As System.Web.UI.WebControls.LinkButton
#End Region
    Protected WithEvents BTNCerca As System.Web.UI.WebControls.Button

    Protected WithEvents HDNselezionato As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroFacolta As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroTipoRicerca As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroValore As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroResponsabileID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroLaureaID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroTipoCdl As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroAnno As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroPeriodo As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_filtroTipoComunitaID As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region

#Region "FORM TreeView"
    Protected WithEvents PNLtreeView As System.Web.UI.WebControls.Panel
    Protected WithEvents RDTcomunita As Telerik.WebControls.RadTreeView
    Protected WithEvents HDN_Path As System.Web.UI.HtmlControls.HtmlInputHidden
#End Region

#Region "Form Dettagli"
    Protected WithEvents PNLdettagli As System.Web.UI.WebControls.Panel
    Protected WithEvents CTRLDettagli As Comunita_OnLine.UC_DettagliComunita
    Protected WithEvents LBlegendaDettagli As System.Web.UI.WebControls.Label
#End Region

#Region "Conferma Eliminazione"
    Protected WithEvents PNLconfermaElimina As System.Web.UI.WebControls.Panel
    Protected WithEvents LBconfermaElimina As System.Web.UI.WebControls.Label
#End Region

#Region "Form Dettagli Cancellazione"
    Protected WithEvents PNLdettagliElimina As System.Web.UI.WebControls.Panel
    Protected WithEvents LBdettagliElimina As System.Web.UI.WebControls.Label
#End Region

#Region " Codice generato da Progettazione Web Form "

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim oServizio As New UCServices.Services_AmministraComunita



        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If
        If Me.SessioneScaduta() Then
            Exit Sub
        End If
        If Not Page.IsPostBack Then
            Me.SetupInternazionalizzazione()
            Session("AdminForChange") = False
            Session("azione") = "load"
            Session("CMNT_path_forAdmin") = ""
            Session("idComunita_forAdmin") = ""
            Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(UCServices.Services_AmministraComunita.Codex)
            oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
        Else
            If Me.ViewState("PermessiAssociati") = "" Then
                Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(UCServices.Services_AmministraComunita.Codex)
            End If
            oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
        End If

        If oServizio.Admin Or oServizio.Delete Or oServizio.Moderate Or oServizio.CreateComunity Or oServizio.Change Then
            If Not Page.IsPostBack Then
                ViewState("SortExspression") = "CMNT_Nome"
                ViewState("SortDirection") = "asc"


                Try
                    Me.RBLstatoComunita.SelectedValue = Main.FiltroStatoComunita.Attiva
                Catch ex As Exception

                End Try

                Dim ComunitaAttualeID As Integer
                Try
                    ComunitaAttualeID = Session("idComunita")
                Catch ex As Exception
                End Try

                If ComunitaAttualeID > 0 Then
                    Dim oComunita As New COL_Comunita
                    oComunita.Id = ComunitaAttualeID

                    If oServizio.Admin Or oServizio.Moderate Or oServizio.CreateComunity Then
                        Me.LKBcrea.Visible = True
                    Else
                        Me.LKBcrea.Visible = False
                    End If
                    If oComunita.HasSottoComunita Then
                        Me.Bind_Dati()
                        Me.ViewState("intCurPage") = 0
                        Me.ViewState("intAnagrafica") = -1
                        Me.LKBtutti.CssClass = "lettera_Selezionata"
                        Me.Setup_Script()

                        If Me.Request.QueryString("toTree") <> "" Then
                            Me.PNLgriglia.Visible = False
                            Me.PNLtreeView.Visible = True
                            Me.PNLmenuPrincipale.Visible = False
                            Me.PNLmenuTree.Visible = True
                            Me.Bind_TreeView(False)
                        Else
                            Me.PNLmenuPrincipale.Visible = True
                            Me.PNLmenuTree.Visible = False
                            Me.Bind_Griglia(True)
                        End If
                    Else
                        Me.PNLgriglia.Visible = False
                        Me.PNLtreeView.Visible = False
                        Me.PNLmenuTree.Visible = False
                        Me.PNLmenuPrincipale.Visible = True
                    End If

                    Me.PNLcontenuto.Visible = True
                    Me.PNLpermessi.Visible = False
                Else
                    Me.PNLmenuPrincipale.Visible = False
                    Me.PNLcontenuto.Visible = False
                    Me.PNLpermessi.Visible = True
                End If
            End If
        Else
            Me.PNLmenuPrincipale.Visible = False
            Me.PNLcontenuto.Visible = False
            Me.PNLpermessi.Visible = True
        End If

        Me.Page.Form.DefaultButton = Me.BTNCerca.UniqueID
        Me.Page.Form.DefaultFocus = Me.TXBValore.UniqueID
        Me.Master.Page.Form.DefaultButton = Me.BTNCerca.UniqueID
        Me.Master.Page.Form.DefaultFocus = Me.TXBValore.UniqueID
    End Sub

    Private Function GetPermessiForPage(ByVal Codex As String) As String
        Dim PermessiAssociati As String
        Dim oPersona As New COL_Persona
        oPersona = Session("objPersona")
        Try
            PermessiAssociati = Permessi(Codex, Me.Page)

            If (PermessiAssociati = "") Then
                PermessiAssociati = "00000000000000000000000000000000"
            End If
        Catch ex As Exception
            PermessiAssociati = "00000000000000000000000000000000"
        End Try
        Return PermessiAssociati
    End Function

    Private Sub Setup_Script()
        Me.LNBtreeView.Attributes.Add("onclick", "window.status='' ;return true;")
        Me.LNBcomprimi.Attributes.Add("onclick", "window.status='';CollapseAll();return false;")
        Me.LNBespandi.Attributes.Add("onclick", "window.status='';ExpandAll();return false;")
    End Sub

    Private Function SessioneScaduta() As Boolean
        Dim oPersona As COL_Persona
        Dim isScaduta As Boolean = True
        Try
            oPersona = Session("objPersona")
            If oPersona.ID > 0 Then
                isScaduta = False
                Return False
            End If
        Catch ex As Exception

        End Try
        If isScaduta Then
            Dim alertMSG As String
            alertMSG = oResource.getValue("LogoutMessage")
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
            Else
                alertMSG = "Session timeout"
            End If
            Dim PageUtility As New OLDpageUtility(Me.Context)
            Dim UrlRedirect As String = PageUtility.GetDefaultLogoutPage ' Me.DefaultUrl
            Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & UrlRedirect & "');</script>")
            Return True
        Else
            Try
                Dim CMNT_ID As Integer = 0
                Try
                    If Session("AdminForChange") = True Then
                        CMNT_ID = Session("idComunita_forAdmin")
                    Else
                        CMNT_ID = Session("idComunita")
                    End If
                Catch ex As Exception
                    Try
                        CMNT_ID = Session("idComunita")
                    Catch ex2 As Exception
                        CMNT_ID = 0
                    End Try

                End Try

                If CMNT_ID <= 0 Then
                    Me.ExitToLimbo()
                    Return True
                End If
            Catch ex As Exception
                Me.ExitToLimbo()
                Return True
            End Try
        End If
    End Function
    Private Sub ExitToLimbo()
        Session("Limbo") = True
        Session("ORGN_id") = 0
        Session("IdRuolo") = ""
        Session("ArrPermessi") = ""
        Session("RLPC_ID") = ""

        Session("AdminForChange") = False
        Session("CMNT_path_forAdmin") = ""
        Session("idComunita_forAdmin") = ""
        Session("TPCM_ID") = ""
        Me.Response.Expires = 0
        Me.PageUtility.RedirectToUrl("Comunita/EntrataComunita.aspx")
    End Sub

#Region "Bind_Dati"
    Private Sub Bind_Dati()
        Dim re_set As Boolean = False
        Try
            If Me.Request.QueryString("re_set") <> "" Then
                re_set = CBool(Me.Request.QueryString("re_set"))
            End If
        Catch ex As Exception

        End Try

        Me.Bind_TipiComunita()
        Me.Bind_Organizzazioni()

        If re_set Then
            Me.SetupSearchParameters()
        Else
            Me.Bind_Responsabili()

            Me.TBLcorsi.Visible = False
            Me.TBLcorsiDiStudio.Visible = False
            Me.LBnoCorsi.Visible = True
        End If
        Me.DDLTipoRicerca.Attributes.Add("onchange", "return AggiornaForm();")
        Me.DDLresponsabile.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLTipoRicerca.Attributes.Add("onchange", "return AggiornaForm();")

        Try
            Me.HDN_filtroAnno.Value = Me.DDLannoAccademico.SelectedValue
            Me.HDN_filtroFacolta.Value = Me.DDLorganizzazione.SelectedValue
            Me.HDN_filtroPeriodo.Value = Me.DDLperiodo.SelectedValue
            Me.HDN_filtroResponsabileID.Value = Me.DDLresponsabile.SelectedValue
            Me.HDN_filtroTipoCdl.Value = Me.DDLtipoCorsoDiStudi.SelectedValue
            Me.HDN_filtroTipoComunitaID.Value = Me.DDLTipo.SelectedValue
            Me.HDN_filtroTipoRicerca.Value = Me.DDLTipoRicerca.SelectedValue
            Me.HDN_filtroValore.Value = Me.TXBValore.Text
            Me.HDNselezionato.Value = Me.HDN_filtroTipoRicerca.Value
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Bind_TipiComunita()
        '...nella ddl che mi far� da filtro delle tipologie di utenti associate al tipo comunit�
        Dim oDataSet As New DataSet
        Dim oTipoComunita As New COL_Tipo_Comunita


        Try
            oDataSet = oTipoComunita.ElencaForFiltri(Session("LinguaID"), True, Session("idComunita"))
            If oDataSet.Tables(0).Rows.Count > 0 Then
                DDLTipo.DataSource = oDataSet
                DDLTipo.DataTextField() = "TPCM_descrizione"
                DDLTipo.DataValueField() = "TPCM_id"
                DDLTipo.DataBind()

                'aggiungo manualmente elemento che indica tutti i tipi di comunit�
                If Me.DDLTipo.Items.Count > 1 Then
                    Me.DDLTipo.Items.Insert(0, New ListItem("-- Tutti --", -1))
                End If

            End If
        Catch ex As Exception
            DDLTipo.Items.Insert(0, New ListItem("-- Tutti --", -1))
        End Try
        Try
            Me.DDLTipo.SelectedIndex = 1
        Catch ex As Exception

        End Try
        'Try
        '    if 
        '    Me.DDLTipo.SelectedValue = Main.TipoComunitaStandard.Corso
        'Catch ex As Exception

        'End Try
        oResource.setDropDownList(Me.DDLTipo, -1)
    End Sub
    Private Sub Bind_Organizzazioni()
        Dim oDataset As New DataSet
        Dim oPersona As New COL_Persona

        Me.DDLorganizzazione.Items.Clear()
        Try
            oPersona = Session("objPersona")
            oDataset = oPersona.GetOrganizzazioniAssociate()

            If oDataset.Tables(0).Rows.Count > 0 Then
                Me.DDLorganizzazione.DataValueField = "ORGN_id"
                Me.DDLorganizzazione.DataTextField = "ORGN_ragioneSociale"
                Me.DDLorganizzazione.DataSource = oDataset
                Me.DDLorganizzazione.DataBind()

                If Me.DDLorganizzazione.Items.Count > 1 Then
                    Me.DDLorganizzazione.Enabled = True

                    If IsNothing(Session("ORGN_id")) = False Then
                        Try
                            Me.DDLorganizzazione.SelectedValue = Session("ORGN_id")
                        Catch ex As Exception
                            Me.DDLorganizzazione.SelectedIndex = 0
                        End Try
                    Else
                        Me.DDLorganizzazione.SelectedIndex = 0
                    End If
                Else
                    Me.DDLorganizzazione.Enabled = False
                End If
            Else
                Me.DDLorganizzazione.Items.Add(New ListItem("< nessuna >", 0))
                Me.DDLorganizzazione.Enabled = False
            End If
        Catch ex As Exception
            Me.DDLorganizzazione.Items.Clear()
            Me.DDLorganizzazione.Items.Add(New ListItem("< nessuna >", 0))
            Me.DDLorganizzazione.Enabled = False
        End Try
        oResource.setDropDownList(Me.DDLorganizzazione, 0)
    End Sub
    Private Sub Bind_Responsabili(Optional ByVal DocenteID As Integer = -1)
        Dim oDataSet As New DataSet
        Dim FacoltaID As Integer = -1
        Dim ComunitaID As Integer = -1
        Try
            Me.DDLresponsabile.Items.Clear()

            If IsArray(Session("ArrComunita")) Then
                Dim ArrComunita(,) As String
                Try
                    Dim oComunitaOrganizzazione As New COL_Comunita
                    ArrComunita = Session("ArrComunita")
                    oComunitaOrganizzazione.Id = ArrComunita(0, 0)

                    FacoltaID = oComunitaOrganizzazione.GetOrganizzazioneID
                Catch ex As Exception

                End Try
            End If

            Dim TipoComuniaID As Integer = -1
            Dim TipoCdlID As Integer = -1
            Dim AnnoAcc As Integer = -1
            Dim PeriodoID As Integer = -1
            Try
                If Session("IdComunita") > 0 Then
                    ComunitaID = Session("IdComunita")
                End If
            Catch ex As Exception

            End Try
            Try
                If Me.CBXautoUpdate.Checked Then
                    TipoComuniaID = Me.DDLTipo.SelectedValue
                Else
                    TipoComuniaID = Me.HDN_filtroTipoComunitaID.Value
                End If
            Catch ex As Exception

            End Try

         
                oDataSet = COL_Comunita.RicercaResponsabili(FacoltaID, ComunitaID, Session("objPersona").id, TipoComuniaID, , , , , Main.FiltroStatoComunita.Attiva)


            If oDataSet.Tables(0).Rows.Count > 0 Then
                DDLresponsabile.DataSource = oDataSet
                DDLresponsabile.DataTextField() = "Anagrafica"
                DDLresponsabile.DataValueField() = "PRSN_ID"
                DDLresponsabile.DataBind()

                DDLresponsabile.Items.Insert(0, New ListItem("-- Tutti --", -1))
            End If
        Catch ex As Exception
            Me.DDLresponsabile.Items.Insert(0, New ListItem("-- Tutti --", -1))
        End Try
        oResource.setDropDownList(Me.DDLresponsabile, -1)
        If DocenteID > 0 Then
            Try
                Me.DDLresponsabile.SelectedValue = DocenteID
            Catch ex As Exception

            End Try
        End If

        Try
            If Me.DDLTipoRicerca.SelectedValue = Main.FiltroComunita.IDresponsabile Then
                Me.DDLresponsabile.Visible = True
                Me.TXBValore.Visible = False
                Me.TXBValore.Text = ""
            Else
                Me.DDLresponsabile.Visible = False
                Me.TXBValore.Visible = True
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Function Filtraggio_Dati(Optional ByVal ApplicaFiltri As Boolean = False) As DataSet
        Dim oDataset As New DataSet
        Dim i, totale, totaleHistory As Integer

        Try
            Dim valore As String = ""
            Dim oFiltroTipoRicerca As Main.FiltroComunita = Main.FiltroComunita.tutti
            Dim oFiltroLettera As Main.FiltroComunita = Main.FiltroComunita.tutti
            Dim oPersona As New COL_Persona

            oPersona = Session("objPersona")
            Try
                If IsNumeric(Me.ViewState("intAnagrafica")) Then
                    oFiltroLettera = CType(Me.ViewState("intAnagrafica"), Main.FiltroComunita)
                Else
                    oFiltroLettera = Main.FiltroComunita.tutti
                    Me.SelezionaLink_All()
                End If
            Catch ex As Exception
                oFiltroLettera = Main.FiltroComunita.tutti
                Me.SelezionaLink_All()
            End Try

            If Me.CBXautoUpdate.Checked Or ApplicaFiltri = True Then
                If Me.TXBValore.Text <> "" Then
                    Me.TXBValore.Text = Trim(Me.TXBValore.Text)
                End If
                valore = Me.TXBValore.Text
            Else
                Try
                    valore = Trim(Me.HDN_filtroValore.Value)
                Catch ex As Exception

                End Try
            End If

            Dim TipoRicercaID As Integer
            If Me.CBXautoUpdate.Checked Or ApplicaFiltri = True Then
                TipoRicercaID = Me.DDLTipoRicerca.SelectedValue
            Else
                Try
                    TipoRicercaID = Me.HDN_filtroTipoRicerca.Value
                Catch ex As Exception
                    TipoRicercaID = -1
                End Try
            End If
            If valore <> "" Or (Me.CBXautoUpdate.Checked And Me.DDLresponsabile.Visible) Or (Not Me.CBXautoUpdate.Checked And TipoRicercaID = Main.FiltroComunita.IDresponsabile) Then
                Select Case TipoRicercaID
                    Case Main.FiltroComunita.nome
                        oFiltroTipoRicerca = Main.FiltroComunita.nome
                    Case Main.FiltroComunita.creataDopo
                        If IsDate(valore) = False Then
                            valore = ""
                        Else
                            oFiltroTipoRicerca = Main.FiltroComunita.creataDopo
                        End If
                    Case Main.FiltroComunita.creataPrima
                        If IsDate(valore) = False Then
                            valore = ""
                        Else
                            oFiltroTipoRicerca = Main.FiltroComunita.creataPrima
                        End If
                    Case Main.FiltroComunita.dataIscrizioneDopo
                        If IsDate(valore) = False Then
                            valore = ""
                        Else
                            oFiltroTipoRicerca = Main.FiltroComunita.dataIscrizioneDopo
                        End If
                    Case Main.FiltroComunita.dataFineIscrizionePrima
                        If IsDate(valore) = False Then
                            valore = ""
                        Else
                            oFiltroTipoRicerca = Main.FiltroComunita.dataFineIscrizionePrima
                        End If
                    Case Main.FiltroComunita.contiene
                        oFiltroTipoRicerca = Main.FiltroComunita.contiene
                    Case Main.FiltroComunita.cognomeDocente
                        oFiltroTipoRicerca = Main.FiltroComunita.cognomeDocente
                    Case Main.FiltroComunita.IDresponsabile
                        Try
                            If Me.CBXautoUpdate.Checked Then
                                valore = Me.DDLresponsabile.SelectedValue
                            Else
                                valore = Me.HDN_filtroResponsabileID.Value
                            End If

                            oFiltroTipoRicerca = Main.FiltroComunita.IDresponsabile
                        Catch ex As Exception
                            valore = -1
                        End Try
                    Case Else
                        valore = ""
                End Select
            End If
            If (Me.CBXautoUpdate.Checked Or ApplicaFiltri = True) And valore = "" Then
                Me.TXBValore.Text = valore
                Me.HDN_filtroValore.Value = ""
            End If

            Dim ComunitaPadreID As Integer
            Try
                ComunitaPadreID = Session("idComunita")
                If ComunitaPadreID < 1 Then
                    ComunitaPadreID = -1
                End If
            Catch ex As Exception
                ComunitaPadreID = -1
            End Try

            Dim FacoltaID, LaureaID, PeriodoID, AAid, TipocomunitaID, TipoCdlID As Integer
            If Me.CBXautoUpdate.Checked Or ApplicaFiltri = True Then
                Me.HDN_filtroTipoRicerca.Value = Me.DDLTipoRicerca.SelectedValue
                Me.HDN_filtroValore.Value = Me.TXBValore.Text
                Me.HDN_filtroResponsabileID.Value = Me.DDLresponsabile.SelectedValue
                Me.HDN_filtroTipoCdl.Value = Me.DDLtipoCorsoDiStudi.SelectedValue
                Me.HDN_filtroAnno.Value = Me.DDLannoAccademico.SelectedValue
                Me.HDN_filtroPeriodo.Value = Me.DDLperiodo.SelectedValue
                Me.HDN_filtroTipoComunitaID.Value = Me.DDLTipo.SelectedValue
            End If


            Try
                FacoltaID = Me.HDN_filtroFacolta.Value
            Catch ex As Exception
                FacoltaID = -1
            End Try
            Try
                LaureaID = Me.HDN_filtroLaureaID.Value
            Catch ex As Exception
                LaureaID = -1
            End Try
            Try
                AAid = Me.HDN_filtroAnno.Value
            Catch ex As Exception
                AAid = -1
            End Try
            Try
                PeriodoID = Me.HDN_filtroPeriodo.Value
            Catch ex As Exception
                PeriodoID = -1
            End Try
            Try
                TipocomunitaID = Me.HDN_filtroTipoComunitaID.Value
            Catch ex As Exception
                TipocomunitaID = -1
            End Try
            Try
                TipoCdlID = Me.HDN_filtroTipoCdl.Value
            Catch ex As Exception
                TipoCdlID = -1
            End Try

            Dim ComunitaPath As String = ""
            If IsArray(Session("ArrComunita")) Then
                Try
                    Dim ArrComunita(,) As String
                    ArrComunita = Session("ArrComunita")
                    totaleHistory = UBound(ArrComunita, 2)
                    ComunitaPath = ArrComunita(2, totaleHistory)
                Catch ex As Exception

                End Try
            End If
            If Not String.IsNullOrEmpty(valore) Then
                valore = valore.Replace("'", "''")
            End If

            Dim oComunita As New COL_Comunita
            If ComunitaPath <> "" Then
  
                        oDataset = oComunita.RicercaComunitaForManagement(Session("LinguaID"), FacoltaID, ComunitaPath, ComunitaPadreID, oPersona.ID, oFiltroTipoRicerca, oFiltroLettera, valore, TipocomunitaID, , , , , Me.RBLstatoComunita.SelectedValue)

            End If
            oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Esteso"))
            oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_EstesoNoSpan"))
            oDataset.Tables(0).Columns.Add(New DataColumn("Proprieta"))
            oDataset.Tables(0).Columns.Add(New DataColumn("Alternative"))
            oDataset.Tables(0).Columns.Add(New DataColumn("Iscritti"))

            If Not oDataset.Tables(0).Columns.Contains("AnnoAccademico") Then
                oDataset.Tables(0).Columns.Add(New DataColumn("AnnoAccademico"))
            End If
            If Not oDataset.Tables(0).Columns.Contains("Periodo") Then
                oDataset.Tables(0).Columns.Add(New DataColumn("Periodo"))
            End If
            If Not oDataset.Tables(0).Columns.Contains("CMNT_Iscritti") Then
                oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Iscritti"))
            End If
            If Not oDataset.Tables(0).Columns.Contains("CMNT_Totale") Then
                oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Totale"))
            End If
            If Not oDataset.Tables(0).Columns.Contains("AnagraficaResponsabile") Then
                oDataset.Tables(0).Columns.Add(New DataColumn("AnagraficaResponsabile"))
            End If
            If Not oDataset.Tables(0).Columns.Contains("CMNT_dataCreazioneText") Then
                oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_dataCreazioneText"))
            End If
            If Not oDataset.Tables(0).Columns.Contains("CMNT_dataCessazioneText") Then
                oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_dataCessazioneText"))
            End If

            Dim oDataview As New DataView
            Dim ElencoComunitaID As String = ","

            oDataview = oDataset.Tables(0).DefaultView
            oDataview.AllowDelete = True
            totale = oDataset.Tables(0).Rows.Count

            If Not (Session("limbo") = True And Me.Request.QueryString("show") = 0) Then
                While oDataview.Count > 0
                    Dim ComunitaID As Integer

                    If ElencoComunitaID = "," Then
                        ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
                        oDataview.RowFilter = "CMNT_ID=" & ComunitaID
                    Else
                        oDataview.RowFilter = "'" & ElencoComunitaID & "' not like '%,' + CMNT_ID + ',%'"

                        If oDataview.Count > 0 Then
                            ComunitaID = oDataview.Item(0).Row.Item("CMNT_id")
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                        End If
                    End If

                    If oDataview.Count = 1 Then
                        oDataview.RowFilter = ""
                        ElencoComunitaID = ElencoComunitaID & ComunitaID & ","
                    ElseIf oDataview.Count > 1 Then
                        oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=1" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                        If oDataview.Count = 1 Then
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'" '' '%," & ComunitaID & ",%' not in ('" & ElencoComunitaID & "')"
                            While oDataview.Count > 0
                                oDataview.Delete(0)
                            End While
                        ElseIf oDataview.Count = 0 Then
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'" '''%," & ComunitaID & ",%' not in ('" & ElencoComunitaID & "')"
                            While oDataview.Count > 1
                                oDataview.Delete(1)
                            End While
                        ElseIf oDataview.Count > 1 Then
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=1" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                            While oDataview.Count > 1
                                oDataview.Delete(1)
                            End While
                            oDataview.RowFilter = "CMNT_ID=" & ComunitaID & " AND ALCM_PercorsoDiretto=0" & " AND '" & ElencoComunitaID & "' not like '%," & ComunitaID & ",%'"
                            While oDataview.Count > 0
                                oDataview.Delete(0)
                            End While
                        End If

                        oDataview.RowFilter = ""
                        ElencoComunitaID = ElencoComunitaID & ComunitaID & ","
                    End If

                End While
                oDataview.RowFilter = ""
                oDataset.AcceptChanges()
            Else
                oDataset.Tables(0).Rows.Clear()
                oDataset.AcceptChanges()
            End If

            Dim ImageBaseDir, img As String
            ImageBaseDir = GetPercorsoApplicazione(Me.Request)
            ImageBaseDir = ImageBaseDir & "/RadControls/TreeView/Skins/Comunita/logo/"
            ImageBaseDir = Replace(ImageBaseDir, "//", "/")

            totale = oDataset.Tables(0).Rows.Count
            Me.DGComunita.Columns(6).Visible = False

            For i = 0 To totale - 1
                Dim oRow As DataRow

                oRow = oDataset.Tables(0).Rows(i)

                Try
                    If IsDBNull(oRow.Item("CMNT_Responsabile")) And Not IsDBNull(oRow.Item("AnagraficaCreatore")) Then
                        oRow.Item("AnagraficaResponsabile") = oResource.getValue("creata")
                        oRow.Item("AnagraficaResponsabile") = oRow.Item("AnagraficaResponsabile").Replace("#%%#", oRow.Item("AnagraficaCreatore"))
                    Else
                        If oRow.Item("CMNT_Responsabile") = "" And Not IsDBNull(oRow.Item("AnagraficaCreatore")) Then
                            oRow.Item("AnagraficaResponsabile") = oResource.getValue("creata")
                            oRow.Item("AnagraficaResponsabile") = oRow.Item("AnagraficaResponsabile").Replace("#%%#", oRow.Item("AnagraficaCreatore"))
                        Else
                            oRow.Item("AnagraficaResponsabile") = oRow.Item("CMNT_Responsabile")
                        End If
                    End If
                Catch ex As Exception

                End Try

                Try
                    If oRow.Item("CMNT_IsChiusa") = True Then
                        oRow.Item("Proprieta") = oResource.getValue("stato.image." & oRow.Item("CMNT_IsChiusa"))
                        oRow.Item("Alternative") = oResource.getValue("stato." & oRow.Item("CMNT_IsChiusa"))
                    ElseIf oRow.Item("ALCM_isChiusaForPadre") = True Then
                        oRow.Item("Proprieta") = oResource.getValue("stato.image." & oRow.Item("ALCM_isChiusaForPadre"))
                        oRow.Item("Alternative") = oResource.getValue("stato." & oRow.Item("ALCM_isChiusaForPadre"))
                    Else
                        oRow.Item("Proprieta") = oResource.getValue("stato.image." & oRow.Item("CMNT_IsChiusa"))
                        oRow.Item("Alternative") = oResource.getValue("stato." & oRow.Item("CMNT_IsChiusa"))
                    End If
                Catch ex As Exception

                End Try

                If IsDBNull(oRow.Item("TPCM_icona")) = False Then
                    img = oRow.Item("TPCM_icona")
                    img = ImageBaseDir & Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
                    oRow.Item("TPCM_icona") = img
                End If

                Try
                    If IsDate(oRow.Item("CMNT_dataInizioIscrizione")) Then
                        If Not Equals(New Date, oRow.Item("CMNT_dataInizioIscrizione")) Then
                            oRow.Item("CMNT_dataInizioIscrizione") = FormatDateTime(oRow.Item("CMNT_dataInizioIscrizione"), DateFormat.GeneralDate)
                        End If
                    End If
                Catch ex As Exception

                End Try
                Try
                    If IsDate(oRow.Item("CMNT_dataFineIscrizione")) Then
                        If Not Equals(New Date, oRow.Item("CMNT_dataFineIscrizione")) Then
                            oRow.Item("CMNT_dataFineIscrizione") = FormatDateTime(oRow.Item("CMNT_dataFineIscrizione"), DateFormat.GeneralDate)
                        End If
                    End If
                Catch ex As Exception

                End Try

                If IsDBNull(oRow.Item("CMNT_AnnoAccademico")) Then
                    oRow.Item("AnnoAccademico") = "&nbsp;"
                Else
                    oRow.Item("AnnoAccademico") = oRow.Item("CMNT_AnnoAccademico")
                End If
                If IsDBNull(oRow.Item("PRDO_descrizione")) Then
                    oRow.Item("Periodo") = "&nbsp;"
                Else
                    oRow.Item("Periodo") = oRow.Item("PRDO_descrizione")
                End If

                Try
                    oRow.Item("CMNT_Esteso") = oRow.Item("CMNT_Nome")
                    oRow.Item("CMNT_EstesoNoSpan") = oRow.Item("CMNT_Nome")

                Catch ex As Exception

                End Try

                Try
                    Dim numIscritti, maxIscritti As Integer
                    maxIscritti = oRow.Item("CMNT_MaxIscritti")
                    numIscritti = oRow.Item("CMNT_Iscritti")
                    Try
                        oRow.Item("Iscritti") = numIscritti
                    Catch ex As Exception
                        oRow.Item("Iscritti") = 0
                        numIscritti = 0
                    End Try

                    If (maxIscritti <= 0) Then
                        oRow.Item("CMNT_Iscritti") = 0
                    Else
                        If numIscritti > maxIscritti Then
                            oRow.Item("CMNT_Iscritti") = maxIscritti - numIscritti
                            oRow.Item("Iscritti") = oResource.getValue("limiti.superato")
                            oRow.Item("Iscritti") = Replace(oRow.Item("Iscritti"), "#num1#", "<b>" & numIscritti & "</b>")
                            oRow.Item("Iscritti") = Replace(oRow.Item("Iscritti"), "#limite#", maxIscritti)
                            oRow.Item("Iscritti") = Replace(oRow.Item("Iscritti"), "#numOver#", numIscritti - maxIscritti)
                        ElseIf numIscritti = maxIscritti Then
                            oRow.Item("CMNT_Iscritti") = -1
                            oRow.Item("Iscritti") = numIscritti & " " & oResource.getValue("limiti") & " <b>" & maxIscritti & "</b>"
                        Else
                            oRow.Item("CMNT_Iscritti") = maxIscritti - numIscritti
                            oRow.Item("Iscritti") = numIscritti & " " & oResource.getValue("limiti") & " <b>" & maxIscritti & "</b>"
                        End If

                        '           Me.DGComunita.Columns(6).Visible = True
                    End If
                Catch ex As Exception

                End Try

                If IsDBNull(oRow.Item("CMNT_dataCreazione")) Then
                    oRow.Item("CMNT_dataCreazioneText") = oResource.getValue("nd") '"n.d"
                Else
                    If Equals(New Date, oRow.Item("CMNT_dataCreazione")) Then
                        oRow.Item("CMNT_dataCreazioneText") = oResource.getValue("nd") '"n.d"
                    Else
                        oRow.Item("CMNT_dataCreazioneText") = CDate(oRow.Item("CMNT_dataCreazione")).ToString("dd/MM/yy HH:mm")
                    End If
                End If
                If IsDBNull(oRow.Item("CMNT_dataCessazione")) Then
                    oRow.Item("CMNT_dataCessazioneText") = oResource.getValue("nd") '"n.d"
                Else
                    If Equals(New Date, oRow.Item("CMNT_dataCessazione")) Then
                        oRow.Item("CMNT_dataCessazioneText") = oResource.getValue("nd") '"n.d"
                    Else
                        oRow.Item("CMNT_dataCessazioneText") = CDate(oRow.Item("CMNT_dataCessazione")).ToString("dd/MM/yy HH:mm")
                    End If
                End If
            Next
        Catch ex As Exception

        End Try
        Return oDataset
    End Function

    Private Sub Bind_Griglia(Optional ByVal ApplicaFiltri As Boolean = False)
        Dim totale As Integer
        Dim oDataset As DataSet

        'carica le comunit� nella datagrid DGComunita
        'se gli passo 0 seleziona tutte
        Me.LBmsgDG.Visible = False
        Me.DGComunita.Visible = True 'se la datagrid era vuota allora era stata nascosta

        Try
            oDataset = Me.Filtraggio_Dati(ApplicaFiltri)
            totale = oDataset.Tables(0).Rows.Count

            If totale > 0 Then
                Dim oDataview As DataView
                oDataview = oDataset.Tables(0).DefaultView
                If ViewState("SortExspression") = "" Then
                    ViewState("SortExspression") = "CMNT_dataCreazione"
                    ViewState("SortDirection") = "desc"
                End If
                oDataview.Sort = ViewState("SortExspression") & " " & ViewState("SortDirection")

                If totale <= Me.DDLNumeroRecord.Items(0).Value Then
                    Me.DDLNumeroRecord.Visible = False
                    Me.LBnumeroRecord_c.Visible = False
                    Me.DGComunita.PagerStyle.Position = PagerPosition.Top
                Else
                    Me.DDLNumeroRecord.Visible = True
                    Me.LBnumeroRecord_c.Visible = True
                    Me.DGComunita.PagerStyle.Position = PagerPosition.TopAndBottom
                End If
                Me.DGComunita.DataSource = oDataview
                Me.DGComunita.DataBind()
            Else
                Me.DDLNumeroRecord.Visible = False
                Me.LBnumeroRecord_c.Visible = False
                Me.DGComunita.Visible = False
                Me.LBmsgDG.Visible = True
                oResource.setLabel_To_Value(LBmsgDG, "LBmsgDG1")
            End If
        Catch ex As Exception
            Me.DDLNumeroRecord.Visible = False
            Me.LBnumeroRecord_c.Visible = False
            Me.DGComunita.Visible = False
            Me.LBmsgDG.Visible = True
            oResource.setLabel_To_Value(LBmsgDG, "LBmsgDG1")
        End Try
    End Sub
#End Region

#Region "Filtro"
    Private Sub LNBapriFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBapriFiltro.Click
        Me.TBRfiltri.Visible = True
        Me.TBRchiudiFiltro.Visible = True
        Me.TBRapriFiltro.Visible = False
        Me.Bind_Griglia()
    End Sub
    Private Sub LNBchiudiFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBchiudiFiltro.Click
        Me.TBRfiltri.Visible = False
        Me.TBRchiudiFiltro.Visible = False
        Me.TBRapriFiltro.Visible = True
        Me.Bind_Griglia()
    End Sub

    Public Sub FiltroLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaltro.Click
        If sender.commandArgument <> "" Then
            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
            Me.ViewState("intAnagrafica") = sender.commandArgument
            sender.CssClass = "lettera_Selezionata"
        Else
            Me.ViewState("intAnagrafica") = -1
            Me.LKBtutti.CssClass = "lettera_Selezionata"
        End If
        Me.ViewState("intCurPage") = 0
        Me.DGComunita.CurrentPageIndex = 0
        Me.Bind_Griglia()
    End Sub
    Private Sub DeselezionaLink(ByVal Lettera As String)
        Lettera = CType(CInt(Lettera), Main.FiltroComunita).ToString

        Dim oLink As System.Web.UI.WebControls.LinkButton
        oLink = Me.FindControlRecursive(Me.Master, "LKB" & Lettera)
        If IsNothing(oLink) = False Then
            oLink.CssClass = "lettera"
        End If
    End Sub
    Private Sub SelezionaLink_All()
        Dim i As Integer
        Try
            For i = Asc("a") To Asc("z") 'status dei link button delle lettere
                Dim oLinkButton As New LinkButton
                oLinkButton = FindControlRecursive(Me.Master, "LKB" & Chr(i))
                Dim Carattere As String = Chr(i)
                If IsNothing(oLinkButton) = False Then
                    oLinkButton.CssClass = "lettera"
                End If
            Next
            Me.LKBaltro.CssClass = "lettera"
            Me.ViewState("intAnagrafica") = -1
            Me.LKBtutti.CssClass = "lettera_Selezionata"
        Catch ex As Exception

        End Try
    End Sub
    Private Sub CBXautoUpdate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXautoUpdate.CheckedChanged
        Me.DDLannoAccademico.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLtipoCorsoDiStudi.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLperiodo.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.DDLresponsabile.AutoPostBack = Me.CBXautoUpdate.Checked
        Me.Bind_Griglia(True)
    End Sub
    Private Sub DDLTipo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLTipo.SelectedIndexChanged
        Me.TBRtipiSpeciali.Visible = False
        Me.TBLcorsi.Visible = False
        Me.TBLcorsiDiStudio.Visible = False
        Me.LBnoCorsi.Visible = False

       
            Me.TBRtipiSpeciali.Visible = False
            Me.LBnoCorsi.Visible = True
            Me.DDLtipoCorsoDiStudi.SelectedIndex = 0
            Me.DDLannoAccademico.SelectedIndex = 0
            Me.DDLperiodo.SelectedIndex = 0


        If Me.CBXautoUpdate.Checked Then
            DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
            DGComunita.CurrentPageIndex = 0
            Me.ViewState("intCurPage") = 0
        End If
        Me.Bind_Responsabili()
        Me.Bind_Griglia()
    End Sub
    Private Sub DDLNumeroRecord_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DDLNumeroRecord.SelectedIndexChanged
        DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
        DGComunita.CurrentPageIndex = 0
        Me.Bind_Griglia()
    End Sub
    Private Sub BTNCerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNCerca.Click
        Me.DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
        Me.DGComunita.CurrentPageIndex = 0
        Me.ViewState("intCurPage") = 0
        Me.Bind_Griglia(True)
    End Sub

    Private Sub DDLorganizzazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganizzazione.SelectedIndexChanged
        DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
        DGComunita.CurrentPageIndex = 0
        Me.Bind_Griglia()
    End Sub
  
    Private Sub RBLstatoComunita_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLstatoComunita.SelectedIndexChanged
        DGComunita.PageSize = DDLNumeroRecord.SelectedItem.Value
        DGComunita.CurrentPageIndex = 0
        Me.Bind_Griglia()
    End Sub
  
#End Region

#Region "Setup Parametri Ricerca"
    Private Sub SaveSearchParameters()
        Try
            Me.Response.Cookies("GestioneComunita")("DDLannoAccademico") = Me.DDLannoAccademico.SelectedValue
            Me.Response.Cookies("GestioneComunita")("DDLNumeroRecord") = Me.DDLNumeroRecord.SelectedValue
            Me.Response.Cookies("GestioneComunita")("DDLorganizzazione") = Me.DDLorganizzazione.SelectedValue
            Me.Response.Cookies("GestioneComunita")("DDLperiodo") = Me.DDLperiodo.SelectedValue
            Me.Response.Cookies("GestioneComunita")("DDLTipo") = Me.DDLTipo.SelectedValue
            Me.Response.Cookies("GestioneComunita")("DDLTipoRicerca") = Me.DDLTipoRicerca.SelectedValue
            Me.Response.Cookies("GestioneComunita")("TXBValore") = Me.TXBValore.Text
            Me.Response.Cookies("GestioneComunita")("intCurPage") = Me.ViewState("intCurPage")
            Me.Response.Cookies("GestioneComunita")("SortDirection") = Me.ViewState("SortDirection")
            Me.Response.Cookies("GestioneComunita")("SortExspression") = Me.ViewState("SortExspression")
            Me.Response.Cookies("GestioneComunita")("DDLtipoCorsoDiStudi") = Me.DDLtipoCorsoDiStudi.SelectedValue
            Me.Response.Cookies("GestioneComunita")("DDLresponsabile") = Me.DDLresponsabile.SelectedValue
        Catch ex As Exception

        End Try
    End Sub
    Private Sub SetupSearchParameters()
        Try
            'Recupero fattori di ricerca relativi all'ordinamento
            Try
                Me.ViewState("SortDirection") = Me.Request.Cookies("GestioneComunita")("SortDirection")
                Me.ViewState("SortExspression") = Me.Request.Cookies("GestioneComunita")("SortExspression")
            Catch ex As Exception

            End Try

            Try
                'Recupero dati relativi alla paginazione corrente
                If IsNumeric(Me.Request.Cookies("GestioneComunita")("intCurPage")) Then
                    Me.ViewState("intCurPage") = CInt(Me.Request.Cookies("GestioneComunita")("intCurPage"))
                    Me.DGComunita.CurrentPageIndex = CInt(Me.ViewState("intCurPage"))
                Else
                    Me.ViewState("intCurPage") = 0
                    Me.DGComunita.CurrentPageIndex = 0
                End If
            Catch ex As Exception
                Me.ViewState("intCurPage") = 0
                Me.DGComunita.CurrentPageIndex = 0
            End Try
            Try
                Me.TXBValore.Text = Me.Request.Cookies("GestioneComunita")("TXBValore")
            Catch ex As Exception
                Me.TXBValore.Text = ""
            End Try

            Try
                Me.DDLorganizzazione.SelectedValue = Me.Request.Cookies("GestioneComunita")("DDLorganizzazione")
            Catch ex As Exception
                Me.Response.Cookies("GestioneComunita")("DDLorganizzazione") = Me.DDLorganizzazione.SelectedValue
            End Try

            ' Setto l'anno accademico
        
            ' Setto l'organizzazione
            Try
                If IsNumeric(Me.Request.Cookies("GestioneComunita")("DDLorganizzazione")) Then
                    Try
                        Me.DDLorganizzazione.SelectedValue = Me.Request.Cookies("GestioneComunita")("DDLorganizzazione")
                    Catch ex As Exception

                    End Try
                End If
            Catch ex As Exception

            End Try

            ' Setto il numero di record
            Try
                If IsNumeric(Me.Request.Cookies("GestioneComunita")("DDLNumeroRecord")) Then
                    Me.DDLNumeroRecord.SelectedValue = Me.Request.Cookies("GestioneComunita")("DDLNumeroRecord")
                End If
            Catch ex As Exception

            End Try

           
            ' Setto il numero di record
            Try
                ' If IsNumeric(Me.Request.Cookies("ListaComunita")("tipo")) Then
                Me.DDLTipo.SelectedValue = Me.Request.Cookies("GestioneComunita")("DDLTipo")

                Me.TBLcorsi.Visible = False
                Me.TBLcorsiDiStudio.Visible = False
                Me.LBnoCorsi.Visible = True
            Catch ex As Exception
                Me.TBLcorsi.Visible = False
                Me.TBLcorsiDiStudio.Visible = False
                Me.LBnoCorsi.Visible = True
            End Try

            ' Setto il tipo di ricerca
            Try
                If IsNumeric(Me.Request.Cookies("GestioneComunita")("DDLTipoRicerca")) Then
                    Me.DDLTipoRicerca.SelectedValue = Me.Request.Cookies("GestioneComunita")("DDLTipoRicerca")
                End If
            Catch ex As Exception

            End Try

            Try
                Me.Bind_Responsabili(Me.Request.Cookies("GestioneComunita")("DDLresponsabile"))
            Catch ex As Exception
                Me.Bind_Responsabili()
            End Try

        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Gestione Griglia"
    Private Sub SortElenco(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DGComunita.SortCommand
        Dim oSortExpression, oSortDirection As String
        oSortExpression = ViewState("SortExspression")
        oSortDirection = ViewState("SortDirection")
        ViewState("SortExspression") = e.SortExpression

        If e.SortExpression = oSortExpression Then

            If ViewState("SortDirection") = "asc" Then
                ViewState("SortDirection") = "desc"
            Else
                ViewState("SortDirection") = "asc"
            End If
        Else
            ViewState("SortDirection") = "asc"
        End If
        Me.Bind_Griglia()
    End Sub
    Sub DGComunita_pageindexchanged(ByVal obj As Object, ByVal e As DataGridPageChangedEventArgs) Handles DGComunita.PageIndexChanged
        DGComunita.CurrentPageIndex = e.NewPageIndex
        Me.Bind_Griglia()
        DGComunita.DataBind()
    End Sub
    Private Sub DGComunita_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGComunita.ItemCreated
        Dim i As Integer

        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If

        If e.Item.ItemType = ListItemType.Header Then
            Dim oSortExspression, oSortDirection, oText, StringaMouse As String
            oSortExspression = ViewState("SortExspression")
            oSortDirection = ViewState("SortDirection")


            For i = 0 To sender.columns.count - 1
                If sender.columns(i).SortExpression <> "" Then
                    Dim oWebControl As WebControl
                    Dim oCell As New TableCell
                    Dim oLabelAfter As New System.Web.UI.WebControls.Label
                    Dim oLabelBefore As New System.Web.UI.WebControls.Label

                    oLabelBefore.Font.Name = "webdings"
                    oLabelBefore.Font.Size = FontUnit.XSmall
                    oLabelBefore.Text = "&nbsp;"

                    oCell = e.Item.Cells(i)
                    If Me.DGComunita.Columns(i).SortExpression <> "" Then
                        If oSortExspression = sender.columns(i).SortExpression Then
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                If oSortDirection = "asc" Then
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGComunita, oLinkbutton, FiltroOrdinamento.Decrescente)
                                Else
                                    oResource.setHeaderOrderbyLink_Datagrid(Me.DGComunita, oLinkbutton, FiltroOrdinamento.Crescente)
                                End If
                                oLabelAfter.CssClass = Me.DGComunita.HeaderStyle.CssClass
                                oLabelAfter.Text = oLinkbutton.Text & " "
                                'oLinkbutton.Font.Name = "webdings"
                                'oLinkbutton.Font.Size = FontUnit.XSmall

                                If oSortDirection = "asc" Then
                                    '  oText = "5"
                                    oText = "<img src='./../images/dg/down.gif' id='Image_" & i & "' >"
                                    If oLinkbutton.Attributes.Item("onmouseout") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseout", "document.forms[0].Image_" & i & ".src='./../images/dg/down.gif';return true;")
                                    Else
                                        StringaMouse = oLinkbutton.Attributes.Item("onmouseout")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/down.gif';return true;")
                                        oLinkbutton.Attributes.Item("onmouseout") = StringaMouse
                                    End If
                                    If oLinkbutton.Attributes.Item("onmouseover") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseover", "document.forms[0].Image_" & i & ".src='./../images/dg/down_over.gif';return true;")
                                    Else
                                        StringaMouse = oLinkbutton.Attributes.Item("onmouseover")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/down_over.gif';return true;")
                                        oLinkbutton.Attributes.Item("onmouseover") = StringaMouse
                                    End If
                                Else
                                    '  oText = "6"
                                    oText = "<img src='./../images/dg/up.gif' id='Image_" & i & "' >"
                                    If oLinkbutton.Attributes.Item("onmouseout") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseout", "document.forms[0].Image_" & i & ".src='./../images/dg/up.gif';return true;")
                                    Else
                                        StringaMouse = oLinkbutton.Attributes.Item("onmouseout")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/up.gif';return true;")
                                        oLinkbutton.Attributes.Item("onmouseout") = StringaMouse
                                    End If
                                    If oLinkbutton.Attributes.Item("onmouseover") = "" Then
                                        oLinkbutton.Attributes.Add("onmouseover", "document.forms[0].Image_" & i & ".src='./../images/dg/up_over.gif';return true;")
                                    Else
                                        StringaMouse = oLinkbutton.Attributes.Item("onmouseover")
                                        StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/up_over.gif';return true;")
                                        oLinkbutton.Attributes.Item("onmouseover") = StringaMouse
                                    End If
                                End If
                                oLinkbutton.Text = oText


                                oCell.Controls.AddAt(0, oLabelAfter)
                            Catch ex As Exception
                                oCell.Controls.AddAt(0, oLabelAfter)
                            End Try
                        Else
                            Try
                                oWebControl = oCell.Controls(0)
                                Dim oLinkbutton As LinkButton
                                oLinkbutton = oWebControl
                                oLinkbutton.CssClass = "ROW_HeaderLink_Small"

                                oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                                oResource.setHeaderOrderbyLink_Datagrid(Me.DGComunita, oLinkbutton, FiltroOrdinamento.Crescente)
                                oLabelAfter.CssClass = Me.DGComunita.HeaderStyle.CssClass
                                oLabelAfter.Text = oLinkbutton.Text & " "
                                'oLinkbutton.Font.Name = "webdings"
                                'oLinkbutton.Font.Size = FontUnit.XSmall
                                oLinkbutton.Text = "<img src='./../images/dg/up.gif' id='Image_" & i & "' >"
                                If oLinkbutton.Attributes.Item("onmouseout") = "" Then
                                    oLinkbutton.Attributes.Add("onmouseout", "document.forms[0].Image_" & i & ".src='./../images/dg/up.gif';return true;")
                                Else
                                    StringaMouse = oLinkbutton.Attributes.Item("onmouseout")
                                    StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/up.gif';return true;")
                                    oLinkbutton.Attributes.Item("onmouseout") = StringaMouse
                                End If
                                If oLinkbutton.Attributes.Item("onmouseover") = "" Then
                                    oLinkbutton.Attributes.Add("onmouseover", "document.forms[0].Image_" & i & ".src='./../images/dg/up_over.gif';return true;")
                                Else
                                    StringaMouse = oLinkbutton.Attributes.Item("onmouseover")
                                    StringaMouse = StringaMouse.Replace("return true;", "document.forms[0].Image_" & i & ".src='./../images/dg/up_over.gif';return true;")
                                    oLinkbutton.Attributes.Item("onmouseover") = StringaMouse
                                End If

                                oCell.Controls.AddAt(0, oLabelAfter)
                            Catch ex As Exception
                                oCell.Controls.AddAt(0, oLabelAfter)
                            End Try
                        End If
                    End If
                End If
            Next
        End If
        If e.Item.ItemType = ListItemType.Pager Then
            Dim oCell As TableCell
            Dim n, num As Integer
            oCell = CType(e.Item.Controls(0), TableCell)

            n = oCell.ColumnSpan
            ' Aggiungo riga con descrizione:

            Try
                Dim oRow As TableRow
                Dim oTableCell As New TableCell
                oRow = oCell.Parent()

                num = 3
                If Me.DGComunita.Columns(6).Visible = True Then
                    num += 1
                End If
                oTableCell.Controls.Add(Me.CreaLegenda)
                oTableCell.ColumnSpan = num
                oTableCell.HorizontalAlign = HorizontalAlign.Left
                oCell.ColumnSpan = 1
                oRow.Cells.AddAt(0, oTableCell)

                e.Item.Cells(0).Attributes.Item("colspan") = num.ToString
            Catch ex As Exception

            End Try

            For n = 0 To oCell.Controls.Count - 1 Step 2
                Dim szLnk As String
                szLnk = "System.Web.UI.WebControls.DataGridLinkButton"
                Dim oWebControl As WebControl

                oWebControl = oCell.Controls(n)

                If (oWebControl.GetType().ToString() = szLnk) Then
                    oWebControl.CssClass = "ROW_PagerLink_Small"
                End If
                Try
                    Dim oLabel As Label
                    oLabel = oWebControl
                    oLabel.Text = oLabel.Text
                    oLabel.CssClass = "ROW_PagerSpan_Small"
                Catch ex As Exception
                    Dim oLinkbutton As LinkButton
                    oLinkbutton = oWebControl
                    oLinkbutton.CssClass = "ROW_PagerLink_Small"
                    oResource.setPageDatagrid(Me.DGComunita, oLinkbutton)
                End Try
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim cssLink As String = "ROW_ItemLink_Small"
            Dim cssRiga As String = "ROW_TD_Small"
            Dim isBloccata As Boolean = False
            Try
                If CBool(e.Item.DataItem("CMNT_Bloccata")) = True Then
                    e.Item.CssClass = "ROW_Disattivate_Small"
                    cssLink = "ROW_ItemLinkDisattivate_Small"
                    cssRiga = "ROW_ItemDisattivate_Small"
                    isBloccata = True
                ElseIf CBool(e.Item.DataItem("CMNT_Archiviata")) = True Then
                    e.Item.CssClass = "ROW_Disabilitate_Small"
                ElseIf e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.CssClass = "ROW_Alternate_Small"
                Else
                    e.Item.CssClass = "ROW_Normal_Small"
                End If
            Catch ex As Exception
                If e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.CssClass = "ROW_Alternate_Small"
                End If
            End Try

            Dim oCell As New TableCell
            Dim PRSN_ID As Integer
            Dim oPersona As New COL_Persona
            Dim oServizio As New UCServices.Services_AmministraComunita

            If Me.ViewState("PermessiAssociati") = "" Then
                oServizio.PermessiAssociati = Me.GetPermessiForPage(Services_AmministraComunita.Codex)
            Else
                oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
            End If

            Try
                oPersona = Session("objPersona")
                PRSN_ID = oPersona.ID
            Catch ex As Exception
                PRSN_ID = -9
            End Try

            oCell = CType(e.Item.Cells(0), TableCell)

            Try
                Dim oImage_Dettagli As ImageButton
                oImage_Dettagli = oCell.FindControl("IMGDettagli")
                oImage_Dettagli.ToolTip = "Dettagli"

            Catch ex As Exception

            End Try

            Try
                Dim oTBRnome As System.Web.UI.WebControls.TableRow
                oTBRnome = e.Item.Cells(2).FindControl("TBRnome")

                If IsNothing(oTBRnome) = False And cssRiga <> "" Then
                    oTBRnome.CssClass = cssRiga
                End If

            Catch ex As Exception

            End Try
            Try
                Dim oTBCnome As System.Web.UI.WebControls.TableRow
                oTBCnome = e.Item.Cells(2).FindControl("TBCnome")

                If IsNothing(oTBCnome) = False And cssRiga <> "" Then
                    oTBCnome.CssClass = cssRiga
                End If

            Catch ex As Exception

            End Try

            Try
                Dim oIMGisChiusa As System.Web.UI.WebControls.Image
                oIMGisChiusa = e.Item.Cells(2).FindControl("IMGisChiusa")

                If IsNothing(oIMGisChiusa) = False Then
                    Dim ImageBaseDir As String
                    ImageBaseDir = GetPercorsoApplicazione(Me.Request)
                    ImageBaseDir = ImageBaseDir & "/RadControls/TreeView/Skins/Comunita/"

                    oIMGisChiusa.Visible = True
                    oIMGisChiusa.ImageUrl = ImageBaseDir & oResource.getValue("stato.image." & e.Item.DataItem("CMNT_isChiusa"))
                    oIMGisChiusa.AlternateText = oResource.getValue("stato." & e.Item.DataItem("CMNT_isChiusa"))
                End If

            Catch ex As Exception

            End Try
            Try
                Dim oImage_Modifica As ImageButton
                oImage_Modifica = oCell.FindControl("IMGEdit")
                'oImage_Modifica.ToolTip = "Modifica"
                oResource.setImageButton_Datagrid(Me.DGComunita, oImage_Modifica, "IMGEdit", True, True, True, False)
                If oServizio.Change Or oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And e.Item.DataItem("CMNT_PRSN_ID") = PRSN_ID) Then
                    oImage_Modifica.Visible = True
                Else
                    oImage_Modifica.Visible = False
                End If
            Catch ex As Exception

            End Try

            Try
                Dim oLBresponsabili As Label
                oLBresponsabili = oCell.FindControl("LBresponsabili")
                If Not IsNothing(oLBresponsabili) Then
                    Dim Creatore, Responsabile As String

                    If Not IsDBNull(e.Item.DataItem("AnagraficaCreatore")) Then
                        Creatore = e.Item.DataItem("AnagraficaCreatore")
                    End If
                    If Not IsDBNull(e.Item.DataItem("CMNT_Responsabile")) Then
                        Responsabile = e.Item.DataItem("CMNT_Responsabile")
                    End If
                    If Creatore <> "" Or Responsabile <> "" Then
                        If Responsabile = "" Then
                            oLBresponsabili.Text = Me.oResource.getValue("LBresponsabili.creatore")

                        ElseIf Creatore = "" Then
                            oLBresponsabili.Text = Me.oResource.getValue("LBresponsabili.responsabile")
                        Else
                            oLBresponsabili.Text = Me.oResource.getValue("LBresponsabili.resp_creatore")
                        End If

                        oLBresponsabili.Text = Replace(oLBresponsabili.Text, "#creatore#", Creatore)
                        oLBresponsabili.Text = Replace(oLBresponsabili.Text, "#responsabile#", Responsabile)
                        oLBresponsabili.CssClass = cssRiga
                    End If
                End If
            Catch ex As Exception

            End Try
            'Try
            '    Dim oImage_AssociaPadre As ImageButton
            '    oImage_AssociaPadre = oCell.FindControl("IMGAssocia")
            '    oImage_AssociaPadre.ToolTip = "Rendi figlia di altre comunit�"

            '    If oServizio.Change Or oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And e.Item.DataItem("CMNT_PRSN_ID") = PRSN_ID) Then
            '        oImage_AssociaPadre.Visible = True
            '    Else
            '        oImage_AssociaPadre.Visible = False
            '    End If
            'Catch ex As Exception

            'End Try

            Try
                Dim oImage_Elimina As ImageButton
                oImage_Elimina = oCell.FindControl("IMGDelete")

                If oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And e.Item.DataItem("CMNT_PRSN_ID") = PRSN_ID) Or oServizio.Delete Then
                    oImage_Elimina.Visible = Not isBloccata
                Else
                    oImage_Elimina.Visible = False
                End If

                If oPersona.TipoPersona.ID = Main.TipoPersonaStandard.SysAdmin Then
                    'If e.Item.DataItem("CMNT_isDiretto") Then 'CMNT_isFiglioDiretto
                    '    oResource.setImageButton_Datagrid(Me.DGComunita, oImage_Elimina, "IMGDelete1", True, True, True, True)
                    '    ' oImage_Elimina.Attributes.Add("onclick", "window.status='Elimina la comunit� dal database.';return confirm(' Sei sicuro di eliminare la comunit� selezionata ?');")
                    '    'oImage_Elimina.ToolTip = "Elimina"
                    'Else
                    '    oResource.setImageButton_Datagrid(Me.DGComunita, oImage_Elimina, "IMGDelete2", True, True, True, True)
                    '    'oImage_Elimina.Attributes.Add("onclick", "window.status='Elimina il link alla comunit� selezionata.';return confirm(' Sei sicuro di eliminare il link alla comunit� selezionata ?');")
                    '    'oImage_Elimina.ToolTip = "Elimina"
                    'End If
                    If e.Item.DataItem("ALCM_HasFigli") Then
                        oResource.setImageButton_Datagrid(Me.DGComunita, oImage_Elimina, "IMGDelete2", True, True, True, True)
                        'oImage_Elimina.Attributes.Add("onclick", "alert('Funzione in fase di realizzazione.');return false;")
                    Else
                        oResource.setImageButton_Datagrid(Me.DGComunita, oImage_Elimina, "IMGDelete2", True, True, True, True)
                        ' oImage_Elimina.Attributes.Add("onclick", "alert('Funzione in fase di realizzazione.');return false;")
                    End If
                Else
                    If e.Item.DataItem("ALCM_HasFigli") Then
                        '  oResource.setImageButton_Datagrid(Me.DGComunita, oImage_Elimina, "IMGDelete3", True, True, True, True)
                        oImage_Elimina.Attributes.Add("onclick", "alert('Funzione in fase di realizzazione.');return false;")
                    Else
                        oResource.setImageButton_Datagrid(Me.DGComunita, oImage_Elimina, "IMGDelete2", True, True, True, True)
                        ' oImage_Elimina.Attributes.Add("onclick", "alert('Funzione in fase di realizzazione.');return false;")
                    End If

                End If

                'If e.Item.DataItem("CMNT_isFiglioDiretto") Then
                '    oImage_Elimina.Attributes.Add("onclick", "window.status='Elimina la comunit� dal database.';return confirm(' Sei sicuro di eliminare la comunit� selezionata ?');")
                '    oImage_Elimina.ToolTip = "Elimina"
                'Else
                '    oImage_Elimina.Attributes.Add("onclick", "window.status='Elimina il link alla comunit� selezionata.';return confirm(' Sei sicuro di eliminare il link alla comunit� selezionata ?');")
                '    oImage_Elimina.ToolTip = "Elimina"
                'End If

            Catch ex As Exception

            End Try

            Try
                Dim oLNBentra As LinkButton
                Dim oLNBiscrivi As LinkButton

                oLNBentra = e.Item.Cells(2).FindControl("LNBentra")
                oLNBiscrivi = e.Item.Cells(2).FindControl("LNBiscrivi")
                If IsNothing(oLNBentra) = False And IsNothing(oLNBiscrivi) = False Then
                    oResource.setLinkButton_Datagrid(Me.DGComunita, oLNBentra, "accedi", True, True)
                    oResource.setLinkButton_Datagrid(Me.DGComunita, oLNBiscrivi, "iscrivi", True, True)

                    If IsDBNull(e.Item.DataItem("RLPC_TPRL_ID")) Then
                        oLNBentra.Visible = False
                        oLNBiscrivi.Visible = True
                    Else
                        If e.Item.DataItem("RLPC_TPRL_ID") > 0 Then
                            oLNBentra.Visible = True
                            oLNBiscrivi.Visible = False
                        Else
                            oLNBentra.Visible = False
                            oLNBiscrivi.Visible = True
                        End If
                    End If

                    oLNBentra.CssClass = cssLink & "_bold"
                    oLNBiscrivi.CssClass = cssLink & "_bold"
                End If
            Catch ex As Exception

            End Try

            Try
                Dim oLNBblocca, oLNBsblocca, oLNBarchivia, oLNBdeArchivia As LinkButton
                oLNBblocca = e.Item.Cells(2).FindControl("LNBblocca")
                oLNBsblocca = e.Item.Cells(2).FindControl("LNBsblocca")
                oLNBarchivia = e.Item.Cells(2).FindControl("LNBarchivia")
                oLNBdeArchivia = e.Item.Cells(2).FindControl("LNBdeArchivia")
                If IsNothing(oLNBblocca) = False Then
                    oResource.setLinkButton_Datagrid(Me.DGComunita, oLNBblocca, "LNBblocca", True, True, True)
                    oResource.setLinkButton_Datagrid(Me.DGComunita, oLNBsblocca, "LNBsblocca", True, True, True)
                    oResource.setLinkButton_Datagrid(Me.DGComunita, oLNBarchivia, "LNBarchivia", True, True, True)
                    oResource.setLinkButton_Datagrid(Me.DGComunita, oLNBdeArchivia, "LNBdeArchivia", True, True, True)

                    oLNBblocca.Visible = False
                    oLNBsblocca.Visible = False
                    oLNBarchivia.Visible = False
                    oLNBdeArchivia.Visible = False
                    If oServizio.Change Or oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And e.Item.DataItem("CMNT_PRSN_ID") = PRSN_ID) Then
                        If e.Item.DataItem("CMNT_Bloccata") Then
                            oLNBsblocca.Visible = True
                        ElseIf e.Item.DataItem("CMNT_Archiviata") Then
                            oLNBdeArchivia.Visible = True
                        Else
                            oLNBarchivia.Visible = True
                            oLNBblocca.Visible = True
                        End If
                    End If
                    oLNBblocca.CssClass = cssLink
                    oLNBsblocca.CssClass = cssLink
                    oLNBarchivia.CssClass = cssLink
                    oLNBdeArchivia.CssClass = cssLink
                End If
            Catch ex As Exception

            End Try

            Try
                Dim oLNBassociaPadri As LinkButton
                oLNBassociaPadri = e.Item.Cells(2).FindControl("LNBassociaPadri")
                If IsNothing(oLNBassociaPadri) = False Then
                    oResource.setLinkButton_Datagrid(Me.DGComunita, oLNBassociaPadri, "LNBassociaPadri", True, True)
                    If oServizio.Change Or oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And e.Item.DataItem("CMNT_PRSN_ID") = PRSN_ID) Then
                        oLNBassociaPadri.Enabled = True
                    Else
                        oLNBassociaPadri.Enabled = False
                    End If
                    oLNBassociaPadri.CssClass = cssLink
                End If
            Catch ex As Exception

            End Try

            Try
                Dim oLNBservizi As LinkButton
                oLNBservizi = e.Item.Cells(2).FindControl("LNBservizi")
                If IsNothing(oLNBservizi) = False Then
                    oResource.setLinkButton_Datagrid(Me.DGComunita, oLNBservizi, "LNBservizi", True, True)
                    If oServizio.GrantPermission Or oServizio.Admin Or oServizio.Moderate Then
                        oLNBservizi.Enabled = True
                    Else
                        oLNBservizi.Enabled = False
                    End If
                    oLNBservizi.CssClass = cssLink
                End If
            Catch ex As Exception

            End Try

            Try
                Dim oLNBiscritti As LinkButton
                oLNBiscritti = e.Item.Cells(2).FindControl("LNBiscritti")
                If IsNothing(oLNBiscritti) = False Then
                    oResource.setLinkButton_Datagrid(Me.DGComunita, oLNBiscritti, "LNBiscritti", True, True)
                    If oServizio.GrantPermission Or oServizio.Admin Or oServizio.Moderate Then
                        oLNBiscritti.Enabled = True
                    Else
                        oLNBiscritti.Enabled = False
                    End If
                    oLNBiscritti.CssClass = cssLink
                End If
            Catch ex As Exception

            End Try

            Try
                Dim oLNBcoordina As LinkButton
                Dim oLBcoordina As Label
                oLNBcoordina = e.Item.Cells(2).FindControl("LNBcoordina")
                oLBcoordina = e.Item.Cells(2).FindControl("LBcoordina")
                If IsNothing(oLNBcoordina) = False Then

                    Try
                        oLBcoordina.Visible = False
                        oLNBcoordina.Visible = False
                    Catch ex As Exception
                        oLBcoordina.Visible = False
                        oLNBcoordina.Visible = False
                    End Try
                    If oServizio.GrantPermission Or oServizio.Admin Or oServizio.Moderate Then
                        oLNBcoordina.Enabled = True
                    Else
                        oLNBcoordina.Enabled = False
                    End If
                    oLNBcoordina.CssClass = cssLink
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Function CreaLegenda() As Table
        Dim oTable As New Table
        Dim oRow As New TableRow
        Dim oTableCell As New TableCell

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(10)
        oTableCell.CssClass = "ROW_Disattivate_Small"
        oTableCell.Text = "&nbsp;"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        oTableCell.Text = oResource.getValue("bloccate")
        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)


        oTableCell = New TableCell
        oTableCell.Width = System.Web.UI.WebControls.Unit.Pixel(10)
        oTableCell.CssClass = "ROW_Disabilitate_Small"
        oTableCell.Text = "&nbsp;"
        oRow.Cells.Add(oTableCell)

        oTableCell = New TableCell
        oTableCell.Text = oResource.getValue("archiviate")
        oTableCell.CssClass = "ROW_Page_Small"
        oRow.Cells.Add(oTableCell)
        oTable.Rows.Add(oRow)

        Return oTable
    End Function

    Private Sub DGComunita_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGComunita.ItemCommand
        Dim CMNT_ID As Integer
        Dim CMNT_Path As String
        Dim CMNT_Hasfigli, CMNT_isDiretto As Boolean
        Try
            Dim oPersona As New COL_Persona
            oPersona = Session("objPersona")
            CMNT_Path = DGComunita.Items(e.Item.ItemIndex).Cells(9).Text()
            CMNT_Hasfigli = CBool(DGComunita.Items(e.Item.ItemIndex).Cells(14).Text())
            CMNT_ID = CInt(DGComunita.DataKeys.Item(e.Item.ItemIndex))
            Select Case e.CommandName
                Case "cancella"
                    Dim oComunita As New COL_Comunita


                    Try

                        If CMNT_Hasfigli Then
                            Dim CMNT_isChiusaForPadre, CMNT_IsChiusa As Boolean


                            CMNT_IsChiusa = Me.DGComunita.Items(e.Item.ItemIndex).Cells(15).Text()
                            CMNT_isChiusaForPadre = Me.DGComunita.Items(e.Item.ItemIndex).Cells(16).Text()
                            If CMNT_isChiusaForPadre Then
                                CMNT_IsChiusa = True
                            End If

                            Me.HDN_Path.Value = CMNT_Path
                            Me.HDNcmnt_ID.Value = CMNT_ID
                            Me.HDN_nodoSel.Value = CMNT_ID & "," & CMNT_Path & "," & CMNT_IsChiusa & "," & CMNT_Hasfigli & ","
                            Me.Bind_TreeView(True, CMNT_ID)
                            Me.PNLtreeView.Visible = True
                            Me.PNLgriglia.Visible = False
                            Me.PNLdettagli.Visible = False
                            Me.HDNreturnTo.Value = "grid"
                        Else
                            CMNT_isDiretto = CBool(DGComunita.Items(e.Item.ItemIndex).Cells(10).Text())
                            Me.HDN_Path.Value = CMNT_Path
                            Me.HDNcmnt_ID.Value = CMNT_ID
                            Me.ResetForm_ToConfirmDelete(False)
                            If CMNT_isDiretto Then
                                Me.oResource.setLabel(Me.LBconfermaElimina)
                            Else
                                Me.LBconfermaElimina.Text = Me.oResource.getValue("LBconfermaElimina.link")
                            End If
                            Me.LBconfermaElimina.Text = Replace(Me.LBconfermaElimina.Text, "#nomeComunita#", "<b>" & COL_Comunita.EstraiNomeBylingua(CMNT_ID, Session("linguaID")) & "</b>")
                            Me.HDN_idPadre_Link.Value = DGComunita.Items(e.Item.ItemIndex).Cells(11).Text()
                            Me.HDNreturnTo.Value = "grid"
                            Me.HDN_isDiretto.Value = CMNT_isDiretto
                        End If
                    Catch ex As Exception

                    End Try
                    'If CBool(DGComunita.Items(e.Item.ItemIndex).Cells(10).Text) Then

                    '      Me.StartToCancel(CMNT_ID, CMNT_Path)

                    '    'tutta le gestione della cancellazione
                    'Else
                    '    CMNT_PadreId_Link = DGComunita.Items(e.Item.ItemIndex).Cells(11).Text
                    '    'Trattasi di link, posso cancellarlo senza problemi......
                    '    Me.CancellaComunitaLink_Singola(CMNT_ID, CMNT_PadreId_Link)
                    'End If

                Case "modifica"
                    Dim i_link As String

                    Session("AdminForChange") = True
                    Session("CMNT_path_forAdmin") = CMNT_Path
                    Session("idComunita_forAdmin") = CMNT_ID

                    i_link = GetPercorsoApplicazione(Me.Request) & "/comunita/WizardModificaComunita.aspx?fromGestione=true&topage=true"
                    Me.SaveSearchParameters()
                    Response.Redirect(i_link)
                Case "servizi"
                    Me.SaveSearchParameters()
                    Session("AdminForChange") = True
                    Session("CMNT_path_forAdmin") = CMNT_Path
                    Session("idComunita_forAdmin") = CMNT_ID

                    Me.Response.Redirect(GetPercorsoApplicazione(Me.Request) & "/comunita/ManagementServizi.aspx?fromGestione=true&topage=true")
                Case "dettagli"
                    Try
                        Dim TPRL_ID As Integer
                        Me.PNLgriglia.Visible = False
                        Me.PNLdettagli.Visible = True
                        Me.PNLmenuTree.Visible = False
                        Me.PNLmenuPrincipale.Visible = False
                        Me.PNLmenuDettagli.Visible = True
                        Me.CTRLDettagli.SetupDettagliComunita(CMNT_ID)

                        If DGComunita.Items(e.Item.ItemIndex).Cells(13).Text = "" Then
                            TPRL_ID = -1
                        Else
                            TPRL_ID = CInt(DGComunita.Items(e.Item.ItemIndex).Cells(13).Text)
                        End If
                        If TPRL_ID > -1 Then
                            Me.LNBentraDettagli.Visible = True
                            Me.LNBiscriviDettagli.Visible = False
                        Else
                            Me.LNBentraDettagli.Visible = False
                            Me.LNBiscriviDettagli.Visible = True
                        End If
                        Me.HDNcmnt_ID.Value = CMNT_ID
                        Me.HDN_Path.Value = CMNT_Path
                        Me.HDNreturnTo.Value = "grid"
                    Catch ex As Exception
                        Me.PNLgriglia.Visible = True
                        Me.PNLdettagli.Visible = False
                        Me.HDNreturnTo.Value = ""
                        Me.Bind_Griglia()
                    End Try
                Case "associa"
                    Session("azione") = "associa"
                    Session("AdminForChange") = True
                    Session("CMNT_path_forAdmin") = CMNT_Path
                    Session("idComunita_forAdmin") = CMNT_ID
                    Me.SaveSearchParameters()
                    Response.Redirect(GetPercorsoApplicazione(Me.Request) & "/comunita/WizardModificaComunita.aspx?fromGestione=true&topage=true")
                Case "iscritti"
                    Me.SaveSearchParameters()
                    Session("AdminForChange") = True
                    Session("CMNT_path_forAdmin") = CMNT_Path
                    Session("idComunita_forAdmin") = CMNT_ID
                    Response.Redirect(GetPercorsoApplicazione(Me.Request) & "/comunita/GestioneIscritti.aspx?fromGestione=true&topage=true")

                Case "coordina"
                    Me.SaveSearchParameters()
                    Session("AdminForChange") = True
                    Session("CMNT_path_forAdmin") = CMNT_Path
                    Session("idComunita_forAdmin") = CMNT_ID
                    Response.Redirect(GetPercorsoApplicazione(Me.Request) & "/comunita/ComunitaCoordinate.aspx?fromGestione=true&topage=true")
                Case "entra"
                    Me.EntraComunita(CMNT_ID, CMNT_Path)
                Case "iscrivi"
                    Me.HDNcmnt_ID.Value = CMNT_ID
                    Me.HDN_Path.Value = CMNT_Path
                    Me.HDNreturnTo.Value = "grid"
                    Session("Azione") = "iscrivi"
                    Me.Iscrivi_Comunita(CMNT_ID, CMNT_Path, Server.MapPath("./../profili/") & oPersona.ID & "\", "./../")

                Case "blocca"
                    COL_Comunita.BloccaMi(CMNT_ID)
                    Me.Bind_Griglia()
                Case "sblocca"
                    COL_Comunita.SbloccaMi(CMNT_ID)
                    Me.Bind_Griglia()
                Case "archivia"
                    COL_Comunita.ArchiviaMi(CMNT_ID)
                    Me.Bind_Griglia()
                Case "deArchivia"
                    COL_Comunita.DeArchiviaMi(CMNT_ID)
                    Me.Bind_Griglia()
                Case Else
                    Me.Bind_Griglia()
            End Select
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Setup Pannelli"
    Private Sub ResetForm_All()
        Me.PNLmenuPrincipale.Visible = False
        Me.PNLmenuTree.Visible = False
        Me.PNLmenuDettagli.Visible = False
        Me.PNLmenuCancella.Visible = False
        Me.PNLdettagli.Visible = False
        Me.PNLgriglia.Visible = False
        Me.PNLtreeView.Visible = False
        Me.PNLconfermaElimina.Visible = False
        Me.PNLdettagliElimina.Visible = False
    End Sub
    Private Sub ResetForm_ToConfirmDelete(ByVal isTree As Boolean)
        Me.ResetForm_All()
        Me.PNLmenuCancella.Visible = True
        Me.PNLtreeView.Visible = isTree
        Me.PNLconfermaElimina.Visible = Not isTree
    End Sub
    Private Sub ResetForm_ToTreeView()
        Me.ResetForm_All()
        Me.PNLmenuTree.Visible = True
        Me.PNLtreeView.Visible = True
    End Sub
    Private Sub ResetForm_ToElenco()
        Me.ResetForm_All()
        Me.PNLmenuPrincipale.Visible = True
        Me.PNLgriglia.Visible = True
    End Sub
#End Region

#Region "Gestione TreeView"
    Private Sub Bind_TreeView(Optional ByVal forConferma As Boolean = True, Optional ByVal checkID As Integer = -1)
        Dim oPersona As New COL_Persona
        Dim oComunita As New COL_Comunita
        Dim oDataset As New DataSet

        'Dim iCMNT_ID As Integer
        'Dim iCMNT_Path, elenco(), Start_Path As String

        Me.RDTcomunita.Nodes.Clear()

        Dim ImageBaseDir As String
        ImageBaseDir = GetPercorsoApplicazione(Me.Request)
        ImageBaseDir = ImageBaseDir & Me.RDTcomunita.ImagesBaseDir().Replace("~", "")

        Try
            Dim totaleHistory, ComunitaPadreID, ComunitaDaEliminareID As Integer
            Dim Path, ArrComunita(,) As String

            oPersona = Session("objPersona")
            If forConferma Then
                '  CMNT_ID = Me.HDNcmnt_ID.Value
                ComunitaPadreID = Session("IdComunita")
                oComunita.Id = ComunitaPadreID
                Me.RDTcomunita.CheckBoxes = True
                Me.ResetForm_ToConfirmDelete(True)
            Else
                ComunitaPadreID = Session("IdComunita")
                Me.HDNcmnt_ID.Value = ComunitaPadreID
                oComunita.Id = ComunitaPadreID

                Me.RDTcomunita.CheckBoxes = False
            End If
            ComunitaDaEliminareID = Me.HDNcmnt_ID.Value
            oComunita.Estrai()

            Dim nodeRoot As New RadTreeNode
            nodeRoot.Text = oResource.getValue("ElencoComunita") '"Elenco Comunit�"

            nodeRoot.Expanded = True
            nodeRoot.ImageUrl = "folder.gif"
            nodeRoot.Value = ""
            nodeRoot.ToolTip = oResource.getValue("ElencoComunita") '"Elenco Comunit�"
            nodeRoot.Category = 0
            nodeRoot.ContextMenuName = "Base"
            nodeRoot.Checkable = False
            Me.RDTcomunita.Nodes.Add(nodeRoot)

            If Session("limbo") = False Then
                Path = "."
                'Recupero il percorso della comunit� attuale, per trovarmi i suoi figli !
                totaleHistory = -1
                If IsArray(Session("ArrComunita")) Then
                    Try
                        ArrComunita = Session("ArrComunita")
                        totaleHistory = UBound(ArrComunita, 2)

                        Path = ArrComunita(2, totaleHistory)
                    Catch ex As Exception

                    End Try
                End If
                '  oDataset = oComunita.ElencaComunita_ForAdmin(Session("LinguaID"), 0, Me.DGComunita.PageSize, Me.DDLorganizzazione.SelectedValue, CMNT_ID, oPersona.Id, -1, Main.FiltroOrdinamento.Crescente, Main.FiltroCampoOrdineComunita.Livello, , "", , , , Main.ElencoRecord.AdAlbero, False)
                oDataset = oComunita.RicercaComunitaAlberoForManagement(Session("LinguaID"), -1, Path, oComunita.Id, Session("objPersona").id)
            End If

            If oDataset.Tables(0).Rows.Count = 0 Then
                ' nessuna comunit� a cui si � iscritti
                Me.GeneraNoNode()
            Else

                Dim oDataview As DataView
                oDataview = oDataset.Tables(0).DefaultView
                Dim o As DataRowView = oDataview.Item(0)

                oDataview.RowFilter = "CMNT_ID=" & ComunitaDaEliminareID ' CMNT_ID


                Dim i As Integer
                If oDataset.Tables(0).Columns.Contains("CMNT_Iscritti") = False Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Iscritti"))
                End If
                For i = 0 To oDataview.Count - 1
                    If oDataview.Item(i).Item("ALCM_PadreVirtuale_ID") = 0 Or oDataview.Item(i).Item("CMNT_id") = ComunitaDaEliminareID Then '0 ComunitaPadreID Then
                        Dim node As RadTreeNode = Me.CreaNodo(oDataview.Item(i), True, forConferma, checkID)

                        If nodeRoot.Checked Then
                            node.Checked = True
                        End If
                        nodeRoot.Nodes.Add(node)
                        Path = oDataview.Item(i).Item("ALCM_Path")
                        oDataview.RowFilter = "ALCM_PadreVirtuale_ID=" & ComunitaDaEliminareID ' CMNT_ID
                        Me.RicorsioneNodi(Path, oDataview, node, forConferma, checkID)
                    End If
                Next

                'If oDataview.Count > 0 Then

                'End If

                'If oDataset.Tables(0).Columns.Contains("CMNT_idPadre_Link") = False Then
                '    oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Iscritti"))
                'End If

                ''End If
                'If oDataset.Tables(0).Columns.Contains("CMNT_Iscritti") = False Then
                '    oDataset.Tables(0).Columns.Add(New DataColumn("CMNT_Iscritti"))
                'End If
                ''oDataset.Relations.Add("NodeRelation", oDataset.Tables(0).Columns("CMNT_ID"), oDataset.Tables(0).Columns("CMNT_idPadre_Link"), False)
                'oDataset.Relations.Add("NodeRelation", oDataset.Tables(0).Columns("CMNT_Path"), oDataset.Tables(0).Columns("CMNT_PathPadre"), False)
                'Dim dbRow As DataRow
                'For Each dbRow In oDataset.Tables(0).Rows
                '    'If forConferma Then
                '    '    If dbRow("CMNT_Path") = Me.HDN_Path.Value Then
                '    '        Dim node As RadTreeNode = CreateNode(dbRow, True, forConferma, checkID)
                '    '        If nodeRoot.Checked Then
                '    '            node.Checked = True
                '    '            node.Enabled = nodeRoot.Enabled
                '    '        End If
                '    '        nodeRoot.Nodes.Add(node)
                '    '        RecursivelyPopulate(dbRow, node, forConferma, checkID)
                '    '    End If
                '    'Else
                '    If dbRow("CMNT_idPadre") = 0 Or dbRow("CMNT_id") = CMNT_ID Then
                '        Dim node As RadTreeNode = CreateNode(dbRow, True, forConferma, checkID)
                '        If nodeRoot.Checked Then
                '            node.Checked = True
                '            '       node.Enabled = False 'nodeRoot.Enabled
                '        End If
                '        nodeRoot.Nodes.Add(node)
                '        RecursivelyPopulate(dbRow, node, forConferma, checkID)
                '    End If
                '    'End If
                'Next dbRow

                If checkID > 0 Then
                    Dim oNode As Telerik.WebControls.RadTreeNode
                    oNode = Me.RDTcomunita.FindNodeByValue(Me.HDN_nodoSel.Value)
                    If Not IsNothing(oNode) Then
                        oNode.ExpandParentNodes()
                        If forConferma Then
                            oNode = Me.RDTcomunita.FindNodeByValue(Me.HDN_nodoSel.Value)
                            oNode.Expanded = True
                            oNode.ExpandChildNodes()
                        End If
                    End If
                End If
                Me.PNLtreeView.Visible = True
            End If
        Catch ex As Exception
            Me.GeneraNoNode()
        End Try
    End Sub
    Private Sub RicorsioneNodi(ByVal Path As String, ByVal oDataView As DataView, ByVal node As RadTreeNode, ByVal forConferma As Boolean, ByVal checkID As Integer)
        Dim OldFiltro As String
        Dim newPath As String
        OldFiltro = oDataView.RowFilter

        oDataView.RowFilter = "ALCM_RealPath <> ALCM_Path AND ALCM_RealPath='" & Path & "'"
        If oDataView.Count > 0 Then
            Dim i As Integer

            For i = 0 To oDataView.Count - 1
                Dim childNode As RadTreeNode = Me.CreaNodo(oDataView.Item(i), False, forConferma, checkID)

                If node.Checked Then
                    childNode.Checked = True
                End If
                node.Nodes.Add(childNode)
                newPath = oDataView.Item(i).Item("ALCM_Path")
                oDataView.RowFilter = "ALCM_RealPath <> ALCM_Path AND ALCM_RealPath='" & newPath & "'"
                Me.RicorsioneNodi(newPath, oDataView, childNode, forConferma, checkID)
                oDataView.RowFilter = "ALCM_RealPath <> ALCM_Path AND ALCM_RealPath='" & Path & "'"
            Next
        End If
        oDataView.RowFilter = OldFiltro
    End Sub
    Private Function CreaNodo(ByVal dbRow As DataRowView, ByVal expanded As Boolean, ByVal forConferma As Boolean, ByVal checkID As Integer) As RadTreeNode
        Dim node As New RadTreeNode
        Dim oPersona As New COL_Persona
        'Dim start As Integer
        'Dim continue As Boolean = False
        'Dim numIscritti, maxIscritti, iscritti As Integer
        'start = 0

        'Dim CMNT_id, RLPC_TPRL_id, CMNT_TPCM_id, PRSN_ID As Integer
        'Dim CMNT_Responsabile, img As String
        'Dim CMNT_isIscritto, CMNT_IsChiusa As Boolean
        'Dim CMNT_Nome, CMNT_NomeVisibile, CMNT_REALpath, CMNT_path As String

        Dim oServizio As New UCServices.Services_AmministraComunita
        Dim forDetails, forDelete, forEntra, forIscrivi, forModifica, forServizi, forPadri, forUtenti, forCoordina, forArchivia, forBlocca, forSblocca, forDeArchivia As Boolean

        forDetails = False
        forEntra = False
        forIscrivi = False
        forModifica = False
        forServizi = False
        forPadri = False
        forUtenti = False
        forCoordina = False
        forArchivia = False
        forBlocca = False
        forSblocca = False
        forDeArchivia = False
        forDelete = False

        oPersona = Session("objPersona")
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If
        If Me.ViewState("PermessiAssociati") = "" Then
            oServizio.PermessiAssociati = Me.GetPermessiForPage(UCServices.Services_AmministraComunita.Codex)
        Else
            oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
        End If
        'PRSN_ID = oPersona.Id

        Dim ComunitaID, TipoRuoloID, TipoComunitaID, PersonaID As Integer
        Dim isIscritto As Boolean = False
        Dim ischiusa As Boolean
        Dim Responsabile As String = ""
        Dim Nome, NomeVisibile, Path, PathPadre As String
        Try
            Dim ImageBaseDir, img As String
            ImageBaseDir = GetPercorsoApplicazione(Me.Request)
            ImageBaseDir = ImageBaseDir & Me.RDTcomunita.ImagesBaseDir().Replace("~", "")
            ComunitaID = dbRow.Item("CMNT_id")
            PersonaID = oPersona.ID

            TipoComunitaID = dbRow.Item("CMNT_TPCM_id")
            If IsDBNull(dbRow.Item("RLPC_TPRL_id")) Then
                TipoRuoloID = -1
            Else
                TipoRuoloID = dbRow.Item("RLPC_TPRL_id")
            End If

            'TROVO IL RESPONSABILE
            If IsDBNull(dbRow.Item("CMNT_Responsabile")) Then
                Responsabile = oResource.getValue("creata")
                Responsabile = Responsabile.Replace("#%%#", dbRow.Item("AnagraficaCreatore"))
            Else
                If dbRow.Item("CMNT_Responsabile") = "" Then
                    Responsabile = oResource.getValue("creata")
                    Responsabile = Responsabile.Replace("#%%#", dbRow.Item("AnagraficaCreatore"))
                Else
                    Responsabile = dbRow.Item("CMNT_Responsabile")
                End If
            End If
            If IsDBNull(dbRow.Item("TPCM_icona")) Then
                img = ""
            Else
                img = dbRow.Item("TPCM_icona")
                img = "./logo/" & Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
                '   img = ImageBaseDir & img
            End If
            If TipoRuoloID > 0 Then
                isIscritto = True
            End If



            Nome = dbRow.Item("CMNT_Nome")
            NomeVisibile = Nome
            ischiusa = dbRow.Item("CMNT_IsChiusa")
            If dbRow.Item("ALCM_isChiusaForPadre") = True Then
                ischiusa = True
            End If

            If ComunitaID > 0 Then
                Nome = Nome & Responsabile
                NomeVisibile = Nome
                'If ischiusa Then
                '    Nome = Nome & Me.GenerateImage(ImageBaseDir & "lucchetto_closed.gif", "Comunit� chiusa")
                'Else
                '    Nome = Nome & Me.GenerateImage(ImageBaseDir & "lucchetto_open.gif", "Comunit� aperta")
                'End If
                Nome = Nome & Me.GenerateImage(ImageBaseDir & oResource.getValue("stato.image." & ischiusa), oResource.getValue("stato." & ischiusa))
                If IsDBNull(dbRow.Item("CMNT_AnnoAccademico")) = False Then
                    If dbRow.Item("CMNT_AnnoAccademico") <> "" Then
                        Nome = Nome & "&nbsp;(" & dbRow.Item("CMNT_AnnoAccademico") & ")&nbsp;"
                    End If
                End If
            Else
                NomeVisibile = Nome
            End If
            Path = dbRow.Item("ALCM_path")
            PathPadre = dbRow.Item("ALCM_REALpath")
            'End If
            If ComunitaID > 0 And forConferma = False Then
                Dim dataStringa As String = ""
                If Not (isIscritto And TipoRuoloID <> -2 And TipoRuoloID <> -3) Then
                    Dim CMNT_dataInizioIscrizione, CMNT_dataFineIscrizione As DateTime

                    If dbRow.Item("CMNT_Iscritti") = 0 Or dbRow.Item("CMNT_Iscritti") > 0 Then
                        If IsDate(dbRow.Item("CMNT_dataInizioIscrizione")) Then
                            CMNT_dataInizioIscrizione = dbRow.Item("CMNT_dataInizioIscrizione")
                            If CMNT_dataInizioIscrizione > Now Then
                                ' devo iscrivermi, ma iscrizioni non aperte !
                                Nome = Nome & "&nbsp;&nbsp;" & oResource.getValue("iscrizioni." & Iscrizioni_code.IscrizioniChiuse)
                            Else
                                If IsDate(dbRow.Item("CMNT_dataFineIscrizione")) Then
                                    CMNT_dataFineIscrizione = dbRow.Item("CMNT_dataFineIscrizione")
                                    If CMNT_dataFineIscrizione < Now Then
                                        dataStringa = oResource.getValue("iscrizioni." & Iscrizioni_code.IscrizioniEntro)
                                        dataStringa = dataStringa.Replace("#%%#", CMNT_dataFineIscrizione)
                                        Nome = Nome & "&nbsp;&nbsp;" & dataStringa
                                    End If
                                End If
                            End If
                        End If
                    ElseIf TipoRuoloID <> -2 Then
                        Nome = Nome & "&nbsp;&nbsp;" & oResource.getValue("iscrizioni." & Iscrizioni_code.IscrizioniComplete)
                    End If
                End If
            End If

            If dbRow.Item("CMNT_Bloccata") Then
                Nome = Nome & "&nbsp;" & Me.oResource.getValue("status.Bloccata")
            ElseIf dbRow.Item("CMNT_Archiviata") Then
                Nome = Nome & "&nbsp;" & Me.oResource.getValue("status.Archiviata")
            End If

            node.Text = Nome
            node.Value = ComunitaID & "," & Path & "," & ischiusa & "," & dbRow.Item("ALCM_HasFigli") & "," & dbRow.Item("ALCM_isDiretto")
            node.Expanded = expanded
            node.ImageUrl = img
            node.ToolTip = NomeVisibile
            node.Category = ComunitaID

            node.Checkable = True
            If dbRow.Item("ALCM_isDiretto") = False Then
                node.CssClass = "TreeNodeDisabled"
                node.Text = node.Text & " - LINK"
            End If
            If ComunitaID > 0 Then
                If TipoRuoloID < 0 Then
                    forEntra = False
                    forIscrivi = True
                Else
                    forEntra = True
                    forIscrivi = False
                End If
                forDetails = True
                forModifica = (oServizio.Change Or oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And dbRow.Item("CMNT_PRSN_ID") = PersonaID))
                forServizi = (oServizio.Change Or oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And dbRow.Item("CMNT_PRSN_ID") = PersonaID))
                forPadri = (oServizio.Change Or oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And dbRow.Item("CMNT_PRSN_ID") = PersonaID))
                forUtenti = (oServizio.Change Or oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And dbRow.Item("CMNT_PRSN_ID") = PersonaID))
              


                If dbRow.Item("CMNT_Bloccata") Then
                    forSblocca = (oServizio.Change Or oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And dbRow.Item("CMNT_PRSN_ID") = PersonaID))
                ElseIf dbRow.Item("CMNT_Archiviata") Then
                    forDeArchivia = (oServizio.Change Or oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And dbRow.Item("CMNT_PRSN_ID") = PersonaID))
                Else
                    forArchivia = (oServizio.Change Or oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And dbRow.Item("CMNT_PRSN_ID") = PersonaID))
                    forBlocca = (oServizio.Change Or oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And dbRow.Item("CMNT_PRSN_ID") = PersonaID))
                End If
            End If
            If ComunitaID = Session("IDcomunita") Then
                node.Checkable = False
                forDelete = False
            Else
                node.Checkable = True
                forDelete = (oServizio.Delete Or oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And dbRow.Item("CMNT_PRSN_ID") = PersonaID))
                forDelete = False
            End If
            If forConferma Then
                If ComunitaID = checkID Then
                    If Path <> Me.HDN_Path.Value Then
                        node.Enabled = False
                    Else
                        Me.HDN_nodoSel.Value = node.Value
                    End If
                    node.Checked = True
                End If
            End If
            Me.CreateContextMenu(node, forDetails, forDelete, forEntra, forIscrivi, forModifica, forServizi, forPadri, forUtenti, forCoordina, forArchivia, forBlocca, forSblocca, forDeArchivia)


        Catch ex As Exception

        End Try


        Return node
    End Function 'CreateNode

    Private Function GenerateImage(ByVal ImageName As String, Optional ByVal Status As String = "") As String
        Dim imageUrl As String
        Dim quote As String
        quote = """"

        imageUrl = "<img  align=absmiddle src=" & quote & ImageName & quote & " alt=" & quote & Status & quote

        imageUrl = imageUrl & " " & " onmouseover=" & quote & "window.status='" & Replace(Status, "'", "\'") & "';return true;" & quote & " "
        imageUrl = imageUrl & " " & " onfocus=" & quote & "window.status='" & Replace(Status, "'", "\'") & "';return true;" & quote & " "
        imageUrl = imageUrl & " " & " onmouseout=" & quote & "window.status='';return true;" & """" & " "
        imageUrl = imageUrl & " >"

        Return imageUrl
    End Function
    Private Function GetIDFromPath(ByVal Tree_Path As String) As String
        Dim oPadri() As String

        If Tree_Path = "" Then
            Return 0
        Else
            If InStr(Tree_Path, ".") > 0 Then
                Tree_Path = Mid(Tree_Path, 2, Tree_Path.Length - 2)
                If InStr(Tree_Path, ".") > 0 Then
                    oPadri = Tree_Path.Split(".")
                    Return oPadri(oPadri.Length - 1)

                Else
                    Return Tree_Path
                End If
            Else
                Return Tree_Path
            End If
        End If
        Return 0
    End Function
    Private Function GetFatherPath(ByVal Tree_Path As String) As String
        Dim stringaPath As String
        Dim pos As Integer

        If Tree_Path = "" Then
            Return ""
        Else
            stringaPath = Mid(Tree_Path, 2, Tree_Path.Length - 2)
            pos = InStrRev(stringaPath, ".")
            If pos > 0 Then
                Return Left(Tree_Path, pos) & "."
            Else
                Return Tree_Path
            End If
        End If
        Return ""
    End Function
    Private Function GetFatherFromPath(ByVal Tree_Path As String) As Integer
        Dim oPadri() As String

        If Tree_Path = "" Then
            Return 0
        Else
            Tree_Path = Mid(Tree_Path, 2, Tree_Path.Length - 2)
            If InStr(Tree_Path, ".") > 0 Then
                oPadri = Tree_Path.Split(".")
                Return oPadri(oPadri.Length - 2)

            Else
                Return Tree_Path
            End If
        End If
        Return 0
    End Function

    'Private Sub RecursivelyPopulate(ByVal dbRow As DataRow, ByVal node As RadTreeNode, ByVal forConferma As Boolean, ByVal checkID As Integer)
    '    Dim childRow As DataRow
    '    For Each childRow In dbRow.GetChildRows("NodeRelation")
    '        Dim childNode As RadTreeNode = CreateNode(childRow, False, forConferma, checkID)
    '        If node.Checked Then
    '            childNode.Checked = True
    '        End If
    '        node.Nodes.Add(childNode)

    '        RecursivelyPopulate(childRow, childNode, forConferma, checkID)
    '    Next childRow
    'End Sub
    'Private Function CreateNode(ByVal dbRow As DataRow, ByVal expanded As Boolean, ByVal forConferma As Boolean, ByVal checkID As Integer) As RadTreeNode
    '    Dim node As New RadTreeNode

    '    Dim start As Integer
    '    Dim continue As Boolean = False
    '    Dim numIscritti, maxIscritti, iscritti As Integer
    '    start = 0

    '    Dim CMNT_id, RLPC_TPRL_id, CMNT_TPCM_id, PRSN_ID As Integer
    '    Dim CMNT_Responsabile, img As String
    '    Dim CMNT_isIscritto, CMNT_IsChiusa As Boolean
    '    Dim CMNT_Nome, CMNT_NomeVisibile, CMNT_REALpath, CMNT_path As String

    '    Dim oServizio As New UCServices.Services_AmministraComunita
    '    Dim oPersona As New COL_Persona
    '    Dim forDetails, forDelete, forEntra, forIscrivi, forModifica, forServizi, forPadri, forUtenti, forCoordina, forArchivia, forBlocca, forSblocca, forDeArchivia As Boolean
    '    forDetails = False
    '    forEntra = False
    '    forIscrivi = False
    '    forModifica = False
    '    forServizi = False
    '    forPadri = False
    '    forUtenti = False
    '    forCoordina = False
    '    forArchivia = False
    '    forBlocca = False
    '    forSblocca = False
    '    forDeArchivia = False
    '    forDelete = False

    '    oPersona = Session("objPersona")
    '    If IsNothing(oResource) Then
    '        SetCulture(Session("LinguaCode"))
    '    End If
    '    If Me.ViewState("PermessiAssociati") = "" Then
    '        oServizio.PermessiAssociati = Me.GetPermessiForPage(oServizio.Codex)
    '    Else
    '        oServizio.PermessiAssociati = Me.ViewState("PermessiAssociati")
    '    End If
    '    PRSN_ID = oPersona.Id
    '    Try
    '        CMNT_id = dbRow.Item("CMNT_id")
    '        CMNT_TPCM_id = dbRow.Item("CMNT_TPCM_id")
    '        If IsDBNull(dbRow.Item("RLPC_TPRL_id")) Then
    '            RLPC_TPRL_id = -1
    '        Else
    '            RLPC_TPRL_id = dbRow.Item("RLPC_TPRL_id")
    '        End If

    '        Dim ImageBaseDir As String
    '        ImageBaseDir = GetPercorsoApplicazione(Me.Request)
    '        ImageBaseDir = ImageBaseDir & Me.RDTcomunita.ImagesBaseDir().Replace("~", "")


    '        'TROVO IL RESPONSABILE
    '        If IsDBNull(dbRow.Item("CMNT_Responsabile")) Then
    '            CMNT_Responsabile = ""
    '            If Not IsDBNull(dbRow.Item("AnagraficaCreatore")) Then
    '                CMNT_Responsabile = " (creata da: " & dbRow.Item("AnagraficaCreatore") & ") "
    '            End If
    '        Else
    '            CMNT_Responsabile = " (" & dbRow.Item("CMNT_Responsabile") & ") "
    '        End If
    '        If IsDBNull(dbRow.Item("TPCM_icona")) Then
    '            img = ""
    '        Else
    '            img = dbRow.Item("TPCM_icona")
    '            img = "./logo/" & Mid(img, InStrRev(img, "/", img.Length - 1) + 1, img.Length)
    '            ' img = ImageBaseDir & img
    '        End If
    '        If IsDBNull(dbRow.Item("CMNT_isIscritto")) Then
    '            CMNT_isIscritto = True
    '        Else
    '            CMNT_isIscritto = dbRow.Item("CMNT_isIscritto")
    '        End If



    '        CMNT_Nome = dbRow.Item("CMNT_Nome")
    '        CMNT_NomeVisibile = CMNT_Nome
    '        CMNT_IsChiusa = dbRow.Item("CMNT_IsChiusa")
    '        If dbRow.Item("CMNT_isChiusaForPadre") = True Then
    '            CMNT_IsChiusa = True
    '        End If

    '        If CMNT_id > 0 Then
    '            CMNT_Nome = CMNT_Nome & CMNT_Responsabile
    '            CMNT_NomeVisibile = CMNT_Nome
    '            If CMNT_IsChiusa Then
    '                CMNT_Nome = CMNT_Nome & Me.GenerateImage(ImageBaseDir & "lucchetto_closed.gif", "Comunit� chiusa")
    '            Else
    '                CMNT_Nome = CMNT_Nome & Me.GenerateImage(ImageBaseDir & "lucchetto_open.gif", "Comunit� aperta")
    '            End If

    '            If dbRow.IsNull("CMNT_AnnoAccademico") = False Then
    '                If dbRow.Item("CMNT_AnnoAccademico") <> "" Then
    '                    CMNT_Nome = CMNT_Nome & "&nbsp;(" & dbRow.Item("CMNT_AnnoAccademico") & ")&nbsp;"
    '                End If
    '            End If
    '        Else
    '            CMNT_NomeVisibile = CMNT_Nome
    '        End If
    '        CMNT_path = dbRow.Item("CMNT_path")
    '        CMNT_REALpath = dbRow.Item("CMNT_REALpath")
    '        'End If
    '        If CMNT_id > 0 Then
    '            If Not (CMNT_isIscritto And RLPC_TPRL_id <> -2 And RLPC_TPRL_id <> -3) Then
    '                Dim CMNT_dataInizioIscrizione, CMNT_dataFineIscrizione As DateTime

    '                If dbRow.Item("CMNT_Iscritti") = 0 Or dbRow.Item("CMNT_Iscritti") > 0 Then
    '                    If IsDate(dbRow.Item("CMNT_dataInizioIscrizione")) Then
    '                        CMNT_dataInizioIscrizione = dbRow.Item("CMNT_dataInizioIscrizione")
    '                        If CMNT_dataInizioIscrizione > Now Then
    '                            ' devo iscrivermi, ma iscrizioni non aperte !
    '                            CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;" & " (iscrizioni aperte il " & CMNT_dataInizioIscrizione & ")"
    '                        Else
    '                            If IsDate(dbRow.Item("CMNT_dataFineIscrizione")) Then
    '                                CMNT_dataFineIscrizione = dbRow.Item("CMNT_dataFineIscrizione")
    '                                If CMNT_dataFineIscrizione < Now Then
    '                                    CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;" & "(Iscrizioni chiuse)"
    '                                End If
    '                            End If
    '                        End If
    '                    End If
    '                ElseIf RLPC_TPRL_id <> -2 Then
    '                    CMNT_Nome = CMNT_Nome & "&nbsp;&nbsp;" & " (n� max iscritti raggiunto)"
    '                End If
    '            End If
    '        End If

    '        node.Text = CMNT_Nome
    '        node.Value = CMNT_id & "," & CMNT_path & "," & CMNT_IsChiusa & "," & dbRow.Item("CMNT_HasFigli") & "," & dbRow.Item("CMNT_isDiretto")
    '        node.Expanded = expanded
    '        node.ImageUrl = img
    '        node.ToolTip = CMNT_NomeVisibile
    '        node.Category = CMNT_id

    '        node.Checkable = True
    '        If dbRow.Item("CMNT_isDiretto") = False Then
    '            node.CssClass = "TreeNodeDisabled"
    '            node.Text = node.Text & " - LINK"
    '        End If


    '        If CMNT_id > 0 Then
    '            If RLPC_TPRL_id < 0 Then
    '                forEntra = False
    '                forIscrivi = True
    '            Else
    '                forEntra = True
    '                forIscrivi = False
    '            End If
    '            forDetails = True
    '            forModifica = (oServizio.Change Or oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And dbRow.Item("CMNT_PRSN_ID") = PRSN_ID))
    '            forServizi = (oServizio.Change Or oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And dbRow.Item("CMNT_PRSN_ID") = PRSN_ID))
    '            forPadri = (oServizio.Change Or oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And dbRow.Item("CMNT_PRSN_ID") = PRSN_ID))
    '            forUtenti = (oServizio.Change Or oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And dbRow.Item("CMNT_PRSN_ID") = PRSN_ID))
    '            If CMNT_TPCM_id = Main.TipoComunitaStandard.Coordinamento Then
    '                forCoordina = (oServizio.Admin Or oServizio.Moderate)
    '            End If


    '            If dbRow.Item("CMNT_Bloccata") Then
    '                forSblocca = (oServizio.Change Or oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And dbRow.Item("CMNT_PRSN_ID") = PRSN_ID))
    '            ElseIf dbRow.Item("CMNT_Archiviata") Then
    '                forDeArchivia = (oServizio.Change Or oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And dbRow.Item("CMNT_PRSN_ID") = PRSN_ID))
    '            Else
    '                forArchivia = (oServizio.Change Or oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And dbRow.Item("CMNT_PRSN_ID") = PRSN_ID))
    '                forBlocca = (oServizio.Change Or oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And dbRow.Item("CMNT_PRSN_ID") = PRSN_ID))
    '            End If
    '        End If
    '        If CMNT_id = Session("IDcomunita") Then
    '            node.Checkable = False
    '            forDelete = False
    '        Else
    '            node.Checkable = True
    '            forDelete = (oServizio.Delete Or oServizio.Admin Or oServizio.Moderate Or (oServizio.CreateComunity And dbRow.Item("CMNT_PRSN_ID") = PRSN_ID))
    '            forDelete = False
    '        End If
    '        If forConferma Then
    '            If CMNT_id = checkID Then
    '                If CMNT_path <> Me.HDN_Path.Value Then
    '                    node.Enabled = False
    '                Else
    '                    Me.HDN_nodoSel.Value = node.Value
    '                End If
    '                node.Checked = True
    '            End If
    '        End If
    '        Me.CreateContextMenu(node, forDetails, forDelete, forEntra, forIscrivi, forModifica, forServizi, forPadri, forUtenti, forCoordina, forArchivia, forBlocca, forSblocca, forDeArchivia)


    '    Catch ex As Exception

    '    End Try


    '    Return node
    'End Function 'CreateNode

    Private Sub GeneraNoNode()
        Dim oRootNode As New RadTreeNode
        Dim oNode As New RadTreeNode

        oRootNode = New RadTreeNode
        oRootNode.Text = oResource.getValue("Comunita")  '"Comunit�: "
        oRootNode.Value = ""
        oRootNode.Expanded = True
        oRootNode.ImageUrl = "folder.gif"
        oRootNode.ToolTip = oResource.getValue("iscritti") '"Elenco comunit� a cui si � iscritti"
        oRootNode.Category = 0
        oRootNode.ContextMenuName = "Base"

        oNode = New RadTreeNode
        oNode.Expanded = True
        oNode.Text = oResource.getValue("noSottoCmnt")  '"Non vi � alcuna sotto-comunit�"
        oNode.Value = ""
        oNode.ToolTip = oResource.getValue("noSottoCmnt")  '"Non vi � alcuna sotto-comunit�"
        oNode.Category = 0
        oNode.Checkable = False
        oRootNode.Nodes.Add(oNode)

        Me.RDTcomunita.Nodes.Clear()
        Me.RDTcomunita.Nodes.Add(oRootNode)
    End Sub
    Private Sub RDTcomunita_NodeContextClick(ByVal o As Object, ByVal e As Telerik.WebControls.RadTreeNodeEventArgs) Handles RDTcomunita.NodeContextClick
        Dim isChiusa, isIscritto, CMNT_Hasfigli As Boolean
        Dim CMNT_ID As Integer
        Dim iCMNT_PAth, Elenco() As String
        Dim oNode As Telerik.WebControls.RadTreeNode
        oNode = e.NodeClicked

        Try
            Dim oRuoloComunita As New COL_RuoloPersonaComunita
            Dim oPersona As New COL_Persona
            oPersona = Session("objPersona")

            Elenco = oNode.Value.Split(",")
            isIscritto = False

            CMNT_ID = oNode.Category
            isChiusa = CBool(Elenco(2))
            oRuoloComunita.EstraiByLinguaDefault(CMNT_ID, Session("objPersona").id)
            If oRuoloComunita.Errore = Errori_Db.None Then
                If oRuoloComunita.TipoRuolo.Id > 0 Then
                    isIscritto = True
                End If
            End If

            iCMNT_PAth = Elenco(1)
            CMNT_Hasfigli = Elenco(3)
            Select Case e.ContextMenuItemID
                Case AzioneTree.Aggiorna
                    Me.Bind_TreeView(Me.RDTcomunita.CheckBoxes)
                Case AzioneTree.Dettagli
                    Try
                        Dim TPRL_ID As Integer
                        Me.PNLtreeView.Visible = False
                        Me.PNLdettagli.Visible = True
                        Me.PNLgriglia.Visible = False
                        Me.PNLmenuTree.Visible = False
                        Me.PNLmenuPrincipale.Visible = False
                        Me.PNLmenuDettagli.Visible = True
                        Me.CTRLDettagli.SetupDettagliComunita(CMNT_ID)

                        If isIscritto Then
                            TPRL_ID = 1
                        Else
                            TPRL_ID = -1
                        End If

                        If TPRL_ID > -1 Then
                            Me.LNBentraDettagli.Visible = True
                            Me.LNBiscriviDettagli.Visible = False
                        Else
                            Me.LNBentraDettagli.Visible = False
                            Me.LNBiscriviDettagli.Visible = True
                        End If
                        Me.HDNcmnt_ID.Value = CMNT_ID
                        Me.HDN_Path.Value = iCMNT_PAth
                        Me.HDNreturnTo.Value = "tree"
                    Catch ex As Exception
                        Me.PNLtreeView.Visible = True
                        Me.PNLdettagli.Visible = False
                        Me.HDNreturnTo.Value = ""
                    End Try
                Case AzioneTree.Entra
                    Me.HDN_Path.Value = iCMNT_PAth
                    Session("Azione") = "entra"
                    Me.HDNreturnTo.Value = "tree"
                    Me.EntraComunita(CMNT_ID, iCMNT_PAth)
                Case AzioneTree.Iscrivi
                    Me.HDN_Path.Value = iCMNT_PAth
                    Me.HDNreturnTo.Value = "tree"
                    Session("Azione") = "iscrivi"
                    Me.Iscrivi_Comunita(CMNT_ID, iCMNT_PAth, Server.MapPath("./../profili/") & oPersona.ID & "\", "./../")
                Case AzioneTree.GestioneServizi
                    'Session("idComunita_forAdmin") = CMNT_ID
                    Session("AdminForChange") = True
                    Session("CMNT_path_forAdmin") = iCMNT_PAth
                    Session("idComunita_forAdmin") = CMNT_ID

                    Me.Response.Redirect(GetPercorsoApplicazione(Me.Request) & "/comunita/ManagementServizi.aspx?fromGestione=true&toTree=true")
                Case AzioneTree.Modifica
                    Dim i_link As String

                    Session("AdminForChange") = True
                    Session("CMNT_path_forAdmin") = iCMNT_PAth
                    Session("idComunita_forAdmin") = CMNT_ID

                    i_link = GetPercorsoApplicazione(Me.Request) & "/comunita/WizardModificaComunita.aspx?fromGestione=true&toTree=true"
                    Me.SaveSearchParameters()
                    Response.Redirect(i_link)
                Case AzioneTree.AssociaA
                    Session("azione") = "associa"
                    Me.SaveSearchParameters()
                    Session("AdminForChange") = True
                    Session("CMNT_path_forAdmin") = iCMNT_PAth
                    Session("idComunita_forAdmin") = CMNT_ID
                    Response.Redirect(GetPercorsoApplicazione(Me.Request) & "/comunita/WizardModificaComunita.aspx?fromGestione=true&toTree=true")
                Case AzioneTree.Coordina
                    Me.SaveSearchParameters()
                    Session("AdminForChange") = True
                    Session("CMNT_path_forAdmin") = iCMNT_PAth
                    Session("idComunita_forAdmin") = CMNT_ID
                    Response.Redirect(GetPercorsoApplicazione(Me.Request) & "/comunita/ComunitaCoordinate.aspx?fromGestione=true&toTree=true")
                Case AzioneTree.GestioneUtenti
                    Session("AdminForChange") = True
                    Session("CMNT_path_forAdmin") = iCMNT_PAth
                    Session("idComunita_forAdmin") = CMNT_ID
                    Response.Redirect(GetPercorsoApplicazione(Me.Request) & "/comunita/GestioneIscritti.aspx?fromGestione=true&topage=true")
                Case AzioneTree.Cancella
                    'Dim oComunita As New COL_Comunita
                    'Dim CMNT_isDiretto As Boolean = False

                    'Try
                    '    Dim ElencoIDcomunita() As String
                    '    Dim CMNT_idPadre_Link, pos As Integer

                    '    oComunita.Id = CMNT_ID
                    '    oComunita.Estrai()
                    '    If oNode.Parent.Category > 0 Then
                    '        If oComunita.IdPadre = oNode.Parent.Category Then
                    '            CMNT_isDiretto = True
                    '        End If
                    '        CMNT_idPadre_Link = oNode.Parent.Category
                    '    Else
                    '        If InStr(iCMNT_PAth, "." & CMNT_ID & ".") = 0 Then
                    '            CMNT_idPadre_Link = 0
                    '        Else
                    '            ElencoIDcomunita = iCMNT_PAth.Split(".")
                    '            CMNT_idPadre_Link = CInt(ElencoIDcomunita(ElencoIDcomunita.Length - 3))
                    '        End If
                    '        If oComunita.IdPadre = CMNT_idPadre_Link Then
                    '            CMNT_isDiretto = True
                    '        End If
                    '    End If
                    '    Me.HDNreturnTo.Value = "tree"

                    '    Me.HDN_nodoPadreSel.Value = oNode.Parent.Value
                    '    Me.HDN_nodoSel.Value = oNode.Value
                    '    If CMNT_Hasfigli Then
                    '        Me.HDN_Path.Value = iCMNT_PAth
                    '        Me.HDNcmnt_ID.Value = CMNT_ID
                    '        Me.Bind_TreeView(True, CMNT_ID)
                    '        Me.PNLtreeView.Visible = True
                    '        Me.PNLgriglia.Visible = False
                    '        Me.PNLdettagli.Visible = False
                    '    Else
                    '        Me.HDN_Path.Value = iCMNT_PAth
                    '        Me.HDNcmnt_ID.Value = CMNT_ID
                    '        Me.PNLtreeView.Visible = False
                    '        Me.PNLgriglia.Visible = False
                    '        Me.PNLdettagli.Visible = False
                    '        Me.PNLconfermaElimina.Visible = True
                    '        If CMNT_isDiretto Then
                    '            Me.LBconfermaElimina.Text = "ATTENZIONE: si desidera procedere con la cancellazione della comunit� [" & oComunita.EstraiNome(CMNT_ID) & "] ?<br>"
                    '            Me.LBconfermaElimina.Text = Me.LBconfermaElimina.Text & "Tutti i file, i messaggi, gli eventi, gli iscritti alla comunit� da eliminare ANDRANNO PERDUTI.<br>"
                    '        Else
                    '            Me.LBconfermaElimina.Text = "ATTENZIONE: si desidera procedere con la cancellazione del link comunit� [" & oComunita.EstraiNome(CMNT_ID) & "] ?<br>"
                    '        End If
                    '        Me.HDN_isDiretto.Value = CMNT_isDiretto
                    '        Me.HDN_idPadre_Link.Value = CMNT_idPadre_Link
                    '    End If
                    'Catch ex As Exception

                    'End Try
                Case AzioneTree.Blocca
                    COL_Comunita.BloccaMi(CMNT_ID)
                    Me.Bind_TreeView()
                Case AzioneTree.Sblocca
                    COL_Comunita.SbloccaMi(CMNT_ID)
                    Me.Bind_TreeView()
                Case AzioneTree.Archivia
                    COL_Comunita.ArchiviaMi(CMNT_ID)
                    Me.Bind_TreeView()
                Case AzioneTree.DeArchivia
                    COL_Comunita.DeArchiviaMi(CMNT_ID)
                    Me.Bind_TreeView()
            End Select
        Catch ex As Exception

        End Try
    End Sub


    Private Sub CreateContextMenu(ByVal childNode As RadTreeNode, ByVal forDetails As Boolean, ByVal forDelete As Boolean, ByVal forEntra As Boolean, ByVal forIscrivi As Boolean, ByVal forModifica As Boolean, ByVal forServizi As Boolean, ByVal forPadri As Boolean, ByVal forUtenti As Boolean, ByVal forCoordina As Boolean, ByVal forArchivia As Boolean, ByVal forBlocca As Boolean, ByVal forSblocca As Boolean, ByVal forDeArchivia As Boolean)
        Dim contextMenus As New ArrayList
        Dim nodeMenu As New Telerik.WebControls.RadTreeViewContextMenu.ContextMenu

        Dim NomeContextMenu As String = "_"

        Dim iMenuDettagli As RadTreeViewContextMenu.ContextMenuItem = Nothing
        Dim iMenuDelete As RadTreeViewContextMenu.ContextMenuItem = Nothing
        Dim iMenuEntra As RadTreeViewContextMenu.ContextMenuItem = Nothing
        Dim iMenuIscrivi As RadTreeViewContextMenu.ContextMenuItem = Nothing
        Dim iMenuModifica As RadTreeViewContextMenu.ContextMenuItem = Nothing
        Dim iMenuUtenti As RadTreeViewContextMenu.ContextMenuItem = Nothing
        Dim iMenuPadri As RadTreeViewContextMenu.ContextMenuItem = Nothing
        Dim iMenuServizi As RadTreeViewContextMenu.ContextMenuItem = Nothing
        Dim iMenuCoordina As RadTreeViewContextMenu.ContextMenuItem = Nothing

        Dim iMenuArchiva As RadTreeViewContextMenu.ContextMenuItem = Nothing
        Dim iMenuBlocca As RadTreeViewContextMenu.ContextMenuItem = Nothing
        Dim iMenuSblocca As RadTreeViewContextMenu.ContextMenuItem = Nothing
        Dim iMenuDeArchivia As RadTreeViewContextMenu.ContextMenuItem = Nothing


        If forDetails Then
            iMenuDettagli = New RadTreeViewContextMenu.ContextMenuItem
            iMenuDettagli.Image = "./images/12.gif"
            iMenuDettagli.PostBack = True
            iMenuDettagli.ID = AzioneTree.Dettagli
            iMenuDettagli.Text = oResource.getValue("menu." & AzioneTree.Dettagli)
            NomeContextMenu = NomeContextMenu & "1_"
        End If
        If forEntra Then
            iMenuEntra = New RadTreeViewContextMenu.ContextMenuItem
            iMenuEntra.Image = "./images/0.gif"
            iMenuEntra.PostBack = True
            iMenuEntra.ID = AzioneTree.Entra
            iMenuEntra.Text = oResource.getValue("menu." & AzioneTree.Entra)
            NomeContextMenu = NomeContextMenu & "3_"
        End If
        If forIscrivi Then
            iMenuIscrivi = New RadTreeViewContextMenu.ContextMenuItem
            iMenuIscrivi.Image = "./images/4.gif"
            iMenuIscrivi.PostBack = True
            iMenuIscrivi.ID = AzioneTree.Iscrivi
            iMenuIscrivi.Text = oResource.getValue("menu." & AzioneTree.Iscrivi)
            NomeContextMenu = NomeContextMenu & "4_"
        End If
        If forModifica Then
            iMenuModifica = New RadTreeViewContextMenu.ContextMenuItem
            iMenuModifica.Image = "./images/11.gif"
            iMenuModifica.PostBack = True
            iMenuModifica.ID = AzioneTree.Modifica
            iMenuModifica.Text = oResource.getValue("menu." & AzioneTree.Modifica)
            NomeContextMenu = NomeContextMenu & "5_"
        End If
        If forUtenti Then
            iMenuUtenti = New RadTreeViewContextMenu.ContextMenuItem
            iMenuUtenti.Image = "./images/15.gif"
            iMenuUtenti.PostBack = True
            iMenuUtenti.ID = AzioneTree.GestioneUtenti
            iMenuUtenti.Text = oResource.getValue("menu." & AzioneTree.GestioneUtenti)
            NomeContextMenu = NomeContextMenu & "6_"
        End If
        If forPadri Then
            iMenuPadri = New RadTreeViewContextMenu.ContextMenuItem
            iMenuPadri.Image = "./images/8.gif"
            iMenuPadri.PostBack = True
            iMenuPadri.ID = AzioneTree.AssociaA
            iMenuPadri.Text = oResource.getValue("menu." & AzioneTree.AssociaA)
            NomeContextMenu = NomeContextMenu & "7_"
        End If
        If forServizi Then
            iMenuServizi = New RadTreeViewContextMenu.ContextMenuItem
            iMenuServizi.Image = "./images/16.gif"
            iMenuServizi.PostBack = True
            iMenuServizi.ID = AzioneTree.GestioneServizi
            iMenuServizi.Text = oResource.getValue("menu." & AzioneTree.GestioneServizi)
            NomeContextMenu = NomeContextMenu & "8_"
        End If
        If forCoordina Then
            iMenuCoordina = New RadTreeViewContextMenu.ContextMenuItem
            iMenuCoordina.Image = "./images/16.gif"
            iMenuCoordina.PostBack = True
            iMenuCoordina.ID = AzioneTree.Coordina
            iMenuCoordina.Text = oResource.getValue("menu." & AzioneTree.Coordina)
            NomeContextMenu = NomeContextMenu & "9_"
        End If

        If forArchivia Then
            iMenuArchiva = New RadTreeViewContextMenu.ContextMenuItem
            iMenuArchiva.Image = "./images/0.gif"
            iMenuArchiva.PostBack = True
            iMenuArchiva.ID = AzioneTree.Archivia
            iMenuArchiva.Text = oResource.getValue("menu." & AzioneTree.Archivia)
            NomeContextMenu = NomeContextMenu & "10_"
        End If
        If forBlocca Then
            iMenuBlocca = New RadTreeViewContextMenu.ContextMenuItem
            iMenuBlocca.Image = "./images/0.gif"
            iMenuBlocca.PostBack = True
            iMenuBlocca.ID = AzioneTree.Blocca
            iMenuBlocca.Text = oResource.getValue("menu." & AzioneTree.Blocca)
            NomeContextMenu = NomeContextMenu & "11_"
        End If
        If forDeArchivia Then
            iMenuSblocca = New RadTreeViewContextMenu.ContextMenuItem
            iMenuSblocca.Image = "./images/0.gif"
            iMenuSblocca.PostBack = True
            iMenuSblocca.ID = AzioneTree.DeArchivia
            iMenuSblocca.Text = oResource.getValue("menu." & AzioneTree.DeArchivia)
            NomeContextMenu = NomeContextMenu & "12_"
        End If
        If forSblocca Then
            iMenuDeArchivia = New RadTreeViewContextMenu.ContextMenuItem
            iMenuDeArchivia.Image = "./images/0.gif"
            iMenuDeArchivia.PostBack = True
            iMenuDeArchivia.ID = AzioneTree.Sblocca
            iMenuDeArchivia.Text = oResource.getValue("menu." & AzioneTree.Sblocca)
            NomeContextMenu = NomeContextMenu & "14_"
        End If

        If forDelete Then
            iMenuDelete = New RadTreeViewContextMenu.ContextMenuItem
            iMenuDelete.Image = "./images/7.gif"
            iMenuDelete.PostBack = True
            iMenuDelete.ID = AzioneTree.Cancella
            iMenuDelete.Text = oResource.getValue("menu." & AzioneTree.Cancella)
            NomeContextMenu = NomeContextMenu & "15_"
        End If
        If forDetails Then
            nodeMenu.Items.Add(iMenuDettagli)
        End If
        If Not IsNothing(iMenuEntra) Then
            nodeMenu.Items.Add(iMenuEntra)
        End If
        If Not IsNothing(iMenuIscrivi) Then
            nodeMenu.Items.Add(iMenuIscrivi)
        End If

        If forDetails Or forEntra Or forIscrivi Then
            Dim iMenuBlanck As New RadTreeViewContextMenu.ContextMenuItem
            iMenuBlanck.Text = ""
            iMenuBlanck.Image = ""
            iMenuBlanck.PostBack = False
            nodeMenu.Items.Add(iMenuBlanck)
        End If

        If forModifica Then
            nodeMenu.Items.Add(iMenuModifica)
        End If
        If forUtenti Then
            nodeMenu.Items.Add(iMenuUtenti)
        End If
        If forPadri Then
            nodeMenu.Items.Add(iMenuPadri)
        End If
        If forServizi Then
            nodeMenu.Items.Add(iMenuServizi)
        End If
        If forCoordina Then
            nodeMenu.Items.Add(iMenuCoordina)
        End If

        Dim showSeparatore As Boolean = False
        Dim showSeparatore1 As Boolean = False
        Dim showSeparatore2 As Boolean = False
        showSeparatore = forDetails Or forEntra Or forIscrivi
        showSeparatore1 = forModifica Or forUtenti Or forPadri Or forServizi Or forCoordina
        showSeparatore2 = forArchivia Or forBlocca Or forDeArchivia Or forSblocca

        If showSeparatore And showSeparatore1 And showSeparatore2 Then
            Dim iMenuBlanck As New RadTreeViewContextMenu.ContextMenuItem
            iMenuBlanck.Text = ""
            iMenuBlanck.Image = ""
            iMenuBlanck.PostBack = False
            nodeMenu.Items.Add(iMenuBlanck)
        End If
        If forArchivia Then
            nodeMenu.Items.Add(iMenuArchiva)
        End If
        If forBlocca Then
            nodeMenu.Items.Add(iMenuBlocca)
        End If
        If forDeArchivia Then
            nodeMenu.Items.Add(iMenuSblocca)
        End If
        If forSblocca Then
            nodeMenu.Items.Add(iMenuDeArchivia)
        End If

        If showSeparatore And showSeparatore1 And showSeparatore2 Then
            Dim iMenuBlanck As New RadTreeViewContextMenu.ContextMenuItem
            iMenuBlanck.Text = ""
            iMenuBlanck.Image = ""
            iMenuBlanck.PostBack = False
            nodeMenu.Items.Add(iMenuBlanck)
        End If
        If forDelete Then
            nodeMenu.Items.Add(iMenuDelete)
        End If
        contextMenus = Me.RDTcomunita.ContextMenus
        If contextMenus.Count = 0 Then
            nodeMenu.Name = NomeContextMenu
            contextMenus.Add(nodeMenu)
        Else
            Dim i, totale As Integer
            Dim found As Boolean = False
            totale = contextMenus.Count - 1
            For i = 0 To contextMenus.Count - 1
                If contextMenus.Item(i).Name = NomeContextMenu Then
                    found = True
                    Exit For
                End If
            Next
            If Not found Then
                nodeMenu.Name = NomeContextMenu
                contextMenus.Add(nodeMenu)
            End If
        End If
        childNode.ContextMenuName = NomeContextMenu
        Me.RDTcomunita.ContextMenus = contextMenus
    End Sub
#End Region

    Private Sub LNBtreeView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBtreeView.Click
        Me.ResetForm_ToTreeView()
        Me.Bind_TreeView(False)
    End Sub
    Private Sub LNBelenco_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBelenco.Click
        Me.ResetForm_ToElenco()
        Me.Bind_Griglia()
    End Sub



    Private Sub LKBcrea_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBcrea.Click
        Response.Redirect(GetPercorsoApplicazione(Me.Request) & "/comunita/WizardCreaComunita.aspx?fromGestione=true")
    End Sub

#Region "Dettagli Comunit�"
    Private Sub LNBentraDettagli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBentraDettagli.Click
        Try
            Me.EntraComunita(Me.HDNcmnt_ID.Value, Me.HDN_Path.Value)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub LNBiscriviDettagli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBiscriviDettagli.Click
        Dim oPersona As New COL_Persona
        oPersona = Session("objPersona")

        Session("Azione") = "iscrivi"
        Me.HDNreturnTo.Value = "dettagli"
        Me.Iscrivi_Comunita(Me.HDNcmnt_ID.Value, Me.HDN_Path.Value, Server.MapPath("./../profili/") & oPersona.ID & "\", "./../")
    End Sub
    Private Sub LNBnascondiDettagli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBnascondiDettagli.Click
        Try
            Me.HDN_Path.Value = ""
            If Me.HDNreturnTo.Value = "grid" Then
                Me.Bind_Griglia()
                Me.ResetForm_ToElenco()
            Else
                Me.ResetForm_ToTreeView()
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Gestione Azioni"
    Private Sub Iscrivi_Comunita(ByVal ComunitaId As Integer, ByVal ComunitaPath As String, ByVal PercorsoProfilo As String, ByVal PercorsoIcone As String)
        Dim oPersona As New COL_Persona
        Dim iResponse As ErroriIscrizioneComunita = ErroriIscrizioneComunita.ErroreGenerico
        Dim alertMSG As String = ""

        oPersona = Session("objPersona")
        iResponse = oPersona.IscrizioneComunitaAsAdmin(ComunitaId, ComunitaPath, PercorsoProfilo, PercorsoIcone)
        lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(oPersona.ID))

        alertMSG = oResource.getValue("ErroriIscrizioneComunita." & CType(iResponse, ErroriIscrizioneComunita))
        If alertMSG <> "" Then
            alertMSG = alertMSG.Replace("'", "\'")
            Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
        End If

        Me.PNLmenuTree.Visible = False
        Me.PNLmenuPrincipale.Visible = False
        Me.PNLmenuDettagli.Visible = False
        Me.PNLdettagli.Visible = False
        Me.PNLtreeView.Visible = False
        Me.PNLgriglia.Visible = False
        If Me.HDNreturnTo.Value = "grid" Then
            Me.PNLgriglia.Visible = True
            Me.PNLmenuPrincipale.Visible = True
            Me.Bind_Griglia()
        ElseIf Me.HDNreturnTo.Value = "dettagli" Then
            Me.PNLdettagli.Visible = True
            Me.PNLmenuDettagli.Visible = True
            If iResponse = Main.ErroriIscrizioneComunita.IscrizioneAvvenuta Then
                Me.LNBentraDettagli.Visible = True
                Me.LNBiscriviDettagli.Visible = False
            Else
                Me.LNBentraDettagli.Visible = False
                Me.LNBiscriviDettagli.Visible = True
            End If
        ElseIf Me.HDNreturnTo.Value = "tree" Then
            Me.PNLtreeView.Visible = True
            Me.PNLmenuTree.Visible = True
            Me.Bind_TreeView()
        Else
            Me.PNLgriglia.Visible = True
            Me.PNLmenuPrincipale.Visible = True
            Me.Bind_Griglia()
        End If
        Session("Azione") = "loaded"
    End Sub

    'Private Sub Iscrivi_Comunita(ByVal CMNT_Path As String)
    '    Dim oComunita As New COL_Comunita
    '    Dim oPersona As New COL_Persona
    '    Dim oRuolo As New COL_RuoloPersonaComunita
    '    Dim PRSN_ID, TPRL_ID, CMNT_ID As Integer
    '    Dim Elenco() As String
    '    Dim isIscritto As Boolean = False
    '    Dim alertMSG As String
    '    Elenco = CMNT_Path.Split(".")
    '    CMNT_ID = Elenco(UBound(Elenco) - 1)

    '    If Session("Azione") = "iscrivi" Then
    '        Try
    '            Dim oTreeComunita As New COL_TreeComunita

    '            oComunita.Id = CMNT_ID
    '            oPersona = Session("objPersona")
    '            PRSN_ID = oPersona.Id
    '            TPRL_ID = oComunita.RuoloDefault()

    '            ' se esiste gi�  e sono il creatore, mi iscrive come admin.....

    '            oRuolo.EstraiByLinguaDefault(CMNT_ID, PRSN_ID)
    '            If oRuolo.Errore = Errori_Db.None Then
    '                'If oRuolo.TipoRuolo.Id = -2 Then
    '                '    ' Se � il creatore...
    '                '    TPRL_ID = CType(Main.TipoRuoloStandard.AdminComunit�, Main.TipoRuoloStandard)
    '                '    oRuolo.TipoRuolo.Id = TPRL_ID
    '                '    oRuolo.Modifica()
    '                'ElseIf oRuolo.TipoRuolo.Id = -3 Then
    '                '    'TRATTASI DI PASSANTE....
    '                '    oRuolo.TipoRuolo.Id = TPRL_ID
    '                '    oRuolo.Modifica()
    '                'End If
    '                oRuolo.TipoRuolo.Id = CType(Main.TipoRuoloStandard.AdminComunit�, Main.TipoRuoloStandard)
    '                oRuolo.Modifica()
    '                If oRuolo.Errore = Errori_Db.None Then
    '                    oRuolo.EstraiByLingua(CMNT_ID, PRSN_ID, Session("LinguaID"))
    '                    oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PRSN_ID & "\"
    '                    oTreeComunita.Nome = PRSN_ID & ".xml"

    '                    oTreeComunita.CambiaRuolo(CMNT_ID, TPRL_ID, oRuolo.TipoRuolo.Nome, oRuolo.Attivato, oRuolo.Abilitato, oRuolo.isResponsabile)
    '                    isIscritto = True

    '                    alertMSG = oResource.getValue("messaggio0")
    '                    If alertMSG <> "" Then
    '                        alertMSG = alertMSG.Replace("'", "\'")
    '                    End If
    '                    Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
    '                Else
    '                    '"Spiacenti, Si � verificato un errore nella registrazione."
    '                    alertMSG = oResource.getValue("messaggio2")
    '                    If alertMSG <> "" Then
    '                        alertMSG = alertMSG.Replace("'", "\'")
    '                    End If
    '                    Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
    '                End If

    '            Else
    '                'Non sono iscritto.....
    '                Dim responso, i As Integer
    '                Dim Ricezione_SMS, Attivato, Responsabile, Abilitato, isAttivato As Boolean
    '                Dim ORGN_ID As Integer
    '                oComunita.Estrai()
    '                oComunita.TipoComunita.Icona = "./../" & oComunita.TipoComunita.Icona
    '                'RuoloPersonaComunita.Abilitato = 1 'sempre a 1! si viene disabilitati solo per comportamento scorretto
    '                'confronto l'id dell'organizzazione di appartenenza della comunit� con quello dell'organizzazione di default della persona che si sta registrando
    '                ORGN_ID = oPersona.GetOrganizzazioneDefault
    '                TPRL_ID = oComunita.RuoloDefault() 'qui si deve usare il tipo di ruolo predefinito per il tipo d comunita della comunita

    '                If oComunita.Organizzazione.Id = ORGN_ID Then
    '                    ' se la persona fa parte della stessa organizzazione della comunita la iscrivo col ruolo di default
    '                    Attivato = Not (oComunita.IsChiusa)
    '                    Abilitato = Not (oComunita.IsChiusa)
    '                Else 'se uno non � di quell'organizzazione come default lo iscrivo disabilitato e disattivato
    '                    Attivato = False
    '                    Abilitato = False
    '                End If

    '                oPersona.AssociaComunita(TPRL_ID, CMNT_ID, oPersona.RicezioneSMS, Attivato, Abilitato, False)

    '                oRuolo.EstraiByLinguaDefault(CMNT_ID, PRSN_ID)
    '                Attivato = oRuolo.Attivato
    '                Abilitato = oRuolo.Abilitato

    '                Dim ArrComunita(,) As String
    '                Try
    '                    ArrComunita = Session("ArrComunita")

    '                    CMNT_Path = ArrComunita(2, UBound(ArrComunita, 2)) & CMNT_ID & "."

    '                Catch ex As Exception
    '                    CMNT_Path = "." & CMNT_ID & "."
    '                End Try
    '                oRuolo.EstraiByLinguaDefault(CMNT_ID, PRSN_ID)

    '                oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PRSN_ID & "\"
    '                oTreeComunita.Nome = PRSN_ID & ".xml"
    '                oTreeComunita.Insert(oComunita, CMNT_Path, oComunita.GetNomeResponsabile_NomeCreatore, oRuolo)


    '                If oPersona.Errore = Errori_Db.None Then
    '                    isIscritto = True
    '                    If Attivato Then
    '                        alertMSG = oResource.getValue("messaggio0")
    '                        If alertMSG <> "" Then
    '                            alertMSG = alertMSG.Replace("'", "\'")
    '                        End If
    '                        Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
    '                    Else
    '                        '"Sei stato iscritto correttamente, ma non sei ancora abilitato!" & vbCrLf & " La tua registrazione dovr� essere valutata!"
    '                        alertMSG = oResource.getValue("messaggio1")
    '                        If alertMSG <> "" Then
    '                            alertMSG = alertMSG.Replace("'", "\'")
    '                        End If
    '                        Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
    '                    End If
    '                Else
    '                    '"Spiacenti, Si � verificato un errore nella registrazione."
    '                    alertMSG = oResource.getValue("messaggio2")
    '                    If alertMSG <> "" Then
    '                        alertMSG = alertMSG.Replace("'", "\'")
    '                    End If
    '                    Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
    '                End If
    '            End If
    '        Catch ex As Exception

    '        End Try
    '    End If
    '    Me.PNLmenuTree.Visible = False
    '    Me.PNLmenuPrincipale.Visible = False
    '    Me.PNLmenuDettagli.Visible = False
    '    Me.PNLdettagli.Visible = False
    '    Me.PNLtreeView.Visible = False
    '    Me.PNLgriglia.Visible = False
    '    If Me.HDNreturnTo.Value = "grid" Then
    '        Me.PNLgriglia.Visible = True
    '        Me.PNLmenuPrincipale.Visible = True
    '        Me.Bind_Griglia()
    '    ElseIf Me.HDNreturnTo.Value = "dettagli" Then
    '        Me.PNLdettagli.Visible = True
    '        Me.PNLmenuDettagli.Visible = True
    '        If isIscritto = True Then
    '            Me.LNBentraDettagli.Visible = True
    '            Me.LNBiscriviDettagli.Visible = False
    '        Else
    '            Me.LNBentraDettagli.Visible = False
    '            Me.LNBiscriviDettagli.Visible = True
    '        End If
    '    ElseIf Me.HDNreturnTo.Value = "tree" Then
    '        Me.PNLtreeView.Visible = True
    '        Me.PNLmenuTree.Visible = True
    '        Me.Bind_TreeView()
    '    Else
    '        Me.PNLgriglia.Visible = True
    '        Me.PNLmenuPrincipale.Visible = True
    '        Me.Bind_Griglia()
    '    End If
    '    Session("Azione") = "loaded"
    'End Sub
    Private Sub EntraComunita(ByVal CMNT_ID As Integer, ByVal CMNT_Path As String)
        Dim oTreeComunita As New COL_TreeComunita
        Dim oPersona As New COL_Persona
        Dim PRSN_ID, RuoloID As Integer

        Try
            oPersona = Session("objPersona")
            PRSN_ID = oPersona.ID

            oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PRSN_ID & "\"
            oTreeComunita.Nome = PRSN_ID & ".xml"
        Catch ex As Exception

        End Try

        Try
            Dim oRuolo As New COL_RuoloPersonaComunita
            oRuolo.EstraiByLinguaDefault(CMNT_ID, PRSN_ID)
            If oRuolo.Errore = Errori_Db.None Then
                RuoloID = oRuolo.TipoRuolo.Id
            End If

            'verifico se l'utente ha l'abilitazione per fare l'accesso alla comunit�

            Dim oComunita As New COL_Comunita

            oComunita.Id = CMNT_ID
            oComunita.Estrai()
            If oComunita.Errore = Errori_Db.None Then
                oComunita.TipoComunita.Icona = "./../" & oComunita.TipoComunita.Icona


                If oRuolo.Abilitato And oRuolo.Attivato Then 'se l'utente � attivato E abilitato allora
                    ' metto in sessione i permessi che l'utente ha per quella comunit�
                    Dim IdRuolo, i, j, dimensione, dimeArrCmnt, ORGN_ID As Integer
                    Session("IdRuolo") = RuoloID
                    Session("IdComunita") = CMNT_ID


                    Dim Elenco_CMNT_ID() As String
                    Elenco_CMNT_ID = CMNT_Path.Split(".")

                    Dim totale As Integer
                    Dim ArrComunita(,) As String

                    With oComunita
                        Session("ORGN_id") = .Organizzazione.Id
                        Try

                            Dim oServizio As New COL_Servizio
                            Dim oDataSet As New DataSet
                            oDataSet = COL_Servizio.ElencaByTipoRuoloByComunita(Session("IdRuolo"), CMNT_ID)
                            totale = oDataSet.Tables(0).Rows.Count - 1

                            Dim ArrPermessi(totale, 2) As String
                            For i = 0 To totale
                                Dim oRow As DataRow
                                oRow = oDataSet.Tables(0).Rows(i)
                                ArrPermessi(i, 0) = oRow.Item("SRVZ_Codice") 'CODICE servizio
                                ArrPermessi(i, 1) = oRow.Item("SRVZ_ID") 'id servizio
                                ArrPermessi(i, 2) = oRow.Item("LKSC_Permessi") 'valore servizio
                            Next
                            Session("ArrPermessi") = ArrPermessi
                        Catch ex As Exception

                        End Try

                        Try
                            If Session("LogonAs") = False Then
                                oRuolo.UpdateUltimocollegamento()
                            End If
                        Catch ex As Exception

                        End Try

                        'Aggiorno gli array relativi al menu history !!!


                        Dim tempArray(,), Path As String

                        If Session("limbo") = True Then
                            j = 0
                            For i = 0 To UBound(Elenco_CMNT_ID) - 1

                                If IsNumeric(Elenco_CMNT_ID(i)) Then
                                    If Elenco_CMNT_ID(i) > 0 Then
                                        ReDim Preserve ArrComunita(3, j)
                                        ArrComunita(0, j) = Elenco_CMNT_ID(i)
                                        ArrComunita(1, j) = COL_Comunita.EstraiNomeBylingua(Elenco_CMNT_ID(i), Session("LinguaID"))

                                        If Path = "" Then
                                            Path = "." & Elenco_CMNT_ID(i) & "."
                                        Else
                                            Path = Path & Elenco_CMNT_ID(i) & "."
                                        End If
                                        ArrComunita(2, j) = Path
                                        ' Ruolo svolto..........
                                        ArrComunita(3, j) = oPersona.GetIDRuoloForComunita(Elenco_CMNT_ID(i))
                                        j = j + 1
                                    End If
                                End If
                            Next

                            Session("ArrComunita") = ArrComunita
                            Session("limbo") = False

                        Else 'altrimento lo faccio per passi successivi

                            'caricamento navigazione albero comunit�
                            Try
                                ArrComunita = Session("ArrComunita")
                                totale = UBound(ArrComunita, 2) 'recupero il numero di comunit� dell'array

                                If oComunita.IdPadre = 0 Then 'se sono in cima all'albero allora inserisco solo il primo elemento
                                    j = 0
                                    For i = 0 To UBound(Elenco_CMNT_ID) - 1

                                        If IsNumeric(Elenco_CMNT_ID(i)) Then
                                            If Elenco_CMNT_ID(i) > 0 Then
                                                ReDim Preserve ArrComunita(3, j)
                                                ArrComunita(0, j) = Elenco_CMNT_ID(i)
                                                ArrComunita(1, j) = COL_Comunita.EstraiNomeBylingua(Elenco_CMNT_ID(i), Session("LinguaID"))

                                                If Path = "" Then
                                                    Path = "." & Elenco_CMNT_ID(i) & "."
                                                Else
                                                    Path = Path & Elenco_CMNT_ID(i) & "."
                                                End If
                                                ArrComunita(2, j) = Path

                                                ' Ruolo svolto..........
                                                ArrComunita(3, j) = oPersona.GetIDRuoloForComunita(Elenco_CMNT_ID(i))
                                                j = j + 1
                                            End If
                                        End If
                                    Next
                                Else
                                    ' Cerco di recuperare solo gli id delle nuove comuni� da agiungere
                                    ' nell'array
                                    Dim Last_Path

                                    'recupero l'ultimo path presente nell'history
                                    Path = ArrComunita(2, totale)
                                    Last_Path = Right(CMNT_Path, CMNT_Path.Length - Path.Length)
                                    Elenco_CMNT_ID = Last_Path.Split(".")

                                    j = totale + 1
                                    For i = 0 To UBound(Elenco_CMNT_ID) - 1
                                        If IsNumeric(Elenco_CMNT_ID(i)) Then
                                            If Elenco_CMNT_ID(i) > 0 Then
                                                ReDim Preserve ArrComunita(3, j)
                                                ArrComunita(0, j) = Elenco_CMNT_ID(i)
                                                ArrComunita(1, j) = COL_Comunita.EstraiNomeBylingua(Elenco_CMNT_ID(i), Session("LinguaID"))

                                                If Path = "" Then
                                                    Path = "." & Elenco_CMNT_ID(i) & "."
                                                Else
                                                    Path = Path & Elenco_CMNT_ID(i) & "."
                                                End If
                                                ArrComunita(2, j) = Path
                                                ' Ruolo svolto..........
                                                ArrComunita(3, j) = oPersona.GetIDRuoloForComunita(Elenco_CMNT_ID(i))
                                                j = j + 1
                                            End If
                                        End If
                                    Next
                                End If
                                Session("ArrComunita") = ArrComunita
                                Session("limbo") = False
                            Catch ex As Exception

                            End Try
                        End If
                    End With

                    Session("RLPC_ID") = oRuolo.Id
                    Dim oResourceConfig As New ResourceManager
                    oResourceConfig = GetResourceConfig(Session("LinguaCode"))
                    oComunita.RegistraAccesso(CMNT_ID, PRSN_ID, oResourceConfig.getValue("systemDBcodice"))
                    Me.PageUtility.SendNotificationUpdateCommunityAccess(PRSN_ID, CMNT_ID, oRuolo.UltimoCollegamento)

                    oTreeComunita.Update(oComunita, CMNT_Path, oComunita.GetNomeResponsabile_NomeCreatore, oRuolo)

                    Session("AdminForChange") = False
                    Session("CMNT_path_forAdmin") = ""
                    Session("idComunita_forAdmin") = ""

                    ' REGISTRAZIONE EVENTO
                    Session("TPCM_ID") = oComunita.TipoComunita.ID
                    Me.PageUtility.AddAction(ActionType.Access, Me.PageUtility.CreateObjectsList(Services_IscrizioneComunita.ObjectType.Community, oComunita.Id))

                    Dim defaultUrl As String = PageUtility.GetCommunityDefaultPage(CMNT_ID, PRSN_ID)
                    If oComunita.ShowCover(CMNT_ID, PRSN_ID) Then
                        If oRuolo.SaltaCopertina Then
                            Me.PageUtility.RedirectToUrl(defaultUrl)
                        Else
                            Me.PageUtility.RedirectToUrl("Generici/Cover.aspx")
                        End If
                    Else
                        Me.PageUtility.RedirectToUrl(defaultUrl)
                    End If
                    'If oComunita.ShowCover(CMNT_ID, PRSN_ID) Then
                    '    If oRuolo.SaltaCopertina Then
                    '        Me.PageUtility.RedirectToUrl(RedirectToDefaultPage(CMNT_ID, PRSN_ID))
                    '    Else
                    '        Me.PageUtility.RedirectToUrl("Generici/Cover.aspx")
                    '    End If
                    'Else
                    '    Me.PageUtility.RedirectToUrl(RedirectToDefaultPage(CMNT_ID, PRSN_ID))   ' se non faccio il redirect mi esegue prima il page_load dell'header e quindi vedo l'id della comunit� a cui ero loggato e non quella corrente
                    'End If
                ElseIf oRuolo.Attivato = False Then
                    Dim alertMSG As String  '"Spiacenti, Non � possibile loggarsi, Non si � stati attivati"
                    alertMSG = oResource.getValue("messaggio3")
                    If alertMSG <> "" Then
                        alertMSG = alertMSG.Replace("'", "\'")
                    End If
                    Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                    oTreeComunita.CambiaAttivazione(CMNT_ID, False, oResource)
                ElseIf oRuolo.Abilitato = False Then
                    Dim alertMSG As String
                    '"Spiacenti, Attualmente l'accesso alla comunit� risulta disabilitato."
                    alertMSG = oResource.getValue("messaggio4s")
                    If alertMSG <> "" Then
                        alertMSG = alertMSG.Replace("'", "\'")
                    End If
                    Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                    oTreeComunita.CambiaAbilitazione(CMNT_ID, False)
                End If
            Else
                oTreeComunita.Delete(CMNT_ID, CMNT_Path)
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Dettagli Cancellazione"
    'Private Sub BTNtoGestione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNtoGestione.Click
    '    If Me.HDNreturnTo.Value = "grid" Then
    '        Me.ResetForm_ToElenco()
    '        Me.Bind_Griglia()
    '        Me.RDTcomunita.CheckBoxes = False
    '    Else
    '        Me.RDTcomunita.CheckBoxes = False
    '        Me.HDN_nodoSel.Value = Me.HDN_nodoPadreSel.Value
    '        Me.HDN_nodoPadreSel.Value = ""
    '        Me.ResetForm_ToTreeView()
    '    End If
    'End Sub
#End Region

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_Management"
        oResource.Folder_Level1 = "Comunita"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(LBtitolo)
            Me.Master.ServiceTitle = .getValue("LBtitolo.text")
            .setLabel(Me.LBNopermessi)
            .setLabel(LBtipoComunita_c)
            .setLabel(LBnumeroRecord_c)
            .setLabel(LBtipoRicerca_c)
            .setLabel(LBvalore_c)
            .setLabel(LBstatoComunita_t)

            .setRadioButtonList(Me.RBLstatoComunita, -1)
            .setRadioButtonList(Me.RBLstatoComunita, 0)
            .setRadioButtonList(Me.RBLstatoComunita, 1)
            .setRadioButtonList(Me.RBLstatoComunita, 2)

            .setDropDownList(DDLTipoRicerca, -2)
            .setDropDownList(DDLTipoRicerca, -3)
            .setDropDownList(DDLTipoRicerca, -5)
            .setDropDownList(DDLTipoRicerca, -6)
            .setDropDownList(DDLTipoRicerca, -7)
            .setButton(Me.BTNCerca, True)
            .setLinkButton(LKBcrea, True, True)
            .setLinkButton(LNBelenco, True, True)
            .setLinkButton(LNBespandi, True, True)
            .setLinkButton(LNBcomprimi, True, True)

            .setLabel(LBorganizzazione_c)

            .setLabel(LBannoAccademico_c)
            .setLabel(LBperiodo_c)
            .setLabel(LBcorsoDiStudi_t)
            .setLinkButton(LNBtreeView, True, False)
            oResource.setHeaderDatagrid(Me.DGComunita, 1, "TPCM_Descrizione", True)
            oResource.setHeaderDatagrid(Me.DGComunita, 2, "CMNT_Nome", True)
            oResource.setHeaderDatagrid(Me.DGComunita, 3, "AnagraficaCreatore", True)
            oResource.setHeaderDatagrid(Me.DGComunita, 4, "CMNT_Responsabile", True)
            oResource.setHeaderDatagrid(Me.DGComunita, 5, "CMNT_dataCreazione", True)
            oResource.setHeaderDatagrid(Me.DGComunita, 6, "Cessazione", True)

            .setLabel(LBlegendaDettagli)
            .setLinkButton(Me.LNBiscriviDettagli, True, True)
            .setLinkButton(Me.LNBnascondiDettagli, True, True)
            .setLinkButton(Me.LNBentraDettagli, True, True)
            .setLinkButton(Me.LNBindietro, True, True)
            .setLinkButton(Me.LNBelimina, True, True, , True)
            .setLinkButton(Me.LKBaltro, True, True)
            .setLinkButton(Me.LKBtutti, True, True)
            Dim i As Integer
            For i = Asc("a") To Asc("z") 'status dei link button delle lettere
                Dim oLinkButton As New LinkButton
                oLinkButton = FindControlRecursive(Me.Master, "LKB" & Chr(i))
                Dim Carattere As String = Chr(i)

                If IsNothing(oLinkButton) = False Then
                    .setLinkButtonLettera(oLinkButton, "#%%#", Carattere.ToUpper, True, True)
                End If
            Next


            .setLinkButton(Me.LNBapriFiltro, True, True)
            .setLinkButton(Me.LNBchiudiFiltro, True, True)
            .setDropDownList(Me.DDLTipoRicerca, -9)
            .setCheckBox(Me.CBXautoUpdate)
        End With
    End Sub
#End Region

#Region "Conferma Cancellazione Da TreeView"

    Private Function CancellaComunita_Singola(ByVal ComunitaID As Integer, ByVal ComunitaPath As String) As Boolean
        Try
            Dim oComunita As New COL_Comunita
            ' Dim oFileDisponibile As New COL_File_Disponibile

            Dim Path As String = ""

            oComunita.Id = ComunitaID

            Dim cacheKey As String = "CommunityRepositorySize_" & ComunitaID
            Dim CommunityPath As String = ""
            If PageUtility.SystemSettings.File.Materiale.DrivePath = "" Then
                CommunityPath = Server.MapPath(PageUtility.BaseUrl & PageUtility.SystemSettings.File.Materiale.VirtualPath)
            Else
                CommunityPath = PageUtility.SystemSettings.File.Materiale.DrivePath
            End If
          
            GenericCacheManager.PurgeCacheItems(cacheKey)
            Me.CurrentPresenter.RemoveCommunity(ComunitaID, CommunityPath, Me.PageUtility.BaseUserRepositoryPath)

            'oFileDisponibile.DeleteAllByCommunity(ComunitaID, PageUtility.Configurations(ComunitaID))


            'Path = Server.MapPath(GetPercorsoApplicazione(Me.Request) & "/Bacheca/")
            'Path = Path.Replace("//", "/")
            'Path = Path.Replace("//", "/")
            'Path = Path.Replace("\\", "\")
            'Path = Path.Replace("\\", "\")
            'Path = Path.Replace("\/", "\")

            'oFile.CancFile(Path & "bacheca_" & ComunitaID & ".htm")
            Dim MailPath As String = ""
            If PageUtility.SystemSettings.File.Mail.DrivePath = "" Then
                MailPath = Server.MapPath(PageUtility.BaseUrl & PageUtility.SystemSettings.File.Mail.VirtualPath)
            Else
                MailPath = PageUtility.SystemSettings.File.Mail.DrivePath
            End If
            If Not (MailPath.EndsWith("\") OrElse MailPath.EndsWith("/")) Then
                MailPath &= "\"
            End If
            lm.Comol.Core.File.Delete.Directory(MailPath & ComunitaID, True)

            Dim CoverPath As String = ""
            If PageUtility.SystemSettings.File.Cover.DrivePath = "" Then
                CoverPath = Server.MapPath(PageUtility.BaseUrl & PageUtility.SystemSettings.File.Cover.VirtualPath)
            Else
                CoverPath = PageUtility.SystemSettings.File.Cover.DrivePath
            End If
            If Not (CoverPath.EndsWith("\") OrElse CoverPath.EndsWith("/")) Then
                CoverPath &= "\"
            End If
            lm.Comol.Core.File.Delete.Directory(CoverPath & ComunitaID, True)

            oComunita.EliminaComunita(ComunitaID)

            If oComunita.Errore = Errori_Db.None Then

                Dim oTreeComunita As New COL_TreeComunita

                Try
                    oTreeComunita.Directory = Server.MapPath(GetPercorsoApplicazione(Me.Request) & "/profili/") & Session("objPersona").id & "\"
                    oTreeComunita.Nome = Session("objPersona").id & ".xml"
                    oTreeComunita.Delete(ComunitaID, ComunitaPath)
                Catch ex As Exception

                End Try
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Function CancellaComunitaLink_Singola(ByVal ComunitaID As Integer, ByVal ComunitaPadreID As Integer, ByVal ComunitaPath As String) As Boolean
        Try
            Dim oComunita As New COL_Comunita
            oComunita.CancellaLinkComunita(ComunitaID, ComunitaPadreID)
            If oComunita.Errore = Errori_Db.None Then
                Dim oTreeComunita As New COL_TreeComunita

                Try
                    oTreeComunita.Directory = Server.MapPath(GetPercorsoApplicazione(Me.Request) & "/profili/") & Session("objPersona").id & "\"
                    oTreeComunita.Nome = Session("objPersona").id & ".xml"
                    oTreeComunita.Delete(ComunitaID, ComunitaPath)
                Catch ex As Exception

                End Try
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function


    Private Sub CancellaNodo(ByRef oNodoPadre As Telerik.WebControls.RadTreeNode, ByRef JustDeleted As String)
        Dim ComunitaID, ComunitaPadreID As Integer
        Dim ComunitaPath, nomeComunita As String
        Dim CMNT_HasFigli, CMNT_isDiretto, CMNT_IsChiusa As Boolean
        Dim i, totale As Integer
        Dim ElencoValori() As String


        totale = oNodoPadre.Nodes.Count - 1


        For i = totale To 0 Step -1
            Dim oNodoFiglio As Telerik.WebControls.RadTreeNode
            oNodoFiglio = oNodoPadre.Nodes(i)
            ElencoValori = oNodoFiglio.Value.Split(",")

            ComunitaID = ElencoValori(0)
            ComunitaPath = ElencoValori(1)
            ComunitaPadreID = oNodoFiglio.Parent.Category
            CMNT_IsChiusa = CBool(ElencoValori(2))
            CMNT_HasFigli = CBool(ElencoValori(3))
            CMNT_isDiretto = CBool(ElencoValori(4))

            nomeComunita = oNodoFiglio.Text
            If InStr(JustDeleted, "," & ComunitaID & ",") > 0 Then
                oNodoFiglio.Checked = False
                oNodoFiglio.Remove()
            Else
                If oNodoFiglio.Nodes.Count = 0 Then
                    If CMNT_isDiretto Then 'Me.LBdettagliElimina.Text & "Eliminata la comunit�: <span class=info_blackMedium>" & nomeComunita & "</span><br>"
                        '  Me.LBdettagliElimina.Text &= "<li><ul><li><span class=info_blackMedium>" & nomeComunita & "</span></li></ul></li>"
                        Me.CancellaComunita_Singola(ComunitaID, ComunitaPath)
                        JustDeleted = JustDeleted & ComunitaID & ","
                    Else
                        If InStrRev(nomeComunita, " - LINK") > 0 Then
                            nomeComunita = nomeComunita.Substring(0, InStrRev(nomeComunita, " - LINK"))
                        End If 'Me.LBdettagliElimina.Text & "Eliminato il link alla comunit�: <span class=info_blackMedium>" & nomeComunita & "</span><br>"
                        ' Me.LBdettagliElimina.Text &= "<li><ul><li><span class=info_blackMedium>" & nomeComunita & "</span></li></ul></li>"
                        Me.CancellaComunitaLink_Singola(ComunitaID, ComunitaPadreID, ComunitaPath)
                    End If
                    oNodoFiglio.Checked = False
                    oNodoFiglio.Remove()
                ElseIf CMNT_isDiretto Then
                    '      Me.LBdettagliElimina.Text &= "<ul>"
                    Me.CancellaNodo(oNodoFiglio, JustDeleted)
                    '  Me.LBdettagliElimina.Text &= "</li>"
                    'If CMNT_isDiretto Then
                    '    JustDeleted = JustDeleted & CMNT_id & ","
                    'End If
                ElseIf CMNT_isDiretto = False Then
                    If InStrRev(nomeComunita, " - LINK") > 0 Then
                        nomeComunita = nomeComunita.Substring(0, InStrRev(nomeComunita, " - LINK"))
                    End If 'Me.LBdettagliElimina.Text & "Eliminato il link alla comunit�: <span class=info_blackMedium>" & nomeComunita & "</span><br>"
                    ' Me.LBdettagliElimina.Text &= "<li><ul><li><span class=info_blackMedium>" & nomeComunita & "</span></li></ul></li>"
                    Me.CancellaComunita_Singola(ComunitaID, ComunitaPath)
                    oNodoFiglio.Checked = False
                    oNodoFiglio.Remove()
                End If
            End If
        Next

    End Sub

    Private Sub LNBelimina_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBelimina.Click
        Dim ComunitaID, ComunitaPadreID As Integer
        Dim ComunitaPath As String
        Dim oComunita As New COL_Comunita


        If Me.PNLconfermaElimina.Visible Then
            Try

                ComunitaID = Me.HDNcmnt_ID.Value
                ComunitaPath = Me.HDN_Path.Value
                ComunitaPadreID = Me.HDN_idPadre_Link.Value

                oComunita.Id = ComunitaID
                oComunita.Estrai()
                If oComunita.IdPadre = ComunitaPadreID Then
                    'Cancello la comunit�
                    Me.CancellaComunita_Singola(ComunitaID, ComunitaPath)
                Else
                    'Cancello il link alla comunit� !
                    Me.CancellaComunitaLink_Singola(ComunitaID, ComunitaPadreID, ComunitaPath)
                End If


            Catch ex As Exception

            End Try
        Else

            Dim oDataset As New DataSet

            Try
                Dim oList As ArrayList
                oList = Me.RDTcomunita.CheckedNodes
                Dim JustDeleted, nomeComunita As String
                Dim CMNT_HasFigli, CMNT_isDiretto, CMNT_IsChiusa As Boolean

                JustDeleted = ","

                'Me.LBdettagliElimina.Text = oResource.getValue("LBdettagliElimina1") ' "Elenco comunit� cancellate (reali o semplici link):<br>"
                While Me.RDTcomunita.CheckedNodes.Count > 0
                    Dim oNode As Telerik.WebControls.RadTreeNode
                    Dim ElencoValori() As String
                    oNode = Me.RDTcomunita.CheckedNodes(0)

                    ElencoValori = oNode.Value.Split(",")

                    ComunitaID = ElencoValori(0)
                    ComunitaPath = ElencoValori(1)
                    ComunitaPadreID = oNode.Parent.Category
                    CMNT_IsChiusa = CBool(ElencoValori(2))
                    CMNT_HasFigli = CBool(ElencoValori(3))
                    CMNT_isDiretto = CBool(ElencoValori(4))

                    If InStr(JustDeleted, "," & ComunitaID & ",") > 0 Then
                        oNode.Checked = False
                        oNode.Remove()
                    Else
                        nomeComunita = oNode.Text
                        If oNode.Nodes.Count = 0 Then
                            'Nessun figlio ! Posso cancellare '''Eliminata la comunit�:
                            If CMNT_isDiretto Then
                                Me.CancellaComunita_Singola(ComunitaID, ComunitaPath)
                                ' Me.LBdettagliElimina.Text &= "<li><span class=info_blackMedium>" & nomeComunita & "</span></li>"
                                JustDeleted = JustDeleted & ComunitaID & ","
                            Else
                                If InStrRev(nomeComunita, " - LINK") > 0 Then
                                    nomeComunita = nomeComunita.Substring(0, InStrRev(nomeComunita, " - LINK"))
                                End If
                                Me.CancellaComunitaLink_Singola(ComunitaID, ComunitaPadreID, ComunitaPath)
                                ' Me.LBdettagliElimina.Text &= "<li><span class=info_blackMedium>" & nomeComunita & "</span></li>"
                            End If
                            oNode.Checked = False
                            oNode.Remove()
                        ElseIf CMNT_isDiretto Then
                            Me.CancellaNodo(oNode, JustDeleted)
                            'JustDeleted = JustDeleted & CMNT_id & ","
                        ElseIf CMNT_isDiretto = False Then
                            If InStrRev(nomeComunita, " - LINK") > 0 Then
                                nomeComunita = nomeComunita.Substring(0, InStrRev(nomeComunita, " - LINK"))
                            End If
                            Me.CancellaComunitaLink_Singola(ComunitaID, ComunitaPadreID, ComunitaPath)
                            '   Me.LBdettagliElimina.Text &= "<li><span class=info_blackMedium>" & nomeComunita & "</span></li>"
                            oNode.Checked = False
                            oNode.Remove()
                        End If
                    End If
                End While
            Catch ex As Exception

            End Try
        End If
        If Me.HDNreturnTo.Value = "grid" Then
            Me.ResetForm_ToElenco()
            Me.Bind_Griglia()
        Else
            Me.ResetForm_ToTreeView()
            Me.HDN_nodoSel.Value = Me.HDN_nodoPadreSel.Value
            Me.Bind_TreeView(False, ComunitaPadreID)
        End If
    End Sub

    Private Sub LNBindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBindietro.Click
        If Me.HDNreturnTo.Value = "grid" Then
            Me.ResetForm_ToElenco()
            Me.Bind_Griglia()
        Else
            Me.ResetForm_ToTreeView()
        End If
        Me.HDNreturnTo.Value = ""
        Me.HDN_Path.Value = ""
        Me.HDNcmnt_ID.Value = ""
        Me.HDN_isDiretto.Value = ""
    End Sub

#End Region

    Private Sub DDLTipoRicerca_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLTipoRicerca.SelectedIndexChanged
        If Me.DDLTipoRicerca.SelectedValue = Main.FiltroComunita.IDresponsabile Then
            Me.DDLresponsabile.Visible = True
            Me.TXBValore.Text = ""
            Me.TXBValore.Visible = False
        Else
            Me.DDLresponsabile.Visible = False
            Me.TXBValore.Visible = True
        End If
        Me.Bind_Griglia()
    End Sub

    Private Sub DDLresponsabile_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLresponsabile.SelectedIndexChanged
        Me.Bind_Griglia()
    End Sub

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_AmministraComunita.Codex)
    End Sub



    Private Function FindControlRecursive(ByVal Root As Control, ByVal Id As String) As Control
        If Root.ID = Id Then
            Return Root
        End If

        For Each Ctl As Control In Root.Controls
            Dim FoundCtl As Control = FindControlRecursive(Ctl, Id)
            If FoundCtl IsNot Nothing Then
                Return FoundCtl
            End If
        Next
        Return Nothing
    End Function

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property

End Class