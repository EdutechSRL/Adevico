<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="Submission.aspx.vb" Inherits="Comunita_OnLine.Submission" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/Modules/CallForPapers/UC/UC_InputRequiredFile.ascx" TagName="CTRLrequiredFile" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/CallForPapers/UC/UC_RenderField.ascx" TagName="CTRLrenderField" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLreport" Src="~/Modules/CallForPapers/UC/UC_SubmissionExport.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayItem" Src="~/Modules/Repository/Common/UC_ModuleRenderAction.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ModalPlayerHeader" Src="~/Modules/Repository/UC_New/UC_ModalPlayerHeader.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header ID="CTRLheader" runat="server" EnableScripts="true" />
    <script type="text/javascript">
        <% = me.CTRLreport.GetControlScript(HDNdownloadTokenValue.ClientID) %>
    </script>
    <script type="text/javascript">
        $(function () {
            $(".fieldobject.checkboxlist").each(function () {
                if ($(this).find(".extraoption").size() > 0) {
                    var $extraoption = $(this).find(".extraoption");
                    var $textoption = $(this).find(".textoption");
                    $extraoption.next("label").after($textoption);
                    if ($extraoption) {
                        if ($extraoption.is(":checked") || ($textoption.find(".extraoption input[type='checkbox']") && $extraoption.find(".extraoption input[type='checkbox']").is(":checked"))) {
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
            $("fieldset.section.collapsed").each(function () {
                var $fieldset = $(this);
                var $legend = $fieldset.children().filter("legend");
                var $children = $fieldset.children().not("legend");
                $children.toggle();
            });

            $("fieldset.section.collapsable legend").click(function () {
                var $legend = $(this);
                var $fieldset = $legend.parent();
                var $children = $fieldset.children().not("legend");
                $children.toggle();
                $fieldset.toggleClass("collapsed");
            });

            $(".persist-area").semiFixed()
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
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="contentwrapper edit clearfix">
        <div class="view compiled">
            <div class="persist-area">
                <div class="topbar persist-header" id="DVtopMenu" runat="server" visible="false">
                    <div class="innerwrapper clearfix">
                        <div class="left">
                            <ul class="sumbmissiondetails">
                                <li class="submitter">
                                    <asp:Literal ID="LTowner_t" runat="server"></asp:Literal>&nbsp;<asp:Label ID="LBowner"
                                        runat="server" /></li>
                                <li class="submittertype">
                                    <asp:Literal ID="LTsubmitterType_t" runat="server"></asp:Literal>&nbsp;<asp:Label
                                        ID="LBsubmitterType" runat="server" /></li>
                                <li class="status">
                                    <asp:Literal ID="LTsubmissionStatus_t" runat="server"></asp:Literal>&nbsp;<asp:Label
                                        ID="LBsubmissionStatus" runat="server"></asp:Label></li>
                                <li class="submissiondate" id="LIsubmissionInfo" runat="server" visible="false">
                                    <asp:Literal ID="LTsubmittedOn_t" runat="server"></asp:Literal>&nbsp;
                                    <asp:Label ID="LBsubmittedOnData" runat="server" CssClass="date" />&nbsp;<asp:Label
                                        ID="LBsubmittedOnTime" runat="server" CssClass="time" />
                                    <span class="submittedby" runat="server" id="SPNsubmittedBy">&nbsp;<asp:Literal ID="LTsubmittedBy_t"
                                        runat="server"></asp:Literal>&nbsp;
                                        <asp:Label ID="LBsubmittedBy" runat="server" />
                                    </span></li>
                                <li class="submissionrevisions" id="LIrevisions" runat="server" visible="false">
                                    <asp:Literal ID="LTrevisionListTitle_t" runat="server">Revisions:</asp:Literal>&nbsp;
                                    <asp:DropDownList ID="DDLrevisions" runat="server" CssClass="revisionhistory" AutoPostBack="true">
                                    </asp:DropDownList>
                                </li>
                            </ul>
                        </div>
                        <div class="right">
                            <asp:HyperLink ID="HYPlist" runat="server" Text="Lista bandi" CssClass="Link_Menu"
                                Visible="false"></asp:HyperLink>
                            <asp:HyperLink ID="HYPsubmissionsList" runat="server" Text="Gestione bandi" CssClass="Link_Menu"
                                Visible="false"></asp:HyperLink>
                            <span class="icons large">
                                <CTRL:CTRLreport ID="CTRLreport" runat="server" isContainer="false" />
                                <span class="icon separator" id="SPNmanage" runat="server">&nbsp;</span>
                                <asp:Button ID="BTNaccept" CssClass="icon accept" runat="server" CommandName="accept"
                                    Visible="false" ToolTip="Accept" />
                                <asp:Button ID="BTNrefuse" CssClass="icon refuse" runat="server" CommandName="refuse"
                                    Visible="false" ToolTip="Refuse" />
                                <asp:Button ID="BTNsubmitForUser" CssClass="icon submitFor" runat="server" CommandName="submit"
                                    Visible="false" ToolTip="Submit for user" />
                                <asp:Button ID="BTNreview" CssClass="icon requestreview" runat="server" CommandName="review" Visible="false" ToolTip="Review" />
                            </span>
                        </div>
                    </div>
                    <div class="revisionsettings innerwrapper clearfix" runat="server" visible="false" id="DVrevision">
                        <div class="fieldobject multiline">
                            <div class="fieldrow fieldinput" id="DVdeadline" runat="server" visible="false">
                                <asp:Label ID="LBdeadline_t" runat="server" AssociatedControlID="RDPdeadline">Entro il:</asp:Label>
                                <telerik:raddatetimepicker id="RDPdeadline" runat="server">
                                </telerik:raddatetimepicker>
                            </div>
                            <div class="fieldrow fieldinput">
                                <asp:Label ID="LBrequestReason_t" runat="server" AssociatedControlID="TXBreason">Motivazione:</asp:Label>
                                <asp:TextBox runat="server" ID="TXBreason" TextMode="multiline"
                                    CssClass="textarea"></asp:TextBox>
                                <asp:Label runat="server" ID="LBreasonHelp" CssClass="inlinetooltip"></asp:Label>
                                <br />
                                <span class="fieldinfo ">
                                    <span class="maxchar" runat="server">
                                        <asp:Literal ID="LTmaxCharsrequest" runat="server"></asp:Literal>
                                        <span class="availableitems">{available}</span>/<span class="totalitems">{total}</span>
                                    </span>
                                </span>
                            </div>
                            <div class="fieldrow fieldinput">
                                <asp:Button ID="BTNaddRequest" runat="server" CommandName="addRequest"
                                    Text="Add" CssClass="Link_Menu" />
                                <asp:Button ID="BTNundoRequest" runat="server" CommandName="cancelRequest" 
                                    Text="Undo" CssClass="Link_Menu"/>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="DVmessages" class="messages" runat="server" visible="false">
                    <asp:MultiView ID="MLVpendingMessage" runat="server" ActiveViewIndex="0">
                        <asp:View runat="server" ID="VIWpendingEmpty">
                            
                        </asp:View>
                        <asp:View runat="server" ID="VIWpendingUser">
                            <div class="message alert">
                                <div class="revisionalert clearfix">
                                    <asp:Label ID="LBrequiredRevision_t" CssClass="revisionrequested" runat="server">
                                        La tua richiesta di Revisione non è stata ancora accettata. Vuoi annullare?
                                    </asp:Label>
                                    <div class="DivEpButton big">
                                        <asp:HyperLink ID="HTPreviewUserSubmission" runat="server" Visible="false" CssClass="Link_Menu">R</asp:HyperLink>
                                        <asp:HyperLink ID="HTPviewUserRequest" runat="server" Visible="false" CssClass="Link_Menu">V</asp:HyperLink>
                                        <asp:Button ID="BTNcancelUserRequest" runat="server" CssClass="Link_Menu" Visible="false"/>
                                        <asp:Button ID="BTNrefuseUserRequest" runat="server" CssClass="Link_Menu"  Visible="false"/>
                                        <asp:Button ID="BTNacceptUserRequest" runat="server" CssClass="Link_Menu" Visible="false"/>
                                    </div>
                                </div>
                            </div>
                            <br />
                        </asp:View>
                        <asp:View runat="server" ID="VIWpendingManager">
                            <div class="message alert">
                                <div class="revisionalert clearfix">
                                    <span class="revisionneeded">
                                        <asp:Literal ID="LTrevisionRequired_t" runat="server">E' stata richiesta una revisione</asp:Literal>
                                        
                                        <span class="revisionapplicant">
                                            <asp:Literal ID="LTrevisionRequiredBy_t" runat="server">da</asp:Literal>
                                            <asp:label ID="LBrevisionRequiredBy" runat="server" CssClass="name"></asp:label>
                                        </span>
                                    </span>
                                    <asp:Label ID="LBrevisionMessage" runat="server" cssclass="revisionmsg clearfix"></asp:Label>
                                    <span class="revisiondate">
                                        <span class="revisiondeadline">
                                            <asp:Literal ID="LTdeadline_t" runat="server">entro il</asp:Literal>
                                            <asp:Label ID="LBdeadlineDate" runat="server" CssClass="date"></asp:Label>
                                        </span>
                                    </span>
                                    <div class="DivEpButton big">
                                        <asp:HyperLink ID="HTPreviewManagerSubmission" runat="server" CssClass="Link_Menu" Visible="false">R</asp:HyperLink>
                                        <asp:Button ID="BTNcancelManagerReview" runat="server" CssClass="Link_Menu" Visible="false"/>
                                    </div>
                                </div>
                            </div>
                            <br />
                        </asp:View>
                    </asp:MultiView>
                    <asp:MultiView ID="MLVrevisionInfo" runat="server" ActiveViewIndex="0">
                        <asp:View runat="server" ID="VIWinfoEmpty">
                            
                        </asp:View>
                        <asp:View runat="server" ID="VIWinfo">
                            <div class="message info">
                                <div class="revisionalert clearfix">
                                    <span class="revisionneeded">
                                        <asp:Literal ID="LTrevisionRequiredInfo_t" runat="server">E' stata richiesta una revisione</asp:Literal>
                                        
                                        <span class="revisionapplicant">
                                            <asp:Literal ID="LTrevisionRequiredByInfo_t" runat="server">da</asp:Literal>
                                            <asp:label ID="LBrevisionRequiredByInfo" runat="server" CssClass="name"></asp:label>
                                        </span>
                                    </span>
                                    <asp:Label ID="LBrevisionMessageInfo" runat="server" cssclass="revisionmsg clearfix"></asp:Label>
                                    <span class="revisionconfirm">
                                        <span class="revisionmessage">
                                            <asp:Literal ID="LTrevisionStatus" runat="server"></asp:Literal>
                                            <span class="revisionapplicant">
                                                <asp:Literal ID="LTrevisionManagedByInfo_t" runat="server">da</asp:Literal>
                                                <asp:label ID="LBrevisionManagedByInfo" runat="server" CssClass="name"></asp:label>
                                            </span>
                                        </span>
                                        <span class="revisiondate">
                                            <asp:Literal ID="LTrevisionDate_t" runat="server">il</asp:Literal>
                                            <asp:Label ID="LBrevisionDate" runat="server" cssclass="date"></asp:Label>
                                        </span>
                                    </span>
                                </div>
                            </div>
                            <br />
                        </asp:View>
                    </asp:MultiView>
                </div>
                <asp:MultiView ID="MLVpreview" runat="server">
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
                    <asp:View ID="VIWcall" runat="server">
                        <fieldset class="section collapsable cfpintro collapsed" runat="server" id="FLDcallInfo" visible="false">
                            <legend>
                                <span class="switchsection handle">&nbsp;</span>
                                <span class="title">
                                    <asp:Label ID="LBcallDescriptionTitle" runat="server"></asp:Label>
                                </span>
                                </legend>
                            <div class="cfpdescription">
                                <div class="renderedtext"><asp:Literal ID="LTcallDescription" runat="server" /></div>
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
                        <asp:Repeater ID="RPTattachments" runat="server">
                            <HeaderTemplate>
                                <fieldset class="section collapsable attachments collapsed">
                                    <legend>
                                        <span class="switchsection handle">&nbsp;</span>
                                        <span class="title">
                                            <asp:Literal ID="LTattachmentsTitle" runat="server"></asp:Literal>
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
                                </ul> </div> </div> </fieldset>
                            </FooterTemplate>
                        </asp:Repeater>
                        <asp:Repeater ID="RPTsections" runat="server">
                            <ItemTemplate>
                                <fieldset class="section collapsable">
                                    <legend>
                                        <span class="switchsection handle">&nbsp;</span>
                                        <span class="title">
                                            <asp:Literal ID="LTsectionTitle" runat="server"></asp:Literal>
                                        </span>
                                        </legend>
                                    <div class="sectiondescription">
                                        <asp:Literal ID="LTsectionDescription" runat="server"></asp:Literal>
                                    </div>
                                    <asp:Repeater ID="RPTfields" runat="server" DataSource="<%#Container.DataItem.Fields%>"
                                        OnItemDataBound="RPTfields_ItemDataBound">
                                        <ItemTemplate>
                                            <CTRL:CTRLrenderField ID="CTRLrenderField" runat="server" />
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
                                            <asp:Literal ID="LTrequiredFilesTitle" runat="server"></asp:Literal>
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
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="HDNdownloadTokenValue" />
</asp:Content>