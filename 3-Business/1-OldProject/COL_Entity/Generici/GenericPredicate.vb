Imports Microsoft.VisualBasic

Public Delegate Function PredicateWrapperDelegate(Of T, A) _
 (ByVal item As T, ByVal argument As A) As Boolean


Public Class GenericPredicate(Of T, A)
	Private _argument As A
	Private _wrapperDelegate As PredicateWrapperDelegate(Of T, A)

	Public Sub New(ByVal argument As A, _
	 ByVal wrapperDelegate As PredicateWrapperDelegate(Of T, A))

		_argument = argument
		_wrapperDelegate = wrapperDelegate
	End Sub

	Private Function InnerPredicate(ByVal item As T) As Boolean
		Return _wrapperDelegate(item, _argument)
	End Function

	Public Shared Widening Operator CType( _
	 ByVal wrapper As GenericPredicate(Of T, A)) _
	 As Predicate(Of T)

		Return New Predicate(Of T)(AddressOf wrapper.InnerPredicate)
	End Operator



End Class
