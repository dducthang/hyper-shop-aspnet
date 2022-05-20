let page = 1;
let taking = 3;
let categories = [];
let brands = [];
let colors = [];
let shoesHeights = [];
let closureTypes = [];
let genders = [];

let orderBy = "Price";


$(document).ready(function () {
    loadProducts()
});

$('.filter').on('click', function(e){
    e.preventDefault();
    e.stopPropagation();
    if (this.classList.contains("category-filter")) {
        categories = [];
        $('.category-form input:checked').each(function () {
            categories.push($(this).val());
        })
    }
    else if (this.classList.contains("brand-filter")) {
        brands = [];
        $('.brand-form input:checked').each(function () {
            brands.push($(this).val());
        })
    }
    else if (this.classList.contains("color-filter")) {
        colors = [];
        $('.color-form input:checked').each(function () {
            colors.push($(this).val());
        })
    }
    else if (this.classList.contains("shoes-height-filter")) {
        shoesHeights = [];
        $('.shoes-height-form input:checked').each(function () {
            shoesHeights.push($(this).val());
        })
    }
    else if (this.classList.contains("closure-type-filter")) {
        closureTypes = [];
        $('.closure-type-form input:checked').each(function () {
            closureTypes.push($(this).val());
        })
    }
    else if (this.classList.contains("gender-filter")) {
        genders = [];
        $('.gender-form input:checked').each(function () {
            genders.push($(this).val());
        })
    }

    loadProducts();
});


$('.clear').on('click', function (e) {
    e.preventDefault();
    e.stopPropagation();
    if (this.classList.contains("category-clear")) {
        categories = [];
        $('.category-form input:checked').each(function () {
            $(this).prop("checked", false);
        })
    }
    else if (this.classList.contains("brand-clear")) {
        brands = [];
        $('.brand-form input:checked').each(function () {
            $(this).prop("checked", false);
        })
    }
    else if (this.classList.contains("color-clear")) {
        colors = [];
        $('.color-form input:checked').each(function () {
            $(this).prop("checked", false);
        })
    }
    else if (this.classList.contains("shoes-height-clear")) {
        shoesHeights = [];
        $('.shoes-height-form input:checked').each(function () {
            $(this).prop("checked", false);
        })

    }
    else if (this.classList.contains("closure-type-clear")) {
        closureTypes = [];
        $('.closure-type-form input:checked').each(function () {
            $(this).prop("checked", false);
        })
    }
    else if (this.classList.contains("gender-clear")) {
        genders = [];
        $('.gender-form input:checked').each(function () {
            $(this).prop("checked", false);
        })
    }

    loadProducts();
});







//utility
function loadProducts(){
    $.ajax({
        url: "/customer/api/products",
        traditional: true,
        data: {
            page,
            taking,
            orderBy,
            categories,
            brands,
            colors,
            shoesHeights,
            closureTypes,
            genders, 

        },
        dataType: "json",
        success: function (res) {
            lastPage = Math.floor(res.productsCount / taking);

            $('.products').html(getProductList(res.productList));
            $('.pages').html(getPagesNumber(lastPage, page));
            $('.products-showing').html(getProductsShowing(res.productList.length, res.productsCount));

        },
        error: function (err) {
            console.log("failed")

        }
    })
}
function getProductList(products) {
    let res = '';
    for (product of products) { 
        res += getProduct(product)
    }
    return res;
}

function getProduct(product) {
    let res =  `
        <div class="col-lg-4 col-md-6">
                  <div class="product">
                    <div class="flip-container">
                      <div class="flipper">
                        <div class="front"><a href="detail.html"><img src="${product.primaryImage}" alt="" class="img-fluid"></a></div>
                        <div class="back">

                        </div>
                      </div>
                    </div><a href="detail.html" class="invisible"><img src="${product.primaryImage}" alt="" class="img-fluid"></a>
                     
                    <div class="text">
                      <h3><a href="detail.html">${product.name}</a></h3>
                      <p class = "p-3 m-1 bg-primary text-white">
                        ${product.colorCount} COLOR
                      </p>
                      <p class="price">
                        ${new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(product.price)}
                      </p>
                      <p class="buttons"><a href="detail.html" class="btn btn-outline-secondary">View detail</a><a href="basket.html" class="btn btn-primary"><i class="fa fa-shopping-cart"></i>Add to cart</a></p>
                    </div>
                    <!-- /.text-->

`

    if (datediff(Date.now(), Date.parse(product.publishedDate)) < 7) {
        res += `
        <div class="ribbon sale">
            <div class="theribbon">NEW</div>
            <div class="ribbon-background"></div>
        </div>`
    }
    res += `
        </div>
            <!-- /.product -->
        </div>`
    return res;
}

function getPagesNumber(lastPage, page) {
    let res = `
<nav
  aria-label="Page navigation example"
  class="d-flex justify-content-center"
>`;
    if (lastPage > 0) {
        res += `<ul class="pagination"><li class="page-item"><a href="#" class="page-link">First</a></li>`;
        let i = Number(page) > 5 ? Number(page) - 4 : 1;
        if (i !== 1) {
            res += `<li class="page-item"><a href="#" class="page-link">...</a></li>`;
        }
        for (; i <= Number(page) + 4 && i <= lastPage; i++) {
            if (i == page) {
                res += `<li class="page-item active"><a href="#" class="page-link">${i}</a></li>`;
            } else {
                res += `<li class="page-item"><a href="#" class="page-link">${i}</a></li>`;
            }
            if (i == Number(page) + 4 && i < lastPage) {
                res += `<li class="page-item"><a href="#" class="page-link">...</a></li>`;
            }
        }
        res += `<li class="page-item">
  <a href="#" class="page-link">Last</a>
</li>`;
        res += `</ul>`;
    }

    res += `</nav>`;
    return res;
}

function getProductsShowing(count, total) {
    return `
    Showing <strong>${count}</strong> of <strong>${total}</strong> products
`
}

function datediff(first, second) {
    return Math.round((second - first) / (1000 * 60 * 60 * 24));
}