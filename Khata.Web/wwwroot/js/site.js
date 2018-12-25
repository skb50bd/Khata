// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


$(document).ready(function () {
    $(".js-clickable-row").click(function () {
        window.location = $(this).data("href");
    });

    $(".js-remove-item").click(function (e) {
        if (confirm("Are you sure you want to remove the item?")) {
            $.ajax({ url: $(e.target).attr("data-href"), method: "DELETE" })
                .done(function () {
                    alert("Success!");
                })
                .fail(function () {
                    alert("Something Failed");
                });
        }
    });
});