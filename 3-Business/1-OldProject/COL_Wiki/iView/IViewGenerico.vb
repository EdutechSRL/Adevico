Public Interface IViewGenerico
    ReadOnly Property MyComunitaCorrente() As COL_BusinessLogic_v2.Comunita.COL_Comunita
    ReadOnly Property MyPersonaCorrente() As COL_BusinessLogic_v2.CL_persona.COL_Persona
    ReadOnly Property MyIscrizioneCorrente() As COL_BusinessLogic_v2.IscrizioneComunita

    Property TitoloPagina() As String
End Interface
