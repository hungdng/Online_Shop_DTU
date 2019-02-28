var user = {
    init: function () {
        user.registerEvents();
    },
    registerEvents: function () {
        $('.btnDelete').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var ok = confirm("Delete successfully");
            if (ok) {
                var id = btn.data('id');
                $.ajax({
                    url: "/Admin/ManageUsers/Delete",
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
user.init();