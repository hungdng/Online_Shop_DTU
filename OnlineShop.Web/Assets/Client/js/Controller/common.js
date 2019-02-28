var common = {
    int: function () {

        common.registerEvent();
    },
    registerEvent: function () {
        $("#txtKeyword").autocomplete({
            minLength: 3,
            source: function (request, response) {
                $.ajax({
                    url: "/Product/GetListProductByName",
                    dataType: "json",
                    data: {
                        keyword: request.term
                    },
                    success: function (res) {
                        response(res.data);
                    }
                });
            },
            focus: function (event, ui) {
                $("#txtKeyword").val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                $("#txtKeyword").val(ui.item.label);
                return false;
            }
        }).autocomplete("instance")._renderItem = function (ul, item) {
            return $("<li>")
                .append("<a>" + item.label + "</a>")
                .appendTo(ul);
        };

        //$("#txtKeyword").autocomplete({
        //    minLength: 3,
        //    source: function (request, response) {
        //        $.ajax({
        //            url: "/Product/GetListObjProductByName",
        //            dataType: "json",
        //            data: {
        //                keyword: request.term
        //            },
        //            success: function (res) {
        //                response(res.data);
        //            }
        //        });
        //    },
        //    focus: function (event, ui) {
        //        $("#txtKeyword").val(ui.item.label);
        //        return false;
        //    },
        //    select: function (event, ui) {
        //        $("#txtKeyword").val(ui.item.ProductName);
        //        console.log(ui.item);
        //        return false;
        //    }
        //}).autocomplete("instance")._renderItem = function (ul, item) {
        //    return $("<li>")
        //        .append("<a>" + item.ProductName + "</a>")
        //        .appendTo(ul);
        //};
    }
}
common.int();