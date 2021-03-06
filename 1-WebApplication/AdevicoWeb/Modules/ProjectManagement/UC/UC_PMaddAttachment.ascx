﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_PMaddAttachment.ascx.vb" Inherits="Comunita_OnLine.UC_PMaddAttachment" %>
<%@ Register TagPrefix="CTRL" TagName="SelectActivity" Src="~/Modules/ProjectManagement/UC/UC_ActivitySelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="RepositoryItemsUploader" Src="~/Modules/Repository/Common/UC_ModuleRepositoryUploader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="InternalFilesUploader" Src="~/Modules/Repository/Common/UC_ModuleInternalUploader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="LinkRepositoryItems" Src="~/Modules/Repository/Common/UC_ModuleInternalLink.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLurls" Src="~/Modules/Common/UC/UC_AddUrlItems.ascx"  %>
<div class="dialog <%=UploaderCssClass %>">
    <div class="fieldobject intro" id="DVdescription" runat="server">
        <div class="fieldrow">
            <div class="description">
                <asp:Literal ID="LTdescription" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
    <div class="fieldobject attachmentinput">
        <CTRL:CTRLurls id="CTRLurls" runat="server" MaxItems="5" Visible="false" />
        <CTRL:InternalFilesUploader id="CTRLinternalUploader" runat="server" MaxFileInput="5" MaxItems="10" Visible="false" AjaxEnabled="false" PostBackTriggers="BTNaddAttachment" />
        <CTRL:LinkRepositoryItems id="CTRLlinkItems" runat="server"  Visible="false"  MaxSelectorWidth="900px" TreeSelect="cascadeselect" RemoveEmptyFolders="true" FolderSelectable="false"/>
        <CTRL:RepositoryItemsUploader id="CTRLrepositoryItemsUploader" runat="server" MaxFileInput="5" MaxItems="10" Visible="false" AjaxEnabled="false" PostBackTriggers="BTNaddAttachment" DisplayErrorInline="true" />
    </div>
    <div class="fieldobject commands" id="DVcommands" runat="server">
        <div class="fieldrow buttons right">
            <asp:LinkButton id="LNBcloseAttachmentWindow" runat="server" CssClass="linkMenu close"></asp:LinkButton>
            <asp:Button id="BTNaddAttachment" runat="server" Text="Add" OnClientClick="return submitUploadWindow();" />
        </div>
    </div>
</div>