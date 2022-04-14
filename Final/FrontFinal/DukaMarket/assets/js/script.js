const swiper = new Swiper(".swiper", {
  // Optional parameters
  direction: "horizontal",
  loop: true,

  // If we need pagination
  pagination: {
    el: ".swiper-pagination",
  },

  // Navigation arrows
  navigation: {
    nextEl: ".swiper-button-next",
    prevEl: ".swiper-button-prev",
  },
});

//   Mobile Menu
$(".mobile-menu-icon-1").click(function () {
  // $(".mobile-menu-block").css("width","100%");
  // $(".mobile-menu-block").css("visibility","visible");
  // $(".mobile-menu-block").css("opacity","1");
  $(".mobile-menu-block").css("display", "block");
  $(".mobile-menu-icon-1").css("display", "none");
  $(".mobile-menu-icon-2").css("display", "block");
});
$(".mobile-menu-icon-2").click(function () {
  // $(".mobile-menu-block").css("width","0");
  // $(".mobile-menu-block").css("visibility","hidden");
  // $(".mobile-menu-block").css("opacity","0");
  $(".mobile-menu-block").css("display", "none");
  $(".mobile-menu-icon-1").css("display", "block");
  $(".mobile-menu-icon-2").css("display", "none");
});
$(".product-description-header").click(function (e) {
  e.preventDefault();
  $(".product-additional-info-body").css("display", "none");
  $(".product-reviews-body").css("display", "none");
  $(".product-description-body").css("display", "block");
  $(".product-description-header").css("color","#fcbe00");
  $(".product-reviews-header").css("color","#b6b6b6");
$(".product-additional-info-header").css("color","#b6b6b6");
});
$(".product-additional-info-header").click(function (e) {
  e.preventDefault();
  $(".product-additional-info-body").css("display", "block");
  $(".product-reviews-body").css("display", "none");
  $(".product-description-body").css("display", "none");
  $(".product-description-header").css("color","#b6b6b6");
  $(".product-reviews-header").css("color","#b6b6b6");
  $(".product-additional-info-header").css("color","#fcbe00");
});
$(".product-reviews-header").click(function (e) {
  e.preventDefault();
  $(".product-additional-info-body").css("display", "none");
  $(".product-reviews-body").css("display", "block");
  $(".product-description-body").css("display", "none");
  $(".product-description-header").css("color","#b6b6b6");
  $(".product-reviews-header").css("color","#fcbe00");
  $(".product-additional-info-header").css("color","#b6b6b6");
});
$(".dashboard-link").click(function (e) {
  e.preventDefault();
  $(".dashboard-body").css("display", "block");
  $(".orders-body").css("display", "none");
  $(".personal-body").css("display", "none");
});
$(".orders-link").click(function (e) {
  e.preventDefault();
  $(".dashboard-body").css("display", "none");
  $(".orders-body").css("display", "block");
  $(".personal-body").css("display", "none");
});
$(".personal-link").click(function (e) {
  e.preventDefault();
  $(".dashboard-body").css("display", "none");
  $(".orders-body").css("display", "none");
  $(".personal-body").css("display", "block");
});
// Mobile Menu Js End
// Accordion
$(document).ready(function () {
  $(".accordionheader").click(function (e) {
    $(".accordionbody").not($(this).next()).slideUp();
    $(this).next().slideToggle();
  });
});
$(document).ready(function () {
  $(".minus").click(function () {
    var $input = $(this).parent().find("input");
    var count = parseInt($input.val()) - 1;
    count = count < 1 ? 1 : count;
    $input.val(count);
    $input.change();
    return false;
  });
  $(".plus").click(function () {
    var $input = $(this).parent().find("input");
    $input.val(parseInt($input.val()) + 1);
    $input.change();
    return false;
  });
});
var lowerSlider = document.querySelector("#lower");
var upperSlider = document.querySelector("#upper");

document.querySelector("#two").value = upperSlider.value;
document.querySelector("#one").value = lowerSlider.value;

var lowerVal = parseInt(lowerSlider.value);
var upperVal = parseInt(upperSlider.value);

upperSlider.oninput = function () {
  lowerVal = parseInt(lowerSlider.value);
  upperVal = parseInt(upperSlider.value);

  if (upperVal < lowerVal + 4) {
    lowerSlider.value = upperVal - 4;
    if (lowerVal == lowerSlider.min) {
      upperSlider.value = 4;
    }
  }
  document.querySelector("#two").value = this.value;
};

lowerSlider.oninput = function () {
  lowerVal = parseInt(lowerSlider.value);
  upperVal = parseInt(upperSlider.value);
  if (lowerVal > upperVal - 4) {
    upperSlider.value = lowerVal + 4;
    if (upperVal == upperSlider.max) {
      lowerSlider.value = parseInt(upperSlider.max) - 4;
    }
  }
  document.querySelector("#one").value = this.value;
};
	// brand slider active js
	$('.brand-active-carousel').slick({
		arrows: false,
		slidesToShow: 4,
		responsive: [
			{
				breakpoint: 992,
				settings: {
					slidesToShow: 2
				}
			},
			{
				breakpoint: 480,
				settings: {
					slidesToShow: 1
				}
			}
		]
	});