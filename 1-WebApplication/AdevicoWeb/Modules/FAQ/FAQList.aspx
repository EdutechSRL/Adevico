<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="FAQList.aspx.vb" Inherits="Comunita_OnLine.FAQList" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Editor" Src="~/Modules/Common/Editor/UC_Editor.ascx" %>


<%-- Eventuali controlli (UC/Telerik) --%>


<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
    <%-- Titolo nella barra del browser --%>    
    <asp:Literal ID="LTpageTitle_t" runat="server">*Lista FAQ</asp:Literal>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
    <%-- non serve --%>    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <%-- Elementi da caricare nell'HEADER: css, js --%>
    <link href="Css/Adevico-UI.css" rel="stylesheet" />
    <script src="Js/JQuery.Adevico-UI.js"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <%-- Contenuto del servizio: eventualmente verificare la parte "pagina larghezza 100%" --%>
    <div class="ADVrow">
        <div class="ADVcol-9" style="position:relative;">
            <h3 class="ADVrow"><asp:LinkButton ID="BTNShowAddFaq" runat="server" CssClass="ADVpull-right img_btn ico_add_s" Text="" Visible="false" /></h3>
            <asp:Panel ID="PNaddFaq" CssClass="AdvBoxOverlayPanelFaq" runat="server" Visible="false">
                <div class="ADVpanel-group">
                    <div class="ADVpanel ADVpanel-body ADVRow">                         
                        <div class="ADVpull-right icons">
                            <asp:LinkButton ID="BTNAnnullaAddFaq" runat="server" CssClass="icon refuse" Text="" Visible="false" />
                            <asp:LinkButton ID="BTaddFaq" runat="server" CssClass="icon accept" Text="" Visible="false" />
                        </div> 
                        <asp:HiddenField ID="HFfaqId" runat="server" />                
                        <div class="ADVpanel-collapse">
                            <asp:Label ID="LBcategoria" runat="server" Text="Categoria"></asp:Label><br />
                            <asp:CheckBoxList ID="CBLcategorie" runat="server"></asp:CheckBoxList>
                        </div><br />
                        <div class="ADVpanel-collapse">
                            <asp:Label ID="LBdomanda" runat="server" Text="Domanda"></asp:Label><br />
                            <asp:TextBox ID="TXTquestion" runat="server" CssClass="ADVform-control"></asp:TextBox>
                        </div><br />
                        <div class="ADVpanel-collapse">
                                <asp:Label ID="LBrisposta" runat="server" Text="Risposta"></asp:Label><br />
                                <CTRL:Editor ID="CTRLeditorText" runat="server"
                                             ContainerCssClass="textarea" LoaderCssClass="loadercssclass inlinewrapper"
                                             EditorCssClass="editorcssclass" AllAvailableFontnames="false"
                                             MaxHtmlLength="800000"/>
                        </div>
                    </div>
                </div> 
            </asp:Panel>   
            <div class="ADVpanel-group">
                <asp:Panel ID="PnNoFaqs" runat="server" CssClass="ADVpanel-heading ADVrow" Visible="false">No FAQ</asp:Panel>
                <asp:Repeater runat="server" ID="RPTFaq">
                    <ItemTemplate>
                          <asp:Panel ID="PnItemFaq" runat="server" CssClass="ADVpanel ADVpanel-default icons">
                            <div class="ADVpanel-heading ADVrow">
                                <div class="ADVpanel-boxButtons">
                                    <div>
                                        <asp:LinkButton ID="BTmodifyFaq" runat="server" Text="" CssClass="icon edit" CommandName="ClickModify" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID")%>' Visible="false" />
                                        <asp:LinkButton ID="BTNdeleteFaq" runat="server" Text="" CssClass="icon delete" CommandName="ClickDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID")%>' Visible="false" />
                                    </div><br />
                                    <div>
                                        <asp:LinkButton ID="BTNUpOrderFaq" runat="server" Text="" CssClass="icon arrowUp" CommandName="ClickOrderUp" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID")%>' Visible="false" />
                                        <asp:LinkButton ID="BTNDownOrderFaq" runat="server" Text="" CssClass="icon arrowDown" CommandName="ClickOrderDwon" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID")%>' Visible="false" />
                                    </div>
                                </div>
                                <h3 class="ADVpanel-title"><asp:Literal runat="server" ID="LTquestion"></asp:Literal></h3>
                                <asp:Literal runat="server" ID="LTcategories"></asp:Literal>
                            </div>
                            <div class="ADVpanel-collapse ADVcollapsed">
                                <div class="ADVpanel-body">
                                    <asp:Literal runat="server" ID="LTanswer"></asp:Literal>
                                </div>
                            </div>
                          </asp:Panel>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>    
        <div class="ADVcol-3">
            <div style="padding:0 0 0 24px;">
                <h3><asp:Literal ID="LTtitolettoCategorie" Text="Categorie" runat="server"></asp:Literal> <asp:LinkButton ID="BTNShowAddCat" runat="server" CssClass="ADVpull-right img_btn ico_add_s" Text="" Visible="false" /></h3>
                <asp:Panel ID="PNaddCat" runat="server" Visible="false">
                    <div>
                        <asp:Label ID="LBnomeCat" runat="server" Text="Nome categoria"></asp:Label><br />
                        <asp:HiddenField ID="HFCatId" runat="server" />
                        <asp:TextBox ID="TXTcatName" runat="server" CssClass="ADVform-control"></asp:TextBox>
                    </div><br />
                    <div class="ADVpull-right icons" style="">
                        <asp:LinkButton ID="BTNannullaAddCat" runat="server" CssClass="icon refuse" Text="" Visible="false" />
                        <asp:LinkButton ID="BTNaddCat" runat="server" CssClass="icon accept" Text="" Visible="false" />
                    </div>
                    <hr style="clear:both;" />
                </asp:Panel>
                <ul class="menuCategorie">
                    <li class="ADVlist-group-item"><asp:HyperLink ID="HLcatNamTuttee" runat="server" CssClass="ADVlink-list-group-item" Text="Tutte" NavigateUrl="?catID=-1" ></asp:HyperLink></li>
                    <asp:Repeater runat="server" ID="RPTCats">
                        <ItemTemplate>
                            <li class="ADVlist-group-item">
                                <div class="ADVpanel-boxButtons icons">
                                    <asp:LinkButton ID="BTNmodifyCat" runat="server" Text="" CssClass="icon edit" CommandName="ClickModify" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID")%>' />
                                    <asp:LinkButton ID="BTNdeleteCat" runat="server" Text="" CssClass="icon delete" CommandName="ClickDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID")%>' />
                                </div>
                                <asp:HyperLink ID="HLcatName" CssClass="ADVlink-list-group-item" runat="server"></asp:HyperLink>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </div>
    </div>
    <script type="text/javascript">        
        <%=strBuildScriptPage.ToString %>
    </script>
</asp:Content>
