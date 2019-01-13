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

    $("#CustomerSelector").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: $("#CustomerSelector").attr("data-path"),
                type: 'GET',
                cache: true,
                data: request,
                dataType: 'json',
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.label,
                            value: item.id
                        };
                    }));
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            $.ajax({
                url: '/api/Customers/' + ui.item.value,
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    $('#DebtBefore').val(data.Debt);
                    updateDebt();
                }
            });

            $('#CustomerSelector').val(ui.item.label);
            $('#CustomerId').val(ui.item.value);
            
            return false;
        }
    });

    function updateDebt() {
        var dbVal = Number($('#DebtBefore').val());
        var aVal = Number($('#Amount').val());
        var result = dbVal - aVal;
        $('#DebtAfter').val(result);
    }    

    $(document).on("change, keyup", "#DebtBefore", updateDebt);
    $(document).on("change, keyup", "#Amount", updateDebt);

    $("#SupplierSelector").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: $("#SupplierSelector").attr("data-path"),
                type: 'GET',
                cache: true,
                data: request,
                dataType: 'json',
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.label,
                            value: item.id
                        };
                    }));
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            $.ajax({
                url: '/api/Suppliers/' + ui.item.value,
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    $('#PayableBefore').val(data.Payable);
                    updatePayable();
                }
            });

            $('#SupplierSelector').val(ui.item.label);
            $('#SupplierId').val(ui.item.value);

            return false;
        }
    });

    function updatePayable() {
        var dbVal = Number($('#PayableBefore').val());
        var aVal = Number($('#Amount').val());
        var result = dbVal - aVal;
        $('#PayableAfter').val(result);
    }

    $(document).on("change, keyup", "#PayableBefore", updatePayable);
    $(document).on("change, keyup", "#Amount", updatePayable);
});