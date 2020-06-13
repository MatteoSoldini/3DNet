$(document).ready(function () {
    M.AutoInit();
});

function getFormData($form) {
    var unindexed_array = $form.serializeArray();
    var indexed_array = { 'supplier' : $('#mySwitch').prop('checked') };

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}

function Send() {
    var $form = $("#form");
    var data = getFormData($form);

    fetch('api/signup', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data),
    })
        .then(response => response.json())
        .then(() => {
            M.toast({ html: 'Registrazione effettuata! ' })

            var user = {
                'email': data.email,
                'password': data.password
            }

            fetch('api/login', {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(user),
            })
                .then(response => response.json())
                .then(data => {
                    M.toast({ html: 'Accesso effettuato! ' })
                    document.cookie = "token=" + data.token + "; max-age=7200; path=/";
                    window.location.replace("/");
                })
                .catch(error => M.toast({ html: 'Errore! ' + error }));
        })
        .catch(error => M.toast({ html: 'Errore! ' + error }));
}