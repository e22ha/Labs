// Ссылка на элемент веб страницы в котором будет отображаться графика
var container;
// Переменные "камера", "сцена" и "отрисовщик"
var camera, scene, renderer;

var keyboard = new THREEx.KeyboardState();

var chase = -1;

var angle = Math.PI / 2;

var planets = [];

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
    camera.position.set(0, 150, 0);

    // Установка точки, на которую камера будет смотреть
    camera.lookAt(new THREE.Vector3(0, 0.0, 0));
    // Создание отрисовщика
    renderer = new THREE.WebGLRenderer({ antialias: false });
    renderer.setSize(window.innerWidth, window.innerHeight);
    // Закрашивание экрана синим цветом, заданным в 16ричной системе
    renderer.setClearColor(0x000000ff, 1);
    container.appendChild(renderer.domElement);
    // Добавление функции обработки события изменения размеров окна
    window.addEventListener("resize", onWindowResize, false);

    const lightAmbient = new THREE.AmbientLight(0x202020); // soft white light
    scene.add(lightAmbient);

    const light = new THREE.PointLight(0xffffff);
    light.position.set(0, 0, 0);
    scene.add(light);

    addSphere(10, "imgs/sunmap.jpg");
    addSphere(3850, "imgs/starmap.jpg");

    planets.push(
        addPlanet(
            2,
            "imgs/Меркурий/mercurymap.jpg",
            20,
            1.5666666667,
            0.0043478261,
            null
        )
    );

    planets.push(
        addPlanet(
            2.5,
            "imgs/Венера/venusmap.jpg",
            30,
            1.1666666667,
            7.6956521739,
            null
        )
    );

    planets.push(
        addPlanet(
            4,
            "imgs/Земля/earthmap1k.jpg",
            45,
            1,
            1,
            addPlanet(0.7, "imgs/Земля/Луна/moonmap1k.jpg", 6, 0.4, 0.1, null)
        )
    );

    planets.push(
        addPlanet(
            3,
            "imgs/Марс/marsmap1k.jpg",
            60,
            0.8043333333,
            1.0869565217,
            null
        )
    );
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
    var delta = clock.getDelta();

    keys(delta);
    keysAngle(delta);

    for (var i = 0; i < planets.length; i++) {
        //создание набора матриц
        var m = new THREE.Matrix4();
        var m1 = new THREE.Matrix4();
        var m2 = new THREE.Matrix4();

        planets[i].a1 += planets[i].s1 * delta;
        planets[i].a2 += planets[i].s2 * delta;

        //создание матрицы поворота (вокруг оси Y) в m1 и матрицы перемещения в m2
        m1.makeRotationY(planets[i].a1);
        m2.setPosition(new THREE.Vector3(planets[i].x, 0, 0));

        //запись результата перемножения m1 и m2 в m
        m.multiplyMatrices(m1, m2);

        m1.makeRotationY(planets[i].a2);

        m.multiplyMatrices(m, m1);

        //установка m в качестве матрицы преобразований объекта object
        planets[i].sphere.matrix = m;
        planets[i].sphere.matrixAutoUpdate = false;

        if (planets[i].sat != null) {
            var sm = new THREE.Matrix4();
            var sm1 = new THREE.Matrix4();
            var sm2 = new THREE.Matrix4();

            planets[i].sat.a1 += planets[i].sat.s1 * delta;
            planets[i].sat.a2 += planets[i].sat.s2 * delta;

            //создание матрицы поворота (вокруг оси Y) в m1 и матрицы перемещения в m2
            sm1.makeRotationY(planets[i].sat.a1);
            sm2.setPosition(new THREE.Vector3(planets[i].sat.x, 0, 0));

            //запись результата перемножения m1 и m2 в m
            sm.multiplyMatrices(sm2, sm1);

            sm1.makeRotationY(planets[i].sat.a2);

            sm.multiplyMatrices(m1, sm);

            //получение матрицы позиции из матрицы объекта
            var mm = new THREE.Matrix4();
            mm.copyPosition(m);
            //получение позиции из матрицы позиции
            var pos = new THREE.Vector3(0, 0, 0);
            pos.setFromMatrixPosition(mm);

            sm2.setPosition(pos);

            sm.multiplyMatrices(m, sm);

            //установка m в качестве матрицы преобразований объекта object
            planets[i].sat.sphere.matrix = sm;
            planets[i].sat.sphere.matrixAutoUpdate = false;

            planets[i].sat.traj.position.copy(pos);
        }
    }

    // Добавление функции на вызов, при перерисовки браузером страницы
    requestAnimationFrame(animate);
    render();
}
function render() {
    // Рисование кадра
    renderer.render(scene, camera);
}

function addSphere(r, tname) {
    //создание геометрии сферы
    var geometry = new THREE.SphereGeometry(r, 32, 32);
    //загрузка текстуры
    var tex = new THREE.TextureLoader().load(tname);
    tex.minFilter = THREE.NearestFilter;
    //создание материала
    var material = new THREE.MeshBasicMaterial({
        map: tex,
        side: THREE.DoubleSide,
    });
    //создание объекта
    var sphere = new THREE.Mesh(geometry, material);
    //размещение объекта в сцене
    scene.add(sphere);
}

function addPlanet(r, tname, x, s1, s2, sat) {
    //создание геометрии сферы
    var geometry = new THREE.SphereGeometry(r, 32, 32);
    //загрузка текстуры
    var tex = new THREE.TextureLoader().load(tname);
    tex.minFilter = THREE.NearestFilter;
    //создание материала
    var material = new THREE.MeshPhongMaterial({
        map: tex,
        side: THREE.DoubleSide,
    });
    //создание объекта
    var sphere = new THREE.Mesh(geometry, material);
    sphere.position.x = x;
    //размещение объекта в сцене
    scene.add(sphere);

    var planet = {};
    planet.sphere = sphere;
    planet.x = x;
    planet.r = r;
    planet.s1 = s1;
    planet.a1 = 0.0;
    planet.s2 = s2;
    planet.a2 = 0.0;
    planet.sat = sat;
    planet.traj = traj(x);

    return planet;
}

function traj(x0) {
    //создание материала для пунктирной линии
    var dashed_material = new THREE.LineDashedMaterial({
        color: 0xffff00, //цвет линии
        dashSize: 2, //размер сегмента
        gapSize: 2, //величина отступа между сегментами
    });

    var points = []; //массив для хранения координат сегментов

    for (var i = 0; i < 360; i++) {
        var x = x0 * Math.cos((i * Math.PI) / 180);
        var z = x0 * Math.sin((i * Math.PI) / 180);
        points.push(new THREE.Vector3(x, 0, z)); //начало линии
    }

    //points.push(new THREE.Vector3(10, 0, 10)); //завершение линии

    var geometry = new THREE.BufferGeometry().setFromPoints(points); //создание геометрии
    var line = new THREE.Line(geometry, dashed_material); //создание модели

    line.computeLineDistances(); //вычисление дистанции между сегментами
    scene.add(line); //добавление модели в сцену

    return line;
}

function keys(delta) {
    if (keyboard.pressed("0")) {
        chase = -1;
    }

    if (keyboard.pressed("1")) {
        chase = 0;
    }
    if (keyboard.pressed("2")) {
        chase = 1;
    }
    if (keyboard.pressed("3")) {
        chase = 2;
    }
    if (keyboard.pressed("4")) {
        chase = 3;
    }

    if (chase > -1) {
        //получение матрицы позиции из матрицы объекта
        var mm = new THREE.Matrix4();
        mm.copyPosition(planets[chase].sphere.matrix);
        //получение позиции из матрицы позиции
        var pos = new THREE.Vector3(0, 0, 0);
        pos.setFromMatrixPosition(mm);

        var x =
            pos.x + planets[chase].r * 4 * Math.cos(angle - planets[chase].a1);
        var z =
            pos.z + planets[chase].r * 4 * Math.sin(angle - planets[chase].a1);

        camera.position.set(x, 0, z);
        camera.lookAt(pos);
    } else {
        // Установка позиции камеры
        camera.position.set(0, 150, 0);

        // Установка точки, на которую камера будет смотреть
        camera.lookAt(new THREE.Vector3(0, 0.0, 0));
    }
}
function keysAngle(delta) {
    if (keyboard.pressed("9")) {
        angle += Math.PI / 20;
    }

    if (keyboard.pressed("8")) {
        angle -= Math.PI / 90;
    }   
    keys(delta);
}
