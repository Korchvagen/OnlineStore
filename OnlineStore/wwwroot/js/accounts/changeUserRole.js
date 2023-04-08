import { successNotification, serverErrorNotification, errorNotification } from '../notifications/notifications.js';

$(document).ready(() => {
    $('body').on('click', '.changeRoleBtn', (e) => {
        changeRole(e.target);
    });
});

function changeRole(button) {
    const currentRole = $(button).parent().prev().find('.currentRole').val();
    const selectedRole = $(button).parent().prev().find('.selectedRole :selected').val();

    if (currentRole !== selectedRole) {
        $.ajax({
            url: '/Accounts/ChangeRole',
            type: 'POST',
            data: { accountId: Number(button.value), selectedRole: selectedRole },
            success: function (response) {
                if (response.success) {
                    $(button).parent().prev().find('.currentRole').val(selectedRole);

                    successNotification('Role successfully changed');
                } else {
                    serverErrorNotification(response);
                }
            },
            error: function (jqXHR) {
                errorNotification(jqXHR);
            }
        });
    }
}