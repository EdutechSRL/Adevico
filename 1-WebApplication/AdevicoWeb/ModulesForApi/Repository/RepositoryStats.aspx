<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="VoidAPI.aspx.vb" Inherits="Comunita_OnLine.VoidAPI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
    Statistiche Repository
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
    <!-- Service Localization (.js) -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- Head Content: .ccs, .js -->
    <link href="css/statRepository.css" rel="stylesheet" />
    <script src="../Resources/frontendCustom/Utils.js"></script>
	
    <link href="../Resources/frontendFrameworks/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../Resources/frontendFrameworks/js/bootstrap.min.js"></script>

    <script src="../Resources/frontendFrameworks/js/angular.min.js"></script>
    <script src="js/appStatRepository.js"></script>

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
    Statistiche Repository<%--=GetLocalization("Title.test.text")--%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="hide">
        <asp:Literal runat="server" ID="LTlocalizationService">APItest</asp:Literal>
        <asp:Literal runat="server" ID="LTlocalizationValue"></asp:Literal>
    </div>    	
	<script type="text/javascript">
	    jQuery("body").attr("ng-app", "appStatRepository");
	    jQuery("body").attr("ng-controller", "MasterCtrl");
	</script>
    <div ng-if="PermissionsError" class="alert alert-danger clearfix">
        <strong>Error:</strong> Forbidden <a href="/" class="btn right btn-danger"><i class="glyphicon glyphicon-refresh"></i> Refresh login</a>
    </div>
	<div id="divMatsterStatisticheRepository" ng-show="!PermissionsError">
		<div id="boxRicerca">
			<div class="clearfix">
				<div style="width:50%;float:left;padding-right:2%;">
                    <div class="form-inline">
						<label>Nome file:</label><br /><input class="form-control" ng-model="queryDocs.Name" style="width:100%;" />
					</div>
                    <div style="clear:both;">                    
                        <div ng-init="queryDocs.IsActive = true">                  
                            <br />          
                            <br />          
                            <br />
                            <i class="glyphicon glyphicon-{{((queryDocs.IsActive) ? 'unchecked' : 'check')}}" ng-click="((queryDocs.IsActive) ? queryDocs.IsActive = undefined : queryDocs.IsActive = true)"></i>
                            <label ng-click="((queryDocs.IsActive) ? queryDocs.IsActive = undefined : queryDocs.IsActive = true)">Mostra tutte le versioni</label>
                        </div>
					    <div class="form-inline">File esaminati: ({{ListaDocuReposFiltered.length}}/{{ListaDocuRepos.length}})</div>
                    </div>
				</div>
                <div style="width:50%;float:left;padding-left:2%;">
					<div id="boxExtendionFilter" class="form-inline">
						<label>Estensione file:</label><br />
                        <div ng-show="ListaFileExtensionUnique && ListaFileExtensionUnique.length > 0" class="clearfix" style="width: 100%;height: auto;">
                            <span ng-repeat="extSel in ListaFileExtensionUnique = (ListaDocuRepos | uniqueTags:'Extension')"
								  ng-click="ToogleFileExtensionFilter(extSel)"
								  class="ADVExt noDbClickSelection {{IndexOfExtensionInList(extSel) > -1 ? 'selected' : ''}}">{{extSel}}</span>
                        </div>
					</div>
					<div class="form-inline">
						<label>Tags:</label><br />
						<div ng-show="ListaFileTagsUnique && ListaFileTagsUnique.length > 0" class="form-control clearfix" style="width: 100%;height: auto;">
							<span ng-repeat="tagSel in ListaFileTagsUnique = (ListaDocuRepos | uniqueTags:'Tags')"
								  ng-click="ToogleFileTagFilter(tagSel)"
								  class="ADVTag noDbClickSelection {{IndexOfTagInList(tagSel) > -1 ? 'selected' : ''}}">{{tagSel}}</span>
						</div>
					</div>
				</div>
			</div>
			
		</div>
		<div id="boxStatisticheRepository">
			<div id="boxIteratorStatisticheRepository" class="clearfix">
			    <div ng-show="!loadingFilesRepoResources && ListaDocuReposFiltered.length == 0" class="boxNotice boxNotice-warning">
				    Nessun documento trovato
			    </div>
                <div ng-if="loadingFilesRepoResources" class="alert alert-info clearfix">
                    <i class="glyphicon glyphicon-refresh rotationON"></i> Loading...
                </div>
				<div ng-repeat="doc in ListaDocuReposShowed =(ListaDocuReposFiltered = (ListaDocuRepos | orderBy: ['OrderVersion','Number'] | 
						filter:queryDocs | filterExtensions: ListaFileExtensionSelected | filterTags: ListaFileTagsSelected)  | 
                         limitTo : MAX_ITERATION_DOC)" 
						class="itemDocRepStatisticheMagic {{(SelectedDoc && SelectedDoc.Id == doc.Id && SelectedDoc.IdVersion == doc.IdVersion) ? 'active': ''}}">
						<div class="itemDocRepStatistiche">								
							<div class="boxcontent">
								<div class="boxicon" title="{{'file' + doc.Id +' v'+ doc.Number}}">
                                    <span class="cornerTopRight">
                                        <i class="{{(doc.IsActive) ? 'glyphicon glyphicon-star':''}}" title="{{'v'+ doc.Number + ' data: ' + (doc.DataVer | date:'dd/MM/yyyy')}}">{{(!doc.IsActive) ? 'v'+ doc.Number:''}}</i>
                                        <i ng-show="doc.Tags" class="glyphicon glyphicon-tags" title="{{doc.Tags}}"></i>
                                    </span>
                                    <i class="icoDoc">{{doc.Extension}}</i>
								</div>
                                <h3 title="{{doc.Name}}{{doc.Extension}}"><a href="{{doc.LinkFile}}">{{doc.Name}}{{doc.Extension}}</a></h3>
								<span>Ver. Downloads: {{doc.VerDownloaded}}</span><br />
                                <span>Tot. Downloads: {{doc.Downloaded}}</span>
							</div>
							<div class="boxfooter">
								<span ng-click="fnSelectedDoc(doc);"><b class="btnUsers"></b></span>
							</div>
						</div>
						<div class="boxUsersDoc" ng-show="SelectedDoc.Id == doc.Id && SelectedDoc.IdVersion == doc.IdVersion">
							<b ng-click="fnSelectedDoc(null)" class="btnCloseThis">X</b>
							<h3>Utenti ({{ListaUtentiReposFiltered.length}}/{{ListaUtentiRepos.length}}) <i ng-click="GetCSVListaUtenti(ListaUtentiReposFiltered)" title="Esporta in CSV"
																													class="icon iconXlsxWhite mediumIcon withAction"></i></h3>
                            <div ng-show="loadingFileDownloadsResources || loadingFilePersonsResources" class="alert alert-info clearfix">
                                <i class="glyphicon glyphicon-refresh rotationON"></i> Loading...
                            </div>
							<table class="ADVtable ADVtable-striped" ng-init="showFindInputs = false"
                                ng-show="SelectedDoc && SelectedDoc.Id == doc.Id && ListaUtentiRepos && ListaUtentiRepos.length > 0">
								<thead>
									<tr>
									  <th>Nome <input ng-show="showFindInputs" class="form-control" placeholder="Nome" ng-model="queryUsers.Name" /></th>
									  <th>Cognome <input ng-show="showFindInputs" class="form-control" placeholder="Cognome" ng-model="queryUsers.Surname" /></th>
									  <th>Mail <input ng-show="showFindInputs" class="form-control" placeholder="Mail" ng-model="queryUsers.Mail" /></th>
									  <th ng-init="HasReadDocTemp">
                                          <span style="white-space:nowrap;"><i class="glyphicon glyphicon-download-alt" style="font-size: 0.9em;"></i> / N°</span> <select ng-show="showFindInputs" class="form-control"  
                                                            ng-model="HasReadDocTempquery" 
                                                            ng-change="((HasReadDocTempquery == '') ? queryUsers.HasReadDoc = undefined : queryUsers.HasReadDoc = HasReadDocTempquery)">
														<option value="">Tutte</option>
														<option value="true">S</option>
														<option value="false" selected>N</option>
													</select>
									  </th>
                                      <th>First</th>
                                      <th>Last</th>
									  <th><i ng-click="showFindInputs = !showFindInputs" class="icon smallIcon iconSearch">&nbsp;</i></th>
									</tr>
								</thead>
								<tbody>
									<tr ng-repeat="user in ListaUtentiReposShowed = (ListaUtentiReposFiltered = (ListaUtentiRepos | 
												    filter : queryUsers) | 
													limitTo : MAX_ITERATION_USER)"
										class="itemUsersCDoc">												
										<td>{{user.Name}}</td><td>{{user.Surname}}</td><td>{{user.Mail}}</td>
                                        <td><span class="advCircle {{user.HasReadDoc ? 'green' : 'red'}}">{{user.HasReadDoc ? 'S' : 'N'}}</span> / {{user.DownloadsCount}}</td>
                                        <td>
                                            <div class="btn-group dropup" ng-if="user.Downloads && user.Downloads.length > 0">
                                                <span class="dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                   {{user.FirstDownload | date:'dd/MM/yyyy'}} <span class="glyphicon glyphicon-info-sign"></span>
                                                </span>
                                                <div class="dropdown-menu dropdown-menu-right inTable">
                                                    <p><strong>Downloads</strong></p>
                                                    <table class="table" style="margin:0;">
                                                        <tr ng-repeat="dwn in user.Downloads track by $index">
                                                            <td>{{$index + 1}}</td><td>{{dwn.CreatedOn | date:'dd/MM/yyyy HH:mm:ss'}}</td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>    
                                        </td>
                                        <td>
                                            <div class="btn-group dropup" ng-if="user.Downloads && user.Downloads.length > 0">
                                                <span class="dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                   {{user.LastDownload | date:'dd/MM/yyyy'}} <span class="glyphicon glyphicon-info-sign"></span>
                                                </span>
                                                <div class="dropdown-menu dropdown-menu-right inTable">
                                                    <p><strong>Downloads</strong></p>
                                                    <table class="table" style="margin:0;">
                                                        <tr ng-repeat="dwn in user.Downloads track by $index">
                                                            <td>{{$index + 1}}</td><td>{{dwn.CreatedOn | date:'dd/MM/yyyy HH:mm:ss'}}</td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>    
                                        </td>
                                        <td>&nbsp;</td>
									</tr>
								</tbody>
								<tfoot ng-show="MAX_ITERATION_USER < ListaUtentiReposFiltered.length && MAX_ITERATION_USER < ListaUtentiRepos.length">
									<tr class="itemUsersCDoc"
                                        ng-click="showMoreUser(user)">
										<td colspan="7"><div
												ng-click="showMoreUserItems()"
												class="boxNotice boxNotice-warning withAction">Get more <i class="triangle">&nbsp;</i></div>
										</td>
									</tr>
								</tfoot>
							</table>
						</div>
				</div>
			</div>
			<div
				ng-show="MAX_ITERATION_DOC < ListaDocuReposFiltered.length && MAX_ITERATION_DOC < ListaDocuRepos.length"
				ng-click="showMoreDocItems()"
				class="boxNotice boxNotice-warning withAction"
				>Get more <i class="triangle">&nbsp;</i></div>
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
