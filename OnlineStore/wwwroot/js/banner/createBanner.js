import { successNotification, serverErrorNotification, errorNotification } from '../notifications/notifications.js';
import { getBannersPartialView } from './editBanner.js';

$(document).ready(() => {
    $('body').on('click', '#createBannerFormBtn', getCreateBannerForm);
    $('body').on('click', '#createBannerBtn', createBanner);
    $('body').on('click', '#cancelCreateBannerBtn', cancelCreateBanner);
});

function getCreateBannerForm() {
    $.ajax({
        url: '/Banners/GetCreateBannerPartialView',
        type: 'GET',
        success: function (response) {
            $('.editBannersContainer__item').last().after(response);
            $('#cancelCreateBannerBtn').show();
            $('#createBannerFormBtn').hide();
        },
        error: function (jqXHR) {
            errorNotification(jqXHR);
        }
    });
}

function createBanner() {
    event.preventDefault();

    let formData = new FormData($('#createBannerForm')[0]);

    $.ajax({
        url: '/Banners/CreateBanner',
        type: 'POST',
        data: formData,
        success: function (response) {
            if (typeof response === "string") {
                $('#createBannerContainer').html(response);
            } else if (response.success === false) {
                serverErrorNotification(response);
            } else {
                successNotification('Banner has been created');
                $('#cancelCreateBannerBtn').show();
                $('#createBannerFormBtn').hide();

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

function cancelCreateBanner() {
    $('#createBannerContainer').remove();
    $('#cancelCreateBannerBtn').hide();
    $('#createBannerFormBtn').show();
}