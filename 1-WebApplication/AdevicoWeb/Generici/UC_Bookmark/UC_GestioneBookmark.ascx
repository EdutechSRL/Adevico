<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_GestioneBookmark.ascx.vb" Inherits="Comunita_OnLine.UC_GestioneBookmark" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="radTree" Namespace="Telerik.WebControls" Assembly="RadTreeView.Net2" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLcreaLink" Src="./UC_CreaBookMark.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLdettagliLink" Src="./UC_DettagliBookmark.ascx" %>

<asp:Table ID="TBLgestione" Runat=server HorizontalAlign=Center  BorderColor=000099 GridLines=both CellSpacing=0>
	<asp:TableRow>
		<asp:TableCell CssClass="top" RowSpan=2>
			<radTree:RadTreeView id="RDTgestioneRaccolta" runat="server" align="left"
			CausesValidation="False" PathToJavaScript="~/Jscript/RadTreeView_Client_3_1.js" skin="BookMark" 
			ImagesBaseDir="~/RadControls/TreeView/Skins/BookMark/" 
			CssFile="~/RadControls/TreeView/Skins/BookMark/StyleSelectFolder.css"
 			 autopostback=true
			XPStyle=true MultipleSelect=false DragAndDrop=false AllowNodeEditing="false" ShowLineImages=false width=250px>
			</radTree:RadTreeView>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell CssClass="top" Width=500px>
			<table border=0>
				<tr>
					<td height=450px class=top>
						<asp:panel ID="PNLdettagli" Runat=server HorizontalAlign=Center Visible=False>
							<table  class="TableUcFile" border=1 cellspacing=0 cellpadding=0 width=500px>
								<tr>
									<td align=left >
										<CTRL:CTRLdettagliLink id="CTRLdettagliLink" runat="server"></CTRL:CTRLdettagliLink>
									</td>
								</tr>
							</table>
						</asp:panel>
						<asp:Panel ID="PNLconfermaElimina" Runat=server HorizontalAlign=Center Visible=False>
							<table  Width="500px" align=center >
								<tr>
									<td height="30px" colspan=2>&nbsp;</td>
								</tr>
								<tr>
									<td align=center colspan=2>
										<asp:Label ID="LBconfermaElimina" Runat=server CssClass="confirmDelete11">
											Se si conferma la cancellazione del Link/della cartella selezionato/a, essa verr� replicata sui "Preferiti" e su tutte le altre comunit� a cui � associato/a,
											sicuro di voler proseguire ?
										</asp:Label>
									</td>
								</tr>
								<tr>
									<td height="30px" colspan=2>&nbsp;</td>
								</tr>
							</table>
						</asp:Panel>
					</td>
				</tr>
			</table>
		</asp:TableCell>
	</asp:TableRow>
</asp:Table>