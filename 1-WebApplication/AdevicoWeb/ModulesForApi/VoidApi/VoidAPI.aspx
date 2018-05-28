<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="VoidAPI.aspx.vb" Inherits="Comunita_OnLine.VoidAPI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
    <!-- Page Title -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
    <!-- Service Localization (.js) -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- Head Content: .ccs, .js -->
    <style type="text/css">
        .AdvSpacer{
            height:177px;
        }
    <%If isForIframe Then%>
	
            /* css temp per iframe*/
            html body{
                background:none !important;
                padding:0;
                margin:0;
            }
			html body #aspnetForm{
				min-width: 0;
			}
            html body .AdvSpacer{
                height:0 !important;
            }
            html body #container {
                padding-top: 0;
                padding-bottom: 0;
                background-color: #fff;
                border: none;
                margin-bottom: 0;
                -moz-border-radius-topleft: 0px;
                -moz-border-radius-topright: 0px;
                -moz-border-radius-bottomright: 0;
                -moz-border-radius-bottomleft: 0;
                -webkit-border-radius: 0px 0px 0 0;
                border-radius: 0px 0px 0 0;
                box-shadow: none;
            }
            html body .page-width {
                margin: auto auto;
                max-width: 100%;
                min-width: 0;
                width: auto;
            }
			html body #serviceinfo,
			html body #header,
			html body #cFooter{
                display:none !important;
            }
            html body #content {
                margin: 0 0;
                min-height: 300px;
                padding: 0;
                text-align: left;
                position: relative;
            }
    <%End If%>
    </style>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    <!-- Title content: service name -->
    <%=GetLocalization("Title.text")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <!-- Service content (page) -->
    <div class="_hide">
        <asp:Literal runat="server" ID="LTlocalizationService">APItest</asp:Literal>
        <span class="localization">
            <asp:Literal runat="server" ID="LTlocalizationValue">
                Message.Warning,
                Message.Info,
                Title.text,
                Title.test.text
            </asp:Literal>
        </span>
        <span class="SAMPLE"><%=GetLocalization("Title.text") %></span>
    </div>
    <br/>
    <%=ShowCookie() %>
    <br/>
   
    
    
    
    
    <%--Add for Session--%>
    
    <asp:UpdatePanel ID="UPTempo" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TMsession" EventName="Tick" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Timer ID="TMsession" runat="server">
    </asp:Timer>

    
</asp:Content>
