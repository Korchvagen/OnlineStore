import { successNotification, serverErrorNotification, errorNotification } from '../notifications/notifications.js';

$(document).ready(() => {
    $('body').on('click', '#openModalBtn', openModal);

    $('body').on('click', '#createProductBtn', createProduct);

    $('body').on('click', '#createProductInfoBtn', createProductInfo);

    $('body').on('click', '#openPrevFormBtn', openPrevForm);

    $('body').on('click', '#closeModalBtn', closeModal);

    $('#modal').on('click', function (e) {
        if (e.target.classList.contains('modal')) {
            closeModal();
        }
    });

    window.onbeforeunload = function () {
        if ($('#modal').hasClass('show')) {
            closeModal();
        }
    };
});

function createProduct() {
    event.preventDefault();

    const modal = $('#modal');
    let formData = new FormData($('#newProductForm')[0]);

    $.ajax({
        url: '/Products/CreateProduct',
        type: 'POST',
        data: formData,
        success: function (response) {
            if (response.success === false) {
                serverErrorNotification(response);
            } else {
                modal.find(".modal-body").html(response);
                modal.modal('show');
            }
        },
        error: function (jqXHR) {
            errorNotification(jqXHR);
        },
        cache: false,
        contentType: false,
        processData: false
    });
}

function createProductInfo() {
    event.preventDefault();

    const modal = $('#modal');
    let formData = new FormData($('#newProductInfoForm')[0]);

    $.ajax({
        url: '/ProductsInfo/CreateProductInfo',
        type: 'POST',
        data: formData,
        success: function (response) {
            if (typeof response === "string") {
                modal.find(".modal-body").html(response);
                modal.modal('show');
            } else if (response.success === false) {
                modal.modal('hide');

                serverErrorNotification(response);
            } else {
                getProducts();
                modal.modal('hide');

                successNotification('Product has been created');
            }
        },
        error: function (jqXHR) {
            errorNotification(jqXHR);
        },
        cache: false,
        contentType: false,
        processData: false
    });
}

function getProducts() {
    const cardsContainer = $('#cardsContainer');

    $.ajax({
        url: '/Products/GetCardsPartialView',
        type: 'GET',
        success: function (response) {
            if (response.success === false) {
                serverErrorNotification(response);
            } else {
                cardsContainer.html(response);
            }
        },
        error: function (jqXHR) {
            errorNotification(jqXHR);
        },
    });
}

function openModal() {
    const modal = $('#modal');

    $('#modalTitle').text('Create new product');

    $.ajax({
        type: 'GET',
        url: '/Products/GetCreatePartialView',
        success: function (response) {
            modal.find(".modal-body").html(response);
            modal.modal('show');
        },
        error: function (jqXHR) {
            errorNotification(jqXHR);
        }
    });
}

function openPrevForm() {
    event.preventDefault();

    const modal = $('#modal');
    let formData = new FormData($('#newProductInfoForm')[0]);

    $.ajax({
        url: '/ProductsInfo/RememberProductInfoData',
        type: 'POST',
        data: formData,
        success: function (response) {
            if (response.success === false) {
                serverErrorNotification(response);
            } else {
                modal.find(".modal-body").html(response);
                modal.modal('show');
            }
        },
        error: function (jqXHR) {
            errorNotification(jqXHR);
        },
        cache: false,
        contentType: false,
        processData: false
    });
}

function closeModal() {
    event.preventDefault();

    const modal = $('#modal');
    const id = $('#productCreationId').val();

    if (id === "" || $('#modalTitle').text() !== "Create new product") {
        modal.modal('hide');
    } else {
        $.ajax({
            url: '/Products/CloseModal',
            type: 'POST',
            data: { id: id },
            success: function (response) {
                if (response.success) {
                    modal.modal('hide');
                } else {
                    modal.modal('show');

                    serverErrorNotification(response);
                }
            },
            error: function (jqXHR) {
                errorNotification(jqXHR);
            }
        });
    }

    $('.modal-dialog').removeAttr('style');
}

export { getProducts };