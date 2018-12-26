// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


$(document).ready(function () {
    $(".js-clickable-row").click(function () {
        window.location = $(this).data("href");
    });

    $(".js-remove-item").click(function (e) {
        swal({
            title: "Are you sure?",
            text: "The item will be removed from records!",
            icon: "warning",
            buttons: true,
            dangerMode: true
        })
            .then((willDelete) => {
                if (willDelete) {
                    $.ajax({ url: $(e.target).attr("data-href"), method: "DELETE" })
                        .done(function () {
                            swal("Poof! The item is removed!", {
                                icon: "success"
                            })
                                .then((value) => {
                                    if (value)
                                        window.location = $(e.target).attr("data-returnUrl");
                                });
                        })
                        .fail(function () {
                            swal("Fail", "Could not delete the item.", "error");
                        });

                } else {
                    swal("The item is safe!");
                }
            });
    });
});