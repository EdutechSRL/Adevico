Imports System
Imports System.Text

''' <summary>
''' Create on: 17/12/2009
''' Create by: Mirco Borsato
''' Convert to vb.net on 19/01/2010
'''
''' Function to create an XML-Excel style document from any type of data.
''' Tested on Excel 2003 and Excel 2010.
''' If you create an .xml document, windows open it in Excel and not the default program associates with *.xml file. (YEAH!)
''' Order:
''' 1. Row += ExcelXML.AddData(obj1.Property1);
''' Row += ExcelXML.AddData(obj1.Property2);
''' ...
''' 2. ForEach item in list:
''' Table += ExcelXML.AddRow(Row); (1 Item == 1 Row)
''' 3. Add header and footer to Table (a sheet per table):
''' Table = ExcelXML.AddWorkSheet("SheetName", Table)
''' 4. You can create multi sheet and multi Table to main document:
''' MainDoc = Table1 + Table2 + ...
''' 5. Add default parameter to document:
''' MainDoc = ExcelXML.AddMain(MainDoc)
''' Note:
''' You can add a linked-cell.
''' If you want link to another cell in document, use GetInternalLink(SheetName, CellName). E.g. SheetName="Main", CellName="A1".
''' Default value for CellName (if = "") is "A1".
''' If you link to a file, in a folder with relative path, you must duplicate folder: E.g. /folder/filename.ext point to /folder/folder/filename.ext.
''' I think this is a bug from Excel2003 to Excel2010. (Manteined for backward compatibility?)
''' </summary>
''' <example>
''' string XML = "";
''' string XMLTable1 = "";
''' string XMLTable2 = "";
''' foreach (FakeObject1 Obj in GetFakeList(15))
''' {
''' string XMLRow = "";
''' XMLRow += ExcelXML.AddLinkData(Obj.Name,ExcelXML.GetInternalLink("Secondary",""));
''' XMLRow += ExcelXML.AddData(Obj.Description);
''' XMLRow += ExcelXML.AddData(Obj.Number);
''' XMLRow += ExcelXML.AddData(Obj.Time);
''' XMLRow += ExcelXML.AddData(Obj.Object);
''' XMLTable1 += ExcelXML.AddRow(XMLRow);
''' }
''' XMLTable2 = XMLTable1; //Only for sample. Put your code for create a New Table
''' XMLTable1 = ExcelXML.AddWorkSheet("Main", XMLTable1);
''' XMLTable2 = ExcelXML.AddWorkSheet("Secondary", XMLTable2);
''' XML = ExcelXML.AddMain(XMLTable1 + XMLTable2);
''' return XML;
''' </example>
''' <remarks>
''' Remember the correct order.
''' </remarks>
Public Class ExcelXML
    Public Shared Function AddMain(ByVal Content As [String]) As [String]
        Dim startExcelXML As String = "<?xml version=""1.0""?>" & _
            vbCr & vbLf & vbCr & vbLf & _
            "<?mso-application progid=""Excel.Sheet""?>" & _
            vbCr & vbLf & _
            "<Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet""" & _
            vbCr & vbLf & " xmlns:o=""urn:schemas-microsoft-com:office:office""" & _
            vbCr & vbLf & " " & _
            "xmlns:x=""urn:schemas-microsoft-com:office:" & "excel""" & _
            vbCr & vbLf & _
            " xmlns:ss=""urn:schemas-microsoft-com:" & "office:spreadsheet"">" & _
            vbCr & vbLf & "<Styles>" & vbCr & vbLf & " " & _
            "<Style ss:ID=""Default"" ss:Name=""Normal"">" & _
            vbCr & vbLf & " " & _
            "<Alignment ss:Vertical=""Bottom""/>" & _
            vbCr & vbLf & _
            " <Borders/>" & _
            vbCr & vbLf & _
            " <Font/>" & _
            vbCr & vbLf & _
            " <Interior/>" & _
            vbCr & vbLf & _
            " <NumberFormat/>" & _
            vbCr & vbLf & _
            " <Protection/>" & _
            vbCr & vbLf & _
            "</Style>" & _
            vbCr & vbLf & " " & vbCr & vbLf & _
            " <Style ss:ID=""Date"">" & _
            vbCr & vbLf & _
            " <NumberFormat ss:Format=""d/m/yy\ h\.mm;@""/>" & _
            vbCr & vbLf & _
            " </Style>" & _
            vbCr & vbLf & _
            " <Style ss:ID=""Time"">" & _
            vbCr & vbLf & _
            " <NumberFormat ss:Format=""[$-F400]h:mm:ss\ AM/PM""/>" & _
            vbCr & vbLf & _
            " </Style>" & _
            vbCr & vbLf & _
            "</Styles>" & _
            vbCr & vbLf & " " + _
            Content & _
            vbCr & vbLf & _
            "</Workbook>"
        Return (startExcelXML)
    End Function
    Public Shared Function AddWorkSheet(ByVal Name As [String], ByVal Content As [String]) As [String]
        Dim StrOut As String
        StrOut = vbCr & vbLf & _
            "<Worksheet ss:Name=""" & _
            Name & _
            """>" & _
            vbCr & vbLf & vbCr & vbLf & _
            "<Table>" & _
            vbCr & vbLf & _
            Content & _
            vbCr & vbLf & _
            "</Table>" & _
            vbCr & vbLf & vbCr & vbLf & _
            "</Worksheet>" & _
            vbCr & vbLf
        Return StrOut
    End Function
    Public Shared Function AddRow(ByVal Content As [String]) As [String]
        Return (vbCr & vbLf & "<Row>" & Content & vbCr & vbLf & "</Row>")
    End Function

#Region "Numeric type"
    'public static String AddData(int Content)
    '{
    ' //1</Data></Cell>
    ' return "\r\n<Cell><Data ss:Type=\"Number\">" +
    ' Content.ToString() +
    ' "</Data></Cell>";
    '}
    Public Shared Function AddData(ByVal Content As Int16) As [String]
        '1</Data></Cell>
        Return (vbCr & vbLf & "<Cell><Data ss:Type=""Number"">" & Content.ToString() & "</Data></Cell>")
    End Function
    Public Shared Function AddData(ByVal Content As Int32) As [String]
        '1</Data></Cell>
        Return (vbCr & vbLf & "<Cell><Data ss:Type=""Number"">" & Content.ToString() & "</Data></Cell>")
    End Function
    Public Shared Function AddData(ByVal Content As Int64) As [String]
        '1</Data></Cell>
        Return (vbCr & vbLf & "<Cell><Data ss:Type=""Number"">" & Content.ToString() & "</Data></Cell>")
    End Function
    Public Shared Function AddData(ByVal Content As [Decimal]) As [String]
        '1</Data></Cell>
        Return (vbCr & vbLf & "<Cell><Data ss:Type=""Number"">" & Content.ToString() & "</Data></Cell>")
    End Function
    Public Shared Function AddData(ByVal Content As Double) As [String]
        '1</Data></Cell>
        Return (vbCr & vbLf & "<Cell><Data ss:Type=""Number"">" & Content.ToString() & "</Data></Cell>")
    End Function
    Public Shared Function AddData(ByVal Content As Single) As [String]
        '1</Data></Cell>
        Return (vbCr & vbLf & "<Cell><Data ss:Type=""Number"">" & Content.ToString() & "</Data></Cell>")
    End Function
#End Region
#Region "String and Generic"
    Public Shared Function AddData(ByVal Content As String) As [String]
        CheckString(Content)
        Return (vbCr & vbLf & "<Cell><Data ss:Type=""String"">" & Content.ToString() & "</Data></Cell>")
    End Function
    Public Shared Function AddData(ByVal Content As Object) As [String]
        Return (vbCr & vbLf & "<Cell><Data ss:Type=""String"">" & Content.ToString() & "</Data></Cell>")
    End Function
#End Region
#Region "DateTime"
    Public Shared Function AddData(ByVal Content As DateTime) As [String]
        Return (vbCr & vbLf & "<Cell ss:StyleID=""Date""><Data ss:Type=""DateTime"">" & [String].Format("{0:s}", Content) & "</Data></Cell>")
    End Function
    Public Shared Function AddData(ByVal Content As TimeSpan) As [String]
        Dim Time As New DateTime(1899, 12, 31, Content.Hours, Content.Minutes, Content.Seconds)
        Return (vbCr & vbLf & "<Cell ss:StyleID=""Time""><Data ss:Type=""DateTime"">" & [String].Format("{0:s}", Time) & "</Data></Cell>")
    End Function
    Public Shared Function AddDataDateTime(ByVal Content As String) As [String]
        Dim Dt As DateTime
        Try
            Dt = System.Convert.ToDateTime(Content)
        Catch ex As Exception
        End Try
        If IsNothing(Dt) Then
            Return (vbCr & vbLf & "<Cell ss:StyleID=""Date""><Data ss:Type=""DateTime""></Data></Cell>")
        ElseIf Dt.Year < 1900 Then
            Return (vbCr & vbLf & "<Cell ss:StyleID=""Date""><Data ss:Type=""DateTime""></Data></Cell>")
        Else
            Return (vbCr & vbLf & "<Cell ss:StyleID=""Date""><Data ss:Type=""DateTime"">" & [String].Format("{0:s}", Dt) & "</Data></Cell>")
        End If

    End Function
#End Region
#Region "Other Type"
    Public Shared Function AddDataBoolean(ByVal Content As Boolean) As [String]
        If IsNothing(Content) Then
            Return (vbCr & vbLf & "<Cell ss:StyleID=""Date""><Data ss:Type=""Boolean""></Data></Cell>")
        ElseIf Content Then
            Return (vbCr & vbLf & "<Cell ss:StyleID=""Date""><Data ss:Type=""Boolean"">1</Data></Cell>")
        Else
            Return (vbCr & vbLf & "<Cell ss:StyleID=""Date""><Data ss:Type=""Boolean"">0</Data></Cell>")
        End If
    End Function
#End Region

#Region "Linked Numeric type"
    Public Shared Function AddLinkData(ByVal Content As Int16, ByVal link As String) As [String]
        Return (vbCr & vbLf & "<Cell ss:HRef=""" & link & """>" & "<Data ss:Type=""Number"">") + Content.ToString() & "</Data></Cell>"
    End Function
    Public Shared Function AddLinkData(ByVal Content As Int32, ByVal link As String) As [String]
        Return (vbCr & vbLf & "<Cell ss:HRef=""" & link & """>" & "<Data ss:Type=""Number"">") + Content.ToString() & "</Data></Cell>"
    End Function
    Public Shared Function AddLinkData(ByVal Content As Int64, ByVal link As String) As [String]
        Return (vbCr & vbLf & "<Cell ss:HRef=""" & link & """>" & "<Data ss:Type=""Number"">") + Content.ToString() & "</Data></Cell>"
    End Function
    Public Shared Function AddLinkData(ByVal Content As [Decimal], ByVal link As String) As [String]
        Return (vbCr & vbLf & "<Cell ss:HRef=""" & link & """>" & "<Data ss:Type=""Number"">") + Content.ToString() & "</Data></Cell>"
    End Function
    Public Shared Function AddLinkData(ByVal Content As Single, ByVal link As String) As [String]
        Return (vbCr & vbLf & "<Cell ss:HRef=""" & link & """>" & "<Data ss:Type=""Number"">") + Content.ToString() & "</Data></Cell>"
    End Function
#End Region
#Region "Linked String and Generic"
    Public Shared Function AddLinkData(ByVal Content As String, ByVal link As String) As [String]
        CheckString(Content)
        Return (vbCr & vbLf & "<Cell ss:HRef=""" & link & """>" & "<Data ss:Type=""String"">") + Content.ToString() & "</Data></Cell>"
    End Function
    Public Shared Function AddLinkData(ByVal Content As Object, ByVal link As String) As [String]
        Return (vbCr & vbLf & "<Cell ss:HRef=""" & link & """>" & "<Data ss:Type=""String"">") + Content.ToString() & "</Data></Cell>"
    End Function
#End Region
    Public Shared Function GetInternalLink(ByVal SheetName As [String], ByVal CellName As [String]) As [String]
        If CellName = "" Then
            CellName = "A1"
        End If
        Return ("#" & SheetName & "!") + CellName
    End Function

    Private Shared Function CheckString(ByVal Input As String) As String
        If IsNothing(Input) Then
            Return ""
        Else
            Return Input
        End If
    End Function
End Class