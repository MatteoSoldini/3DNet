function getVolume(geometry) {
    let position = geometry.attributes.position;
    let faces = position.count / 3;
    let sum = 0;
    let p1 = new THREE.Vector3(),
        p2 = new THREE.Vector3(),
        p3 = new THREE.Vector3();
    for (let i = 0; i < faces; i++) {
        p1.fromBufferAttribute(position, i * 3 + 0);
        p2.fromBufferAttribute(position, i * 3 + 1);
        p3.fromBufferAttribute(position, i * 3 + 2);
        sum += signedVolumeOfTriangle(p1, p2, p3);
    }
    return sum;
}

function signedVolumeOfTriangle(p1, p2, p3) {
    return p1.dot(p2.cross(p3)) / 6.0;
}

function STLViewer(model, elementID) {
    var elem = document.getElementById(elementID)

    var camera = new THREE.PerspectiveCamera(70,
        elem.clientWidth / elem.clientHeight, 1, 1000);

    var renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true });
    renderer.setSize(elem.clientWidth, elem.clientHeight);
    elem.appendChild(renderer.domElement);

    window.addEventListener('resize', function () {
        renderer.setSize(elem.clientWidth, elem.clientHeight);
        camera.aspect = elem.clientWidth / elem.clientHeight;
        camera.updateProjectionMatrix();
    }, false);

    var controls = new THREE.OrbitControls(camera, renderer.domElement);
    controls.enableDamping = true;
    controls.rotateSpeed = 0.20;
    controls.dampingFactor = 0.1;
    controls.enableZoom = true;
    controls.autoRotate = true;
    controls.autoRotateSpeed = .75;

    var scene = new THREE.Scene();
    scene.add(new THREE.HemisphereLight(0xffffff, 1.5));

    (new THREE.STLLoader()).load(model, function (geometry) {
        var material = new THREE.MeshPhongMaterial({
            color: 0x209c90,
            specular: 100,
            shininess: 100
        });
        var mesh = new THREE.Mesh(geometry, material);
        scene.add(mesh);

        var middle = new THREE.Vector3();
        geometry.computeBoundingBox();
        geometry.boundingBox.getCenter(middle);
        mesh.geometry.applyMatrix(new THREE.Matrix4().makeTranslation(
            -middle.x, -middle.y, -middle.z));

        var largestDimension = Math.max(geometry.boundingBox.max.x,
            geometry.boundingBox.max.y,
            geometry.boundingBox.max.z)
        camera.position.z = largestDimension * 1.5;

        var description = '<h5>Volume: <b>' + Math.floor(getVolume(geometry) / 1000) + 'cm<sup>3</sup></b>';

        $('#description').html(description);

        var animate = function () {
            requestAnimationFrame(animate);
            controls.update();
            renderer.render(scene, camera);
        };
        animate();
    });
}


$(document).ready(function () {
    M.AutoInit();

    if (!!$.cookie('token')) {
        $('#nav-mobile').html('<li><a onclick="logout()"><b>Logout</b></a></li>');
    }
    else {
        $('#nav-mobile').html('<li><a href="login.html"><b>Login</b></a></li><li> <a href="signup.html"><b>Registrati</b></a></li >');
    }

    const myHeaders = new Headers();
    myHeaders.append('Authorization', 'Bearer ' + $.cookie('token'));

    var url_string = window.location.href;
    var url = new URL(url_string);
    var paramValue = url.searchParams.get("file");

    fetch("api/stlfiles/" + paramValue, {
        method: 'GET',
        headers: myHeaders
    })
        .then(result => {
            console.log(result);
            STLViewer(result.url, "model")
            M.toast({ html: 'Caricamento effettuato! ' })
        })
        .catch(error => {
            M.toast({ html: 'Errore!: ' + error })
        });
});