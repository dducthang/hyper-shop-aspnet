$(document).ready(function () {
	$('#product-table').DataTable({
		"ajax": {
			"url": "/admin/product/getall",
		},
		"columns": [
			{
				"data": "primaryImage", 
				"render": function (data) {
					return '<img class="img-fluid" src="'+ data + '" />';
				},
				"width": "15%"
			},
			{ "data": "name", "width": "15%" },
			{ "data": "price", "width": "15%" },
			{ "data": "gender", "width": "15%" },
			{
				"data": "id",
				"render": function (data) {
					return `
						<div class="w-75 btn-group" role="group">
						<a href="/Admin/Product/Edit?id=${data}"
						class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
						<a onClick=Delete('/Admin/Product/Delete/${data}')
						class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
					</div>
						`
				},
				"width": "15%"
			}
		]
	});
});

function Delete(url) {

}