$(document).ready(function () {
  loadStock();
});

$("input[name='colorId']").on('change', function () {
  loadStock();
});

function loadStock() {
  $.ajax({
    url: '/Customer/Api/Stock',
    data: {
      productId: $('.product-id').val(),
        colorId: $("input[name='colorId']:checked").val(),
    },
    success: function (res) {
      $('.product-carousel').html(getCarousel(res.images));
      $('.owl-thumbs').html(getThumbs(res.images));
      $('.flex-item input').attr('disabled', true);
      for (item of res.sizes) {
        const id = '#' + item.sizeId;
        $(id).attr('disabled', false);
      }

      $(function () {
        $('.shop-detail-carousel').owlCarousel({
          items: 1,
          thumbs: true,
          nav: false,
          dots: false,
          loop: true,
          autoplay: true,
          thumbsPrerendered: true,
        });

        $('#main-slider').owlCarousel({
          items: 1,
          nav: false,
          dots: true,
          autoplay: true,
          autoplayHoverPause: true,
          dotsSpeed: 400,
        });

        $('#get-inspired').owlCarousel({
          items: 1,
          nav: false,
          dots: true,
          autoplay: true,
          autoplayHoverPause: true,
          dotsSpeed: 400,
        });

        $('.product-slider').owlCarousel({
          items: 1,
          dots: true,
          nav: false,
          responsive: {
            480: {
              items: 1,
            },
            765: {
              items: 2,
            },
            991: {
              items: 3,
            },
            1200: {
              items: 5,
            },
          },
        });

        // productDetailGallery(4000);
        utils();

        // ------------------------------------------------------ //
        // For demo purposes, can be deleted
        // ------------------------------------------------------ //

        var stylesheet = $('link#theme-stylesheet');
        $("<link id='new-stylesheet' rel='stylesheet'>").insertAfter(
          stylesheet
        );
        var alternateColour = $('link#new-stylesheet');

        if ($.cookie('theme_csspath')) {
          alternateColour.attr('href', $.cookie('theme_csspath'));
        }

        $('#colour').change(function () {
          if ($(this).val() !== '') {
            var theme_csspath = 'css/style.' + $(this).val() + '.css';

            alternateColour.attr('href', theme_csspath);

            $.cookie('theme_csspath', theme_csspath, {
              expires: 365,
              path: document.URL.substr(0, document.URL.lastIndexOf('/')),
            });
          }

          return false;
        });
      });

      $(window).on('load', function () {
        $(this).alignElementsSameHeight();
      });

      $(window).resize(function () {
        setTimeout(function () {
          $(this).alignElementsSameHeight();
        }, 150);
      });

      function utils() {
        /* click on the box activates the radio */

        $('#checkout').on(
          'click',
          '.box.shipping-method, .box.payment-method',
          function (e) {
            var radio = $(this).find(':radio');
            radio.prop('checked', true);
          }
        );
        /* click on the box activates the link in it */

        $('.box.clickable').on('click', function (e) {
          window.location = $(this).find('a').attr('href');
        });
        /* external links in new window*/

        $('.external').on('click', function (e) {
          e.preventDefault();
          window.open($(this).attr('href'));
        });
        /* animated scrolling */

        $('.scroll-to, .scroll-to-top').click(function (event) {
          var full_url = this.href;
          var parts = full_url.split('#');
          if (parts.length > 1) {
            scrollTo(full_url);
            event.preventDefault();
          }
        });

        function scrollTo(full_url) {
          var parts = full_url.split('#');
          var trgt = parts[1];
          var target_offset = $('#' + trgt).offset();
          var target_top = target_offset.top - 100;
          if (target_top < 0) {
            target_top = 0;
          }

          $('html, body').animate(
            {
              scrollTop: target_top,
            },
            1000
          );
        }
      }

      $.fn.alignElementsSameHeight = function () {
        $('.same-height-row').each(function () {
          var maxHeight = 0;

          var children = $(this).find('.same-height');

          children.height('auto');

          if ($(document).width() > 768) {
            children.each(function () {
              if ($(this).innerHeight() > maxHeight) {
                maxHeight = $(this).innerHeight();
              }
            });

            children.innerHeight(maxHeight);
          }

          maxHeight = 0;
          children = $(this).find('.same-height-always');

          children.height('auto');

          children.each(function () {
            if ($(this).innerHeight() > maxHeight) {
              maxHeight = $(this).innerHeight();
            }
          });

          children.innerHeight(maxHeight);
        });
      };
    },
    error: function (e) {
      console.log(e);
    },
  });
}

function getCarousel(images) {
  res = `<div data-slider-id="1" class="owl-carousel shop-detail-carousel">`;
  for (item of images) {
    res += ` <div class="item"> <img src="${item}" alt="" class="img-fluid"></div>`;
  }
  res += `</div>`;
  return res;
}
function getThumbs(images) {
  res = ``;
  for (item of images) {
    res += ` <button class="owl-thumb-item"><img src="${item}" style="width: 110px;" alt="" class="img-fluid"></button>`;
  }
  return res;
}
