Namespace WikiNew
    Public Class PresenterWikiHome
        Inherits PresenterGenerico

        Private view As IViewWikiHome
        Private oManagerWiki As COL_Wiki.WikiNew.iManagerWiki



        ''' <summary>
        ''' Inizializzazione presenter
        ''' </summary>
        ''' <param name="view">Per iniettare la view</param>
        ''' <param name="DbSource">Per inizializzare nHybernate o SQL: in deployment SOLO SQL</param>
        ''' <param name="UseCache">Indica se utilizzare o meno la cache</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal view As IViewWikiHome, Optional ByVal DbSource As FactoryWiki.ConnectionType = FactoryWiki.ConnectionType.SQL, Optional ByVal UseCache As Boolean = True)
            Me.view = view

            oManagerWiki = COL_Wiki.FactoryWiki.CreateManagerWikiHome2(DbSource, UseCache)

        End Sub

        Public Sub BindDati(ByVal Binding As Binding)
            Dim oSezione As COL_Wiki.WikiNew.SezioneWiki

            Select Case Binding
                Case WikiNew.Binding.Reset
                    If Not Me.BindNavigatore(oSezione) Then 'non Ho sezioni
                        Me.view.Show(Visualizzazioni.NoTopic)
                    Else    'ho sezioni
                        If Me.BindTopicSezione(oSezione) Then
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

                    If Me.BindTopicSezione(oSezione) Then
                        Me.view.Show(Visualizzazioni.ListaTopic)
                    Else
                        Me.view.Show(Visualizzazioni.NoTopic)
                    End If
                    '-----------Alessandro x ricerca di topics e visualizzaione singolo
                Case WikiNew.Binding.TopicSearch
                    If Me.BindCercaTopic(Me.view.SearchString) Then
                        Me.view.Show(VisualizzazioniHome.ListaTopicSearched)
                    Else
                        Me.view.Show(Visualizzazioni.NoTopic)
                    End If
                Case WikiNew.Binding.TopicView
                    Me.BindTopicTest(Me.view.ActualTopicId)
                    Me.view.Show(Visualizzazioni.ShowTopic)


            End Select
        End Sub


        ''' <summary>
        '''  Recupera i dati per il navigatore tra le sezione e forza il bind della vista su tale elemento
        ''' </summary>
        ''' <returns>TRUE se esistono sezioni, FALSE se non esistono sezioni per la wiki</returns>
        ''' <remarks></remarks>
        Private Function BindNavigatore(ByRef Sezione As WikiNew.SezioneWiki, Optional ByVal Forced As Boolean = False) As Boolean
            Dim Sezioni, Sezioni2 As New ArrayList
            Sezioni = Me.oManagerWiki.CaricaSezioniHome(Forced)
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
        Private Function BindTopicSezione(ByVal oSezione As WikiNew.SezioneWiki) As Boolean
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
        Private Function BindCercaTopic(ByVal text As String) As Boolean
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
        Private Sub BindTopicTest(ByVal TopicID As Guid)
            Dim oTopic As New WikiNew.TopicWiki
            oTopic.ID = TopicID
            oTopic.Sezione = New COL_Wiki.WikiNew.SezioneWiki
            oTopic.Sezione.ID = Me.view.ActualSezioneId

            Me.oManagerWiki.CaricaTopic(oTopic)
            Me.view.BindTopicTest(oTopic)
        End Sub

    End Class
End Namespace