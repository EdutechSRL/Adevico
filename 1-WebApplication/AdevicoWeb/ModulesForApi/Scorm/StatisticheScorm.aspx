<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="VoidAPI.aspx.vb" Inherits="Comunita_OnLine.VoidAPI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
    Statistiche Scorm
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
    <!-- Service Localization (.js) -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- Head Content: .ccs, .js -->
    <link href="css/statScorm.css" rel="stylesheet" />
    <script src="../Resources/frontendCustom/Utils.js"></script>
	
    <link href="../Resources/frontendFrameworks/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../Resources/frontendFrameworks/js/bootstrap.min.js"></script>

    <script src="../Resources/frontendFrameworks/js/angular.min.js"></script>
    <script src="js/appStatScorm.js"></script>
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
    Statistiche Scorm<%--=GetLocalization("Title.test.text")--%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="hide">
        <asp:Literal runat="server" ID="LTlocalizationService">APItest</asp:Literal>
        <asp:Literal runat="server" ID="LTlocalizationValue"></asp:Literal>
    </div>    	
	<script type="text/javascript">
	    jQuery("body").attr("ng-app", "appStatScorm");
	    jQuery("body").attr("ng-controller", "MasterCtrl");
	</script>
    <div ng-if="PermissionsError" class="alert alert-danger clearfix">
        <strong>Error:</strong> Forbidden <a href="/" class="btn right btn-danger"><i class="glyphicon glyphicon-refresh"></i> Refresh login</a>
    </div>
	<div id="divMatsterStatScrom">
		<div id="boxStatScrom" ng-cloak ng-if="!PermissionsError">
			<div ng-hide="ViewMode == 'FromParams'">
				<div>
					<div><a id="btnModalComunityPersons" class="btn btn-default btn-lg advPulsantone" ng-click="CheckPersonUpdates();" data-toggle="modal" data-target="#modalComunityPersons">
						<i ng-show="loadingUserResources" class="glyphicon glyphicon-refresh rotationON pull-right"></i>
						<span style="display: inline-block;vertical-align: middle;line-height:20px;">
							Seleziona Utenti ({{ListaUtentiScormChecked.length}})								
						</span><br />
						<span style="display: inline-block;vertical-align: middle;line-height:40px;font-size:30px;position:relative;">
							<i class="glyphicon glyphicon-user"></i>
							<i class="glyphicon glyphicon-user" style="position: absolute;top: 6px;left: 11px;z-index: 564;"></i>
						</span>
					</a>
					<a id="btnModalComunityScormFile" class="btn btn-default btn-lg advPulsantone" ng-click="CheckFileScormUpdates();" data-toggle="modal" data-target="#modalComunityScormFile">
						<i ng-show="loadingDocResources" class="glyphicon glyphicon-refresh rotationON pull-right"></i>
						<span style="display: inline-block;vertical-align: middle;line-height:20px;">
							Seleziona File ({{ListaDocScormChecked.length}})								
						</span><br />
						<span style="display: inline-block;vertical-align: middle;line-height:40px;font-size:30px;position:relative;">
							<i class="glyphicon glyphicon-file"></i>	
							<i class="glyphicon glyphicon-file" style="position: absolute;top: 6px;left: 11px;z-index: 564;"></i>
						</span>
					</a>
					</div><br />
					<div class="clearfix" ng-show="ListaUtentiScormChecked.length > 0 && ListaDocScormChecked.length > 0">
						<a ng-disabled="ListaUtentiScormChecked.length < 1 || ListaDocScormChecked.length < 1"
								class="btn btn-primary pull-right"
								ng-click="CheckPlayFileScormUpdates()">
							Applica filtri
						</a>
						<a class="btn btn-default {{GroupByPerson ? 'active' : ''}}" ng-click="ToggleGroupByPerson(true)">
							Group by User <i class="glyphicon glyphicon-user"></i>
						</a>&nbsp;
						<a class="btn btn-default {{GroupByPerson ? '' : 'active'}}" ng-click="ToggleGroupByPerson(false)">
							Group by File <i class="glyphicon glyphicon-file"></i>
						</a>
					</div>
				</div>
				<br />
			</div>
			<div ng-show="ListaTreePlays && ListaTreePlays.length > 0">
				<div class="panel panel-heading">
					<h2>Play Scorm
						<b class="btn btn-exportCSV pull-right">
							<i class="glyphicon glyphicon-arrow-down"></i>
							<i ng-show="ListaTreePlays && ListaTreePlays.length > 0"
							   class="icon iconXlsx mediumIcon withAction pull-right"
							   ng-click="GetCSVListaPlayScorm(ListaTreePlays)" title="Esporta in CSV"></i>
						</b>
						<i style="margin: 3px 6px;" class="pull-right withAction glyphicon glyphicon-sort-by-alphabet{{(OrderReverse) ? '-alt' : ''}}" ng-click="sortBy('Surname')"></i>
					</h2>
				</div>
				<div ng-repeat="docPlay in ListaTreePlays | orderBy:OrderPropertyName:OrderReverse" class="panel panel-default panel-heading">
					<h3><i class="glyphicon glyphicon-file"></i> {{ docPlay.Name }} {{ (docPlay.Surname) ? docPlay.Surname : '' }}</h3>
					<table class="table table-striped table-condensed">
						<thead>
							<tr>
								<th>Nome</th>
								<th>Ver.</th>
								<th>Data play</th>
								<th>Activity</th>
								<th>Tempo</th>
								<th>Score</th>
								<th>Stato</th>
								<th width="54">&nbsp;</th>
							</tr>
						</thead>
						<tbody ng-show="docPlay.Plays && docPlay.Plays.length > 0">
							<tr ng-repeat="play in docPlay.Plays | orderBy:'LabelName'">
								<td>
									{{ play.LabelName }}
								</td>
								<td>{{play.VersionNumber }}</td>
								<td>{{play.EndPlayOn | date:'dd/MM/yyyy HH:mm:ss'}}</td>
								<td>{{play.ActivitiesDone }}/{{play.ActivitiesTotal }}</td>
								<td>{{play.PlayTime }}{{ play.MinTime ? '/'+ play.MinTime : ''}}</td>
								<td>{{play.PlayScore }}{{ play.MinScore ? '/'+ play.MinScore : ''}}</td>
								<td>
									<span ng-show="play.AlreadyCompleted && play.Status != 6" class="label label-success" title="completato in passato, vedi dettagli">*</span>
									<span ng-show="play.Status" class="label {{(play.Status == 6) ? 'label-success' : (play.Status > 0) ? 'label-warning' : 'label-danger'}}">
										{{getScormStatus(play)}}
									</span>
								</td>
								<td>
									<b class="btn btn-default btn-sm {{(play.Status > 0) ? '' : 'invisible'}}" ng-click="CheckDettailPlayFileScorm(play);"
										data-toggle="modal" data-target="#modalDettailPlaysScormFile">Detail</b>
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
			<div ng-hide="ListaTreePlays && ListaTreePlays.length > 0" class="alert alert-info"> <strong>Info:</strong> seleziona file e utenti tramite gli appositi filtri</div>
		</div>
		<!-- View selector Persons -->
		<div id="modalComunityPersons" class="modal fade" role="dialog">
			<div class="modal-dialog modal-lg">
				<div class="modal-content">
					<div class="modal-body" ng-init="ListaUtentiScormCheckedTEMP = [];isAllPersSelected = true;"">
						<div class="boxPersonsDoc">
							<div class="boxRicerca">
								<div class="clearfix">
									<div class="clearfix">
										<b class="btn btn-exportCSV pull-right">
											<i class="glyphicon glyphicon-arrow-down"></i>
											<i class="icon iconXlsx mediumIcon" ng-click="GetCSVListaGenerica(ListaUtentiScormFiltered)" title="Esporta in CSV"></i>
										</b>
										<h4>Filtro selezione utenti</h4>
									</div>
									<div class="row">
										<div class="form-group col-md-4">
											<label class="control-label">Nome</label>
											<input class="form-control" placeholder="Nome" ng-model="queryPersons.Name" />
										</div>
										<div class="form-group col-md-4">
											<label class="control-label">Cognome</label>
											<input class="form-control" placeholder="Cognome" ng-model="queryPersons.Surname" />
										</div>
										<div class="form-group col-md-4">
											<label class="control-label">Mail</label>
											<input class="form-control" placeholder="Mail" ng-model="queryPersons.Mail" />
										</div>  
									</div>
									<div class="row">                                   
										<div class="form-group col-md-4">
											<label class="control-label">Stato</label>
											<select class="form-control" ng-model="queryPersons.IsActiveCommunity">
                                                <option value="">All</option>
                                                <option value="true">Attivi</option>
                                                <option value="false">Disattivi</option>
											</select>
										</div>                									
										<div class="form-group col-md-4" ng-init="roleValue=''">
											<label class="control-label">Ruoli</label>
											<select class="form-control" ng-model="roleValue" ng-change="queryPersonsStrict = myParseInt(roleValue)">
                                                <option value="">All</option>
                                                <option ng-repeat="role in ListaUserRoles" value="{{role.Id}}">{{role.Name}}</option>
											</select>
										</div>
										<div class="form-group col-md-4">
											&nbsp;
										</div>    
									</div>
								</div><br />
								<div class="clearfix">
									<a class="btn btn-primary pull-right"
									   ng-click="applicaCheckedPerson(ListaUtentiScormCheckedTEMP)"
									   style="margin-left:12px;" data-dismiss="modal">
										Conferma
									</a>
									<b class="btn btn-default pull-right" data-dismiss="modal">Annulla</b>
									
									<b class="btn btn-default" ng-click="CheckPersonUpdates()"><i class="glyphicon glyphicon-refresh {{loadingUserResources ? 'rotationON':''}} pull-right"></i></b>
									<b class="btn btn-default" 
									   ng-click="(queryPersons && queryPersons.select && queryPersons.select != '!undefined') ? queryPersons.select = '!undefined' : queryPersons.select = 'true'">
										Mostra selezionati ({{(ListaUtentiScormCheckedTEMP = (ListaUtentiScorm | filter : { select : true })).length}})
									</b>
									<b class="btn btn-default" 
									   ng-click="toggleCheckTuttiUsers(isAllPersSelected = !isAllPersSelected)">
									   {{isAllPersSelected ? 'Seleziona' : 'Deseleziona' }} tutti
									</b>
								</div>
							</div>
							<div ng-hide="ListaUtentiScorm && ListaUtentiScorm.length > 0" class="alert alert-info">
								Dati in caricamento
								<i class="glyphicon glyphicon-refresh {{loadingUserResources ? 'rotationON':''}} pull-right"></i>
							</div>
							<table ng-show="ListaUtentiScorm && ListaUtentiScorm.length > 0" class="table table-striped table-condensed" ng-init="showFindInputs = false">
								<thead>
									<tr>
										<th>&nbsp;</th>
										<th ng-click="userSortBy('Name')">
											<i ng-show="OrderUserPropertyName === 'Name'" class="glyphicon glyphicon-arrow-{{(OrderUserReverse)?'up':'down'}}"></i> Nome
										</th>
										<th ng-click="userSortBy('Surname')">
											<i ng-show="OrderUserPropertyName === 'Surname'" class="glyphicon glyphicon-arrow-{{(OrderUserReverse)?'up':'down'}}"></i> Cognome
										</th>
										<th ng-click="userSortBy('Mail')">
											<i ng-show="OrderUserPropertyName === 'Mail'" class="glyphicon glyphicon-arrow-{{(OrderUserReverse)?'up':'down'}}"></i> Mail
										</th>
										<th ng-click="userSortBy('RoleName')">
											<i ng-show="OrderUserPropertyName === 'RoleName'" class="glyphicon glyphicon-arrow-{{(OrderUserReverse)?'up':'down'}}"></i> Ruolo
										</th>
										<th>
											Active
										</th>
									</tr>
								</thead>
								<tbody>
									<tr ng-repeat="Person in ListaUtentiScormShowed =(ListaUtentiScormFiltered = (ListaUtentiScorm |
											filter:queryPersons | filter : queryPersonsStrict : true) | orderBy:OrderUserPropertyName:OrderUserReverse |
											limitTo : MAX_ITERATION_Person)"
										class="itemPersonsCDoc">
										<td><input type="checkbox" ng-model="Person.select"/></td>
										<td>{{Person.Name}}</td>
										<td>{{Person.Surname}}</td>
										<td>{{Person.Mail}}</td>
										<td>{{ SetRoleName(Person) || Person.RoleName }}</td>
										<td><i class="glyphicon glyphicon-{{Person.IsActiveCommunity ? 'ok' : 'remove'}}"></i></td>
									</tr>
								</tbody>
								<tfoot>
									<tr class="itemPersonsCDoc" ng-click="showMorePerson(Person)">
										<td colspan="6">
											<div ng-show="MAX_ITERATION_Person < ListaUtentiScormFiltered.length && MAX_ITERATION_Person < ListaUtentiScorm.length"
												 ng-click="showMorePersonItems()"
												 class="boxNotice boxNotice-warning withAction">Get more<i class="triangle">&nbsp;</i></div>
										</td>
									</tr>
								</tfoot>
							</table>
						</div>
					</div>
				</div>
			</div>
		</div>
		<!-- View selector Files -->
		<div id="modalComunityScormFile" class="modal fade" role="dialog">
			<div class="modal-dialog modal-lg">
				<div class="modal-content">
					<div class="modal-body" ng-init="ListaDocScormCheckedTEMP = []; isAllSelected = true;">
						<div class="boxRicerca">
							<div class="clearfix">
								<div class="clearfix">									
									<b class="btn btn-exportCSV pull-right">
										<i class="glyphicon glyphicon-arrow-down"></i>
										<i class="icon iconXlsx mediumIcon withAction pull-right" ng-click="GetCSVListaGenerica(ListaDocScormFiltered,
											['Name','Extension', 'Path', 'LastVersionId', 'TotalDownload', 'TotalPlay', 'LastUpdate'],
											['Name','Extension', 'Path', 'LastVersionId', 'TotalDownload', 'TotalPlay', 'LastUpdate'],
											'Scorm file')" title="Esporta in CSV"></i>
									</b>
									<h4>Filtro selezione file</h4>
								</div>
								<div class="row">
									<div class="form-group col-md-4">
										<label class="control-label">Nome file:</label>
										<input class="form-control" ng-model="queryDocs.Name" />
									</div>
								</div><br />
								<div class="clearfix">
									<b class="btn btn-default" ng-click="CheckFileScormUpdates()"><i class="glyphicon glyphicon-refresh {{loadingDocResources ? 'rotationON':''}} pull-right"></i></b>
									<b class="btn btn-default" 
									   ng-click="(queryDocs && queryDocs.select && queryDocs.select != '!undefined') ? queryDocs.select = '!undefined' : queryDocs.select = 'true'">
										Mostra selezionati ({{(ListaDocScormCheckedTEMP = (ListaDocScorm | filter : { select : true })).length}})
									</b>
									<b class="btn btn-default" 
									   ng-click="toggleCheckTuttiDocs(isAllSelected = !isAllSelected)">
									   {{isAllSelected ? 'Seleziona' : 'Deseleziona' }} tutti
									</b>
									<a class="btn btn-primary pull-right" style="margin-left:12px;" ng-click="applicaCheckedFiles(ListaDocScormCheckedTEMP)" data-dismiss="modal">Conferma</a>
									<a class="btn btn-default pull-right" data-dismiss="modal">Annulla</a>
								</div>
							</div>
						</div>
						<div id="boxStatisticheRepository">						
							<div ng-hide="ListaDocScormFiltered && ListaDocScormFiltered.length > 0" class="alert alert-info">
								Dati in caricamento
								<i class="glyphicon glyphicon-refresh {{loadingDocResources ? 'rotationON':''}} pull-right"></i>
							</div>
							<div ng-show="ListaDocScormFiltered.length == 0" class="boxNotice boxNotice-warning">
								Nessun Documento trovato
							</div>
							<div id="boxIteratorStatisticheRepository" class="clearfix">
								<div ng-repeat="doc in ListaDocScormShowed = (ListaDocScormFiltered = (ListaDocScorm |
									 filter:queryDocs | filterTags: ListaFileTagsSelected) |
									 limitTo : MAX_ITERATION_DOC) | orderBy:OrderDocPropertyName:OrderDocReverse"
									 class="itemDocRepStatisticheMagic">
									<div class="itemDocRepStatistiche">
										<div class="boxcontent">
											<div><input type="checkbox" class="pull-right" ng-model="doc.select" /></div>
											<i class="icon iconScorm">&nbsp;</i><br>
											<h3 title="{{doc.Name}}">{{doc.Name}}</h3>
											<span>Play Count: {{doc.TotalPlay}}</span>
										</div>
									</div>
								</div>
							</div>
							<div ng-show="MAX_ITERATION_DOC < ListaDocScormFiltered.length && MAX_ITERATION_DOC < ListaDocScorm.length"
								 ng-click="showMoreDocItems()"
								 class="boxNotice boxNotice-warning withAction">Get more<i class="triangle">&nbsp;</i></div>
						</div>
					</div>
				</div>

			</div>
		</div>                        
		<!-- View dettail Paly -->
		<div id="modalDettailPlaysScormFile" class="modal fade" role="dialog">
			<div class="modal-dialog modal-lg">
				<div class="modal-content">
					<div class="modal-body" 
						 ng-show="DetailPlayScorm && DetailPlayScorm.ListaPlaysOneByOne && DetailPlayScorm.PersonEntity && DetailPlayScorm.FileEntity">
						<div>
							<div class="panel panel-heading">
								<a class="btn btn-default pull-right" data-dismiss="modal">X</a>
								<h2>Play Scorm</h2>
							</div>
							<div class="panel panel-default panel-heading">
								<div class="row">
									<h3 class="col-md-5"><i class="glyphicon glyphicon-user"></i> {{ DetailPlayScorm.PersonEntity.Name }} {{ DetailPlayScorm.PersonEntity.Surname }}</h3>
									<h3 class="col-md-6"><i class="glyphicon glyphicon-file"></i> {{ DetailPlayScorm.FileEntity.Name }}</h3>
									<div class="col-md-1">
										<b class="btn btn-exportCSV pull-right">
											<i class="glyphicon glyphicon-arrow-down"></i>
											<i class="icon iconXlsx mediumIcon withAction pull-right" ng-click="GetCSVListaGenerica(DetailPlayScorm.ListaPlaysOneByOne,
												['VersionNumber','ActivitiesDone', 'ActivitiesTotal', 'PlayTime', 'MinTime', 'ScormCompletion', 'Status', 'AlreadyCompleted'],
												['VersionNumber','ActivitiesDone', 'ActivitiesTotal', 'PlayTime', 'MinTime', 'ScormCompletion', 'Status', 'AlreadyCompleted'],
												'Scorm dettail file')" title="Esporta in CSV"></i>
										</b>
									</div>
								</div>
								<table class="table table-striped table-condensed">
									<thead>
										<tr>
											<th>Ver.</th>											
											<th>Data play</th>
											<th>Activity</th>
											<th>Tempo</th>
											<th>Score</th>
											<th>Scorm comp.</th>
											<th>Stato</th>
										</tr>
									</thead>
									<tbody ng-show="DetailPlayScorm.ListaPlaysOneByOne.length > 0">
										<tr ng-repeat="play in DetailPlayScorm.ListaPlaysOneByOne">
											<td>{{play.VersionNumber }}</td>
											<td>{{play.EndPlayOn | date:'dd/MM/yyyy HH:mm:ss'}}</td>
											<td>{{play.ActivitiesDone }}/{{play.ActivitiesTotal }}</td>
											<td>{{play.PlayTime }}{{ play.MinTime ? '/'+ play.MinTime : ''}}</td>
											<td>{{play.PlayScore }}{{ play.MinScore ? '/'+ play.MinScore : ''}}</td>
											<td>{{play.ScormCompletion}}</td>
											<td>
												<span ng-show="play.Status" class="label {{(play.Status == 6) ? 'label-success' : (play.Status > 0) ? 'label-warning' : 'label-danger'}}">
													{{getScormStatus(play)}}
												</span>
											</td>
										</tr>
									</tbody>
								</table>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>		
    				
	<script type="text/javascript">
	    jQuery("#divMatsterStatScrom .modal").appendTo("body");
	</script>
	
	<div style="height:0;overflow:hidden;position:relative;">
	<!-- Service content (page) -->
	<h1>Test</h1>
    <ol>
        <li>
            <%=ShowCookie().Replace(";", "</li><li>")%>
        </li>
    </ol>
	</div>
	
	 <%--Add for Session--%>
    
    <asp:UpdatePanel ID="UPTempo" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TMsession" EventName="Tick" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Timer ID="TMsession" runat="server"></asp:Timer>
</asp:Content>
