import { serverErrorNotification, errorNotification } from '../notifications/notifications.js';

$(document).ready(() => {
    $('.slider-card').addClass('first');

    $('body').on('click', '#prevCardBtn', showPrevCard);

    $('body').on('click', '#nextCardBtn', showNextCard);
});

function showNextCard() {
    let cardsNumber = $('#cards').children().length;
    let shownCardDistance = $('.shown').offset().left - $('#cards').offset().left;
    const slideDistance = 300;

    $('#prevCardBtn').attr('disabled', null);

    if (!$('.slider-card').last().hasClass('shown')) {
        let nextCard = $('.shown').next('.slider-card');

        $('.shown').removeClass('shown');
        nextCard.addClass('shown');
        $('#cards').css({ 'transform': 'translateX(-' + (shownCardDistance + slideDistance) + 'px)' });
    } else {
        let id = $('.shown .id').val();

        $.ajax({
            url: '/Products/GetSliderCard',
            type: 'POST',
            data: { productId: id, isFromStart: true, cardsNumber: cardsNumber },
            success: function (response) {
                if (response.success === false) {
                    serverErrorNotification(response);
                }
                else if (response.status === 204 && $('.slider-card').last().hasClass('shown')) {
                    $('.slider-card').last().addClass('last');
                    $('.slider-card').last().removeClass('shown');
                    $('.slider-card').first().addClass('shown');
                    $('#cards').css({ 'transform': 'translateX(0px)' });
                } else {
                    let prevShownCard = $('.shown');

                    $('.shown').after(response);
                    prevShownCard.removeClass('shown');
                    $('#cards').css({ 'transform': 'translateX(-' + (cardsNumber * slideDistance) + 'px)' });
                }
            },
            error: function (jqXHR) {
                errorNotification(jqXHR);
            }
        });
    }
}

function showPrevCard() {
    let cardsNumber = $('#cards').children().length;
    let shownCardDistance = $('.shown').offset().left - $('#cards').offset().left;
    const slideDistance = 300;

    if (shownCardDistance !== 0) {
        let prevCard = $('.shown').prev();

        $('.shown').removeClass('shown');
        prevCard.addClass('shown');
        $('#cards').css({ 'transform': 'translateX(' + (-shownCardDistance + slideDistance) + 'px)' });
    }
    else if (shownCardDistance === 0 && $('.slider-card').last().hasClass('last')) {
        $('.slider-card').first().removeClass('shown');
        $('.last').addClass('shown');
        $('#cards').css({ 'transform': 'translateX(-' + ((cardsNumber - 1) * slideDistance) + 'px)' });
    }

    if ($('.shown').hasClass('first') && !$('.slider-card').last().hasClass('last')) {
        $('#prevCardBtn').prop('disabled', true);
    }
}