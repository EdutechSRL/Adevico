Imports COL_DataLayer

Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Core.DomainModel

Namespace WikiNew
    Public Class ManagerWikidB
        Inherits ObjectBase
        Implements iManagerWiki



#Region "Proprietà"
        Private _PersonaCorrente As COL_Persona
        Private _ComunitaCorrente As COL_Comunita
        Private _UseCache As Boolean
        Private _applicationContext As iApplicationContext

        Public Property ApplicationContext As iApplicationContext
            Get
                Return _applicationContext
            End Get
            Set(value As iApplicationContext)
                _applicationContext = value
            End Set
        End Property

        Public Property PersonaCorrente() As COL_Persona Implements iManagerWiki.PersonaCorrente
            Get
                Return _PersonaCorrente
            End Get
            Set(ByVal value As COL_Persona)
                _PersonaCorrente = value
            End Set
        End Property
        Public Property ComunitaCorrente() As COL_Comunita Implements iManagerWiki.ComunitaCorrente
            Get
                Return _ComunitaCorrente
            End Get
            Set(ByVal value As COL_Comunita)
                _ComunitaCorrente = value
            End Set
        End Property
        Public Property UseCache() As Boolean Implements iManagerWiki.UseCache
            Get
                Return _UseCache
            End Get
            Set(ByVal value As Boolean)
                _UseCache = value
            End Set
        End Property
#End Region

        Public Sub New(ByVal oPersonaCorrente As COL_Persona, ByVal oComunitaCorrente As COL_Comunita, ByVal Usecache As Boolean, ByVal ocontext As iApplicationContext)
            Me._ComunitaCorrente = oComunitaCorrente
            Me._PersonaCorrente = oPersonaCorrente
            Me._UseCache = Usecache
            Me.ApplicationContext = ocontext

        End Sub

        Public Sub New(ByVal Usecache As Boolean)
            Me._ComunitaCorrente = New COL_Comunita
            Me._PersonaCorrente = New COL_Persona
            Me._UseCache = Usecache
        End Sub

#Region "Caricamenti"
        ''' <summary>
        '''  Carica la Wiki della comunità impostata nel metodo NEW
        ''' </summary>
        ''' <param name="forced">
        ''' Se "TRUE" forza il caricamento da dB
        ''' </param>
        ''' <returns>L'oggetto Wiki con i parametri della Wiki della comunità</returns>
        ''' <remarks>Testare CACHE</remarks>
        Public Function CaricaWiki(Optional ByVal forced As Boolean = False) As WikiNew.Wiki Implements WikiNew.iManagerWiki.CaricaWiki

            Dim tmpWiki As WikiNew.Wiki
            If Me.UseCache Then
                ''Recupero l'ID corretto della cache
                Dim cacheKey As String = CachePolicy.Wiki(Me.ComunitaCorrente.Id)

                ''Se non esiste o se è forzato il recupero da db
                If ObjectBase.Cache(cacheKey) Is Nothing Or forced Then

                    'Recupero da dB
                    tmpWiki = Me.CaricaWikidB()
                    'Lo inserisco nella cache
                    ObjectBase.Cache.Insert(cacheKey, tmpWiki, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, New TimeSpan(0, 30, 0))
                Else
                    'Altrimenti recupero dalla cache
                    tmpWiki = CType(ObjectBase.Cache(cacheKey), WikiNew.Wiki)
                End If
            Else
                'Recupero da dB
                tmpWiki = Me.CaricaWikidB()
            End If
            tmpWiki.DisplayAuthors = True

            Return tmpWiki
        End Function
        Private Function CaricaWikidB() As WikiNew.Wiki

            Dim tmpWiki As New WikiNew.Wiki
            Dim oComunita As New COL_Comunita(Me.ComunitaCorrente.Id)

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim responso As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "NEW_sp_Wiki_Estrai"
                .CommandType = CommandType.StoredProcedure

                oParam = objAccesso.GetAdvancedParameter("@CMNT_Id", Me.ComunitaCorrente.Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@WIKI_id", "", ParameterDirection.Output, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@WIKI_nome", "", ParameterDirection.Output, SqlDbType.VarChar, , 100)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                'objAccesso.GetExecuteNotQuery(oRequest)
                responso = objAccesso.GetExecuteNotQuery(oRequest)

                With tmpWiki
                    .ID = New System.Guid(oRequest.GetValueFromParameter(2))
                    .Nome = oRequest.GetValueFromParameter(3)
                    .Comunita = oComunita
                    'Eventualmente .estrai, se serve...
                End With
                Dim str As String = tmpWiki.ID.ToString

            Catch ex As Exception
                tmpWiki = Nothing
            End Try

            Return tmpWiki
        End Function

        ''' <summary>
        '''  Carica le sezioni di una data Wiki
        ''' </summary>
        ''' <param name="oWiki">Wiki da cui caricare le sezioni</param>
        ''' <param name="forced">Forza il caricamento da dB</param>
        ''' <returns>Una lista delle sezioni relative alla Wiki</returns>
        ''' <remarks>Testare CACHE</remarks>
        Public Function CaricaSezioni(ByVal oWiki As WikiNew.Wiki, Optional ByVal forced As Boolean = False) As IList Implements iManagerWiki.CaricaSezioni

            Dim tmpSezioni As ArrayList

            If Me.UseCache Then
                Dim cacheKey As String = CachePolicy.WikiSezioni(oWiki.ID.ToString)
                If ObjectBase.Cache(cacheKey) Is Nothing Or forced Then
                    tmpSezioni = CaricaSezionidB(oWiki)

                    ObjectBase.Cache.Insert(cacheKey, tmpSezioni, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, New TimeSpan(0, 30, 0))
                Else
                    tmpSezioni = CType(ObjectBase.Cache(cacheKey), ArrayList)
                End If
            Else
                tmpSezioni = CaricaSezionidB(oWiki)
            End If

            Return tmpSezioni
        End Function
        Private Function CaricaSezionidB(ByVal oWiki As WikiNew.Wiki) As ArrayList

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataReader As IDataReader
            Dim objAccesso As New COL_DataAccess

            Dim ARsezioni As New ArrayList()

            With oRequest
                .Command = "NEW_sp_Wiki_CaricaSezioni"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@Wiki_Id", oWiki.ID.ToString, ParameterDirection.Input, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                oDataReader = objAccesso.GetdataReader(oRequest)
                Dim DataNull As New DateTime(2000, 1, 1)

                While oDataReader.Read
                    Dim oSezione As New WikiNew.SezioneWiki
                    With oSezione
                        .ID = New System.Guid(oDataReader("WKSZ_id").ToString)
                        .NomeSezione = oDataReader("WKSZ_nome")
                        .DataInserimento = GenericValidator.ValData(oDataReader("WKSZ_dataInserimento"), DataNull)
                        .Persona = New COL_Persona(oDataReader("WKSZ_PRSN_id"))

                        .IsDeleted = GenericValidator.ValBool(oDataReader("WKSZ_isDeleted"), False)
                        .IsDefault = GenericValidator.ValBool(oDataReader("WKSZ_isDefault"), False)
                        .Descrizione = GenericValidator.ValString(oDataReader("WKSZ_Descrizione"), "")
                        .PlainDescription = GenericValidator.ValString(oDataReader("PlainDescription"), "")
                        .IsPubblica = GenericValidator.ValBool(oDataReader("WKSZ_isPubblica"), False)
                        .Persona.Nome = GenericValidator.ValString(oDataReader("PRSN_nome"), "")
                        .Persona.Cognome = GenericValidator.ValString(oDataReader("PRSN_cognome"), "")
                    End With
                    ARsezioni.Add(oSezione)
                End While
            Catch ex As Exception

            End Try

            'Dim oSezione As New WikiNew.SezioneWiki
            '.Add(oSezione)
            Return ARsezioni
        End Function

        ''' <summary>
        ''' Carica TUTTI i TOPIC di una data sezione
        ''' </summary>
        ''' <param name="oSezione">Sezione di cui caricare i Topic.</param>
        ''' <param name="forced">Forza il caricamento da dB</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CaricaTopics( _
         ByVal oSezione As WikiNew.SezioneWiki, _
         Optional ByVal forced As Boolean = False _
         ) As IList Implements iManagerWiki.CaricaTopics

            Dim tmpTopic As IList
            If Me.UseCache Then
                Dim cacheKey As String = CachePolicy.WikiTopics(oSezione.ID.ToString)
                If ObjectBase.Cache(cacheKey) Is Nothing Or forced Then
                    tmpTopic = CaricaTopicsdB(oSezione)

                    ObjectBase.Cache.Insert(cacheKey, tmpTopic, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, New TimeSpan(0, 30, 0))
                Else
                    tmpTopic = CType(ObjectBase.Cache(cacheKey), IList)
                End If
            Else
                tmpTopic = CaricaTopicsdB(oSezione)
            End If

            Return tmpTopic '
        End Function
        Private Function CaricaTopicsdB(ByVal oSezione As WikiNew.SezioneWiki) As IList

            Dim ListaTopic As New ArrayList() 'IList(Of WikiNew.TopicWiki)

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataReader As IDataReader
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "NEW_sp_Wiki_CaricaTopics" 'Sezione_Id
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@Sezione_Id", oSezione.ID.ToString, ParameterDirection.Input, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                oDataReader = objAccesso.GetdataReader(oRequest)
                Dim DataNull As New DateTime(2000, 1, 1)

                While oDataReader.Read
                    Dim oTopic As New WikiNew.TopicWiki
                    Dim oSezioneTmp As New WikiNew.SezioneWiki

                    With oTopic
                        .ID = New System.Guid(oDataReader("WKTP_id").ToString)
                        oSezione.ID = oDataReader("WKTP_WKSZ_id")
                        .Sezione = oSezioneTmp
                        .Contenuto = oDataReader("WKTP_contenuto")
                        .Nome = oDataReader("WKTP_nome")
                        .DataInserimento = GenericValidator.ValData(oDataReader("WKTP_dataInserimento"), DataNull)
                        .DataModifica = GenericValidator.ValData(oDataReader("WKTP_dataModifica"), DataNull)
                        .Persona = New COL_Persona(oDataReader("WKTP_PRSN_id"))
                        .Persona.Nome = oDataReader("PRSN_nome")
                        .Persona.Cognome = oDataReader("PRSN_cognome")
                        .isCancellato = GenericValidator.ValBool(oDataReader("WKTP_isDeleted"), False)
                        .IsPubblica = GenericValidator.ValBool(oDataReader("WKTP_isPubblica"), False)
                    End With

                    ListaTopic.Add(oTopic)
                End While

            Catch ex As Exception
            Finally
                Try
                    If oDataReader.IsClosed = False Then
                        oDataReader.Close()
                    End If
                Catch ex As Exception
                End Try
            End Try

            Return ListaTopic
        End Function

        ''' <summary>
        ''' Carica lo storico di un Topic
        ''' </summary>
        ''' <param name="oTopic">Topic di cui caricare lo storico</param>
        ''' <param name="forced">Forza il caricamento da dB</param>
        ''' <returns>Lista della cronologia</returns>
        ''' <remarks>Testare CACHE</remarks>
        Public Function CaricaStoriaTopic( _
         ByVal oTopic As WikiNew.TopicWiki, _
         Optional ByVal forced As Boolean = False _
         ) As IList Implements iManagerWiki.CaricaStoriaTopic

            Dim tmpTopicHistory As IList
            If Me.UseCache Then
                Dim cacheKey As String = CachePolicy.WikiTopicsHistory(oTopic.ID.ToString)
                If ObjectBase.Cache(cacheKey) Is Nothing Or forced Then
                    tmpTopicHistory = CaricaStoriaTopicdB(oTopic)

                    ObjectBase.Cache.Insert(cacheKey, tmpTopicHistory, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, New TimeSpan(0, 30, 0))
                Else
                    tmpTopicHistory = CType(ObjectBase.Cache(cacheKey), IList)
                End If
            Else
                tmpTopicHistory = CaricaStoriaTopicdB(oTopic)

            End If
            Return tmpTopicHistory
        End Function
        Private Function CaricaStoriaTopicdB(ByVal oTopic As WikiNew.TopicWiki) As IList

            Dim oStoriaTopic As New List(Of WikiNew.TopicHistoryWiki)

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataReader As IDataReader
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "NEW_sp_Wiki_CaricaStoriaTopic" 'Sezione_Id
                .CommandType = CommandType.StoredProcedure
                Dim IdStr As String = oTopic.ID.ToString
                oParam = objAccesso.GetAdvancedParameter("@Topic_Id", oTopic.ID.ToString, ParameterDirection.Input, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Dim DataNull As New DateTime(2000, 1, 1)

            Try
                oDataReader = objAccesso.GetdataReader(oRequest)
                While oDataReader.Read
                    Dim oTopicHistoryWiki As New WikiNew.TopicHistoryWiki
                    With oTopicHistoryWiki
                        ' , , , , , 
                        .ID = New System.Guid(oDataReader("WKTH_id").ToString)
                        .Topic = New WikiNew.TopicWiki
                        .Topic.ID = oDataReader("WKTH_WKTP_id")
                        .Contenuto = oDataReader("WKTH_contenuto")
                        .Nome = oDataReader("WKTH_nome")
                        .DataModifica = GenericValidator.ValData(oDataReader("WKTH_dataModifica"), DataNull)
                        .Persona = New COL_Persona(CInt(oDataReader("WKTH_PRSN_id")))
                        .Persona.Nome = oDataReader("PRSN_nome")
                        .Persona.Cognome = oDataReader("PRSN_cognome")
                        .isCancellato = GenericValidator.ValBool(("WKTH_isDeleted"), False)
                    End With
                    oStoriaTopic.Add(oTopicHistoryWiki)
                End While
            Catch ex As Exception
            End Try



            Return oStoriaTopic

        End Function

        ''' <summary>
        ''' Carica TUTTI i Topic relativi ad una WIKI.
        ''' </summary>
        ''' <param name="oWiki"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' ATTENZIONE!
        ''' Al momento NON viene utilizzata. 
        ''' Puo' venire comoda per caricare TUTTI i dati in cache,
        ''' M A ( ! ! ! )
        ''' effettua più chiamate al dB nel caso NON ci siano i DATI in cache,
        ''' o nel caso FORCED sia TRUE
        ''' </remarks>
        Public Function CaricaAllTopicWiki(ByVal oWiki As WikiNew.Wiki, Optional ByVal Forced As Boolean = False) As IList Implements iManagerWiki.CaricaAllTopicWiki
            Dim Arraycompleto As New ArrayList

            If Me.UseCache Then
                Dim AllSezioni As IList
                AllSezioni = CaricaSezioni(oWiki, Forced)
                For Each item As WikiNew.SezioneWiki In AllSezioni
                    For Each topic As WikiNew.TopicWiki In CaricaTopics(item, Forced)
                        Arraycompleto.Add(topic)
                    Next
                Next
                Arraycompleto.Sort(New Util.GenericNestedComparer("DataModifica", Util.GenericNestedComparer.OrderEnum.Descending))
            Else
                Arraycompleto = CaricaAllTopicWikidB(oWiki)
            End If

            Return Arraycompleto
        End Function
        Private Function CaricaAllTopicWikidB(ByVal oWiki As WikiNew.Wiki) As ArrayList
            Dim arTopic As New ArrayList

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataReader As IDataReader
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "NEW_sp_Wiki_CaricaAllTopic" 'Sezione_Id
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@Wiki_Id", oWiki.ID.ToString, ParameterDirection.Input, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Dim DataNull As New DateTime(2000, 1, 1)

            Try
                oDataReader = objAccesso.GetdataReader(oRequest)
                While oDataReader.Read
                    Dim oTopicWiki As New WikiNew.TopicWiki

                    With oTopicWiki
                        .ID = New System.Guid(oDataReader("WKTP_id").ToString)
                        .Sezione = New WikiNew.SezioneWiki
                        .Sezione.ID = oDataReader("WKTP_WKSZ_id")
                        .Contenuto = oDataReader("WKTP_contenuto")
                        .Nome = oDataReader("WKTP_nome")
                        .DataInserimento = GenericValidator.ValData(oDataReader("WKTP_dataInserimento"), DataNull)
                        .DataModifica = GenericValidator.ValData(oDataReader("WKTP_dataModifica"), DataNull)
                        .Persona.ID = oDataReader("WKTP_PRSN_id")
                        .isCancellato = GenericValidator.ValBool(oDataReader("WKTP_isDeleted"), False)
                    End With

                    arTopic.Add(oTopicWiki)
                End While
            Catch ex As Exception
            End Try

            Return arTopic
        End Function

        ''' <summary>
        ''' Carica tutti i dati di una singola sezione
        ''' </summary>
        ''' <param name="oSezione">
        ''' ByRef la sezione di cui caricare i dati. 
        ''' E' necessario che l'ID della sezione e della wiki siano impostati.
        ''' </param>
        ''' <remarks>
        ''' Per la CACHE è NECESSARIO avere il WIKI.ID da cui vado a recuperare la lista sezioni da cui carico la singola sezione...</remarks>
        Public Sub CaricaSezione(ByRef oSezione As SezioneWiki) Implements iManagerWiki.CaricaSezione
            If Me.UseCache Then
                'Me.CaricaSezionedB(oSezione)

                Dim cacheKey As String = CachePolicy.WikiSezioni(oSezione.Wiki.ID.ToString)

                Dim oSezioni As IList
                Try
                    oSezioni = ObjectBase.Cache(cacheKey)
                Catch ex As Exception
                    oSezioni = Nothing
                End Try

                If Not IsNothing(oSezioni) Then
                    For Each item As WikiNew.SezioneWiki In oSezioni
                        If item.ID = oSezione.ID Then
                            With oSezione
                                .NomeSezione = item.NomeSezione
                                .Descrizione = item.Descrizione
                                .PlainDescription = item.PlainDescription
                                .DataInserimento = item.DataInserimento
                                .LastModify = item.LastModify

                                .IsDefault = item.IsDeleted
                                .IsDeleted = item.IsDeleted
                                .IsPubblica = item.IsPubblica

                                If Not IsNothing(item.Persona) Then
                                    If IsNothing(.Persona) Then
                                        .Persona = New COL_Persona
                                    End If
                                    .Persona = item.Persona
                                End If
                                If Not IsNothing(item.Wiki) Then
                                    If IsNothing(.Wiki) Then
                                        .Wiki = New WikiNew.Wiki
                                    End If
                                    .Wiki = item.Wiki
                                End If
                            End With
                            'oSezione = item
                            Exit Sub
                        End If
                    Next
                End If
                'Se lo trova, lo carica ed esce dalla sub.
                'Se non lo trova, non c'e' in cache e/o non sto usando la cache, lo carico da dB
            End If
            Me.CaricaSezionedB(oSezione)
        End Sub
        Public Sub CaricaSezionedB(ByRef oSezione As SezioneWiki)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataReader As IDataReader = Nothing
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "SELECT WKSZ_id,WKSZ_WIKI_id,WKSZ_nome,WKSZ_dataInserimento,WKSZ_PRSN_id,WKSZ_isDeleted,WKSZ_isDefault,WKSZ_Descrizione,PlainDescription,WKSZ_isPubblica,PRSN_nome,PRSN_cognome FROM WIKI_SEZIONE Left Join PERSONA ON WIKI_SEZIONE.WKSZ_PRSN_id = PERSONA.PRSN_id "
                .Command &= " WHERE (WKSZ_id = '" + oSezione.ID.ToString() + "')"
                .CommandType = CommandType.Text

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Dim DataNull As New DateTime(2000, 1, 1)

            Try
                oDataReader = objAccesso.GetdataReader(oRequest)
                While oDataReader.Read
                    With oSezione
                        .ID = New System.Guid(oDataReader("WKSZ_id").ToString)


                        .Wiki = New WikiNew.Wiki
                        .Wiki.ID = New Guid(oDataReader("WKSZ_WIKI_id").ToString)
                        .NomeSezione = oDataReader("WKSZ_nome")
                        .DataInserimento = oDataReader("WKSZ_dataInserimento")
                        .Persona = New COL_Persona(oDataReader("WKSZ_PRSN_id"))
                        .IsDeleted = GenericValidator.ValBool(oDataReader("WKSZ_isDeleted"), False)
                        .IsDefault = GenericValidator.ValBool(oDataReader("WKSZ_isDefault"), False)
                        .Descrizione = oDataReader("WKSZ_Descrizione")
                        .PlainDescription = oDataReader("PlainDescription")
                        .IsPubblica = GenericValidator.ValBool(oDataReader("WKSZ_isPubblica"), True)
                        .Persona.Nome = oDataReader("PRSN_nome")
                        .Persona.Cognome = oDataReader("PRSN_cognome")
                    End With

                End While
            Catch ex As Exception
            Finally
                If Not IsNothing(oDataReader) Then
                    oDataReader.Close()
                End If
            End Try

            'Return arTopic

            'Dim oRequest As New COL_Request
            'Dim oParam As New COL_Request.Parameter
            'Dim responso As Integer
            'Dim objAccesso As New COL_DataAccess

            'With oRequest
            '    .Command = "NEW_sp_Wiki_Sezione_Estrai"
            '    .CommandType = CommandType.StoredProcedure

            '    '1 - @WKSZ_id as varchar(40)
            '    oParam = objAccesso.GetAdvancedParameter("@WKSZ_id", oSezione.ID.ToString, ParameterDirection.Input, SqlDbType.VarChar, , 40)
            '    .Parameters.Add(oParam)

            '    '2 - @WKSZ_WIKI_id as varchar(40) output
            '    oParam = objAccesso.GetAdvancedParameter("@WKSZ_WIKI_id", "", ParameterDirection.Output, SqlDbType.VarChar, , 40)
            '    .Parameters.Add(oParam)

            '    '3 - @WKSZ_nome as varchar(50) output
            '    oParam = objAccesso.GetAdvancedParameter("@WKSZ_nome", "", ParameterDirection.Output, SqlDbType.NVarChar, , 100)
            '    .Parameters.Add(oParam)

            '    '4 - @WKSZ_dataInserimento as datetime output
            '    oParam = objAccesso.GetAdvancedParameter("@WKSZ_dataInserimento", "", ParameterDirection.Output, SqlDbType.DateTime)
            '    .Parameters.Add(oParam)

            '    '5 - @WKSZ_PRSN_id as int output
            '    oParam = objAccesso.GetAdvancedParameter("@WKSZ_PRSN_id", "", ParameterDirection.Output, SqlDbType.Int)
            '    .Parameters.Add(oParam)

            '    '6 - @WKSZ_isDeleted as bit output
            '    oParam = objAccesso.GetAdvancedParameter("@WKSZ_isDeleted", "", ParameterDirection.Output, SqlDbType.Bit)
            '    .Parameters.Add(oParam)

            '    '7 - @WKSZ_isDefault as bit output
            '    oParam = objAccesso.GetAdvancedParameter("@WKSZ_isDefault", "", ParameterDirection.Output, SqlDbType.Bit)
            '    .Parameters.Add(oParam)

            '    '8 - @WKSZ_Descrizione as nvarchar(100) output
            '    oParam = objAccesso.GetAdvancedParameter("@WKSZ_Descrizione", "", ParameterDirection.Output, SqlDbType.NVarChar, , -1)
            '    .Parameters.Add(oParam)

            '    oParam = objAccesso.GetAdvancedParameter("@PlainDescription", "", ParameterDirection.Output, SqlDbType.NVarChar, , -1)
            '    .Parameters.Add(oParam)

            '    '9 - @WKSZ_isPubblica as bit output
            '    oParam = objAccesso.GetAdvancedParameter("@WKSZ_isPubblica", "", ParameterDirection.Output, SqlDbType.Bit)
            '    .Parameters.Add(oParam)

            '    '10 - @PRSN_nome
            '    oParam = objAccesso.GetAdvancedParameter("@PRSN_nome", "", ParameterDirection.Output, SqlDbType.NVarChar, , 40)
            '    .Parameters.Add(oParam)

            '    '11 - @PRSN_cognome
            '    oParam = objAccesso.GetAdvancedParameter("@PRSN_cognome", "", ParameterDirection.Output, SqlDbType.NVarChar, , 40)
            '    .Parameters.Add(oParam)



            '    .Role = COL_Request.UserRole.Admin
            '    .transactional = False
            'End With
            'Try
            '    responso = objAccesso.GetExecuteNotQuery(oRequest)
            '    Dim DefaultDate As New DateTime(2000, 1, 1)
            '    With oSezione
            '        .Wiki = New WikiNew.Wiki
            '        .Wiki.ID = New Guid(oRequest.GetValueFromParameter(2).ToString)
            '        .NomeSezione = oRequest.GetValueFromParameter(3)
            '        .DataInserimento = GenericValidator.ValData(oRequest.GetValueFromParameter(4), DefaultDate)
            '        .Persona = New COL_Persona(oRequest.GetValueFromParameter(5))
            '        .IsDeleted = GenericValidator.ValBool(oRequest.GetValueFromParameter(6), False)
            '        .IsDefault = GenericValidator.ValBool(oRequest.GetValueFromParameter(7), False)
            '        .Descrizione = oRequest.GetValueFromParameter(8)
            '        .PlainDescription = oRequest.GetValueFromParameter(9)
            '        .IsPubblica = GenericValidator.ValBool(oRequest.GetValueFromParameter(10), True)
            '        .Persona.Nome = oRequest.GetValueFromParameter(11)
            '        .Persona.Cognome = oRequest.GetValueFromParameter(12)

            '    End With
            'Catch ex As Exception

            'End Try
        End Sub

        ''' <summary>
        ''' Carica tutti i dati di un singolo topic
        ''' </summary>
        ''' <param name="oTopic">
        ''' ByRef del Topic di cui caricare i dati. 
        ''' E' necessario che l'ID del Topic e della relativa Sezioni siano impostati.
        ''' </param>
        ''' <remarks>
        ''' Testare CACHE
        ''' Per la CACHE si rende necessario l'ID della sezione
        ''' </remarks>
        Public Sub CaricaTopic(ByRef oTopic As TopicWiki) Implements WikiNew.iManagerWiki.CaricaTopic
            If Me.UseCache Then
                Dim cacheKey As String = CachePolicy.WikiTopics(oTopic.Sezione.ID.ToString)

                Dim oTopics As IList
                Try
                    oTopics = ObjectBase.Cache(cacheKey)
                Catch ex As Exception
                    oTopics = Nothing
                End Try

                If Not IsNothing(oTopics) Then
                    For Each item As WikiNew.TopicWiki In oTopics
                        If item.ID = oTopic.ID Then
                            With oTopic
                                'Se faccio "oTopic = item", l'oggetto oTopic non rimane lo stesso e
                                'fuori si incasina!!!

                                .DataModifica = item.DataModifica
                                .DataInserimento = item.DataInserimento
                                .Contenuto = item.Contenuto
                                .isCancellato = item.isCancellato
                                .IsPubblica = item.IsPubblica
                                .Nome = item.Nome
                                .Persona = New COL_Persona(item.Persona.ID, item.Persona.Nome, item.Persona.Cognome)
                                If IsNothing(.Sezione) Then
                                    .Sezione = New WikiNew.SezioneWiki
                                End If
                                If Not item.Sezione.ID = System.Guid.Empty Then
                                    'Se il guid non è vuoto lo riassegno, altrimenti tengo l'originale
                                    .Sezione.ID = item.Sezione.ID
                                End If
                            End With

                            Exit Sub
                        End If
                    Next
                End If
            End If
            Me.CaricaTopicdB(oTopic)
        End Sub
        Private Sub CaricaTopicdB(ByRef oTopic As TopicWiki)

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "NEW_sp_Wiki_Topic_Estrai"
                .CommandType = CommandType.StoredProcedure
                '1
                oParam = objAccesso.GetAdvancedParameter("@WKTP_id", oTopic.ID.ToString, ParameterDirection.Input, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                Dim oDataset As New DataSet


                '@WKTP_contenuto
                '@WKTP_nome
                '@WKTP_dataInserimento
                '@WKTP_dataModifica
                '@WKTP_PRSN_id
                '@WKTP_isDeleted
                '@WKTP_WKSZ_id
                '@WKTP_isPubblica

                oDataset = objAccesso.GetdataSet(oRequest)
                If oDataset.Tables(0).Rows.Count = 1 Then
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(0)

                    Dim DefaultDate As New DateTime(2000, 1, 1)

                    With oTopic
                        '.ID = New System.Guid(oRequest.GetValueFromParameter(1))
                        .Contenuto = oRow.Item("WKTP_contenuto")
                        .Nome = oRow.Item("WKTP_nome")
                        .DataInserimento = GenericValidator.ValData(oRow.Item("WKTP_dataInserimento"), DefaultDate)
                        .DataModifica = GenericValidator.ValData(oRow.Item("WKTP_dataModifica"), DefaultDate)
                        .Persona = New COL_Persona()
                        .Persona.ID = oRow.Item("WKTP_PRSN_id")
                        .isCancellato = oRow.Item("WKTP_isDeleted")
                        .Sezione = New COL_Wiki.WikiNew.SezioneWiki()
                        .Sezione.ID = New Guid(oRow.Item("WKTP_WKSZ_id").ToString)
                        .IsPubblica = GenericValidator.ValBool(oRow.Item("WKTP_isPubblica"), True)
                        .Persona.Nome = oRow.Item("PRSN_nome")
                        .Persona.Cognome = oRow.Item("PRSN_cognome")
                    End With

                    'Me.n_Errore = Errori_Db.None
                Else
                    'Me.n_Errore = Errori_Db.DBReadExist
                End If
            Catch ex As Exception
                'Me.n_Errore = Errori_Db.DBReadExist
                '_Creatore = Nothing
            End Try

        End Sub

        ''' <summary>
        ''' Carica una specifica cronologia
        ''' </summary>
        ''' <param name="oTopicCrono"></param>
        ''' <param name="forced"></param>
        ''' <remarks></remarks>
        Public Sub CaricaTopicCrono(ByRef oTopicCrono As TopicHistoryWiki, Optional ByVal forced As Boolean = False) Implements iManagerWiki.CaricaTopicCrono
            If Me.UseCache Then
                Dim cacheKey As String = CachePolicy.WikiTopicsHistory(oTopicCrono.Topic.ID.ToString)
                Dim oTopicsCrono As IList

                Try
                    oTopicsCrono = ObjectBase.Cache(cacheKey)
                Catch ex As Exception
                    oTopicsCrono = Nothing
                End Try

                If Not IsNothing(oTopicsCrono) Then
                    For Each item As WikiNew.TopicHistoryWiki In oTopicsCrono
                        If item.ID = oTopicCrono.ID Then
                            With oTopicCrono
                                .Contenuto = item.Contenuto
                                .DataModifica = item.DataModifica
                                .isCancellato = item.isCancellato
                                '.IsNew = item.IsNew
                                .Nome = item.Nome
                                .Persona = item.Persona
                                .Topic = item.Topic
                            End With
                            'oTopicCrono = item
                            Exit Sub
                        End If
                    Next
                End If
            End If
            Me.dBCaricaTopicCrono(oTopicCrono)
        End Sub
        Private Sub dBCaricaTopicCrono(ByRef oTopicCrono As COL_Wiki.WikiNew.TopicHistoryWiki)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            'Dim oRequest As New COL_Request
            'Dim oParam As New COL_Request.Parameter
            'Dim responso As Integer
            'Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "NEW_sp_Wiki_TopicCrono_Estrai"
                .CommandType = CommandType.StoredProcedure

                '1
                oParam = objAccesso.GetAdvancedParameter("@WKTH_id", oTopicCrono.ID.ToString, ParameterDirection.Input, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                Dim oDataset As New DataSet


                oDataset = objAccesso.GetdataSet(oRequest)
                If oDataset.Tables(0).Rows.Count = 1 Then
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(0)

                    Dim DefaultDate As New DateTime(2000, 1, 1)


                    'Try
                    '    'objAccesso.GetExecuteNotQuery(oRequest)
                    '    responso = objAccesso.GetExecuteNotQuery(oRequest)
                    '    'oTopic = New COL_Wiki.WikiNew.TopicWiki
                    '                 WKTH_WKTP_id()
                    '   ,WKTH_contenuto
                    '   ,WKTH_nome
                    '   ,WKTH_dataModifica
                    '   ,WKTH_PRSN_id
                    '   ,WKTH_isDeleted
                    ',PRSN_nome
                    ',PRSN_cognome
                    With oTopicCrono
                        .Topic = New WikiNew.TopicWiki
                        .Topic.ID = New Guid(oRow.Item("WKTH_WKTP_id").ToString)
                        .Contenuto = oRow.Item("WKTH_contenuto")
                        .Nome = oRow.Item("WKTH_nome")
                        .DataModifica = oRow.Item("WKTH_dataModifica")
                        .Persona = New COL_Persona()
                        .Persona.ID = oRow.Item("WKTH_PRSN_id")
                        .Persona.Nome = oRow.Item("PRSN_nome")
                        .Persona.Cognome = oRow.Item("PRSN_cognome")
                        .isCancellato = oRow.Item("WKTH_isDeleted")
                    End With


                Else

                    'Me.n_Errore = Errori_Db.DBReadExist
                End If
            Catch ex As Exception

            End Try


        End Sub
#End Region

#Region "Salvataggi"
        ''' <summary>
        ''' Salva la Wiki, aggiornando dB e CACHE se necessario.
        ''' </summary>
        ''' <param name="oWiki"></param>
        ''' <remarks>
        ''' Testare Cache
        ''' </remarks>
        Public Sub SalvaWiki(ByVal oWiki As WikiNew.Wiki) Implements iManagerWiki.SalvaWiki
            Me.SalvaWikidb(oWiki)


            If Me.UseCache Then
                Dim cacheKey As String = CachePolicy.Wiki(Me.ComunitaCorrente.Id)
                If ObjectBase.Cache(cacheKey) Is Nothing Then
                    ObjectBase.Cache.Insert(cacheKey, oWiki, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, New TimeSpan(0, 30, 0))
                Else
                    ObjectBase.Cache.Item(cacheKey) = oWiki
                End If
            End If
        End Sub
        Public Sub SalvaWikidb(ByVal oWiki As WikiNew.Wiki)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            If IsNothing(oWiki.ID) Then
                oWiki.ID = New System.Guid()
            End If




            With oRequest
                .Command = "NEW_sp_Wiki_Salva"
                .CommandType = CommandType.StoredProcedure

                '@WIKI_id
                oParam = objAccesso.GetAdvancedParameter("@WIKI_id", oWiki.ID.ToString, ParameterDirection.Input, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)

                '@WIKI_nome
                oParam = objAccesso.GetAdvancedParameter("@WIKI_nome", oWiki.Nome, ParameterDirection.Input, SqlDbType.VarChar, , 100)
                .Parameters.Add(oParam)

                '@WIKI_CMNT_id
                oParam = objAccesso.GetAdvancedParameter("@WIKI_CMNT_id", oWiki.Comunita.Id, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                '@isNew bit
                oParam = objAccesso.GetAdvancedParameter("@isNew", oWiki.IsNew, ParameterDirection.Input, SqlDbType.Bit)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)


            Catch
                'n_Errore = Errori_Db.DBChange
            End Try
        End Sub

        ''' <summary>
        ''' Salva la sezione, aggiornando dB e CACHE se necessario.
        ''' </summary>
        ''' <param name="oSezione"></param>
        ''' <remarks></remarks>
        Public Sub SalvaSezione(ByVal oSezione As WikiNew.SezioneWiki) Implements iManagerWiki.SalvaSezione
            Me.SalvaSezionedB(oSezione)
            If Me.UseCache Then
                Dim cacheKey As String = CachePolicy.WikiSezioni(oSezione.Wiki.ID.ToString)
                Dim oSezioni As IList

                Try
                    oSezioni = ObjectBase.Cache(cacheKey)
                Catch ex As Exception
                    oSezioni = Nothing
                End Try

                If Not IsNothing(oSezioni) Then
                    For Each item As WikiNew.SezioneWiki In oSezioni
                        If item.ID = oSezione.ID Then
                            item = oSezione
                            Exit For
                        End If
                    Next

                    ObjectBase.Cache(cacheKey) = oSezioni
                End If

                Dim cacheKeyHome As String = CachePolicy.WikiSezioniHome()
                If Not ObjectBase.Cache(cacheKeyHome) Is Nothing Then
                    ObjectBase.Cache(cacheKeyHome) = Nothing
                End If
            End If
            '        GenericSession.SaveOrUpdate(oSezione)
            '        GenericSession.Flush()
            '        CaricaSezioni(oSezione.Wiki, True)
        End Sub
        Private Sub SalvaSezionedB(ByVal oSezione As WikiNew.SezioneWiki)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            'NEW_sp_Wiki_SalvaSezione()
            With oRequest
                .Command = "NEW_sp_Wiki_SalvaSezione"
                .CommandType = CommandType.StoredProcedure

                If IsNothing(oSezione.ID) Then
                    oSezione.ID = New System.Guid()
                End If

                '@WKSZ_id uniqueidentifier,
                oParam = objAccesso.GetAdvancedParameter("@WKSZ_id", oSezione.ID.ToString, ParameterDirection.Input, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)

                '@WKSZ_WIKI_id uniqueidentifier,
                oParam = objAccesso.GetAdvancedParameter("@WKSZ_WIKI_id", oSezione.Wiki.ID.ToString, ParameterDirection.Input, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)

                '@WKSZ_nome varchar(50),
                oParam = objAccesso.GetAdvancedParameter("@WKSZ_nome", oSezione.NomeSezione, ParameterDirection.Input, SqlDbType.VarChar, , 50)
                .Parameters.Add(oParam)

                '@WKSZ_dataInserimento varchar(30),
                oParam = objAccesso.GetAdvancedParameter("@WKSZ_dataInserimento", Main.DateToString(oSezione.DataInserimento), ParameterDirection.Input, SqlDbType.VarChar, , 30)
                .Parameters.Add(oParam)

                '@WKSZ_PRSN_id int,
                oParam = objAccesso.GetAdvancedParameter("@WKSZ_PRSN_id", oSezione.Persona.ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                '@WKSZ_isDeleted bit,
                oParam = objAccesso.GetAdvancedParameter("@WKSZ_isDeleted", oSezione.IsDeleted, ParameterDirection.Input, SqlDbType.Bit)
                .Parameters.Add(oParam)

                '@WKSZ_isDefault bit,
                oParam = objAccesso.GetAdvancedParameter("@WKSZ_isDefault", oSezione.IsDefault, ParameterDirection.Input, SqlDbType.Bit)
                .Parameters.Add(oParam)

                '@WKSZ_Descrizione nvarchar(100),
                oParam = objAccesso.GetAdvancedParameter("@WKSZ_Descrizione", oSezione.Descrizione, ParameterDirection.Input, SqlDbType.NVarChar, , -1)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PlainDescription", oSezione.PlainDescription, ParameterDirection.Input, SqlDbType.NVarChar, , -1)
                .Parameters.Add(oParam)

                '@WKSZ_isPubblica bit
                oParam = objAccesso.GetAdvancedParameter("@WKSZ_isPubblica", oSezione.IsPubblica, ParameterDirection.Input, SqlDbType.Bit)
                .Parameters.Add(oParam)

                '@isNew bit
                oParam = objAccesso.GetAdvancedParameter("@isNew", oSezione.IsNew, ParameterDirection.Input, SqlDbType.Bit)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch
                'n_Errore = Errori_Db.DBChange
            End Try

        End Sub

        ''' <summary>
        ''' Salva il Topic, aggiornando dB ed eventualmente CACHE se necessario.
        ''' </summary>
        ''' <param name="oTopic"></param>
        ''' <remarks></remarks>
        Public Sub SalvaTopic(ByRef oTopic As WikiNew.TopicWiki, ByRef allowsavequit As Boolean) Implements iManagerWiki.SalvaTopic
            Me.SalvaTopicdB(oTopic, allowsavequit)

            If Me.UseCache Then
                Dim cacheKey As String = CachePolicy.WikiTopics(oTopic.Sezione.ID.ToString)
                Dim oTopics As IList

                Try
                    oTopics = ObjectBase.Cache(cacheKey)
                Catch ex As Exception
                    oTopics = Nothing
                End Try

                Dim Index As Integer = -1
                If Not IsNothing(oTopics) Then
                    For Each item As COL_Wiki.WikiNew.TopicWiki In oTopics
                        If item.ID = oTopic.ID Then
                            Index = oTopics.IndexOf(item)
                            Exit For
                        End If
                    Next
                    If Not Index = -1 Then
                        oTopics.RemoveAt(Index)
                    End If
                    oTopics.Add(oTopic)
                    ObjectBase.Cache(cacheKey) = oTopics
                End If

            End If
        End Sub
        Private Sub SalvaTopicdB(ByRef oTopic As COL_Wiki.WikiNew.TopicWiki, ByRef allowsavequit As Boolean)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "NEW_sp_Wiki_TopicAdd"
                .CommandType = CommandType.StoredProcedure

                Dim TopicIdstr As String = oTopic.ID.ToString

                If IsNothing(oTopic.ID) Then
                    oTopic.ID = New System.Guid()
                End If


                '@WKTP_id as uniqueidentifier,
                oParam = objAccesso.GetAdvancedParameter("@WKTP_id", oTopic.ID.ToString, ParameterDirection.Input, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)

                '@WKTP_nome as VarChar(100),
                oParam = objAccesso.GetAdvancedParameter("@WKTP_nome", oTopic.Nome, ParameterDirection.Input, SqlDbType.VarChar, , 100)
                .Parameters.Add(oParam)

                '@WKTP_contenuto as Text,
                oParam = objAccesso.GetAdvancedParameter("@WKTP_contenuto", oTopic.Contenuto, ParameterDirection.Input, SqlDbType.NText)
                .Parameters.Add(oParam)

                '@WKTP_dataInserimento as varchar(30),
                oParam = objAccesso.GetAdvancedParameter("@WKTP_dataInserimento", Main.DateToString(oTopic.DataInserimento), ParameterDirection.Input, SqlDbType.VarChar, , 30)
                .Parameters.Add(oParam)

                '@WKTP_dataModifica as varchar(30),
                oParam = objAccesso.GetAdvancedParameter("@WKTP_dataModifica", Main.DateToString(oTopic.DataModifica), ParameterDirection.Input, SqlDbType.VarChar, , 30)
                .Parameters.Add(oParam)

                '@WKTP_PRSN_id as int,
                oParam = objAccesso.GetAdvancedParameter("@WKTP_PRSN_id", oTopic.Persona.ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                '@WKTP_WKSZ_id as uniqueidentifier,
                oParam = objAccesso.GetAdvancedParameter("@WKTP_WKSZ_id", oTopic.Sezione.ID.ToString, ParameterDirection.Input, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)

                '@WKTP_isDeleted as bit
                oParam = objAccesso.GetAdvancedParameter("@WKTP_isDeleted", oTopic.isCancellato, ParameterDirection.Input, SqlDbType.Bit)
                'If oTopic.isCancellato Then
                '    oParam = objAccesso.GetAdvancedParameter("@WKTP_isDeleted", 1, ParameterDirection.Input, SqlDbType.Bit)
                'Else
                '    oParam = objAccesso.GetAdvancedParameter("@WKTP_isDeleted", 0, ParameterDirection.Input, SqlDbType.Bit)
                'End If
                .Parameters.Add(oParam)


                '@WKTP_isPubblica as bit
                oParam = objAccesso.GetAdvancedParameter("@WKTP_isPubblica", oTopic.IsPubblica, ParameterDirection.Input, SqlDbType.Bit)
                'If oTopic.isCancellato Then
                '    oParam = objAccesso.GetAdvancedParameter("@WKTP_isDeleted", 1, ParameterDirection.Input, SqlDbType.Bit)
                'Else
                '    oParam = objAccesso.GetAdvancedParameter("@WKTP_isDeleted", 0, ParameterDirection.Input, SqlDbType.Bit)
                'End If
                .Parameters.Add(oParam)

                '@isNew bit
                oParam = objAccesso.GetAdvancedParameter("@isNew", oTopic.IsNew, ParameterDirection.Input, SqlDbType.Bit)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch
                'n_Errore = Errori_Db.DBChange
            End Try
        End Sub

        ''' <summary>
        ''' Salva un TopicHistory
        ''' </summary>
        ''' <param name="oTopicHistory">
        ''' Oggetto sa salvare
        ''' </param>
        ''' <remarks></remarks>
        Public Sub SalvaTopicHistory(ByVal oTopicHistory As WikiNew.TopicHistoryWiki) Implements iManagerWiki.SalvaTopicHistory
            Me.SalvaTopicHistorydB(oTopicHistory)

            If Me.UseCache Then
                Dim tmpTopicsHistory As IList
                Dim cacheKey As String = CachePolicy.WikiTopicsHistory(oTopicHistory.Topic.ID.ToString)

                Try
                    tmpTopicsHistory = ObjectBase.Cache(cacheKey)
                Catch ex As Exception
                    tmpTopicsHistory = Nothing
                End Try

                If Not IsNothing(tmpTopicsHistory) Then
                    tmpTopicsHistory.Add(oTopicHistory)
                    ObjectBase.Cache(cacheKey) = tmpTopicsHistory
                End If
            End If
        End Sub
        Private Sub SalvaTopicHistorydB(ByVal oTopicHistory As WikiNew.TopicHistoryWiki)
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess
            '@WKTP_dataModifica
            With oRequest
                .Command = "NEW_sp_Wiki_TopicHistoryAdd"
                .CommandType = CommandType.StoredProcedure

                If IsNothing(oTopicHistory.ID) Then
                    'oTopic.ID = New System.Guid()
                End If

                '@WKTH_id as uniqueidentifier,
                oParam = objAccesso.GetAdvancedParameter("@WKTH_id", oTopicHistory.ID.ToString, ParameterDirection.Input, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)

                '@WKTH_nome as VarChar(100),
                oParam = objAccesso.GetAdvancedParameter("@WKTH_nome", oTopicHistory.Nome, ParameterDirection.Input, SqlDbType.VarChar, , 100)
                .Parameters.Add(oParam)

                '@WKTH_contenuto as nVarChar(Max),
                oParam = objAccesso.GetAdvancedParameter("@WKTH_contenuto", oTopicHistory.Contenuto, ParameterDirection.Input, SqlDbType.Text)
                .Parameters.Add(oParam)

                '@WKTH_dataModifica as varchar(30),
                oParam = objAccesso.GetAdvancedParameter("@WKTH_dataModifica", Main.DateToString(oTopicHistory.DataModifica), ParameterDirection.Input, SqlDbType.VarChar, , 30)
                .Parameters.Add(oParam)

                '@WKTH_PRSN_id as int,
                oParam = objAccesso.GetAdvancedParameter("@WKTH_PRSN_id", oTopicHistory.Persona.ID, ParameterDirection.Input, SqlDbType.Int)
                .Parameters.Add(oParam)

                Dim TopiIdStr As String = oTopicHistory.Topic.ID.ToString
                '@WKTH_WKTP_id as uniqueidentifier,
                oParam = objAccesso.GetAdvancedParameter("@WKTH_WKTP_id", oTopicHistory.Topic.ID.ToString, ParameterDirection.Input, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)

                '@WKTH_isDeleted as bit
                oParam = objAccesso.GetAdvancedParameter("@WKTH_isDeleted", oTopicHistory.isCancellato, ParameterDirection.Input, SqlDbType.Bit)
                .Parameters.Add(oParam)

                '@isNew bit
                oParam = objAccesso.GetAdvancedParameter("@isNew", oTopicHistory.IsNew, ParameterDirection.Input, SqlDbType.Bit)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                objAccesso.GetExecuteNotQuery(oRequest)
            Catch ex As Exception
                'n_Errore = Errori_Db.DBChange
            End Try
        End Sub

        ''' <summary>
        ''' Inutilizzato
        ''' </summary>
        ''' <param name="oTopic"></param>
        ''' <remarks>
        ''' Doveva servire per le immagini per avere un ID Topic 
        ''' al momento del caricamento delle stesse.
        ''' Tolto, grazie all'utilizzo dei GUID. 
        ''' Creo un guid, lo assegno come Topic.ID dell'immagine, 
        ''' poi salvo il Topic con quel guid.
        ''' </remarks>
        Public Sub PreSaveTopic(ByVal oTopic As WikiNew.TopicWiki) Implements iManagerWiki.PreSaveTopic
            Dim allowsavequit As Boolean
            Me.SalvaTopic(oTopic, allowsavequit)

        End Sub
#End Region

#Region "Cancellazioni,ricerca e altro"
        'PARTE AGGIJNTA DA ALESSANDRO -- Carica i topic in una lista partendo dalla stringa di ricerca passata
        Public Function CercaTopic(ByVal text As String, ByVal RowIndex As Integer, ByVal pageSize As Integer) As System.Collections.ArrayList Implements iManagerWiki.CercaTopic
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataReader As IDataReader
            Dim objAccesso As New COL_DataAccess

            Dim ATopics As New ArrayList()

            With oRequest
                .Command = "sp_Wiki_CercaTopic"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@Wiki_TEXT", text, ParameterDirection.Input, SqlDbType.VarChar, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@RowIndex", RowIndex, ParameterDirection.Input, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@pageSize", pageSize, ParameterDirection.Input, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                oDataReader = objAccesso.GetdataReader(oRequest)
                Dim DataNull As New DateTime(2000, 1, 1)

                While oDataReader.Read
                    Dim oTopic As New WikiNew.TopicWiki
                    With oTopic
                        .ID = New System.Guid(oDataReader("WKTP_id").ToString)
                        .Nome = oDataReader("WKTP_nome")
                        .DataInserimento = GenericValidator.ValData(oDataReader("WKTP_dataInserimento"), DataNull)
                        .DataModifica = GenericValidator.ValData(oDataReader("WKTP_dataModifica"), DataNull)
                        .Contenuto = GenericValidator.ValString(oDataReader("WKTP_Contenuto"), "")
                        .Comunita = GenericValidator.ValString(oDataReader("CMNT_nome"), "")
                        .Sezione.NomeSezione = GenericValidator.ValString(oDataReader("WKSZ_nome"), "")

                    End With
                    ATopics.Add(oTopic)
                End While
            Catch ex As Exception

            End Try

            Return ATopics
        End Function
        Public Function CercaTopicComunita(ByVal text As String, ByVal RowIndex As Integer, ByVal pageSize As Integer, Optional ByVal IDcomunita As Integer = Nothing) As System.Collections.ArrayList Implements iManagerWiki.CercaTopicComunita
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataReader As IDataReader
            Dim objAccesso As New COL_DataAccess

            Dim ATopics As New ArrayList()

            With oRequest
                .Command = "sp_Wiki_CercaTopics_Comunita"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@Wiki_TEXT", text, ParameterDirection.Input, SqlDbType.VarChar, , 100)
                .Parameters.Add(oParam)
                If (IDcomunita = Nothing) Then
                    oParam = objAccesso.GetAdvancedParameter("@comunita", Me.ComunitaCorrente.Id.ToString, ParameterDirection.Input, SqlDbType.VarChar, , 40)
                Else
                    oParam = objAccesso.GetAdvancedParameter("@comunita", IDcomunita, ParameterDirection.Input, SqlDbType.VarChar, , 40)

                End If

                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@RowIndex", RowIndex, ParameterDirection.Input, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@pageSize", pageSize, ParameterDirection.Input, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                oDataReader = objAccesso.GetdataReader(oRequest)
                Dim DataNull As New DateTime(2000, 1, 1)

                While oDataReader.Read
                    Dim oTopic As New WikiNew.TopicWiki
                    With oTopic
                        .ID = New System.Guid(oDataReader("WKTP_id").ToString)
                        .Nome = oDataReader("WKTP_nome")
                        .DataInserimento = GenericValidator.ValData(oDataReader("WKTP_dataInserimento"), DataNull)
                        .DataModifica = GenericValidator.ValData(oDataReader("WKTP_dataModifica"), DataNull)
                        .Contenuto = GenericValidator.ValString(oDataReader("WKTP_Contenuto"), "")
                        .Comunita = GenericValidator.ValString(oDataReader("CMNT_nome"), "")
                        .Sezione.NomeSezione = GenericValidator.ValString(oDataReader("WKSZ_nome"), "")

                    End With
                    ATopics.Add(oTopic)
                End While
            Catch ex As Exception

            End Try

            Return ATopics
        End Function
        Public Function CercaTopicComPub(ByVal text As String, ByVal RowIndex As Integer, ByVal pageSize As Integer) As System.Collections.ArrayList Implements iManagerWiki.CercaTopicComPub
            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataReader As IDataReader
            Dim objAccesso As New COL_DataAccess

            Dim ATopics As New ArrayList()

            With oRequest
                .Command = "sp_Wiki_CercaTopics_Comunita_Pubblici"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@Wiki_TEXT", text, ParameterDirection.Input, SqlDbType.VarChar, , 100)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@comunita", Me.ComunitaCorrente.Id.ToString, ParameterDirection.Input, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@RowIndex", RowIndex, ParameterDirection.Input, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)
                oParam = objAccesso.GetAdvancedParameter("@pageSize", pageSize, ParameterDirection.Input, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)


                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                oDataReader = objAccesso.GetdataReader(oRequest)
                Dim DataNull As New DateTime(2000, 1, 1)

                While oDataReader.Read
                    Dim oTopic As New WikiNew.TopicWiki
                    With oTopic
                        .ID = New System.Guid(oDataReader("WKTP_id").ToString)
                        .Nome = oDataReader("WKTP_nome")
                        .DataInserimento = GenericValidator.ValData(oDataReader("WKTP_dataInserimento"), DataNull)
                        .DataModifica = GenericValidator.ValData(oDataReader("WKTP_dataModifica"), DataNull)
                        .Contenuto = GenericValidator.ValString(oDataReader("WKTP_Contenuto"), "")
                        .Comunita = GenericValidator.ValString(oDataReader("CMNT_nome"), "")
                        .Sezione.NomeSezione = GenericValidator.ValString(oDataReader("WKSZ_nome"), "")

                    End With
                    ATopics.Add(oTopic)
                End While
            Catch ex As Exception

            End Try

            Return ATopics
        End Function

        Public Sub DeleteTopic(ByVal oTopic As WikiNew.TopicWiki) Implements iManagerWiki.DeleteTopic
            oTopic.isCancellato = True
            Dim allowsavequit As Boolean
            Me.SalvaTopic(oTopic, allowsavequit)

        End Sub
        Public Sub UnDeleteTopic(ByVal oTopic As TopicWiki) Implements iManagerWiki.UnDeleteTopic
            oTopic.isCancellato = False
            Dim allowsavequit As Boolean
            Me.SalvaTopic(oTopic, allowsavequit)
        End Sub


        Public Function UltimaDataModifica(ByVal oSezione As WikiNew.SezioneWiki) As DateTime Implements iManagerWiki.UltimaDataModifica
            Return Me.dBUltimaDataModifica(oSezione)
        End Function
        Private Function dBUltimaDataModifica(ByVal oSezione As WikiNew.SezioneWiki) As DateTime

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "NEW_sp_Wiki_UltimaModifica"
                .CommandType = CommandType.StoredProcedure
                oParam = objAccesso.GetAdvancedParameter("@WKSZ_Id", oSezione.ID.ToString, ParameterDirection.Input, SqlDbType.VarChar, , 40)

                .Parameters.Add(oParam)
                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                objAccesso.GetExecuteNotQuery(oRequest)

                Try
                    Return CDate(oRequest.GetValueFromParameter(2))
                Catch ex As Exception
                    Return Nothing
                End Try
            Catch ex As Exception
                Return Nothing
            End Try
            Return Nothing
        End Function
#End Region

#Region "Metodi HOME"
        'I metodi "HOME" sono utilizzati nella pagina iniziale per recuperare i dati pubblici relativi
        'a Sezioni e Topic
        'Anche gli algoritmi di cache subiranno qualche variazione...
        Public Function CaricaSezioniHome(Optional ByVal forced As Boolean = False) As System.Collections.IList Implements iManagerWiki.CaricaSezioniHome
            Dim tmpSezioni As ArrayList

            If Me.UseCache Then
                Dim cacheKey As String = CachePolicy.WikiSezioniHome()
                If ObjectBase.Cache(cacheKey) Is Nothing Or forced Then
                    tmpSezioni = CaricaSezioniHomedB()

                    ObjectBase.Cache.Insert(cacheKey, tmpSezioni, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, New TimeSpan(0, 30, 0))
                Else
                    tmpSezioni = CType(ObjectBase.Cache(cacheKey), ArrayList)
                End If
            Else
                tmpSezioni = CaricaSezioniHomedB()
            End If

            Return tmpSezioni
        End Function

        ''' <summary>
        ''' Carica tutte le sezioni pubbliche dal dB.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>Inoltre viene inserito l'oggetto WIKI e WIKI.COMUNITA con relativi ID e NOME</remarks>
        Private Function CaricaSezioniHomedB() As ArrayList

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim oDataReader As IDataReader
            Dim objAccesso As New COL_DataAccess

            Dim ARsezioni As New ArrayList()

            With oRequest
                .Command = "NEW_sp_Wiki_CaricaSezioniHome"
                .CommandType = CommandType.StoredProcedure

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With

            Try
                oDataReader = objAccesso.GetdataReader(oRequest)
                Dim DataNull As New DateTime(2000, 1, 1)

                While oDataReader.Read
                    Dim oSezione As New WikiNew.SezioneWiki
                    With oSezione
                        .ID = New System.Guid(oDataReader("WKSZ_id").ToString)
                        .NomeSezione = oDataReader("WKSZ_nome")
                        .DataInserimento = GenericValidator.ValData(oDataReader("WKSZ_dataInserimento"), DataNull)
                        .Persona = New COL_Persona(oDataReader("WKSZ_PRSN_id"))

                        .IsDeleted = GenericValidator.ValBool(oDataReader("WKSZ_isDeleted"), False)
                        .IsDefault = GenericValidator.ValBool(oDataReader("WKSZ_isDefault"), False)
                        .Descrizione = GenericValidator.ValString(oDataReader("WKSZ_Descrizione"), "")
                        .IsPubblica = GenericValidator.ValBool(oDataReader("WKSZ_isPubblica"), False)
                        .Persona.Nome = GenericValidator.ValString(oDataReader("PRSN_nome"), "")
                        .Persona.Cognome = GenericValidator.ValString(oDataReader("PRSN_cognome"), "")

                        .Wiki = New COL_Wiki.WikiNew.Wiki
                        .Wiki.ID = New System.Guid(oDataReader("WIKI_id").ToString)
                        .Wiki.Nome = GenericValidator.ValString(oDataReader("WIKI_nome"), "")
                        .Wiki.Comunita = New COL_Comunita(GenericValidator.ValInteger(oDataReader("CMNT_id"), 0))
                        .Wiki.Comunita.Nome = GenericValidator.ValString(oDataReader("CMNT_nome"), "")

                    End With
                    ARsezioni.Add(oSezione)
                End While
            Catch ex As Exception

            End Try

            'Dim oSezione As New WikiNew.SezioneWiki
            '.Add(oSezione)
            Return ARsezioni
        End Function

        Public Sub CaricaSezioneHome(ByRef oSezione As SezioneWiki) Implements iManagerWiki.CaricaSezioneHome
            If Me.UseCache Then
                'Me.CaricaSezionedB(oSezione)

                Dim cacheKey As String = CachePolicy.WikiSezioniHome()

                Dim oSezioni As IList
                Try
                    oSezioni = ObjectBase.Cache(cacheKey)
                Catch ex As Exception
                    oSezioni = Nothing
                End Try

                If Not IsNothing(oSezioni) Then
                    For Each item As WikiNew.SezioneWiki In oSezioni
                        If item.ID = oSezione.ID Then
                            oSezione = item
                            Exit Sub
                        End If
                    Next
                End If
                'Se lo trova, lo carica ed esce dalla sub.
                'Se non lo trova, non c'e' in cache e/o non sto usando la cache, lo carico da dB
            End If
            Me.CaricaSezioneHomedB(oSezione)

        End Sub
        Public Sub CaricaSezioneHomedB(ByRef oSezione As SezioneWiki)

            Dim oRequest As New COL_Request
            Dim oParam As New COL_Request.Parameter
            Dim responso As Integer
            Dim objAccesso As New COL_DataAccess

            With oRequest
                .Command = "NEW_sp_Wiki_SezioneHome_Estrai"
                .CommandType = CommandType.StoredProcedure

                '1 - @WKSZ_id as varchar(40)
                oParam = objAccesso.GetAdvancedParameter("@WKSZ_id", oSezione.ID.ToString, ParameterDirection.Input, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)

                '2 - @WKSZ_WIKI_id as varchar(40) output
                oParam = objAccesso.GetAdvancedParameter("@WKSZ_WIKI_id", "", ParameterDirection.Output, SqlDbType.VarChar, , 40)
                .Parameters.Add(oParam)

                '3 - @WKSZ_nome as varchar(50) output
                oParam = objAccesso.GetAdvancedParameter("@WKSZ_nome", "", ParameterDirection.Output, SqlDbType.NVarChar, , 100)
                .Parameters.Add(oParam)

                '4 - @WKSZ_dataInserimento as datetime output
                oParam = objAccesso.GetAdvancedParameter("@WKSZ_dataInserimento", "", ParameterDirection.Output, SqlDbType.DateTime)
                .Parameters.Add(oParam)

                '5 - @WKSZ_PRSN_id as int output
                oParam = objAccesso.GetAdvancedParameter("@WKSZ_PRSN_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                '6 - @WKSZ_isDeleted as bit output
                oParam = objAccesso.GetAdvancedParameter("@WKSZ_isDeleted", "", ParameterDirection.Output, SqlDbType.Bit)
                .Parameters.Add(oParam)

                '7 - @WKSZ_isDefault as bit output
                oParam = objAccesso.GetAdvancedParameter("@WKSZ_isDefault", "", ParameterDirection.Output, SqlDbType.Bit)
                .Parameters.Add(oParam)

                '8 - @WKSZ_Descrizione as nvarchar(100) output
                oParam = objAccesso.GetAdvancedParameter("@WKSZ_Descrizione", "", ParameterDirection.Output, SqlDbType.NVarChar, , -1)
                .Parameters.Add(oParam)

                oParam = objAccesso.GetAdvancedParameter("@PlainDescription", "", ParameterDirection.Output, SqlDbType.NVarChar, , -1)
                .Parameters.Add(oParam)

                '9 - @WKSZ_isPubblica as bit output
                oParam = objAccesso.GetAdvancedParameter("@WKSZ_isPubblica", "", ParameterDirection.Output, SqlDbType.Bit)
                .Parameters.Add(oParam)

                '10 - @PRSN_nome
                oParam = objAccesso.GetAdvancedParameter("@PRSN_nome", "", ParameterDirection.Output, SqlDbType.NVarChar, , 40)
                .Parameters.Add(oParam)

                '11 - @PRSN_cognome
                oParam = objAccesso.GetAdvancedParameter("@PRSN_cognome", "", ParameterDirection.Output, SqlDbType.NVarChar, , 40)
                .Parameters.Add(oParam)




                '12 - @WIKI_id varchar(40)
                oParam = objAccesso.GetAdvancedParameter("@WIKI_id", "", ParameterDirection.Output, SqlDbType.NVarChar, , 40)
                .Parameters.Add(oParam)

                '13 - @WIKI_nome varchar(50)
                oParam = objAccesso.GetAdvancedParameter("@WIKI_nome", "", ParameterDirection.Output, SqlDbType.NVarChar, , 50)
                .Parameters.Add(oParam)

                '14 - @CMNT_id int
                oParam = objAccesso.GetAdvancedParameter("@CMNT_id", "", ParameterDirection.Output, SqlDbType.Int)
                .Parameters.Add(oParam)

                '15 - @CMNT_nome
                oParam = objAccesso.GetAdvancedParameter("@CMNT_nome", "", ParameterDirection.Output, SqlDbType.NVarChar, , 200)
                .Parameters.Add(oParam)

                .Role = COL_Request.UserRole.Admin
                .transactional = False
            End With
            Try
                responso = objAccesso.GetExecuteNotQuery(oRequest)
                Dim DefaultDate As New DateTime(2000, 1, 1)
                With oSezione
                    .Wiki = New WikiNew.Wiki
                    .Wiki.ID = New Guid(oRequest.GetValueFromParameter(2).ToString)
                    .NomeSezione = oRequest.GetValueFromParameter(3)
                    .DataInserimento = GenericValidator.ValData(oRequest.GetValueFromParameter(4), DefaultDate)
                    .Persona = New COL_Persona(oRequest.GetValueFromParameter(5))
                    .IsDeleted = GenericValidator.ValBool(oRequest.GetValueFromParameter(6), False)
                    .IsDefault = GenericValidator.ValBool(oRequest.GetValueFromParameter(7), False)
                    .Descrizione = oRequest.GetValueFromParameter(8)
                    .IsPubblica = GenericValidator.ValBool(oRequest.GetValueFromParameter(9), True)
                    .Persona.Nome = oRequest.GetValueFromParameter(10)
                    .Persona.Cognome = oRequest.GetValueFromParameter(11)

                    .Wiki = New COL_Wiki.WikiNew.Wiki
                    .Wiki.ID = New Guid(oRequest.GetValueFromParameter(12))
                    .Wiki.Nome = oRequest.GetValueFromParameter(13)

                    .Wiki.Comunita = New COL_Comunita
                    .Wiki.Comunita.Id = oRequest.GetValueFromParameter(14)
                    .Wiki.Comunita.Nome = oRequest.GetValueFromParameter(15)
                End With
            Catch ex As Exception

            End Try
        End Sub

#End Region

        ''' <summary>
        ''' Utilizza i metodi dei topic generici, ma filtra quelli che sono cancellati e non sono pubblici
        ''' </summary>
        ''' <param name="oSezione"></param>
        ''' <param name="forced"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CaricaTopicsHome(ByVal oSezione As SezioneWiki, Optional ByVal forced As Boolean = False) As System.Collections.IList Implements iManagerWiki.CaricaTopicsHome
            Dim ALOut, ALIn As IList
            Dim oTopic As COL_Wiki.WikiNew.TopicWiki

            ALIn = Me.CaricaTopics(oSezione, forced)
            ALOut = New ArrayList

            For Each item As COL_Wiki.WikiNew.TopicWiki In ALIn
                If (item.isCancellato = False And item.IsPubblica = True) Then
                    oTopic = New COL_Wiki.WikiNew.TopicWiki
                    With oTopic
                        .ID = item.ID
                        .Contenuto = item.Contenuto
                        .DataInserimento = item.DataInserimento
                        .DataModifica = item.DataModifica
                        .isCancellato = item.isCancellato
                        .IsPubblica = item.IsPubblica
                        .IsNew = item.IsNew
                        .Nome = item.Nome
                        .Persona = item.Persona
                        .Sezione = item.Sezione
                    End With
                    ALOut.Add(oTopic)
                End If
            Next
            Return ALOut
        End Function


    End Class
End Namespace