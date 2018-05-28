<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="VoidAPI.aspx.vb" Inherits="Comunita_OnLine.VoidAPI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
    Statistiche MoocsStats
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
    <!-- Service Localization (.js) -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- Head Content: .ccs, .js -->
    <link href="css/statMoocsStats.css" rel="stylesheet" />
    <script src="../Resources/frontendCustom/Utils.js"></script>
	
    <link href="../Resources/frontendFrameworks/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../Resources/frontendFrameworks/js/bootstrap.min.js"></script>

    <script src="../Resources/frontendFrameworks/js/angular.min.js"></script>
    <script src="js/appMoocsStats.js"></script>
    <% If isForIframe Then %>
	    <link rel="Stylesheet" href="../Resources/Styles/forIframeRemoveBroder.css" />	           
    <% End If%>
    <style type="text/css">
        .AdvSpacer{
            height:177px;
        }
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
		.list-group.simil-calendar .list-group-item{
            display:inline-block;
            width:54px;
            height:30px;
            line-height:30px;
            background-color:#fff;
            border-radius:0;
			float: right;
			padding: 0;
			text-align: center;
			font-size: 10px;
		}
        .list-group.simil-calendar .list-group-item.active{
            display:inline-block;
            height:30px;
            line-height:30px;
            background-color: #dde4be;
            border-color: #A8B85A;
            color: #333;
		}
		.list-group.stars-list .list-group-item{
			display:inline-block;
            padding: 0 10px;
		}

        #divMatsterMoocsStats h3,
        #divMatsterMoocsStats h4,
        #divMatsterMoocsStats h5{
            color:#555;
            font-size:1.5em;
        }
	</style>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Statistiche Moocs<%--=GetLocalization("Title.test.text")--%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="hide">
        <asp:Literal runat="server" ID="LTlocalizationService">APItest</asp:Literal>
         <asp:Literal runat="server" ID="LTlocalizationValue"></asp:Literal>
    </div>    	
	<script type="text/javascript">
	    jQuery("body").attr("ng-app", "appMoocsStats");
	    jQuery("body").attr("ng-controller", "MasterCtrl");
	</script>
    <div ng-if="PermissionsError" class="alert alert-danger clearfix">
        <strong>Error:</strong> Forbidden <a href="/" class="btn right btn-danger"><i class="glyphicon glyphicon-refresh"></i> Refresh login</a>
    </div>
	<div id="divMatsterMoocsStats">        
		<div id="boxMoocsStats" ng-cloak ng-if="!PermissionsError">
            <div ng-show="loadingAttivitaComunitaResources || loadingMoocsInfoResources" class="alert alert-info clearfix">
                <i class="glyphicon glyphicon-refresh rotationON"></i> Loading...
            </div>
            <div id="box-attivita">
                <div class="row">
                    <div class="box-adv-info adv-col-md-6" ng-if="moocsAttivitaInfo && moocsAttivitaInfo.DatesList">
                        <h4>Costanza participazione (ultimi 30g)</h4>
                        <div ng-if="moocsAttivitaInfo && moocsAttivitaInfo.DatesList && moocsAttivitaInfo.DatesList.length && moocsAttivitaInfo.DatesList.length > 0">
                            <br />
                            <div>
                                <span class="btn btn-primary" style="width:50%">
                                    <strong>Ultimo accesso</strong><br />
                                    {{ moocsAttivitaInfo.DatesList[(moocsAttivitaInfo.DatesList.length -1)] | date:'dd/MM/yyyy' }}
                                </span>
                                <span class="btn btn-primary" style="width:49%">
                                    <strong>Ultima Striscia</strong><br />
                                    {{ calcolareStriciaCorrente() }} Giorni
                                </span>
                            </div>
                            <br />
                        </div>
                        <ul class="list-group simil-calendar">
                            <li ng-repeat="d in last30days" class="list-group-item {{CompareDates(d)}}" title="{{d | date:'dd/MM/yyyy'}}">{{d | date:'dd/MM'}}</li>
                        </ul>
                    </div>
					<div class="box-adv-info adv-col-md-6">
                        <h4>Progressi</h4>
                        <ul class="list-group mookCompletition">
                            <li class="list-group-item" ng-repeat="mooc in moocsInfo.Moocs" ng-show="mooc.Info.mType == 2">
                                <h5>{{mooc.PathName}}</h5> 
                                <span class="completitionBar {{GetMoocsStatus(mooc.Info)}}">{{mooc.Info.Completion}}% (min {{mooc.Info.MinCompletion}}%)</span>
                            </li>
                        </ul>
                        <div ng-if="moocsInfo && moocsInfo.Moocs && moocsInfo.Moocs.length == 0" class="alert alert-info">Non c'è attività in questo mooc</div>
                    </div>
                </div>
            </div>
            <div id="box-moocsstat" ng-if="moocsInfo && moocsInfo.Moocs">
                <div class="row">                    
                    <div class="box-adv-info adv-col-md-6" ng-if="moocsModuleAction && moocsModuleAction.Actions">
                        <h4>Attività recenti</h4>
                        <ul class="list-group">
                            <li class="list-group-item" ng-repeat="action in moocsModuleAction.Actions"><strong>{{action.CommunityName}} - {{action.ServiceName}}</strong><br />
							<span title="{{action.LastActionDate | date:'dd/MM/yyyy HH:mm:ss'}}">{{dateDiffarence(action.LastActionDate)}}</span></li>
                        </ul>
                        <div ng-if="moocsModuleAction && moocsModuleAction.Actions && moocsModuleAction.Actions.length == 0" class="alert alert-info">Non ci sono attività recenti</div>
                    </div>
                    <div class="box-adv-info adv-col-md-6">
                        <h4>Crediti acquisiti</h4>
                        <ul class="list-group stars-list">
                            <li ng-repeat="mooc in moocsInfo.Moocs" class="list-group-item" style="border: 0 none;" ng-if="mooc.Info.mType == 2"><span class="star {{GetMoocsStatus(mooc.Info)}}">&nbsp;</span></li>
                            <li ng-show="moocsInfo.CokadeCompleted" class="list-group-item" style="border: 0 none;">
                                <div ng-show="!moocsInfo.CokadeCompletedGold" class="cokadecontainer">
                                    <div class="cokadeItem silver" style="width:60px;height:82px;">silver</div>
                                </div>
                                <div ng-show="moocsInfo.CokadeCompletedGold" class="cokadecontainer">
                                    <div class="cokadeItem gold" style="width:60px;height:82px;">gold</div>
                                </div>
                            </li>
                            <li ng-repeat="mooc in moocsInfo.Moocs" class="list-group-item" style="border: 0 none;" ng-if="mooc.Info.mType == 3"><span class="star {{GetMoocsStatus(mooc.Info)}}">&nbsp;</span></li>
                        </ul>
                        <div ng-if="moocsInfo && moocsInfo.Moocs && moocsInfo.Moocs.length == 0" class="alert alert-info">Non c'è attività in questo mooc</div>
                    </div>
                </div>
            </div>
		</div>
    </div>				
	<script type="text/javascript">
	    jQuery("#divMatsterMoocsStats .modal").appendTo("body");
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
