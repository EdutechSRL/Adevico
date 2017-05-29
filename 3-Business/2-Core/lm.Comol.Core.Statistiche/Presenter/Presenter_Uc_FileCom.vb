Public Class Presenter_Uc_FileCom
    Private _view As I_Uc_FileCom

    Public Sub New(ByVal View As I_Uc_FileCom)
        Me._view = View
    End Sub

    Public Sub BindUserList(ByVal CommunityID As Integer, Optional ByVal SearchBy As SearchBy = SearchBy.Tutti, Optional ByVal Value As String = "")


        Dim RealTable As List(Of lm.Comol.Core.Statistiche.StatFileUsr)
        If SearchBy = SearchBy.Tutti Or Value = "" Then
            RealTable = FileManager.GetUserListCom(CommunityID)
        Else
            RealTable = FileManager.GetUserListCom(CommunityID, SearchBy, Value)
        End If


        Dim OrderField As OrderFields = Me._view.OrderField
        Dim OrderDir As Boolean = Me._view.OrderDirection
        Dim PageNum As Integer = 0
        Try
            PageNum = Me._view.Pager.PageIndex
        Catch ex As Exception
        End Try

        Dim PageSize As Integer = Me._view.PageSize

        'PageNum -= 1
        If PageNum < 0 Then
            PageNum = 0
        End If

        If PageSize < 5 Then
            PageSize = 5
        End If

        Dim _TotalRecord As Integer = RealTable.Count
        Dim _NumRec As Integer = 0

        Dim _TotalPage As Integer = _TotalRecord / PageSize
        '        __ __    __ __ __         ___ ___ _
        If (_TotalPage * PageSize) < _TotalRecord Then
            _TotalPage += 1
        End If

        If PageNum + 1 >= _TotalPage Then 'Ultima pagina
            _NumRec = _TotalRecord - (_TotalPage * PageSize)
        Else
            _NumRec = PageSize
        End If

        If _NumRec < 0 Then
            _NumRec = PageSize
        End If
        Dim skipRec As Integer = PageNum * PageSize

        Dim Query = From oSFU In RealTable Select oSFU
        Select Case OrderField
            Case OrderFields.Name
                If OrderDir Then
                    Query = From oSFU In RealTable Select oSFU Order By oSFU.Nome Ascending Skip skipRec Take _NumRec
                Else
                    Query = From oSFU In RealTable Select oSFU Order By oSFU.Nome Descending Skip skipRec Take _NumRec
                End If

            Case OrderFields.FamName
                If OrderDir Then
                    Query = From oSFU In RealTable Select oSFU Order By oSFU.Cognome Ascending Skip skipRec Take _NumRec
                Else
                    Query = From oSFU In RealTable Select oSFU Order By oSFU.Cognome Descending Skip skipRec Take _NumRec
                End If

            Case OrderFields.Role
                If OrderDir Then
                    Query = From oSFU In RealTable Select oSFU Order By oSFU.Ruolo Ascending Skip skipRec Take _NumRec
                Else
                    Query = From oSFU In RealTable Select oSFU Order By oSFU.Ruolo Descending Skip skipRec Take _NumRec
                End If
            Case OrderFields.Down_Num
                If OrderDir Then
                    Query = From oSFU In RealTable Select oSFU Order By oSFU.NumDown Ascending Skip skipRec Take _NumRec
                Else
                    Query = From oSFU In RealTable Select oSFU Order By oSFU.NumDown Descending Skip skipRec Take _NumRec
                End If
            Case Else
                If OrderDir Then
                    Query = From oSFU In RealTable Select oSFU Order By oSFU.Cognome Ascending Skip skipRec Take _NumRec
                Else
                    Query = From oSFU In RealTable Select oSFU Order By oSFU.Cognome Descending Skip skipRec Take _NumRec
                End If
        End Select

        '-+-+-+-+-+-+-  >> P A G E R << -+-+-+-+-+-+-
        Dim oPager As New lm.Comol.Core.DomainModel.PagerBase(PageSize, PageNum, _TotalPage)
        oPager.Count = _TotalRecord
        oPager.PageIndex = PageNum
        Me._view.Pager = oPager

        Me._view.BindListUtenti(Query.ToList())
    End Sub

    Public Enum SearchBy As Integer
        Tutti = -1
        Nome = 1
        Cognome = 2
        NomeECognome = 3
    End Enum

    Public Enum OrderFields As Integer
        Name = 1
        FamName = 2
        Role = 3
        Down_Num = 5
    End Enum
End Class
