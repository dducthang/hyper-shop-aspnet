$('#cart-form').on('submit', function (e) {
    e.preventDefault()
    e.stopPropagation()
    if ($('input[name=sizeId]:checked').val() == undefined) {

        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'You need provide Size!'
        })
        return
    }

    const form = $(this);

    const url = form.attr('action');
    const method = form.attr('method');
    const data = form.serialize();

    
    $.ajax({
        url,
        method,
        data,
        success: function (data) {
            toastr.success('Add to cart successfully')
        },
        error: function (err) {
            toastr.error("Add to cart failed")
        }
    })
})