Public Interface I_Uc_FilePers
    Inherits IpersonalContainer

    Sub BindUserList(ByVal oUserList As IList(Of FilePersonalStat))
    WriteOnly Property TotaleDownload()

End Interface
