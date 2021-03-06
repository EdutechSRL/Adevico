Type.registerNamespace("Telerik.Web.UI.Widgets");
Type.registerNamespace("Telerik.Web.UI.Editor.DialogControls");
(function(){var a=$telerik.$;
var b=Telerik.Web.UI;
Telerik.Web.UI.Editor.DialogControls.FileManagerDialog=function(c){Telerik.Web.UI.Editor.DialogControls.FileManagerDialog.initializeBase(this,[c]);
};
Telerik.Web.UI.Editor.DialogControls.FileManagerDialog.registerClass("Telerik.Web.UI.Editor.DialogControls.FileManagerDialog",Telerik.Web.UI.RadWebControl,Telerik.Web.IParameterConsumer);
Type.registerNamespace("Telerik.Web.UI.Widgets");
Telerik.Web.UI.Widgets.FileManager=function(c){Telerik.Web.UI.Widgets.FileManager.initializeBase(this,[c]);
this._clientParameters=null;
};
Telerik.Web.UI.Widgets.FileManager.prototype={initialize:function(){var c=window.localization;
Telerik.Web.UI.Widgets.FileManager.callBaseMethod(this,"initialize");
if(this.get_insertButton()){$addHandlers(this.get_insertButton(),{click:this._insertClickHandler},this);
this.get_insertButton().title=c.Insert;
}if(this.get_cancelButton()){$addHandlers(this.get_cancelButton(),{click:this._cancelClickHandler},this);
this.get_cancelButton().title=c.Cancel;
}if(this._fileBrowser!=null){this._fileBrowser.add_itemSelected(Function.createDelegate(this,this._browserItemClickHandler));
this._fileBrowser.add_fileOpen(Function.createDelegate(this,this._browserDoubleClickHandler));
this._fileBrowser.add_folderChange(Function.createDelegate(this,this._browserDirectoryChangeyHandler));
this._fileBrowser.add_load(Function.createDelegate(this,this._browserLoad));
this._fileBrowser.add_delete(Function.createDelegate(this,this._browserItemDeleteHandler));
}},clientInit:function(c){this._clientParameters=c;
var e=this._getQueryStringParameter();
var f=null;
try{f=c.get_value();
}catch(d){}if(e&&f){this._initialItem=f;
}else{this._initialItem=null;
}this._filePreviewer.clientInit(c);
},_getQueryStringParameter:function(){var d=location.search;
if(d){var f=d.substring(1,d.length);
var e="&PreselectedItemUrl=";
if(f.indexOf(e)!=-1){f=f.substring(f.indexOf(e)+e.length);
var c=f.indexOf("&");
if(c==-1){c=f.length-1;
}f=decodeURIComponent(f.substring(0,c));
return f;
}}return null;
},_stripProtocolAndServerName:function(d){var c=d.indexOf("//");
if(c>=0){c=d.indexOf("/",c+2);
if(c>=0){return d.substring(c);
}}return d;
},get_clientParameters:function(){return this._clientParameters;
},populateObjectProperties:function(c){this._filePreviewer.populateObjectProperties(c);
},get_selectedItem:function(){return this._fileBrowser.get_selectedItem();
},_insertClickHandler:function(){var f=this._filePreviewer.getResult();
var e=this.get_previewerType();
var d=e?e.replace("Previewer","Manager"):"FileBrowser";
var c=new Telerik.Web.UI.EditorCommandEventArgs(d,null,f);
window.setTimeout(function(){Telerik.Web.UI.Dialogs.CommonDialogScript.get_windowReference().close(c);
},0);
},_cancelClickHandler:function(){Telerik.Web.UI.Dialogs.CommonDialogScript.get_windowReference().close();
},_browserDirectoryChangeyHandler:function(d,c){this.browserDirectoryChange(d,c);
},_browserItemClickHandler:function(d,c){this.browserItemClick(d,c);
},_browserDoubleClickHandler:function(d,c){this.browserItemClick(d,c);
if(!c.get_item().isDirectory()){this._insertClickHandler();
}},_browserItemDeleteHandler:function(f,c){var d=f.get_currentDirectory(),e=f.createItemFromPath(d);
this.setBrowserItem(e);
},_browserLoad:function(e,c){var d=e.get_selectedItem();
if(d){this.setBrowserItem(d);
}},currentDirPermissions:function(){return this._fileBrowser.get_currentPermissions();
},browserItemClick:function(d,c){this.setBrowserItem(c.get_item());
},browserDirectoryChange:function(e,c){var d=c.get_item();
if(d!=null){this._filePreviewer.setItem(d);
this.enableButton(this.get_insertButton(),d.get_type()!=Telerik.Web.UI.FileExplorerItemType.Directory);
}},setBrowserItem:function(c){this._filePreviewer.setItem(c);
this.enableButton(this.get_insertButton(),this.shouldEnableButton());
},shouldEnableButton:function(){var d=this._fileBrowser.get_selectedItems();
for(var c=0;
c<d.length;
c++){if(d[c].get_type()==Telerik.Web.UI.FileExplorerItemType.File){return true;
}}return false;
},enableButton:function(c,d){if(c==null){return;
}Telerik.Web.UI.RadFormDecorator.set_enabled(c,d);
},dispose:function(){if(this.get_insertButton()){$clearHandlers(this.get_insertButton());
}if(this.get_cancelButton()){$clearHandlers(this.get_cancelButton());
}Telerik.Web.UI.Widgets.FileManager.callBaseMethod(this,"dispose");
}};
a.registerControlProperties(b.Widgets.FileManager,{imageEditorFileSuffix:"",enableImageEditor:false,previewerType:null,cancelButton:null,insertButton:null,filePreviewer:null,fileBrowser:null,initialItem:null});
Telerik.Web.UI.Widgets.FileManager.registerClass("Telerik.Web.UI.Widgets.FileManager",Telerik.Web.UI.RadWebControl,Telerik.Web.IParameterConsumer);
Telerik.Web.UI.Widgets.FilePreviewer=function(c){Telerik.Web.UI.Widgets.FilePreviewer.initializeBase(this,[c]);
this._item=null;
};
Telerik.Web.UI.Widgets.FilePreviewer.prototype={initialize:function(){Telerik.Web.UI.Widgets.FilePreviewer.callBaseMethod(this,"initialize");
},clientInit:function(c){},setItem:function(c){this._item=c;
},getResult:function(){return this._item;
},_selectOptionByValue:function(h,g,c,d){if(typeof(d)=="undefined"){d=0;
}if(typeof(c)=="undefined"){c=false;
}if(c){g=g.toLowerCase();
}h.selectedIndex=-1;
for(var e=0;
e<h.options.length;
e++){var f=h.options[e].value;
f=c?f:f.toLowerCase();
if(f==g){h.selectedIndex=e;
return;
}}h.selectedIndex=d;
},populateObjectProperties:function(c){},get_browser:function(){return this._browser;
},set_browser:function(c){this._browser=c;
},dispose:function(){Telerik.Web.UI.Widgets.FilePreviewer.callBaseMethod(this,"dispose");
}};
Telerik.Web.UI.Widgets.FilePreviewer.registerClass("Telerik.Web.UI.Widgets.FilePreviewer",Telerik.Web.UI.RadWebControl);
})();
