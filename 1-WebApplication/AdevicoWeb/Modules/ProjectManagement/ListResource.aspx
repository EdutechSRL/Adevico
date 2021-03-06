﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ListResource.aspx.vb" Inherits="Comunita_OnLine.ListResource" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="TopControls" Src="~/Modules/ProjectManagement/UC/List/UC_DasboardListTopControl.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="List" Src="~/Modules/ProjectManagement/UC/List/UC_ProjectListPlain.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Tree" Src="~/Modules/ProjectManagement/UC/List/UC_ProjectListTree.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/ProjectManagement/UC/UC_ProjectManagementHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ModalPlayerHeader" Src="~/Modules/Repository/UC_New/UC_ModalPlayerHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header id="CTRLheader" runat="server" LoadProgressBarHeader="true" LoadTaskHeader="false"></CTRL:Header>
    <CTRL:ModalPlayerHeader ID="CTRLmodalPlayerHeader" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView ID="MLVprojectList" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWempty" runat="server">
            <br />
            <br />
            <br />
            <br />
            <asp:Label ID="LBemptyMessage" runat="server"></asp:Label>
            <br />
            <br />
            <br />
            <br />
        </asp:View>
        <asp:View ID="VIWcontent" runat="server">
            <div class="DivEpButton DivEpButtonTop">
                <div class="ddbuttonlist enabled" id="DVnewProjectTop" runat="server" visible="false"><!--
                    --><asp:HyperLink ID="HYPnewProjectTop" runat="server" Text="*New project" CssClass="linkMenu ddbutton"/><!--
                    --><asp:HyperLink ID="HYPnewPersonalProjectTop" runat="server" Text="*New personal" CssClass="linkMenu ddbutton"/><!--
                --></div>
                <asp:HyperLink ID="HYPmanagerDashboardTop" class="linkMenu" runat="server" Text="*Dashboard" Visible="false"></asp:HyperLink>
            </div>
            <CTRL:TopControls id="CTRLtopControls" runat="server"></CTRL:TopControls>
            <CTRL:List id="CTRLlistPlain" runat="server" Visible="false"></CTRL:List>
            <CTRL:Tree id="CTRLlistTree" runat="server" Visible="false"></CTRL:Tree>
            <div class="DivEpButton DivEpButtonBottom" runat="server" visible="false" id="DVcommandsBottom">
                <div class="ddbuttonlist enabled" id="DVnewProjectBottom" runat="server" visible="false"><!--
                    --><asp:HyperLink ID="HYPnewProjectBottom" runat="server" Text="*New project" CssClass="linkMenu ddbutton"/><!--
                    --><asp:HyperLink ID="HYPnewPersonalProjectBottom" runat="server" Text="*New personal" CssClass="linkMenu ddbutton"/><!--
                --></div>
                <asp:HyperLink ID="HYPmanagerDashboardBottom" class="linkMenu" runat="server" Text="*Dashboard" Visible="false"></asp:HyperLink>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>