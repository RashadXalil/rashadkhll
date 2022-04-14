window.addEventListener("DOMContentLoaded", () => {
  $("#loadingwrapper").css({
    display: "block",
    visibility: "visible",
    opacity: "1",
  })
  setTimeout(loading,500)
});
function loading(){
  $("#loadingwrapper").fadeOut(700,function() {
    $("#loadingwrapper").remove();
  })
}
let nav = document.querySelector("#mynav");
window.addEventListener("scroll", function (e) {
  if (window.scrollY > 50) {
    nav.style.backgroundColor = "white";
    nav.style.height = "80px";
    let button = this.document.querySelector(("#signup"))
    nav.style.boxShadow =
      "rgba(100, 100, 111, 0.2) 0px 7px 29px 0px";
  }
  if (window.scrollY < 50) {
    nav.style.backgroundColor = "transparent";
    nav.style.boxShadow = "none";
  }
});
$(document).ready(function () {
  AOS.init({
    duration: 1000,
  });
});
$(document).ready(function(){
  $(".accordionheader").click(function(e){
    $(".accordionbody").not($(this).next()).slideUp();
      $(this).next().slideToggle();
  })
});
$(document).ready(function(){ 
  $(".hamburger").click(function(e){
    if(!$(".accordion").hasClass(".active")){
      $(".accordion").css("display","block");
      $(".accordion").addClass(".active");
    }
    else{
      $(".accordion").css("display","none");
      $(".accordion").removeClass(".active")
    }
  })
});
$(".num").counterUp({delay:10,time:1000})