Type.registerNamespace("Telerik.Web.UI");
(function(){var a=$telerik.$;
var b=Telerik.Web.UI;
var c="Mobile";
b.DialogDefinition=function(){this.Width="600px";
this.Height="400px";
this.Title="";
this.Behaviors=36;
this.Modal=true;
this.VisibleStatusbar=false;
this.VisibleTitlebar=true;
this.ClientCallbackFunction="";
};
b.DialogDefinition.registerClass("Telerik.Web.UI.DialogDefinition",null);
b.DialogDefinitionsDictionary=function(g){for(var f in g){var h=g[f];
var d=new b.DialogDefinition();
for(var e in h){d[e]=h[e];
}this[f]=d;
}};
b.DialogDefinitionsDictionary.registerClass("Telerik.Web.UI.DialogDefinitionsDictionary",null);
b.DialogOpenEventArgs=function(e,d){b.DialogOpenEventArgs.initializeBase(this);
this._dialogName=e;
if(d){this._parameters=d;
}else{this._parameters={};
}};
b.DialogOpenEventArgs.prototype={get_dialogName:function(){return this._dialogName;
},set_parameters:function(d){this._parameters=d;
},get_parameters:function(){return this._parameters;
}};
b.DialogOpenEventArgs.registerClass("Telerik.Web.UI.DialogOpenEventArgs",Sys.EventArgs);
b.RadDialogOpener=function(d){b.RadDialogOpener.initializeBase(this,[d]);
this._dialogDefinitions={};
this._handlerChecked=false;
this._dialogParametersProviderTypeName="";
this._dialogUniqueID="";
this._dialogContainers={};
};
b.RadDialogOpener.prototype={initialize:function(){b.RadDialogOpener.callBaseMethod(this,"initialize");
this._dialogDefinitions=new b.DialogDefinitionsDictionary(this.get_dialogDefinitions());
},get_dialogDefinitions:function(){return this._dialogDefinitions;
},openUrl:function(p,d,q,j,g,f,o,k,e,m,n,i){i="EXTERNAL_URL"+(i||"default");
var h=this._getDialogContainer(i);
h.set_width(q+"px");
h.set_height(j+"px");
h.set_behaviors(e||b.WindowBehaviors.Default);
h.set_modal(k==true);
h.set_visibleStatusbar(m==true);
h.set_visibleTitlebar(n==true);
h.set_title(o?o:"");
h.set_keepInScreenBounds(true);
var l=new b.DialogOpenEventArgs(p,d);
this.raiseEvent("open",l);
h.ClientParameters=d;
h.set_clientCallBackFunction(g);
h.setUrl(p);
h.show();
h.center();
window.setTimeout(function(){h.setActive(true);
},100);
},open:function(h,e,d){if(!this._handlerChecked){this._checkDialogHandler(this.get_handlerUrl());
}h=this._getExistingPrefixedDialogName(h);
var g=this._getDialogDefinition(h);
var j=new b.DialogOpenEventArgs(h,e);
this.raiseEvent("open",j);
e=j.get_parameters();
if(!d){d=g.ClientCallbackFunction;
}var f;
if(this.get_useClassicDialogs()){f=$create(b.ClassicDialog,{dialogOpener:this});
f.ClientParameters=e;
this._applyParameters(h,f);
if(d){f.set_clientCallBackFunction(d);
}window.__getCurrentRadEditorRadWindowReference=function(){return f;
};
var k=this._openBrowserWindow(f,g,h);
k.radWindow=f;
return k;
}else{f=this._getDialogContainer(h);
var i=g.Height||0;
if(!f.get_popupElement()){f.set_height(i);
f.set_width(g.Width||0);
f.set_behaviors(g.Behaviors);
f.set_modal(g.Modal);
f.set_visibleStatusbar(g.VisibleStatusbar);
f.set_visibleTitlebar(g.VisibleTitlebar);
f.set_keepInScreenBounds(true);
}if(g.ReloadOnShow!=null){f.set_reloadOnShow(g.ReloadOnShow);
}f.ClientParameters=e;
this._applyParameters(h,f);
if(h!="SpellCheckDialog"){this._sizeCenterCellDelegate=Function.createDelegate(this,this._sizeCenterCellWindowLoadHandler);
this._autoSizeDialogDelegate=Function.createDelegate(this,this._autoSizeDialogWindowLoadHandler);
f.add_pageLoad(this._sizeCenterCellDelegate);
f.add_pageLoad(this._autoSizeDialogDelegate);
}if(d){f.set_clientCallBackFunction(d);
}this._showDialogContainer(f);
if(!f.get_popupElement()){f.set_height(i);
}window.setTimeout(function(){if(f.isVisible()){f.center();
f.setActive(true);
}},100);
}},openLight:function(g,e,d){var h=195;
var k=350;
if(e){if(e.height){h=e.height;
}if(e.width){k=e.width;
}}var i=new b.DialogOpenEventArgs(g,e);
this.raiseEvent("open",i);
e=i.get_parameters();
var f=this._getDialogContainer(g);
f.set_height(h);
f.set_width(k);
f.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move+Telerik.Web.UI.WindowBehaviors.Close);
f.set_modal(true);
f.set_visibleStatusbar(false);
f.set_visibleTitlebar(true);
if(d){f.set_clientCallBackFunction(d);
}f.ClientParameters=e;
Telerik.Web.UI.LightDialogsControllerClass.initializeLightDialog(f);
this._showDialogContainer(f);
f.set_height(h);
f.center();
if(e.stripPopupHeight===true){var j=f.get_popupElement();
if(j){j.style.height="";
}}window.setTimeout(function(){f.setActive(true);
},100);
},_openBrowserWindow:function(d,e,f){var g="width="+parseInt(e.Width,10)+",height="+parseInt(e.Height,10);
g+=",resizable=0,scrollbars=0,status=0,toolbar=0,menubar=0,directories=0";
return d.open(g,f);
},_showDialogContainer:function(d){d.show();
if(this.get_renderMode()==b.RenderMode.Mobile){d.set_modal(false);
d.maximize();
}},_sizeCenterCellWindowLoadHandler:function(d){if(this._sizeCenterCellDelegate){d.remove_pageLoad(this._sizeCenterCellDelegate);
delete this._sizeCenterCellDelegate;
}if(typeof(Telerik)=="undefined"){return;
}var e=function(j){j.remove_show(e);
var h=j.get_contentFrame(),g=h.contentWindow.document||h.contentDocument,f=j.get_contentElement()||j.ui.contentCell||j.ui.content,i=new RegExp("height\\s*:\\s*"+g.body.offsetHeight+"px(\\s*;)?","gmi");
f.style.height=g.body.offsetHeight+"px";
f.style.cssText=f.style.cssText.replace(i,"height: "+g.body.offsetHeight+"px !important;");
};
if(d&&d.isVisible&&d.isVisible()){e(d);
}else{if(d&&d.isVisible&&!d.isVisible()){d.add_show(e);
}}},_autoSizeDialogWindowLoadHandler:function(e){if(this._autoSizeDialogCellDelegate){e.remove_pageLoad(this._autoSizeDialogDelegate);
delete this._autoSizeDialogCellDelegate;
}if(typeof(Telerik)=="undefined"){return;
}var d=function(f){if($telerik.quirksMode){return;
}f.remove_show(d);
f.set_width(20);
f.set_height(20);
f.autoSize();
f.center();
};
if(e&&e.isVisible&&e.isVisible()){d(e);
}else{if(e&&e.isVisible&&!e.isVisible()){e.add_show(d);
}}},_applyParameters:function(h,e){var j=this._getDialogParameters(h);
if(!j){return;
}var m="&dp="+encodeURIComponent(j);
var d=this._getBaseDialogUrl(h);
var n=d.length+m.length;
var k=this._dialogParametersProviderTypeName=="";
var l=k&&n<=this.get_dialogUrlLengthLimit();
if(l){var g=e.get_navigateUrl();
var o=d+m;
if(g!=o){if(this.get_useClassicDialogs()||e.isCreated()){e.setUrl(o);
}else{e.set_navigateUrl(o);
}}else{var f=e.get_contentFrame();
if(f&&f.contentWindow&&f.contentWindow.$find){var i=f.contentWindow.initDialog;
if(i){f.contentWindow.setTimeout(function(){i();
},1);
}}}}else{e.setUrl(d);
e.DialogParameters=j;
}},_closeContainerDelegate:function(d){this.raiseEvent("close",d);
},_getDialogContainer:function(d){if(typeof(this._dialogContainers[d])=="undefined"){var e=$find(this.get_id()+d);
if(null!=e){e.dispose();
}this._dialogContainers[d]=this.get_container().clone(this.get_id()+d);
var f=this;
this._dialogContainers[d].get_dialogOpener=function(){return f;
};
this._dialogContainers[d].add_close(Function.createDelegate(this,this._closeContainerDelegate));
}return this._dialogContainers[d];
},_getBaseDialogUrl:function(d){var e=this.get_handlerUrl().indexOf("?")<0?"?":"&";
var f=this.get_handlerUrl()+e+"DialogName="+d;
if(this.get_enableTelerikManagers()){f+="&UseRSM=true";
}f+="&renderMode="+this._renderMode;
f+="&Skin="+this.get_skin()+"&Title="+encodeURIComponent(this._getDialogDefinition(d)["Title"])+"&doid="+this._dialogUniqueID+"&dpptn="+encodeURIComponent(this._dialogParametersProviderTypeName)+this.get_additionalQueryString();
return f;
},_getDialogDefinition:function(e){var d=this.get_dialogDefinitions()[e];
if(d){return d;
}else{throw Error.argumentNull("dialogName",String.format("Dialog Parameters for the {0} dialog do not exist",e));
}},_getDialogParameters:function(d){return this._getDialogDefinition(d)["SerializedParameters"];
},_getExistingPrefixedDialogName:function(e){var f=this._prefixDialogName(e);
var d=this.get_dialogDefinitions();
if(d[f]){return f;
}else{return e;
}},_prefixDialogName:function(d){var e=(this._renderMode==b.RenderMode.Mobile);
var f=e?c:"";
if(d.indexOf(f)===0){return d;
}else{return f+d;
}},_checkDialogHandler:function(g){var f=g.indexOf("?")<0?"?":"&";
var e=g+f+"checkHandler=true";
var h=new Sys.Net.WebRequest();
h.set_url(e);
h.add_completed(Function.createDelegate(this,this._checkRequestCompleted));
var d=new Sys.Net.XMLHttpExecutor();
h.set_executor(d);
d.executeRequest();
},_checkRequestCompleted:function(e,d){if(e.get_responseAvailable()){var f=e.get_responseData();
if(f&&f.indexOf("HandlerCheckOK")>0){this._handlerChecked=true;
return;
}}window.alert("Web.config registration missing!\n The Telerik dialogs require a HttpHandler registration in the web.config file. Please, use the control's Smart Tag to add the handler automatically, or see the help for more information: Controls > RadEditor > Dialogs > Introduction");
}};
a.registerControlProperties(b.RadDialogOpener,{additionalQueryString:"",enableTelerikManagers:false,handlerUrl:"",container:null,dialogUrlLengthLimit:2000,useClassicDialogs:false,skin:""});
a.registerControlEvents(b.RadDialogOpener,["open","close"]);
b.RadDialogOpener.registerClass("Telerik.Web.UI.RadDialogOpener",b.RadWebControl);
b.ClassicDialog=function(e,d){b.ClassicDialog.initializeBase(this);
this.BrowserWindow=window;
this._dialogOpener=null;
this._clientCallBackFunction=null;
this._window=null;
this._url="";
this._events={close:[],beforeClose:[],show:[]};
};
b.ClassicDialog.prototype={close:function(d){this.raiseEvent("beforeClose");
this.raiseEvent("close");
if(null!=d&&!(d instanceof Sys.UI.DomEvent)){var e=this.get_clientCallBackFunction();
if(typeof(e)=="string"){e=eval(e);
}if(e){e(this,d);
}}var f=this.get_contentFrame();
f.setTimeout(function(){f.close();
f.parent.focus();
},100);
},open:function(e,d){this._window=window.open(this.get_navigateUrl(),d,e);
this._window.focus();
if(!this._window.contentWindow){this._window.contentWindow=this._window;
}this.raiseEvent("show");
return this._window;
},get_dialogOpener:function(){return this._dialogOpener;
},set_dialogOpener:function(d){this._dialogOpener=d;
},get_clientCallBackFunction:function(){return this._clientCallBackFunction;
},set_clientCallBackFunction:function(d){this._clientCallBackFunction=d;
},setUrl:function(d){this._url=d;
},get_navigateUrl:function(){return this._url;
},get_contentFrame:function(){return this._window;
},set_title:function(e){if(this._window&&this._window.document){var d=this._window.document;
d.title=e;
}},dispose:function(){this._window=null;
this._clientCallBackFunction=null;
this._dialogOpener=null;
this._events=null;
b.ClassicDialog.callBaseMethod(this,"dispose");
},add_close:function(d){Array.add(this._events.close,d);
},remove_close:function(d){Array.remove(this._events.close,d);
},add_show:function(d){Array.add(this._events.show,d);
},remove_show:function(d){Array.remove(this._events.show,d);
},add_beforeClose:function(d){Array.add(this._events.beforeClose,d);
},remove_beforeClose:function(d){Array.remove(this._events.beforeClose,d);
},raiseEvent:function(e,d){var f=this._events[e];
this._raiseEvent(f,d);
},_raiseEvent:function(e,d){if(!e||(e.length===0)){return;
}e=Array.clone(e);
if(!e._handler){e._handler=function(k,g){for(var h=0,j=e.length;
h<j;
h++){e[h](k,g);
}};
}var f=e._handler;
if(f){if(!d){d=Sys.EventArgs.Empty;
}f(this,d);
}}};
b.ClassicDialog.registerClass("Telerik.Web.UI.ClassicDialog",Sys.Component);
})();
Type.registerNamespace("Telerik.Web.UI");
if(typeof(Telerik.Web.UI.EditorCommandEventArgs)=="undefined"){Telerik.Web.UI.EditorCommandEventArgs=function(a,b,c){Telerik.Web.UI.EditorCommandEventArgs.initializeBase(this);
this._name=this._commandName=a;
this._tool=b;
this._value=c;
this.value=c;
this._callbackFunction=null;
};
Telerik.Web.UI.EditorCommandEventArgs.prototype={get_name:function(){return this._name;
},get_commandName:function(){return this._commandName;
},get_tool:function(){return this._tool;
},get_value:function(){return this._value;
},set_value:function(a){this.value=a;
this._value=a;
},set_callbackFunction:function(a){this._callbackFunction=a;
}};
Telerik.Web.UI.EditorCommandEventArgs.registerClass("Telerik.Web.UI.EditorCommandEventArgs",Sys.CancelEventArgs);
}