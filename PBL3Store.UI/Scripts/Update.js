$(document).ready(function () {
    $('.frmUpdateToCart').submit(function (e) {
        e.preventDefault();
        let BookId = $(this).find('input[name=BookId]').val();
        let quantity = $(this).find('input[name=Quantity]').val();
        const DATA = { BookId: BookId, Quantity: quantity };
        console.log(data);
        $.ajax({
            url: '/Cart/UpdateToCart',
            type: 'POST',
            data: DATA,
            success: function (res) {
                console.log(res);
                if (res.state) {
                    $(document).trigger('updateToCartEvent');
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
    $(document).on('updateToCartEvent', function () {
        $.ajax({
            url: '/Cart/CartUpdateSummary',
            type: "GET",
            success: function (res) {
                $('.cart-update-summary').html(res);
            },
            error: function (res) {
                console.log(res);
            }
        });
    });
})