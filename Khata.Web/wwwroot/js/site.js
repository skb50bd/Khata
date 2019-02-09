﻿const swalDelete = Swal.mixin({
    confirmButtonClass: 'btn btn-danger',
    cancelButtonClass: 'btn btn-secondary mr-4',
    buttonsStyling: false
});

const swalSave = Swal.mixin({
    confirmButtonClass: 'btn btn-success',
    cancelButtonClass: 'btn btn-danger mr-4',
    buttonsStyling: false
});

function attachClickableRow()  {
    var ref = $(this).data("href");
    window.location.href = ref;
}

$(document).ready(function () {
    //$('.collapse').collapse();
    $(".immutable-submit").submit(function (event) {
        event.preventDefault();
        swalSave.fire({
            title: 'Are you sure?',
            text: "Are you sure want to save thre record?",
            type: 'info',
            showCancelButton: true,
            confirmButtonText: 'Yes, save it!',
            cancelButtonText: 'No, cancel!',
            reverseButtons: true
        }).then((result) => {
            if (result.value) {
                this.submit(true);
            }
            else {
                swalSave.fire(
                    'Cacelled',
                    'Item not saved!',
                    'info'
                );
            }
        });
    });

    $('.datepicker').datepicker();
    $('.datepicker').datepicker("option", "dateFormat", "dd/mm/yy");

    $(".js-clickable-row").click(attachClickableRow);

    $(".js-clickable-transaction").click(function () {
        window.location = "/" + $(this).data("table") + "/Details?id=" + $(this).data("row");
    });

    $(".js-remove-item").click(function (e) {
        swalDelete.fire({
            title: 'Are you sure?',
            text: "The item will be removed from records!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, remove it!',
            cancelButtonText: 'No, cancel!',
            reverseButtons: true
        }).then((result) => {
            if (result.value) {
                $.ajax({ url: $(e.target).attr("data-href"), method: "DELETE" })
                    .done(function () {
                        swalDelete.fire(
                            'Removed!',
                            'Your file has been removed.',
                            'success'
                        ).then((value) => {
                            if (value)
                                window.location = $(e.target).attr("data-returnUrl");
                        });
                    })
                    .fail(function () {
                        swalDelete.fire(
                            'Failed',
                            'Could not remove the item :(',
                            'warning'
                        );
                    });
            } else if (
                // Read more about handling dismissals
                result.dismiss === Swal.DismissReason.cancel
            ) {
                swalDelete.fire(
                    'Cancelled',
                    'Your file is safe :)',
                    'error'
                );
            }
        });
    });


    $(function () {
        $('[data-toggle="popover"]').popover();
    });

    $('.popover-dismiss').popover({
        trigger: 'hover'
    });

    $(function () {
        $('[data-toggle="tooltip"]').tooltip();
    });

    $(".ajax-tab").click((e) => {
        fetch($(e.target).attr("data-partial-source"))
            .then((response) => {
                return response.text();
            })
            .then((result) => {
                var id = $(e.target).attr("aria-controls");
                var d = document.getElementById(id);
                d.innerHTML = result;
            }).then(() =>
                $(".js-clickable-row")
                    .click(attachClickableRow));
    });
});

