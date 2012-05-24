function mainmenu(){
	$(" #nav ul ").css({display: "none"});
	$(" #nav li").hover(function(){
		$(this).find('ul:first:hidden').css({visibility: "visible",display: "none"}).slideDown(1000);
		},function(){
			$(this).find('ul:first').slideUp(1000);
		});
	}
	$(document).ready(function(){
		mainmenu();
});
