Imports lm.Comol.Core.DomainModel

Public Class FactoryWiki
    Private _object As Object
    Private _applicationContext As iApplicationContext

    Public Property [Object]() As Object
        Get
            Return _object
        End Get
        Set(ByVal value As Object)
            _object = value
        End Set
    End Property
    Public Enum ConnectionType
        SQL = 1
        Hybernate = 2
    End Enum

    Public Shared Function CreateManagerWiki(ByVal ConType As ConnectionType, ByVal oPersona As COL_BusinessLogic_v2.CL_persona.COL_Persona, ByVal oComunita As COL_BusinessLogic_v2.Comunita.COL_Comunita, ByVal ocontext As iApplicationContext, Optional ByVal UseCache As Boolean = True) As WikiNew.iManagerWiki

        Dim oManager As WikiNew.iManagerWiki 'WikiNew.AbstractManagerWiki
        Select Case ConType
            Case ConnectionType.SQL
                oManager = New WikiNew.ManagerWikidB(oPersona, oComunita, UseCache, ocontext)
                'oManager = New WikiNew.ManagerWikidB()
                'oManager.PersonaCorrente = oPersona
                'oManager.ComunitaCorrente = oComunita
            Case ConnectionType.Hybernate
                'oManager = New WikiNew.ManagerWikiHibernate(oPersona, oComunita)
        End Select
        Return oManager
    End Function
    Public Shared Function CreateManagerWikiTest(ByVal ConType As ConnectionType, ByVal oPersona As COL_BusinessLogic_v2.CL_persona.COL_Persona, ByVal oComunita As COL_BusinessLogic_v2.Comunita.COL_Comunita, ByVal ocontext As iApplicationContext, Optional ByVal UseCache As Boolean = True) As WikiNew.iManagerWiki

        Dim oManager As WikiNew.iManagerWiki 'WikiNew.AbstractManagerWiki
        Select Case ConType
            Case ConnectionType.SQL
                oManager = New WikiNew.ManagerWikidB(oPersona, oComunita, UseCache, ocontext)
                'oManager = New WikiNew.ManagerWikidB()
                'oManager.PersonaCorrente = oPersona
                'oManager.ComunitaCorrente = oComunita
            Case ConnectionType.Hybernate
                'oManager = New WikiNew.ManagerWikiHibernate(oPersona, oComunita)
        End Select
        Return oManager
    End Function

    Public Shared Function CreateManagerWikiHome(ByVal ConType As ConnectionType, Optional ByVal UseCache As Boolean = True) As WikiNew.iManagerWiki

        Dim oManager As WikiNew.iManagerWiki 'WikiNew.AbstractManagerWiki
        Select Case ConType
            Case ConnectionType.SQL
                oManager = New WikiNew.ManagerWikidB(UseCache)

                'oManager = New WikiNew.ManagerWikidB()
                'oManager.PersonaCorrente = oPersona
                'oManager.ComunitaCorrente = oComunita
            Case ConnectionType.Hybernate
                'oManager = New WikiNew.ManagerWikiHibernate(oPersona, oComunita)
        End Select
        Return oManager
    End Function

    '-----------AGGIUNTA PER RICERCA---------------------
    Public Shared Function CreateManagerWikiHome2(ByVal ConType As ConnectionType, Optional ByVal UseCache As Boolean = True) As WikiNew.iManagerWiki

        Dim oManager As WikiNew.iManagerWiki 'WikiNew.AbstractManagerWiki
        Select Case ConType
            Case ConnectionType.SQL
                oManager = New WikiNew.ManagerWikidB(UseCache)

                'oManager = New WikiNew.ManagerWikidB()
                'oManager.PersonaCorrente = oPersona
                'oManager.ComunitaCorrente = oComunita
            Case ConnectionType.Hybernate
                'oManager = New WikiNew.ManagerWikiHibernate(oPersona, oComunita)
        End Select
        Return oManager
    End Function
    'Sezione modificata da alessandro
    Public Shared Function CreateManagerWikiUC(ByVal ConType As ConnectionType, ByVal oPersona As COL_BusinessLogic_v2.CL_persona.COL_Persona, ByVal oComunita As COL_BusinessLogic_v2.Comunita.COL_Comunita, ByVal ocontext As iApplicationContext, Optional ByVal UseCache As Boolean = True) As WikiNew.iManagerWiki

        Dim oManager As WikiNew.iManagerWiki 'WikiNew.AbstractManagerWiki
        Select Case ConType
            Case ConnectionType.SQL
                oManager = New WikiNew.ManagerWikidB(oPersona, oComunita, UseCache, ocontext)
                'oManager = New WikiNew.ManagerWikidB()
                'oManager.PersonaCorrente = oPersona
                'oManager.ComunitaCorrente = oComunita
            Case ConnectionType.Hybernate
                'oManager = New WikiNew.ManagerWikiHibernate(oPersona, oComunita)
        End Select
        Return oManager
    End Function



End Class
