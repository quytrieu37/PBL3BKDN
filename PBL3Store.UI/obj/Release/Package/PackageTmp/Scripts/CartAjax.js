$(document).ready(function () {
    $('.frmAddToCart').submit(function (e) {
        e.preventDefault();
        let BookId = $(this).find('input[name=BookId]').val();
        let Quantity = $(this).find('input[name=Quantity]').val();
        const data = { BookId: BookId, Quantity: Quantity };
        console.log(data);
        $.ajax({
            url: '/Cart/AddToCart',
            type: 'POST',
            data: data,
            success: function (res) {
                console.log(res);
                if (res.state) {
                    $(document).trigger('addToCartEvent');
                }
                else {
                    console.log('Them that bai');
                }
            },
            error: function (res) {
                console.log('fail');
            }
        });
    });
    $(document).on('addToCartEvent', function () {
        $.ajax({
            url: '/Cart/CartSummary',
            type: "GET",
            success: function (res) {
                $('.cart-summary').html(res);
            },
            error: function (res) {
                console.log(res);
            }
        });
    });
})

