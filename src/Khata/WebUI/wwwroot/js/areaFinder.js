function findArea(section) {
    let cash = ["Cash"];
    let dashboard = ["Dashboard"];
    let inventory = [
        "Products",
        "Services"
    ];
    let incoming = [
        "Sales",
        "Debt Payments",
        "Purchase Returns",
        "Deposits"
    ];
    let outgoing = [
        "Expenses",
        "Purchases",
        "Supplier Payments",
        "Salary Payments",
        "Salary Issues",
        "Refunds",
        "Withdrawals"
    ];
    let outlets = ["Outlets"];
    let people = [
        "Customers",
        "Suppliers",
        "Employees"
    ];
    let reporting = ["Reporting"];

    if (cash.includes(section)) return "Cash";
    if (dashboard.includes(section)) return "Dashboard";
    if (inventory.includes(section)) return "Inventory";
    if (incoming.includes(section)) return "Incoming";
    if (outgoing.includes(section)) return "Outgoing";
    if (outlets.includes(section)) return "Outlets";
    if (people.includes(section)) return "People";
    if (reporting.includes(section)) return "Reporting";
}