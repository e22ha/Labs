//импорт библиотеки three.js
// import * as THREE from "";
import * as THREE from "three";

//импорт библиотек для загрузки моделей и материалов
import { MTLLoader } from "./lib/MTLLoader.js";
import { OBJLoader } from "./lib/OBJLoader.js";

import { OrbitControls } from "./lib/OrbitControls.js";

let ListModel = [
    // {
    //     name: "house",
    //     path: "js/model/House/",
    //     oname: "Cyprys_House.obj",
    //     pname: "Cyprys_House.mtl",
    // },
    {
        name: "bush",
        path: "js/model/Bush/",
        oname: "Bush1.obj",
        pname: "Bush1.mtl",
    },
    {
        name: "grade",
        path: "js/model/Grade/",
        oname: "grade.obj",
        pname: "grade.mtl",
    },
];

let loadedModels = new Map();

// Ссылка на элемент веб страницы в котором будет отображаться графика
var container;
// Переменные "камера", "сцена" и "отрисовщик"
var camera, scene, renderer;

var controls;

var keyboard = new THREEx.KeyboardState();

var clock = new THREE.Clock();
var ringLoad;
var N = 200;

var cursor;
var L = 32;

var circle;
var radius = 10;
var mouse = { x: 0, y: 0 };

var targetList = [];
var objectlist = [];

var imagedata, geometry;
var brushMode = true;
var toolMode = false;
var orbitMode = false;

var selected = null;

var stats = new Stats();
// Функция инициализации камеры, отрисовщика, объектов сцены и т.д.
init();
// Обновление данных по таймеру браузера

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
    controlsOn(false);
    //controls.listenToKeyEvents( window ); // optional

    controls.addEventListener("change", render); // call this only in static scenes (i.e., if there is no animation loop)

    controls.enableDamping = true; // an animation loop is required when either damping or auto-rotation are enabled
    controls.dampingFactor = 0.05;

    controls.screenSpacePanning = false;

    controls.minDistance = 10;
    controls.maxDistance = 1000;

    controls.maxPolarAngle = Math.PI / 2;

    // Установка позиции камеры
    camera.position.set(1.5 * N, N * 0.8, N / 2);

    // Установка точки, на которую камера будет смотреть
    camera.lookAt(new THREE.Vector3(N / 2, 0, N / 2));

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

    // const geometry = new THREE.TorusKnotGeometry(10, 3, 100, 16);
    // const material = new THREE.MeshBasicMaterial({ color: 0xf00f00 });
    // ringLoad = new THREE.Mesh(geometry, material);
    // ringLoad.castShadow = true;
    // ringLoad.receiveShadow = true;
    // ringLoad.position.set(N/2, 0, N/2);
    // scene.add(ringLoad);

    for (let i = 0; i < ListModel.length; i++) {
        // loadedModels.push(
        //   [
        addMesh(
            ListModel[i].name,
            ListModel[i].path,
            ListModel[i].oname,
            ListModel[i].pname
        );
        //]);
    }
}

function loadScene() {
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
    gui();

    animate();
}

function gui() {
    var gui = new dat.GUI();
    gui.width = 200;
    //массив переменных, ассоциированных с интерфейсом
    var params = {
        rx: 0,
        ry: 0,
        rz: 0,
        brush: true,
        tool: false,
        orbit: false,
        addHouse: function () {
            addObj("house");
        },
        addBush: function () {
            addObj("bush");
        },
        addGrade: function () {
            addObj("grade");
        },
        del: function () {
            if (selected.userData.cube != null) {
                //поиск индекса эллемента link в массиве draworder
                var ind = objectlist.indexOf(selected.userData.cube);
                //если такой индекс существует, удаление одного эллемента из массива
                if (~ind) objectlist.splice(ind, 1);
                //удаление из сцены объекта, на который ссылается link
                scene.remove(selected.userData.cube.userData.model);
                scene.remove(selected.userData.cube);

                console.log("O: " + objectlist);
            }
        },
    };
    //создание вкладки
    var folder1 = gui.addFolder("Rotate");
    //ассоциирование переменных отвечающих за масштабирование
    //в окне интерфейса они будут представлены в виде слайдера
    //минимальное значение - 1, максимальное – 100, шаг – 1
    //listen означает, что изменение переменных будет отслеживаться
    var meshRX = folder1.add(params, "rx").min(1).max(360).step(1).listen();
    var meshRY = folder1.add(params, "ry").min(1).max(360).step(1).listen();
    var meshRZ = folder1.add(params, "rz").min(1).max(360).step(1).listen();
    //при запуске программы папка будет открыта
    folder1.open();
    //описание действий совершаемых при изменении ассоциированных значений
    5;
    meshRX.onChange(function (value) {
        if (selected != null) {
            var ang = new THREE.Vector3();
            ang.copy(selected.userData.cube.userData.model.rotation);
            
            selected.userData.cube.userData.model.rotation.x =
                (value * Math.PI) / 180;
            
            selected.userData.cube.rotation.x =
                selected.userData.cube.userData.model.rotation.x;

            selected.userData.bbox.setFromObject(selected);
            selected.userData.bbox.getCenter(selected.userData.cube.position);
            
            selected.userData.bbox.getCenter(selected.userData.obb.position);
            selected.userData.obb.basis.extractRotation(selected.matrixWorld);
            
            rerotate(ang);
        }
    });
    meshRY.onChange(function (value) {
        if (selected != null) {
            var ang = new THREE.Vector3();
            ang.copy(selected.userData.cube.userData.model.rotation);
            selected.userData.cube.userData.model.rotation.y =
            (value * Math.PI) / 180;
            selected.userData.cube.rotation.y =
            selected.userData.cube.userData.model.rotation.y;
            selected.userData.bbox.setFromObject(selected);
            selected.userData.obb.setFromObject(selected);
            selected.userData.obb.basis.extractRotation(selected.matrixWorld);
            selected.userData.bbox.getCenter(selected.userData.cube.position);
            selected.userData.obb.getCenter(selected.userData.cube.position);
            rerotate(ang);
        }
    });
    meshRZ.onChange(function (value) {
        if (selected != null) {
            var ang = new THREE.Vector3();
            ang.copy(selected.userData.cube.rotation);
            selected.userData.cube.userData.model.rotation.z =
            (value * Math.PI) / 180;
            selected.userData.cube.rotation.z =
            selected.userData.cube.userData.model.rotation.z;
            selected.userData.bbox.setFromObject(selected);
            selected.userData.obb.setFromObject(selected);
            selected.userData.obb.basis.extractRotation(selected.matrixWorld);
            selected.userData.bbox.getCenter(selected.userData.cube.position);
            selected.userData.obb.getCenter(selected.userData.cube.position);
            rerotate(ang);
        }
    });
    var ctrlPanel = gui.addFolder("Cursor Mode");
    //добавление чек бокса с именем brush
    var cubeVisible = ctrlPanel.add(params, "brush").name("brush").listen();
    cubeVisible.onChange(function (value) {
        brushMode = value;
        params.tool = false;
        toolMode = false;
        params.orbit = false;
        orbitMode = false;

        curVis(brushMode);
        controlsOn(orbitMode);
    });
    //добавление чек бокса с именем brush
    var cubeVisible_1 = ctrlPanel.add(params, "tool").name("tool").listen();
    cubeVisible_1.onChange(function (value) {
        toolMode = value;
        params.brush = false;
        brushMode = false;
        params.orbit = false;
        orbitMode = false;

        curVis(brushMode);
        controlsOn(orbitMode);
    });
    //добавление чек бокса с именем orbit
    var cubeVisible_2 = ctrlPanel.add(params, "orbit").name("orbit").listen();
    cubeVisible_2.onChange(function (value) {
        orbitMode = value;
        params.brush = false;
        brushMode = false;
        params.tool = false;
        toolMode = false;

        console.log("bMode: " + brushMode);
        console.log("tMode: " + toolMode);
        console.log("oMode: " + orbitMode);

        controlsOn(orbitMode);
        curVis(brushMode);
    });
    ctrlPanel.open();

    //добавление кнопок, при нажатии которых будут вызываться функции addMesh
    //и delMesh соответственно. Функции описываются самостоятельно.
    gui.add(params, "addHouse").name("add house");
    gui.add(params, "addBush").name("add bush");
    gui.add(params, "addGrade").name("add grade");
    gui.add(params, "del").name("delete");

    //при запуске программы интерфейс будет раскрыт
    gui.open();
}

function rerotate(oldAng){
    for (var i = 0; i < objectlist.length; i++) {
        if (objectlist[i].userData.model != selected) {
            objectlist[i].userData.model.userData.cube.material.visible = false;
            intr = intersect(
                selected.userData,
                objectlist[i].userData.model.userData
            );

            //объект пересечение с которым было обнаружено
            //становится видимым
            if (intr) {
                objectlist[i].userData.model.userData.cube.material.visible = true;
                selected.userData.cube.userData.model.rotation.copy(oldAng);
                selected.userData.bbox.setFromObject(selected);
                selected.userData.obb.setFromObject(selected);
                selected.userData.obb.basis.extractRotation(selected.matrixWorld);
                selected.userData.bbox.getCenter(selected.userData.cube.position);
                selected.userData.obb.getCenter(selected.userData.cube.position);
                break;
            } 
        }
    }
}

function addObj(type) {
    var object = loadedModels.get(type).clone();
    var X = Math.random() * N;
    var Z = Math.random() * N;
    object.position.x = X;
    object.position.z = Z;
    var h = getPixel(imagedata, Math.round(X), Math.round(Z));
    object.position.y = h / 5;

    object.rotation.y = Math.PI * 8;
    var s = Math.random() * 100 + 100;
    s /= N;
    if (type == "bush") s = (Math.random() * 100 + 1000) / N;
    object.scale.set(s, s, s);

    var model = object.clone();

    scene.add(model);

    //создание объекта Box3 и установка его вокруг объекта object
    model.userData.bbox = new THREE.Box3();
    model.userData.bbox.setFromObject(model);
    //создание куба единичного размера
    var geometry = new THREE.BoxGeometry(1, 1, 1);
    var material = new THREE.MeshBasicMaterial({
        color: 0x00ff00,
        wireframe: true,
    });

    var obb = {};
    //структура состоит из матрицы поворота, позиции и половины размера
    obb.basis = new THREE.Matrix4();
    obb.halfSize = new THREE.Vector3();
    obb.position = new THREE.Vector3();

    model.userData.cube = new THREE.Mesh(geometry, material);

    //получение позиции центра объекта
    var pos = new THREE.Vector3();
    model.userData.bbox.getCenter(pos);

    //получение размеров объекта
    var size = new THREE.Vector3();
    model.userData.bbox.getSize(size);
    //установка позиции и размера объекта в куб
    model.userData.cube.position.copy(pos);
    model.userData.cube.scale.set(size.x, size.y, size.z);

    model.userData.cube.material.visible = false;

    model.userData.cube.userData.model = model;

    //получение позиции центра объекта
    model.userData.bbox.getCenter(obb.position);
    //получение размеров объекта
    model.userData.bbox.getSize(obb.halfSize).multiplyScalar(0.5);
    //получение матрицы поворота объекта
    obb.basis.extractRotation(model.matrixWorld);
    //структура хранится в поле userData объекта
    model.userData.obb = obb;

    scene.add(model.userData.cube);
    objectlist.push(model.userData.cube);
}

function controlsOn(state) {
    controls.enabled = state;
    controls.rotate = state;
}

function curVis(state) {
    if (circle != null)
        // circle.material = new THREE.LineBasicMaterial({
        //     color: 0xff0000, //цвет линии
        // });

        circle.visible = state;
    cursor.visible = state;
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
    //console.log("CursorMode: brush;
    if (brushMode) {
        var d = 0; //если не определять, то он будет стирать поле
        if (whichButton == 1) d = 1;
        else if (whichButton == 3) d = -1;
        if (isPressed == true) {
            hsphere(d, delta);
            // targetList.forEach(element => {

            //         var h = getPixel(imagedata, cursor.position.x, cursor.position.z);
            //         element.position.y = h / 5;

            // });
        }
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
    if (brushMode) {
        if (event.wheelDelta > 0) radius++;
        if (event.wheelDelta < 0) radius--;

        circle.scale.set(radius, 1, radius);
    }
}





var intr;

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

    if (brushMode) {
        if (intersects.length > 0) {
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

    if (toolMode) {
        if (intersects.length > 0)
            if (selected != null)
                if (isPressed) {
                    var oldPos = new THREE.Vector3();
                    oldPos.copy(selected.position);

                    selected.position.copy(intersects[0].point);
                                selected.userData.bbox.setFromObject(selected);
                                selected.userData.bbox.getCenter(
                                    selected.userData.cube.position
                                );

                                selected.userData.bbox.getCenter(
                                    selected.userData.obb.position
                                );
                    //перебор всех OBB объектов сцены
                    for (var i = 0; i < objectlist.length; i++) {
                        if (objectlist[i].userData.model != selected) {
                            objectlist[i].userData.model.userData.cube.material.visible = false;
                            intr = intersect(
                                selected.userData,
                                objectlist[i].userData.model.userData
                            );

                            //объект пересечение с которым было обнаружено
                            //становится видимым
                            if (intr) {
                                objectlist[i].userData.model.userData.cube.material.visible = true;
                                selected.position.copy(oldPos);
                                selected.userData.bbox.setFromObject(selected);
                                selected.userData.bbox.getCenter(selected.userData.cube.position);
                                selected.userData.bbox.getCenter( selected.userData.obb.position);
                                break;
                            } 
                        }
                    }
                }
    }
}

var isPressed = false;
var whichButton; //1 - left, 2 - wheel 3 - right

function onDocumentMouseDown(event) {
    isPressed = true;
    if (brushMode) {
        if (event.which == 1) whichButton = 1;
        if (event.which == 2) whichButton = 2;
        if (event.which == 3) whichButton = 3;
    }

    if (toolMode) {
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
        var intersects = ray.intersectObjects(objectlist, true);

        if (intersects.length > 0) {
            if (selected) selected.userData.cube.material.visible = false;
            selected = intersects[0].object.userData.model;
            selected.userData.cube.material.visible = true;
        } else if (intersects.length == 0) {
            if (selected) selected.userData.cube.material.visible = false;
            selected = null;
        }
    }
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

    for (var i = 0; i < objectlist.length; i++) {
        var x = objectlist[i].userData.model.position.x; //получение координат объекта по X
        var z = objectlist[i].userData.model.position.z; //получение координат объекта по Z

        var h =
            radius * radius -
            ((x - pos.x) * (x - pos.x) + (z - pos.z) * (z - pos.z));
        if (h > 0) {
            objectlist[i].userData.model.position.y += Math.sqrt(h) * k * delta;
            objectlist[i].position.y += Math.sqrt(h) * k * delta;
            // console.log("h in ol:  " + Math.sqrt(h));
        }
    }

    geometry.setAttribute("position", vertices); //установка изменённых вершин
    geometry.computeVertexNormals(); //пересчёт нормалей
    geometry.attributes.position.needsUpdate = true; //обновление вершин
    geometry.attributes.normal.needsUpdate = true; //обновление нормалей
}
var I = 0; //count model
function addMesh(name, path, oname, mname) {
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
                                child.parent = object;
                            }
                        });

                        object.parent = object;

                        object.position.x = 0;
                        object.position.z = 0;

                        object.position.y = 0;

                        object.rotation.y = Math.PI * 8;
                        var s = Math.random() * 100 + 100;
                        s /= N;
                        object.scale.set(s, s, s);
                        loadedModels.set(name, object);
                        console.log(loadedModels);
                        I++;
                        if (I >= ListModel.length) loadScene();
                        console.log(I);

                        //return object.clone();
                    },
                    onProgress,
                    onError
                );
        });
}

//оригинал алгоритма и реализацию класса OBB можно найти по ссылке:
//https://github.com/Mugen87/yume/blob/master/src/javascript/engine/etc/OBB.js
function intersect(ob1, ob2) {
    var xAxisA = new THREE.Vector3();
    var yAxisA = new THREE.Vector3();
    var zAxisA = new THREE.Vector3();
    var xAxisB = new THREE.Vector3();
    var yAxisB = new THREE.Vector3();

    var zAxisB = new THREE.Vector3();
    var translation = new THREE.Vector3();
    var vector = new THREE.Vector3();

    var axisA = [];
    var axisB = [];
    var rotationMatrix = [[], [], []];
    var rotationMatrixAbs = [[], [], []];
    var _EPSILON = 1e-3;

    var halfSizeA, halfSizeB;
    var t, i;

    ob1.obb.basis.extractBasis(xAxisA, yAxisA, zAxisA);
    ob2.obb.basis.extractBasis(xAxisB, yAxisB, zAxisB);

    // push basis vectors into arrays, so you can access them via indices
    axisA.push(xAxisA, yAxisA, zAxisA);
    axisB.push(xAxisB, yAxisB, zAxisB);
    // get displacement vector
    vector.subVectors(ob2.obb.position, ob1.obb.position);
    // express the translation vector in the coordinate frame of the current
    // OBB (this)
    for (i = 0; i < 3; i++) {
        translation.setComponent(i, vector.dot(axisA[i]));
    }
    // generate a rotation matrix that transforms from world space to the
    // OBB's coordinate space
    for (i = 0; i < 3; i++) {
        for (var j = 0; j < 3; j++) {
            rotationMatrix[i][j] = axisA[i].dot(axisB[j]);
            rotationMatrixAbs[i][j] = Math.abs(rotationMatrix[i][j]) + _EPSILON;
        }
    }
    // test the three major axes of this OBB
    for (i = 0; i < 3; i++) {
        vector.set(
            rotationMatrixAbs[i][0],
            rotationMatrixAbs[i][1],
            rotationMatrixAbs[i][2]
        );
        halfSizeA = ob1.obb.halfSize.getComponent(i);
        halfSizeB = ob2.obb.halfSize.dot(vector);

        if (Math.abs(translation.getComponent(i)) > halfSizeA + halfSizeB) {
            return false;
        }
    }
    // test the three major axes of other OBB
    for (i = 0; i < 3; i++) {
        vector.set(
            rotationMatrixAbs[0][i],
            rotationMatrixAbs[1][i],
            rotationMatrixAbs[2][i]
        );
        halfSizeA = ob1.obb.halfSize.dot(vector);
        halfSizeB = ob2.obb.halfSize.getComponent(i);
        vector.set(
            rotationMatrix[0][i],
            rotationMatrix[1][i],
            rotationMatrix[2][i]
        );
        t = translation.dot(vector);
        if (Math.abs(t) > halfSizeA + halfSizeB) {
            return false;
        }
    }
    // test the 9 different cross-axes
    // A.x <cross> B.x
    halfSizeA =
        ob1.obb.halfSize.y * rotationMatrixAbs[2][0] +
        ob1.obb.halfSize.z * rotationMatrixAbs[1][0];
    halfSizeB =
        ob2.obb.halfSize.y * rotationMatrixAbs[0][2] +
        ob2.obb.halfSize.z * rotationMatrixAbs[0][1];
    t =
        translation.z * rotationMatrix[1][0] -
        translation.y * rotationMatrix[2][0];
    if (Math.abs(t) > halfSizeA + halfSizeB) {
        return false;
    }
    // A.x < cross> B.y
    halfSizeA =
        ob1.obb.halfSize.y * rotationMatrixAbs[2][1] +
        ob1.obb.halfSize.z * rotationMatrixAbs[1][1];
    halfSizeB =
        ob2.obb.halfSize.x * rotationMatrixAbs[0][2] +
        ob2.obb.halfSize.z * rotationMatrixAbs[0][0];
    t =
        translation.z * rotationMatrix[1][1] -
        translation.y * rotationMatrix[2][1];
    if (Math.abs(t) > halfSizeA + halfSizeB) {
        return false;
    }

    // A.x <cross> B.z
    halfSizeA =
        ob1.obb.halfSize.y * rotationMatrixAbs[2][2] +
        ob1.obb.halfSize.z * rotationMatrixAbs[1][2];
    halfSizeB =
        ob2.obb.halfSize.x * rotationMatrixAbs[0][1] +
        ob2.obb.halfSize.y * rotationMatrixAbs[0][0];
    t =
        translation.z * rotationMatrix[1][2] -
        translation.y * rotationMatrix[2][2];
    if (Math.abs(t) > halfSizeA + halfSizeB) {
        return false;
    }
    // A.y <cross> B.x
    halfSizeA =
        ob1.obb.halfSize.x * rotationMatrixAbs[2][0] +
        ob1.obb.halfSize.z * rotationMatrixAbs[0][0];
    halfSizeB =
        ob2.obb.halfSize.y * rotationMatrixAbs[1][2] +
        ob2.obb.halfSize.z * rotationMatrixAbs[1][1];
    t =
        translation.x * rotationMatrix[2][0] -
        translation.z * rotationMatrix[0][0];
    if (Math.abs(t) > halfSizeA + halfSizeB) {
        return false;
    }
    // A.y <cross> B.y
    halfSizeA =
        ob1.obb.halfSize.x * rotationMatrixAbs[2][1] +
        ob1.obb.halfSize.z * rotationMatrixAbs[0][1];
    halfSizeB =
        ob2.obb.halfSize.x * rotationMatrixAbs[1][2] +
        ob2.obb.halfSize.z * rotationMatrixAbs[1][0];
    t =
        translation.x * rotationMatrix[2][1] -
        translation.z * rotationMatrix[0][1];
    if (Math.abs(t) > halfSizeA + halfSizeB) {
        return false;
    }
    // A.y <cross> B.z
    halfSizeA =
        ob1.obb.halfSize.x * rotationMatrixAbs[2][2] +
        ob1.obb.halfSize.z * rotationMatrixAbs[0][2];
    halfSizeB =
        ob2.obb.halfSize.x * rotationMatrixAbs[1][1] +
        ob2.obb.halfSize.y * rotationMatrixAbs[1][0];
    t =
        translation.x * rotationMatrix[2][2] -
        translation.z * rotationMatrix[0][2];
    if (Math.abs(t) > halfSizeA + halfSizeB) {
        return false;
    }
    // A.z <cross> B.x
    halfSizeA =
        ob1.obb.halfSize.x * rotationMatrixAbs[1][0] +
        ob1.obb.halfSize.y * rotationMatrixAbs[0][0];
    halfSizeB =
        ob2.obb.halfSize.y * rotationMatrixAbs[2][2] +
        ob2.obb.halfSize.z * rotationMatrixAbs[2][1];
    t =
        translation.y * rotationMatrix[0][0] -
        translation.x * rotationMatrix[1][0];
    if (Math.abs(t) > halfSizeA + halfSizeB) {
        return false;
    }
    // A.z <cross> B.y
    halfSizeA =
        ob1.obb.halfSize.x * rotationMatrixAbs[1][1] +
        ob1.obb.halfSize.y * rotationMatrixAbs[0][1];
    halfSizeB =
        ob2.obb.halfSize.x * rotationMatrixAbs[2][2] +
        ob2.obb.halfSize.z * rotationMatrixAbs[2][0];
    t =
        translation.y * rotationMatrix[0][1] -
        translation.x * rotationMatrix[1][1];
    if (Math.abs(t) > halfSizeA + halfSizeB) {
        return false;
    }
    // A.z <cross> B.z
    halfSizeA =
        ob1.obb.halfSize.x * rotationMatrixAbs[1][2] +
        ob1.obb.halfSize.y * rotationMatrixAbs[0][2];
    halfSizeB =
        ob2.obb.halfSize.x * rotationMatrixAbs[2][1] +
        ob2.obb.halfSize.y * rotationMatrixAbs[2][0];
    t =
        translation.y * rotationMatrix[0][2] -
        translation.x * rotationMatrix[1][2];
    if (Math.abs(t) > halfSizeA + halfSizeB) {
        return false;
    }
    // no separating axis exists, so the two OBB don't intersect
    return true;
}
