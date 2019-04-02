$(document).ready(function () {
    $("#SupplierSelector").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: $("#SupplierSelector").attr("data-path"),
                type: "GET",
                cache: true,
                data: request,
                dataType: "json",
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
                url: "/api/Suppliers/" + ui.item.value,
                type: "GET",
                dataType: "json",
                success: function (data) {
                    $("#PayableBefore").val(data.payable);
                    updatePayable();
                }
            });

            $("#SupplierSelector").val(ui.item.label);
            $("#SupplierId").val(ui.item.value);

            return false;
        }
    });

    function updatePayable() {
        var dbVal = Number($("#PayableBefore").val());
        var aVal = Number($("#Amount").val());
        var result = dbVal - aVal;
        $("#PayableAfter").val(result);
    }

    $(document).on("change, keyup", "#PayableBefore", updatePayable);
    $(document).on("change, keyup", "#Amount", updatePayable);
    
});