// Ссылка на элемент веб страницы в котором будет отображаться графика
var container;
// Переменные "камера", "сцена" и "отрисовщик"
var camera, scene, renderer;
var sphere;
var clock = new THREE.Clock();

// Функция инициализации камеры, отрисовщика, объектов сцены и т.д.
init();
// Обновление данных по таймеру браузера
animate();

// В этой функции можно добавлять объекты и выполнять их первичную настройку
function init() {
    // Получение ссылки на элемент html страницы
    container = document.getElementById("container");
    // Создание "сцены"
    scene = new THREE.Scene();
    // Установка параметров камеры
    // 45 - угол обзора
    // window.innerWidth / window.innerHeight - соотношение сторон
    // 1 - 4000 - ближняя и дальняя плоскости отсечения
    camera = new THREE.PerspectiveCamera(
        45,
        window.innerWidth / window.innerHeight,
        1,
        4000
    );
    // Установка позиции камеры
    camera.position.set(0, 100, 0);

    // Установка точки, на которую камера будет смотреть
    camera.lookAt(new THREE.Vector3(0, 0.0, 0));
    // Создание отрисовщика
    renderer = new THREE.WebGLRenderer({
        antialias: false,
    });
    renderer.setSize(window.innerWidth, window.innerHeight);
    // Закрашивание экрана синим цветом, заданным в 16ричной системе
    renderer.setClearColor(0x000000ff, 1);
    container.appendChild(renderer.domElement);
    // Добавление функции обработки события изменения размеров окна
    window.addEventListener(
        "resize",
        onWindowResize,
        false
    );

    //add sphere
    var geometry = new THREE.SphereGeometry(35, 64, 64);
    // var material = new THREE.MeshBasicMaterial({ color: 0xffffff });
    // var sphere = new THREE.Mesh(geometry, material);
    // scene.add(sphere);
    //создание списка смещений для вершин
    //создание списка смещений для вершин
    var displacement = new Float32Array(
        geometry.attributes.position.array.length
    );
    for (
        var i = 0;
        i < geometry.attributes.position.array.length;
        i++
    ) {
        var rand = Math.random() * 2 - 1; //случайное значение в диапазоне от -1 до 1
        displacement[i] = rand;
    }
    
    geometry.computeTangents();
    
    for (
        var i = 0;
        i < geometry.attributes.tangent.array.length;
        i++
    ) {
        geometry.attributes.tangent.array[i] =
            -geometry.attributes.tangent.array[i];
    }

    //установка списка смещений в качестве атрибута геометрии
    geometry.setAttribute(
        "displacement",
        new THREE.BufferAttribute(displacement, 1)
    );

    //загрузка текстуры
    var earthTex = new THREE.TextureLoader().load(
        "img/earth_atmos_2048.jpg"
    );
    var earthTex_ = new THREE.TextureLoader().load(
        "img/earth_lights_2048.png"
    );

    var earthTex_norm = new THREE.TextureLoader().load(
        "img/earth_normal_2048.jpg"
    );

    var shaderMaterial = new THREE.ShaderMaterial({
        uniforms: {
            dTex: { value: earthTex }, //текстура
            nTex: { value: earthTex_ },
            normTex: { value: earthTex_norm }, //текстура
            lightPosition: { value: new THREE.Vector3(10000.0, 0.0, 0.0) },            color: {
                value: new THREE.Vector4(
                    1.0,
                    1.0,
                    1.0,
                    1.0
                ),
            },
            ambientColor: {
                value: new THREE.Vector4(
                    0.5,
                    0.5,
                    0.5,
                    1.0
                ),
            },
            lightColor: {
                value: new THREE.Vector4(
                    1.0,
                    1.0,
                    1.0,
                    1.0
                ),
            },
            //,
            //amp: { value: 1.0 }, //аплитуда
        },
        vertexShader:
            document.getElementById("vertexShader")
                .textContent,
        fragmentShader: document.getElementById(
            "fragmentShader"
        ).textContent,
    });

    sphere = new THREE.Mesh(geometry, shaderMaterial);
    sphere.rotation.y = Math.PI / -1.5;
    sphere.rotation.x = Math.PI / -4;
    scene.add(sphere);
}

function onWindowResize() {
    // Изменение соотношения сторон для виртуальной камеры
    camera.aspect = window.innerWidth / window.innerHeight;
    camera.updateProjectionMatrix();
    // Изменение соотношения сторон рендера
    renderer.setSize(window.innerWidth, window.innerHeight);
}

// В этой функции можно изменять параметры объектов и обрабатывать действия пользователя
function animate() {
    var time = clock.getDelta();
    sphere.rotation.y += (Math.PI /4*time);
    //sphere.material.uniforms.amp.value = Math.cos(time) * 5;
    // Добавление функции на вызов, при перерисовки браузером страницы
    requestAnimationFrame(animate);
    render();
}
function render() {
    // Рисование кадра
    renderer.render(scene, camera);
}
