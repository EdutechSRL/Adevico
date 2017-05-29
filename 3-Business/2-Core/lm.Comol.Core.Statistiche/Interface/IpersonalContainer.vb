Public Interface IpersonalContainer
	Sub Bind(ByVal Community_Id As Integer, ByVal User_Id As Integer)
	Property UserID() As Integer
	Property CommunityID() As Integer
End Interface