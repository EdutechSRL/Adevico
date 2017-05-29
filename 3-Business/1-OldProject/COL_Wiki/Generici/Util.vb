'Contiene tutte le CLASSI di L3 che vengono utilizzate dalle Wiki.
'Necessario recuperarle ed inserirle qui... (se non esistono già in COL)
Imports System.Text.RegularExpressions
Imports System.Reflection

Namespace Util

    Public Class HTMLUtil
        ''' <summary>
        ''' estrae un elenco degli header [H*] [/H*]
        ''' </summary>
        ''' <param name="strHTML"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ExtractH(ByVal strHTML As String) As IList
            Dim matchs As System.Text.RegularExpressions.MatchCollection

            Dim outputlist As IList = New ArrayList

            outputlist.Clear()

            Dim objRegExp As New Regex("<[ \t]*h(1|2|3|4|5|6|)(.|\n)+?/[ \t]*h(1|2|3|4|5|6)[ \t]*>", RegexOptions.IgnoreCase) '<h*>...</h*>
            matchs = objRegExp.Matches(strHTML)

            For Each match As System.Text.RegularExpressions.Match In matchs
                outputlist.Add(LowerTags(match.Value, "h1;h2;h3;h4;h5;h6"))
            Next

            Return outputlist
        End Function

        ''' <summary>
        ''' rende lowerCase tutti i tag indicati
        ''' </summary>
        ''' <param name="strHTML"></param>
        ''' <param name="tags"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function LowerTags(ByVal strHTML As String, ByVal tags As String) As String
            Dim strOutput As String = strHTML

            Dim tagArr() As String = tags.Split(",;".ToCharArray)

            For Each tag As String In tagArr
                If tag <> "" Then
                    strOutput = LowerTag(strOutput, tag)
                End If
            Next

            Return strOutput
        End Function

        ''' <summary>
        ''' rende lowerCase tutti i gli elementi tag
        ''' </summary>
        ''' <param name="strHTML"></param>
        ''' <param name="tag"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function LowerTag(ByVal strHTML As String, ByVal tag As String) As String
            Dim objRegExp As New Regex("<[ \t]*/[ \t]*" + tag + "[ \t]*>", RegexOptions.IgnoreCase) '</tag>
            Dim strOutput As String

            strOutput = objRegExp.Replace(strHTML, "</" + tag.ToLower + ">")

            objRegExp = New Regex("<[ \t]*" + tag + "[ \t]*>", RegexOptions.IgnoreCase) '<tag>
            strOutput = objRegExp.Replace(strOutput, "<" + tag.ToLower + ">")

            Return strOutput
        End Function
    End Class

    Public Class StringUtil
        Public Shared Function ReplicaStr(ByVal volte As Integer, ByVal stringa As String) As String
            Dim i As Integer = 0
            Dim risultato As String = ""
            For i = 0 To volte
                risultato += stringa
            Next
            Return risultato
        End Function
    End Class

    Public Class GenericNestedComparer
        Implements IComparer

        Public Enum OrderEnum
            Ascending = 1
            Descending = -1
            none = 0
        End Enum
        Private _propertieslist As String
        Private _order As OrderEnum
        Private _orders As String = ""

        Public Property Orders() As String
            Get
                Return _orders
            End Get
            Set(ByVal value As String)
                _orders = value
            End Set
        End Property

        Public Property PropertiesList() As String
            Get
                Return _propertieslist
            End Get
            Set(ByVal value As String)
                If value.Contains(";") Then
                    _orders = value
                End If
                _propertieslist = value
            End Set
        End Property

        Public Property Order() As OrderEnum
            Get
                Return _order
            End Get
            Set(ByVal value As OrderEnum)
                _order = value
            End Set
        End Property

        Public Function GetNested(ByVal WorkObj As Object, ByVal PropertiesList As String) As Object
            If WorkObj IsNot Nothing Then
                Dim WorkingType As Type = WorkObj.GetType
                Dim Properties() As String
                Dim WorkingObject As Object
                Properties = PropertiesList.Split(".")
                Try
                    WorkingObject = WorkObj

                    For i As Integer = 0 To Properties.Length - 1
                        Dim y As PropertyInfo = WorkingType.GetProperty(Properties(i))
                        WorkingObject = y.GetValue(WorkingObject, Nothing)
                        WorkingType = y.PropertyType
                    Next

                    Return WorkingObject
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                Return Nothing
            End If

        End Function

        Public Sub New(Optional ByVal Properties As String = "", Optional ByVal Order As OrderEnum = OrderEnum.Ascending)
            MyBase.New()
            PropertiesList = Properties
            Me.Order = Order
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

            If Orders <> "" Then
                Dim orderslist() As String = Orders.Split(";")
                Dim c As Integer = 0
                Dim idx As Integer = 0


                While (c = 0) And (idx < orderslist.Length)
                    If orderslist(idx).Contains(" ASC") Then
                        Me.Order = OrderEnum.Ascending
                    Else
                        If orderslist(idx).Contains(" DESC") Then
                            Me.Order = OrderEnum.Descending
                        End If
                    End If

                    Me.PropertiesList = orderslist(idx).Replace(" ASC", "").Replace(" DESC", "")

                    c = Me.compareSingolo(x, y)
                    idx += 1
                End While
                Return c
            Else
                Return compareSingolo(x, y)
            End If

        End Function

        Public Function compareSingolo(ByVal x As Object, ByVal y As Object) As Integer
            Dim x_obj As IComparable
            Dim y_obj As IComparable
            x_obj = GetNested(x, PropertiesList)
            y_obj = GetNested(y, PropertiesList)

            Return x_obj.CompareTo(y_obj) * Order

        End Function

    End Class
End Namespace