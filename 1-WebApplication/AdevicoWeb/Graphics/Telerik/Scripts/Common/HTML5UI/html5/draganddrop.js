(function(b,a){a(["./kendo.core","./kendo.userevents"],b);
})(function(){(function(a,P){var A=window.kendo,L=A.support,h=window.document,d=A.Class,R=A.ui.Widget,F=A.Observable,Q=A.UserEvents,K=a.proxy,w=a.extend,x=A.getOffset,n={},u={},r={},C,v=A.elementUnderCursor,B="keyup",b="change",p="dragstart",z="hold",i="drag",k="dragend",j="dragcancel",y="hintDestroyed",l="dragenter",o="dragleave",q="drop";
function f(V,T){try{return a.contains(V,T)||V==T;
}catch(U){return false;
}}function E(T,U){return parseInt(T.css(U),10)||0;
}function S(U,T){return Math.min(Math.max(U,T.min),T.max);
}function e(T,U){var Z=x(T),X=Z.left+E(T,"borderLeftWidth")+E(T,"paddingLeft"),Y=Z.top+E(T,"borderTopWidth")+E(T,"paddingTop"),V=X+T.width()-U.outerWidth(true),W=Y+T.height()-U.outerHeight(true);
return{x:{min:X,max:V},y:{min:Y,max:W}};
}function c(W,Y,U){var aa,Z,V=0,X=Y&&Y.length,T=U&&U.length;
while(W&&W.parentNode){for(V=0;
V<X;
V++){aa=Y[V];
if(aa.element[0]===W){return{target:aa,targetElement:W};
}}for(V=0;
V<T;
V++){Z=U[V];
if(L.matchesSelector.call(W,Z.options.filter)){return{target:Z,targetElement:W};
}}W=W.parentNode;
}return P;
}var M=F.extend({init:function(U,V){var W=this,T=U[0];
W.capture=false;
if(T.addEventListener){a.each(A.eventMap.down.split(" "),function(){T.addEventListener(this,K(W._press,W),true);
});
a.each(A.eventMap.up.split(" "),function(){T.addEventListener(this,K(W._release,W),true);
});
}else{a.each(A.eventMap.down.split(" "),function(){T.attachEvent(this,K(W._press,W));
});
a.each(A.eventMap.up.split(" "),function(){T.attachEvent(this,K(W._release,W));
});
}F.fn.init.call(W);
W.bind(["press","release"],V||{});
},captureNext:function(){this.capture=true;
},cancelCapture:function(){this.capture=false;
},_press:function(T){var U=this;
U.trigger("press");
if(U.capture){T.preventDefault();
}},_release:function(T){var U=this;
U.trigger("release");
if(U.capture){T.preventDefault();
U.cancelCapture();
}}});
var I=F.extend({init:function(T){var U=this;
F.fn.init.call(U);
U.forcedEnabled=false;
a.extend(U,T);
U.scale=1;
if(U.horizontal){U.measure="offsetWidth";
U.scrollSize="scrollWidth";
U.axis="x";
}else{U.measure="offsetHeight";
U.scrollSize="scrollHeight";
U.axis="y";
}},makeVirtual:function(){a.extend(this,{virtual:true,forcedEnabled:true,_virtualMin:0,_virtualMax:0});
},virtualSize:function(U,T){if(this._virtualMin!==U||this._virtualMax!==T){this._virtualMin=U;
this._virtualMax=T;
this.update();
}},outOfBounds:function(T){return T>this.max||T<this.min;
},forceEnabled:function(){this.forcedEnabled=true;
},getSize:function(){return this.container[0][this.measure];
},getTotal:function(){return this.element[0][this.scrollSize];
},rescale:function(T){this.scale=T;
},update:function(U){var W=this,X=W.virtual?W._virtualMax:W.getTotal(),T=X*W.scale,V=W.getSize();
if(X===0&&!W.forcedEnabled){return;
}W.max=W.virtual?-W._virtualMin:0;
W.size=V;
W.total=T;
W.min=Math.min(W.max,V-T);
W.minScale=V/X;
W.centerOffset=(T-V)/2;
W.enabled=W.forcedEnabled||(T>V);
if(!U){W.trigger(b,W);
}}});
var J=F.extend({init:function(T){var U=this;
F.fn.init.call(U);
U.x=new I(w({horizontal:true},T));
U.y=new I(w({horizontal:false},T));
U.container=T.container;
U.forcedMinScale=T.minScale;
U.maxScale=T.maxScale||100;
U.bind(b,T);
},rescale:function(T){this.x.rescale(T);
this.y.rescale(T);
this.refresh();
},centerCoordinates:function(){return{x:Math.min(0,-this.x.centerOffset),y:Math.min(0,-this.y.centerOffset)};
},refresh:function(){var T=this;
T.x.update();
T.y.update();
T.enabled=T.x.enabled||T.y.enabled;
T.minScale=T.forcedMinScale||Math.min(T.x.minScale,T.y.minScale);
T.fitScale=Math.max(T.x.minScale,T.y.minScale);
T.trigger(b);
}});
var H=F.extend({init:function(T){var U=this;
w(U,T);
F.fn.init.call(U);
},outOfBounds:function(){return this.dimension.outOfBounds(this.movable[this.axis]);
},dragMove:function(U){var Y=this,V=Y.dimension,T=Y.axis,W=Y.movable,X=W[T]+U;
if(!V.enabled){return;
}if((X<V.min&&U<0)||(X>V.max&&U>0)){U*=Y.resistance;
}W.translateAxis(T,U);
Y.trigger(b,Y);
}});
var G=d.extend({init:function(U){var W=this,X,Y,V,T;
w(W,{elastic:true},U);
V=W.elastic?0.5:0;
T=W.movable;
W.x=X=new H({axis:"x",dimension:W.dimensions.x,resistance:V,movable:T});
W.y=Y=new H({axis:"y",dimension:W.dimensions.y,resistance:V,movable:T});
W.userEvents.bind(["move","end","gesturestart","gesturechange"],{gesturestart:function(Z){W.gesture=Z;
W.offset=W.dimensions.container.offset();
},gesturechange:function(ab){var ah=W.gesture,ag=ah.center,Z=ab.center,ai=ab.distance/ah.distance,ad=W.dimensions.minScale,ac=W.dimensions.maxScale,aa;
if(T.scale<=ad&&ai<1){ai+=(1-ai)*0.8;
}if(T.scale*ai>=ac){ai=ac/T.scale;
}var ae=T.x+W.offset.left,af=T.y+W.offset.top;
aa={x:(ae-ag.x)*ai+Z.x-ae,y:(af-ag.y)*ai+Z.y-af};
T.scaleWith(ai);
X.dragMove(aa.x);
Y.dragMove(aa.y);
W.dimensions.rescale(T.scale);
W.gesture=ab;
ab.preventDefault();
},move:function(Z){if(Z.event.target.tagName.match(/textarea|input/i)){return;
}if(X.dimension.enabled||Y.dimension.enabled){X.dragMove(Z.x.delta);
Y.dragMove(Z.y.delta);
Z.preventDefault();
}else{Z.touch.skip();
}},end:function(Z){Z.preventDefault();
}});
}});
var N=L.transitions.prefix+"Transform",O;
if(L.hasHW3D){O=function(U,V,T){return"translate3d("+U+"px,"+V+"px,0) scale("+T+")";
};
}else{O=function(U,V,T){return"translate("+U+"px,"+V+"px) scale("+T+")";
};
}var D=F.extend({init:function(T){var U=this;
F.fn.init.call(U);
U.element=a(T);
U.element[0].style.webkitTransformOrigin="left top";
U.x=0;
U.y=0;
U.scale=1;
U._saveCoordinates(O(U.x,U.y,U.scale));
},translateAxis:function(T,U){this[T]+=U;
this.refresh();
},scaleTo:function(T){this.scale=T;
this.refresh();
},scaleWith:function(T){this.scale*=T;
this.refresh();
},translate:function(T){this.x+=T.x;
this.y+=T.y;
this.refresh();
},moveAxis:function(T,U){this[T]=U;
this.refresh();
},moveTo:function(T){w(this,T);
this.refresh();
},refresh:function(){var U=this,V=U.x,W=U.y,T;
if(U.round){V=Math.round(V);
W=Math.round(W);
}T=O(V,W,U.scale);
if(T!=U.coordinates){if(A.support.browser.msie&&A.support.browser.version<10){U.element[0].style.position="absolute";
U.element[0].style.left=U.x+"px";
U.element[0].style.top=U.y+"px";
}else{U.element[0].style[N]=T;
}U._saveCoordinates(T);
U.trigger(b);
}},_saveCoordinates:function(T){this.coordinates=T;
}});
function g(T,X){var V=X.options.group,U=T[V],W;
R.fn.destroy.call(X);
if(U.length>1){for(W=0;
W<U.length;
W++){if(U[W]==X){U.splice(W,1);
break;
}}}else{U.length=0;
delete T[V];
}}var s=R.extend({init:function(T,V){var W=this;
R.fn.init.call(W,T,V);
var U=W.options.group;
if(!(U in u)){u[U]=[W];
}else{u[U].push(W);
}},events:[l,o,q],options:{name:"DropTarget",group:"default"},destroy:function(){g(u,this);
},_trigger:function(V,U){var W=this,T=n[W.options.group];
if(T){return W.trigger(V,w({},U.event,{draggable:T,dropTarget:U.dropTarget}));
}},_over:function(T){this._trigger(l,T);
},_out:function(T){this._trigger(o,T);
},_drop:function(U){var V=this,T=n[V.options.group];
if(T){T.dropped=!V._trigger(q,U);
}}});
s.destroyGroup=function(U){var T=u[U]||r[U],V;
if(T){for(V=0;
V<T.length;
V++){R.fn.destroy.call(T[V]);
}T.length=0;
delete u[U];
delete r[U];
}};
s._cache=u;
var t=s.extend({init:function(T,V){var W=this;
R.fn.init.call(W,T,V);
var U=W.options.group;
if(!(U in r)){r[U]=[W];
}else{r[U].push(W);
}},destroy:function(){g(r,this);
},options:{name:"DropTargetArea",group:"default",filter:null}});
var m=R.extend({init:function(T,U){var V=this;
R.fn.init.call(V,T,U);
V._activated=false;
V.userEvents=new Q(V.element,{global:true,allowSelection:true,filter:V.options.filter,threshold:V.options.distance,start:K(V._start,V),hold:K(V._hold,V),move:K(V._drag,V),end:K(V._end,V),cancel:K(V._cancel,V),select:K(V._select,V)});
V._afterEndHandler=K(V._afterEnd,V);
V._captureEscape=K(V._captureEscape,V);
},events:[z,p,i,k,j,y],options:{name:"Draggable",distance:(A.support.touch?0:5),group:"default",cursorOffset:null,axis:null,container:null,filter:null,ignore:null,holdToDrag:false,dropped:false},cancelHold:function(){this._activated=false;
},_captureEscape:function(T){var U=this;
if(T.keyCode===A.keys.ESC){U._trigger(j,{event:T});
U.userEvents.cancel();
}},_updateHint:function(X){var Z=this,V,Y=Z.options,U=Z.boundaries,T=Y.axis,W=Z.options.cursorOffset;
if(W){V={left:X.x.location+W.left,top:X.y.location+W.top};
}else{Z.hintOffset.left+=X.x.delta;
Z.hintOffset.top+=X.y.delta;
V=a.extend({},Z.hintOffset);
}if(U){V.top=S(V.top,U.y);
V.left=S(V.left,U.x);
}if(T==="x"){delete V.top;
}else{if(T==="y"){delete V.left;
}}Z.hint.css(V);
},_shouldIgnoreTarget:function(U){var T=this.options.ignore;
return T&&a(U).is(T);
},_select:function(T){if(!this._shouldIgnoreTarget(T.event.target)){T.preventDefault();
}},_start:function(U){var Y=this,X=Y.options,T=X.container,V=X.hint;
if(this._shouldIgnoreTarget(U.touch.initialTouch)||(X.holdToDrag&&!Y._activated)){Y.userEvents.cancel();
return;
}Y.currentTarget=U.target;
Y.currentTargetOffset=x(Y.currentTarget);
if(V){if(Y.hint){Y.hint.stop(true,true).remove();
}Y.hint=A.isFunction(V)?a(V.call(Y,Y.currentTarget)):V;
var W=x(Y.currentTarget);
Y.hintOffset=W;
Y.hint.css({position:"absolute",zIndex:20000,left:W.left,top:W.top}).appendTo(h.body);
Y.angular("compile",function(){Y.hint.removeAttr("ng-repeat");
return{elements:Y.hint.get(),scopeFrom:U.target};
});
}n[X.group]=Y;
Y.dropped=false;
if(T){Y.boundaries=e(T,Y.hint);
}if(Y._trigger(p,U)){Y.userEvents.cancel();
Y._afterEnd();
}Y.userEvents.capture();
a(h).on(B,Y._captureEscape);
},_hold:function(T){this.currentTarget=T.target;
if(this._trigger(z,T)){this.userEvents.cancel();
}else{this._activated=true;
}},_drag:function(T){var U=this;
T.preventDefault();
U._withDropTarget(T,function(V,W){if(!V){if(C){C._trigger(o,w(T,{dropTarget:a(C.targetElement)}));
C=null;
}return;
}if(C){if(W===C.targetElement){return;
}C._trigger(o,w(T,{dropTarget:a(C.targetElement)}));
}V._trigger(l,w(T,{dropTarget:a(W)}));
C=w(V,{targetElement:W});
});
U._trigger(i,w(T,{dropTarget:C}));
if(U.hint){U._updateHint(T);
}},_end:function(T){var U=this;
U._withDropTarget(T,function(V,W){if(V){V._drop(w({},T,{dropTarget:a(W)}));
C=null;
}});
U._trigger(k,T);
U._cancel(T.event);
},_cancel:function(){var T=this;
T._activated=false;
if(T.hint&&!T.dropped){setTimeout(function(){T.hint.stop(true,true).animate(T.currentTargetOffset,"fast",T._afterEndHandler);
},0);
}else{T._afterEnd();
}},_trigger:function(U,T){var V=this;
return V.trigger(U,w({},T.event,{x:T.x,y:T.y,currentTarget:V.currentTarget,initialTarget:T.touch?T.touch.initialTouch:null,dropTarget:T.dropTarget}));
},_withDropTarget:function(V,U){var aa=this,Y,X,W=aa.options,Z=u[W.group],T=r[W.group];
if(Z&&Z.length||T&&T.length){Y=v(V);
if(aa.hint&&f(aa.hint[0],Y)){aa.hint.hide();
Y=v(V);
if(!Y){Y=v(V);
}aa.hint.show();
}X=c(Y,Z,T);
if(X){U(X.target,X.targetElement);
}else{U();
}}},destroy:function(){var T=this;
R.fn.destroy.call(T);
T._afterEnd();
T.userEvents.destroy();
T.currentTarget=null;
},_afterEnd:function(){var T=this;
if(T.hint){T.hint.remove();
}delete n[T.options.group];
T.trigger("destroy");
T.trigger(y);
a(h).off(B,T._captureEscape);
}});
A.ui.plugin(s);
A.ui.plugin(t);
A.ui.plugin(m);
A.TapCapture=M;
A.containerBoundaries=e;
w(A.ui,{Pane:G,PaneDimensions:J,Movable:D});
})(window.kendo.jQuery);
return window.kendo;
},typeof define=="function"&&define.amd?define:function(a,b){b();
});
