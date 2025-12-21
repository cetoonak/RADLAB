window.saveAsFile = function (fileName, byteBase64) {
    var link = this.document.createElement('a');
    link.download = fileName;
    link.href = "data:application/octet-stream;base64," + byteBase64;
    this.document.body.appendChild(link);
    link.click();
    this.document.body.removeChild(link);
    //alert("Downloaded");
}

window.onblur = function () {
    try {
        var video = document.getElementById("video");
        video.pause();
    }
    catch {

    }
}

function loadVideo(time) {
    try {
        var video = document.getElementById("video");
        video.load();
        video.currentTime = time;
    }
    catch {

    }
}

function videoTimeUpdate() {
    try {
        var video = document.getElementById("video");
        if (!video.seeking) {
            var HFSupposedCurrentTime = document.getElementById("HFSupposedCurrentTime");
            var HFSonKaydedilenTime = document.getElementById("HFSonKaydedilenTime");
            var time = video.currentTime;

            var timeInt = Math.round(video.currentTime);
            var sonKaydedilenTime = Math.round(HFSonKaydedilenTime.value);

            HFSupposedCurrentTime.value = timeInt;

            if (sonKaydedilenTime + 2 == timeInt) {
                sonKaydedilenTime = sonKaydedilenTime + 2;
                document.getElementById("HFSonKaydedilenTime").value = sonKaydedilenTime;
                document.getElementById("btnVideoTimeKaydet").click();
            }

            //var timeInt = Math.round(time);

            //if ((timeInt % 2) == 0) {
                //document.getElementById("btnVideoTimeKaydet").click();
            //}
        }
    }
    catch {

    }
}

function videoSeeking() {
    try {
        var video = document.getElementById("video");
        var supposedCurrentTime = document.getElementById("HFSupposedCurrentTime").value;

        var delta = video.currentTime - supposedCurrentTime;

        if (delta > 0.01) {
            video.currentTime = supposedCurrentTime;
        }

        /*if (Math.abs(delta) > 0.01) {
            video.currentTime = supposedCurrentTime;
        }*/
    }
    catch {

    }
}

function bsPopupAc(id) {
    document.getElementById(id + "Ac").click();
}

function bsPopupKapat(id) {
    document.getElementById(id + "Kapat").click();
}

function git(id) {
    document.querySelector('#' + id).scrollIntoView();
}

//function litePick(id) {
//    const picker = new Litepicker({
//        element: document.getElementById(id),
//        format: 'DD.MM.YYYY',
//        firstDay: 1,
//        lang: 'tr-TR',
//        singleMode: false,
//        tooltipText: {
//            one: 'gün',
//            other: 'gün'
//        },
//        tooltipNumber: (totalDays) => {
//            return totalDays - 1;
//        }
//    });
//}


//function easyPick(id) {
//    const picker = new easepick.create({
//        element: document.getElementById(id),
//        lang: "tr-TR",
//        format: "DD.MM.YYYY",
//        css: [
//            'css/easypicker.css',
//        ],
//    });

//    document.getElementById(id).readOnly = false;
//}

//function easyPickRange(id) {
//    const picker = new easepick.create({
//        element: document.getElementById(id),
//        lang: "tr-TR",
//        format: "DD.MM.YYYY",
//        css: [
//            'css/easypicker.css',
//        ],
//        plugins: ['RangePlugin'],
//        RangePlugin: {
//            tooltipNumber(num) {
//                return num - 1;
//            },
//            locale: {
//                one: 'gün',
//                other: 'gün',
//            },
//        },
//    });

//    document.getElementById(id).readOnly = false;
//}

function dateRangePick(id, range) {
    $('#' + id).daterangepicker({
        opens: 'right',
        autoApply: range == false,
        autoUpdateInput: false,
        singleDatePicker: range == false,
        showDropdowns: false,
        //minDate: '01/28/2020'
        //maxDate: '01/28/2022'
        //minYear: 1901,
        //maxYear: 2100,
        locale: {
            'format': 'DD.MM.YYYY',
            'separator': ' - ',
            'applyLabel': 'Tamam',
            'cancelLabel': 'Vazgeç',
            'fromLabel': 'den',
            'toLabel': 'ile',
            'customRangeLabel': 'Özel',
            'weekLabel': 'H',
            'daysOfWeek': [
                'Paz',
                'Pzt',
                'Sal',
                'Çar',
                'Per',
                'Cum',
                'Cmt'
            ],
            'monthNames': [
                'Ocak',
                'Şubat',
                'Mart',
                'Nisan',
                'Mayıs',
                'Haziran',
                'Temmuz',
                'Ağustos',
                'Eylül',
                'Ekim',
                'Kasım',
                'Aralık'
            ],
            'firstDay': 1
        }
    });

    $('#' + id).on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format('DD.MM.YYYY') + ' - ' + picker.endDate.format('DD.MM.YYYY'));
    });

    $('#' + id).on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
    });
}

function getValue (id) {
    return document.getElementById(id).value;
};

function setValue(id, value) {
    document.getElementById(id).value = value;
};

function tooltipTrigger() {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    })
};







function tbKrediKartiNoOnInput() {
    $('#tbKrediKartiNo').on('input propertychange paste', function () {
        var value = $('#tbKrediKartiNo').val();
        var formattedValue = formatCardNumber(value);
        $('#tbKrediKartiNo').val(formattedValue);
    });
}

function formatCardNumber(value) {
    var value = value.replace(/\D/g, '');
    var formattedValue;
    var maxLength;
    // american express, 15 digits
    if ((/^3[47]\d{0,13}$/).test(value)) {
        formattedValue = value.replace(/(\d{4})/, '$1 ').replace(/(\d{4}) (\d{6})/, '$1 $2 ');
        maxLength = 17;
    } else if ((/^3(?:0[0-5]|[68]\d)\d{0,11}$/).test(value)) { // diner's club, 14 digits
        formattedValue = value.replace(/(\d{4})/, '$1 ').replace(/(\d{4}) (\d{6})/, '$1 $2 ');
        maxLength = 16;
    } else if ((/^\d{0,16}$/).test(value)) { // regular cc number, 16 digits
        formattedValue = value.replace(/(\d{4})/, '$1 ').replace(/(\d{4}) (\d{4})/, '$1 $2 ').replace(/(\d{4}) (\d{4}) (\d{4})/, '$1 $2 $3 ');
        maxLength = 19;
    }

    $('#tbKrediKartiNo').attr('maxlength', maxLength);
    return formattedValue;
}

function printDiv() {
    window.print();
}

function click(id) {
    document.getElementById(id).click();
}