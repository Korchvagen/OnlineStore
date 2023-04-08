import { successNotification, serverErrorNotification, errorNotification } from '../notifications/notifications.js';

$(document).ready(() => {
    $('body').on('click', '.deleteProductBtn', (e) => {
        deleteProduct(e.target.value);
    })

    $('body').on('click', '#deletionConfirmedBtn', (e) => {
        deletionConfirmed(e.target.value);
    })
});

function deleteProduct(id) {
    event.preventDefault();

    const modal = $('#modal');

    $('#modalTitle').text('Delete product');

    $.ajax({
        url: '/Products/DeleteProduct',
        type: 'POST',
        data: { id: Number(id) },
        success: function (response) {
            if (response.success === false) {
                serverErrorNotification(response);
            } else {
                $('.modal-dialog').css("maxWidth", "900px");
                modal.find(".modal-body").html(response);
                modal.modal('show');
            }
        },
        error: function (jqXHR) {
            errorNotification(jqXHR);
        }
    });
}

function deletionConfirmed(id) {
    event.preventDefault();

    const modal = $('#modal');
    const cardsContainer = $('#cardsContainer');

    $.ajax({
        url: '/Products/ConfirmedDeletion',
        type: 'POST',
        data: { id: Number(id) },
        success: function (response) {
            if (response.success === false) {
                modal.modal('show');

                serverErrorNotification(response);
            } else {
                cardsContainer.html(response);
                modal.modal('hide');

                successNotification('Product has been deleted');
            }
        },
        error: function (jqXHR) {
            errorNotification(jqXHR);
        }
    });
}