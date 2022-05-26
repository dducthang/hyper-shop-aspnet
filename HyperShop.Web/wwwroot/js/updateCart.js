$('.cart-item-delete').on('click', function (e) {
    e.preventDefault()
    e.stopPropagation()
    const element = $(this)
    const cartId = element.attr('id')
    const url = element.attr('href')

    
    $.ajax({
        url,
        data: {
            cartId
        },
        success: function (data) {

            //update total
            const priceSub = Number($('#'+cartId).parent().siblings(".item-total").text().replace(/[^0-9.-]+/g, ""))
            let total = Number($('.cart-total').text().replace(/[^0-9.-]+/g, ""))
            total = total - priceSub
            total = new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(total)
            $('.cart-total').text(total)

            //update item count
            let itemCount = $('.item-count').text()
            itemCount--
            $('.item-count').text(itemCount)
            $('.item-count-unit').text(itemCount > 1 ? "items" : "item");

            element.closest("tr").remove();
            toastr.success('Delete item successfully')
        },
        error: function (err) {
            toastr.error("Delete item failed")
        }
    })
})

$('.update-cart-btn').on('click', function (e) {
    e.preventDefault
    e.stopPropagation
    form = $('.cart-form')
    data = form.serialize()
    url = $(this).attr('formaction')
    $.ajax({
        url,
        data,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('.cart-table-body').html(getCartTableBody(data))
            $('.cart-total').text(getTotal(data))
            
        },
        error: function (err) {
            console.log(err)
        }
    })
})

function getCartTableBody(items) {
    let res = ``;
    for (item of items) {
        res += `
        <tr>
            <td><a href="#"><img src="${item.productImage}" alt="${item.productName}"></a></td>
            <td><a href="#">${item.productName}</a></td>
            <td>${item.size}</td>
            <td>
            <input type="number" min="0" max="${item.stockQuantity}" name="${item.cartId}" value="${item.quantity}" class="form-control qty-input">
            </td>
            <td>${new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(item.price)}</td>
            <td>$0.00</td>
            <td class = "item-total">${new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format((item.price * item.quantity))}</td>
            <td><a href="/Customer/Cart/DeleteItem" id="${item.CartId}" class="cart-item-delete"><i class="fa fa-trash-o"></i></a></td>
        </tr>
`
    }
    return res;
}

function getTotal(items) {
    let total = 0;
    for (item of items) {
        total += item.price * item.quantity;
    }
    return new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format((total));
}