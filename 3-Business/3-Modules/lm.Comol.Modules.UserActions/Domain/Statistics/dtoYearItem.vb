Namespace lm.Comol.Modules.UserActions.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoYearItem
        Public Value As Integer
        Public Items As List(Of dtoMonthItem)

        Public Sub New()
            Items = New List(Of dtoMonthItem)
        End Sub
        Public Sub New(year As Integer, month As Integer, day As Integer)
            Items = New List(Of dtoMonthItem)
            Value = year
            Items.Add(New dtoMonthItem(month, day))
        End Sub

        Public Function GetOrderdMonth() As List(Of dtoMonthItem)
            Return Items.Distinct.OrderBy(Function(m) m.Value).ToList()
        End Function
        Public Function GetOrderdMonthValues() As List(Of Integer)
            Return Items.Distinct.OrderBy(Function(m) m.Value).Select(Function(m) m.Value).ToList()
        End Function
        Public Function GetFirstDate() As DateTime
            If Items.Any Then
                Return GetOrderdMonth.FirstOrDefault().GetFirstDate(Value)
            Else
                Return New DateTime(Value, 1, 1)
            End If
        End Function
        Public Function GetLastDate() As DateTime
            If Items.Any Then
                Return GetOrderdMonth.Last().GetLastDate(Value)
            Else
                Return New DateTime(Value, 12, 31)
            End If
        End Function
        
        Public Sub AddMonth(month As Integer, day As Integer)
            If Not Items.Where(Function(m) m.Value = month).Any Then
                Items.Add(New dtoMonthItem(month, day))
            Else
                Items.Where(Function(m) m.Value = month).FirstOrDefault.AddDay(day)
            End If
        End Sub
    End Class

    <Serializable(), CLSCompliant(True)> Public Class dtoMonthItem
        Public Value As Integer
        Public Days As List(Of Integer)
        Public Sub New()
            Days = New List(Of Integer)
        End Sub
        Public Sub New(month As Integer)
            Days = New List(Of Integer)
            Value = month
        End Sub
        Public Sub New(month As Integer, day As Integer)
            Days = New List(Of Integer)
            Value = month
            Days.Add(day)
        End Sub
        Public Sub AddDay(day As Integer)
            If Not Days.Contains(day) Then
                Days.Add(day)
            End If
        End Sub
        Public Function GetOrderdDays() As List(Of Integer)
            Return Days.Distinct.OrderBy(Function(m) m).ToList()
        End Function
        Public Function GetFirstDate(ByVal year As Integer) As DateTime
            If Days.Any Then
                Return New DateTime(year, Value, GetOrderdDays.FirstOrDefault())
            Else
                Return New DateTime(year, Value, 1)
            End If
        End Function
        Public Function GetLastDate(ByVal year As Integer) As DateTime
            If Days.Any Then
                Return New DateTime(year, Value, GetOrderdDays.Last)
            Else
                Return New DateTime(year, Value, 1)
            End If
        End Function
    End Class
End Namespace