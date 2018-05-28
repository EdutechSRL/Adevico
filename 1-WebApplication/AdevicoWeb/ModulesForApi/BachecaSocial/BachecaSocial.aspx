<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="VoidAPI.aspx.vb" Inherits="Comunita_OnLine.VoidAPI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
    Bacheca social
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
    <!-- Service Localization (.js) -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- Head Content: .ccs, .js -->
    <script src="../Resources/frontendCustom/Utils.js"></script>
	<link rel="Stylesheet" href="css/bachecaSocial.css" />
	<style type="text/css">
        .AdvSpacer{
            height:177px;
        }
    </style>
    <% If isForIframe Then %>
	    <link rel="Stylesheet" href="../Resources/Styles/forIframeRemoveBroder.css" />	           
    <% End If%>
	<script async defer src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCASRJqlZwddRJ04Etc076bMBGiVcmU4Ao" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Bacheca social<%--=GetLocalization("Title.test.text")--%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="hide">
        <asp:Literal runat="server" ID="LTlocalizationService">APItest</asp:Literal>
        <asp:Literal runat="server" ID="LTlocalizationValue"></asp:Literal>
    </div>
	
	<div id="divMatsterStatisticheAdevico">
		<div id="boxBachecaSocial"></div>
	</div>							
	<script type="text/javascript" src="js/jQueryWidgetBachecaSocial.js"></script>
	
	<div style="height:0;overflow:hidden;position:relative;">
	    <!-- Service content (page) -->
	    <h1>Test</h1>
        <ol>
            <li>
                <%=ShowCookie().Replace(";", "</li><li>")%>
            </li>
        </ol>
	</div>
	 <%--Add for Session--%>
    
    <asp:UpdatePanel ID="UPTempo" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TMsession" EventName="Tick" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Timer ID="TMsession" runat="server">
    </asp:Timer>
</asp:Content>
