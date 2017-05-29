jQuery(function () {
    jQuery(".ADVpanel-group .ADVpanel-heading").click(function (e) {
        var x = jQuery(".ADVpanel-group .ADVpanel-heading").index(this);
		if (x < 0)
			return;

		var elClicked = jQuery(this);
		var elCollapse = elClicked.parents(".ADVpanel-group").find(".ADVpanel-collapse:eq(" + x + ")");
		var elBody = elCollapse.find(".ADVpanel-body:first");
		if (elBody.size() == 1)
		    elCollapse.height(elBody.outerHeight(true));
		

		if (elClicked.hasClass("ADVselected")) {
			elClicked.removeClass("ADVselected");
			elCollapse.addClass("ADVcollapsed");
		} else {
			elClicked.addClass("ADVselected");
			elCollapse.removeClass("ADVcollapsed");
		}
			
    });

    
    jQuery(".ADVpanel-group .ADVpanel-heading a").click(function (e) {
        e.stopPropagation()
    });
});