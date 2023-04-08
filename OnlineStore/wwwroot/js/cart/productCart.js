import { successNotification, serverErrorNotification, errorNotification, errorInfo } from '../notifications/notifications.js';

$(document).ready(() => {
    $('body').on('click', '.addProductToCartBtn', (e) => {
        addProductToCart(e.target.value);
    })

    $('body').on('click', '.deleteProductFromCartBtn', (e) => {
        deleteProductFromCart(e.target.value);
    })

    $('body').on('click', '.decreaseAmountBtn', (e) => {
        decreaseAmount(e.target.value);
    })

    $('body').on('click', '.increaseAmountBtn', (e) => {
        increaseAmount(e.target.value);
    })

    $('body').on('change', '.amountInput', (e) => {
        changeOrdersAmount(e.target.value);
    })

    if (window.location.pathname === "/Carts") {
        countPrice();
    }
});

function countPrice() {
    const totalPrices = $('.totalPrice').toArray();
    const priceResult = totalPrices.reduce((acc, curr) => acc + Number(curr.textContent), 0);

    $('#priceResult').html(priceResult);
}

function addProductToCart(id) {
    $.ajax({
        url: '/Accounts/GetAccount',
        type: 'POST',
        data: { id: Number(id) },
        success: function (response) {
            if (response.success === true) {
                successNotification("Poduct has been added to your cart");
            } else if (response.success === false && (response.status === 404 || response.status === 401)) {
                errorInfo(response.status === 404 ? "You are not authorized" : "Your account is not activated");
            } else {
                console.log(response.description);
                serverErrorNotification(response);
            }
        },
        error: function (jqXHR) {
            errorNotification(jqXHR);
        }
    });
}

function deleteProductFromCart(id) {
    $.ajax({
        url: '/Orders/DeleteOrder',
        type: 'POST',
        data: { id: Number(id) },
        success: function (response) {
            if (response.success === false) {
                serverErrorNotification(response);
            } else {
                $('#cartContainer').html(response);

                countPrice();
            }
        },
        error: function (jqXHR) {
            errorNotification(jqXHR);
        }
    });
}

function decreaseAmount(props) {
    const id = Number(props.split(', ')[0]);
    const ordersAmount = Number(props.split(', ')[1]);

    if (ordersAmount - 1 === 0) {
        deleteProductFromCart(id);

        countPrice();
    } else {
        $.ajax({
            url: '/Orders/ChangeOrdersAmount',
            type: 'POST',
            data: { id: id, ordersAmount: ordersAmount, isIncrease: false },
            success: function (response) {
                if (response.success === false) {
                    serverErrorNotification(response);
                } else {
                    $('#cartContainer').html(response);

                    countPrice();
                }
            },
            error: function (jqXHR) {
                errorNotification(jqXHR);
            }
        });
    }
}

function increaseAmount(props) {
    const id = Number(props.split(', ')[0]);
    const ordersAmount = Number(props.split(', ')[1]);
    const amount = Number(props.split(', ')[2]);

    if (ordersAmount + 1 > amount) {
        errorInfo(`Only ${amount} units available`);
    } else {
        $.ajax({
            url: '/Orders/ChangeOrdersAmount',
            type: 'POST',
            data: { id: id, ordersAmount: ordersAmount, isIncrease: true },
            success: function (response) {
                if (response.success === false) {
                    serverErrorNotification(response);
                } else {
                    $('#cartContainer').html(response);

                    countPrice();
                }
            },
            error: function (jqXHR) {
                errorNotification(jqXHR);
            }
        });
    }
}

function changeOrdersAmount() {
    let id = $('.amountInput').attr('data-id');
    let amount = $('.amountInput').attr('data-amount');
    let ordersAmount = Number($('.amountInput').val());

    if (ordersAmount === 0) {
        deleteProductFromCart(id);

        countPrice();
    } else if (Number.isNaN(ordersAmount) || ordersAmount < 0) {
        errorInfo(`Incorrect input`);
    } else if (ordersAmount > amount) {
        errorInfo
    } else {
        $.ajax({
            url: '/Orders/ChangeOrdersAmountByInput',
            type: 'POST',
            data: { id: id, ordersAmount: ordersAmount },
            success: function (response) {
                if (response.success === false) {
                    serverErrorNotification(response);
                } else {
                    $('#cartContainer').html(response);

                    countPrice();
                }
            },
            error: function (jqXHR) {
                errorNotification(jqXHR);
            }
        });
    }
}