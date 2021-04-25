"use strict";
/* -------------------------------------
 Google Analytics
 change UA-XXXXX-X to be your site's ID.
 -------------------------------------- */
/*(function(b,o,i,l,e,r){b.GoogleAnalyticsObject=l;b[l]||(b[l]=
 function(){(b[l].q=b[l].q||[]).push(arguments)});b[l].l=+new Date;
 e=o.createElement(i);r=o.getElementsByTagName(i)[0];
 e.src='//www.google-analytics.com/analytics.js';
 r.parentNode.insertBefore(e,r)}(window,document,'script','ga'));
 ga('create','UA-XXXXX-X','auto');ga('send','pageview');*/
/* -------------------------------------
 CUSTOM FUNCTION WRITE HERE
 -------------------------------------- */
$(document).ready(function (e) {

    /*-------------------------------------
     HOME SLIDER
     --------------------------------------*/
    var swiper = new Swiper('#tg-home-slider',
            {
                pagination: '.swiper-pagination',
                nextButton: '.swiper-button-next',
                prevButton: '.swiper-button-prev',
                paginationClickable: true,
                mousewheelControl: false,
                direction: 'vertical',
                parallax: true,
                speed: 2000
            }
    );
    /*-------------------------------------
     NAV FUNCTION
     --------------------------------------*/
    function navFunction() {
        $('#tg-burger-menu').on('click', function (event) {
            event.preventDefault();
            $('#tg-nav, #tg-burger-menu').fadeOut();
            $('#tg-additional-nav').delay(500).fadeIn();
        });
        $('#tg-add-menu').on('click', function (event) {
            event.preventDefault();
            $('#tg-nav, #tg-burger-menu').delay(500).fadeIn();
            $('#tg-additional-nav').fadeOut();
        });
    }
    navFunction();


    $('#btntogglemenu').click(function () {
        $('#tg-navigationmob').toggleClass('pullmobmenuleft');
        //$('#wrapper').toggleClass('pullwrapper');
        $('.overlayadd').toggleClass('clsmenuoverlay');
    });

   
    //Tabs
    var tabFinish = 0;
    $(document).on('click', '.nav-tab-item', function () {
        cearviewpnrdetails(1);
        cearviewpnrdetails(2);
        var $t = $(this);
        if (tabFinish || $t.hasClass('active')) return false;
        tabFinish = 1;
        $t.closest('.nav-tab').find('.nav-tab-item').removeClass('active');
        $t.addClass('active');
        var index = $t.parent().parent().find('.nav-tab-item').index(this);
        $t.closest('.tab-wrapper').find('.tab-info:visible').fadeOut(500, function () {
            $t.closest('.tab-wrapper').find('.tab-info').eq(index).fadeIn(500, function () {
                tabFinish = 0;
            });
        });
    });
    

    /* -------------------------------------
     CATEGORY ICONS
     -------------------------------------- */
    function categoryIcon() {
        $('.tg-nav-tabs li a').on('click', function (event) {
            event.preventDefault();
            var hrefValue = $(this).attr('href').replace('#', '');
            $('.tg-category-icon').find('.active').removeClass('active');
            $('.tg-category-icon div.' + hrefValue).addClass('active');
        })
    }
    categoryIcon();
    $('.tg-category-icon').on('click', '.next', function (e) {
        e.preventDefault();
        var currentActive = $(this).parents('.tg-category-holder').find('.tg-categoriesarea .tg-categories .active');
        var nextActive = currentActive.next();
        var currentHref = nextActive.find('a').attr('href');

        if (currentHref == 'undefined' || currentHref == null) {
            nextActive = $('.tg-category-holder').find('.tg-categoriesarea .tg-categories li').eq(0);
            currentHref = nextActive.find('a').attr('href');
        }

        // Catergoty Title
        var hrefValue = currentHref.replace('#', '');
        $('.tg-category-icon').find('.active').removeClass('active');
        $('.tg-category-icon div.' + hrefValue).addClass('active');
        // Tab List Active
        $('.tg-category-holder').find('.tg-categoriesarea .tg-categories li').removeClass('active');
        nextActive.addClass('active');
        // Content Active
        $('.tg-category-content').find('.tab-pane').removeClass('active').removeClass('in');
        $('.tg-category-content').find(currentHref).addClass('active').addClass('in');
    });
    $('.tg-category-icon').on('click', '.prev', function (e) {
        e.preventDefault();
        var currentActive = $(this).parents('.tg-category-holder').find('.tg-categoriesarea .tg-categories .active');
        var prevActive = currentActive.prev();
        var currentHref = prevActive.find('a').attr('href');

        var currentIndex = currentActive.index();
        if (currentIndex == 0) {
            prevActive = $('.tg-category-holder').find('.tg-categoriesarea .tg-categories li').last();
            currentHref = prevActive.find('a').attr('href');
        }

        // Catergoty Title
        var hrefValue = currentHref.replace('#', '');
        $('.tg-category-icon').find('.active').removeClass('active');
        $('.tg-category-icon div.' + hrefValue).addClass('active');
        // Tab List Active
        $('.tg-category-holder').find('.tg-categoriesarea .tg-categories li').removeClass('active');
        prevActive.addClass('active');
        // Content Active
        $('.tg-category-content').find('.tab-pane').removeClass('active').removeClass('in');
        $('.tg-category-content').find(currentHref).addClass('active').addClass('in');
    });
    /* -------------------------------------
     DESTINATION SLIDER
     -------------------------------------- */
    $("#tg-destination-slider").owlCarousel({
        autoPlay: true,
        items: 3,
        itemsDesktop: [1199, 3],
        itemsDesktopSmall: [991, 3],
        itemsTabletSmall: [767, 2],
        slideSpeed: 300,
        singleItem: false,
        navigation: false,
        pagination: true,
        paginationSpeed: 400,
        navigationText: [
            "<i class='tg-prev flaticon-left-arrow23'></i>",
            "<i class='tg-next flaticon-left-arrow23'></i>"
        ]
    });
    /* -------------------------------------
     VIDEO SLIDER
     -------------------------------------- */
    var swiper = new Swiper(
            '#tg-video-slider',
            {
                pagination: '.swiper-pagination',
                nextButton: '.swiper-button-next',
                prevButton: '.swiper-button-prev',
                paginationClickable: true,
                mousewheelControl: true,
                direction: 'vertical',
                parallax: true,
                speed: 2000
            }
    );
    /* -------------------------------------
     DESTINATION SLIDER
     -------------------------------------- */
    $("#tg_packages_slider").owlCarousel({
        autoPlay: true,
        nav: true,
        loop: true,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 1
            },
            992: {
                items: 4
            }
        },
  
        //itemsDesktop: [1199, 4],
        //itemsDesktopSmall: [991, 3],
        //itemsTabletSmall: [767, 2],
        //slideSpeed: 300,
        //singleItem: false,
        //navigation: false,
        //pagination: true,
        //paginationSpeed: 400,
        navigationText: [
            "<i class='tg-prev flaticon-left-arrow23'></i>",
            "<i class='tg-next flaticon-left-arrow23'></i>"
        ]
    });
    $("#slider_home").owlCarousel({
        autoPlay: true,
        nav: true,
        loop: true,
        responsive: {
            0: {
                items: 3
            },
            600: {
                items: 3
            },
            992: {
                items: 3
            }
        },
  
        //itemsDesktop: [1199, 4],
        //itemsDesktopSmall: [991, 3],
        //itemsTabletSmall: [767, 2],
        //slideSpeed: 300,
        //singleItem: false,
        //navigation: false,
        //pagination: true,
        //paginationSpeed: 400,
        navigationText: [
            "<i class='tg-prev flaticon-left-arrow23'></i>",
            "<i class='tg-next flaticon-left-arrow23'></i>"
        ]
    });

    /* -------------------------------------
     TEAM SLIDER
     -------------------------------------- */
    $("#tg-team-slider").owlCarousel({
        autoPlay: true,
        items: 3,
        itemsDesktop: [1199, 3],
        itemsDesktopSmall: [991, 3],
        itemsTabletSmall: [767, 2],
        slideSpeed: 300,
        singleItem: false,
        navigation: false,
        pagination: true,
        paginationSpeed: 400,
        navigationText: [
            "<i class='tg-prev flaticon-left-arrow23'></i>",
            "<i class='tg-next flaticon-left-arrow23'></i>"
        ]
    });
    /* -------------------------------------
     PARTNER SLIDER
     -------------------------------------- */
    $("#tg-partners-slider").owlCarousel({
        autoPlay: true,
        items: 4,
        itemsDesktop: [1199, 3],
        itemsDesktopSmall: [991, 3],
        itemsTabletSmall: [767, 2],
        slideSpeed: 300,
        singleItem: false,
        navigation: false,
        pagination: true,
        paginationSpeed: 400,
        navigationText: [
            "<i class='tg-prev flaticon-left-arrow23'></i>",
            "<i class='tg-next flaticon-left-arrow23'></i>"
        ]
    });
    /* -------------------------------------
     TESTIMONIALS SLIDER
     -------------------------------------- */
    function testimonialsSlider() {
        var sync1 = $("#tg-testimonials-message-slider");
        var sync2 = $("#tg-testimonials-pagger-slider");

        sync1.owlCarousel({
            singleItem: true,
            slideSpeed: 1000,
            navigation: false,
            pagination: true,
            afterAction: syncPosition,
            responsiveRefreshRate: 200,
        });
        sync2.owlCarousel({
            items: 3,
            itemsDesktop: [1199, 3],
            itemsDesktopSmall: [979, 3],
            itemsTablet: [768, 3],
            itemsMobile: [479, 2],
            pagination: false,
            responsiveRefreshRate: 100,
            afterInit: function (el) {
                el.find(".owl-item").eq(0).addClass("synced");
            }
        });
        function syncPosition(el) {
            var current = this.currentItem;
            $("#tg-testimonials-pagger-slider")
                    .find(".owl-item")
                    .removeClass("synced")
                    .eq(current)
                    .addClass("synced")
            if ($("#tg-testimonials-pagger-slider").data("owlCarousel") !== undefined) {
                center(current)
            }
        }
        $("#tg-testimonials-pagger-slider").on("click", ".owl-item", function (e) {
            e.preventDefault();
            var number = $(this).data("owlItem");
            sync1.trigger("owl.goTo", number);
        });
        function center(number) {
            var sync2visible = sync2.data("owlCarousel").owl.visibleItems;
            var num = number;
            var found = false;
            for (var i in sync2visible) {
                if (num === sync2visible[i]) {
                    var found = true;
                }
            }
            if (found === false) {
                if (num > sync2visible[sync2visible.length - 1]) {
                    sync2.trigger("owl.goTo", num - sync2visible.length + 2)
                } else {
                    if (num - 1 === -1) {
                        num = 0;
                    }
                    sync2.trigger("owl.goTo", num);
                }
            } else if (num === sync2visible[sync2visible.length - 1]) {
                sync2.trigger("owl.goTo", sync2visible[1])
            } else if (num === sync2visible[0]) {
                sync2.trigger("owl.goTo", num - 1)
            }
        }
    }
    testimonialsSlider();
    /* -------------------------------------
     BLOG POST SLIDER
     -------------------------------------- */
    $("#tg-post-slider").owlCarousel({
        autoPlay: false,
        items: 2,
        itemsDesktop: [1199, 2],
        itemsDesktopSmall: [991, 2],
        itemsTabletSmall: [767, 2],
        slideSpeed: 300,
        singleItem: false,
        navigation: false,
        pagination: true,
        paginationSpeed: 400,
        navigationText: [
            "<i class='tg-prev flaticon-left-arrow23'></i>",
            "<i class='tg-next flaticon-left-arrow23'></i>"
        ]
    });
    /* -------------------------------------
     PRETTY PHOTO GALLERY
     -------------------------------------- */
    $("a[data-rel]").each(function () {
        $(this).attr("rel", $(this).data("rel"));
    });
    $("a[data-rel^='prettyPhoto']").prettyPhoto({
        animation_speed: 'normal',
        theme: 'dark_square',
        slideshow: 3000,
        autoplay_slideshow: false,
        social_tools: false
    });
    /* -------------------------------------
     TOGGLE SEARCH RESULT VIEW
     -------------------------------------- */
    function toggleView() {
        $('.tg-view-type li a').on('click', function (event) {
            var hrefValue = $(this).attr('href');
            event.preventDefault();
            $('.tg-view-type li').removeClass('active');
            $(this).parent().addClass('active');
            $('#tg-content .tg-search-result').removeClass('tg-grid-view').addClass(hrefValue);
            $('#tg-content .tg-search-result').removeClass('tg-list-view').addClass(hrefValue);
        });
    }
    toggleView();
    /* -------------------------------------
     PRICE RANGE SLIDER
     -------------------------------------- */
    //function rangSlider() {
    //    $("#tg-rangeradius").slider({
    //        min: 0,
    //        max: 5000,
    //        slide: function (event, ui) {
    //            $("#amount").val("km" + ui.value);
    //        }
    //    });
    //    $("#amount").val("km" + $("#tg-rangeradius").slider("value"));
    //}
    //rangSlider();

//obt3 = new Vivus('obturateur3', {type: 'oneByOne', duration: 150})
    try {
        $('#ps4').appear(function () {
//            alert('test');
            var svg1 = new Walkway({
                selector: '#ps1',
                duration: 3500
            }).draw();
        });
    } catch (err) {
    }
    $('#tg-category-slider li a').on('click', function () {
        var svg1 = new Walkway({
            selector: '#ps1',
            duration: 3500
        }).draw();

        var svg2 = new Walkway({
            selector: '#ps2',
            duration: 3500
        }).draw();

        var svg3 = new Walkway({
            selector: '#ps3',
            duration: 3500
        }).draw();

        var svg4 = new Walkway({
            selector: '#ps4',
            duration: 3500
        }).draw();

        var svg5 = new Walkway({
            selector: '#ps5',
            duration: 3500
        }).draw();

        var svg6 = new Walkway({
            selector: '#ps6',
            duration: 3500
        }).draw();
    });
    $(window).load(function () {
        $('.tg-gallarycontent').isotope({
            itemSelector: '.gallary-item'
        });
        initFullPage();
        $('.loading').fadeOut(700); //Added by sarnaraj on 20170123 for preloader stop...
    });

    /*==============================*/
    /* 06 - FULL PAGE */
    /*==============================*/
    function initFullPage() {
        $('html, body').scrollTop(0);
    }

    $(window).scroll(function () {
        if ($(window).scrollTop() >= 80) {
            if ($('#dvmodifyouter').length)
                $('#dvmodifyouter').addClass('clsstickmodifysearch');
            $('.removexs12').removeClass('col-xs-12');
            $('.removexs12').addClass('col-xs-10');
            $('.removexs12').addClass('marglftoffset1');
            $('#scrolltop').fadeIn();
            $(" #dvFmofify .modEco").css("padding-right", "0px");
            $("#dvFmofify .modexchange").css("left", "-1px")
            $("#dvFmofify .modEco").css("width", "35.6%")
            $("#dvFmofify   .btnpress").css("width", "36.1%")
            $("#dvFmofify .modAdult").css("width", "17%")
        } else {
            if ($('#dvmodifyouter').length)
                $('#dvmodifyouter').removeClass('clsstickmodifysearch');
            $('.removexs12').addClass('col-xs-12');
            $('.removexs12').removeClass('col-xs-10');
            $('.removexs12').removeClass('marglftoffset1');
            $('#scrolltop').fadeOut();
            $(" #dvFmofify .modEco").css("padding-right", "12px");
            $("#dvFmofify .modexchange").css("left", "-10px")
            $("#dvFmofify .modEco").css("width", "36%")
            $("#dvFmofify .modAdult").css("width", "17.2%")
        }
    });

    $('#scrolltop').on('click',function () {
        $("html, body").animate({ scrollTop: 0 }, 600);
        return false;
    });

    /* ---------------------------------------
     GALLERY
     -------------------------------------- */
    var $container = $('.tg-gallarycontent');
    var $optionSets = $('.option-set');
    var $optionLinks = $optionSets.find('a');
    function doIsotopeFilter() {
        if ($().isotope) {
            var isotopeFilter = '';
            $optionLinks.each(function () {
                var selector = $(this).attr('data-filter');
                var link = window.location.href;
                var firstIndex = link.indexOf('filter=');
                if (firstIndex > 0) {
                    var id = link.substring(firstIndex + 7, link.length);
                    if ('.' + id == selector) {
                        isotopeFilter = '.' + id;
                    }
                }
            });
            $container.isotope({
                itemSelector: '.gallary-item',
                filter: isotopeFilter
            });
            $optionLinks.each(function () {
                var $this = $(this);
                var selector = $this.attr('data-filter');
                if (selector == isotopeFilter) {
                    if (!$this.hasClass('active')) {
                        var $optionSet = $this.parents('.option-set');
                        $optionSet.find('.active').removeClass('active');
                        $this.addClass('active');
                    }
                }
            });
            $optionLinks.on('click', function () {
                var $this = $(this);
                var selector = $this.attr('data-filter');
                $container.isotope({itemSelector: '.gallary-item', filter: selector});
                if (!$this.hasClass('active')) {
                    var $optionSet = $this.parents('.option-set');
                    $optionSet.find('.active').removeClass('active');
                    $this.addClass('active');
                }
                return false;
            });
        }
    }
    var isotopeTimer = window.setTimeout(function () {
        window.clearTimeout(isotopeTimer);
        doIsotopeFilter();
    }, 1000);
    /* -------------------------------------
     COUNTER
     -------------------------------------- */
    try {
        $('.tg-counters').appear(function () {
            $('.tg-timer').countTo()
        });
    } catch (err) {
    }
    /* -------------------------------------
     Google Map
     -------------------------------------- */
    //$("#tg-location-map").gmap3({
    //    marker: {
    //        address: "1600 Elizabeth St, Melbourne, Victoria, Australia",
    //        options: {
    //            title: "Robert Frost Elementary School",
    //            icon: new google.maps.MarkerImage("../images/map-marker.png"),
    //        }
    //    },
    //    map: {
    //        options: {
    //            zoom: 16,
    //            scrollwheel: false,
    //            disableDoubleClickZoom: true,
    //        }
    //    }
    //});
});