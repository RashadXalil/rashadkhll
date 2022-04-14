$(document).ready(function () {
    $(document).on("click", ".delete-product", function (e) {
        e.preventDefault();

        console.log("clicked")
        let path = $(this).attr("href")

        Swal.fire({
            title: 'Are You Sure?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Delete!'
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(path).then(Response => {
                    if (Response.ok) {
                        Swal.fire(
                            'Deleted!',
                            'Selected File Deleted Successfully',
                            'success'
                        ).then(function () {
                            location.reload();
                        })
                    }
                    else {
                        Swal.fire(
                            'Failed!',
                            'Selected File Deleted Successfully',
                            'success'
                        )
                        location.reload();
                    }
                })
            }
        })
    })

});