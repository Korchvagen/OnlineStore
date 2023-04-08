import { successNotification, serverErrorNotification, errorNotification, errorInfo } from '../notifications/notifications.js';

$(document).ready(() => {
    $('body').on('click', '#getPaymentPartialViewBtn', getPaymentPartialView);

    $('body').on('click', '#confirmPaymentBtn', confirmPayment);
});

function getPaymentPartialView() {
    const modal = $('#modal');

    $('#modalTitle').text('Payment proof');

    $.ajax({
        url: '/Carts/GetPaymentPartialView',
        type: 'GET',
        success: function (response) {
            if (response.success === false) {
                serverErrorNotification(response);
            } else {
                modal.find('.modal-body').html(response);
                modal.modal('show');

                timer();
            }
        },
        error: function (jqXHR) {
            errorNotification(jqXHR);
        }
    });
}

function confirmPayment() {
    event.preventDefault();

    const modal = $('#modal');

    if ($('#confirmationCode').val() === $('#code').val()) {
        $.ajax({
            url: '/Carts/DeleteOrders',
            type: 'GET',
            success: function (response) {
                if (response.success) {
                    modal.modal('hide');

                    getCartsListPartialView()

                    successNotification('Payment was successful');
                } else {
                    serverErrorNotification(response);
                }
            },
            error: function (jqXHR) {
                errorNotification(jqXHR);
            }
        });
    } else {
        errorInfo('Wrong code');
    }
}

function timer() {
    const modal = $('#modal');
    let time = 120;

    let t = setInterval(() => {
        if (!$('#modal').hasClass('show')) clearInterval(t);

        time--;

        if (time < 0) {
            modal.modal('hide');

            clearInterval(t);
        } else if (time < 120 && time >= 70) $('#timer').text(`1:${time - 60}`);

        else if (time < 70 && time >= 60) $('#timer').text(`1:0${time - 60}`);

        else if (time < 60 && time >= 10) $('#timer').text(`0:${time}`);

        else $('#timer').text(`0:0${time}`);
        console.log('zzzz');
    }, 1000);
}

function getCartsListPartialView() {
    $.ajax({
        url: '/Orders/GetOrdersPartialView',
        type: 'GET',
        success: function (response) {
            if (response.success === false) {
                serverErrorNotification(response);
            } else {
                $('#cartContainer').html(response);
            }
        },
        error: function (jqXHR) {
            errorNotification(jqXHR);
        }
    });
}