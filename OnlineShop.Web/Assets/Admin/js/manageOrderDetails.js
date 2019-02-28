var homeController = {
    init: function () {
        homeController.registerEvent();
    },
    registerEvent: function () {
        $('#frmSaveData').validate({
            rules: {
                txtSaleNumber: {
                    required: true,
                    number: true,
                    min: 1
                },
                txtSalePrice: {
                    required: true,
                    number: true,
                    min: 1
                },

            },
            messages: {
                txtSaleNumber: {
                    required: "Sale Number is REQUIRED",
                    number: "Sale Number has to be NUMBER",
                    min: "Sale Number has to be OVER 1"
                },
                txtSalePrice: {
                    required: "Sale Price is REQUIRED",
                    number: "Sale Price has to be NUMBER",
                    min: "Sale Price has to be OVER 1"
                },

            }
        });

        $('#frmSaveDataCreate').validate({
            rules: {
                txtSaleNumber2: {
                    required: true,
                    number: true,
                    min: 1
                },
                txtSalePrice2: {
                    required: true,
                    number: true,
                    min: 1
                },
                txtProductID: {
                    required: true,
                },
            },
            messages: {
                txtSaleNumber2: {
                    required: "Sale Number is REQUIRED",
                    number: "Sale Number has to be NUMBER",
                    min: "Sale Number has to be OVER 1"
                },
                txtSalePrice2: {
                    required: "Sale Price is REQUIRED",
                    number: "Sale Price has to be NUMBER",
                    min: "Sale Price has to be OVER 1"
                },
                txtProductID: {
                    required: "Please choose at least ONE product"
                }
            }
        });

        $('.btnAddNew').off('click').on('click', function () {
            $('#modalAdd').show();
            homeController.resetForm();
        });

        $('#btnSave').off('click').on('click', function () {
            if ($('#frmSaveData').valid()) {
                var id = $(this).data('id');
                homeController.saveData();
            }
        });

        $('#btnSaveCreate').off('click').on('click', function () {
            if ($('#frmSaveDataCreate').valid()) {
                var id = $(this).data('id');
                homeController.createData();
            }

        });

        $('.btn-edit').off('click').on('click', function () {
            var id = $(this).data('id');
            $('#modalAddUpdate').show();
            homeController.loadDetail(id);
        });

        $('.btn-delate').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Are you sure Delete ?", function (result) {
                if (result) {
                    homeController.deleteDetail(id);
                }
            });
        });
        $('#txtProductID').autocomplete({
            minLength: 2,
            source: function (request, response) {
                $.ajax({
                    url: '/Admin/ManageOrderDetails/GetProductJson',
                    dataType: "json",
                    data: {
                        productName: request.term
                    },
                    success: function (res) {
                        response(res.data)
                    }
                })
            },
            focus: function (event, ui) {
                $("#txtProductID").val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                $("#txtProductID").val(ui.item.ProductName);
                $("#txtSalePrice2").val(ui.item.PromotionPrice > 0 ? ui.item.PromotionPrice : ui.item.Price);
                $("#idProduct").val(ui.item.ProductID);
                return false;
            }
        }).autocomplete("instance")._renderItem = function (ul, item) {
            return $("<li>")
                .append("<a>" + item.ProductName + "</a>")
                .appendTo(ul);
        };
    },

    loadData: function (id) {
        $.ajax({
            url: '/Admin/ManageOrderDetails/LoadData',
            type: 'GET',
            data: {
                id: id,
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var html = '';
                    var data = response.data.Items;
                    data.map((pro) => {
                        html +=
                            `
                                 <tr>
                                    <td>${pro.Product.ProductName}</td>
                                    <td>${pro.SaleNumber}</td>
                                    <td>${pro.SalePrice.toLocaleString('en-EN')} </td>
                                    <td>${pro.TotalCost.toLocaleString('en-EN')}</td>
                                    <td>
                                        <button data-toggle="modal" data-target="#modalAddUpdate" class ="btn btn-sm btn-primary btn-edit" data-id=${pro.OrderDetailID}><i class ="fa fa-edit"></i></button>
                                        <button class ="btn btn-sm btn-danger btn-delate" data-id=${pro.OrderDetailID}><i class ="fa fa-trash-o"></i></button>
                                    </td>
                                 </tr>
                            `
                    });
                    $('#order-detail').html(html);
                    homeController.registerEvent();
                }
            }
        })
    },

    loadDetail: function (id) {
        $.ajax({
            url: '/Admin/ManageOrderDetails/GetDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status == true) {
                    var data = res.data;
                    $('#hidID').val(data.OrderDetailID);
                    $('#txtSaleNumber').val(data.SaleNumber);
                    $('#txtSalePrice').val(data.SalePrice);
                    $('#txtProductSpec').val(data.ProductSpec)
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

    createData: function (id) {
        var productID = $('#idProduct').val();
        var saleNumber = parseInt($('#txtSaleNumber2').val());
        var salePrice = parseFloat($('#txtSalePrice2').val());
        //var productSpec = $('#txtProductSpec2').val();
        var oid = $('#oID').val();
        // var hid = $('#hidID2').val();
        var orderDetail = {
            ProductID: productID,
            SaleNumber: saleNumber,
            SalePrice: salePrice,
            OrderID: oid,
            //ProductSpec: productSpec,
            //OrderDetailID: hid,
        };

        $.ajax({
            url: '/Admin/ManageOrderDetails/CreateData',
            data: {
                strEmployee: JSON.stringify(orderDetail)
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;
                    bootbox.alert("Create Success", function () {
                        $('#modalAdd').modal('hide');
                        $("#totalCost-Order").val(data.TotalCost);
                        $("#amount-Order").val(data.Amount);
                        homeController.loadData(oid);
                    });
                }
                else {
                    bootbox.alert(response.message);
                }
            },
            error: function (err) {
                console.log(err);
            }

        });
    },

    saveData: function () {
        var saleNumber = parseInt($('#txtSaleNumber').val());
        var salePrice = parseFloat($('#txtSalePrice').val());
        var productSpec = $('#txtProductSpec').val();
        var id = $('#hidID').val();
        var orderId = $("#Orders_ID").val();
        var orderDetail = {
            SaleNumber: saleNumber,
            SalePrice: salePrice,
            ProductSpec: productSpec,
            OrderDetailID: id
        };

        $.ajax({
            url: '/Admin/ManageOrderDetails/SaveData',
            data: {
                strEmployee: JSON.stringify(orderDetail)
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;
                    bootbox.alert("Save Success", function () {
                        $('#modalAddUpdate').modal('hide');
                        $("#totalCost-Order").val(data.TotalCost);
                        $("#amount-Order").val(data.Amount);
                        homeController.loadData(orderId);
                    });
                }
                else {
                    bootbox.alert(response.message);
                }
            },
            error: function (err) {
                console.log(err);
            }

        });
    },

    deleteDetail: function (id) {
        var orderId = $("#Orders_ID").val();
        $.ajax({
            //url: '/Home/Delete',
            url: '/Admin/ManageOrderDetails/Delete',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;
                    bootbox.alert("Delete Success", function () {
                        $('#modalAddUpdate').modal('hide');
                        $("#totalCost-Order").val(data.TotalCost);
                        $("#amount-Order").val(data.Amount);
                        homeController.loadData(orderId);
                    });
                }
                else {
                    bootbox.alert(response.message);
                }
            },
            error: function (err) {
                console.log(err);
            }

        });
    },

    resetForm: function () {
        $('#hidID').val('');
        $('#txtProductID').val('');
        $('#txtSaleNumber2').val('');
        $('#txtSalePrice2').val('');
    },
}

homeController.init();