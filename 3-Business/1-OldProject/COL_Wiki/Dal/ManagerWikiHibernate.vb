Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita

Namespace WikiNew
    Public Class ManagerWikiHibernate
        Inherits ObjectBase
        Implements iManagerWiki


        Private _Persona As COL_Persona
        Private _Comunita As COL_Comunita
        Private _UseCache As Boolean
#Region "Proprietà"
        Public Property PersonaCorrente() As COL_BusinessLogic_v2.CL_persona.COL_Persona Implements iManagerWiki.PersonaCorrente
            Get
                Return _Persona
            End Get
            Set(ByVal value As COL_BusinessLogic_v2.CL_persona.COL_Persona)
                _Persona = value
            End Set
        End Property

        Public Property ComunitaCorrente() As COL_BusinessLogic_v2.Comunita.COL_Comunita Implements iManagerWiki.ComunitaCorrente
            Get
                Return _Comunita
            End Get
            Set(ByVal value As COL_BusinessLogic_v2.Comunita.COL_Comunita)
                _Comunita = value
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
        Public Sub New(ByVal oPersona As COL_Persona, ByVal oComunita As COL_Comunita, Optional ByVal UseCache As Boolean = False)
            Me.PersonaCorrente = oPersona
            Me.ComunitaCorrente = oComunita
            Me.UseCache = UseCache

            'oManagerPermessi = New ManagerPermessi()
            'Dim oPermesso As New Permesso
            'oPermesso = oManagerPermessi.GetPermessi(ComunitaCorrente, PersonaCorrente, Serviziowiki.codex)
            'oServizioWiki = New ServizioWiki(oPermesso)
        End Sub
#Region "Caricamenti"
        Public Function CaricaWiki(Optional ByVal forced As Boolean = False) As WikiNew.Wiki Implements iManagerWiki.CaricaWiki
            Dim tmpWiki As WikiNew.Wiki

            ' recupero l'id della cache
            Dim cacheKey As String = CachePolicy.Wiki(Me._Comunita.Id)

            ' se non trovo in cache o forced=true forzo il caricamento da db
            If ObjectBase.Cache(cacheKey) Is Nothing Or forced = True Then
                ' VEDERE COME SI FA IN HYBERNET
                'inserisco il wiki recuperato nella cache
                ObjectBase.Cache.Insert(cacheKey, tmpWiki, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, New TimeSpan(0, 30, 0))
            Else
                'recupero il wiki dalla cache
                tmpWiki = CType(ObjectBase.Cache.Item(cacheKey), WikiNew.Wiki)
            End If
            Return tmpWiki
        End Function

        Public Function CaricaSezioni(ByVal oWiki As WikiNew.Wiki, Optional ByVal forced As Boolean = False) As System.Collections.IList Implements iManagerWiki.CaricaSezioni

        End Function

        Public Function CaricaAllTopicWiki(ByVal oWiki As WikiNew.Wiki, Optional ByVal forced As Boolean = False) As System.Collections.IList Implements iManagerWiki.CaricaAllTopicWiki

        End Function


        Public Function CaricaStoriaTopic(ByVal oTopic As WikiNew.TopicWiki, Optional ByVal forced As Boolean = False) As System.Collections.IList Implements iManagerWiki.CaricaStoriaTopic

        End Function

        Public Function CaricaTopic(ByVal oSezione As WikiNew.SezioneWiki, Optional ByVal forced As Boolean = False) As System.Collections.IList Implements iManagerWiki.CaricaTopics

        End Function



#End Region
#Region "Salvataggi"

        Public Sub SalvaTopic(ByRef oTopic As WikiNew.TopicWiki, ByRef allowsavequit As Boolean) Implements iManagerWiki.SalvaTopic

        End Sub

        Public Sub SalvaTopicHistory(ByVal oTopicHistory As WikiNew.TopicHistoryWiki) Implements iManagerWiki.SalvaTopicHistory

        End Sub

        Public Sub PreSaveTopic(ByVal oTopic As WikiNew.TopicWiki) Implements iManagerWiki.PreSaveTopic

        End Sub

        Public Sub SalvaSezione(ByVal oSezione As WikiNew.SezioneWiki) Implements iManagerWiki.SalvaSezione

        End Sub

#End Region
        Public Sub DeleteTopic(ByVal oTopic As WikiNew.TopicWiki) Implements iManagerWiki.DeleteTopic

        End Sub
        Public Function UltimaDataModifica(ByVal oSezione As WikiNew.SezioneWiki) As Date Implements iManagerWiki.UltimaDataModifica

        End Function
        Public Sub CaricaTopic1(ByRef oTopic As TopicWiki) Implements iManagerWiki.CaricaTopic

        End Sub
        Public Sub CaricaSezione(ByRef oSezione As SezioneWiki) Implements iManagerWiki.CaricaSezione

        End Sub
        Public Sub CaricaTopicCrono(ByRef oTopicCrono As TopicHistoryWiki, Optional ByVal forced As Boolean = False) Implements iManagerWiki.CaricaTopicCrono

        End Sub
        Public Sub SalvaWiki(ByVal oWiki As Wiki) Implements iManagerWiki.SalvaWiki

        End Sub
        Public Sub CaricaSezioneHome(ByRef oSezione As SezioneWiki) Implements iManagerWiki.CaricaSezioneHome

        End Sub
        Public Function CaricaSezioniHome(Optional ByVal forced As Boolean = False) As System.Collections.IList Implements iManagerWiki.CaricaSezioniHome

        End Function
        Public Function CaricaTopicsHome(ByVal oSezione As SezioneWiki, Optional ByVal forced As Boolean = False) As System.Collections.IList Implements iManagerWiki.CaricaTopicsHome

        End Function
        Public Sub UnDeleteTopic(ByVal oTopic As TopicWiki) Implements iManagerWiki.UnDeleteTopic

        End Sub
        Public Function CercaTopic(ByVal text As String, ByVal RowIndex As Integer, ByVal pageSize As Integer) As System.Collections.ArrayList Implements iManagerWiki.CercaTopic

        End Function
        Public Function CercaTopicComPub(ByVal text As String, ByVal RowIndex As Integer, ByVal pageSize As Integer) As System.Collections.ArrayList Implements iManagerWiki.CercaTopicComPub

        End Function


        Public Function CercaTopicComunita(ByVal text As String, ByVal RowIndex As Integer, ByVal pageSize As Integer, Optional ByVal IDcomunita As Integer = Nothing) As System.Collections.ArrayList Implements iManagerWiki.CercaTopicComunita

        End Function


    End Class
End Namespace