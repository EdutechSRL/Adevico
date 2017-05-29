'Utilities per generare Link del tag <head> (Javascript, CSS etc)
Public Class HTMLlinkHelpers

	Public Shared Function CSS(ByVal url As String) As HtmlControls.HtmlLink
		Dim link As HtmlControls.HtmlLink = New HtmlControls.HtmlLink()
		link.Attributes.Add("type", "text/css")
		link.Attributes.Add("rel", "stylesheet")
		link.Attributes.Add("href", url)

		Return link
	End Function

	Public Shared Function CSS(ByVal url As String, ByVal media As String) As HtmlControls.HtmlLink
		Dim link As HtmlControls.HtmlLink = New HtmlControls.HtmlLink()
		link.Attributes.Add("type", "text/css")
		link.Attributes.Add("rel", "stylesheet")
		link.Attributes.Add("media", media)
		link.Attributes.Add("href", url)

		Return link
	End Function

	Public Shared Function JS(ByVal url As String) As HtmlControls.HtmlGenericControl
		Dim link As HtmlControls.HtmlGenericControl = New HtmlControls.HtmlGenericControl("script")
		link.Attributes.Add("type", "text/javascript")
		link.Attributes.Add("language", "javascript")
		link.Attributes.Add("src", url)

		Return link
	End Function

End Class