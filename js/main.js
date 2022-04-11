//импорт библиотеки three.js
// import * as THREE from "";
import * as THREE from "three";

//импорт библиотек для загрузки моделей и материалов
import { MTLLoader } from "./lib/MTLLoader.js";
import { OBJLoader } from "./lib/OBJLoader.js";

import { OrbitControls } from "./lib/OrbitControls.js";

let model3D = [
    {
        name: "house",
        path: "js/model/House/",
        oname: "Cyprys_House.obj",
        pname: "Cyprys_House.mtl",
    },
    {
        name: "bush",
        path: "./model",
        oname: "./Bush/Bush1.obj",
        pname: "Bush/Bush1.mtl",
    },
    { name: "", path: "model/", oname: "", pname: "" },
];

// Ссылка на элемент веб страницы в котором будет отображаться графика
var container;
// Переменные "камера", "сцена" и "отрисовщик"
var camera, scene, renderer;

var controls;
var controlsState;

var keyboard = new THREEx.KeyboardState();

var clock = new THREE.Clock();

var N = 200;

var cursor;
var L = 32;

var circle;
var radius = 10;
var mouse = { x: 0, y: 0 };
var targetList = [];
var imagedata, geometry;
var brushState = true;
var cursorMode = true;

var stats = new Stats();
// Функция инициализации камеры, отрисовщика, объектов сцены и т.д.
init();
// Обновление данных по таймеру браузера
animate();

// В этой функции можно добавлять объекты и выполнять их первичную настройку
function init() {
    // Получение ссылки на элемент html страницы
    container = document.getElementById("container");

    stats.showPanel(0); // 0: fps, 1: ms, 2: mb, 3+: custom
    document.body.appendChild(stats.dom);
    // Создание "сцены"
    scene = new THREE.Scene();
    scene.fog = new THREE.FogExp2(0xcccccc, 0.002);

    // Установка параметров камеры
    // 45 - угол обзора
    // window.innerWidth / window.innerHeight - соотношение сторон
    // 1 - 4000 - ближняя и дальняя плоскости отсечения
    camera = new THREE.PerspectiveCamera(
        45,
        window.innerWidth / window.innerHeight,
        1,
        600
    );
    // Установка позиции камеры
    camera.position.set(N / 2, N, N * 1.5);

    // Установка точки, на которую камера будет смотреть
    camera.lookAt(new THREE.Vector3(N / 2, 0.0, N / 2));
    // Создание отрисовщика
    renderer = new THREE.WebGLRenderer({ antialias: true });
    renderer.setPixelRatio(window.devicePixelRatio);

    renderer.setSize(window.innerWidth, window.innerHeight);
    renderer.shadowMap.enabled = true;
    renderer.shadowMap.type = THREE.PCFShadowMap;
    renderer.setClearColor(0x001e1e1e, 1);
    container.appendChild(renderer.domElement);
    // controls

    controls = new OrbitControls(camera, renderer.domElement);
    controlsOn(true);
    //controls.listenToKeyEvents( window ); // optional

    controls.addEventListener("change", render); // call this only in static scenes (i.e., if there is no animation loop)

    controls.enableDamping = true; // an animation loop is required when either damping or auto-rotation are enabled
    controls.dampingFactor = 0.05;

    controls.screenSpacePanning = false;

    controls.minDistance = 10;
    controls.maxDistance = 1000;

    controls.maxPolarAngle = Math.PI / 2;

    // var effect = new THREE.AsciiEffect(renderer);
    // effect.setSize(window.innerWidth, window.innerHeight);
    // container.append(effect.domElement);
    // Добавление функции обработки события изменения размеров окна
    window.addEventListener("resize", onWindowResize, false);

    renderer.domElement.addEventListener(
        "mousedown",
        onDocumentMouseDown,
        false
    );
    renderer.domElement.addEventListener("mouseup", onDocumentMouseUp, false);
    renderer.domElement.addEventListener(
        "mousemove",
        onDocumentMouseMove,
        false
    );
    renderer.domElement.addEventListener("wheel", onDocumentMouseScroll, false);

    addLight();

    var canvas = document.createElement("canvas");
    var context = canvas.getContext("2d");
    var img = new Image();
    img.onload = function () {
        canvas.width = N;
        canvas.height = N;
        context.drawImage(img, 0, 0);
        imagedata = context.getImageData(0, 0, img.width, img.height);
        // Пользовательская функция генерации ландшафта
        terrain();
    };
    // Загрузка изображения с картой высот
    img.src = "img/plateau.jpg";

    cursor = addCursor();
    circle = addCircle(L);

    //объект интерфейса и его ширина
    var gui = new dat.GUI();
    gui.width = 200;
    //массив переменных, ассоциированных с интерфейсом
    var params = {
        sx: 0,
        sy: 0,
        sz: 0,
        brush: true,
        addHouse: function () {
            console.log(cursor);
            cursor = addMesh(
                model3D[0].path,
                model3D[0].oname,
                model3D[0].pname
            );
        },
        del: function () {
            delMesh();
        },
    };
    //создание вкладки
    var folder1 = gui.addFolder("Scale");
    //ассоциирование переменных отвечающих за масштабирование
    //в окне интерфейса они будут представлены в виде слайдера
    //минимальное значение - 1, максимальное – 100, шаг – 1
    //listen означает, что изменение переменных будет отслеживаться
    var meshSX = folder1.add(params, "sx").min(1).max(100).step(1).listen();
    var meshSY = folder1.add(params, "sy").min(1).max(100).step(1).listen();
    var meshSZ = folder1.add(params, "sz").min(1).max(100).step(1).listen();
    //при запуске программы папка будет открыта
    folder1.open();
    //описание действий совершаемых при изменении ассоциированных значений
    5;
    meshSX.onChange(function (value) {});
    meshSY.onChange(function (value) {});
    meshSZ.onChange(function (value) {});
    var ctrlPanel = gui.addFolder("Cursor Mode");
    //добавление чек бокса с именем brush
    var cubeVisible = ctrlPanel.add(params, "brush").name("brush").listen();
    cubeVisible.onChange(function (value) {
        cursorMode = value;
    });
    ctrlPanel.open();

    //добавление кнопок, при нажатии которых будут вызываться функции addMesh
    //и delMesh соответственно. Функции описываются самостоятельно.
    gui.add(params, "addHouse").name("add house");
    gui.add(params, "del").name("delete");

    //при запуске программы интерфейс будет раскрыт
    gui.open();
}

function controlsOn(state) {
    controls.enabled = state;
    controls.rotate = state;
}

function addLight() {
    const dirLight1 = new THREE.DirectionalLight(0xffffff);
    dirLight1.position.set(1, 1, 1);
    scene.add(dirLight1);

    const dirLight2 = new THREE.DirectionalLight(0x002288);
    dirLight2.position.set(-1, -1, -1);
    scene.add(dirLight2);

    const ambientLight = new THREE.AmbientLight(0x222222);
    scene.add(ambientLight);

    //настройка расчёта теней от источника освещения
    dirLight1.shadow.mapSize.width = 1024; //ширина карты теней в пикселях
    dirLight1.shadow.mapSize.height = 1024; //высота карты теней в пикселях
    dirLight1.shadow.camera.near = 1; //расстояние, ближе которого не будет теней
    dirLight1.shadow.camera.far = 3000;
    //настройка расчёта теней от источника освещения
    dirLight2.shadow.mapSize.width = 1024; //ширина карты теней в пикселях
    dirLight2.shadow.mapSize.height = 1024; //высота карты теней в пикселях
    dirLight2.shadow.camera.near = 1; //расстояние, ближе которого не будет теней
    dirLight2.shadow.camera.far = 3000;
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
    //console.log("CursorMode: " + cursorMode);
    if (cursorMode == false) {
        controlsOn(true);
        circle.material = new THREE.LineBasicMaterial({
            color: 0xff0000, //цвет линии
        });
    } else if (keyboard.pressed("shift")) 
    {
        controlsOn(true);
        circle.material = new THREE.LineBasicMaterial({
            color: 0xff0000, //цвет линии
        });
    }
    else {
        controlsOn(false);
        var d = 0; //если не определять, то он будет стирать поле
        if (whichButton == 1) d = 1;
        else if (whichButton == 3) d = -1;
        if (isPressed == true) hsphere(d, delta);
        circle.material = new THREE.LineBasicMaterial({
            color: 0xffff00, //цвет линии
        });
        
    }
    // Добавление функции на вызов, при перерисовки браузером страницы

    stats.begin();

    // monitored code goes here

    stats.end();

    requestAnimationFrame(animate);
    render();
}
function render() {
    // Рисование кадра
    renderer.render(scene, camera);
}

function getPixel(imagedata, x, y) {
    var position = (x + imagedata.width * y) * 4,
        data = imagedata.data;
    return data[position];
}

function terrain() {
    geometry = new THREE.BufferGeometry(); // Создание структуры для хранения геометрии
    var vertices = []; // Объявление массива для хранения вершин
    var faces = []; // Объявление массива для хранения индексов
    var uvs = []; // Массив для хранения текстурных координат

    for (var i = 0; i < N; i++)
        for (var j = 0; j < N; j++) {
            //получение цвета пикселя в десятом столбце десятой строки изображения
            var h = getPixel(imagedata, i, j);

            vertices.push(i, h / 5, j); // Добавление координат третьей вершины в массив вершин
            uvs.push(i / (N - 1), j / (N - 1)); // Добавление цвета для первой вершины (красный)
        }

    for (var i = 0; i < N - 1; i++)
        for (var j = 0; j < N - 1; j++) {
            faces.push(i + j * N, i + 1 + j * N, i + 1 + (j + 1) * N); // Добавление индексов (порядок соединения вершин) в массив индексов
            faces.push(i + j * N, i + 1 + (j + 1) * N, i + (j + 1) * N); // Добавление индексов (порядок соединения вершин) в массив индексов
        }

    //Добавление вершин и индексов в геометрию
    geometry.setAttribute(
        "position",
        new THREE.Float32BufferAttribute(vertices, 3)
    );
    geometry.setIndex(faces);

    //Добавление текстурных координат в геометрию
    geometry.setAttribute("uv", new THREE.Float32BufferAttribute(uvs, 2));

    geometry.computeVertexNormals();

    // Загрузка текстуры yachik.jpg из папки pics
    var tex = new THREE.TextureLoader().load("img/grasstile.jpg");

    var material = new THREE.MeshLambertMaterial({
        map: tex,
        wireframe: false,
        side: THREE.DoubleSide,
    });

    // Режим повторения текстуры
    tex.wrapS = tex.wrapT = THREE.RepeatWrapping;
    // Повторить текстуру 10х10 раз
    tex.repeat.set(3, 3);

    // Создание объекта и установка его в определённую позицию
    var mesh = new THREE.Mesh(geometry, material);
    mesh.position.set(0, 0, 0);

    // Добавление объекта в сцену
    mesh.receiveShadow = true;
    scene.add(mesh);

    targetList.push(mesh);
}

function onDocumentMouseScroll(event) {
    if (event.wheelDelta > 0) radius++;
    if (event.wheelDelta < 0) radius--;

    circle.scale.set(radius, 1, radius);
}
function onDocumentMouseMove(event) {
    mouse.x = (event.clientX / window.innerWidth) * 2 - 1;
    mouse.y = -(event.clientY / window.innerHeight) * 2 + 1;

    //создание луча, исходящего из позиции камеры и проходящего сквозь позицию курсора мыши
    var vector = new THREE.Vector3(mouse.x, mouse.y, 1);
    vector.unproject(camera);
    var ray = new THREE.Raycaster(
        camera.position,
        vector.sub(camera.position).normalize()
    );
    // создание массива для хранения объектов, с которыми пересечётся луч
    var intersects = ray.intersectObjects(targetList);

    // если луч пересёк какой-либо объект из списка targetList
    if (intersects.length > 0) {
        //печать списка полей объекта
        // console.log(cursor);

        cursor.position.copy(intersects[0].point);

        circle.position.copy(intersects[0].point);
        for (
            var i = 0;
            i < circle.geometry.attributes.position.array.length - 1;
            i += 3
        ) {
            //получение позиции в локальной системе координат
            var pos = new THREE.Vector3();

            pos.x = circle.geometry.attributes.position.array[i];
            pos.y = circle.geometry.attributes.position.array[i + 1];
            pos.z = circle.geometry.attributes.position.array[i + 2];
            //нахождение позиции в глобальной системе координат
            pos.applyMatrix4(circle.matrixWorld);

            var x = Math.round(pos.x);
            var z = Math.round(pos.z);

            var ind = (z + x * N) * 3;

            if (ind >= 0 && ind < geometry.attributes.position.array.length)
                circle.geometry.attributes.position.array[i + 1] =
                    geometry.attributes.position.array[ind + 1];
        }
        circle.geometry.attributes.position.needsUpdate = true;

        cursor.position.y += 2.5;
        circle.position.y = 0.2;
    }
}

var isPressed = false;
var whichButton; //1 - left, 2 - wheel 3 - right

function onDocumentMouseDown(event) {
    isPressed = true;
    if (event.which == 1) whichButton = 1;
    if (event.which == 2) whichButton = 2;
    if (event.which == 3) whichButton = 3;
}
function onDocumentMouseUp(event) {
    isPressed = false;
    if (event.which == 1) whichButton = 1;
    if (event.which == 2) whichButton = 2;
    if (event.which == 3) whichButton = 3;
}

function addCursor() {
    //параметры цилиндра: диаметр вершины, диаметр основания, высота, число сегментов
    var geometry = new THREE.CylinderGeometry(1.5, 0, 5, 64);
    var cyMaterial = new THREE.MeshLambertMaterial({ color: 0x888888 });
    var cylinder = new THREE.Mesh(geometry, cyMaterial);
    scene.add(cylinder);

    return cylinder;
}

function addCircle(l) {
    //создание материала для пунктирной линии

    var dashed_material = new THREE.LineBasicMaterial({
        color: 0xffff00, //цвет линии
    });

    var points = []; //массив для хранения координат сегментов

    var k = 360 / l;
    for (var i = 0; i < l; i++) {
        var x = Math.cos((k * i * Math.PI) / 180);
        var z = Math.sin((k * i * Math.PI) / 180);
        points.push(new THREE.Vector3(x, 0, z)); //начало линии
    }
    var geometry = new THREE.BufferGeometry().setFromPoints(points); //создание геометрии
    var line = new THREE.Line(geometry, dashed_material); //создание модели

    line.computeLineDistances(); //вычисление дистанции между сегментами
    line.scale.set(radius, 1, radius);
    scene.add(line); //добавление модели в сцену

    return line;
}

function hsphere(k, delta) {
    var pos = new THREE.Vector3();
    pos.copy(cursor.position);

    var vertices = geometry.getAttribute("position"); //получение массива вершин плоскости
    for (var i = 0; i < vertices.array.length; i += 3) {
        var x = vertices.array[i]; //получение координат вершин по X
        var z = vertices.array[i + 2]; //получение координат вершин по Z

        var h =
            radius * radius -
            ((x - pos.x) * (x - pos.x) + (z - pos.z) * (z - pos.z));

        if (h > 0) {
            vertices.array[i + 1] += Math.sqrt(h) * k * delta; //изменение координат по Y
        }
    }
    geometry.setAttribute("position", vertices); //установка изменённых вершин
    geometry.computeVertexNormals(); //пересчёт нормалей
    geometry.attributes.position.needsUpdate = true; //обновление вершин
    geometry.attributes.normal.needsUpdate = true; //обновление нормалей
}

function addMesh(path, oname, mname) {
    //где path – путь к папке с моделями
    const onProgress = function (xhr) {
        //выполняющаяся в процессе загрузки
        if (xhr.lengthComputable) {
            const percentComplete = (xhr.loaded / xhr.total) * 100;
            console.log(Math.round(percentComplete, 2) + "% downloaded");
        }
    };
    const onError = function () {}; //выполняется в случае возникновения ошибки
    const manager = new THREE.LoadingManager();
    new MTLLoader(manager)
        .setPath(path) //путь до модели
        .load(mname, function (materials) {
            //название материала
            materials.preload();
            new OBJLoader(manager)
                .setMaterials(materials) //установка материала
                .setPath(path) //путь до модели
                .load(
                    oname,
                    function (object) {
                        object.traverse(function (child) {
                            if (child instanceof THREE.Mesh) {
                                child.castShadow = true;
                            }
                        });

                        var X = Math.random();
                        var Z = Math.random();
                        object.position.x = X;
                        object.position.z = Z;
                        var h = getPixel(
                            imagedata,
                            Math.round(X),
                            Math.round(Z)
                        );
                        object.position.y = h / 5;

                        object.rotation.y = Math.PI * 8;
                        var s = Math.random() * 100 + 100;
                        s /= N;
                        object.scale.set(s, s, s);

                        scene.add(object.clone());

                        console.log();
                        return object.clone();
                    },
                    onProgress,
                    onError
                );
        });
}
