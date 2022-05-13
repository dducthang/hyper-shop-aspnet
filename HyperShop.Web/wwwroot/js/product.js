
$(document).ready(function () {
	dataTable = $('#product-table').DataTable({
		"ajax": {
			"url": "/admin/product/getall",
		},
		"columns": [
			{
				"data": "primaryImage", 
				"render": function (data) {
					return '<img class="img-fluid" src="' + data + '" style="width:50px;;">';
				},
				"width": "1%"
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
						class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i>Edit</a>
						<a href="/Admin/Stock?productId=${data}"
						class="btn btn-secondary mx-2"> <i class="bi bi-pencil-square"></i>Stock</a>

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
	Swal.fire({
		title: 'Are you sure?',
		text: "You won't be able to revert this!",
		icon: 'warning',
		showCancelButton: true,
		confirmButtonColor: '#3085d6',
		cancelButtonColor: '#d33',
		confirmButtonText: 'Yes, delete it!'
	}).then((result) => {
		if (result.isConfirmed) {
			$.ajax({
				url,
				type: 'DELETE',
				success: function (data) {
					if (data.success) {
						dataTable.ajax.reload();
						Swal.fire(
							'Success!',
							data.message,
						)
					}
					else {
						Swal.fire(
							'Error!',
							data.message,x
						)
					}
				}
			})

		}
	})
}