<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortalCLEAN.Master" CodeBehind="VoidAPI.aspx.vb" Inherits="Comunita_OnLine.VoidAPI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
    Test Page
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
    <!-- Service Localization (.js) -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- Head Content: .ccs, .js -->
    <link href="css/statScorm.css" rel="stylesheet" />

    <script src="../Resources/frontendFrameworks/js/angular.min.js"></script>
    <script src="js/appStatScorm.js"></script>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    <%=GetLocalization("Title.test.text")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="hide">
        <asp:Literal runat="server" ID="LTlocalizationService">APItest</asp:Literal>
        <asp:Literal runat="server" ID="LTlocalizationValue"></asp:Literal>
    </div>    	
	<script type="text/javascript">
		jQuery("body").attr("ng-app","appStatScorm");
		jQuery("body").attr("ng-controller","MasterCtrl");
	</script>
	<div id="divMatsterStatScrom">
		<div id="boxStatScrom">
			<div class="panel panel-default panel-heading">
				<div>
					<div>
						<a class="btn btn-default btn-lg advPulsantone" ng-click="CheckPersonUpdates();" data-toggle="modal" data-target="#modalComunityPersons">
							<div><br /></div>
							Utenti <br />
							<i class="glyphicon glyphicon-user"></i> ({{ListaUtentiScormChecked.length}})
							<div><br /></div>
						</a>
						<a class="btn btn-default btn-lg advPulsantone" ng-click="CheckFileScormUpdates();" data-toggle="modal" data-target="#modalComunityScormFile">
							<div><br /></div>
							File <br />
							<i class="glyphicon glyphicon-file"></i> ({{ListaDocScormChecked.length}})
							<div><br /></div>
						</a>
					</div>
					<br />
					<div class="clearfix">
						<button ng-disabled="ListaUtentiScormChecked.length < 1 || ListaDocScormChecked.length < 1"
								type="button" class="btn btn-primary pull-right"
								ng-click="CheckPlayFileScormUpdates()">
							Applica filtri ({{ListaPlayDocRepos.length}})
						</button>

						<a class="btn btn-default {{GroupByPerson ? 'active' : ''}}" ng-click="GroupByPerson = true;">
							Group by <i class="glyphicon glyphicon-user"></i> User
						</a>&nbsp;
						<a class="btn btn-default {{GroupByPerson ? '' : 'active'}}" ng-click="GroupByPerson = false;">
							Group by <i class="glyphicon glyphicon-file"></i> File
						</a>
					</div>
				</div>
			</div>
			<div ng-show="ListaTreePlays && ListaTreePlays.length > 0">
				<div class="panel panel-heading">
					<h2>Play Scorm 
						<i ng-show="ListaTreePlays && ListaTreePlays.length > 0"
						   class="icon iconXlsx mediumIcon withAction pull-right"
						   ng-click="GetCSVListaPlayScorm(ListaTreePlays)" title="Esporta in CSV"></i>
					</h2>
				</div>
				<div ng-repeat="docPlay in ListaTreePlays" class="panel panel-default panel-heading">
					<h3><i class="glyphicon glyphicon-file"></i> {{ docPlay.Name }}</h3>
					<table class="table table-striped table-condensed">
						<thead>
							<tr>
								<th>Nome</th>
								<th>Ver.</th>
								<th>Activity</th>
								<th>Tempo</th>
								<th>Score</th>
								<th>Scorm comp.</th>
								<th>Stato</th>
								<th width="54">&nbsp;</th>
							</tr>
						</thead>
						<tbody ng-show="docPlay.Plays && docPlay.Plays.length > 0">
							<tr ng-repeat="play in docPlay.Plays">
								<td>
									{{ (play.PersonEntity) ? play.PersonEntity.Name + play.PersonEntity.Surname : '' }}
									{{ (play.FileEntity) ? play.FileEntity.Name : '' }}
								</td>
								<td>{{play.VersionNumber }}</td>
								<td>{{play.ActivitiesDone }}/{{play.ActivitiesTotal }}</td>
								<td>{{play.PlayTime }}{{ play.MinTime ? '/'+ play.MinTime : ''}}</td>
								<th>{{play.PlayScore }}{{ play.MinScore ? '/'+ play.MinScore : ''}}</th>
								<td>{{play.ScormCompletion}}</td>
								<td>
									<span ng-show="play.Status" class="label {{(play.Status == 6) ? 'label-success' : (play.Completion > 0) ? 'label-warning' : 'label-danger'}}">
										{{getScormStatus(play.Status)}}
										{{(play.AlreadyCompleted) ? '*' : '' }}
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
		</div>
		<!-- View selector Persons -->
		<div id="modalComunityPersons" class="modal fade" role="dialog">
			<div class="modal-dialog modal-lg">
				<div class="modal-content">
					<div class="modal-body" ng-init="ListaUtentiScormCheckedTEMP = []">
						<div class="boxPersonsDoc">
							<div class="boxRicerca">
								<div class="clearfix">
									<div class="clearfix">
										<a class="btn btn-primary pull-right"
										   ng-click="applicaCheckedPerson(ListaUtentiScormCheckedTEMP)"
										   style="margin-left:12px;" data-dismiss="modal">
											Conferma
										</a>
										<button type="button" class="btn btn-default pull-right" data-dismiss="modal">Annulla</button>
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
								</div><br />
								<div class="clearfix">
									<i class="icon iconXlsx mediumIcon withAction pull-right" ng-click="GetCSVListaGenerica(ListaUtentiScormFiltered)" title="Esporta in CSV"></i>
									<b class="btn btn-default" 
									   ng-click="(queryPersons && queryPersons.select && queryPersons.select != '!undefined') ? queryPersons.select = '!undefined' : queryPersons.select = 'true'">
										Mostra selezionati ({{(ListaUtentiScormCheckedTEMP = (ListaUtentiScorm | filter : { select : true })).length}})
									</b>
								</div>
							</div>
							<table ng-show="ListaUtentiScorm && ListaUtentiScorm.length > 0" class="table table-striped table-condensed" ng-init="showFindInputs = false">
								<thead>
									<tr>
										<th>&nbsp;</th>
										<th>
											Nome 
										</th>
										<th>
											Cognome 
										</th>
										<th>
											Mail
										</th>
									</tr>
								</thead>
								<tbody>
									<tr ng-repeat="Person in ListaUtentiScormShowed =(ListaUtentiScormFiltered = (ListaUtentiScorm |
											filter : queryPersons) |
											limitTo : MAX_ITERATION_Person)"
										class="itemPersonsCDoc">
										<td><input type="checkbox" ng-model="Person.select"/></td>
										<td>{{Person.Name}}</td>
										<td>{{Person.Surname}}</td>
										<td>{{Person.Mail}}</td>
									</tr>
								</tbody>
								<tfoot>
									<tr class="itemPersonsCDoc" ng-click="showMorePerson(Person)">
										<td colspan="4">
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
									<a class="btn btn-primary pull-right" style="margin-left:12px;" ng-click="applicaCheckedFiles(ListaDocScormCheckedTEMP)" data-dismiss="modal">Conferma</a>
									<button type="button" class="btn btn-default pull-right" data-dismiss="modal">Annulla</button>
									<h4>Filtro selezione file</h4>
								</div>
								<div class="row">
									<div class="form-group col-md-4">
										<label class="control-label">Nome file:</label>
										<input class="form-control" ng-model="queryDocs.Name" />
									</div>
								</div><br />
								<div class="clearfix">
									<b class="btn btn-default" 
									   ng-click="(queryDocs && queryDocs.select && queryDocs.select != '!undefined') ? queryDocs.select = '!undefined' : queryDocs.select = 'true'">
										Mostra selezionati ({{(ListaDocScormCheckedTEMP = (ListaDocScorm | filter : { select : true })).length}})
									</b>
									<b class="btn btn-default" 
									   ng-click="toggleCheckTuttiDocs(ListaDocScormCheckedTEMP, isAllSelected = !isAllSelected)">
									   {{isAllSelected ? 'Seleziona' : 'Deseleziona' }} tutti
									</b>
									<i class="icon iconXlsx mediumIcon withAction pull-right" ng-click="GetCSVListaGenerica(ListaDocScormFiltered,
									['Name','Extension', 'Path', 'LastVersionId', 'TotalDownload', 'TotalPlay', 'LastUpdate'],
									['Name','Extension', 'Path', 'LastVersionId', 'TotalDownload', 'TotalPlay', 'LastUpdate'],
									'Scorm file')" title="Esporta in CSV"></i>
								</div>
							</div>
						</div>
						<div id="boxStatisticheRepository">
							<div ng-show="ListaDocScormFiltered.length == 0"
								 class="boxNotice boxNotice-warning">
								Nessun Docmento trovato
							</div>
							<div id="boxIteratorStatisticheRepository" class="clearfix">
								<div ng-repeat="doc in ListaDocScormShowed = (ListaDocScormFiltered = (ListaDocScorm |
									 filter:queryDocs | filterTags: ListaFileTagsSelected) |
									 limitTo : MAX_ITERATION_DOC)"
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
								<button type="button" class="btn btn-default pull-right" data-dismiss="modal">X</button>
								<h2>Play Scorm</h2>
							</div>
							<div class="panel panel-default panel-heading">
								<div class="row">
									<h3 class="col-md-6"><i class="glyphicon glyphicon-user"></i> {{ DetailPlayScorm.PersonEntity.Name }} {{ DetailPlayScorm.PersonEntity.Surname }}</h3>
									<h3 class="col-md-6"><i class="glyphicon glyphicon-file"></i> {{ DetailPlayScorm.FileEntity.Name }}</h3>
								</div>
								<table class="table table-striped table-condensed">
									<thead>
										<tr>
											<th>Ver.</th>
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
											<td>{{play.ActivitiesDone }}/{{play.ActivitiesTotal }}</td>
											<td>{{play.PlayTime }}{{ play.MinTime ? '/'+ play.MinTime : ''}}</td>
											<th>{{play.PlayScore }}{{ play.MinScore ? '/'+ play.MinScore : ''}}</th>
											<td>{{play.ScormCompletion}}</td>
											<td>
												<span ng-show="play.Status" class="label {{(play.Status == 6) ? 'label-success' : (play.Completion > 0) ? 'label-warning' : 'label-danger'}}">
													{{getScormStatus(play.Status)}}
													{{(play.AlreadyCompleted) ? '*' : '' }}
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
	<!-- Service content (page) -->
	<h1>Test</h1>
    <ol>
        <li>
            <%=ShowCookie().Replace(";", "</li><li>")%>
        </li>
    </ol>
</asp:Content>
