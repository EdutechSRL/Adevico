Type.registerNamespace("Telerik.Web.UI.Editor");
Type.registerNamespace("Telerik.Web.UI.Editor.Modules");
Telerik.Web.UI.Editor.ModulesManager=function(a){this._editor=a;
this._modules=[];
this._onEditorModeChangeDelegate=Function.createDelegate(this,this._onEditorModeChange);
};
Telerik.Web.UI.Editor.ModulesManager.prototype={initialize:function(){this.createModules();
this._editor.add_modeChange(this._onEditorModeChangeDelegate);
},getModuleByName:function(b){for(var a=0;
a<this._modules.length;
a++){if(this._modules[a].get_name()==b){return this._modules[a];
}}return null;
},createModules:function(){if(!this._editor){return;
}var b=this._editor.get_modulesJSON();
for(var a=0;
a<b.length;
a++){if(b[a].name!="RadEditorTrackChangesInfo"||this._editor.get_enableComments()||this._editor.get_enableTrackChanges()){this.createModule(b[a]);
}}},createModule:function(a){if(a.attributes){for(var f in a.attributes){a[f.toLowerCase()]=a.attributes[f];
}}var d=a.enabled;
if(false==d){return;
}a.editor=this._editor;
var h=a.name;
if(h){a.title=this._editor.getLocalizedString(h);
}var b=null;
try{b=eval("Telerik.Web.UI.Editor.Modules."+h);
}catch(c){}if(!b){try{b=eval(h);
}catch(c){}}if(b){var g=document.createElement("div");
var i=this._getModuleZone(a.dockingzone);
delete a.dockingzone;
delete a.dockable;
if(i){if(i.innerHTML=="&nbsp;"||i.innerHTML.length==1){i.innerHTML="";
}i.appendChild(g);
}this._modules[this._modules.length]=$create(b,a,null,null,g);
}},_getModuleZone:function(b){var a=this._editor.get_id();
var c=$get(a+b);
if(!c){c=$get(a+"Module");
}return c;
},_onEditorModeChange:function(d,a){var b=Telerik.Web.UI.EditModes;
var c=d.get_mode();
this.setModulesVisible((c==b.Design));
},setModulesVisible:function(a){var d=this._modules;
var b=false;
var c;
if(!a&&this._enabledModules==null){b=true;
this._enabledModules={};
for(c=0;
c<d.length;
c++){var e=d[c];
if(e.get_visible()){this._enabledModules[e.get_name()]=true;
}e.set_visible(false);
}}else{if(a&&this._enabledModules){b=true;
for(c=0;
c<d.length;
c++){var f=d[c];
var g=this._enabledModules[f.get_name()];
if(g){f.set_visible(true);
}}this._enabledModules=null;
}}if(b){this._fixIEBottomZoneDisplacement(a);
}},_fixIEBottomZoneDisplacement:function(b){if($telerik.isIE){if(!this._emptySpan){this._emptySpan=document.createElement("span");
var a=this._getModuleZone("Bottom");
if(a){var c=this._emptySpan;
c.innerHTML="&nbsp;";
c.style.display="none";
a.appendChild(c);
}}this._emptySpan.style.display=b?"none":"";
}},get_modules:function(){return this._modules;
}};
Telerik.Web.UI.Editor.ModulesManager.registerClass("Telerik.Web.UI.Editor.ModulesManager",null);
Telerik.Web.UI.Editor.Modules.ModuleBase=function(a){Telerik.Web.UI.Editor.Modules.ModuleBase.initializeBase(this,[a]);
this._editor=null;
this._name="";
this._visible=true;
this._enabled=true;
this._rendered=false;
this._enableMaxWidth=true;
this._title="";
this._className="reModule";
this._scriptFile="";
this._attributes={};
this.isSafari=$telerik.isSafari;
this.isIE=$telerik.isIE;
this.isOpera=$telerik.isOpera;
this.isFirefox=$telerik.isFirefox;
};
Telerik.Web.UI.Editor.Modules.ModuleBase.prototype={initialize:function(){Telerik.Web.UI.Editor.Modules.ModuleBase.callBaseMethod(this,"initialize");
if(this.get_visible()){this.render();
}},render:function(){var a=this.get_element();
if(a){a.className=this._className;
}this._rendered=true;
},toggleVisibility:function(){this.set_visible(!this.get_visible());
},attachEventHandler:function(c,b){var a=this.get_editor();
if(a){a.attachEventHandler(c,b);
}},_getLocalizedString:function(b,a){return this._editor.getLocalizedString(b,a);
},get_editor:function(){return this._editor;
},set_editor:function(a){this._editor=a;
},get_attributes:function(){return this._attributes;
},set_attributes:function(a){this._attributes=a;
},get_scriptFile:function(){return this._scriptFile;
},set_scriptFile:function(a){this._scriptFile=a;
},get_visible:function(){var a=this.get_element();
if(!a){return false;
}return(a.style.display!="none");
},set_visible:function(a){if(a&&!this._rendered){this.render();
}var b=this.get_element();
b.style.display=a?"":"none";
},get_enabled:function(){return this._enabled;
},set_enabled:function(a){this._enabled=a;
},get_title:function(){return this._title;
},set_title:function(a){this._title=a;
},get_name:function(){return this._name;
},set_name:function(a){this._name=a;
},dispose:function(){Telerik.Web.UI.Editor.Modules.ModuleBase.callBaseMethod(this,"dispose");
}};
Telerik.Web.UI.Editor.Modules.ModuleBase.registerClass("Telerik.Web.UI.Editor.Modules.ModuleBase",Sys.UI.Control);
Telerik.Web.UI.Editor.Modules.RadEditorDomInspector=function(a){Telerik.Web.UI.Editor.Modules.RadEditorDomInspector.initializeBase(this,[a]);
};
Telerik.Web.UI.Editor.Modules.RadEditorDomInspector.prototype={initialize:function(){this._onSelectionChangeDelegate=Function.createDelegate(this,this.showDomPath);
this._editorPathArray=[];
this._removeElementString=this._getLocalizedString("DomInspectorRemoveElement","Remove Element");
Telerik.Web.UI.Editor.Modules.RadEditorDomInspector.callBaseMethod(this,"initialize");
},dispose:function(){this.clear();
this._registerMouseHandlers(false);
Telerik.Web.UI.Editor.Modules.RadEditorDomInspector.callBaseMethod(this,"dispose");
},render:function(){Telerik.Web.UI.Editor.Modules.RadEditorDomInspector.callBaseMethod(this,"render");
this.clear();
this.get_editor().add_selectionChange(this._onSelectionChangeDelegate);
this._registerMouseHandlers(true);
this.showDomPath();
},_registerMouseHandlers:function(a){var c=this.get_element();
if(true==a){var b={click:this._onMouseClick,mouseover:this._onMouseOver,mouseout:this._onMouseOut};
$addHandlers(c,b,this);
}else{if(c){$clearHandlers(c);
}}},_onMouseOver:function(b){var a=this._getReferredEditorElement(b);
if(!a||this._isSelectedElement(a)){return;
}try{Sys.UI.DomElement.addCssClass(a,"RadEDomMouseOver");
}catch(b){}},_onMouseOut:function(b){var a=this._getReferredEditorElement(b);
if(!a){return;
}try{Sys.UI.DomElement.removeCssClass(a,"RadEDomMouseOver");
if(""==a.className){a.removeAttribute("className",0);
a.removeAttribute("class",0);
}}catch(b){}},_onMouseClick:function(a){var d=a.target;
if(!d||d.tagName!="A"){return null;
}if(d.innerHTML==this._removeElementString){var b=this._editorPathArray[0];
this.removeSelectedElement(b);
}else{var c=this._getReferredEditorElement(a);
this.selectElement(c);
}return $telerik.cancelRawEvent(a);
},clear:function(){this.get_element().innerHTML="&nbsp;";
this._editorPathArray=[];
},_createRemoveLink:function(){var a=document.createElement("a");
a.innerHTML=this._removeElementString;
a.href="javascript:void(0)";
a.className="reModule_domlink";
this.get_element().appendChild(a);
},addDomCouple:function(a,d){if(!a||!a.tagName){return;
}var e=this.get_element();
var b=document.createElement("a");
b.oncontextmenu=$telerik.cancelRawEvent;
b.href="javascript:void(0);";
b.innerHTML=a.tagName;
b.className=d?"reModule_domlink_selected ":"reModule_domlink";
e.appendChild(b);
var c=document.createElement("span");
c.innerHTML="&nbsp;> ";
e.appendChild(c);
},_getPathArray:function(c,b){var a=[];
if(null!=c&&c.nodeType==3){c=c.parentNode;
}while(c!=b&&null!=c){a[a.length]=c;
c=c.parentNode;
}return a;
},_isSelectedElement:function(b){var a=this._editorPathArray;
if(a&&a[0]==b){return true;
}},_getReferredEditorElement:function(a){var g=a.target;
if(!g||g.tagName!="A"){return null;
}var d=this.get_element().getElementsByTagName("A");
var c=-1;
for(var b=0;
b<d.length;
b++){if(d[b]==g){c=b;
break;
}}if(c>-1){var f=this._editorPathArray.concat([]).reverse();
return f[c];
}},showDomPath:function(){if(!this.get_visible()){return;
}try{var c=this.get_editor().getSelectedElement();
if(!c){return;
}var b=this.get_editor().get_contentArea();
if(this.isIE&&!b.contains(c)){return;
}this.clear();
this._editorPathArray=this._getPathArray(c,b);
var a=this._editorPathArray;
for(var e=a.length-1;
e>=0;
e--){this.addDomCouple(a[e],(e==0));
}if(a.length>0){this._createRemoveLink();
}}catch(d){}},selectElement:function(a){try{this._selectedElement=a;
this.get_editor().selectElement(a);
this._selectedElement=null;
}catch(b){}},removeSelectedElement:function(b){try{var a;
if(b.tagName=="TD"||b.tagName=="TH"){this.get_editor().fire("DeleteCell");
}else{if(b.tagName=="TR"){this.get_editor().fire("DeleteRow");
}else{if(b.tagName=="TABLE"||b.tagName=="TBODY"||b.tagName=="THEAD"||b.tagName=="TFOOT"||b.tagName=="EMBED"||b.tagName=="OBJECT"||b.tagName=="INPUT"||b.tagName=="IMG"||b.tagName=="HR"){a=new Telerik.Web.UI.Editor.GenericCommand(this._removeElementString,this.get_editor().get_contentWindow(),this.get_editor());
var d=b.parentNode;
d.removeChild(b);
this.get_editor().setFocus();
this.get_editor().executeCommand(a);
}else{if(b.tagName!="BODY"){a=new Telerik.Web.UI.Editor.GenericCommand(this._removeElementString,this.get_editor().get_contentWindow(),this.get_editor());
Telerik.Web.UI.Editor.Utils.removeNode(b);
this.get_editor().setFocus();
this.get_editor().executeCommand(a);
}}}}}catch(c){}this.get_editor().raiseEvent("selectionChange");
}};
Telerik.Web.UI.Editor.Modules.RadEditorDomInspector.registerClass("Telerik.Web.UI.Editor.Modules.RadEditorDomInspector",Telerik.Web.UI.Editor.Modules.ModuleBase);
Telerik.Web.UI.Editor.Modules.RadEditorHtmlInspector=function(a){Telerik.Web.UI.Editor.Modules.RadEditorHtmlInspector.initializeBase(this,[a]);
};
Telerik.Web.UI.Editor.Modules.RadEditorHtmlInspector.prototype={initialize:function(){this._onSelectionChangedDelegate=Function.createDelegate(this,this._onSelectionChanged);
this._intervalDelegate=Function.createDelegate(this,this.updateEditorContent);
this._textarea=null;
Telerik.Web.UI.Editor.Modules.RadEditorHtmlInspector.callBaseMethod(this,"initialize");
},dispose:function(){if(this._textarea){this._textarea.value="";
}this._clearInterval();
Telerik.Web.UI.Editor.Modules.RadEditorHtmlInspector.callBaseMethod(this,"dispose");
},_clearInterval:function(){if(this._interval){window.clearInterval(this._interval);
}},set_visible:function(a){Telerik.Web.UI.Editor.Modules.RadEditorHtmlInspector.callBaseMethod(this,"set_visible",[a]);
if(a){this._interval=window.setInterval(this._intervalDelegate,4000);
this._onSelectionChanged();
}else{this._clearInterval();
}},render:function(){Telerik.Web.UI.Editor.Modules.RadEditorHtmlInspector.callBaseMethod(this,"render");
var b=document.createElement("textarea");
b.style.width="99%";
b.className="reTextarea";
b.setAttribute("rows","10");
b.setAttribute("cols","80");
this._textarea=b;
if(!this.isIE){b.onclick=new Function("this.focus();");
}var a=this.get_element();
a.appendChild(b);
this.get_editor().add_selectionChange(this._onSelectionChangedDelegate);
},updateEditorContent:function(){if(!this.get_visible()){return;
}var a=this._textarea.value;
var b=this.get_editor().get_contentArea().innerHTML;
if(a==this._oldContent||a==b){return;
}this._oldContent=a;
this._updateFlag=true;
this.get_editor().set_html(a,this._getLocalizedString("Typing"),false);
this._textarea.focus();
},_onSelectionChanged:function(){if(this._updateFlag){this._updateFlag=false;
return;
}this._textarea.value=this.get_editor().get_contentArea().innerHTML;
}};
Telerik.Web.UI.Editor.Modules.RadEditorHtmlInspector.registerClass("Telerik.Web.UI.Editor.Modules.RadEditorHtmlInspector",Telerik.Web.UI.Editor.Modules.ModuleBase);
Telerik.Web.UI.Editor.Modules.RadEditorNodeInspector=function(a){Telerik.Web.UI.Editor.Modules.RadEditorNodeInspector.initializeBase(this,[a]);
this._updateMainPanelDelegate=Function.createDelegate(this,this._updateMainPanel);
this._onToolValueSelectedDelegate=Function.createDelegate(this,this._onToolValueSelected);
this._onDropDownBeforeShowDelegate=Function.createDelegate(this,this._onDropDownBeforeShow);
this._tools={};
this._toolNames={};
this._selectedElement=null;
};
Telerik.Web.UI.Editor.Modules.RadEditorNodeInspector.prototype={_nodeAttributesArray:{TABLE:["width","borderColor","cellSpacing","bgColor","className","SetTableProperties","height","cellPadding","align","border"],TH:["width","borderColor","bgColor","className","SetCellProperties","height","align","noWrap","border"],TD:["width","borderColor","bgColor","className","SetCellProperties","height","align","noWrap","border"],TR:["width","className","height"],A:["href","className","LinkManager","title","target"],IMG:["width","borderColor","className","SetImageProperties","height","align","border","alt","title"],INPUT:["NAME","width","height","id","title","className","value"],FORM:["className","width","height","NAME","action","id"],TEXTAREA:["className","width","height","NAME","id","rows","cols"]},_nodeInspectorAttributesArray:[["rows","NAME","width","cellSpacing","borderColor","href","alt","align","value","target","SetTableProperties","SetCellProperties","LinkManager"],["cols","id","height","action","cellPadding","border","bgColor","title","noWrap","className","SetImageProperties"]],initialize:function(){Telerik.Web.UI.Editor.Modules.RadEditorNodeInspector.callBaseMethod(this,"initialize");
this.get_editor().add_selectionChange(this._updateMainPanelDelegate);
this._invalidValueString=this._getLocalizedString("NodeInspectorInvalidValue","Invalid value. Please enter a number.");
},render:function(){Telerik.Web.UI.Editor.Modules.RadEditorNodeInspector.callBaseMethod(this,"render");
var a=this.get_element();
a.style.height="50px";
},get_skin:function(){return this._editor.get_skin();
},getNamedCssForSelectedElement:function(a){return this.get_editor().getCssArray(a);
},dispose:function(){this._tools=[];
this._mainPanel=null;
Telerik.Web.UI.Editor.Modules.RadEditorNodeInspector.callBaseMethod(this,"dispose");
},_createMainPanel:function(){var b=this._tools;
var f=this._nodeInspectorAttributesArray;
var g=document.createElement("table");
g.border=0;
g.cellSpacing=0;
g.cellPadding=0;
for(var k=0;
k<f.length;
k++){var c=f[k];
var j=g.insertRow(-1);
for(var d=0;
d<c.length;
d++){var e=c[d];
var h=j.insertCell(-1);
h.style.display="none";
h.setAttribute("controlName",e);
h.innerHTML=this._getLocalizedString(e,e);
h.className="reModuleLabel";
h=j.insertCell(-1);
h.style.display="none";
h.setAttribute("controlHolder",e);
var a=this._getControlByName(e);
if(a){b[e]=a;
h.appendChild(a.get_element());
}}}return g;
},_updateMainPanel:function(){if(!this.get_visible()){return;
}if(!this._isMainCreated){this._mainPanel=this._createMainPanel();
this._mainPanel.style.display="none";
this.get_element().appendChild(this._mainPanel);
this._isMainCreated=true;
}var c=this.get_editor();
var f=c.getSelectedElement();
if(!f||Telerik.Web.UI.Editor.Utils.isEditorContentArea(f)||f.ownerDocument!=c.get_document()){this._mainPanel.style.display="none";
return;
}if(f.tagName=="TBODY"&&this.isOpera){f=f.parentNode;
}var a=this._nodeAttributesArray[f.tagName];
if(!a){var d=Telerik.Web.UI.Editor.Utils.getElementParentByTag(f,"A");
if(!d){d=Telerik.Web.UI.Editor.Utils.getElementParentByTag(f,"TD");
}if(!d){d=Telerik.Web.UI.Editor.Utils.getElementParentByTag(f,"TH");
}if(d){f=d;
}else{this._mainPanel.style.display="none";
return;
}}var g=null;
if(this._selectedElement){try{g=this._selectedElement.tagName;
}catch(b){}}if(!this._selectedElement||(g!=f.tagName)){this._tools.align.setTagName(f.tagName);
}this._selectedElement=f;
this._updateControlValues(this._selectedElement);
this._mainPanel.style.display="";
},_arrayValueExists:function(b,a){return Array.contains(a,b);
},_issValidAttribValue:function(b){if(null==b){return false;
}b=b.trim();
if(""==b){return true;
}var a=parseInt(b);
if(isNaN(a)){return false;
}return true;
},_onDropDownBeforeShow:function(f,a){var c=this.get_editor();
var h=f.get_name();
var b=f.get_items();
if(b&&b.length>0){return;
}var d=null;
switch(h){case"className":var e=this._selectedElement;
var g=e&&e.tagName?e.tagName:"";
d=c.getCssArray(g);
break;
case"target":d=[["_blank",this._getLocalizedString("blank","New Window")],["_self",this._getLocalizedString("self","Same Window")],["_parent",this._getLocalizedString("parent","Parent window")],["_top",this._getLocalizedString("top","Top browser window")],["_search",this._getLocalizedString("search","Search pane")],["_media",this._getLocalizedString("media","Media pane")]];
break;
case"bgColor":case"borderColor":d=c.get_colors();
break;
}if(d){f.set_items(d);
}},_onToolValueSelected:function(b,a){if(b){if(Telerik.Web.UI.EditorDropDown.isInstanceOfType(b)||Telerik.Web.UI.EditorSpinBox.isInstanceOfType(b)||Telerik.Web.UI.EditorCheckBox.isInstanceOfType(b)||Telerik.Web.UI.EditorTextBox.isInstanceOfType(b)){this.fire(b);
}else{this.get_editor().fire(b.get_name());
}}},executeStyleRuleCommand:function(c,e,d,f){var b=this.get_editor();
var a=new Telerik.Web.UI.Editor.StyleRuleCommand(f,b.get_contentWindow(),c,e,d,b);
b.executeCommand(a);
},executeAttributeCommand:function(d,a,e,f){var c=this.get_editor();
var b=new Telerik.Web.UI.Editor.AttributeCommand(f,c.get_contentWindow(),d,a,e,c);
c.executeCommand(b);
},_updateControlValues:function(o){var f=this._nodeAttributesArray[o.tagName];
var l=this._mainPanel;
var g=this._tools;
for(var r=0;
r<l.rows.length;
r++){var p=l.rows[r];
for(var j=0;
j<p.cells.length;
j++){var n=p.cells[j];
var d=n.getAttribute("controlName");
if(d){n.style.display=this._arrayValueExists(d,f)?"":"none";
}var c=n.getAttribute("controlHolder");
if(c){n.style.display=this._arrayValueExists(c,f)?"":"none";
if("none"==n.style.display){continue;
}var e=g[c];
var b=o.getAttribute?o.getAttribute(c,2):"";
var q;
if(c=="noWrap"){var m=(o.style.whiteSpace=="nowrap")?"nowrap":"";
if(!m){m=o.noWrap;
}e.set_value(m);
}else{if(c=="border"){q=parseInt(o.style.borderWidth);
if(isNaN(q)){q="";
}e.set_value(q);
}else{if(c=="borderColor"||c=="bgColor"){if(c=="bgColor"){c="backgroundColor";
}q=o.style[c];
if(!q){q=o.getAttribute(c);
}if(q){var k=q.indexOf(")");
if(k!=-1){q=q.substring(0,k+1);
}}e.set_color(q);
}else{if(c=="align"){var u=o.tagName.toLowerCase();
switch(u){case"img":e.updateValue("",null);
var h=($telerik.isIE)?"styleFloat":"cssFloat";
var t=(typeof(o.style[h])=="undefined")?"":o.style[h];
var y=(typeof(o.style.verticalAlign)=="undefined")?"":o.style.verticalAlign;
if(y==""&&t!=""){switch(t){case"left":e.updateValue("left",null);
break;
case"right":e.updateValue("right",null);
break;
}}if(t==""){switch(y){case"top":e.updateValue("top",null);
break;
case"middle":e.updateValue("absmiddle",null);
break;
case"text-bottom":e.updateValue("bottom",null);
break;
}}break;
case"td":case"th":var w=o.style.textAlign;
var z=o.style.verticalAlign;
var a=o.getAttribute("align");
var x=o.getAttribute("vAlign");
if((w=="left"||w=="center"||w=="right")&&(z=="top"||z=="middle"||z=="bottom")){e.updateValue(w,z);
}else{if(a||x){e.updateValue(a,x);
}else{e.updateValue(null,null);
}}break;
default:e.updateValue(o.getAttribute("align"),o.getAttribute("vAlign"));
break;
}}else{if(c=="target"){var v=o.getAttribute(c);
e.updateValue(v||"");
}else{if(c=="width"||c=="height"){var s=o.style[c];
if(!s){s=o.getAttribute(c);
}if(!s&&o.nodeName=="IMG"){s=(c=="width")?o.offsetWidth:o.offsetHeight;
}e.set_value(s);
}else{if("name"==c.toLowerCase()){e.set_value(o.name);
}else{if("className"==c){if(!this.isIE){b=o.getAttribute("class");
}if(!b){b="";
}e.updateValue(b);
}else{if(b&&e.set_value){e.set_value(b);
}else{if(e.set_value){e.set_value("");
}}}}}}}}}}}}}},fire:function(i){if(!i){return;
}var e=i.get_name();
var l=this._getLocalizedString(e,e);
if(!l){l=e;
}var c,j;
var h=this._selectedElement;
if("AlignmentSelector"==e){var a=i.getAlign();
var m=i.getVAlign();
l=this._getLocalizedString("Align","Align");
var n=this._getLocalizedString("vAlign","vAlign");
var g=($telerik.isIE)?"styleFloat":"cssFloat";
var k=h.tagName.toLowerCase();
var d=false;
switch(k){case"img":switch(a){case"left":this.executeStyleRuleCommand(h,g,"left",l);
this.executeStyleRuleCommand(h,"verticalAlign","",l);
break;
case"right":this.executeStyleRuleCommand(h,g,"right",l);
this.executeStyleRuleCommand(h,"verticalAlign","",l);
break;
case"top":this.executeStyleRuleCommand(h,g,"",l);
this.executeStyleRuleCommand(h,"verticalAlign","top",l);
break;
case"bottom":this.executeStyleRuleCommand(h,g,"",l);
this.executeStyleRuleCommand(h,"verticalAlign","text-bottom",l);
break;
case"absmiddle":this.executeStyleRuleCommand(h,g,"",l);
this.executeStyleRuleCommand(h,"verticalAlign","middle",l);
break;
default:this.executeStyleRuleCommand(h,g,"",l);
this.executeStyleRuleCommand(h,"verticalAlign","",l);
break;
}d=true;
break;
case"td":case"th":this.executeStyleRuleCommand(h,"textAlign",a,l);
this.executeStyleRuleCommand(h,"verticalAlign",m,n);
d=true;
break;
default:this.executeAttributeCommand(h,"align",a,l);
this.executeAttributeCommand(h,"vAlign",m,n);
break;
}if(d){h.removeAttribute("align");
h.removeAttribute("vAlign");
}}else{if("borderColor"==e){j=i.get_selectedItem();
this.executeStyleRuleCommand(this._selectedElement,"borderColor",j,l);
h.removeAttribute("borderColor");
}else{if("bgColor"==e){j=i.get_selectedItem();
this.executeStyleRuleCommand(this._selectedElement,"backgroundColor",j,l);
h.removeAttribute("bgColor");
}else{if("border"==e){j=i.get_selectedItem();
if(!this._issValidAttribValue(j)){alert(this._invalidValueString);
return;
}if(j){j+="px";
this.executeStyleRuleCommand(this._selectedElement,"borderWidth",j,l);
this.executeStyleRuleCommand(this._selectedElement,"borderStyle","solid",l);
}else{this.executeStyleRuleCommand(this._selectedElement,"borderWidth","",l);
this.executeStyleRuleCommand(this._selectedElement,"borderStyle","",l);
}h.removeAttribute("border");
}else{if("width"==e||"height"==e){j=i.get_selectedItem();
if(!this._issValidAttribValue(j)){alert(this._invalidValueString);
return;
}function f(q){var p=""+q;
if(p.indexOf("%")!=-1){return p;
}else{var o=p.match(/(em|ex|px|in|cm|mm|pt|pc)$/);
p=parseInt(p);
if(!isNaN(p)){p=(o)?p+o[0]:p+"px";
return p;
}}return q;
}j=f(j);
if(this._selectedElement.removeAttribute){this._selectedElement.removeAttribute(e);
}this.executeStyleRuleCommand(this._selectedElement,e,j,l);
}else{if("noWrap"==e){c=i.get_selectedItem();
if(c){this.executeStyleRuleCommand(this._selectedElement,"whiteSpace","nowrap",l);
}else{this.executeStyleRuleCommand(this._selectedElement,"whiteSpace","",l);
}}else{var b=e;
c=i.get_selectedItem();
switch(e){case"background":case"className":case"target":case"value":break;
case"cellSpacing":case"cellPadding":if(!this._issValidAttribValue(c)){alert(this._invalidValueString);
return;
}break;
case"NAME":if(!this.isIE){b="name";
}}this.executeAttributeCommand(h,b,c,l);
}}}}}}if(this._selectedElement){this._updateControlValues(this._selectedElement);
}if(this._selectedElement.style.cssText==""){this._selectedElement.removeAttribute("style");
}},_getControlByName:function(c){var a=null;
var d={text:this._getLocalizedString(c),name:c,addClickHandler:true,skin:this.get_skin()};
var b={valueSelected:this._onToolValueSelectedDelegate,show:this._onDropDownBeforeShowDelegate};
switch(c){case"className":d.text=this._getLocalizedString("className");
d.width="90px";
a=Telerik.Web.UI.EditorButton.createTool(d,b,Telerik.Web.UI.Editor.ApplyClassDropDown);
break;
case"borderColor":case"bgColor":d.addCustomColorText=this._getLocalizedString("AddCustomColor");
a=Telerik.Web.UI.EditorButton.createTool(d,b,Telerik.Web.UI.Editor.ColorPicker);
break;
case"align":d.name="AlignmentSelector";
d.text=this._getLocalizedString("align");
a=Telerik.Web.UI.EditorButton.createTool(d,b,Telerik.Web.UI.Editor.AlignmentSelector);
break;
case"SetCellProperties":case"SetTableProperties":case"SetImageProperties":case"LinkManager":delete b.show;
a=Telerik.Web.UI.EditorButton.createTool(d,b);
break;
case"target":d.sizetofit=true;
d.width="90px";
a=Telerik.Web.UI.EditorButton.createTool(d,b,Telerik.Web.UI.EditorUpdateableDropDown);
break;
case"noWrap":delete b.show;
a=Telerik.Web.UI.EditorButton.createTool(d,b,Telerik.Web.UI.EditorCheckBox,document.createElement("span"));
break;
case"width":case"height":case"cellPadding":case"cellSpacing":case"rows":case"cols":case"border":delete b.show;
a=Telerik.Web.UI.EditorButton.createTool(d,b,Telerik.Web.UI.EditorSpinBox,document.createElement("span"));
break;
default:delete b.show;
a=Telerik.Web.UI.EditorButton.createTool(d,b,Telerik.Web.UI.EditorTextBox,document.createElement("span"));
}return a;
}};
Telerik.Web.UI.Editor.Modules.RadEditorNodeInspector.registerClass("Telerik.Web.UI.Editor.Modules.RadEditorNodeInspector",Telerik.Web.UI.Editor.Modules.ModuleBase);
Telerik.Web.UI.Editor.Modules.RadEditorStatistics=function(a){Telerik.Web.UI.Editor.Modules.RadEditorStatistics.initializeBase(this,[a]);
};
Telerik.Web.UI.Editor.Modules.RadEditorStatistics.prototype={initialize:function(){this._enableMaxWidth=false;
this._wordsString=this._getLocalizedString("StatisticsWords","Words:");
this._charactersString=this._getLocalizedString("StatisticsCharacters","Characters:");
this._onDoCountDelegate=Function.createDelegate(this,this.doCount);
Telerik.Web.UI.Editor.Modules.RadEditorStatistics.callBaseMethod(this,"initialize");
},render:function(){Telerik.Web.UI.Editor.Modules.RadEditorStatistics.callBaseMethod(this,"render");
this.get_editor().add_selectionChange(this._onDoCountDelegate);
this.doCount();
},doCount:function(){if(!this.get_visible()){return;
}var c=this.get_editor().get_text();
var i=0;
var b=0;
if(c){var f=/[!\.?;,:&_\-\�\{\}\[\]\(\)~#'"]/g;
c=c.replace(f,"");
var h=/(^\s+)|(\s+$)/g;
c=c.replace(h,"");
if(c){var g=/\s+/;
var a=c.split(g);
i=a.length;
var e=/(\r\n)+/g;
c=c.replace(e,"");
b=c.length;
}}var d=this.get_element();
d.innerHTML="<span style='line-height:22px'>"+this._wordsString+" "+i+" &nbsp;&nbsp;"+this._charactersString+" "+b+"&nbsp;</span>";
}};
Telerik.Web.UI.Editor.Modules.RadEditorStatistics.registerClass("Telerik.Web.UI.Editor.Modules.RadEditorStatistics",Telerik.Web.UI.Editor.Modules.ModuleBase);
Type.registerNamespace("Telerik.Web.UI.Editor.Modules");
(function(c,a,b){c.Editor.Modules.RadEditorTrackChangesInfo=function(d){c.Editor.Modules.RadEditorTrackChangesInfo.initializeBase(this,[d]);
};
c.Editor.Modules.RadEditorTrackChangesInfo.prototype={initialize:function(){this._onRadEditorTrackChangesInfoDelegate=Function.createDelegate(this,this.getRadEditorTrackChangesInfo);
this._isTrackChangeOnFocus=false;
this._author="";
this._trackChangeType="";
this._currTrackChangeElement=null;
c.Editor.Modules.RadEditorTrackChangesInfo.callBaseMethod(this,"initialize");
},render:function(){c.Editor.Modules.RadEditorTrackChangesInfo.callBaseMethod(this,"render");
this.get_editor().add_selectionChange(this._onRadEditorTrackChangesInfoDelegate);
var d=this;
var q=this.get_element();
q.id="rade_tccomment_view_id_"+this._editor._clientStateFieldID;
q.style.width=q.parentNode.style.width;
q.className="reCommentViewContainer";
var p=this;
var h=document.createElement("DIV");
h.id="rade_tccomment_divV_id_"+this._editor._clientStateFieldID;
q.appendChild(h);
var f=document.createElement("DIV");
f.id="rade_tccomment_divEditTitle_id_"+this._editor._clientStateFieldID;
f.className="reCommentTitle";
f.innerHTML="Comment";
this._divEditTitle=f;
var j=document.createElement("DIV");
j.id="rade_tccomment_divViewTitle_id_"+this._editor._clientStateFieldID;
j.className="reCommentTitle";
j.innerHTML="Comment";
this._divViewTitle=j;
var e=document.createElement("DIV");
e.id="rade_tccomment_divEdit_id_"+this._editor._clientStateFieldID;
e.style.width=q.style.width;
e.className="reCommentEdit";
e.style.display="none";
this._divEdit=e;
h.appendChild(e);
e.appendChild(f);
var i=document.createElement("DIV");
i.id="rade_tccomment_divView_id_"+this._editor._clientStateFieldID;
i.style.width=q.style.width;
i.className="reCommentView";
i.style.display="none";
this._divView=i;
h.appendChild(i);
i.appendChild(j);
var g=document.createElement("DIV");
g.id="rade_tccomment_divInfo_id_"+this._editor._clientStateFieldID;
g.className="reCommentInfoPanel";
g.style.width=q.style.width;
g.style.display="none";
this._divInfo=g;
h.appendChild(g);
var s=document.createElement("TEXTAREA");
s.id="rade_textarea_id_"+this._editor._clientStateFieldID;
s.className="reCommentTextArea";
s.setAttribute("rows","2");
s.setAttribute("cols","25");
this._textarea=s;
e.appendChild(s);
var o=document.createElement("input");
o.id="rade_tccomment_save_id_"+this._editor._clientStateFieldID;
o.type="button";
o.value="Save";
o.className="reCommentButton";
o.onclick=function(){var t=p._textarea.value.replace(/^\s+|\s+$/g,"");
if(t==""||t.toLowerCase()=="<enter here>"){alert("Cannot enter empty comment.");
if(s.setActive){s.setActive();
}s.focus();
return;
}p._commentText.innerHTML=t;
p._commentAttr.value=t;
p._timeStampAttr.value=new Date().getTime();
p._titleAttr.value=p._author+": "+t+": "+new Date(parseInt(p._timeStampAttr.value)).toLocaleString();
a("#"+p._divEdit.id).hide();
p._show(a("#"+p._divView.id));
l().removeLastComment(true);
};
e.appendChild(o);
var m=document.createElement("input");
m.id="rade_tccomment_cancel_id_"+this._editor._clientStateFieldID;
m.type="button";
m.value="Cancel";
m.className="reCommentButton";
m.onclick=function(){p._textarea.value=p._commentText.innerHTML;
a("#"+p._divView.id).hide();
a("#"+p._divEdit.id).hide();
l().removeLastComment(false);
};
e.appendChild(m);
var n=document.createElement("input");
n.id="rade_tccomment_edit_id_"+this._editor._clientStateFieldID;
n.type="button";
n.value="Edit";
n.className="reCommentButton";
n.onclick=function(){p._textarea.value=p._commentText.innerHTML;
a("#"+p._divView.id).hide();
p._show(a("#"+p._divEdit.id));
p._textarea.focus();
if(p._textarea.value=="<Enter here>"){p._textarea.select();
}};
var r=document.createElement("span");
r.id="rade_tccomment_spanView_id_"+this._editor._clientStateFieldID;
this._commentText=r;
r.className="reCommentViewText";
i.appendChild(r);
i.appendChild(n);
function l(){var u=d.get_editor()._modulesManager._modules;
for(var t=0;
t<u.length;
t++){if(u[t]._name=="RadEditorTrackChangesInfo"){return u[t];
break;
}}}var k=this._editor._clientStateFieldID;
if(k){a("#rade_textarea_id_"+k).focus(function(){a(this).filter(function(){return a(this).val()==""||a(this).val()=="<Enter here>";
}).removeClass("reCommentTextAreaWatermark").val("");
});
a("#rade_textarea_id_"+k).blur(function(){a(this).filter(function(){return a(this).val()=="";
}).addClass("reCommentTextAreaWatermark").val("<Enter here>");
});
}},_show:function(d){this._hideOtherVisibleContainers(d);
d.fadeIn(300);
},_hideOtherVisibleContainers:function(e){var d=(e)?e.attr("id"):"";
a(".reCommentEdit:visible[id!="+d+"],.reCommentView:visible[id!="+d+"],.reCommentInfoPanel:visible[id!="+d+"]").fadeOut(300);
},getRadEditorTrackChangesInfo:function(){if(!this.get_visible()){return;
}var e=a("#"+this._divInfo.id);
var d=a("#"+this._divEdit.id);
var f=a("#"+this._divView.id);
var g=this.get_editor();
var i=g.getSelectedElement();
var l=Telerik.Web.UI.Editor.TrackChangesUtils.findTrackChangesParentElement(i,g);
this._author="";
if(null!=l){this._timeStampAttr=l.HtmlElement.getAttributeNode(b.Attribute.Timestamp);
var k=null==this._timeStampAttr?"":" on "+new Date(parseInt(this._timeStampAttr.value)).toLocaleString();
this._currTrackChangeElement=l;
this._author=(l.User=="")?"You":l.User;
if(Telerik.Web.UI.Editor.TrackChangesElementType.Comment==l.Type){this._commentAttr=l.HtmlElement.getAttributeNode(b.Attribute.Comment);
this._titleAttr=l.HtmlElement.getAttributeNode(b.Attribute.Title);
this._commentText.innerHTML=(null==this._commentAttr)?"":this._commentAttr.value;
if(this._author==g.get_reviewer()){this._textarea.value=this._commentText.innerHTML||"<Enter here>";
e.hide();
if(this._commentAttr.value==" "||this._titleAttr.value=="#empty comment#"){f.hide();
this._show(d);
var j=this._textarea;
window.setTimeout(function(){try{if(j.setActive){j.setActive();
}j.focus();
}catch(m){return;
}},300);
}else{d.hide();
this._show(f);
this.removeLastComment(false);
}}else{f.hide();
d.hide();
this._show(e);
this._divInfo.innerHTML=this._author+": "+this._commentText.innerHTML+k;
this.removeLastComment(false);
}}else{var h=" ";
if(Telerik.Web.UI.Editor.TrackChangesElementType.Delete==l.Type){h="Deleted by "+this._author+k;
}else{if(Telerik.Web.UI.Editor.TrackChangesElementType.Insert==l.Type){h="Inserted by "+this._author+k;
}else{if(Telerik.Web.UI.Editor.TrackChangesElementType.FormatChange==l.Type){h="Format change("+l.BrowserCommand+") by "+this._author+k;
}}}this._divInfo.innerHTML=h;
f.hide();
d.hide();
h!=" "?this._show(e):e.fadeOut(300);
this.removeLastComment(false);
}}else{this.removeLastComment(false);
if(a(d).is(":visible")){a(d).fadeOut(300);
}if(a(f).is(":visible")){a(f).fadeOut(300);
}if(a(e).is(":visible")){a(e).fadeOut(300);
}}},removeLastComment:function(j){var f=this.get_editor();
var g=f._clientStateFieldID;
var k=f.get_document().getElementById(f.get_commentID());
if(k==null||typeof(k)==undefined){var l=$telerik.radControls;
for(var h=0;
h<l.length;
h++){if(l[h].get_commentID&&l[h]._clientStateFieldID!=g){k=l[h].get_document().getElementById(l[h].get_commentID());
break;
}}}if(k){try{if(j){k.removeAttribute("id");
}else{f.removeFromStack(Telerik.Web.UI.Editor.TrackChangesCommandStrings.AddComment);
a(k).contents().first().unwrap();
}}catch(d){f.undo(1);
}return true;
}return false;
}};
c.Editor.Modules.RadEditorTrackChangesInfo.registerClass("Telerik.Web.UI.Editor.Modules.RadEditorTrackChangesInfo",c.Editor.Modules.ModuleBase);
})(Telerik.Web.UI,$telerik.$,Telerik.Web.UI.Editor.TrackChangesConstants);
