﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated. 
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class ImportUsersFromCSV

    '''<summary>
    '''HYPbackToManagement control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents HYPbackToManagement As Global.System.Web.UI.WebControls.HyperLink

    '''<summary>
    '''MLVcsvImport control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents MLVcsvImport As Global.System.Web.UI.WebControls.MultiView

    '''<summary>
    '''VIWempty control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents VIWempty As Global.System.Web.UI.WebControls.View

    '''<summary>
    '''LBnoPermissionToAdd control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents LBnoPermissionToAdd As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''VIWwizard control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents VIWwizard As Global.System.Web.UI.WebControls.View

    '''<summary>
    '''BTNbackTop control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents BTNbackTop As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''BTNnextTop control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents BTNnextTop As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''BTNcompleteTop control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents BTNcompleteTop As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''LBstepTitle control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents LBstepTitle As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''LBstepDescription control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents LBstepDescription As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''MLVwizard control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents MLVwizard As Global.System.Web.UI.WebControls.MultiView

    '''<summary>
    '''VIWsourceCSV control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents VIWsourceCSV As Global.System.Web.UI.WebControls.View

    '''<summary>
    '''CTRLcsvSelector control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents CTRLcsvSelector As Global.Comunita_OnLine.UC_GenericCSVuploader

    '''<summary>
    '''VIWfieldsMatcher control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents VIWfieldsMatcher As Global.System.Web.UI.WebControls.View

    '''<summary>
    '''CTRLfieldsMatcher control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents CTRLfieldsMatcher As Global.Comunita_OnLine.UC_GenericFieldsMatcher

    '''<summary>
    '''VIWitemsSelector control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents VIWitemsSelector As Global.System.Web.UI.WebControls.View

    '''<summary>
    '''CTRLitemsSelector control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents CTRLitemsSelector As Global.Comunita_OnLine.UC_GenericItemsSelector

    '''<summary>
    '''VIWsummary control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents VIWsummary As Global.System.Web.UI.WebControls.View

    '''<summary>
    '''DVsummary control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents DVsummary As Global.System.Web.UI.HtmlControls.HtmlGenericControl

    '''<summary>
    '''LBsummary control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents LBsummary As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''DVimportUsers control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents DVimportUsers As Global.System.Web.UI.HtmlControls.HtmlGenericControl

    '''<summary>
    '''RPMimportUsers control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents RPMimportUsers As Global.Telerik.Web.UI.RadProgressManager

    '''<summary>
    '''RPAimportUsers control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents RPAimportUsers As Global.Telerik.Web.UI.RadProgressArea

    '''<summary>
    '''VIWcomplete control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents VIWcomplete As Global.System.Web.UI.WebControls.View

    '''<summary>
    '''LBcompleteInfo control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents LBcompleteInfo As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''SPNusers control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents SPNusers As Global.System.Web.UI.HtmlControls.HtmlGenericControl

    '''<summary>
    '''LBinvitedUsersErrors_t control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents LBinvitedUsersErrors_t As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''LBusers control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents LBusers As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''VIWerror control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents VIWerror As Global.System.Web.UI.WebControls.View

    '''<summary>
    '''LTerrors control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents LTerrors As Global.System.Web.UI.WebControls.Literal

    '''<summary>
    '''BTNbackBottom control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents BTNbackBottom As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''BTNnextBottom control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents BTNnextBottom As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''BTNcompleteBottom control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents BTNcompleteBottom As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Master property.
    '''</summary>
    '''<remarks>
    '''Auto-generated property.
    '''</remarks>
    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property
End Class