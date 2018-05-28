<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="VoidAPI.aspx.vb" Inherits="Comunita_OnLine.VoidAPI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
    Gestisci utenti
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
    <!-- Service Localization (.js) -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="css/appUsersManagement.css" rel="stylesheet" />
    <script src="../Resources/frontendCustom/Utils.js"></script>
	
    <link href="../Resources/frontendFrameworks/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../Resources/frontendFrameworks/js/bootstrap.min.js"></script>

    <script src="../Resources/frontendFrameworks/js/angular.min.js"></script>
    <script src="js/appUsersManagement.js"></script>
    <script src="js/AdvApiServices.js"></script>

	<style type="text/css">
		div#nav-main ul#top li.communitydefaultview a {
			height: 36px;
			width: 27px;
		}

		#toolbar #tools {
			padding-left: 30%;
			width: 100%;
		}
		
        body input.form-control{
            width:100%;
        }
	</style>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    <!-- Title content: service name -->
    <%=GetLocalization("Title.text")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <!-- Service content (page) -->
    <div class="hide">
        <asp:Literal runat="server" ID="LTlocalizationService">APItest</asp:Literal>
        <span class="localization">
            <asp:Literal runat="server" ID="LTlocalizationValue">
                Message.Warning,
                Message.Info,
                Title.text,
                Title.test.text
            </asp:Literal>
        </span>
    </div>

    <script type="text/javascript">
	    jQuery("body").attr("ng-app", "appUsersManagement");
	    jQuery("body").attr("ng-controller", "MasterCtrl");
	</script>
	<div id="divMatsterUsersManagement" ng-init="genFilter = {}">
        <div ng-if="loadingUserRolesResources" class="alert alert-info clearfix">
            Aggiornamento <b class="btn right btn-info"><i class="glyphicon glyphicon-refresh"></i></b>
        </div>
        <div ng-if="PermissionsError" class="alert alert-danger clearfix">
            <strong>Error:</strong> Forbidden <a href="" class="btn right btn-danger"><i class="glyphicon glyphicon-refresh"></i> Refresh</a>
        </div>
        <div style="position:relative;z-index:11;">            
			<b class="btn btn-exportCSV" style="position:absolute;right:20px;top:20px;">
				<i class="glyphicon glyphicon-arrow-down"></i>
				<i class="icon iconXlsx mediumIcon" ng-click="GetCSVListaUtentiComunita()" title="Esporta in CSV"></i>
			</b>
        </div>
        <div class="jumbotron container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <h4 style="margin:0 0 10px 0;">Ruoli</h4>
                    <div class="btn-group">
                        <b class="btn btn-default {{(CommunityRolesSel.indexOf(r.Id) > -1) ? 'active':''}}" ng-repeat="r in CommunityUserRoles" title="id({{r.Id}})" ng-click="toggleFilterRole(r.Id)">
                        <i class="glyphicon glyphicon-{{(CommunityRolesSel.indexOf(r.Id) > -1) ? 'check':'unchecked'}}"></i>&nbsp;
                        {{r.Name}}&nbsp;<span class="label label-default">{{(CommunityUsersFiltered | filter : { RoleId : r.Id} : true).length}}</span>
                        </b>
                    </div>
                    <hr />
                    <h4>Scadenza</h4>
                    <div class="btn-group">
                        <span class="btn btn-default {{(ScadenzaStatesSel.length > 0) ? '':'active'}}" ng-click="toggleScadenzaStatesSel(null)">
                            <i class="glyphicon glyphicon-{{(ScadenzaStatesSel.length > 0) ? 'unchecked':'check'}}"></i>&nbsp;
                            Tutti&nbsp;<span class="label label-info">&nbsp;{{CommunityUsers.length}}&nbsp;</span>
                        </span>
                        <span class="btn btn-default {{(ScadenzaStatesSel.indexOf(ts.code) > -1) ? 'active':''}}" ng-repeat="ts in TipiScadenze" ng-click="toggleScadenzaStatesSel(ts.code)">
                            <i class="glyphicon glyphicon-{{(ScadenzaStatesSel.indexOf(ts.code) > -1) ? 'check':'unchecked'}}"></i>&nbsp;
                            {{ts.label}}&nbsp;<span class="label {{ts.customClasses}}">&nbsp;{{CountTypeScad[ts.code]}}&nbsp</span>
                        </span>
                    </div>
                    <hr />
                    <h4>Ricerca</h4>
                    <div>
                        <div class="row">
                            <div class="form-group col-md-4">
                                <label>Nome</label>
                                <input ng-model="genFilter.Name" type="text" class="form-control" />
                            </div>
                            <div class="form-group col-md-4">
                                <label>Cognome</label>
                                <input ng-model="genFilter.Surname" type="text" class="form-control" />
                            </div>
                            <div class="form-group col-md-4">
                                <label>Mail</label>
                                <input ng-model="genFilter.Mail" type="text" class="form-control" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div ng-init="showBoxAzioniMultiple = false">
            <div class="clearfix" style="position:relative;z-index:11;">&nbsp;
                <b ng-show="CommunityUsersSel && CommunityUsersSel.length" 
                    class="btn btn-default {{showBoxAzioniMultiple ? 'active' : ''}}" style="position:absolute;bottom:0;right:0;"
                    ng-click="showBoxAzioniMultiple = !showBoxAzioniMultiple">
                    Azioni selezionati <i class="glyphicon glyphicon-chevron-down"></i>
                </b>
                <div id="box-azioni-multiple" class="jumbotron container-fluid" 
                    style="position:absolute;left:0; top:18px; width:100%;"
                    ng-show="showBoxAzioniMultiple && CommunityUsersSel && CommunityUsersSel.length">
                    <h4 style="margin:0 0 10px 0;">Azioni su selezione multipla</h4>
                    <div id="multiselect-box">
                        <div class="clearfix">
                            <ul class="nav nav-tabs">
                              <li class="tab-item active"><a data-parent="#multiselect-box" data-target="#multibox-utente">Utenti</a></li>
                              <li class="tab-item"><a data-parent="#multiselect-box"  data-target="#multibox-ruolo">Ruoli</a></li>
                              <li class="tab-item"><a data-parent="#multiselect-box"  data-target="#multibox-scadenza">Scadenze</a></li>
                            </ul>
                        </div>
                        <div id="multibox-utente" role="tab" class="collapse in tab-box">
                            <b class="btn btn-default" ng-click="LMSAPIservices.enableSelUsers()">Abilita</b>&nbsp;
                            <b class="btn btn-default" ng-click="LMSAPIservices.disableSelUsers()">Disabilita</b>&nbsp;
                            <b class="btn btn-default" ng-click="LMSAPIservices.deleteSelUsers()">Elimina</b>
                        </div>
                        <div id="multibox-ruolo" role="tab" class="collapse tab-box">
                            <div class="form-group form-inline" ng-init="roleIdSel = '15'">
                                <select class="form-control" ng-model="roleIdSel">
                                    <option ng-repeat="r in CommunityUserRoles" value="{{r.Id}}">{{r.Name}}</option>
                                </select>&nbsp;
                                <b class="btn btn-default" ng-click="LMSAPIservices.setRolesInSelUsers(roleIdSel)">Imposta</b>
                            </div>
                        </div>
                        <div id="multibox-scadenza" role="tab" class="collapse tab-box">
                            <div>
                                <div class="form-group form-inline">
                                    <label>Inizio scadenza</label>&nbsp;
                                    <b class="btn btn-default" ng-click="LMSAPIservices.setSelUsersExpiryStartNow()">Forza inizio</b>&nbsp;
                                    <b class="btn btn-default" ng-click="LMSAPIservices.resetSelUsersExpiryStart()">Azzera inizio</b>
                                </div>
                            </div>
                            <hr />
                            <div>
                                <div class="form-group form-inline" ng-init="_validity = '-1';_extendValidity='true';_startBehaviour='0';">
                                    <label>Scadenze</label>&nbsp;
                                    <select class="form-control" ng-model="_validity">
                                        <option ng-repeat="d in DelaySubconfLocal" value="{{d.Value}}">{{d.Key}}</option>
                                    </select>&nbsp;
                                    <select class="form-control" ng-model="_extendValidity">
                                        <option value="true">Estendi</option>
                                        <option value="false">Sovrascrivi</option>
                                    </select>&nbsp;
                                    <select class="form-control" ng-model="_startBehaviour">
                                        <option value="0">Non reimpostare</option>
                                        <option value="1">Azzera inizio</option>
                                        <option value="2">Forza inizio</option>
                                    </select>&nbsp;
                                    <b class="btn btn-default" ng-click="LMSAPIservices.setSelUsersExpiration(_validity, _extendValidity, _startBehaviour)">Imposta</b>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <table class="table table-stripped">
            <tr>
                <th>
                    <b ng-click="selectAll(CommunityUsersSel.length)">
                        <i class="glyphicon glyphicon-{{(CommunityUsersSel.length > 0) ? 'check':'unchecked'}}"></i>&nbsp;({{CommunityUsersSel.length}})
                    </b>
                </th>
                <th hidden>Id</th>
                <th>Name</th>
                <th>Surname</th>
                <th>Mail</th>
                <th>Id ruolo</th>
                <th style="width:1%;text-align:center;">Abilitato</th>
                <th>Durata</th>
                <th>Primo accesso</th>
                <th>Scadenza</th>
                <th style="width:1%;text-align:center;">Expired</th>
                <th>Actions</th>
            </tr>
            <tr ng-repeat="usr in CommunityUsersViewed = (CommunityUsersFiltered = (CommunityUsers | isInArrayScadTypes : ScadenzaStatesSel | filter : genFilter | isInArray : CommunityRolesSel : 'RoleId') | limitTo : CommunityUsersLimitTo)">
                <td>
                    <b ng-click="toggleUser(usr.Id)">
                        <i class="glyphicon glyphicon-{{(CommunityUsersSel.indexOf(usr.Id) > -1) ? 'check':'unchecked'}}"></i>
                    </b>
                </td>
                <td hidden>{{usr.Id}}</td>
                <td>{{usr.Name}}</td>
                <td>{{usr.Surname}}</td>
                <td>{{usr.Mail}}</td>
                <td title="{{usr.RoleName}} ({{usr.RoleId}})">{{(usr.RoleName ? usr.RoleName[0] : '')}}</td>
                <td style="text-align:center;">
                    <i title="{{(usr.IsActiveCommunity ? '':'disabilitato in comunità ')}}{{(usr.IsEnabledPortal ? '':'disabilitato a livello portale ')}}" 
                       class="{{usr.IsActiveCommunity && usr.IsEnabledPortal ? 'glyphicon glyphicon-ok':'glyphicon glyphicon-remove'}}"></i>
                </td>
                <td>
                    <span ng-if="usr.DurationDay > 0">{{usr.DurationDay}}</span>
                </td>
                <td>
                    <span ng-if="usr.StartDate && usr.EndDate">{{usr.StartDate | date : "dd/MM/yyyy"}}</span>
                </td>
                <td>
                    <span ng-if="usr.StartDate && usr.EndDate">{{usr.EndDate | date : "dd/MM/yyyy"}}</span><span ng-if="usr.MissingDays > 0"> ({{usr.MissingDays}})</span>
                </td>
                <td><i class="{{usr.Expired ? 'glyphicon glyphicon-ok':''}}"></i></td>
                <td>
                    <div class="btn-group">
                        <b class="btn btn-default" ng-if="!usr.IsResponsabile" ng-click="LMSAPIservices.setResponsible(usr)">
                            <span style="position: relative;display: inline-block;width: 14px;height: 14px;">
                                <i class="glyphicon glyphicon-user"></i>
                                <i class="glyphicon glyphicon-arrow-up" style="font-size:0.6em;position:absolute;right:-5px;top:0px;"></i>
                            </span>
                        </b>
                        <b class="btn btn-default" ng-if="usr.IsResponsabile" disabled>                           
                            <span style="font-size: 14px;line-height: 14px;min-width: 14px;display: inline-block;font-weight: bold;">R</span>
                        </b>
                        <b class="btn btn-default" ng-click="LMSAPIservices.selezUserInfo(usr)">
                            <i class="glyphicon glyphicon-info-sign"></i>
                        </b>
                    </div>
                </td>
            </tr>
        </table>
        <div ng-if="CommunityUsers.length > CommunityUsersLimitTo">
            <div ng-click="showMoreCommunityUsersLimitTo()" class="alert alert-warning" style="text-align:center;" >Show more...</div>
        </div>
    </div>    	
    <div id="divMatsterUserInfo">
        <div id="modalUserInfo" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" ng-click="selezUserInfo(null)">&times;</button>
                        <h4 class="modal-title" style="text-align:left;padding-left:8px;"><i class="glyphicon glyphicon-user"></i> {{userInfosel.Surname}} {{userInfosel.Name}}</h4>
                    </div>
                    <div class="modal-body" style="text-align:left;">
                            <ul class="list-group">
                                <li class="list-group-item list-group-item-info">Genearl info</li>
                                <li class="list-group-item">RoleName: <span>{{userInfosel.RoleName}}</span> - <span>{{(userInfosel.IsResponsabile ? '(R)': '')}}</span></li>
                                <li class="list-group-item">Mail: <span>{{userInfosel.Mail}}</span></li>
                                <li class="list-group-item">Active: 
                                    <i title="{{(userInfosel.IsActiveCommunity ? '':'disabilitato in comunità ')}}{{(userInfosel.IsEnabledPortal ? '':'disabilitato a livello portale ')}}" 
                                        class="{{userInfosel.IsActiveCommunity && userInfosel.IsEnabledPortal ? 'glyphicon glyphicon-ok':'glyphicon glyphicon-remove'}}"></i>
                                </li>
                                <li class="list-group-item">Scadenze: <span>{{userInfosel.StartDate | date : 'dd/MM/yyyy hh:mm:ss'}} - {{userInfosel.EndDate| date : 'dd/MM/yyyy hh:mm:ss'}} {{(userInfosel.Expired ? '(Expired)':'')}}</span></li>
                            </ul>                        
                            <div ng-if="userInfosel.ExtendedInfo">
                                <ul class="list-group">
                                    <li class="list-group-item list-group-item-info">Extra info</li>
                                    <li class="list-group-item">CF: <span>{{userInfosel.ExtendedInfo.TaxCode}}</span></li>
                                    <li class="list-group-item">LanguageCode: <span>{{userInfosel.ExtendedInfo.LanguageCode}}</span></li>
                                    <li class="list-group-item">Note: <span>{{userInfosel.ExtendedInfo.Note}}</span></li>
                                    <li class="list-group-item">Province: <span>{{userInfosel.ExtendedInfo.Province}} ({{userInfosel.ExtendedInfo.ProvinceCode}})</span></li>
                                    <li class="list-group-item">Country: <span>{{userInfosel.ExtendedInfo.Country}}</span></li>
                                    <li class="list-group-item">BirthDate: <span>{{userInfosel.ExtendedInfo.BirthDate | date : 'dd/MM/yyyy'}}</span></li>
                                    <li class="list-group-item">BirthPlace: <span>{{userInfosel.ExtendedInfo.BirthPlace}}</span></li>
                                    <li class="list-group-item">Address: <span>{{userInfosel.ExtendedInfo.Address}}</span></li>
                                    <li class="list-group-item">ZIPcode: <span>{{userInfosel.ExtendedInfo.ZIPcode}}</span></li>
                                    <li class="list-group-item">City: <span>{{userInfosel.ExtendedInfo.City}}</span></li>
                                    <li class="list-group-item">TelephonePrimary: <span>{{userInfosel.ExtendedInfo.TelephonePrimary}}</span></li>
                                    <li class="list-group-item">TelephoneSecondary: <span>{{userInfosel.ExtendedInfo.TelephoneSecondary}}</span></li>
                                    <li class="list-group-item">TelephoneMobile: <span>{{userInfosel.ExtendedInfo.TelephoneMobile}}</span></li>
                                    <li class="list-group-item">FAX: <span>{{userInfosel.ExtendedInfo.FAX}}</span></li>
                                    <li class="list-group-item">WebSite: <span>{{userInfosel.ExtendedInfo.WebSite}}</span></li>
                                </ul>
                            </div>
                            <div ng-if="userInfosel.Company">
                                <ul class="list-group">
                                    <li class="list-group-item list-group-item-info">Company info</li>
                                    <li class="list-group-item">Name: <span>{{userInfosel.Company.Name}}</span></li>
                                    <li class="list-group-item">TaxCodeVAT: <span>{{userInfosel.Company.TaxCodeVAT}}</span></li>
                                    <li class="list-group-item">REA: <span>{{userInfosel.Company.REA}}</span></li>
                                    <li class="list-group-item">Città: <span>{{userInfosel.Company.City}}</span></li>
                                    <li class="list-group-item">Indirizzo: <span>{{userInfosel.Company.Address}}</span></li>
                                    <li class="list-group-item">Association: <span>{{userInfosel.Company.Association}}</span></li>
                                </ul>
                            </div>
                            <div ng-if="userInfosel.Agency">
                                <ul class="list-group">
                                    <li class="list-group-item list-group-item-info">Agency info</li>
                                    <li class="list-group-item">Name: <span>{{userInfosel.Agency.Current}}</span></li>
                                    <li class="list-group-item">History: 
                                        <ul>                                       
                                            <li ng-repeat="hisA in userInfosel.Agency.History"><span>({{hisA.DateStart | date : 'dd/MM/yyyy'}} - {{hisA.DateEnd | date : 'dd/MM/yyyy'}}{{(hisA.IsCurrent?' (current)':'')}}) </span> {{hisA.Name}}</li>
                                        </ul>
                                    </li>
                                </ul>
                            </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
	<script type="text/javascript">
        jQuery("#divMatsterUserInfo .modal").appendTo("body");
        jQuery(document).ready(function () {
            jQuery("#multiselect-box").on("click", ".tab-item > a", function () {
                var elJ = jQuery(this);
                var parent = jQuery(elJ.attr("data-parent"));
                var target = jQuery(elJ.attr("data-target"));
                parent.find(".tab-item").removeClass("active");
                parent.find(".tab-box").removeClass("in");
                elJ.parents(".tab-item").addClass("active");
                target.addClass("in");
            });
        });
	</script>

    <br/>
    <div class="hide">
    <%=ShowCookie() %>
    </div>
    <br/>
   
    <!-- Valori default "expiration" -->
    <span class="ExpirationValues hide">
        <script>
        DelaySubconfLocal = <%=GetConfig("DelaySubconfLocal")%>;
        </script>
    </span>
    
    
    
    <%--Add for Session--%>
    
    <asp:UpdatePanel ID="UPTempo" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TMsession" EventName="Tick" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Timer ID="TMsession" runat="server">
    </asp:Timer>


</asp:Content>
