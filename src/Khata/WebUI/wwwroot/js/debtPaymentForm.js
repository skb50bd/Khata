$(document).ready(function () {
    
    function updateDebt() {
        const dbVal = Number($("#DebtBefore").val());
        const aVal = Number($("#Amount").val());
        const result = dbVal - aVal;
        $("#DebtAfter").val(result);
    }

    $("#CustomerSelector").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: $("#CustomerSelector").attr("data-path"),
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
                url: `/api/Customers/${ui.item.value}`,
                type: "GET",
                dataType: "json",
                success: function (data) {
                    $("#DebtBefore").val(data.debt);
                    updateDebt();
                }
            });

            $("#CustomerSelector").val(ui.item.label);
            $("#customer-id").val(ui.item.value);

            return false;
        }
    });

    $(document).on("change, keyup", "#DebtBefore", updateDebt);
    $(document).on("change, keyup", "#Amount", updateDebt);
});