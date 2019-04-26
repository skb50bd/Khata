const immutableSubmits      = gecn("immutable-submit");
const fromDate              = gei("from-date");
const toDate                = gei("to-date");
const blankButtons          = gecn("blank-link-button");
const inputs                = $(":input");
const ajaxTabs              = gecn("ajax-tab");
const clickableRows         = gecn("js-clickable-row");
const clickableTransactions = gecn("js-clickable-transaction");
const removableItems        = gecn("js-remove-item");

function getDate(elem = null) {
    var date;
    try {
        date = $.datepicker.parseDate("dd/mm/yy", elem.value);
    } catch (error) {
        date = new Date();
    }

    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString();
    const formattedMonth = month.length === 1 ? `0${month}` : month;
    const day = date.getDate().toString();
    const formattedDay = day.length === 1 ? `0${day}` : day;
    return formattedDay + "/" + formattedMonth + "/" + year;
}

const swalDelete = Swal.mixin({
    confirmButtonClass: "btn btn-danger",
    cancelButtonClass: "btn btn-secondary mr-4",
    buttonsStyling: false
});

const swalSave = Swal.mixin({
    confirmButtonClass: "btn btn-success",
    cancelButtonClass: "btn btn-danger mr-4",
    buttonsStyling: false
});

function viewRecord(event) {
    const ref = $(event.target).data("href");
    window.location.href = ref;
}

function viewTransaction(event) {
    window.location =
        `/${$(event.target).data("table")}/Details?id=${$(event.target).data("row")}`;
}

function confirmFormSubmit(event) {
    event.preventDefault();
    swalSave.fire({
        title: "Are you sure?",
        text: "Are you sure want to save the record?",
        type: "info",
        showCancelButton: true,
        confirmButtonText: "Yes, save it!",
        cancelButtonText: "No, cancel!",
        reverseButtons: true
    }).then((result) => {
        if (result.value) {
            this.submit(true);
        }
        else {
            swalSave.fire(
                "Cancelled",
                "Item not saved!",
                "info"
            );
        }
    });
}

function confirmRemove(event) {
    swalDelete.fire({
        title: "Are you sure?",
        text: "The item will be removed from records!",
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, remove it!",
        cancelButtonText: "No, cancel!",
        reverseButtons: true
    }).then((result) => {
        if (result.value) {
            $.ajax({ url: $(event.target).attr("data-href"), method: "DELETE" })
                .done(function () {
                    swalDelete.fire(
                        "Removed!",
                        "Your file has been removed.",
                        "success"
                    ).then((value) => {
                        if (value)
                            window.location = $(event.target).attr("data-returnUrl");
                    });
                })
                .fail(function () {
                    swalDelete.fire(
                        "Failed",
                        "Could not remove the item :(",
                        "warning"
                    );
                });
        } else if (
            // Read more about handling dismissals
            result.dismiss === Swal.DismissReason.cancel
        ) {
            swalDelete.fire(
                "Cancelled",
                "Your file is safe :)",
                "error"
            );
        }
    });
}

function loadAjaxTabContent(event) {
    $.ajax({
        url: $(event.target).attr("data-partial-source"),
        type: "GET",
        dataType: "html",
        success: function (response) {
            const id = $(event.target).attr("aria-controls");
            const d = gei(id);
            d.innerHTML = response;
        }
    }).then(() =>
        $(".js-clickable-row")
            .click(viewRecord));
}

function enterToTab(e) {
    if (e.which === 13) {
        const nextInput = getNextActiveInput(this);
        if (nextInput) {
            if (nextInput.getAttribute("type") !== "submit"
                && e.target.getAttribute("type") !== "submit"
                && !$(e.target).is("textarea")
                && !$(nextInput).is("button")) {
                e.preventDefault();
                nextInput.focus();
            }
        }
        if (typeof lineItemNetPrice !== "undefined") {
            if (e.target === lineItemNetPrice)
                lineItemSelector.focus();
        }
    }
}

$(document).ready(function () {
    
    immutableSubmits.onsubmit = confirmFormSubmit;

    $(".datepicker").datepicker();
    $(".datepicker").datepicker("option", "dateFormat", "dd/mm/yy");

    clickableRows.onclick = viewRecord;
    clickableTransactions.onclick = viewTransaction;
    removableItems.onclick = confirmRemove;

    $('[data-toggle="popover"]').popover();

    $(".popover-dismiss").popover({
        trigger: "hover"
    });

    $('[data-toggle="tooltip"]').tooltip();

    ajaxTabs.onclick = loadAjaxTabContent;


    $(fromDate).datepicker()
        .on(
            "change",
            function () {
                to.datepicker(
                    "option",
                    "minDate",
                    getDate(this));
            });

    $(toDate).datepicker()
        .on(
            "change",
            function () {
                from.datepicker(
                    "option",
                    "maxDate",
                    getDate(this));
            });

    $(fromDate).datetimepicker({
        format: "DD/MM/YYYY",
        useCurrent: true
    });

    $(toDate).datetimepicker({
        format: "DD/MM/YYYY",
        useCurrent: false
    });

    $(fromDate).on(
        "change.datetimepicker",
        function (e) {
            $(toDate).datetimepicker("minDate", e.date);
        });

    $(toDate).on(
        "change.datetimepicker",
        function (e) {
            $(fromDate).datetimepicker("maxDate", e.date);
        });

    inputs.onkeypress = enterToTab;

    inputs.focus(function () {
        $(this).select();
    });

    $(blankButtons).click((e) => {
        e.stopPropagation();
    });
});

