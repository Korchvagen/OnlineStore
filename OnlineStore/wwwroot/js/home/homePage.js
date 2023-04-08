import { errorNotification } from '../notifications/notifications.js';

$(document).ready(() => {
    if (window.location.pathname === "/") {
        getBanners();
    }

    $('.menu').on('click', (e) => {
        const isActive = $('.menu').is(e.target) && $('.menu').hasClass('active');

        $('.menu').toggleClass('active', !isActive);
        $('.menu__content').toggleClass('active', !isActive);
    });

    $(document).on('click', (e) => {
        if (!$('.menu').is(e.target) && $('.menu').has(e.target).length === 0) {
            $('.menu').removeClass('active');
            $('.menu__content').removeClass('active');
        }
    });
});

function getBanners() {
    $.ajax({
        url: '/Banners/GetBanners',
        type: 'GET',
        success: function (response) {
            if (response.success === false) {
                $('#bannerContainer').remove();
            } else {
                setBanner(response.data);
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

function setBanner(banners) {
    let bannerNumber = 0;

    let interval = setInterval(() => {
        window.onbeforeunload = function () {
            clearInterval(interval);
        };

        if (bannerNumber === banners.length - 1) {
            bannerNumber = -1;
        }

        $('#bannerImage').fadeOut(800, () => {
            $('#bannerImage').attr("src", `data:image/jpeg;base64,${banners[++bannerNumber].image}`);
            $('#bannerImage').fadeIn(400);
        });
    }, 7000);

    $('#bannerImage').on("click", () => {
        let url = location.origin + banners[bannerNumber].link;

        $(location).attr('href', url);
    });
}