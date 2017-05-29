'Imports ManagedDesigns.ApplicationBlocks.Validation.Constraints
Namespace WikiNew

    <Serializable()> Public Class TopicImmaginiWiki
        'Inherits DomainObject

        Private _ID As Guid
        Private _AlternateTopicImmagini As String
        Private _Width As String
        Private _Height As String
        Private _oFile As Files
        Private _oTopic As TopicWiki

        Property ID() As Guid
            Get
                Return _ID
            End Get
            Set(ByVal value As Guid)
                _ID = value
            End Set
        End Property
        Property Alternate() As String
            Get
                Return _AlternateTopicImmagini
            End Get
            Set(ByVal value As String)
                _AlternateTopicImmagini = value
            End Set
        End Property
        Property Width() As String
            Get
                Return _Width
            End Get
            Set(ByVal value As String)
                _Width = value
            End Set
        End Property
        Property Height() As String
            Get
                Return _Height
            End Get
            Set(ByVal value As String)
                _Height = value
            End Set
        End Property
        Property oFile() As Files
            Get
                Return _oFile
            End Get
            Set(ByVal value As Files)
                _oFile = value
            End Set
        End Property
        Property oTopic() As TopicWiki
            Get
                Return _oTopic
            End Get
            Set(ByVal value As TopicWiki)
                _oTopic = value
            End Set
        End Property

    End Class

End Namespace