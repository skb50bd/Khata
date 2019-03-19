const sleep = (milliseconds) => {
    return new Promise(resolve => setTimeout(resolve, milliseconds));
};

const formatter = new Intl.NumberFormat('en-BD', {
    style: 'currency',
    currency: 'BDT',
    currencyDisplay: 'symbol',
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
});

function toFixedIfNecessary(value, dp) {
    return parseFloat(value).toFixed(dp);
}

function getDate(elem = null) {
    var date;
    try {
        date = $.datepicker.parseDate("dd/mm/yy", elem.value);
    } catch (error) {
        date = new Date();
    }

    var year = date.getFullYear(),
        month = (date.getMonth() + 1).toString(),
        formatedMonth = month.length === 1 ? "0" + month : month,
        day = date.getDate().toString(),
        formatedDay = day.length === 1 ? "0" + day : day;
    return formatedDay + "/" + formatedMonth + "/" + year;
}


const swalDelete = Swal.mixin({
    confirmButtonClass: 'btn btn-danger',
    cancelButtonClass: 'btn btn-secondary mr-4',
    buttonsStyling: false
});

const swalSave = Swal.mixin({
    confirmButtonClass: 'btn btn-success',
    cancelButtonClass: 'btn btn-danger mr-4',
    buttonsStyling: false
});

function attachClickableRow() {
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
        $.ajax({
            url: $(e.target).attr("data-partial-source"),
            type: 'GET',
            dataType: 'html',
            success: function (response) {
                var id = $(e.target).attr("aria-controls");
                var d = document.getElementById(id);
                d.innerHTML = response;
            }
        }).then(() =>
            $(".js-clickable-row")
                .click(attachClickableRow));
    });

    $(function () {
        from = $("#from")
            .datepicker()
            .on("change", function () {
                to.datepicker("option", "minDate", getDate(this));
            });
        to = $("#to").datepicker()
            .on("change", function () {
                from.datepicker("option", "maxDate", getDate(this));
            });
    });

    $(function () {
        $('#fromDate').datetimepicker({
            format: 'DD/MM/YYYY',
            useCurrent: true
        });
        $('#toDate').datetimepicker({
            format: 'DD/MM/YYYY',
            useCurrent: false
        });
        $("#fromDate").on("change.datetimepicker", function (e) {
            $('#datetimepicker8').datetimepicker('minDate', e.date);
        });
        $("#toDate").on("change.datetimepicker", function (e) {
            $('#fromDate').datetimepicker('maxDate', e.date);
        });
    });

    var inputs = $(':input').keypress(function (e) {
        if (e.which === 13) {
            var nextInput = inputs.get(inputs.index(this) + 1);
            if (nextInput) {
                if (nextInput.getAttribute('type') !== 'submit'
                    && e.target.getAttribute('type') !== 'submit'
                    && !$(e.target).is('textarea')
                    && !$(nextInput).is('button')) {
                    e.preventDefault();
                    nextInput.focus();
                }
            }
        }
    });
});

