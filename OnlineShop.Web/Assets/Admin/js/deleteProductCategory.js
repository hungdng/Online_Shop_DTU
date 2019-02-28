var brand = {
    init: function () {
        brand.registerEvents();
    },
    registerEvents: function () {
        $('.btnDelete').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var ok = confirm("Are u sure want to delete?");
            if (ok) {
                var id = btn.data('id');
                $.ajax({
                    url: "/Admin/ManageProductCategory/Delete",
                    data: { id: id },
                    dataType: "json",
                    type: "POST",
                    success: function (response) {
                        if (response.status) {
                            location.reload();
                            console.log("Delete successfully");
                        }
                    }
                });
            }
        });
    }
}
brand.init();