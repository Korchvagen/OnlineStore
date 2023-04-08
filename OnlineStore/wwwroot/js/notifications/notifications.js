function successNotification(text) {
    Swal.fire({
        position: 'top-end',
        icon: 'success',
        text: text,
        showConfirmButton: false,
        timer: 1000
    });
}

function serverErrorNotification(response) {
    Swal.fire({
        position: 'top-end',
        icon: 'error',
        text: `${response.description}`,
        showConfirmButton: false,
        timer: 1500
    });
}

function errorNotification(jqXHR) {
    Swal.fire({
        position: 'top-end',
        icon: 'error',
        text: `${jqXHR.description}`,
        showConfirmButton: false,
        timer: 1500
    });
}

function errorInfo(text) {
    Swal.fire({
        position: 'top-end',
        icon: 'error',
        text: text,
        showConfirmButton: false,
        timer: 1500
    });
}

export { successNotification, serverErrorNotification, errorNotification, errorInfo };