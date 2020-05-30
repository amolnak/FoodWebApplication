/*!
 * JavaScript
 * Original author: N21 Mobile
 * VVersion 1.0 2017-01-30
 */

// Call Vibration API  ---------------------------------
//$("a").vibrate("short");
$("input").vibrate("short");
$(".toggle").vibrate("short");
$(".toggle-flip").vibrate("short");
$(".iOSToggle").vibrate("short");

// HIDE OPTION IN SELECT DROPDOWN ON MAIN LEFT MENU HOVER  ---------------------------------
$(".main-sidebar").mouseover(function() {
	var ntoselectDropDown=$("select"); var length = $('select > option').length;
	ntoselectDropDown.attr('size',length); ntoselectDropDown.attr('size',0);
	ntoselectDropDown.addClass('noShadow selectformBor');
});
$(".card").click(function() { $("select").removeClass('noShadow selectformBor'); });

$(document).ready(function(){
// CUSTOM AND SMOOTH SCROLL  ---------------------------------
    /* $("html").niceScroll({
        cursorcolor: "#424242", background :"#CED0D3", cursoropacitymin: 0, cursoropacitymax: 1, cursorwidth: "5px", cursorborder: "0",
        cursorborderradius: "0", touchbehavior: false, autohidemode: false, scrollspeed: 40, zindex: "99999"
    }).resize(); */
	
// PAGE SCROLL TOP  ---------------------------------
	$(window).scroll(function () { if ($(this).scrollTop() > 100) { $('.scrollup').fadeIn(); } else { $('.scrollup').fadeOut(); } });
    $('.scrollup').click(function () { $("html, body").animate({ scrollTop: 0 }, 600); return false; });
	
// FOR TOOLTIP  ---------------------------------
	$('[data-toggle="tooltip"]').tooltip({ container : 'body' }); $('body [data-toggle="popover"]').popover();
});

// FULL SCREEN SITE  ---------------------------------
//var app = function() {
//$(function() { fullscreenMode(); });
//	var fullscreenMode = function() {
//		$('#toggle-fullscreen.expand').on('click',function(){
//			$(document).toggleFullScreen()
//			$('#toggle-fullscreen .fa').toggleClass('fa-expand fa-compress');  
//		});
//	};
//}();