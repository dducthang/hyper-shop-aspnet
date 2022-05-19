$(document).ready(function () {
    loadProducts(1)
});

function loadProducts(page, categories, brands, colors, shoesHeights, closureType, genders){
    $.ajax({
        url: "/customer/api/products",
        traditional: true,
        data: {
            page,
            categories,
            brands,
            colors,
            shoesHeights,
            closureType,
            genders, 

        },
        dataType: "json",
        success: function (res) {
            $('.products').html(getProductList(res));
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
    return `
        <div class="col-lg-4 col-md-6">
                  <div class="product">
                    <div class="flip-container">
                      <div class="flipper">
                        <div class="front"><a href="detail.html"><img src="${product.primaryImage}" alt="" class="img-fluid"></a></div>
                        <div class="back">

                        </div>
                      </div>
                    </div><a href="detail.html" class="invisible"><img src="${product.primaryImage}" alt="" class="img-fluid"></a>
                     <table>
                               <tr>
                                   <td><a href="#1"><img src="${product.primaryImage}" alt="" class="img-fluid" style="width:50px;"></a> </td>
                                   <td><a href="#2"><img src="${product.primaryImage}" alt="" class="img-fluid" style="width:50px;"></a> </td>
                                   <td><a href="#3"><img src="${product.primaryImage}" alt="" class="img-fluid" style="width:50px;"></a> </td>


                               </tr>
                           </table>
                    <div class="text">
                      <h3><a href="detail.html">${product.name}</a></h3>
                      <p class="price">
                        ${new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(product.price)}
                      </p>
                      <p class="buttons"><a href="detail.html" class="btn btn-outline-secondary">View detail</a><a href="basket.html" class="btn btn-primary"><i class="fa fa-shopping-cart"></i>Add to cart</a></p>
                    </div>
                    <!-- /.text-->
 </div>
                  <!-- /.product            -->
                </div>
`

                    //if((DateTime.Now-item.PublishedDate).TotalDays < 7){
                    //        <div class="ribbon new">
                    //            <div class="theribbon">NEW</div>
                    //            <div class="ribbon-background"></div>
                    //        </div>
                    //    }

                 
}