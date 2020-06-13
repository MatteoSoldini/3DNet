function logout() {
    document.cookie = "token=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    window.location.replace("/login.html");
}

function upload() {
    if (!!$.cookie('token')) {
        const fileField = document.querySelector('input[type="file"]');

        const myHeaders = new Headers();
        myHeaders.append('filename', fileField.files[0].name);
        myHeaders.append('Authorization', 'Bearer ' + $.cookie('token'));

        fetch('api/_3dmodel', {
            method: 'POST',
            headers: myHeaders,
            body: fileField.files[0]
        })
            .then(response => response.json())
            .then(result => {
                M.toast({ html: 'Caricamento effettuato! ' })
                updateTable();
            })
            .catch(error => {
                M.toast({ html: 'Errore!: ' + error })
            });
    } else {
        // no cookie
    }
        
}

function updateTable() {
    const myHeaders = new Headers();
    myHeaders.append('Authorization', 'Bearer ' + $.cookie('token'));

    fetch('api/_3dModel', {
        method: 'GET',
        headers: myHeaders
    })
        .then(response => response.json())
        .then(result => {
            var data = ''
            result.forEach(function (entry) {
                var button = '<a href=object.html?file=' + entry.id + ' class="btn-floating btn-large waves effect waves-light red"><i class="material-icons">3d_rotation</i></a>';
                data += '<tr><td>' + entry.file_location + '</td><td>' + button + '</td></tr >';
                $('#content').html(data);
                console.log(entry);
            });

            M.toast({ html: 'Caricamento effettuato! ' })
        })
        .catch(error => {
            M.toast({ html: 'Errore!: ' + error })
        });
}

$(document).ready(function () {
    M.AutoInit();

    if (!!$.cookie('token')) {
        $('.bg').hide();
        $('#upload').show();
        $('#table').show();
        updateTable()
        $('#nav-mobile').html('<li><a onclick="logout()"><b>Logout</b></a></li>');
    }
    else {
        $('.bg').show();
        $('#upload').hide();
        $('#table').hide();
        $('#nav-mobile').html('<li><a href="login.html"><b>Login</b></a></li><li> <a href="signup.html"><b>Registrati</b></a></li >');
    }
});