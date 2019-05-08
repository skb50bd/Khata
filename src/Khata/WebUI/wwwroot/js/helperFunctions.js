const sleep = (milliseconds) => {
    return new Promise(resolve => setTimeout(resolve, milliseconds));
};

function gei(id) {
    return document.getElementById(id);
}

function gecn(className) {
    return document.getElementsByClassName(className);
}

const formatter = new Intl.NumberFormat(
    "en-BD",
    {
        style: "currency",
        currency: "BDT",
        localeMatcher: "lookup",
        currencyDisplay: "symbol",
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });

function toFixedIfNecessary(value, dp) {
    return parseFloat(value).toFixed(dp);
}

function getNextActiveInput(inputElem) {
    const discountPercentage = gei("discount-percentage");

    var nextInput = inputs.get(inputs.index(inputElem) + 1);
    while (nextInput.disabled || nextInput === discountPercentage) {
        nextInput = inputs.get(inputs.index(nextInput) + 1);
        if (nextInput === discountPercentage)
            nextInput = inputs.get(inputs.index(nextInput) + 1);
    }
    return nextInput;
}