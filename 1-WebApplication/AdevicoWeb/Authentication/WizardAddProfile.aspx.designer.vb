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


Partial Public Class WizardAddProfile

    '''<summary>
    '''HYPmanage control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents HYPmanage As Global.System.Web.UI.WebControls.HyperLink

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
    '''VIWprofileTypes control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents VIWprofileTypes As Global.System.Web.UI.WebControls.View

    '''<summary>
    '''CTRLprofileTypes control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents CTRLprofileTypes As Global.Comunita_OnLine.UC_AuthenticationStepProfileType

    '''<summary>
    '''VIWorganization control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents VIWorganization As Global.System.Web.UI.WebControls.View

    '''<summary>
    '''CTRLorganizations control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents CTRLorganizations As Global.Comunita_OnLine.UC_AuthenticationStepOrganizations

    '''<summary>
    '''VIWauthenticationTypes control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents VIWauthenticationTypes As Global.System.Web.UI.WebControls.View

    '''<summary>
    '''CTRLauthentication control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents CTRLauthentication As Global.Comunita_OnLine.UC_AuthenticationStepAuthenticationTypes

    '''<summary>
    '''VIWuserInfo control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents VIWuserInfo As Global.System.Web.UI.WebControls.View

    '''<summary>
    '''CTRLprofileInfo control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents CTRLprofileInfo As Global.Comunita_OnLine.UC_AuthenticationStepProfileInfo

    '''<summary>
    '''VIWcomplete control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents VIWcomplete As Global.System.Web.UI.WebControls.View

    '''<summary>
    '''CTRLsummary control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents CTRLsummary As Global.Comunita_OnLine.UC_AuthenticationStepSummary

    '''<summary>
    '''VIWprofileError control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents VIWprofileError As Global.System.Web.UI.WebControls.View

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
