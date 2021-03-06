(function(b,a){a(["./kendo.list","./kendo.mobile.scroller"],b);
})(function(){(function(a,y){var l=window.kendo,x=l.ui,r=x.Select,o=l.support.mobileOS,b=l._activeElement,m=l.keys,n=".kendoDropDownList",h="disabled",q="readonly",f="change",j="k-state-focused",g="k-state-default",v="k-state-disabled",c="aria-disabled",d="aria-readonly",s="k-state-selected",k="mouseenter"+n+" mouseleave"+n,w="tabindex",u="filter",t="accept",p=a.proxy;
var i=r.extend({init:function(z,C){var E=this;
var A=C&&C.index;
var B,F,D;
E.ns=n;
C=a.isArray(C)?{dataSource:C}:C;
r.fn.init.call(E,z,C);
C=E.options;
z=E.element.on("focus"+n,p(E._focusHandler,E));
E._inputTemplate();
E._reset();
E._prev="";
E._word="";
E._wrapper();
E._tabindex();
E.wrapper.data(w,E.wrapper.attr(w));
E._span();
E._popup();
E._mobile();
E._dataSource();
E._ignoreCase();
E._filterHeader();
E._aria();
E._enable();
E._oldIndex=E.selectedIndex=-1;
if(A!==y){C.index=A;
}E._initialIndex=C.index;
E._optionLabel();
E._initList();
E._cascade();
if(C.autoBind){E.dataSource.fetch();
}else{if(E.selectedIndex===-1){D=C.text||"";
if(!D){B=C.optionLabel;
F=B&&C.index===0;
if(E._isSelect){if(F){D=B;
}else{D=z.children(":selected").text();
}}else{if(!z[0].value&&F){D=B;
}}}E._textAccessor(D);
}}l.notify(E);
},options:{name:"DropDownList",enabled:true,autoBind:true,index:0,text:null,value:null,delay:500,height:200,dataTextField:"",dataValueField:"",optionLabel:"",cascadeFrom:"",cascadeFromField:"",ignoreCase:true,animation:{},filter:"none",minLength:1,virtual:false,template:null,valueTemplate:null,optionLabelTemplate:null,groupTemplate:null,fixedGroupTemplate:null},events:["open","close",f,"select","filtering","dataBinding","dataBound","cascade"],setOptions:function(z){r.fn.setOptions.call(this,z);
this.listView.setOptions(z);
this._inputTemplate();
this._accessors();
this._filterHeader();
this._enable();
this._aria();
},destroy:function(){var z=this;
z.wrapper.off(n);
z.element.off(n);
z._inputWrapper.off(n);
z._arrow.off();
z._arrow=null;
z.optionLabel.off();
r.fn.destroy.call(z);
},open:function(){var z=this;
if(z.popup.visible()){return;
}if(!this.dataSource.view().length||z._state===t){z._open=true;
z._state="rebind";
if(z.filterInput){z.filterInput.val("");
}z._filterSource();
}else{z.popup.open();
z._focusElement(z.filterInput);
z._focusItem();
}},toggle:function(z){this._toggle(z,true);
},_initList:function(){var A=this;
var z=this.options;
var B;
if(z.virtual){B={autoBind:false,dataValueField:z.dataValueField,dataSource:this.dataSource,selectable:true,height:this.options.height,groupTemplate:z.groupTemplate||"#:data#",fixedGroupTemplate:z.fixedGroupTemplate||"#:data#",template:z.template||"#:"+l.expr(z.dataTextField,"data")+"#",change:a.proxy(this._listChange,this),click:a.proxy(this._click,this),activate:function(){var C=this.focus();
if(C){A._focused.add(A.filterInput).attr("aria-activedescendant",C.attr("id"));
}},deactivate:function(){A._focused.add(A.filterInput).removeAttr("aria-activedescendant");
},listBound:a.proxy(this._listBound,this)};
if(typeof z.virtual==="object"){a.extend(B,z.virtual);
}this.listView=new l.ui.VirtualList(this.ul,B);
}else{this.listView=new l.ui.StaticList(this.ul,{dataValueField:z.dataValueField,dataSource:this.dataSource,groupTemplate:z.groupTemplate||"#:data#",fixedGroupTemplate:z.fixedGroupTemplate||"#:data#",template:z.template||"#:"+l.expr(z.dataTextField,"data")+"#",activate:function(){var C=this.focus();
if(C){A._focused.add(A.filterInput).attr("aria-activedescendant",C.attr("id"));
}},click:a.proxy(this._click,this),change:a.proxy(this._listChange,this),deactivate:function(){A._focused.add(A.filterInput).removeAttr("aria-activedescendant");
},dataBinding:function(){A.trigger("dataBinding");
A._angularItems("cleanup");
},dataBound:a.proxy(this._listBound,this)});
}this.listView.value(this.options.value);
},current:function(z){var A;
if(z===y){A=this.listView.focus();
if(!A&&this.selectedIndex===0&&this.optionLabel[0]){return this.optionLabel;
}return A;
}this._focus(z);
},dataItem:function(A){var B=this;
var z;
if(A===y){z=B.listView.selectedDataItems()[0];
if(!z&&this.optionLabel[0]){z={};
e(z,B.options.dataTextField.split("."),B._optionLabelText());
e(z,B.options.dataValueField.split("."),"");
}return z;
}if(typeof A!=="number"){A=a(B.items()).index(A);
}return B.listView.data()[A];
},refresh:function(){this.listView.refresh();
},text:function(C){var D=this;
var z,B;
var A=D.options.ignoreCase;
C=C===null?"":C;
if(C!==y){if(typeof C==="string"){B=A?C.toLowerCase():C;
D._select(function(E){E=D._text(E);
if(A){E=(E+"").toLowerCase();
}return E===B;
});
z=D.dataItem();
if(z){C=z;
}}D._textAccessor(C);
}else{return D._textAccessor();
}},value:function(A){var z=this;
if(A===y){A=z._accessor()||z.listView.value()[0];
return A===y||A===null?"":A;
}if(A===null){A="";
}A=A.toString();
z.listView.one("change",function(){z._old=z._accessor();
z._oldIndex=z.selectedIndex;
});
z.listView.value(A);
z._fetchData();
},_optionLabel:function(){var C=this;
var A=C.options;
var z=A.optionLabel;
var B=A.optionLabelTemplate;
if(!z){C.optionLabel=a();
return;
}if(!B){B="#:";
if(typeof z==="string"){B+="data";
}else{B+=l.expr(A.dataTextField,"data");
}B+="#";
}if(typeof B!=="function"){B=l.template(B);
}C.optionLabelTemplate=B;
C.optionLabel=a('<div class="k-list-optionlabel">'+B(z)+"</div>").prependTo(C.list).click(a.proxy(this._click,this));
C.angular("compile",function(){return{elements:C.optionLabel};
});
},_optionLabelText:function(){var z=this.options.optionLabel;
return(typeof z==="string")?z:this._text(z);
},_listBound:function(){var G=this;
var z=G.listView.data();
var D=z.length;
var E=G.options.optionLabel;
var B=G._state===u;
var A=G.element[0];
var F;
var C;
var H;
G._angularItems("compile");
if(!G.options.virtual){C=G._height(B?(D||1):D);
G._calculateGroupPadding(C);
}if(G.popup.visible()){G.popup._position();
}if(G._isSelect){F=A.selectedIndex;
H=G.value();
if(D){if(E){E=G._option("",this._optionLabelText());
}}else{if(H){F=0;
E=G._option(H,G.text());
}}G._options(z,E);
A.selectedIndex=F===-1?0:F;
}G._hideBusy();
G._makeUnselectable();
if(!B){if(G._open){G.toggle(!!D);
}G._open=false;
if(!G._fetch){if(D){if(!this.listView.value().length&&this._initialIndex>-1&&this._initialIndex!==null){this.select(this._initialIndex);
}this._initialIndex=null;
}else{if(this._textAccessor()!==E){this.listView.value("");
this._selectValue(null);
}}}}else{this.listView.first();
}G.trigger("dataBound");
},_listChange:function(){this._selectValue(this.listView.selectedDataItems()[0]);
if(this._old&&this._oldIndex===-1){this._oldIndex=this.selectedIndex;
}},_focusHandler:function(){this.wrapper.focus();
},_focusinHandler:function(){this._inputWrapper.addClass(j);
this._prevent=false;
},_focusoutHandler:function(){var B=this;
var z=B._state===u;
var A=window.self!==window.top;
if(!B._prevent){clearTimeout(B._typing);
if(z){B._select(B._focus(),!B.listView.dataItems()[0]);
}if(!z||B.dataItem()){}if(l.support.mobileOS.ios&&A){B._change();
}else{B._blur();
}B._inputWrapper.removeClass(j);
B._prevent=true;
B._open=false;
B.element.blur();
}},_wrapperMousedown:function(){this._prevent=!!this.filterInput;
},_wrapperClick:function(z){z.preventDefault();
this._focused=this.wrapper;
this._toggle();
},_editable:function(C){var E=this;
var B=E.element;
var z=C.disable;
var D=C.readonly;
var F=E.wrapper.add(E.filterInput).off(n);
var A=E._inputWrapper.off(k);
if(!D&&!z){B.removeAttr(h).removeAttr(q);
A.addClass(g).removeClass(v).on(k,E._toggleHover);
F.attr(w,F.data(w)).attr(c,false).attr(d,false).on("keydown"+n,p(E._keydown,E)).on("focusin"+n,p(E._focusinHandler,E)).on("focusout"+n,p(E._focusoutHandler,E)).on("mousedown"+n,p(E._wrapperMousedown,E));
E.wrapper.on("click"+n,p(E._wrapperClick,E));
if(!E.filterInput){F.on("keypress"+n,p(E._keypress,E));
}}else{if(z){F.removeAttr(w);
A.addClass(v).removeClass(g);
}else{A.addClass(g).removeClass(v);
F.on("focusin"+n,p(E._focusinHandler,E)).on("focusout"+n,p(E._focusoutHandler,E));
}}B.attr(h,z).attr(q,D);
F.attr(c,z).attr(d,D);
},_option:function(A,z){return'<option value="'+A+'">'+z+"</option>";
},_keydown:function(A){var D=this;
var C=A.keyCode;
var z=A.altKey;
var E=D.ul[0];
var B;
if(C===m.LEFT){C=m.UP;
}else{if(C===m.RIGHT){C=m.DOWN;
}}A.keyCode=C;
if(z&&C===m.UP){D._focusElement(D.wrapper);
}B=D._move(A);
if(B){return;
}if(!D.popup.visible()||!D.filterInput){if(C===m.HOME){B=true;
D._firstItem();
}else{if(C===m.END){B=true;
D._lastItem();
}}if(B){D._select(D._focus());
A.preventDefault();
}}if(!z&&!B&&D.filterInput){D._search();
}},_selectNext:function(H,C){var G=this,F,E=C,A=G.listView.data(),D=A.length,B=G.options.ignoreCase,z=function(J,I){J=J+"";
if(B){J=J.toLowerCase();
}if(J.indexOf(H)===0){G._select(I);
if(!G.popup.visible()){G._change();
}return true;
}};
for(;
C<D;
C++){F=G._text(A[C]);
if(F&&z(F,C)){return true;
}}if(E>0&&E<D){C=0;
for(;
C<=E;
C++){F=G._text(A[C]);
if(F&&z(F,C)){return true;
}}}return false;
},_keypress:function(A){var C=this;
if(A.which===0||A.keyCode===l.keys.ENTER){return;
}var z=String.fromCharCode(A.charCode||A.keyCode);
var B=C.selectedIndex;
var D=C._word;
if(C.options.ignoreCase){z=z.toLowerCase();
}if(z===" "){A.preventDefault();
}if(C._last===z&&D.length<=1&&B>-1){if(!D){D=z;
}if(C._selectNext(D,B+1)){return;
}}C._word=D+z;
C._last=z;
C._search();
},_popupOpen:function(){var z=this.popup;
z.wrapper=l.wrap(z.element);
if(z.element.closest(".km-root")[0]){z.wrapper.addClass("km-popup km-widget");
this.wrapper.addClass("km-widget");
}},_popup:function(){r.fn._popup.call(this);
this.popup.one("open",p(this._popupOpen,this));
},_click:function(z){var A=z.item||a(z.currentTarget);
if(this.trigger("select",{item:A})){this.close();
return;
}this._userTriggered=true;
this._select(A);
this._focusElement(this.wrapper);
this._blur();
},_focusElement:function(B){var z=b();
var D=this.wrapper;
var C=this.filterInput;
var A=B===C?D:C;
if(C&&A[0]===z){this._prevent=true;
this._focused=B.focus();
}},_filter:function(B){if(B){var A=this;
var z=A.options.ignoreCase;
if(z){B=B.toLowerCase();
}A._select(function(C){var D=A._text(C);
if(D!==y){D=(D+"");
if(z){D=D.toLowerCase();
}return D.indexOf(B)===0;
}});
}},_search:function(){var B=this,z=B.dataSource,A=B.selectedIndex,C=B._word;
clearTimeout(B._typing);
if(B.options.filter!=="none"){B._typing=setTimeout(function(){var D=B.filterInput.val();
if(B._prev!==D){B._prev=D;
B.search(D);
}B._typing=null;
},B.options.delay);
}else{B._typing=setTimeout(function(){B._word="";
},B.options.delay);
if(A===-1){A=0;
}if(!B.ul[0].firstChild){z.one(f,function(){if(z.data()[0]&&A>-1){B._selectNext(C,A);
}}).fetch();
return;
}B._selectNext(C,A);
}},_get:function(z){var A,B,C;
if(this.optionLabel[0]){if(typeof z==="number"){z-=1;
}else{if(z instanceof jQuery&&z.hasClass("k-list-optionlabel")){z=-1;
}}}if(typeof z==="function"){A=this.listView.data();
for(C=0;
C<A.length;
C++){if(z(A[C])){z=C;
B=true;
break;
}}if(!B){z=-1;
}}return z;
},_firstItem:function(){if(this.optionLabel[0]){this._focus(this.optionLabel);
}else{this.listView.first();
}},_lastItem:function(){this.optionLabel.removeClass("k-state-focused");
this.listView.last();
},_nextItem:function(){if(this.optionLabel.hasClass("k-state-focused")){this.optionLabel.removeClass("k-state-focused");
this.listView.first();
}else{this.listView.next();
}},_prevItem:function(){if(this.optionLabel.hasClass("k-state-focused")){return;
}this.listView.prev();
if(!this.listView.focus()){this.optionLabel.addClass("k-state-focused");
}},_focusItem:function(){var B=this.listView;
var z=B.focus();
var A=B.select();
A=A[A.length-1];
if(A===y&&this.options.highlightFirst&&!z){A=0;
}if(A!==y){B.focus(A);
}else{if(this.options.optionLabel){this._focus(this.optionLabel);
this._select(this.optionLabel);
}else{B.scrollToIndex(0);
}}},_focus:function(z){var A=this.listView;
var B=this.optionLabel;
if(z===y){z=A.focus();
if(!z&&B.hasClass("k-state-focused")){z=B;
}return z;
}B.removeClass("k-state-focused");
z=this._get(z);
A.focus(z);
if(z===-1){B.addClass("k-state-focused");
}},_select:function(z,A){var B=this.optionLabel;
z=this._get(z);
if(!A&&this._state===u){this.listView.clearIndices();
this.listView.filter(false);
this._state=t;
}B.removeClass("k-state-focused k-state-selected");
this.listView.select(z);
if(z===-1){this._selectValue(null);
this._focus(B.addClass("k-state-selected"));
}},_selectValue:function(z){var D="";
var C="";
var A=this.listView.select();
var B=this.options.optionLabel;
A=A[A.length-1];
if(A===y){A=-1;
}if(z){C=z;
D=this._dataValue(z);
if(B){A+=1;
}}else{if(B){this._focus(this.optionLabel);
C=this._optionLabelText();
if(typeof B==="string"){D="";
}else{D=this._value(B);
}A=0;
}}this.selectedIndex=A;
if(D===null){D="";
}this._textAccessor(C);
this._accessor(D,A);
this._triggerCascade();
},_mobile:function(){var B=this,z=B.popup,A=z.element.parents(".km-root").eq(0);
if(A.length&&o){z.options.animation.open.effects=(o.android||o.meego)?"fadeIn":(o.ios||o.wp)?"slideIn:up":z.options.animation.open.effects;
}},_filterHeader:function(){var A;
var B=this.options;
var z=B.filter!=="none";
if(this.filterInput){this.filterInput.off(n).parent().remove();
this.filterInput=null;
}if(z){A='<span unselectable="on" class="k-icon k-i-search">select</span>';
this.filterInput=a('<input class="k-textbox"/>').attr({role:"listbox","aria-haspopup":true,"aria-expanded":false});
this.list.prepend(a('<span class="k-list-filter" />').append(this.filterInput.add(A)));
}},_span:function(){var B=this,C=B.wrapper,z="span.k-input",A;
A=C.find(z);
if(!A[0]){C.append('<span unselectable="on" class="k-dropdown-wrap k-state-default"><span unselectable="on" class="k-input">&nbsp;</span><span unselectable="on" class="k-select"><span unselectable="on" class="k-icon k-i-arrow-s">select</span></span></span>').append(B.element);
A=C.find(z);
}B.span=A;
B._inputWrapper=a(C[0].firstChild);
B._arrow=C.find(".k-icon");
},_wrapper:function(){var B=this,A=B.element,z=A[0],C;
C=A.parent();
if(!C.is("span.k-widget")){C=A.wrap("<span />").parent();
C[0].style.cssText=z.style.cssText;
}A.hide();
B._focused=B.wrapper=C.addClass("k-widget k-dropdown k-header").addClass(z.className).css("display","").attr({unselectable:"on",role:"listbox","aria-haspopup":true,"aria-expanded":false});
},_clearSelection:function(){var A=this;
var z=A.options.optionLabel;
A.options.value="";
if(A.dataSource.view()[0]&&(z||A._userTriggered)){A.select(0);
}else{A.select(-1);
A._textAccessor(A.options.optionLabel);
}},_inputTemplate:function(){var A=this,z=A.options.valueTemplate;
if(!z){z=a.proxy(l.template("#:this._text(data)#",{useWithBlock:false}),A);
}else{z=l.template(z);
}A.valueTemplate=z;
},_textAccessor:function(F){var z=this.listView.selectedDataItems()[0];
var E=this.valueTemplate;
var C=this.options;
var B=C.optionLabel;
var D=this.span;
if(F!==y){if(a.isPlainObject(F)||F instanceof l.data.ObservableObject){z=F;
}else{if(B&&this._optionLabelText()===F){z=B;
E=this.optionLabelTemplate;
}}if(z===y){if(C.dataTextField){z={};
e(z,C.dataTextField.split("."),F);
e(z,C.dataValueField.split("."),this._accessor());
}else{z=F;
}}var A=function(){return{elements:D.get(),data:[{dataItem:z}]};
};
this.angular("cleanup",A);
D.html(E(z));
this.angular("compile",A);
}else{return D.text();
}}});
function e(C,A,E){var B=0,D=A.length-1,z;
for(;
B<D;
++B){z=A[B];
if(!(z in C)){C[z]={};
}C=C[z];
}C[A[D]]=E;
}x.plugin(i);
})(window.kendo.jQuery);
return window.kendo;
},typeof define=="function"&&define.amd?define:function(a,b){b();
});
