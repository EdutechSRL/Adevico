﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="MapReorder.aspx.vb" Inherits="Comunita_OnLine.MapReorder" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Info" Src="~/Modules/ProjectManagement/UC/UC_ProjectDateInfo.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ProjectTreeItem" Src="~/Modules/ProjectManagement/UC/UC_ProjectTreeItem.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Attachment" Src="~/Modules/ProjectManagement/UC/UC_DialogProjectAttachments.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ModalPlayerHeader" Src="~/Modules/Repository/UC_New/UC_ModalPlayerHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/ProjectManagement/Css/ProjectManagement.css" rel="Stylesheet" />
    <link href="../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
    <link rel="stylesheet" href="../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css" />
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.treeTable.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.autoresize.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ui.datepicker-de.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ui.datepicker-es.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ui.datepicker-en-GB.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ui.datepicker-it.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/ProjectManagement/projectmanagement.js"></script>

    <script type="text/javascript" src="<%#ResolveUrl("~/Jscript/Modules/Common/jquery-sortable.js")%>"></script>
    <link rel="stylesheet" href="<%#ResolveUrl("~/Graphics/JQuery/Css/jquery-sortable.css")%>" />
    <asp:Literal ID="LTreorderScript" runat="server">
    <script language="javascript" type="text/javascript">
        $(function () {
            var group = $("ul.sortabletree").sortable({
                handle: ".text",

                onDrop: function (item, container, _super) {
                    var ser = "";

                    group.find("li.sortableitem").each(function () {

                        var $parent = $(this).parents("li.sortableitem").first();
                        var parentId = 0;
                        if ($parent.size() > 0) {
                            parentId = $parent.attr("id")
                        }

                        ser = ser + $(this).attr("id") + ":" + parentId + ";";
                    });
                    $('.serialize_output').first().val(ser);
                    _super(item, container);
                }
            });
        });
        
    </script>
    </asp:Literal>
    <script language="javascript" type="text/javascript">
        function onUpdating() {
            $.blockUI({ message: '<h1><%#Me.OnLoadingTranslation %></h1>' });
            return true;
        }
        $(document).ready(function () {
            $(".editable .edit.datepicker input[type='text']").datepicker("option", $.datepicker.regional["<%=LoaderCultureInfo.TwoLetterISOLanguageName%>"]);
            $(".editable .edit.datepicker input[type='text']").datepicker("option", "dateFormat", '<%=CurrentDatePickerShortDatePattern %>');
            HighlightControlToValidate();

            $('#<%=BTNsaveProjectDateInfoTop.ClientID %>').click(function () {
                return ValidateInputControls();
            });
            $('#<%=BTNsaveProjectDateInfoBottom.ClientID %>').click(function () {
                return ValidateInputControls();
            });
        });

        function ValidateInputControls() {
            var error = false;
            if (typeof (Page_Validators) != "undefined") {
                for (var i = 0; i < Page_Validators.length; i++) {
                    if (!Page_Validators[i].isvalid) {
                        error = true;
                        if ($('#' + Page_Validators[i].controltovalidate).parents(".table.taskmap td")) {
                            $('#' + Page_Validators[i].controltovalidate).parents(".table.taskmap td").first().addClass("error"); //.css("background", "#f3d74f");
                            var $editable = $('#' + Page_Validators[i].controltovalidate).parents(".table.taskmap td").first().parents(".editable").first();
                            var $edit = $editable.children(".edit");
                            var $input = $edit.find("input[type='text']");
                            $input.val($(this).html().replace("&nbsp;", ""));
                            $editable.removeClass("viewmode").addClass("editmode");
                            $editable.find("input[type='hidden']").val("edit");
                            $input.focus();

                        }
                        else {
                            $('#' + Page_Validators[i].controltovalidate).parents(".editable").first().addClass("error");
                            var $editable = $('#' + Page_Validators[i].controltovalidate).parents(".editable").first();
                            var $edit = $editable.children(".edit");
                            var $input = $edit.find("input[type='text']");
                            $input.val($(this).html().replace("&nbsp;", ""));
                            $editable.removeClass("viewmode").addClass("editmode");
                            $editable.find("input[type='hidden']").val("edit");
                            $input.focus();
                        }
                    }
                    else {
                        $('#' + Page_Validators[i].controltovalidate).parents(".table.taskmap td").first().removeClass("error"); //.css("background", "white");
                        $('#' + Page_Validators[i].controltovalidate).parents(".editable").first().removeClass("error");
                    }
                }
            }

            if (!error) {
                return onUpdating();
            }
            else {
                return false;
            }

        }

        function HighlightControlToValidate() {
            if (typeof (Page_Validators) != "undefined") {
                for (var i = 0; i < Page_Validators.length; i++) {
                    $('#' + Page_Validators[i].controltovalidate).blur(function () {
                        var validatorctrl = getValidatorUsingControl($(this).attr("ID"));
                        if (validatorctrl != null && !validatorctrl.isvalid) {
                            $(this).parents(".table.taskmap td").first().addClass("error"); //.css("background", "#f3d74f");
                            $(this).parents(".editable").first().addClass("error");
                        }
                        else {
                            $(this).parents(".table.taskmap td").first().removeClass("error"); //.css("background", "white");
                            $(this).parents(".editable").first().removeClass("error");
                        }
                    });
                }
            }
        }

        function getValidatorUsingControl(controltovalidate) {
            var length = Page_Validators.length;
            for (var j = 0; j < length; j++) {
                if (Page_Validators[j].controltovalidate == controltovalidate) {
                    return Page_Validators[j];
                }
            }
            return null;
        }

        $(document).ready(function () {
            $(".view-modal.view-dlgsaveorder").dialog({
                appendTo: "form",
                closeOnEscape: false,
                modal: true,
                width: 600,
                height: 450,
                minHeight: 200,
                minWidth: 200,
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                    $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                }
            });
        });
    </script>
     <CTRL:ModalPlayerHeader ID="CTRLmodalPlayerHeader" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView ID="MLVprojectMap" runat="server" ActiveViewIndex="1">
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
        <asp:View ID="VIWmap" runat="server">
            <div class="contentwrapper">
                <div class="DivEpButton DivEpButtonTop">
                    <asp:Button ID="BTNsaveProjectDateInfoTop" runat="server" Text="*Save" CssClass="linkMenu savesubmit" CausesValidation="true" Visible="false" />
                    <asp:Button ID="BTNsaveProjectReorderTop" runat="server" Text="*Apply" CssClass="linkMenu" CausesValidation="false" OnClientClick="return onUpdating();" Visible="false" />
                    <asp:HyperLink ID="HYPgoToProjectEditTop" class="linkMenu" runat="server" Text="*Edit project" Visible="false"></asp:HyperLink>
                    <asp:HyperLink ID="HYPbackToResourceDashboardTop" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                    <asp:HyperLink ID="HYPbackToManagerDashboardTop" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                    <asp:HyperLink ID="HYPbackToProjectsTop" class="linkMenu" runat="server" Text="*Back" Visible="false"></asp:HyperLink>
                </div>
                <div class="fieldobject header">
                    <div class="fieldrow title clearfix">
                        <div class="left">
                            <h2><span class="projectnametitle">
                                <span><asp:Literal id="LTprojectName" runat="server"></asp:Literal></span>
                                <span class="icons">
                                    <asp:Label ID="LBattachments" runat="server" Visible="false" CssClass="icon xs attacchment">&nbsp;</asp:Label>
                                </span>
                            </span></h2>
                        </div>
                        <div class="right">
                            <span class="extra"></span>
                        </div>
                    </div>
                </div>
                <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
                <div class="messages" id="DVreorderMessage" runat="server" visible="false">
                    <div class="message actionrequired">
                        <div class="fieldobject">
                            <div class="fieldrow description">
                                <div class="description">
                                    <asp:literal ID="LTreorderMessageDescription" runat="server">*</asp:literal>
                                </div>
                            </div>
                            <div class="fieldrow">
                                <div class="fieldoptions">
                                   <asp:RadioButtonList ID="RBLreorderOptions" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">

                                   </asp:RadioButtonList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <CTRL:Info ID="CTRLprojectInfo" runat="server"></CTRL:Info>
                <div class="fieldobject toolbar clearfix">
                    <div class="fieldrow left">
                        <label><asp:Literal ID="LTmapReorderIstructions" runat="server">*Drag and drop to arrange</asp:Literal></label>
                    </div>
                    <div class="fieldrow right">
                        <span class="btnswitchgroup small"><!--
                        --><asp:HyperLink ID="HYPprojectMap" runat="server" CssClass="btnswitch first">*List view</asp:HyperLink><!--
                        --><asp:HyperLink ID="HYPprojectMapReorder" runat="server" CssClass="btnswitch active">*Tree view</asp:HyperLink><!--
                        --><asp:HyperLink ID="HYPprojectGantt" runat="server" CssClass="btnswitch last">*Gantt</asp:HyperLink><!--
                        --><asp:HyperLink ID="HYPprojectMapBulkEdit" runat="server" CssClass="btnswitch last" Visible="false">*Bulk Edit</asp:HyperLink><!--
                    --></span>
                    </div>
                </div>
                <div class="sortabletree header clearfix" id="DVsortableTreeHeader" runat="server">
                    <span class="text">
                        <asp:Label ID="LBthName" CssClass="name fakecell first" runat="server">*Task Name</asp:Label>
                        <span class="details">
                            <asp:Label ID="LBthDuration" CssClass="duration fakecell" runat="server">*Duration</asp:Label>
                            <asp:Label ID="LBthPredecessors" CssClass="links fakecell" runat="server">*Links</asp:Label>
                            <asp:Label ID="LBthStartDate" CssClass="date start fakecell" runat="server">*Start date</asp:Label>
                            <asp:Label ID="LBthEndDate" CssClass="date end fakecell last" runat="server">*End date</asp:Label>
                        </span>
                    </span>
                </div>
                <div class="sortabletree wrapper" id="DVsortableTree" runat="server">
                    <asp:Repeater ID="RPTprojectTree" runat="server">
			            <HeaderTemplate>
				            <ul class="sortabletree categories root">        
			            </HeaderTemplate>
			            <ItemTemplate>
                            <CTRL:ProjectTreeItem ID="CTRLchild" runat="server" />
			            </ItemTemplate>
			            <FooterTemplate>
				             </ul>     
			            </FooterTemplate>
		            </asp:Repeater>
                </div>
                <div class="DivEpButton DivEpButtonBottom" runat="server" visible="false" id="DVcommandsBottom">
                    <asp:Button ID="BTNsaveProjectDateInfoBottom" runat="server" Text="*Save" CssClass="linkMenu" OnClientClick="return onUpdating();" Visible="false" />
                    <asp:Button ID="BTNsaveProjectReorderBottom" runat="server" Text="*Apply" CssClass="linkMenu" CausesValidation="false" OnClientClick="return onUpdating();" Visible="false"/>
                    <asp:HyperLink ID="HYPgoToProjectEditBottom" class="linkMenu" runat="server" Text="*Edit project" Visible="false"></asp:HyperLink>
                    <asp:HyperLink ID="HYPbackToResourceDashboardBottom" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                    <asp:HyperLink ID="HYPbackToManagerDashboardBottom" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                    <asp:HyperLink ID="HYPbackToProjectsBottom" class="linkMenu" runat="server" Text="*Back" Visible="false"></asp:HyperLink>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
    <div class="dialog dlgsaveorder view-modal view-dlgsaveorder" title="*Confirm action" runat="server" visible="false" id="DVconfirmReorder">
        <div class="fieldobject">
            <div class="fieldrow description">
                <asp:literal ID="LTconfirmReorderAction" runat="server"></asp:literal>
            </div>
            <asp:Repeater ID="RPTconfirmOptions" runat="server">
                <ItemTemplate>
                    <div class="fieldrow">
                        <input type="radio" name="RDconfirmAction" value="<%#Cint(Container.DataItem.Action) %>" <%#IsSelected(Container.DataItem) %> />
                        <label for=""><asp:Literal id="LTconfirmOption" runat="server"></asp:Literal></label>
                        <asp:Label ID="LBconfirmOptionDescription" runat="server" CssClass="description"></asp:Label>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <div class="fieldrow buttons right">
                <asp:Button ID="BTNconfirmReorderAction" runat="server" CssClass="linkMenu" Text="*Apply"/>
                <asp:LinkButton ID="LNBcloseConfirmReorderAction" runat="server" CssClass="linkMenu close" Text="*Cancel" OnClientClick="return false;" CausesValidation="false" />
            </div>
        </div>
    </div>
    <input type="hidden" id="HDMserializeTasks" runat="server" class="serialize_output" />
    <CTRL:Attachment id="CTRLattachment" runat="server" CssClass="dlgprojectattachments" Visible="false"></CTRL:Attachment>
</asp:Content>