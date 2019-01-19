// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


$(document).ready(function () {
    // Sidebar

    $('#sidebarCollapse').on('click', function () {
        $('#sidebar, #content').toggleClass('active');
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });

    // Sidebar end

    //$('.collapse').collapse();

    $('.datepicker').datepicker();
    $('.datepicker').datepicker("option", "dateFormat", "dd/mm/yy");

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

    // Supplier Payment

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

    // Supplier Payment End


    // Salary Payment 

    $("#EmployeeSelector").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: $("#EmployeeSelector").attr("data-path"),
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
                url: '/api/Employees/' + ui.item.value,
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    $('#BalanceBefore').val(data.Balance);
                    updateBalance();
                }
            });

            $('#EmployeeSelector').val(ui.item.label);
            $('#EmployeeId').val(ui.item.value);

            return false;
        }
    });

    function updateBalance() {
        var bbVal = Number($('#BalanceBefore').val());
        var aVal = Number($('#Amount').val());
        if ($('#Amount').attr('data-type') === 'issue')
            aVal = -1 * aVal;

        var result = bbVal - aVal;
        $('#BalanceAfter').val(result);
    }

    $(document).on("change, keyup", "#BalanceBefore", updateBalance);
    $(document).on("change, keyup", "#Amount", updateBalance);

    // Salary Payment End
});