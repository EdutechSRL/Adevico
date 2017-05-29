Public Class Presenter_Uc_FilePers
    Private _view As I_Uc_FilePers

    Public Sub New(ByVal View As I_Uc_FilePers)
        Me._view = View
    End Sub
    Public Sub BindFileStat()
        Me._view.TotaleDownload = FileManager.CountaDownload(Me._view.UserID, Me._view.CommunityID)
        Me._view.BindUserList(FileManager.GetFilePersStat(Me._view.UserID, Me._view.CommunityID))
    End Sub
End Class