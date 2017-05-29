'Imports ManagedDesigns.ApplicationBlocks.Validation.Constraints

Namespace WikiNew

    <Serializable()> Public Class TopicHistoryWiki
        'Inherits DomainObject

        Private _ID As Guid
        Private _Nome As String
        Private _Contenuto As String
        Private _DataModifica As Nullable(Of DateTime)
        Private _Persona As COL_BusinessLogic_v2.CL_persona.COL_Persona
        Private _Topic As TopicWiki
        Private _isDeleted As Boolean
        Private _IsNew As Boolean

        'Private _Progressivo As Integer

        Public Sub New()
            _Persona = New COL_BusinessLogic_v2.CL_persona.COL_Persona
            _Topic = New COL_Wiki.WikiNew.TopicWiki
        End Sub

        Public Property ID() As Guid
            Get
                Return _ID
            End Get
            Set(ByVal value As Guid)
                _ID = value
            End Set
        End Property

        Public Property Nome() As String
            Get
                Return _Nome
            End Get
            Set(ByVal value As String)
                _Nome = value
            End Set
        End Property

        Public Property Contenuto() As String
            Get
                Return _Contenuto
            End Get
            Set(ByVal value As String)
                _Contenuto = value
            End Set
        End Property
        'Public ReadOnly Property ContenutoIndicizzato()
        '    Get
        '        Dim NumElem As Integer = 0
        '        Dim TocText As String
        '        TocText = "<div style=""background-color:#6699cc"">Indice<br/>"
        '        'Dim TocAnchorLink As String = "&nbsp;<a href=""#GTT"">Torna su</a>"
        '        Dim Indicizzato As String
        '        Indicizzato = _Contenuto
        '        Dim hlist As IList
        '        hlist = Util.HTMLUtil.ExtractH(_Contenuto)
        '        For Each element As String In hlist
        '            Dim NewElement As String
        '            NewElement = ReplaceHeader(element)
        '            If NewElement <> element Then
        '                NumElem += 1
        '                Indicizzato = Indicizzato.Replace(element, CreaAncorLink(element, NumElem.ToString))
        '                NewElement = CreaAnchor(NewElement, NumElem.ToString)
        '                TocText += NewElement
        '            End If
        '        Next
        '        TocText += "</div>"
        '        Indicizzato = Indicizzato.Replace("_{TOC}_", TocText)
        '        Return Indicizzato
        '    End Get
        'End Property

        Private Function ReplaceHeader(ByVal Header As String) As String
            Dim replaced As String = Header
            replaced = replaced.Replace("<h1>", "-&nbsp;")
            replaced = replaced.Replace("</h1>", "<br/>")
            replaced = replaced.Replace("<h2>", Util.StringUtil.ReplicaStr(3, "&nbsp;"))
            replaced = replaced.Replace("</h2>", "<br/>")
            replaced = replaced.Replace("<h3>", Util.StringUtil.ReplicaStr(6, "&nbsp;"))
            replaced = replaced.Replace("</h3>", "<br/>")
            replaced = replaced.Replace("<h4>", Util.StringUtil.ReplicaStr(9, "&nbsp;"))
            replaced = replaced.Replace("</h4>", "<br/>")
            replaced = replaced.Replace("<h5>", Util.StringUtil.ReplicaStr(12, "&nbsp;"))
            replaced = replaced.Replace("</h5>", "<br/>")
            replaced = replaced.Replace("<h6>", Util.StringUtil.ReplicaStr(15, "&nbsp;"))
            replaced = replaced.Replace("</h6>", "<br/>")
            Return replaced
        End Function

        Private Function CreaAncorLink(ByVal TestoLink As String, ByVal IdAnchor As String, Optional ByVal Ritorno As Boolean = True) As String
            Dim TocAnchorLink As String = "&nbsp;<a href=""#GTT"">Torna su</a>"
            Dim AnchorLink As String = ""
            AnchorLink = "<a id=""" + IdAnchor + """>" + TestoLink + "</a>"
            If Ritorno Then
                AnchorLink += TocAnchorLink
            End If
            Return AnchorLink
        End Function

        Private Function CreaAnchor(ByVal TestoAnchor As String, ByVal IdAnchor As String) As String
            Dim Anchor As String = ""
            Anchor = "<a href=""#" + IdAnchor + """>" + TestoAnchor + "</a>"
            Return Anchor
        End Function

        '<Nullable(True)> 
        Public Property DataModifica() As Nullable(Of DateTime)
            Get
                Return _DataModifica
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _DataModifica = value
            End Set
        End Property

        Public Property Persona() As COL_BusinessLogic_v2.CL_persona.COL_Persona
            Get
                Return _Persona
            End Get
            Set(ByVal value As COL_BusinessLogic_v2.CL_persona.COL_Persona)
                _Persona = value
            End Set
        End Property

        Public Property Topic() As TopicWiki
            Get
                Return _Topic
            End Get
            Set(ByVal value As TopicWiki)
                _Topic = value
            End Set
        End Property

        Public Property isCancellato() As Boolean
            Get
                Return _isDeleted
            End Get
            Set(ByVal value As Boolean)
                _isDeleted = value
            End Set
        End Property

        Public Property IsNew() As Boolean
            Get
                Return _IsNew
            End Get
            Set(ByVal value As Boolean)
                _IsNew = value
            End Set
        End Property
    End Class
End Namespace