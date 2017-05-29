Public Class PresenterGenerico

    'Private _oResource As ResourceManager
    'Public Property oResource() As ResourceManager
    '    Get
    '        Return Me._oResource
    '    End Get
    '    Set(ByVal value As ResourceManager)
    '        Me._oResource = value
    '    End Set
    'End Property

    Private view As IViewGenerico

    ''' <summary>
    ''' Serve solo all'intellisense, altrimenti da errore...
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
    End Sub
    ''' <summary>
    ''' Per inizializzare la pagina che verrà pilotata dal presenter.
    ''' Utilizzare sempre questa
    ''' </summary>
    ''' <param name="view"></param>
    ''' <remarks>
    ''' Manca l'oggetto oResource! Rivedere la logica nella nuova ottica...
    ''' </remarks>
    Public Sub New(ByVal view As IViewGenerico)
        Me.view = view
    End Sub

    'Public Sub SetCulture(ByVal Code As String)
    '    Me.oResource = New ResourceManager

    '    Me.oResource.UserLanguages = Code
    '    Me.oResource.ResourcesName = "pg_bacheca"
    '    Me.oResource.Folder_Level1 = "Generici"
    '    Me.oResource.setCulture()

    '    'Me.view.SetUpInternazionalizzazione '!?!?!?
    'End Sub

End Class
