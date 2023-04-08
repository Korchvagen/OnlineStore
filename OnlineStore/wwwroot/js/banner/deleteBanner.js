import { successNotification, serverErrorNotification, errorNotification } from '../notifications/notifications.js';
import { getBannersPartialView } from './editBanner.js';

$(document).ready(() => {
    $('body').on('click', '.deleteBannerBtn', (e) => {
        deleteBanner(e.target.value);
    });
});

function deleteBanner(id) {
    event.preventDefault();

    $.ajax({
        url: '/Banners/DeleteBanner',
        type: 'POST',
        data: { id: id },
        success: function (response) {
            if (response.success === false) {
                serverErrorNotification(response);
            } else {
                successNotification('Banner has been deleted');

                getBannersPartialView();
            }
        },
        error: function (jqXHR) {
            errorNotification(jqXHR);
        }
    });
}