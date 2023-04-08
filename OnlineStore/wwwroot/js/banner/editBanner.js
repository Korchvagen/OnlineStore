import { serverErrorNotification, errorNotification, successNotification } from '../notifications/notifications.js';

$(document).ready(() => {
    $('body').on('click', '#openeditBannerFromBtn', getBannersPartialView);

    $('body').on('click', '.editBannerBtn', editBanner);
});

function getBannersPartialView() {
    const modal = $('#modal');

    $('#modalTitle').text('Banner edit');

    $.ajax({
        url: '/Banners/GetEditBannersPartialView',
        type: 'GET',
        success: function (response) {
            if (response.success === false) {
                modal.modal('hide');

                serverErrorNotification(response);
            } else {
                $('.modal-dialog').css("maxWidth", "1000px");
                modal.find(".modal-body").html(response);
                $('#cancelCreateBannerBtn').hide();
                modal.modal('show');
            }
        },
        error: function (jqXHR) {
            errorNotification(jqXHR);
        }
    });
}

function editBanner(button) {
    event.preventDefault();

    let formContainer = button.target.closest('.form-container');
    let form = button.target.closest('.editBannerForm');
    let formData = new FormData(form);

    $.ajax({
        url: '/Banners/EditBanner',
        type: 'POST',
        data: formData,
        success: function (response) {
            if (typeof response === "string") {
                formContainer.innerHTML = response;
            }else if (response.success === false) {
                serverErrorNotification(response);
            } else {
                successNotification('Banner has been updated');

                getBannersPartialView();
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

export { getBannersPartialView };