function initDataTable(name) {
    $('#' + name).dataTable({
        "bProcessing": true,
        "bServerSide": true,
        "sAjaxSource": "home/" + name
    });
}

$(document).ready(function () {
    initDataTable("demoOne");
    initDataTable("demoTwo");
    initDataTable("demoThree");
    initDataTable("demoFour");
});

