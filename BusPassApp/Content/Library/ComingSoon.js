(function ($) {
    "use strict";


    /*-------------------------------------
      Vegas Slider
      -------------------------------------*/
    if ($.fn.vegas !== undefined && $("#vegas-slide").length) {
        var target_slider = $("#vegas-slide"),
            vegas_options = target_slider.data('vegas-options');
        if (typeof vegas_options === "object") {
            target_slider.vegas(vegas_options);
        }
    }

    /*-------------------------------------
      Animated Headline
      -------------------------------------*/

    if ($.fn.animatedHeadline !== undefined && $(".ah-animate").length) {
        var target_slider = $(".ah-animate"),
            ah_options = target_slider.data('line-options');
        if (typeof ah_options === "object") {
            target_slider.animatedHeadline(ah_options);
        }
    }

    /*-------------------------------------
      Section background image
      -------------------------------------*/
    $("[data-bg-image]").each(function () {
        var img = $(this).data("bg-image");
        $(this).css({
            backgroundImage: "url(" + img + ")"
        });
    });

    /*-------------------------------------
       Youtube Video
    -------------------------------------*/
    if ($.fn.YTPlayer !== undefined) {
        $('.youtube-video').each(function () {
            var self = $(this),
                videoId = self.data("video-id");
            self.YTPlayer({
                mute: false,
                fitToBackground: true,
                videoId: videoId,
                playerVars: {
                    modestbranding: 0,
                    autoplay: 1,
                    controls: 0,
                    showinfo: 0,
                    branding: 0,
                    frameborder: 0,
                    loop: 1,
                    rel: 0,
                    autohide: 0,
                    start: 1,
                    height: 1,
                }
            });
        })
        $('.youtube-video2').each(function () {
            var self = $(this),
                videoId = self.data("video-id");
            self.YTPlayer({
                mute: false,
                fitToBackground: true,
                videoId: videoId,
                playerVars: {
                    modestbranding: 0,
                    autoplay: 1,
                    controls: 0,
                    showinfo: 0,
                    branding: 0,
                    frameborder: 0,
                    loop: 1,
                    rel: 0,
                    autohide: 0,
                    start: 19,
                    height: 1,
                }
            });
        })
    }

    $(function () {

        if ($.fn.ripples !== undefined) {
            $('#wrapper').ripples({
                resolution: 356,
                dropRadius: 20,
                perturbance: 0.04,
            });
        }
        /*-------------------------------------
            Countdown activation code
        -------------------------------------*/
        var eventCounter = $(".countdown");
        if (eventCounter.length) {
            eventCounter.countdown("2019/12/12", function (e) {
                $(this).html(
                    e.strftime(
                        "<div class='countdown-section'><div><div class='countdown-number'>%D</div> <div class='countdown-unit'>Day%!D</div> </div></div><div class='countdown-section'><div><div class='countdown-number'>%H</div> <div class='countdown-unit'>Hour%!H</div> </div></div><div class='countdown-section'><div><div class='countdown-number'>%M</div> <div class='countdown-unit'>Minutes</div> </div></div><div class='countdown-section'><div><div class='countdown-number'>%S</div> <div class='countdown-unit'>Second</div> </div></div>"
                    )
                );
            });
        }
    });
})(jQuery);