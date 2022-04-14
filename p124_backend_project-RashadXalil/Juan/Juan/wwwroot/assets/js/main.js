$(document).ready(function () {
    $(document).on("click", ".quick-view", function (e) {
        e.preventDefault();

        let url = $(this).attr("href");

        fetch(url)
            .then(response => response.text())
            .then(data => {

                console.log(data)
                $("#quick_view .modal-dialog").html(data)

                $("#quick_view").modal(true)

            });

    })
    $(document).on("click", ".add-to-basket", function (e) {
        e.preventDefault();
        let url = $(this).attr("href");
        console.log(url)
        fetch(url)
            .then(function (response) {
                return response.text();
            }).then(data => {
                $(".minicart-content-box").html(data)
                var notifCounter = $(".notification-counter").val();
                $(".notification").html(notifCounter);
            });

    })
    $(document).on("click", ".remove-basket", function (e) {
        e.preventDefault();

        let path = $(this).attr("href");

        fetch(path)
            .then(response => {
                if (response.ok) {
                    toastr["success"]("Removed !")
                }
                else {
                    toastr["error"]("Can't Removed !")
                }
                return response.text();
            }).then(data => {
                $(".minicart-content-box").html(data)
                let prodcount = $(".notification-counter").val();
                $(".notification").html(prodcount)
            })
        

    })
})