<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="VoidAPI.aspx.vb" Inherits="Comunita_OnLine.VoidAPI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
    Scheduling Tool
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
    <!-- Service Localization (.js) -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript">
		var QueryString = function () {
		  var query_string = {};
		  var query = window.location.search.substring(1);
		  var vars = query.split("&");
		  for (var i=0;i<vars.length;i++) {
			var pair = vars[i].split("=");
			if (typeof query_string[pair[0]] === "undefined") {
			  query_string[pair[0]] = decodeURIComponent(pair[1]);
			} else if (typeof query_string[pair[0]] === "string") {
			  var arr = [ query_string[pair[0]],decodeURIComponent(pair[1]) ];
			  query_string[pair[0]] = arr;
			} else {
			  query_string[pair[0]].push(decodeURIComponent(pair[1]));
			}
		  } 
		  return query_string;
		}();
		jQuery(document).ready(function(){
			if(QueryString && QueryString.id && QueryString.idd){
				var href = "https://demo.elearning-center.it/Site/SchedulingTool/Poll/" + QueryString.id + "/" + QueryString.idd + "?BehaviorMode=iframe";
				document.getElementById("iframeCMS").src = href;
			}
		});
		function setIframeHeight(){
			/* not work *
			var iFrameEl = document.getElementById('idIframe');
			if(iFrameEl) {
				iFrameEl.height = "";
				iFrameEl.height = iFrameEl.contentWindow.document.body.scrollHeight + "px";
			}  
			*/
		}
	</script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Scheduling Tool<%--=GetLocalization("Title.test.text")--%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="hide">
        <asp:Literal runat="server" ID="LTlocalizationService">APItest</asp:Literal>
        <asp:Literal runat="server" ID="LTlocalizationValue"></asp:Literal>
    </div>    
    <div>		
		<iframe id="iframeCMS" src="https://demo.elearning-center.it/Site/SchedulingTool/Index/14?BehaviorMode=iframe" onload="setIframeHeight()" frameborder="0" height="790" width="100%"></iframe>
    </div>
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
