Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.Refactor.WikiService.Business

Namespace WikiNew



    Public Class PresenterWiki
        'Inherits PresenterGenerico

        Private view As IViewWiki
        Private oManagerWiki As COL_Wiki.WikiNew.iManagerWiki 'WikiNew.AbstractManagerWiki  '

        Private _Context As iApplicationContext

        ''' <summary>
        ''' Inizializzazione presenter
        ''' </summary>
        ''' <param name="view">Per iniettare la view</param>
        ''' <param name="DbSource">Per inizializzare nHybernate o SQL: in deployment SOLO SQL</param>
        ''' <param name="UseCache">Indica se utilizzare o meno la cache</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal view As IViewWiki, ByVal ocontext As iApplicationContext, Optional ByVal DbSource As FactoryWiki.ConnectionType = FactoryWiki.ConnectionType.SQL, Optional ByVal UseCache As Boolean = True)
            Me.view = view
            Dim IdComunita As Integer = -1
            Try
                IdComunita = Me.view.MyComunitaCorrente.Id
            Catch ex As Exception
            End Try
            If IdComunita >= 0 Then
                oManagerWiki = COL_Wiki.FactoryWiki.CreateManagerWikiTest(DbSource, Me.view.MyPersonaCorrente, Me.view.MyComunitaCorrente, ocontext, False)
            Else
                Me.view.Show(Visualizzazioni.NoPermessi)
            End If

        End Sub
#Region "ForHOME"
        ''' <summary>
        '''  Recupera i dati per il navigatore tra le sezione e forza il bind della vista su tale elemento
        ''' </summary>
        ''' <returns>TRUE se esistono sezioni, FALSE se non esistono sezioni per la wiki</returns>
        ''' <remarks></remarks>
        Private Function BindNavigatoreHome(ByRef Sezione As WikiNew.SezioneWiki, Optional ByVal Forced As Boolean = False) As Boolean
            Dim Sezioni, Sezioni2 As New ArrayList
            Sezioni = Me.oManagerWiki.CaricaSezioniHome(Forced)
            If Sezioni.Count > 0 Then
                'Verifichiamo che sia l'amministratore
                '--- Solo non gestisce togliamo i topic eleiminati
                If Not (Me.view.Servizio.Admin Or Me.view.Servizio.GestioneTopics) Then
                    For i As Integer = 0 To Sezioni.Count - 1
                        Dim elemento As SezioneWiki = Sezioni.Item(i)
                        If Not elemento.IsDeleted Then
                            Sezioni2.Add(Sezioni.Item(i))
                        End If
                    Next
                    Sezioni = Sezioni2
                End If
            End If
            If Sezioni.Count > 0 Then
                Me.view.BindNavigatore(Sezioni)
                Sezione = Sezioni.Item(0)
                Me.view.ActualSezioneId = Sezione.ID 'Uso quello della PRIMA sezione
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Raccoglie i dati dei Topic relativi ed aggiorna la vista
        ''' </summary>
        ''' <returns>TRUE: esistono Topic, FALSE: non esistono Topic</returns>
        ''' <remarks></remarks>
        Private Function BindTopicSezioneHome(ByVal oSezione As WikiNew.SezioneWiki) As Boolean
            Dim ArlTopic As ArrayList = Me.oManagerWiki.CaricaTopicsHome(oSezione)

            If ArlTopic.Count > 0 Then
                Me.view.BindTopicsSezione(ArlTopic, oSezione)
                Return True
            Else
                Return False
            End If

        End Function

        ''' <summary>
        ''' Raccoglie i dati dei Topic relativi ed aggiorna la vista
        ''' </summary>
        ''' <returns>TRUE: esistono Topic, FALSE: non esistono Topic</returns>
        ''' <remarks></remarks>
        Private Function BindCercaTopicHome(ByVal text As String) As Boolean
            Dim ATopic As ArrayList = Me.oManagerWiki.CercaTopic(Me.view.SearchString, 1, 100)

            If ATopic.Count > 0 Then
                Me.view.BindTopicsFunded(ATopic)

                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Raccoglie i dati del topic per la visualizzazione della stessa
        ''' </summary>
        ''' <param name="TopicID"></param>
        ''' <remarks></remarks>
        Private Sub BindTopicTestHome(ByVal TopicID As Guid)
            Dim oTopic As New WikiNew.TopicWiki
            oTopic.ID = TopicID
            oTopic.Sezione = New COL_Wiki.WikiNew.SezioneWiki
            oTopic.Sezione.ID = Me.view.ActualSezioneId

            Me.oManagerWiki.CaricaTopic(oTopic)
            Me.view.BindTopicTest(oTopic)
        End Sub
#End Region



#Region "Binding"
        Public Sub BackToSection()
            If Me.view.ActualTopicId = System.Guid.Empty Then
                BindDati(COL_Wiki.WikiNew.Binding.Reset)
            Else
                Dim oTopic As New WikiNew.TopicWiki
                oTopic.ID = Me.view.ActualTopicId
                Me.oManagerWiki.CaricaTopic(oTopic)
                If Not IsNothing(oTopic.Sezione) AndAlso oTopic.Sezione.ID <> System.Guid.Empty Then
                    Me.view.ActualSezioneId = oTopic.Sezione.ID
                    BindDati(COL_Wiki.WikiNew.Binding.ResetToUrlSection)
                Else
                    BindDati(COL_Wiki.WikiNew.Binding.Reset)
                End If
            End If
        End Sub
        Public Sub BindDati(ByVal BindingTest As Binding)
            Dim oSezione As COL_Wiki.WikiNew.SezioneWiki

            If Me.view.MyComunitaCorrente.Id = 0 Then
                'Qui facciamo tutto quello che vogliamo per la parte pubblica

                Select Case BindingTest
                    Case WikiNew.Binding.Reset
                        If Not Me.BindNavigatoreHome(oSezione) Then 'non Ho sezioni
                            Me.view.Show(Visualizzazioni.NoTopic)
                        Else    'ho sezioni
                            If Me.BindTopicSezioneHome(oSezione) Then
                                Me.view.Show(Visualizzazioni.ListaTopic)
                            Else
                                Me.view.Show(Visualizzazioni.NoTopic)
                            End If
                        End If
                    Case WikiNew.Binding.ResetToUrlSection
                        oSezione = New COL_Wiki.WikiNew.SezioneWiki
                        oSezione.ID = Me.view.ActualSezioneId
                        If Not Me.BindNavigatoreHome(oSezione) Then 'non Ho sezioni
                            Me.view.Show(Visualizzazioni.NoTopic)
                        Else    'ho sezioni
                            If Me.BindTopicSezioneHome(oSezione) Then
                                Me.view.Show(Visualizzazioni.ListaTopic)
                            Else
                                Me.view.Show(Visualizzazioni.NoTopic)
                            End If
                        End If
                    Case WikiNew.Binding.SezioneGoto
                        oSezione = New COL_Wiki.WikiNew.SezioneWiki()
                        oSezione.ID = Me.view.ActualSezioneId
                        Dim ooSezione As New COL_Wiki.WikiNew.SezioneWiki
                        ooSezione.ID = Me.view.ActualSezioneId

                        Me.oManagerWiki.CaricaSezioneHome(oSezione)

                        If Me.BindTopicSezioneHome(oSezione) Then
                            Me.view.Show(Visualizzazioni.ListaTopic)
                        Else
                            Me.view.Show(Visualizzazioni.NoTopic)
                        End If
                        '-----------Alessandro x ricerca di topics e visualizzaione singolo
                    Case WikiNew.Binding.TopicSearch
                        If Me.BindCercaTopicHome(Me.view.SearchString) Then
                            Me.view.Show(Visualizzazioni.ListaTopicSearched)
                        Else
                            Me.view.Show(Visualizzazioni.NoTopic)
                        End If
                    Case WikiNew.Binding.TopicView
                        Me.BindTopicTestHome(Me.view.ActualTopicId)
                        Me.view.Show(Visualizzazioni.ShowTopic)

                    Case WikiNew.Binding.TopicViewForced
                        Me.BindTopic(Me.view.ActualTopicId)
                        If Me.view.TopicIsPubblico Then
                            Me.view.Show(Visualizzazioni.ShowTopic)
                        Else
                            Me.view.Show(Visualizzazioni.NoPermessi)
                        End If



                End Select



            Else
                'Qui facciamo quello che serve per la parte della comunità.


                Select Case BindingTest
                    Case WikiNew.Binding.Reset  'Al primo caricamento della pagina
                        'Esiste la Wiki?
                        Dim oWiki As COL_Wiki.WikiNew.Wiki
                        Try
                            oWiki = Me.oManagerWiki.CaricaWiki(True)
                            Me.view.ShowAuthors = oWiki.DisplayAuthors
                        Catch ex As Exception
                            oWiki = Nothing
                        End Try
                        If IsNothing(oWiki) Then 'non esiste la wiki
                            Me.view.ActualWikiId = System.Guid.Empty
                            If Me.view.Servizio.Admin Or Me.view.Servizio.GestioneWiki Then
                                Me.view.Show(Visualizzazioni.NoWiki)
                            Else
                                Me.view.Show(Visualizzazioni.NoPermessi)
                            End If
                        Else    'la Wiki Esiste
                            Me.view.ActualWikiId = oWiki.ID
                            If Not Me.BindNavigatore(oSezione, oWiki, True) Then 'non Ho sezioni
                                Me.view.ActualSezioneId = Guid.Empty
                                If Me.view.Servizio.Admin Or Me.view.Servizio.GestioneSezioni Then
                                    Me.view.Show(Visualizzazioni.NoSezione)
                                Else
                                    Me.view.Show(Visualizzazioni.NoSezioniNoPermessi)
                                End If
                            Else    'ho sezioni
                                If Me.BindTopicSezione(oSezione) Then 'Ho Topic
                                    Me.view.Show(Visualizzazioni.ListaTopic)
                                Else 'Non ho Topic
                                    Me.view.ActualTopicId = Guid.Empty
                                    If Me.view.Servizio.Admin Or Me.view.Servizio.GestioneTopics Then
                                        Me.view.Show(Visualizzazioni.NoTopic)
                                    Else
                                        Me.view.Show(Visualizzazioni.NoTopicNoPermessi)
                                    End If
                                End If
                            End If
                        End If
                    Case WikiNew.Binding.ResetToUrlSection

                        Dim oWiki As COL_Wiki.WikiNew.Wiki
                        Try
                            oWiki = Me.oManagerWiki.CaricaWiki(True)
                            Me.view.ShowAuthors = oWiki.DisplayAuthors
                        Catch ex As Exception
                            oWiki = Nothing
                        End Try

                        oSezione = New COL_Wiki.WikiNew.SezioneWiki
                        oSezione.ID = Me.view.ActualSezioneId

                        If IsNothing(oWiki) Then 'non esiste la wiki
                            Me.view.ActualWikiId = System.Guid.Empty
                            If Me.view.Servizio.Admin Or Me.view.Servizio.GestioneWiki Then
                                Me.view.Show(Visualizzazioni.NoWiki)
                            Else
                                Me.view.Show(Visualizzazioni.NoPermessi)
                            End If
                        Else    'la Wiki Esiste
                            Me.view.ActualWikiId = oWiki.ID
                            If Not Me.BindNavigatoreWithSelectedSection(oSezione, oWiki, True) Then 'non Ho sezioni
                                Me.view.ActualSezioneId = Guid.Empty
                                If Me.view.Servizio.Admin Or Me.view.Servizio.GestioneSezioni Then
                                    Me.view.Show(Visualizzazioni.NoSezione)
                                Else
                                    Me.view.Show(Visualizzazioni.NoSezioniNoPermessi)
                                End If
                            Else    'ho sezioni
                                Me.oManagerWiki.CaricaSezione(oSezione)
                                If Me.BindTopicSezione(oSezione) Then 'Ho Topic
                                    Me.view.Show(Visualizzazioni.ListaTopic)
                                Else 'Non ho Topic
                                    Me.view.ActualTopicId = Guid.Empty
                                    If Me.view.Servizio.Admin Or Me.view.Servizio.GestioneTopics Then
                                        Me.view.Show(Visualizzazioni.NoTopic)
                                    Else
                                        Me.view.Show(Visualizzazioni.NoTopicNoPermessi)
                                    End If
                                End If
                            End If
                        End If




                    Case WikiNew.Binding.WikiAdd
                        If Me.view.Servizio.Admin Or Me.view.Servizio.GestioneWiki Then
                            Me.view.ActualWikiId = Guid.Empty
                            Me.view.Show(Visualizzazioni.NoWiki)
                            Me.view.DisplayAuthors = True
                        End If

                    Case WikiNew.Binding.WikiModify
                        If Me.view.Servizio.Admin Or Me.view.Servizio.GestioneWiki Then
                            Dim oWiki As New COL_Wiki.WikiNew.Wiki
                            oWiki.ID = Me.view.ActualWikiId
                            oWiki = Me.oManagerWiki.CaricaWiki()
                            Me.view.NomeWiki = oWiki.Nome
                            Me.view.ActualWikiId = oWiki.ID
                            Me.view.Show(Visualizzazioni.WikiMod)
                            Me.view.DisplayAuthors = oWiki.DisplayAuthors
                            Me.view.ShowAuthors = oWiki.DisplayAuthors
                        End If

                    Case WikiNew.Binding.SezioneGoto
                        oSezione = New COL_Wiki.WikiNew.SezioneWiki()
                        oSezione.ID = Me.view.ActualSezioneId
                        oSezione.Wiki = New COL_Wiki.WikiNew.Wiki
                        oSezione.Wiki.ID = Me.view.ActualWikiId

                        If (oSezione.ID = Guid.Empty) Then
                            Me.view.Show(Visualizzazioni.ListaTopic)
                        Else
                            Me.oManagerWiki.CaricaSezione(oSezione)
                            Dim oWiki As COL_Wiki.WikiNew.Wiki
                            Try
                                oWiki = Me.oManagerWiki.CaricaWiki(True)
                                Me.view.ShowAuthors = oWiki.DisplayAuthors
                                Me.BindNavigatoreWithSelectedSection(oSezione, oWiki)
                            Catch ex As Exception
                                oWiki = Nothing
                            End Try

                            If Me.BindTopicSezione(oSezione) Then
                                Me.view.Show(Visualizzazioni.ListaTopic)
                            Else
                                If Me.view.Servizio.Admin Or Me.view.Servizio.GestioneSezioni Then
                                    Me.view.Show(Visualizzazioni.NoTopic)
                                Else
                                    Me.view.Show(Visualizzazioni.NoTopicNoPermessi)
                                End If
                            End If
                        End If


                    Case WikiNew.Binding.SezioneAdd
                        If Me.view.Servizio.Admin Or Me.view.Servizio.GestioneSezioni Then
                            Me.ClearSezione()
                            Me.view.Show(Visualizzazioni.AddSezione)
                        End If

                    Case WikiNew.Binding.SezioneModify
                        If Me.view.Servizio.Admin Or Me.view.Servizio.GestioneSezioni Then
                            Me.BindSezione(Me.view.ActualSezioneId)
                            Me.view.Show(Visualizzazioni.AddSezione)
                        End If

                    Case WikiNew.Binding.TopicAdd
                        If Me.view.Servizio.Admin Or Me.view.Servizio.GestioneTopics Then
                            Me.ClearTopic()
                            Me.view.Show(Visualizzazioni.AddTopic)
                        End If
                    Case WikiNew.Binding.TopicModify
                        If Me.view.Servizio.Admin Or Me.view.Servizio.GestioneTopics Then
                            Me.BindTopic(Me.view.ActualTopicId)
                            Me.view.Show(Visualizzazioni.AddTopic)
                        End If

                    Case WikiNew.Binding.TopicCronologia
                        'If Me.view.Servizio.Admin Or Me.view.Servizio.GestioneCronologia Then
                        Dim oTopic As New WikiNew.TopicWiki
                        oTopic.ID = Me.view.ActualTopicId
                        If Me.BindCronologia(oTopic) Then
                            Me.view.Show(Visualizzazioni.CronologiaTopic)
                        Else
                            Me.view.Show(Visualizzazioni.CronologiaNoTopic)
                        End If
                        'End If
                    Case WikiNew.Binding.TopicView
                        Me.BindTopic(Me.view.ActualTopicId)
                        Me.view.Show(Visualizzazioni.ShowTopic)

                    Case WikiNew.Binding.TopicViewForced
                        Me.BindTopic(Me.view.ActualTopicId)
                        If Me.view.TopicIsPubblico Then
                            Me.view.Show(Visualizzazioni.ShowTopic)
                        Else
                            Me.view.Show(Visualizzazioni.NoPermessi)
                        End If

                    Case WikiNew.Binding.TopicSearch
                        If Me.BindCercaTopic(Me.view.SearchString) Then
                            Me.view.Show(Visualizzazioni.ListaTopicSearched)
                        Else
                            Me.view.Show(Visualizzazioni.NoTopic)
                        End If
                    Case WikiNew.Binding.ComunityList
                        'If Not (Me.view.ActualTopicId = Guid.Empty) Then
                        Me.view.BindComunita()
                        Me.view.Show(Visualizzazioni.ListaComunita)

                    Case WikiNew.Binding.ImportTopics
                        Me.BindTopicImport(Me.view.ExternalComunityID)
                        Me.view.Show(Visualizzazioni.ListaTopicsImport)

                    Case WikiNew.Binding.ImportTopic
                        Me.BindImporta(Me.view.ExternalTopicID)

                End Select

            End If

        End Sub

        ''' <summary>
        ''' Raccoglie i dati dei Topic relativi ed aggiorna la vista
        ''' </summary>
        ''' <returns>TRUE: esistono Topic, FALSE: non esistono Topic</returns>
        ''' <remarks></remarks>
        Private Function BindCercaTopic(ByVal text As String) As Boolean
            'Dim ATopic As ArrayList = Me.oManagerWiki.CercaTopic(Me.view.SearchString, 1, 100)

            Dim ATopic As ArrayList = Me.oManagerWiki.CercaTopicComPub(Me.view.SearchString, 1, 100)

            If ATopic.Count > 0 Then
                Me.view.BindTopicsFunded(ATopic)

                Return True
            Else
                Return False
            End If
        End Function

        Private Function BindTopicImport(ByVal Idcomunita As Integer) As Boolean


            'Private Function BindCercaTopicComunita(ByVal text As String) As Boolean
            Dim rowIndex As Integer
            rowIndex = (Me.view.pageIndex * Me.view.pageSize) - (Me.view.pageSize - 1)
            Dim ATopic As ArrayList = Me.oManagerWiki.CercaTopicComunita("%", rowIndex, Me.view.pageSize + 1, Idcomunita)
            If ATopic.Count > 0 Then
                'Vedo in base a quanti risultati se è kl'ultima pagina
                If ATopic.Count = Me.view.pageSize + 1 Then
                    ATopic.RemoveAt(ATopic.Count - 1)
                    Me.view.IsLastPage = False
                Else
                    Me.view.IsLastPage = True
                End If
                Me.view.BindTopicsImport(ATopic)

                Return True
            Else
                Return False
            End If

        End Function


        ''' <summary>
        '''  Recupera i dati per il navigatore tra le sezione e forza il bind della vista su tale elemento
        ''' </summary>
        ''' <returns>TRUE se esistono sezioni, FALSE se non esistono sezioni per la wiki</returns>
        ''' <remarks></remarks>
        Private Function BindNavigatore(ByRef Sezione As WikiNew.SezioneWiki, ByVal oWiki As COL_Wiki.WikiNew.Wiki, Optional ByVal Forced As Boolean = False) As Boolean
            Dim Sezioni, sezioni2 As New ArrayList
            Dim TmpSezioneDef, TmpSezione As WikiNew.SezioneWiki

            Dim first As Boolean = True
            Dim HasDefault As Boolean = False

            Me.view.TitoloPagina = oWiki.Nome

            Sezioni = Me.oManagerWiki.CaricaSezioni(oWiki, Forced)
            If Sezioni.Count > 0 Then
                'Verifichiamo che sia l'amministratore
                '--- Solo non gestisce togliamo i topic eleiminati
                If Not (Me.view.Servizio.Admin Or Me.view.Servizio.GestioneTopics) Then
                    For i As Integer = 0 To Sezioni.Count - 1
                        Dim elemento As SezioneWiki = Sezioni.Item(i)
                        If Not elemento.IsDeleted Then
                            sezioni2.Add(Sezioni.Item(i))
                        End If
                    Next
                    Sezioni = sezioni2
                End If

            End If

            If Sezioni.Count > 0 Then
                Me.view.BindNavigatore(Sezioni)
                For Each oSezione As COL_Wiki.WikiNew.SezioneWiki In Sezioni
                    If first Then
                        TmpSezione = oSezione
                        first = False
                    End If
                    If oSezione.IsDefault Then
                        TmpSezioneDef = oSezione
                        HasDefault = True
                        Exit For
                    End If
                Next
                If HasDefault Then
                    Sezione = TmpSezioneDef
                Else
                    Sezione = TmpSezione
                End If
                Me.view.ActualSezioneId = Sezione.ID
                Return True
            Else
                Return False
            End If
        End Function
        Private Function BindNavigatoreWithSelectedSection(ByRef Sezione As WikiNew.SezioneWiki, ByVal oWiki As COL_Wiki.WikiNew.Wiki, Optional ByVal Forced As Boolean = False) As Boolean
            Dim Sezioni, sezioni2 As New ArrayList
            Dim TmpSezioneDef, TmpSezione As WikiNew.SezioneWiki

            Dim first As Boolean = True
            Dim HasDefault As Boolean = False

            Me.view.TitoloPagina = oWiki.Nome

            Sezioni = Me.oManagerWiki.CaricaSezioni(oWiki, Forced)
            If Sezioni.Count > 0 Then
                'Verifichiamo che sia l'amministratore
                '--- Solo non gestisce togliamo i topic eleiminati
                If Not (Me.view.Servizio.Admin Or Me.view.Servizio.GestioneTopics) Then
                    For i As Integer = 0 To Sezioni.Count - 1
                        Dim elemento As SezioneWiki = Sezioni.Item(i)
                        If Not elemento.IsDeleted Then
                            sezioni2.Add(Sezioni.Item(i))
                        End If
                    Next
                    Sezioni = sezioni2
                End If

            End If

            If Sezioni.Count > 0 Then
                Dim ID As System.Guid = Sezione.ID
                Me.view.BindNavigatore(Sezioni)
                If Not (From s In Sezioni Where DirectCast(s, WikiNew.SezioneWiki).ID = ID Select s).Any Then
                    For Each oSezione As COL_Wiki.WikiNew.SezioneWiki In Sezioni
                        If first Then
                            TmpSezione = oSezione
                            first = False
                        End If
                        If oSezione.IsDefault Then
                            TmpSezioneDef = oSezione
                            HasDefault = True
                            Exit For
                        End If
                    Next
                    If HasDefault Then
                        Sezione = TmpSezioneDef
                    Else
                        Sezione = TmpSezione
                    End If
                End If


                Me.view.ActualSezioneId = Sezione.ID
                Return True
            Else
                Return False
            End If
        End Function
        ''' <summary>
        ''' Raccoglie le informazioni di una sezione per visualizzarle in testa alla lista di topic
        ''' </summary>
        ''' <param name="SezioneID"></param>
        ''' <remarks></remarks>
        Private Sub BindSezione(ByRef SezioneID As System.Guid)
            Dim oSezione As New WikiNew.SezioneWiki
            oSezione.ID = SezioneID
            oSezione.Wiki = New COL_Wiki.WikiNew.Wiki
            oSezione.Wiki.ID = Me.view.ActualWikiId
            Me.oManagerWiki.CaricaSezione(oSezione)
            Me.view.BindSezione(oSezione)
        End Sub

        ''' <summary>
        ''' Raccoglie i dati dei Topic relativi ed aggiorna la vista
        ''' </summary>
        ''' <returns>TRUE: esistono Topic, FALSE: non esistono Topic</returns>
        ''' <remarks></remarks>
        Private Function BindTopicSezione(ByVal oSezione As WikiNew.SezioneWiki) As Boolean

            Dim ArlTopic As ArrayList = Me.oManagerWiki.CaricaTopics(oSezione)
            Dim ArlTopic2 As New ArrayList
            If ArlTopic.Count > 0 Then
                '--- Solo non gestisce togliamo i topic eleiminati e se non abbiamo attivata la visualizzazione dei cancellati
                If Not ((Me.view.Servizio.Admin Or Me.view.Servizio.GestioneTopics) And Me.view.ViewDeleted) Then
                    For i As Integer = 0 To ArlTopic.Count - 1
                        Dim elemento As COL_Wiki.WikiNew.TopicWiki = ArlTopic.Item(i)
                        If Not elemento.isCancellato Then
                            ArlTopic2.Add(ArlTopic.Item(i))
                        End If
                    Next
                    ArlTopic = ArlTopic2
                End If
                'Me.view.BindTopicsSezione(ArlTopic, oSezione)
            End If
            'Verifichimo se ci sono ancora topic pubblici
            If ArlTopic.Count > 0 Then
                Me.view.BindTopicsSezione(ArlTopic, oSezione)
                Return True
            Else
                Me.view.BindTopicsSezione(ArlTopic, oSezione)
                Return False
            End If
        End Function

        ''' <summary>
        ''' Raccoglie i dati del topic per la successiva modifica
        ''' </summary>
        ''' <param name="TopicID"></param>
        ''' <remarks></remarks>
        Private Sub BindTopic(ByVal TopicID As Guid)
            Dim oTopic As New WikiNew.TopicWiki
            oTopic.ID = TopicID
            Dim a As String = oTopic.ID.ToString
            oTopic.Sezione = New COL_Wiki.WikiNew.SezioneWiki

            Me.oManagerWiki.CaricaTopic(oTopic)
            Me.view.OriginalSezioneID = oTopic.Sezione.ID
            Me.view.BindTopicTest(oTopic)

            'Dim test As String = oTopic.Sezione.ID.ToString
            'Dim test2 As String = Me.view.OriginalSezioneID.ToString
        End Sub

        ''' <summary>
        ''' Raccoglie i dati della cronologia di un topic
        ''' </summary>
        ''' <param name="oTopic"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function BindCronologia(ByVal oTopic As WikiNew.TopicWiki) As Boolean
            Dim Lista As IList
            Lista = Me.oManagerWiki.CaricaStoriaTopic(oTopic)
            If Lista.Count > 0 Then
                Me.view.bindcronologia(Lista)
                Return True
            Else
                Return False
            End If
        End Function

#End Region

#Region "Clear, Elimina, Recover"

        Private Sub ClearSezione()
            Me.view.BindSezione(New WikiNew.SezioneWiki())
        End Sub
        Private Sub ClearTopic()
            Me.view.BindTopic(New WikiNew.TopicWiki)
            Me.view.ActualTopicId = Nothing
        End Sub

        Public Sub EliminaSezione()
            Dim oSezione As New COL_Wiki.WikiNew.SezioneWiki
            oSezione.ID = Me.view.ActualSezioneId
            oSezione.Wiki = New COL_Wiki.WikiNew.Wiki
            oSezione.Wiki.ID = Me.view.ActualWikiId

            'Necessario, altrimenti viene "arato" tutto il contenuto
            Me.oManagerWiki.CaricaSezione(oSezione)
            oSezione.Wiki.ID = Me.view.ActualWikiId
            oSezione.IsDeleted = True
            Me.oManagerWiki.SalvaSezione(oSezione)
            Me.view.NotifySectionDelete(view.MyComunitaCorrente.Id, view.MyPersonaCorrente.Anagrafica, oSezione.ID, oSezione.NomeSezione)
            Me.BindDati(Binding.SezioneGoto)
        End Sub
        Public Sub RecuperaSezione()
            Dim oSezione As New COL_Wiki.WikiNew.SezioneWiki
            oSezione.ID = Me.view.ActualSezioneId
            oSezione.Wiki = New COL_Wiki.WikiNew.Wiki
            oSezione.Wiki.ID = Me.view.ActualWikiId

            'Necessario, altrimenti viene "arato" tutto il contenuto
            Me.oManagerWiki.CaricaSezione(oSezione)
            oSezione.Wiki.ID = Me.view.ActualWikiId
            oSezione.IsDeleted = False
            Me.oManagerWiki.SalvaSezione(oSezione)

            Me.view.NotifySectionRipristina(view.MyComunitaCorrente.Id, oSezione.ID, oSezione.NomeSezione, oSezione.DataInserimento, view.MyPersonaCorrente.Anagrafica)
            Me.BindDati(Binding.SezioneGoto)
        End Sub

        Public Sub EliminaTopic()
            Dim oTopic As New COL_Wiki.WikiNew.TopicWiki
            oTopic.ID = Me.view.ActualTopicId
            oTopic.Sezione = New COL_Wiki.WikiNew.SezioneWiki
            oTopic.Sezione.ID = Me.view.ActualSezioneId

            'Necessario, altrimenti viene "arato" tutto il contenuto
            Me.oManagerWiki.CaricaTopic(oTopic)

            oManagerWiki.DeleteTopic(oTopic)

                'anche
                Me.BindDati(Binding.SezioneGoto)
                Me.view.NotifyTopicDelete(view.MyComunitaCorrente.Id, oTopic.ID, oTopic.Nome, view.MyPersonaCorrente.Anagrafica)
        End Sub

        Public Sub ForceEliminaTopic()
            Dim oTopic As New COL_Wiki.WikiNew.TopicWiki
            oTopic.ID = Me.view.ActualTopicId
            oTopic.Sezione = New COL_Wiki.WikiNew.SezioneWiki
            oTopic.Sezione.ID = Me.view.ActualSezioneId

            'Necessario, altrimenti viene "arato" tutto il contenuto
            Me.oManagerWiki.CaricaTopic(oTopic)


            Me.oManagerWiki.DeleteTopic(oTopic)

            'anche
            Me.BindDati(Binding.SezioneGoto)
            Me.view.NotifyTopicDelete(view.MyComunitaCorrente.Id, oTopic.ID, oTopic.Nome, view.MyPersonaCorrente.Anagrafica)
        End Sub
        Public Sub ripristinaTopic()
            Dim oTopic As New COL_Wiki.WikiNew.TopicWiki
            oTopic.ID = Me.view.ActualTopicId
            oTopic.Sezione = New COL_Wiki.WikiNew.SezioneWiki
            oTopic.Sezione.ID = Me.view.ActualSezioneId

            'Necessario, altrimenti viene "arato" tutto il contenuto
            Me.oManagerWiki.CaricaTopic(oTopic)
            Me.oManagerWiki.UnDeleteTopic(oTopic)
            'questo (reset)
            Me.view.NotifyTopicRipristina(view.MyComunitaCorrente.Id, oTopic.ID, oTopic.Nome, oTopic.DataInserimento, view.MyPersonaCorrente.Anagrafica)

            Me.BindDati(Binding.SezioneGoto)
        End Sub


        Public Sub RecoverTopicCrono(ByVal oTopicCrono As COL_Wiki.WikiNew.TopicHistoryWiki)
            'L'attuale TOPIC va in CRONO, il CRONO in TOPIC...

            Dim oTopic As New COL_Wiki.WikiNew.TopicWiki
            oTopic.ID = Me.view.ActualTopicId
            oTopic.Sezione = New COL_Wiki.WikiNew.SezioneWiki
            oTopic.Sezione.ID = Me.view.ActualSezioneId

            oTopicCrono.Topic = oTopic

            Me.oManagerWiki.CaricaTopic(oTopic)
            Me.oManagerWiki.CaricaTopicCrono(oTopicCrono)

            Dim oTopicCronoNew As New COL_Wiki.WikiNew.TopicHistoryWiki

            With oTopicCronoNew
                .ID = Guid.NewGuid
                .Contenuto = oTopic.Contenuto
                .DataModifica = Now()
                .isCancellato = oTopic.isCancellato
                .Nome = oTopic.Nome
                .Persona = oTopic.Persona
                .Topic = oTopic
                .IsNew = True
            End With

            With oTopic
                .Nome = oTopicCrono.Nome
                .Contenuto = oTopicCrono.Contenuto
                .Persona = Me.view.MyPersonaCorrente
                .DataModifica = Now()
                .isCancellato = oTopicCrono.isCancellato
                .Sezione = New COL_Wiki.WikiNew.SezioneWiki()
                .Sezione.ID = Me.view.ActualSezioneId
                .IsNew = False
            End With
            Dim allowsavequit As Boolean
            Me.oManagerWiki.SalvaTopicHistory(oTopicCronoNew)
            Me.oManagerWiki.SalvaTopic(oTopic, allowsavequit)
            Me.view.NotifyTopicRipristina(view.MyComunitaCorrente.Id, oTopic.ID, oTopic.Nome, oTopic.DataInserimento, view.MyPersonaCorrente.Anagrafica)

            Me.BindDati(Binding.SezioneGoto)
        End Sub

#End Region

        Public Function GetWikiID() As String
            Dim oWiki As New COL_Wiki.WikiNew.Wiki
            oWiki = Me.oManagerWiki.CaricaWiki()
            Me.view.ShowAuthors = oWiki.DisplayAuthors
            Return oWiki.ID.ToString
        End Function
#Region "Save or Update"
        'Ritorna se è un nuovo topic!!!
        Public Sub SaveOrUpdateTopic(ByVal Continua As Boolean)
            Dim IsNew As Boolean = (IsNothing(Me.view.ActualTopicId) OrElse Me.view.ActualTopicId = System.Guid.Empty)
            'Raccolta nuovi dati
            Dim oTopicNew As New COL_Wiki.WikiNew.TopicWiki
            '  


            oTopicNew.Sezione = New COL_Wiki.WikiNew.SezioneWiki

            If IsNew Then
                'IsNew = True
                'Dati per nuovo inserimento
                'With oTopicNew
                With oTopicNew
                    .ID = Guid.NewGuid
                    .DataInserimento = Now()
                    .isCancellato = Me.view.TopicIsCancellato
                    .IsPubblica = Me.view.TopicIsPubblico
                    'AGGIUNTO ORA !!!
                    .Sezione.ID = Me.view.ActualSezioneId
                    Me.view.ActualTopicId = .ID
                End With
                'Return True
            Else
                'IsNew = False
                'Dati per Aggiornamento
                oTopicNew.ID = Me.view.ActualTopicId

                oTopicNew.Sezione.ID = Me.view.OriginalSezioneID 'Me.view.ActualSezioneId

                Me.oManagerWiki.CaricaTopic(oTopicNew)

                'BackUp dati
                Dim oTopicCrono As New COL_Wiki.WikiNew.TopicHistoryWiki
                oTopicCrono.ID = Guid.NewGuid
                With oTopicNew
                    oTopicCrono.Contenuto = .Contenuto
                    oTopicCrono.DataModifica = .DataModifica
                    oTopicCrono.isCancellato = .isCancellato
                    oTopicCrono.Nome = .Nome
                    oTopicCrono.Persona = .Persona
                    oTopicCrono.Topic = oTopicNew
                    oTopicCrono.IsNew = True
                    Me.oManagerWiki.SalvaTopicHistory(oTopicCrono)
                End With
                'Return False
            End If
            With oTopicNew
                '.Sezione = New COL_Wiki.WikiNew.SezioneWiki
                '.Sezione.ID = Me.view.OriginalSezioneID 'Me.view.ActualSezioneId
                Dim test As String = .Sezione.ID.ToString
                .Sezione.Wiki = New COL_Wiki.WikiNew.Wiki

                'Aggiornamento nuovi dati
                .Contenuto = Me.view.TopicText
                .Nome = Me.view.TopicNome

                .DataModifica = Now
                .IsPubblica = Me.view.TopicIsPubblico
                '.Persona = New COL_BusinessLogic_v2.CL_persona.COL_Persona
                .Persona = Me.view.MyPersonaCorrente
                .IsNew = IsNew
            End With
            Dim allowsavequit As Boolean
            'Salvataggio:
            Me.oManagerWiki.SalvaTopic(oTopicNew, allowsavequit)

            If IsNew Then
                Me.view.NotifyTopicAdd(view.MyComunitaCorrente.Id, oTopicNew.ID, oTopicNew.Nome, view.MyPersonaCorrente.Anagrafica)
            Else
                Me.view.NotifyTopicEdit(view.MyComunitaCorrente.Id, oTopicNew.ID, oTopicNew.Nome, view.MyPersonaCorrente.Anagrafica)
            End If

            Me.view.ShowErrorMessage = Not allowsavequit

            If Continua = False Then
                Continua = Not allowsavequit
            End If

            'Aggiornamento vista
            If Continua Then
                Me.BindTopic(oTopicNew.ID)
                Me.view.Show(Visualizzazioni.AddTopic)
            Else
                Me.oManagerWiki.CaricaSezione(oTopicNew.Sezione) 'Carico i dati della sezione.
                'necessario agli aggiornamenti vari nella vista...
                Me.BindTopicSezione(oTopicNew.Sezione)
                Me.view.Show(Visualizzazioni.ListaTopic)
            End If
        End Sub

        Public Sub SwitchToList()

            Dim oTopicNew As New COL_Wiki.WikiNew.TopicWiki

            oTopicNew.Sezione = New COL_Wiki.WikiNew.SezioneWiki

            oTopicNew.ID = Me.view.ActualTopicId

            oTopicNew.Sezione.ID = Me.view.OriginalSezioneID 'Me.view.ActualSezioneId

            Me.oManagerWiki.CaricaTopic(oTopicNew)

            Me.oManagerWiki.CaricaSezione(oTopicNew.Sezione) 'Carico i dati della sezione.
            'necessario agli aggiornamenti vari nella vista...
            Me.BindTopicSezione(oTopicNew.Sezione)
            Me.view.Show(Visualizzazioni.ListaTopic)
        End Sub
        Public Sub SaveOrUpdateWiki()
            If Me.view.Servizio.Admin Or Me.view.Servizio.GestioneWiki Then
                Dim oWiki As New COL_Wiki.WikiNew.Wiki
                If Me.view.ActualWikiId = Guid.Empty Then
                    With oWiki
                        .ID = Guid.NewGuid
                        .Nome = Me.view.NomeWiki
                        .Comunita = Me.view.MyComunitaCorrente
                        .IsNew = True
                        .DisplayAuthors = Me.view.DisplayAuthors
                    End With
                Else
                    oWiki = Me.oManagerWiki.CaricaWiki(True)
                    oWiki.Nome = Me.view.NomeWiki
                    oWiki.IsNew = False
                    oWiki.DisplayAuthors = Me.view.DisplayAuthors
                    Me.view.ShowAuthors = oWiki.DisplayAuthors
                End If
                Me.view.ActualWikiId = oWiki.ID
                Me.oManagerWiki.SalvaWiki(oWiki)
            End If
            Me.BindDati(Binding.Reset)
        End Sub
        ' Server per vedere se è nuova o aggiornata
        Public Sub SaveOrUpdateSezione()
            If Me.view.Servizio.Admin Or Me.view.Servizio.GestioneSezioni Then
                Dim oSezione As New COL_Wiki.WikiNew.SezioneWiki
                Dim isNew As Boolean = False
                Dim OldName As String = ""
                If Me.view.ActualSezioneId = Guid.Empty Then
                    'Nuova Sezione
                    With oSezione
                        .ID = Guid.NewGuid
                        .DataInserimento = Now
                        .Persona = Me.view.MyPersonaCorrente
                        .Wiki = New COL_Wiki.WikiNew.Wiki
                        .Wiki.ID = Me.view.ActualWikiId
                        Dim strTest As String = .Wiki.ID.ToString
                        .NomeSezione = Me.view.SezioneNome
                        .Descrizione = Me.view.SezioneDescrizione
                        .PlainDescription = Me.view.SectionDescriptionText
                        .IsDefault = Me.view.SezioneIsDefault
                        .IsPubblica = Me.view.SezioneIsPubblica
                        .IsDeleted = False
                        .IsNew = True
                    End With
                    isNew = True
                    'Return True
                Else
                    oSezione.ID = Me.view.ActualSezioneId
                    oSezione.Wiki = New COL_Wiki.WikiNew.Wiki
                    oSezione.Wiki.ID = Me.view.ActualWikiId

                    Me.oManagerWiki.CaricaSezione(oSezione)
                    OldName = oSezione.NomeSezione
                    With oSezione
                        .Persona = Me.view.MyPersonaCorrente
                        .NomeSezione = Me.view.SezioneNome
                        .Descrizione = Me.view.SezioneDescrizione
                        .IsDefault = Me.view.SezioneIsDefault
                        .IsPubblica = Me.view.SezioneIsPubblica
                        .PlainDescription = Me.view.SectionDescriptionText
                        .IsNew = False
                    End With
                    'Return False
                End If
                Me.oManagerWiki.SalvaSezione(oSezione)
                If isNew Then
                    Me.view.NotifySectionAdd(Me.view.MyComunitaCorrente.Id, view.MyPersonaCorrente.Anagrafica, oSezione.NomeSezione, oSezione.ID)
                ElseIf OldName <> oSezione.NomeSezione Then
                    Me.view.NotifySectionEdit(Me.view.MyComunitaCorrente.Id, view.MyPersonaCorrente.Anagrafica, oSezione.ID, OldName, oSezione.NomeSezione)
                End If
            End If
            Me.BindDati(Binding.Reset)
        End Sub
#End Region

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

        Private Function BindImporta(ByRef IdTopic As Guid) As Boolean

            Dim oTopic As New WikiNew.TopicWiki
            oTopic.ID = IdTopic
            oTopic.Sezione = New COL_Wiki.WikiNew.SezioneWiki
            Me.oManagerWiki.CaricaTopic(oTopic)
            Dim test12 As String = oTopic.Contenuto
            With oTopic
                .ID = Guid.NewGuid
                .DataInserimento = Now()
                .isCancellato = False 'Me.view.TopicIsCancellato
                .IsPubblica = False
                .Sezione.ID = Me.view.ActualSezioneId

                Dim test As String = .Sezione.ID.ToString
                .Sezione.Wiki = New COL_Wiki.WikiNew.Wiki
                '.Contenuto = Me.view.TopicText
                '.Nome = Me.view.TopicNome
                .DataModifica = Now
                '.IsPubblica = Me.view.TopicIsPubblico
                '.Persona = Me.view.MyPersonaCorrente
                .IsNew = True
            End With
            Dim allowsavequit As Boolean
            Me.oManagerWiki.SalvaTopic(oTopic, allowsavequit)
            'Me.BindDati(Binding.SezioneGoto)
        End Function


    End Class

    ''' <summary>
    ''' Indica su cosa effettuare il bind dei dati
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum Binding
        Reset = 0 'Mostro l'elenco dei topic
        WikiAdd = 1
        WikiModify = 2 'non c'e...
        SezioneAdd = 11
        SezioneModify = 12
        SezioneGoto = 13
        TopicAdd = 20
        TopicModify = 21
        TopicCronologia = 22
        TopicView = 23
        TopicSearch = 24
        ComunityList = 25
        ImportTopics = 26
        ImportTopic = 27
        TopicViewForced = 28
        ResetToUrlSection = 29
    End Enum

End Namespace