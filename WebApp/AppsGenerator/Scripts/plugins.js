(function ($) {
$.fn.loading = function (options) {
    $.blockUI({ message: '<h1><img src="../Content/ajax-loader.gif" /></h1>' });
};


$.fn.clearLoading = function (options) {
    $.unblockUI();
};

})(jQuery);