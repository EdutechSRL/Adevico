﻿<Serializable()>
Public Class dtoExportRow
    Inherits lm.Comol.Core.DomainModel.Helpers.Export.ExportCsvBaseHelper
    Public Property Index As Integer
    Public Property IdAnswer As Long
    Public Property DisplayInfo As dtoDisplayName
    Public Property Items As List(Of String)

    Sub New()
        Items = New List(Of String)
        DisplayInfo = New dtoDisplayName
    End Sub

    Public Function Export(ByVal fullInfo As Boolean, displayTaxCode As Boolean) As String
        Dim result = IdAnswer & EndFieldItem & IIf(fullInfo, AppendItem(DisplayInfo.Surname) & AppendItem(DisplayInfo.Name) & IIf(displayTaxCode, AppendItem(DisplayInfo.TaxCode), "") & AppendItem(DisplayInfo.OtherInfos), AppendItem(DisplayInfo.DisplayName))
        If Items.Count > 0 Then
            Items.ForEach(Sub(c) result &= AppendItem(c))
        End If

        Return result
    End Function

End Class

<Serializable()>
Public Class dtoExportQuestionnaire
    Public Property Cells As List(Of String)
    Public Property Rows As List(Of dtoExportRow)

    Sub New()
        Cells = New List(Of String)
        Rows = New List(Of dtoExportRow)
    End Sub

End Class