function loaderShow() {
    $("#laoder").removeClass('tp-hide');
}
function loaderHide() {
    $("#laoder").addClass('tp-hide');
}
  window.dataLayer = window.dataLayer || [];
  function gtag(){dataLayer.push(arguments);}
  gtag('js', new Date());

  gtag('config', 'UA-153182251-1');
window.fbAsyncInit = function () {
    FB.init({
        appId: '3242127552528704',
        status: true,
        cookie: true,
        xfbml: true,
        version: 'v2.4'
    });
};
(function () {
    var e = document.createElement('script'); e.async = true;
    e.src = document.location.protocol +
        '//connect.facebook.net/en_US/sdk.js';
    document.getElementById('head').appendChild(e);
}());
(function () {
 $('body,html').animate({
     scrollTop: 0
 }, 800);
 $(window).scroll(function () {
     if ($(this).scrollTop() > 50) {
         $('#btntop').removeClass('tp-hide');
         $('#btntop').addClass('tp-show');
     }
     else {
         $('#btntop').removeClass('tp-show');
         $('#btntop').fadeOut('tp-hide');
     }
 });
 $('#btntop').click(function (e) {
     $('body,html').animate({
         scrollTop: 0
     }, 800);
 });
 $('#btnSubmit').click(function (e) {
     var obj = {};
     obj.Name = $('#name').val();
     obj.Email = $('#txtemail').val();
     obj.Phone = $('#txtphone').val();
     obj.Message = $('#txtmsg').val();
     $.ajax({
         url: '/Home/SendEmail',
         type: 'Post',
         data: JSON.stringify(obj),
         dataType: 'json',
         contentType: "application/json",
         success: function (data, status, xhr) {
             alert("We have receieved your concern ! our team well get back to you shortly..");
         },
         error: function (data, status, xhr) {

         }
     });
 });
})();

