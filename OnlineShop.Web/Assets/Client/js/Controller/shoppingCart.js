var htmlOrderComplete = '';
var BASE_URL = "http://localhost:50337/";
var cart = {
    int: function () {
        cart.setClassInit();
        cart.loadData();
        cart.loadProductIncart();
        cart.registerEvent();        
    },
    registerEvent: function () {

        $('#frmPayment').validate({
            rules: {
                name: "required",
                address: "required",
                email: {
                    required: true,
                    email: true
                },
                phone: {
                    required: true,
                    number: true
                }
            },
            messages: {
                name: "Yêu cầu nhập tên",
                address: "Yêu cầu nhập địa chỉ",
                email: {
                    required: "Yêu cầu nhập email",
                    email: "Định dạng email chưa đúng"
                },
                phone: {
                    required: "Yêu cầu nhập số điện thoại",
                    number: "Số điện thoại phải là số."
                }
            }
        });

        $("div.product-by-manufacture > ul > li").off('click').on('click', function () {
            var cateID = $(this).parent().data('cateid');
            console.log(cateID);
            var id = $(this).find("a").data('id');
            cart.loadProductByCategory(id, cateID);

        });

        $('.btn-contact').off('click').on('click', function (e) {
            e.preventDefault();
            swal("Thông báo!", "The product is not in the merchant's contact for more information")
        });

        $('.btn-AddToCart').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = $(this).data('id');
            cart.addItem(productId);
        });

        $('.btnDeleteItem').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = $(this).data('id');

            swal({
                title: "Are you sure?",
                text: "Delete this product?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Delete",
                closeOnConfirm: false
            },
                function () {
                   
                    cart.deleteItem(productId);
                    swal("Delete Success!", "", "success");
                });
        });

       
        $('.txtQuantity').off('keyup').on('keyup', function (e) {         
            var quantity = parseInt($(this).val()) || 0;
            var productid = $(this).data('id');

            if (quantity < 0)
                quantity = 0;
            var price = parseFloat($(this).data('price'));
            if (isNaN(quantity) === false) {

                var amount = quantity * price;

                $('#amount_' + productid).text(amount.toLocaleString());
            }
            else {
                $('#amount_' + productid).text(0);
            }
            cart.setSubTotalAndTotalOrder();

            cart.updateAll();
        });

        $('.txtQuantity').off('keydown').on('keydown', function (e) {
            if (!((e.keyCode > 95 && e.keyCode < 106)
                || (e.keyCode > 47 && e.keyCode < 58)
                || e.keyCode === 8)) {
                return false;
            }
            
        });

        $('#btnContinue').off('click').on('click', function (e) {
            e.preventDefault();
            window.location.href = "/";
        });

        $('#btnDeleteAll').off('click').on('click', function (e) {
            e.preventDefault();
           
            swal({
                title: "Are you sure?",
                text: "Delete All Product?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Xóa",
                closeOnConfirm: false
            },
                function () {

                    cart.deleteAll();
                    swal("Delete Success!", "", "success");
                });
        });

        $('.cart-tab li a').off('click').on('click', function (e) {
            e.preventDefault();
        });

        $('#btnCheckout').off('click').on('click', function (e) {
            e.preventDefault();
            $('.cart-tab li#tab-checkout a').addClass("active");
            $('.cart-tab li#tab-checkout a').parent('li').prevAll('li').find('a').addClass("active");
            $('.cart-tab li#tab-checkout a').parent('li').nextAll('li').find('a').removeClass("active");
        });

        $('#checkUserLoginInfo').off('click').on('click', function () {
            if ($(this).prop('checked')) {
                cart.getLoginUser();
            }
            else {
                $('#txtFullName').val('');
                $('#txtEmail').val('');
                $('#txtPhoneNumber').val('');
                $('#txtAddess').val('');
            } 
        });

        $('#btnCreateOrder').off('click').on('click', function (e) {
            e.preventDefault();
            var isValid = $('#frmPayment').valid();
            if (isValid) {
                cart.createOrder();
                $('#btnCreateOrder').attr("href", "#order-complete");
                $('.cart-tab li#tab-complete a').addClass("active");
                $('.cart-tab li#tab-complete a').parent('li').prevAll('li').find('a').addClass("active");
                $('.cart-tab li#tab-complete a').parent('li').nextAll('li').find('a').removeClass("active");
            }
        });

        $('.btn-quickView').off('click').on('click', function (e) {
            e.preventDefault();
            $('#productModal').modal('show');
            var id = $(this).data('id');
            cart.loadProductDetail(id);

        });
    },
    createOrder: function () {
        var order = {
            CustomerName: $('#txtFullName').val(),
            CustomerAddress: $('#txtAddess').val(),
            CustomerEmail: $('#txtEmail').val(),
            CustomerMobile: $('#txtPhoneNumber').val(),
            Note: $('#txtNote').val(),
            PaymentMethod: "Thanh toán tiền mặt",
            OrderStatusID: "NOPROCESSING",
            Amount: parseInt($('#checkout-subtotal').data('id')),
            TotalCost: parseInt($('#checkout-total').data('id'))
        }
        $.ajax({
            url: '/ShoppingCart/CreateOrder',
            type: 'POST',
            dataType: 'json',
            data: {
                orderViewModel: JSON.stringify(order)
            },
            success: function (response) {
                if (response.status) {
                    cart.deleteAll();
                    var data = response.data;

                    var order_code = `
                    <li>
                        <h6>ORDER NO</h6>
                        <p>${data.BillCode}</p>
                    </li>
                    <li>
                         <h6>ORDER NO</h6>
                         <p>${data.BillCode}</p>
                    </li>
                    <li>
                         <h6>ORDER NO</h6>
                         <p>${data.BillCode}</p>
                    </li>
                    <li>
                         <h6>ORDER NO</h6>
                         <p>${data.BillCode}</p>
                    </li>
                `;
                    var info_complete = `
                    <li>
                        <span>Customer:</span>
                        ${data.CustomerName}
                    </li>
                    <li>
                        <span>ADDRESS:</span>
                        ${data.CustomerAddress}
                    </li>
                    <li>
                        <span>Email:</span>
                         ${data.CustomerEmail}
                    </li>
                    <li>
                        <span>PHONE : </span>
                         ${data.CustomerMobile}
                    </li>
                `;
                    $('#order-code').html(order_code);
                    $('#tableOrderComplete').html(htmlOrderComplete);
                    $('#info-order-complete').html(info_complete);
                }
            }
        });
    },
    getLoginUser: function () {
        $.ajax({
            url: '/ShoppingCart/GetUser',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var user = response.data;

                    $('#txtFullName').val(user.FullName);
                    $('#txtEmail').val(user.Email);
                    $('#txtPhoneNumber').val(user.PhoneNumber);
                    $('#txtAddess').val(user.Address);
                }
            }
        });
    },
    setSubTotalAndTotalOrder: function () {
        $('#lblTotalOrder').text(cart.getTotalOrder().toLocaleString());
        $('#lblSubTotalOrder').text(cart.getTotalOrder().toLocaleString());
    },
    getTotalOrder: function () {
        var listTextBox = $('.txtQuantity');
        var total = 0;
        $.each(listTextBox, function (i, item) {
            var qty = parseInt($(item).val());
            total += (parseInt($(item).val()) || 0) * parseFloat($(item).data('price'));
        });
        return total;
    },
    setClassInit: function () {
        $(".pro-tab-menu > ul > li:first-child").addClass("active");
        $("div.product-tab > div.tab-content > div:first-child").addClass("active");

        let itemUl = $('.pro-tab-menu > ul');
        $.each(itemUl, function (i, item) {

            var cateid = $(item).data('cateid');
            var id = $('div#' + cateid +' div.tab-content-product-manufacture:first-child').attr("id");

            cart.loadProductByCategory(id, cateid);
            console.log("so " + i + " :" + cateid);
        });
        
    },   
    addItem: function (productId) {
        $.ajax({
            url: '/ShoppingCart/Add',
            data: {
                productId: productId
            },
            type: 'POST',
            dataType: 'json',
            success: function (res) {
                if (res.status) {                    
                    swal("Add Product Success!", "", "success");
                    cart.loadProductIncart();
                }
            }
        });
    },
    deleteItem: function (productId) {
        $.ajax({
            url: '/ShoppingCart/DeleteItem',
            data: {
                productId: productId
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();
                    cart.loadProductIncart();
                }
            }
        });
    },
    deleteAll: function () {
        $.ajax({
            url: '/ShoppingCart/DeleteAll',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();
                }
            }
        });
    },
    updateAll: function () {
        var cartList = [];

        $.each($('.txtQuantity'), function (i, item) {
            cartList.push({
                ProductID: $(item).data('id'),
                Quantity: $(item).val()
            });
        });

        $.ajax({
            url: '/ShoppingCart/update',
            type: 'POST',
            data: {
                cartData: JSON.stringify(cartList)
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();
                    console.log('Update OK');
                }
            }
        });
    },
    loadData: function () {
        $.ajax({
            url: '/ShoppingCart/GetAll',
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status && res.data!==null) {
                    let html = '';
                    let htmlForDetailCheckOutProducts = '';
                    let htmlForDetailCheckOutTotal = '';
                    let data = res.data;
                    data.map((pro) => {
                        html += `
                            <!-- tr -->
                                <tr>
                                    <td class="product-thumbnail">
                                        <div class="pro-thumbnail-img">
                                            <img src="${pro.Product.ThumbnailImage}" alt="">
                                        </div>
                                        <div class="pro-thumbnail-info text-left">
                                            <h6 class="product-title-2">
                                                <a href="#">${pro.Product.ProductName}</a>
                                            </h6>
                                        </div>
                                    </td>
                                    <td class="product-price">${(pro.Product.PromotionPrice > 0 ? pro.Product.PromotionPrice : pro.Product.Price).toLocaleString()}</td>
                                    <td class="product-quantity">
                                       <input type="number" min="0" data-price="${pro.Product.PromotionPrice > 0 ? pro.Product.PromotionPrice : pro.Product.Price}" data-id ="${pro.ProductID}" value="${pro.Quantity}" name="qtybutton" class="cart-plus-minus-box txtQuantity ">                                       
                                    </td>
                                    <td id="amount_${pro.ProductID}" class="product-subtotal">${(pro.Quantity * (pro.Product.PromotionPrice > 0 ? pro.Product.PromotionPrice : pro.Product.Price)).toLocaleString()}</td>
                                    <td class="product-remove">
                                        <a href="" data-id="${pro.ProductID}" class="btnDeleteItem"><i class="zmdi zmdi-close"></i></a>
                                    </td>
                                </tr>
                            <!-- tr -->
                                `
                    });

                    $("#shoppingCart").html(html);

                    if (html === '') {
                        $("#shoppingCart").html('<tr><td colspan="5">No data found</td></tr>');                

                        $("#div-delete-all").empty();
                        $("#div-checkout").empty();
                    }
                    else {
                        $("#div-delete-all").empty();
                        $("#div-checkout").empty();
                        $("#div-delete-all").append('<button class="submit-btn-1 black-bg btn-hover-2" id="btnDeleteAll">Remove all</button>');
                        $("#div-checkout").append('<button class="submit-btn-1 black-bg btn-hover-2" id="btnCheckout" href="#checkout" data-toggle="tab">Pay</button>');
                    }
                    //---------------------------------------------------
                    data.map((prod) => {
                        htmlForDetailCheckOutProducts += `
                            <tr>
                                <td class="td-title-1">${prod.Product.ProductName} ${prod.Quantity > 1 ? ('x ' + prod.Quantity) : '' }</td>
                                <td class="td-title-2">${(prod.Product.PromotionPrice > 0 ? prod.Product.PromotionPrice : prod.Product.Price).toLocaleString()}</td>
                            </tr>
                    `
                    });

                    htmlForDetailCheckOutTotal = `
                    <tr>
                        <td class="td-title-1">Amount</td>
                        <td class="td-title-2" id="checkout-subtotal" data-id="${cart.getTotalOrder()}">${cart.getTotalOrder().toLocaleString()}</td>
                    </tr>
                    <tr>
                        <td class="td-title-1">Shipping and Handing</td>
                        <td class="td-title-2">0</td>
                    </tr>
                    <tr>
                        <td class="td-title-1">VAT</td>
                        <td class="td-title-2">0</td>
                    </tr>
                    <tr>
                        <td class ="order-total">Total Cost</td>
                        <td class="order-total-price" id="checkout-total" data-id="${cart.getTotalOrder()}">${cart.getTotalOrder().toLocaleString()}</td>
                    </tr>
                    `;

                    let htmlTableCheckOut = htmlForDetailCheckOutProducts + htmlForDetailCheckOutTotal;
                    $("#table-detail-checkout").html(htmlTableCheckOut);

                    htmlOrderComplete = '';

                    htmlOrderComplete = htmlTableCheckOut;
                    //----------------------------------------
                    cart.setSubTotalAndTotalOrder();
                    cart.registerEvent();
                }
            }
        })
    },
    loadProductByCategory: function (id, cateID) {
        $.ajax({
            url: '/Home/LoadProductManufacture',
            data: {
                id: id,
                cateID: cateID
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
                                            <a href="${pro.Alias + '.p-' + pro.ProductID + '.html'}">
                                                <img src="${pro.ThumbnailImage}" alt="${pro.ProductName}" />
                                            </a>
                                        </div>
                                        <div class="product-info">
                                            <h6 class="product-title">
                                                <a href="${pro.Alias + '.p-' + pro.ProductID + '.html'}">${pro.ProductName}</a>
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
                                : ` <h3 style="margin-bottom:5px;" class="pro-price">${pro.Price > 0 ? pro.Price.toLocaleString() : 'Liên hệ'}</h3> <h4 style= "margin-bottom: 10px;" class="pro-price">&nbsp;</h4>`}
                            
                                            
                                            <ul class="action-button">
                                                <li>
                                                    <a href="#" title="Wishlist"><i class="zmdi zmdi-favorite"></i></a>
                                                </li>
                                                <li>                                                    
                                                    <a href="" data-id="${pro.ProductID}" title="Quick View" class="btn-quickView"><i class="zmdi zmdi-zoom-in"></i></a>
                                                </li>
                                                <li>
                                                    <a href="#" title="Compare"><i class="zmdi zmdi-refresh"></i></a>
                                                </li>
                                                <li>
                                                    <a href="" class="${pro.Price > 0 ? 'btn-AddToCart' : 'btn-contact'}" data-id="${pro.ProductID}" title="Add To Cart"><i class="zmdi zmdi-shopping-cart-plus"></i></a>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                                <!-- product-item end -->
                            `;
                    });
                    $('div#' + cateID + '.tab-content > div.active > div.row').html(html);
                    cart.registerEvent();
                }
            }
        });
    },
    loadProductDetail: function (id) {
        $.ajax({
            url: '/Home/GetProductDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status === true) {
                    var data = res.data;
                    let url = BASE_URL + data.Alias + ".p-" + data.ProductID + ".html";
                    $('#thumbnail').attr('src', data.ThumbnailImage);
                    $('#product-name-modal').text(data.ProductName);
                    $('#btn-add-cart-in-modal').attr('data-id', data.ProductID);
                    //var urlx = "http://cameraquangduc.com/"; 
                    //$('#btn-share-facebook-modal').attr('data-href', urlx);
                    if (data.PromotionPrice > 0) {
                        $('#new-price').text(data.PromotionPrice.toLocaleString('en-EN'));
                        $('#old-price').text(data.Price.toLocaleString('en-EN'));
                    }
                    else {
                        $('#new-price').text(data.Price.toLocaleString('en-EN'));
                        $('#old-price').text('');
                    }
                    
                    
                }
                else {
                    alert(res.message);
                }
            },
            error: function (err) {
                console.log(err)
            }
        });

    },
    loadProductIncart: function () {
        $.ajax({
            url: '/ShoppingCart/GetAll',
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status && res.data !== null) {
                    let html = '';
                    let htmlForDetailCheckOutProducts = '';
                    let htmlForDetailCheckOutTotal = '';
                    let data = res.data;
                    let TotalQuantity = 0;
                    let ToalCost = 0;
                    data.map((pro) => {
                        html += `
                            <!-- single-cart -->
                            <div class="single-cart clearfix">
                                <div class="cart-img f-left">
                                    <a href="#">
                                        <img src="${pro.Product.ThumbnailImage}" alt="Cart Product" style="width:100px;" />
                                    </a>
                                    <div class="del-icon">
                                        <a href="" data-id="${pro.ProductID}" class="btnDeleteItem">
                                            <i class="zmdi zmdi-close"></i>
                                        </a>
                                    </div>
                                </div>
                                <div class="cart-info f-left">
                                    <h6 class="text-capitalize">
                                        <a href="${pro.Product.Alias}.p-${pro.Product.ProductID}.html">${pro.Product.ProductName}</a>
                                    </h6>
                                    <p>
                                        <span>Giá <strong>:</strong></span>${(pro.Product.PromotionPrice > 0 ? pro.Product.PromotionPrice : pro.Product.Price).toLocaleString()}
                                    </p> 
                                    <p>
                                        <span>SL <strong>:</strong></span>${pro.Quantity}
                                    </p> 
                                </div>
                            </div>  
                                `
                        TotalQuantity += pro.Quantity;
                        ToalCost += (pro.Quantity * (pro.Product.PromotionPrice > 0 ? pro.Product.PromotionPrice : pro.Product.Price))
                    });

                    $("#your-cart").html(html);                  
                    $('#count-product-in-cart').text(TotalQuantity < 10 ? "0" + TotalQuantity : TotalQuantity).toLocaleString('en-EN');
                    $('#total-cost-in-cart').text(ToalCost.toLocaleString('en-EN'));
                    //cart.setSubTotalAndTotalOrder();
                    //cart.registerEvent();
                }
            }
        })
    },
}
cart.int();