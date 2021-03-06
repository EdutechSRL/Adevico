<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucDomandaTestoLiberoEdit.ascx.vb"
    Inherits="Comunita_OnLine.ucDomandaTestoLiberoEdit" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<asp:Panel ID="PNLDomanda" runat="server">
    <asp:FormView ID="FRVDomanda" runat="server" CellPadding="4" ForeColor="#333333"
        Width="100%">
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#2461BF" />
        <RowStyle BackColor="#EFF3FB" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <ItemTemplate>
            <table width="100%">
                <td>
                     <div id="DIVpaginaCorrente" runat="server">
                        <asp:Label ID="LBLPaginaCorrente" runat="server" Text=""></asp:Label>
                        <asp:DropDownList Enabled="<%#Not isDomandaReadOnly%>" ID="DDLPagina" runat="server"
                            DataTextField="nomePagina" DataValueField="id">
                        </asp:DropDownList>
                    </div>
                </td>
                <td valign="middle" align="right" class="DIVHelp">
                    <asp:Label runat="server" ID="LBAiuto" ></asp:Label>
                </td>
                <td width="30px">
                    <div class="DIVHelp">
                        <asp:HyperLink ID="HYPhelp" runat="server" ImageUrl="~/Questionari/img/Help.png"/>
                    </div>
                </td>
            </table>
            <br />
            <br />
            <asp:Label ID="LBTestoDomanda" runat="server" Text=""></asp:Label><br />
              <CTRL:CTRLeditor id="CTRLeditorTestoDomanda" runat="server" ContainerCssClass="containerclass"
                LoaderCssClass="loadercssclass" EditorHeight="300px" EditorWidth="100%" ModuleCode="SRVQUST" AutoInitialize="True">
                </CTRL:CTRLeditor>
            <br />
            <div style="display: <%#visibilityValutazione%>;">
                <asp:Label ID="LBPeso" runat="server" Text="Peso" Width="176px"></asp:Label>
                <asp:TextBox ID="TXBPeso" runat="server"></asp:TextBox><br />
                <asp:CompareValidator runat="server" ID="COVPesoInt" Operator="DataTypeCheck" Type="Integer"
                    Display="Dynamic" ControlToValidate="TXBPeso" />
                <asp:CompareValidator runat="server" ID="COVPesoNot0" Operator="GreaterThan" ValueToCompare="0"
                    Type="Integer" Display="Dynamic" ControlToValidate="TXBPeso" />
                <br />
                <asp:Label ID="LBdifficolta" runat="server" Text="" Width="176px"></asp:Label>
                <asp:DropDownList Enabled='<%#Not isDomandaReadOnly%>' ID="DDLDifficolta" runat="server">
                    <asp:ListItem Value="0"></asp:ListItem>
                    <asp:ListItem Selected="True" Value="1"></asp:ListItem>
                    <asp:ListItem Value="2"></asp:ListItem>
                </asp:DropDownList>
                <br />
                <br />
                <asp:CheckBox runat="server" ID="CHKisValutabile" Checked='<%#DataBinder.Eval(Container, "DataItem.isValutabile")%>' />
            </div>
            <br />
            <asp:CheckBox runat="server" ID="CHKisObbligatoria" Checked='<%#DataBinder.Eval(Container, "DataItem.isObbligatoria")%>' />
            
            <asp:Label ID="LBNumeroOpzioni" runat="server" Text=""></asp:Label>
            <asp:DropDownList Enabled='<%#Not isDomandaReadOnly%>' ID="DDLNumeroOpzioni" runat="server"
                AutoPostBack="true" OnSelectedIndexChanged="selezionaNumeroOpzioni">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
                <asp:ListItem>12</asp:ListItem>
                <asp:ListItem>13</asp:ListItem>
                <asp:ListItem>14</asp:ListItem>
                <asp:ListItem>15</asp:ListItem>
                <asp:ListItem>16</asp:ListItem>
                <asp:ListItem>17</asp:ListItem>
                <asp:ListItem>18</asp:ListItem>
                <asp:ListItem>19</asp:ListItem>
                <asp:ListItem>20</asp:ListItem>
            </asp:DropDownList>
            <br />
            <br />
            <asp:DataList runat="server" Width="100%" ID="DLOpzioni" OnItemCommand="eliminaOpzione"
                Visible="true">
                <ItemTemplate>
                    <table cellspacing="0" width="100%" bordercolor="black" bgcolor="white" style="border: 1px solid #333;"
                        cellpadding="5">
                        <tr>
                            <td>
                                <asp:Label ID="LBLEtichetta" runat="server" Text=""></asp:Label>
                            </td>
                            <td align="right">
                                <asp:ImageButton ID="IMBElimina" Visible='<%#Not isDomandaReadOnly%>' runat="server"
                                    ImageUrl="../img/elimina.gif" CommandName="elimina" AlternateText=""></asp:ImageButton>
                            </td>
                        </tr>
                        <tr>
                            <td bordercolor="#EFF3FB" colspan="2">
                                <CTRL:CTRLeditor id="CTRLeditorEtichetta" runat="server" ContainerCssClass="containerclass"
                                    LoaderCssClass="loadercssclass" EditorHeight="230px" EditorWidth="100%" ModuleCode="SRVQUST" AutoInitialize="True"
                                    HTML='<%#Eval("etichetta")%>'>
                                    </CTRL:CTRLeditor>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <asp:CheckBox runat="server" ID="CHKisSingleLine" Text="Riga singola"/>
                                </div>                                                                     
                                <div style="display: <%#visibilityValutazione%>;">
                                    <asp:Label ID="LBPesoRisposta" runat="server" AssociatedControlID="TXBPesoRisposta"></asp:Label>
                                    <asp:TextBox ID="TXBPesoRisposta" runat="server" Width="30px" Text='<%#DataBinder.Eval(Container, "DataItem.peso")%>'></asp:TextBox>
                                    %
                                </div>
                            </td>
                            <td>
                                <asp:CompareValidator runat="server" ID="COVPesoIntOpzioni" Operator="DataTypeCheck"
                                    Type="Integer" Display="Dynamic" ControlToValidate="TXBPesoRisposta" ErrorMessage="" />
                                <asp:RangeValidator runat="server" ID="RVPesoMin100" MinimumValue="-100" MaximumValue="100"
                                    Type="Integer" ControlToValidate="TXBPesoRisposta" ErrorMessage="" />
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
            <br />
            <asp:Label ID="LBTestoDopoDomanda" runat="server" Text=""></asp:Label><br />
            <asp:TextBox ID="TXBTestoDopoDomanda" runat="server" Width="100%" TextMode="MultiLine"
                MaxLength="250"></asp:TextBox>
            <br />
            <asp:Label ID="LBSuggerimento" runat="server" Text=""></asp:Label>
            <br />
            <asp:TextBox ID="TXBSuggerimento" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
            <br />
            </center>
        </ItemTemplate>
    </asp:FormView>
</asp:Panel>
