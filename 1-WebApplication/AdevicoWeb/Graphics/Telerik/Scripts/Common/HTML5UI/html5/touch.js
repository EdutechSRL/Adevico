(function(b,a){a(["./kendo.core","./kendo.userevents"],b);
})(function(){(function(a,h){var c=window.kendo,i=c.ui.Widget,e=a.proxy,b=Math.abs,d=20;
var f=c.Class.extend({init:function(k,j,l){l=a.extend({minXDelta:30,maxYDelta:20,maxDuration:1000},l);
new c.UserEvents(k,{surface:l.surface,allowSelection:true,start:function(m){if(b(m.x.velocity)*2>=b(m.y.velocity)){m.sender.capture();
}},move:function(o){var p=o.touch,n=o.event.timeStamp-p.startTime,m=p.x.initialDelta>0?"right":"left";
if(b(p.x.initialDelta)>=l.minXDelta&&b(p.y.initialDelta)<l.maxYDelta&&n<l.maxDuration){j({direction:m,touch:p,target:p.target});
p.cancel();
}}});
}});
var g=i.extend({init:function(j,m){var n=this;
i.fn.init.call(n,j,m);
m=n.options;
j=n.element;
n.wrapper=j;
function k(o){return function(p){n._triggerTouch(o,p);
};
}function l(o){return function(p){n.trigger(o,{touches:p.touches,distance:p.distance,center:p.center,event:p.event});
};
}n.events=new c.UserEvents(j,{filter:m.filter,surface:m.surface,minHold:m.minHold,multiTouch:m.multiTouch,allowSelection:true,press:k("touchstart"),hold:k("hold"),tap:e(n,"_tap"),gesturestart:l("gesturestart"),gesturechange:l("gesturechange"),gestureend:l("gestureend")});
if(m.enableSwipe){n.events.bind("start",e(n,"_swipestart"));
n.events.bind("move",e(n,"_swipemove"));
}else{n.events.bind("start",e(n,"_dragstart"));
n.events.bind("move",k("drag"));
n.events.bind("end",k("dragend"));
}c.notify(n);
},events:["touchstart","dragstart","drag","dragend","tap","doubletap","hold","swipe","gesturestart","gesturechange","gestureend"],options:{name:"Touch",surface:null,global:false,multiTouch:false,enableSwipe:false,minXDelta:30,maxYDelta:20,maxDuration:1000,minHold:800,doubleTapTimeout:800},cancel:function(){this.events.cancel();
},_triggerTouch:function(k,j){if(this.trigger(k,{touch:j.touch,event:j.event})){j.preventDefault();
}},_tap:function(j){var l=this,k=l.lastTap,m=j.touch;
if(k&&(m.endTime-k.endTime<l.options.doubleTapTimeout)&&c.touchDelta(m,k).distance<d){l._triggerTouch("doubletap",j);
l.lastTap=null;
}else{l._triggerTouch("tap",j);
l.lastTap=m;
}},_dragstart:function(j){this._triggerTouch("dragstart",j);
},_swipestart:function(j){if(b(j.x.velocity)*2>=b(j.y.velocity)){j.sender.capture();
}},_swipemove:function(l){var n=this,m=n.options,o=l.touch,k=l.event.timeStamp-o.startTime,j=o.x.initialDelta>0?"right":"left";
if(b(o.x.initialDelta)>=m.minXDelta&&b(o.y.initialDelta)<m.maxYDelta&&k<m.maxDuration){n.trigger("swipe",{direction:j,touch:l.touch});
o.cancel();
}}});
c.ui.plugin(g);
})(window.kendo.jQuery);
return window.kendo;
},typeof define=="function"&&define.amd?define:function(a,b){b();
});
