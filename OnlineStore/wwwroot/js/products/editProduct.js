import { serverErrorNotification, errorNotification } from '../notifications/notifications.js';
import { getProducts } from './createProduct.js';

$(document).ready(() => {
    $('body').on('click', '.getEditPopupBtn', (e) => {
        getEditPopup(e.target.value);
    });

    $('body').on('click', '#editProductBtn', editProduct);

    $('body').on('click', '#editProductInfoBtn', editProductInfo);
});

function getEditPopup(id) {
    const modal = $('#modal');

    $.ajax({
        url: '/Products/GetEditProductModal',
        type: 'POST',
        dataType: 'html',
        data: { id: Number(id) },
        success: function (response) {
            if (response.success === false) {
                modal.modal('show');

                serverErrorNotification(response);
            } else {
                $('.modal-dialog').css("maxWidth", "1000px");
                $('#modalTitle').text('Product edit');
                modal.find('.modal-body').html(response);
                modal.modal('show');
            }
        },
        error: function (jqXHR) {
            errorNotification(jqXHR);
        }
    });
}

function editProduct() {
    event.preventDefault();

    let formData = new FormData($('#editProductForm')[0]);
    const path = window.location.pathname.split('/');

    $.ajax({
        url: '/Products/EditProduct',
        type: 'POST',
        data: formData,
        success: function (response) {
            if (response.success === false) {
                serverErrorNotification(response);
            } else {
                $('#editProductFormContainer').html(response);

                if (path[2] === "ProductPage") {
                    getProductPagePartialView();
                } else {
                    getProducts();
                }
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

function editProductInfo() {
    event.preventDefault();

    let formData = new FormData($('#editProductInfoForm')[0]);
    const path = window.location.pathname.split('/');

    $.ajax({
        url: '/ProductsInfo/EditProductInfo',
        type: 'POST',
        data: formData,
        success: function (response) {
            if (response.success === false) {
                serverErrorNotification(response);
            } else {
                $('#editProductInfoFormContainer').html(response);

                if (path[2] === "ProductPage") {
                    getProductPagePartialView();
                } else {
                    getProducts();
                }
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

function getProductPagePartialView() {
    const partialViewContainer = $('#productPageInfo');

    $.ajax({
        url: '/Products/GetProductPagePartialView',
        type: 'GET',
        data: { id: Number($('#Id').val()) },
        success: function (response) {
            if (response.success === false) {
                serverErrorNotification(response);
            } else {
                partialViewContainer.html(response);
            }
        },
        error: function (jqXHR) {
            errorNotification(jqXHR);
        },
    });
}