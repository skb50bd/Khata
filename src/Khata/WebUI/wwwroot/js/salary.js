$(document).ready(function () {
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
                    $('#BalanceBefore').val(data.balance);
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

    var buttons = document.getElementsByClassName('blank-link');
    for (var i = 0; i < buttons.length; i++) {
        buttons[i].addEventListener('click', function (event) {
            e.preventDefault();
        });
    }
});