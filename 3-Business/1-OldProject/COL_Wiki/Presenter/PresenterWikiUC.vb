Imports lm.Comol.Core.DomainModel

'Imports LM.Comol.Entities
'Imports LM.Comol.Business
Namespace WikiNew
    Public Class PresenterWikiUC
        'Inherits PresenterGenerico

        Private view As IViewWikiUC
        Private oManagerWiki As COL_Wiki.WikiNew.iManagerWiki 'WikiNew.AbstractManagerWiki  '

        ''' <summary>
        ''' Inizializzazione presenter
        ''' </summary>
        ''' <param name="view">Per iniettare la view</param>
        ''' <param name="DbSource">Per inizializzare nHybernate o SQL: in deployment SOLO SQL</param>
        ''' <param name="UseCache">Indica se utilizzare o meno la cache</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal view As IViewWikiUC, ByVal ocontext As iApplicationContext, Optional ByVal DbSource As FactoryWiki.ConnectionType = FactoryWiki.ConnectionType.SQL, Optional ByVal UseCache As Boolean = True)
            Me.view = view
            Dim IdComunita As Integer = -1
            Try
                IdComunita = Me.view.MyComunitaCorrente.Id
            Catch ex As Exception
            End Try
            If IdComunita > 0 Then
                oManagerWiki = COL_Wiki.FactoryWiki.CreateManagerWikiUC(DbSource, Me.view.MyPersonaCorrente, Me.view.MyComunitaCorrente, ocontext, False)
            Else
                Me.view.Show(VisualizzazioniUC.NoPermessi)
            End If

        End Sub

#Region "Binding"

        Public Sub BindDati(ByVal BindingTest As Binding)
            Dim oSezione As COL_Wiki.WikiNew.SezioneWiki

            Select Case BindingTest
                Case WikiNew.Binding.Reset  'Al primo caricamento della pagina
                    'Esiste la Wiki?
                    Dim oWiki As COL_Wiki.WikiNew.Wiki
                    Try
                        oWiki = Me.oManagerWiki.CaricaWiki(True)
                    Catch ex As Exception
                        oWiki = Nothing
                    End Try

                    If IsNothing(oWiki) Then 'non esiste la wiki
                        Me.view.ActualWikiId = System.Guid.Empty
                        If Me.view.Servizio.Admin Or Me.view.Servizio.GestioneWiki Then
                            Me.view.Show(VisualizzazioniUC.NoWiki)
                        Else
                            Me.view.Show(VisualizzazioniUC.NoPermessi)
                        End If
                    Else    'la Wiki Esiste
                        Me.view.ActualWikiId = oWiki.ID
                    End If

                Case WikiNew.Binding.TopicSearch
                    If Me.BindCercaTopic(Me.view.SearchString) Then
                        Me.view.Show(VisualizzazioniUC.ListaTopicSearched)
                    Else
                        Me.view.Show(VisualizzazioniUC.NoTopic)
                    End If
                Case WikiNew.BindingUC.TopicSearchComunita
                    If Me.BindCercaTopicComunita(Me.view.SearchString) Then
                        Me.view.Show(VisualizzazioniUC.ListaTopicSearched)
                    Else
                        Me.view.Show(VisualizzazioniUC.NoTopic)
                    End If
                Case WikiNew.BindingUC.TopicSearchComPub
                    If Me.BindCercaTopicComPub(Me.view.SearchString) Then
                        Me.view.Show(VisualizzazioniUC.ListaTopicSearched)
                    Else
                        Me.view.Show(VisualizzazioniUC.NoTopic)
                    End If



            End Select
        End Sub


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
            'Me.view.BindSezione(oSezione)
        End Sub
#End Region

#Region "Clear, Elimina, Recover"

        Private Sub ClearSezione()
            'Me.view.BindSezione(New WikiNew.SezioneWiki())
        End Sub
        Private Sub ClearTopic()
            'Me.view.BindTopic(New WikiNew.TopicWiki)
            Me.view.ActualTopicId = Nothing
        End Sub

#End Region

        Public Function GetWikiID() As String
            Dim oWiki As New COL_Wiki.WikiNew.Wiki
            oWiki = Me.oManagerWiki.CaricaWiki()
            Return oWiki.ID.ToString
        End Function


        ''' <summary>
        ''' Raccoglie i dati dei Topic relativi ed aggiorna la vista
        ''' </summary>
        ''' <returns>TRUE: esistono Topic, FALSE: non esistono Topic</returns>
        ''' <remarks></remarks>
        Private Function BindCercaTopic(ByVal text As String) As Boolean
            Dim rowIndex As Integer
            rowIndex = (Me.view.pageIndex * Me.view.pageSize) - (Me.view.pageSize - 1)
            Dim ATopic As ArrayList = Me.oManagerWiki.CercaTopic(Me.view.SearchString, rowIndex, Me.view.pageSize + 1)
            If ATopic.Count > 0 Then
                'Vedo in base a quanti risultati se è kl'ultima pagina
                If ATopic.Count = Me.view.pageSize + 1 Then
                    ATopic.RemoveAt(ATopic.Count - 1)
                    Me.view.IsLastPage = False
                Else
                    Me.view.IsLastPage = True
                End If
                Me.view.BindTopicsFunded(ATopic)

                Return True
            Else
                Return False
            End If
        End Function
        'Ritorno tutti i record senza l'ultimo
        '
        Private Function BindCercaTopicComunita(ByVal text As String) As Boolean
            Dim rowIndex As Integer
            rowIndex = (Me.view.pageIndex * Me.view.pageSize) - (Me.view.pageSize - 1)
            Dim ATopic As ArrayList = Me.oManagerWiki.CercaTopicComunita(Me.view.SearchString, rowIndex, Me.view.pageSize + 1)
            If ATopic.Count > 0 Then
                'Vedo in base a quanti risultati se è kl'ultima pagina
                If ATopic.Count = Me.view.pageSize + 1 Then
                    ATopic.RemoveAt(ATopic.Count - 1)
                    Me.view.IsLastPage = False
                Else
                    Me.view.IsLastPage = True
                End If
                Me.view.BindTopicsFunded(ATopic)

                Return True
            Else
                Return False
            End If

        End Function
        Private Function BindCercaTopicComPub(ByVal text As String) As Boolean
            'Calcolo della rowindex a partire dalla pagina e dalla dimensione
            Dim rowIndex As Integer
            rowIndex = (Me.view.pageIndex * Me.view.pageSize) - (Me.view.pageSize - 1)
            'Richedo la dimensione della pagina più grande per verificare se è l'ultima
            Dim ATopic As ArrayList = Me.oManagerWiki.CercaTopicComPub(Me.view.SearchString, rowIndex, Me.view.pageSize + 1)
            If ATopic.Count > 0 Then
                'Vedo in base a quanti risultati se è kl'ultima pagina
                If ATopic.Count = Me.view.pageSize + 1 Then
                    ATopic.RemoveAt(ATopic.Count - 1)
                    Me.view.IsLastPage = False
                Else
                    Me.view.IsLastPage = True
                End If
                Me.view.BindTopicsFunded(ATopic)

                Return True
            Else
                Return False
            End If


        End Function
        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

    End Class


    ''' <summary>
    ''' Indica su cosa effettuare il bind dei dati
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum BindingUC
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
        TopicSearch = 24 'solo in quelli pubblici
        TopicSearchComunita = 25 'Solo in quelli di comunità
        TopicSearchComPub = 26 'in entrambi
    End Enum

End Namespace