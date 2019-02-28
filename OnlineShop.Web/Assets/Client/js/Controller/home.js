var homeController = {
    init: function () {
        homeController.setClassInit();
        homeController.registerEvent();
    },
    registerEvent: function () {
        $("div.pro-tab-menu > ul > li").off('click').on('click', function () {
            var id = $(this).find("a").data('id');
            homeController.loadProductByCategory(id);
            
        });
    },
    setClassInit: function () {
        $(".pro-tab-menu > ul > li:first-child").addClass("active");
        $("div.product-tab > div.tab-content > div:first-child").addClass("active");

        var id = $("div.tab-pane:first-child").attr("id");
        if (id != undefined)
            homeController.loadProductByCategory(id);
    },
    loadProductByCategory: function (id) {
        $.ajax({
            url: '/Home/LoadProductManufacture',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status === true) {
                    var data = res.data;
                    var html = '';
                    data.map((pro) => {
                        html +=
                            `
                               <!-- product-item start -->
                                <div class="col-md-3 col-sm-4 col-xs-12">
                                    <div class="product-item">
                                        <div class="product-img">
                                            <a href="single-product.html">
                                                <img src="${pro.ThumbnailImage}" alt="" />
                                            </a>
                                        </div>
                                        <div class="product-info">
                                            <h6 class="product-title">
                                                <a href="single-product.html">${pro.ProductName}</a>
                                            </h6>
                                            <div class="pro-rating">
                                                <a href="#"><i class="zmdi zmdi-star"></i></a>
                                                <a href="#"><i class="zmdi zmdi-star"></i></a>
                                                <a href="#"><i class="zmdi zmdi-star"></i></a>
                                                <a href="#"><i class="zmdi zmdi-star-half"></i></a>
                                                <a href="#"><i class="zmdi zmdi-star-outline"></i></a>
                                            </div>
                                            ${pro.PromotionPrice > 0 ?
                                `<h3 style="margin-bottom:5px;" class="pro-price">${pro.PromotionPrice.toLocaleString()}</h3>
                                                    <h4 style=" text-decoration: line-through; margin-bottom: 10px;" class="pro-price">${pro.Price.toLocaleString()}</h4>`
                            : ` <h3 style="margin-bottom:5px;" class="pro-price">${pro.Price > 0 ? pro.Price.toLocaleString() : 'Contact'}</h3> <h4 style= "margin-bottom: 10px;" class="pro-price">&nbsp;</h4>`}
                            
                                            
                                            <ul class="action-button">
                                                <li>
                                                    <a href="#" title="Wishlist"><i class="zmdi zmdi-favorite"></i></a>
                                                </li>
                                                <li>
                                                    <a href="#" data-toggle="modal" data-target="#productModal" title="Quickview"><i class="zmdi zmdi-zoom-in"></i></a>
                                                </li>
                                                <li>
                                                    <a href="#" title="Compare"><i class="zmdi zmdi-refresh"></i></a>
                                                </li>
                                                <li>
                                                    <a href="" class="${pro.Price > 0 ? 'btn-AddToCart' : 'btn-contact'}" data-id="${pro.ProductID}" title="Add to cart"><i class="zmdi zmdi-shopping-cart-plus"></i></a>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                                <!-- product-item end -->
                            `;                        
                    });
                    $('div.tab-content > div.active > div.row').html(html);
                }
            }
        });
    }
}

homeController.init();