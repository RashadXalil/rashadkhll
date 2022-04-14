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
$(document).ready(function () {
    $("#searchString").on("keyup", function () {
        var formSearch = document.getElementById("searchProd");
        var url = formSearch.action;

        var value = $(this).val().toLowerCase();

        if (value.length < 3) {
            if (value == undefined || value == null || value == "") {
                $(".search-result").children().remove();
                $(".search-result").css("display", "none");
            }
            return;
        }

        if (value == undefined || value == null || value == "") {
            $(".search-result").children().remove();
            $(".search-result").css("display", "none");
            return;
        }

        const formData = new FormData();

        formData.append("searchStr", value)

        fetch(url, {
            method: "POST",
            body: formData,
            headers: {
                'Accept': 'application/json'
            }
        })
            .then(response => {
                if (!response.ok) {
                    throw response
                }
                return response.text();
            })
            .then(data => {
                $(".search-result").html(data);
                $(".search-result").css("display", "block")
            })
    })
    window.addEventListener("DOMContentLoaded", () => {
        $("#loadingwrapper").css({
            display: "block",
            visibility: "visible",
            opacity: "1",
        })
        setTimeout(loading,500)
    });
    function loading() {
        $("#loadingwrapper").fadeOut(700, function () {
            $("#loadingwrapper").remove();
        })
    }
    $(document).on("click", ".add-to-basket", function (e) {
        e.preventDefault();
        let url = $(this).attr("href");
        console.log(url)
        fetch(url)
            .then(function (response) {
                return response.text();
            }).then(data => {
                $(".basket-block").html(data)
            });
    })
    $(document).on("click", ".add-to-wishlist", function (e) {
        e.preventDefault();
        let url = $(this).attr("href");
        console.log(url)
        fetch(url)
            .then(function (response) {
                return response.text();
            }).then(data => {
                $(".wishlist-block").html(data)
            });
    })
    $(document).on("click", ".remove-basket", function (e) {
        e.preventDefault();
        let path = $(this).attr("href");
        fetch(path)
            .then(response => {
                return response.text();
            }).then(data => {
                $(".basket-block").html(data)
            })
    })

    $(document).ready(function () {
        $(document).on("click", ".product-image-item-0", function (e) {
            console.log("hello")
            $(".image-block-0").css("display", "block")
            $(".image-block-1").css("display", "none")
            $(".image-block-2").css("display", "none")
        })
        $(document).on("click", ".product-image-item-1", function (e) {
            console.log("hello")
            $(".image-block-0").css("display", "none")
            $(".image-block-1").css("display", "block")
            $(".image-block-2").css("display", "none")
        })
        $(document).on("click", ".product-image-item-2", function (e) {
            console.log("hello")
            $(".image-block-0").css("display", "none")
            $(".image-block-1").css("display", "none")
            $(".image-block-2").css("display", "block")
        })

    });
    $(document).on("click", ".quick-view", function (e) {
        e.preventDefault();

        let url = $(this).attr("href");

        fetch(url)
            .then(response => response.text())
            .then(data => {
                $(".quick-view-body").html(data)
            });
    })
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
        $(".mobile-menu-block").css("display", "none");
        $(".mobile-menu-icon-1").css("display", "block");
        $(".mobile-menu-icon-2").css("display", "none");
    });
    $(".product-description-header").click(function (e) {
        e.preventDefault();
        $(".product-additional-info-body").css("display", "none");
        $(".product-reviews-body").css("display", "none");
        $(".product-description-body").css("display", "block");
        $(".product-description-header").css("color", "#fcbe00");
        $(".product-reviews-header").css("color", "#b6b6b6");
        $(".product-additional-info-header").css("color", "#b6b6b6");
    });
    $(".product-additional-info-header").click(function (e) {
        e.preventDefault();
        $(".product-additional-info-body").css("display", "block");
        $(".product-reviews-body").css("display", "none");
        $(".product-description-body").css("display", "none");
        $(".product-description-header").css("color", "#b6b6b6");
        $(".product-reviews-header").css("color", "#b6b6b6");
        $(".product-additional-info-header").css("color", "#fcbe00");
    });

    $(".product-reviews-header").click(function (e) {
        e.preventDefault();
        $(".product-additional-info-body").css("display", "none");
        $(".product-reviews-body").css("display", "block");
        $(".product-description-body").css("display", "none");
        $(".product-description-header").css("color", "#b6b6b6");
        $(".product-reviews-header").css("color", "#fcbe00");
        $(".product-additional-info-header").css("color", "#b6b6b6");
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
    $(".quick-modal").click(function (e) {
        e.preventDefault();
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
        $(".nav-link-1").click(function (e) {
            $(".tab-content-1").css("display", "block")
            $(".tab-content-2").css("display", "none")
            $(".tab-content-3").css("display", "none")
        });
        $(".nav-link-2").click(function (e) {
            $(".tab-content-1").css("display", "none")
            $(".tab-content-2").css("display", "block")
            $(".tab-content-3").css("display", "none")
        });
        $(".nav-link-3").click(function (e) {
            $(".tab-content-1").css("display", "none")
            $(".tab-content-2").css("display", "none")
            $(".tab-content-3").css("display", "block")
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
})

