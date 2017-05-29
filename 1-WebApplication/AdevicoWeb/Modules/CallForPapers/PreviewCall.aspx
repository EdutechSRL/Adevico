﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ExternalService.Master" CodeBehind="PreviewCall.aspx.vb" Inherits="Comunita_OnLine.PreviewCall" %>
<%@ MasterType VirtualPath="~/ExternalService.Master" %>
<%@ Register Src="~/Modules/CallForPapers/UC/UC_InputField.ascx" TagName="CTRLInputField" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/CallForPapers/UC/UC_InputRequiredFile.ascx" TagName="CTRLrequiredFile" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayItem" Src="~/Modules/Repository/Common/UC_ModuleRenderAction.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ModalPlayerHeader" Src="~/Modules/Repository/UC_New/UC_ModalPlayerHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header ID="CTRLheader" runat="server"/>
     <script type="text/javascript">
         $(function () {
             $(".fieldobject.checkboxlist").each(function () {
                 if ($(this).find(".extraoption").size() > 0) {
                     var $extraoption = $(this).find(".extraoption");
                     var $textoption = $(this).find(".textoption");

                     $extraoption.next("label").after($textoption);
                     if ($extraoption) {
                         if ($extraoption.is(":checked")) {
                             $textoption.find("input").attr("disabled", false);
                             $textoption.removeClass("disabled");
                         } else {
                             $textoption.find("input").attr("disabled", true);
                             $textoption.addClass("disabled");
                         }
                     }
                 }
             });
             $(".fieldobject.radiobuttonlist").each(function () {
                 if ($(this).find(".extraoption").size() > 0) {
                     var $extraoption = $(this).find(".extraoption input[type='radio']");
                     var $textoption = $(this).find(".textoption");

                     $extraoption.next("label").after($textoption);
                     if ($(this).find("input[type='radio']")) {
                         if ($(this).is(":checked")) {
                             $textoption.find("input").attr("disabled", false);
                             $textoption.removeClass("disabled");
                         } else {
                             $textoption.find("input").attr("disabled", true);
                             $textoption.addClass("disabled");
                         }
                     }
                 }
             });
             $(".fieldobject.radiobuttonlist input[type='radio']").change(function () {

                 if ($(this).parents("span.extraoption").first().size() > 0) {
                     var $textoption = $(this).parents(".radiobuttonlist").first().find(".textoption");
                     $textoption.find("input").attr("disabled", false);
                     $textoption.removeClass("disabled");
                 } else {
                     var $textoption = $(this).parents(".radiobuttonlist").first().find(".textoption");
                     $textoption.find("input").attr("disabled", true);
                     $textoption.addClass("disabled");
                 }
             });

             $(".fieldobject.checkboxlist input[type='checkbox']").change(function () {

                 if ($(this).is(".extraoption")) {
                     var ischecked = $(this).is(":checked");
                     var $textoption = $(this).parents(".checkboxlist").first().find(".textoption");
                     $textoption.find("input").attr("disabled", !ischecked);
                     $textoption.toggleClass("disabled");
                 }
             });
         });

         $(function () {
             $(".fieldinput input").add(".fieldinput textarea.textarea").focus(function () {
                 $(this).siblings(".inlinetooltip").addClass("visible");
             });

             $(".fieldobject.date .fieldinput input")
                .add(".fieldobject.datetime .fieldinput input")
                .add(".fieldobject.time .fieldinput input").focus(function () {
                 $(this).parents(".fieldobject").find(".inlinetooltip").addClass("visible");
             });
             $(".fieldobject.date .fieldinput input")
                .add(".fieldobject.datetime .fieldinput input")
                .add(".fieldobject.time .fieldinput input").blur(function () {
                 $(this).parents(".fieldobject").find(".inlinetooltip").removeClass("visible");
             });

             $(".fieldinput input[type=file]").hover(function () {
                 $(this).siblings(".inlinetooltip").addClass("visible");
             },
            function () {
                $(this).siblings(".inlinetooltip").removeClass("visible");
            });

             $(".fieldinput input").add(".fieldinput textarea.textarea").blur(function () {
                 $(this).siblings(".inlinetooltip").removeClass("visible");
             });

             $("fieldset.section legend").click(function () {
                 var $legend = $(this);
                 var $fieldset = $legend.parent();
                 var $children = $fieldset.children().not("legend");
                 $children.toggle();
                 $fieldset.toggleClass("collapsed");
             });

             /*$(".fieldobject.checkboxlist").checkboxList({
             listSelector: "span.checkboxlist",
             errorSelector: ".fieldrow.fieldinput label, .fieldinfo",
             checkOnStart: true,
             applySelective: false,
             skipWriteMinMax: true,
             error: {
             min: ".minmax .min",
             max: ".minmax .max"
             }
             })*/
             //$(".fieldrow.fieldinput .checkboxlist")

             //attivare dopo aver importato il link al file jquery.checkboxList.js nelle pagine del CallForPaper
             $(".fieldobject.checkboxlist").checkboxList({
                 listSelector: "span.inputcheckboxlist",
                 errorSelector: ".fieldrow.fieldinput label",
                 checkOnStart: true,
                 error: {
                     min: ".minmax .min",
                     max: ".minmax .max"
                 }
             });

             $(".fieldobject.disclaimer.custom").checkboxList({
                 listSelector: "span.inputcheckboxlist",
                 errorSelector: "self",
                 checkOnStart: true,
                 error: {
                     min: ".minmax .min",
                     max: ".minmax .max"
                 }
             });
             //jquery.textVal.js

             $(".fieldobject.singleline .fieldrow.fieldinput").textVal({
                 textSelector: "input.inputtext",
                 charAvailable: ".fieldinfo .maxchar .availableitems",
                 errorSelector: ".fieldrow.fieldinput label, .fieldinfo",
                 charMax: ".fieldinfo .maxchar .totalitems"

             });

             $(".fieldobject.multiline .fieldrow.fieldinput").textVal({
                 textSelector: "textarea.textarea",
                 charAvailable: ".fieldinfo .maxchar .availableitems",
                 errorSelector: ".fieldrow.fieldinput label, .fieldinfo",
                 charMax: ".fieldinfo .maxchar .totalitems"
             });
         });
    </script>
    <CTRL:ModalPlayerHeader ID="CTRLmodalPlayerHeader" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView ID="MLVpreview" runat="server">
        <asp:View ID="VIWempty" runat="server">
            <br /><br /><br /><br />
            <asp:Label ID="LBpreviewMessage" runat="server"></asp:Label>
            <br /><br /><br /><br />
        </asp:View>
        <asp:View ID="VIWcall" runat="server">
            <div class="view preview">
                <div class="DivEpButton">
                    <asp:HyperLink ID="HYPlist" runat="server" Text="Lista bandi" CssClass="Link_Menu closepopup" Enabled="true" ></asp:HyperLink>
                    <asp:Button ID="BTNvirtualDeleteSubmission" runat="server" Text="Cancella domanda" Enabled="false" Visible="false"/>
                    <asp:Button ID="BTNsaveSubmissionTop" runat="server" Text="Salva"  Enabled="false" Visible="false"/>
                    <asp:Button ID="BTNsubmitSubmissionTop" runat="server" Text="Sottometti definitivamente" Enabled="false" Visible="false"/>
                </div>
                <div class="persist-area1">
                <div class="topbar persist-header">
                <div class="innerwrapper clearfix" id="DVsubmittersLoader" runat="server" visible="false">
                    <div class="left">
                        <span class="partecipantselect">
                            <asp:Label ID="LBsubmitterSelector_t" runat="server" AssociatedControlID="DDLsubmitters"></asp:Label>
                            <asp:DropDownList ID="DDLsubmitters" runat="server" AutoPostBack="true"></asp:DropDownList>
                        </span>
                    </div>
                </div>
                </div>
                <fieldset class="section collapsable cfpintro">
                    <legend>
                    <span class="switchsection handle">&nbsp;</span>
                    <span class="title">
                        <asp:Label id="LBcallDescriptionTitle" runat="server"></asp:Label>
                    </span>
                    </legend>
                    <div class="cfpdescription">
                        <div class="renderedtext"><asp:Literal id="LTcallDescription" runat="server"/></div>
                    </div>
                    <div class="cfpdetails">
                        <span class="expiration">
                            <asp:Label ID="LBtimeValidity_t" runat="server">Validità</asp:Label>
                            <asp:Label ID="LBstartDate" CssClass="startdate" runat="server"></asp:Label>&nbsp;-&nbsp;
                            <asp:Label ID="LBendDate" CssClass="enddate" runat="server"></asp:Label>
                        </span>
                        <asp:Label runat="server" ID="LBwinnerinfo" class="winnerinfo" Visible="false"></asp:Label>
                    </div>
                </fieldset>
                <fieldset class="section partecipants" runat="server" id ="FLDpartecipants" visible="false">
                    <legend>
                    <span class="switchsection handle">&nbsp;</span>
                    <span class="title">
                        <asp:Literal id="LTsubmitterTypesTitle" runat="server"></asp:Literal>
                    </span>
                    </legend>
                    <div class="sectiondescription">
		                <div class="messages">
			                <div class="message alert">
				               <asp:Literal ID="LTpreviewSubmittersMessage" runat="server"></asp:Literal>
			                </div>
		                </div>
	                </div>
                    <div class="fieldrow">
                        <asp:RadioButtonList ID="RBLsubmitters" Enabled="false" runat="server" CssClass="rbldl" RepeatDirection="Vertical" RepeatLayout="Flow"></asp:RadioButtonList>
                    </div>
                </fieldset>
                <asp:Repeater id="RPTattachments" runat="server">
                    <HeaderTemplate>
                        <fieldset class="section collapsable attachments">
                            <legend>
                            <span class="switchsection handle">&nbsp;</span>
                            <span class="title">
                                <asp:Literal id="LTattachmentsTitle" runat="server"></asp:Literal>
                            </span>
                            </legend>
                            <div class="fieldobject">
                                <div class="fieldrow">
                                    <ul class="attachedfiles">
                    </HeaderTemplate>
                    <ItemTemplate>
                                        <li class="attachedfile">
                                            <CTRL:DisplayItem ID="CTRLdisplayItem" runat="server" EnableAnchor="true" DisplayExtraInfo="false" DisplayLinkedBy="false"  />
                                            <div class="cfpdescription" runat="server" id="DVdescription" visible="false">
                                                <asp:Label ID="LBattachmentDescription" runat="server"></asp:Label>
                                            </div>
                                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                                    </ul>
                                </div>
                            </div>
                         </fieldset>
                    </FooterTemplate>
                </asp:Repeater>
                <asp:Repeater ID="RPTsections" runat="server">
                    <ItemTemplate>
                        <fieldset class="section collapsable">
                            <legend>
                            <span class="switchsection handle">&nbsp;</span>
                            <span class="title">
                                <asp:Literal id="LTsectionTitle" runat="server"></asp:Literal>
                            </span>
                            </legend>
                            <div class="sectiondescription">
                                <asp:Literal ID="LTsectionDescription" runat="server"></asp:Literal>
                            </div>
                            <asp:Repeater ID="RPTfields" runat="server" DataSource="<%#Container.DataItem.Fields%>" OnItemDataBound="RPTfields_ItemDataBound">
                                <ItemTemplate>
                                    <CTRL:CTRLInputField ID="CTRLinputField" runat="server" />
                                </ItemTemplate>
                            </asp:Repeater>
                        </fieldset>
                    </ItemTemplate>   
                </asp:Repeater>
                 <asp:Repeater ID="RPTrequiredFiles" runat="server">
                    <HeaderTemplate>
                        <fieldset class="section collapsable">
                            <legend>
                            <span class="switchsection handle">&nbsp;</span>
                            <span class="title">
                                <asp:Literal id="LTrequiredFilesTitle" runat="server"></asp:Literal>
                            </span>
                            </legend>
                            <div class="sectiondescription">
                                <asp:Literal ID="LTrequiredFilesDescription" runat="server"></asp:Literal>
                            </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <CTRL:CTRLrequiredFile ID="CTRLrequiredFile" runat="server" />
                    </ItemTemplate>   
                    <FooterTemplate>
                       
                        </fieldset>
                    </FooterTemplate>
                </asp:Repeater>
                <div class="CFPBoxes" id="DVbottomBox" runat="server" visible="false">
                    <div class="CFPBox boxsave left">
                         <asp:Button ID="BTNsaveSubmissionBottom" runat="server" Text="Salva" Enabled="false" />

                        <div class="cfpdescription">
                            <asp:Literal ID="LTsaveBottomExplanation" runat="server">
                            Salva la domanda e potrai modificarla in un secondo momento. La domanda non è
                            sottomessa (e quindi valida) fino a quando non verrà premuto sottometti definitivamente</asp:Literal>
                        </div>
                    </div>
                    <div class="CFPBox boxsubmit right">
                        <asp:Button ID="BTNsubmitSubmissionBottom" runat="server" Text="Sottometti definitivamente"  Enabled="false" />
                        <div class="cfpdescription">
                            <asp:Literal ID="LTsaveAndSubmitBottomExplanation" runat="server">Sottometti definitivamente la domanda affinchè venga valutata. Non sarà più possibile
                            modificare la domanda</asp:Literal>
                        </div>
                    </div>
                </div>
            </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>