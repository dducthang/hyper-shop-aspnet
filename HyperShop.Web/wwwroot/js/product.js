$(document).ready(function () {
    $('#product-table').DataTable({
        "ajax": {
            "url": "/admin/product/getall",
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "price", "width": "15%" },
            { "data": "gender", "width": "15%" },]
    });
});