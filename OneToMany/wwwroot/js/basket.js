$(function () {

    $(document).on("click", ".add-product-basket", function () {

        let id = parseInt($(this).attr("data-id"));

        $.ajax({
            url: `home/addproducttobasket?id=${id}`,
            type: 'post',
            success: function (response) {

                $(".rounded-circle").text(response.count);
                $(".basket-total-price").text(`CART($${response.total})`);
            }
        })
    })


    //remove basket
    $(document).on("click", ".delete-basket-data", function () {

        let id = parseInt($(this).attr("data-id"));

        $.ajax({
            url: `cart/DeleteProductFromBasket?id=${id}`,
            type: "Post",
            success: function (response) {

                $(".rounded-circle").text(response.count);
                $(".basket-total-price").text(`CART($${response.total})`);
                $(".cart-total").text(`Total : ${response.total}`);
                if (response.total == 0) {
                    $(".cart-body").html(`<div class="alert alert-warning" role="alert">
                                       Cart page is empty </div>`)
                }
                else {
                    $(`[data-id = ${id}]`).parent().parent().remove();
                }
            },
        });
    })
})