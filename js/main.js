//импорт библиотеки three.js
import * as THREE from "./lib/three.module.js";
//импорт библиотек для загрузки моделей и материалов
import { MTLLoader } from "./lib/MTLLoader.js";
import { OBJLoader } from "./lib/OBJLoader.js";

// Ссылка на элемент веб страницы в котором будет отображаться графика
var container;
// Переменные "камера", "сцена" и "отрисовщик"
var camera, scene, renderer;

var N = 100;

var cursor;
var mouse = { x: 0, y: 0 };
var targetList = [];
var imagedata, geometry;

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
    camera.position.set(N / 2, N, N * 1.5);

    // Установка точки, на которую камера будет смотреть
    camera.lookAt(new THREE.Vector3(N / 2, 0.0, N / 6));
    // Создание отрисовщика
    renderer = new THREE.WebGLRenderer({ antialias: false });
    renderer.setSize(window.innerWidth, window.innerHeight);
    renderer.shadowMap.enabled = true;
    renderer.shadowMap.type = THREE.PCFShadowMap;
    renderer.setClearColor(0x001e1e1e, 1);
    container.appendChild(renderer.domElement);
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
}

function addLight() {
    const light = new THREE.PointLight(0xffffff);
    light.position.set(10, 300, 250); //позиция источника освещения
    // light.target = new THREE.Object3D();
    // light.target.position.set(N/2, 0, N/2)
    // scene.add(light.target);
    light.castShadow = true; //включение расчёта теней от источника освещения
    scene.add(light); //добавление источника освещения sв сцену

    //настройка расчёта теней от источника освещения
    light.shadow.mapSize.width = 1024; //ширина карты теней в пикселях
    light.shadow.mapSize.height = 1024; //высота карты теней в пикселях
    light.shadow.camera.near = 1; //расстояние, ближе которого не будет теней
    light.shadow.camera.far = 3000;

    var helper = new THREE.CameraHelper(light.shadow.camera);
    scene.add(helper);
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
    // Добавление функции на вызов, при перерисовки браузером страницы
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

function onDocumentMouseScroll(event) {}
function onDocumentMouseMove(event) {
    //определение позиции мыши
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
        console.log(intersects[0]);
        cursor.position.copy(intersects[0].point);
        cursor.position.y+=2.5;
    }
}
function onDocumentMouseDown(event) {}
function onDocumentMouseUp(event) {}

function addCursor() {
    //параметры цилиндра: диаметр вершины, диаметр основания, высота, число сегментов
    var geometry = new THREE.CylinderGeometry(1.5, 0, 5, 64);
    var cyMaterial = new THREE.MeshLambertMaterial({ color: 0x888888 });
    var cylinder = new THREE.Mesh(geometry, cyMaterial);
    scene.add(cylinder);

    return cylinder;
}
