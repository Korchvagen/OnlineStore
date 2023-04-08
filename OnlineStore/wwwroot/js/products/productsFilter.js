import { serverErrorNotification, errorNotification } from '../notifications/notifications.js';
import { getProducts } from './createProduct.js';

$(document).ready(() => {
    $('body').on('click', '#productsFilterBtn', productsFilter);

    $('body').on('click', '#clearFilterBtn', () => {
        event.preventDefault();

        $('#productsFilterForm').trigger('reset');

        getProducts();
    });
});

function productsFilter() {
    event.preventDefault();

    const cardsContainer = $('#cardsContainer');
    const formData = new FormData($('#productsFilterForm')[0]);

    $.ajax({
        url: '/Products/ProductsFilter',
        type: 'POST',
        data: formData,
        cache: false,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.success === false) {
                serverErrorNotification(response);
            } else {
                cardsContainer.html(response);
            }
        },
        error: function (jqXHR) {
            errorNotification(jqXHR);
        }
    });
}