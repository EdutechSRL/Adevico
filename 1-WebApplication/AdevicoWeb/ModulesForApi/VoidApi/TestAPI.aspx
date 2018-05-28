<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="VoidAPI.aspx.vb" Inherits="Comunita_OnLine.VoidAPI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
    Test Page
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
    <!-- Service Localization (.js) -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- Head Content: .ccs, .js -->
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    <%=GetLocalization("Title.test.text")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="hide">
        <asp:Literal runat="server" ID="LTlocalizationService">APItest</asp:Literal>
        <asp:Literal runat="server" ID="LTlocalizationValue"></asp:Literal>
    </div>
    <!-- Service content (page) -->
	<h1>Test</h1>
    <ol>
        <li>
            <%=ShowCookie().Replace(";", "</li><li>")%>
        </li>
    </ol>
    
        <%--Add for Session--%>
    
    <asp:UpdatePanel ID="UPTempo" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TMsession" EventName="Tick" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Timer ID="TMsession" runat="server">
    </asp:Timer>
</asp:Content>
