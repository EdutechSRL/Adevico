Imports System.Text

Namespace WikiNew
    Public Enum SingoloPermesso
        Lettura = 1
        Scrittura = 2
        Modifica = 4
        Cancellazione = 8
        Moderazione = 16
        AssegnarePermesso = 32
        Amministrazione = 64
        Send = 128
        Receive = 256
        Syncronize = 512
        Browse = 1024
        Print = 2048
        CambiaProprietario = 4096
        RipristinaRuoloPrecedente = 8192
        RipristinaUtentePrecedente = 16384
    End Enum

    <Serializable()> Public Class Permesso
        'Inherits DomainObject
        Implements IComparable


        Private _id As Integer
        Private _descrizione As String
        Private _nome As String
        Private _posizione As Integer
        Private _intpermesso As Integer

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal intpermesso As Integer)
            MyBase.New()
            Me._intpermesso = intpermesso
        End Sub

#Region "Property"
        Public Property IntPermesso() As Integer
            Get
                Return _intpermesso
            End Get
            Set(ByVal value As Integer)
                _intpermesso = value
            End Set
        End Property

        Public Property BinPermesso() As String
            Get
                Return ToBinary(_intpermesso)
            End Get
            Set(ByVal value As String)
                _intpermesso = ToInteger(value)
            End Set
        End Property

        Public Property BinPermessoReversed() As String
            Get
                Return Reverse(ToBinary(_intpermesso))
            End Get
            Set(ByVal value As String)
                _intpermesso = ToInteger(Reverse(value))
            End Set
        End Property

        Public Property EnumPermesso() As SingoloPermesso
            Get
                Return Me.IntPermesso
            End Get
            Set(ByVal value As SingoloPermesso)
                Me.IntPermesso = value
            End Set
        End Property
#End Region


        Public Property Descrizione() As String
            Get
                Return _descrizione
            End Get
            Set(ByVal value As String)
                _descrizione = value
            End Set
        End Property

        Public Property ID() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        'Public Property IntPermesso() As Integer
        '    Get

        '    End Get
        '    Set(ByVal value As Integer)

        '    End Set
        'End Property

        Public Property Nome() As String
            Get
                Return _nome
            End Get
            Set(ByVal value As String)
                _nome = value
            End Set
        End Property

        Public Property Posizione() As Integer
            Get
                Return _posizione
            End Get
            Set(ByVal value As Integer)
                _posizione = value
            End Set
        End Property





#Region "Utility"

        Public Function Reverse(ByVal [String] As String) As String
            Dim final As New StringBuilder
            For i As Integer = [String].Length - 1 To 0 Step -1 'read the string backwards
                final.Append([String].Substring(i, 1))
            Next
            Return final.ToString
        End Function

        Public Function ToInteger(ByVal x As String) As Integer
            Dim temp As Integer
            Dim ch As Char
            Dim multiply As Long = 1
            Dim subtract As Integer = 1
            Dim len As Integer

            For Each ch In x
                For len = 1 To x.Length - subtract
                    multiply *= 2
                Next
                multiply *= Integer.Parse(ch.ToString)
                temp += multiply
                subtract += 1
                multiply = 1
            Next

            Return temp
        End Function

        Public Function ToBinary(ByVal x As Integer) As String
            Dim temp As String = ""

            Do While x > 0
                If x Mod 2 = 0 Then
                    temp = "0" + temp
                Else
                    temp = "1" + temp
                End If
                x = x \ 2
            Loop
            Return temp
        End Function

        Public Function Pow2(ByVal number As Integer) As Integer
            Return 2 ^ number
        End Function

        Public Sub addPermesso(ByVal intPermesso As Integer)
            _intpermesso = _intpermesso Or intPermesso
        End Sub

        Public Sub removePermesso(ByVal intPermesso As Integer)
            If Me.Check(intPermesso) Then
                Me.IntPermesso -= intPermesso
            End If
        End Sub

        Public Function ToListSingoloPermesso() As IList
            Dim tmp As New ArrayList

            Dim st As String = Me.BinPermesso
            Dim i As Integer
            Dim k As Integer = 0
            For i = st.Length - 1 To 0 Step -1
                If st.Chars(i) = "1" Then
                    Dim x As SingoloPermesso = Pow2(k)
                    tmp.Add(x)

                End If
                k += 1
            Next

            Return tmp
        End Function

#End Region

#Region "Checker"

        Public Function Check(ByVal oPermesso As Permesso) As Boolean
            Return Check(oPermesso.IntPermesso)
        End Function

        Public Function Check(ByVal iPermesso As Integer) As Boolean
            Dim out As UInt32
            out = Me.IntPermesso And iPermesso

            Return (out > 0)
        End Function

        Public Function Check(ByVal iPermesso As Integer, ByVal strict As Boolean) As Boolean
            If strict Then
                Return CheckStrict(iPermesso)
            Else
                Return Check(iPermesso)
            End If
        End Function

        Public Function Check(ByVal oPermesso As Permesso, ByVal strict As Boolean) As Boolean
            If strict Then
                Return CheckStrict(oPermesso)
            Else
                Return Check(oPermesso)
            End If
        End Function

        Public Function CheckStrict(ByVal iPermesso As Integer) As Boolean
            Dim out As UInt32
            out = Me.IntPermesso And iPermesso

            Return (out = Me.IntPermesso)
        End Function

        Public Function CheckStrict(ByVal oPermesso As Permesso) As Boolean
            Dim out As UInt32
            out = Me.IntPermesso And oPermesso.IntPermesso

            Return (out = Me.IntPermesso)
        End Function
#End Region


        Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
            Dim other As Permesso
            other = CType(obj, Permesso)

            Return Me.IntPermesso.CompareTo(other.IntPermesso)
        End Function

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            Dim other As Permesso
            other = CType(obj, Permesso)

            Return Me.IntPermesso.Equals(other.IntPermesso)
        End Function

    End Class
End Namespace