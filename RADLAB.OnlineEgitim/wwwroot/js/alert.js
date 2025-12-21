function SweetConfirm(title, text, icon, confirmButtonText, cancelButtonText) {
    return new Promise(resolve => {
        Swal.fire({
            title,
            text,
            icon,
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText,
            cancelButtonText
        }).then((result) => {
            resolve(result.isConfirmed);
        })
    });
}

function SweetMessage(title, text, icon) {
    return new Promise(resolve => {
        Swal.fire(
            title,
            text,
            icon
        )
    });
}

function SweetToast(title, icon) {
    const Toast = Swal.mixin({
        toast: true,
        position: 'bottom-end',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    })

    return new Promise(resolve => {
        Toast.fire({
            icon,
            title
        })
    });
}

//function SweetBigConfirm(title, text, icon, confirmButtonText, deniedButtonText, cancelButtonText) {
//    return new Promise(resolve => {
//        Swal.fire({
//            title,
//            text,
//            icon,
//            showDenyButton: true,
//            showCancelButton: true,
//            confirmButtonColor: '#3085d6',
//            cancelButtonColor: '#d33',
//            confirmButtonText,
//            deniedButtonText,
//            cancelButtonText
//        }).then((result) => {
//            resolve(result.isConfirmed);
//        })
//    });
//}