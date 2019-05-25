const immutableSubmits = gecn("immutable-submit");
const fromDate = gei("from-date");
const toDate = gei("to-date");
const fromText = gei("from-text");
const toText = gei("to-text");
var blankButtons = gecn("blank-link-button");
const inputs = $(":input");
const ajaxTabs = gecn("ajax-tab");
var clickableRows = gecn("js-clickable-row");
const clickableTransactions = gecn("js-clickable-transaction");
const removableItems = gecn("js-remove-item");
const sendReportButton = gei("send-report-button");


function attachLinksToTds() {
    for (let i = 0; i < clickableRows.length; i++) {
        const row = clickableRows[i];
        for (let j = 0; j < row.children.length; j++) {
            const column = row.children[j];
            $(column).data("href", $(row).data("href"));
        }
    }
}

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
    cancelButtonClass: "btn btn-secondary mr-2",
    buttonsStyling: false
});

const swalQuestion = Swal.mixin({
    confirmButtonClass: "btn btn-primary",
    cancelButtonClass: "btn btn-secondary mr-2",
    buttonsStyling: false
});

const swalSave = Swal.mixin({
    confirmButtonClass: "btn btn-success",
    cancelButtonClass: "btn btn-danger mr-4",
    buttonsStyling: false
});

function viewRecord(event) {
    var target = $(event.target);
    if (target.tagName === "td")
        target = target.closest("tr");

    const ref = target.data("href");
    window.location.href = ref;
}

function viewTransaction(event) {
    const tableName = $(event.target).data("table");
    const areaName = findArea(tableName);
    const rowId = $(event.target).data("row");
    window.location = `/${areaName}/${tableName}/Details?id=${rowId}`;
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
            $.ajax(
                {
                    url: $(event.target).attr("data-href"),
                    method: "DELETE"
                }
            ).done(function () {
                swalDelete.fire(
                    "Removed!",
                    "Your file has been removed.",
                    "success"
                ).then((value) => {
                    if (value)
                        window.location = $(event.target).attr("data-returnUrl");
                });
            }).fail(function () {
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

function sendEmailReport(event) {
    swalQuestion.fire({
        title: "Send the reports now?",
        text: "The reports will be sent to the specified email addresses.",
        type: "question",
        showCancelButton: true,
        confirmButtonText: "Yes, send now!",
        cancelButtonText: "No, cancel!",
        reverseButtons: true
    }).then((result) => {
        if (result.value) {
            $.ajax({ url: sendReportButton.getAttribute("data-href"), method: "POST" })
                .done(function () {
                    swalQuestion.fire(
                        "Sent!",
                        "Reports have been sent.",
                        "success"
                    );
                })
                .fail(function () {
                    swalQuestion.fire(
                        "Failed",
                        "Could not send the reports.",
                        "warning"
                    );
                });
        } else if (
            // Read more about handling dismissals
            result.dismiss === Swal.DismissReason.cancel
        ) {
            swalQuestion.fire(
                "Cancelled",
                "Nothing sent :)",
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
    }).then(() => {
        blankButtons = gecn("blank-link-button");
        clickableRows = gecn("js-clickable-row");
        $(clickableRows).click(viewRecord);
        attachLinksToTds();
        $(clickableRows).click(viewRecord);
        $(blankButtons).click((e) => {
            e.stopPropagation();
        });
    });
}

function enterToTab(e) {
    if (e.which === 13) {
        const nextInput = getNextActiveInput(this);
        if (nextInput) {
            if (nextInput.getAttribute("type") !== "submit"
                && e.target.getAttribute("type") !== "submit"
                && !($(e.target).is("textarea") && $(e.target).val() !== "")
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
    attachLinksToTds();
    $(immutableSubmits).submit(confirmFormSubmit);

    $(clickableRows).click(viewRecord);
    $(clickableTransactions).click(viewTransaction);
    $(removableItems).click(confirmRemove);

    if (sendReportButton)
        sendReportButton.onclick = sendEmailReport;

    $('[data-toggle="popover"]').popover();

    $(".popover-dismiss").popover({
        trigger: "hover"
    });

    $('[data-toggle="tooltip"]').tooltip();

    $(ajaxTabs).click(loadAjaxTabContent);


    $(".daterange").daterangepicker({
        opens: 'center',
        autoApply: true,
        locale: {
            format: "DD/MM/YYYY",
            separator: " - ",
            applyLabel: "Apply",
            cancelLabel: "Cancel",
            fromLabel: "From",
            toLabel: "To",
            customRangeLabel: "Custom",
            weekLabel: "W",
            daysOfWeek: [
                "Su",
                "Mo",
                "Tu",
                "We",
                "Th",
                "Fr",
                "Sa"
            ],
            monthNames: [
                "January",
                "February",
                "March",
                "April",
                "May",
                "June",
                "July",
                "August",
                "September",
                "October",
                "November",
                "December"
            ],
            firstDay: 6
        }
    }, function (start, end, label) {
        fromText.value = start.format("DD/MM/YYYY");
        toText.value = end.format("DD/MM/YYYY");
    });

    $(".dr-datepicker")
        .daterangepicker({
            singleDatePicker: true,
            showDropdowns: true,
            locale: {
                format: "DD/MM/YYYY"
            },
            minYear: 2010,
            maxYear: parseInt(moment().format('YYYY'), 10) + 1
        }, function (start, end, label) {

        });

    $(".dr-show").click(() => $(".dr-datepicker").show());


    $(inputs).keypress(enterToTab);

    $(inputs).focus(function () {
        $(this).select();
    });

    $(blankButtons).click((e) => {
        e.stopPropagation();
    });


    // Collapsible 
    // Add minus icon for collapse element which is open by default

    $(".collapse.show").each(function () {
        $(this).prev(".card-header").find(".fa").addClass("fa-minus").removeClass("fa-plus");
    });

    // Toggle plus minus icon on show hide of collapse element
    $(".collapse")
        .on('show.bs.collapse',
            function () {
                $(this).prev(".card-header")
                    .find(".fa")
                    .removeClass("fa-plus")
                    .addClass("fa-minus");
            })
        .on('hide.bs.collapse',
            function () {
                $(this).prev(".card-header")
                    .find(".fa")
                    .removeClass("fa-minus")
                    .addClass("fa-plus");
            });
});

