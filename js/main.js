//импорт библиотеки three.js
import * as THREE from "./lib/three.module.js";
//импорт библиотек для загрузки моделей и материалов
import { MTLLoader } from "./lib/MTLLoader.js";
import { OBJLoader } from "./lib/OBJLoader.js";

import { GLTFLoader } from "./lib/GLTFLoader.js";

// Ссылка на элемент веб страницы в котором будет отображаться графика
var container;
// Переменные "камера", "сцена" и "отрисовщик"
var camera, scene, renderer;

var N = 500;
var a = 0.0;
var imagedata;

var clock = new THREE.Clock();

var keyboard = new THREEx.KeyboardState();

var chase = 0;

var axisY = new THREE.Vector3(0, 1, 0);

var route;

var geometry;

var mixer,
    morphs = [];
var routes = [];

var controllObject;

mixer = new THREE.AnimationMixer(scene);

var oligth = new THREE.PointLight(0xffffff, 2, 150, 2);

// Функция инициализации камеры, отрисовщика, объектов сцены и т.д.
init();
// Обновление данных по таймеру браузера
animate();

// В этой функции можно добавлять объекты и выполнять их первичную настройку
function init() {
    // Получение ссылки на элемент html страницы
    container = document.getElementById("container");
    scene = new THREE.Scene();

    defaultCam();

    renderer = new THREE.WebGLRenderer({ antialias: false });
    renderer.setSize(window.innerWidth, window.innerHeight);
    // Закрашивание экрана синим цветом, заданным в 16ричной системе
    renderer.setClearColor(0x995599ff, 1);
    renderer.shadowMap.enabled = true;
    renderer.shadowMap.type = THREE.PCFShadowMap;
    container.appendChild(renderer.domElement);
    // Добавление функции обработки события изменения размеров окна
    window.addEventListener("resize", onWindowResize, false);

    addLight();

    var canvas = document.createElement("canvas");
    var context = canvas.getContext("2d");
    var img = new Image();
    img.onload = function () {
        canvas.width = 500;
        canvas.height = 500;
        context.drawImage(img, 0, 0);
        imagedata = context.getImageData(0, 0, img.width, img.height);
        // Пользовательская функция генерации ландшафта
        terrain();
    };
    // Загрузка изображения с картой высот
    img.src = "img/plateau.jpg";

    // вызов функции загрузки модели (в функции Init)
    loadModel("models/", "Tree.obj", "Tree.mtl");

    route = curveCreator();

    routes.push(route);

    loadAnimatedModel("models/Parrot.glb", false);
    controllObject = loadAnimatedModel("models/Horse.glb", true);
}

function defaultCam() {
    camera = new THREE.PerspectiveCamera(
        45,
        window.innerWidth / window.innerHeight,
        1,
        4000
    );
    camera.position.set(N / 2, 250, N + 300);
    camera.lookAt(new THREE.Vector3(N / 2, 0.0, 100));
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

    scene.add(oligth);

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

function getPixel(imagedata, x, y) {
    var position = (x + imagedata.width * y) * 4,
        data = imagedata.data;
    return data[position];
}

var T = 8.0;
var t = 0.0;
var folowBird = false;
var folowControlObject = false;
var speed = 40;

// В этой функции можно изменять параметры объектов и обрабатывать действия пользователя
function animate() {
    // воспроизведение анимаций (в функции animate)
    keys();

    switch (chase) {
        case 0:
            defaultCam();
            folowBird = false;
            folowControlObject = false;
            break;
        case 1:
            folowBird = true;
            folowControlObject = false;
            break;
            break;
        case 2:
            folowBird = false;
            folowControlObject = true;
            break;
        default:
            break;
    }
    
    var delta = clock.getDelta();
    t += delta;

    mixer.update(delta);
    for (var i = 0; i < morphs.length; i++) {
        var morph = morphs[i];
        var r = routes[i];
        if (r != null) {
            if (t + 0.001 >= T) t = 0.0;

            var pos = new THREE.Vector3();
            pos.copy(r.getPointAt(t / T));

            morph.position.copy(pos);

            var nextpoint = new THREE.Vector3();
            nextpoint.copy(r.getPointAt((t + 0.001) / T));

            oligth.position.copy(pos);

            morph.lookAt(nextpoint);

            if (folowBird == true) {
                // установка смещения камеры относительно объекта
                var relativeCameraOffset = new THREE.Vector3(0, 30, -100);
                var m1 = new THREE.Matrix4();
                var m2 = new THREE.Matrix4();
                // получение поворота объекта
                m1.extractRotation(morph.matrixWorld);
                // получение позиции объекта
                m2.extractPosition(morph.matrixWorld);
                m1.multiplyMatrices(m2, m1);
                // получение смещения позиции камеры относительно объекта
                var cameraOffset = relativeCameraOffset.applyMatrix4(m1);
                // установка позиции и направления взгляда камеры
                camera.position.copy(cameraOffset);
                camera.lookAt(morph.position);
            }
        }
        if (controllObject != null && folowControlObject == true) {
            // установка смещения камеры относительно объекта
            var relativeCameraOffset = new THREE.Vector3(0, 80, -160);
            var m1 = new THREE.Matrix4();
            var m2 = new THREE.Matrix4();
            // получение поворота объекта
            m1.extractRotation(controllObject.matrixWorld);
            // получение позиции объекта
            m2.extractPosition(controllObject.matrixWorld);
            m1.multiplyMatrices(m2, m1);
            // получение смещения позиции камеры относительно объекта
            var cameraOffset = relativeCameraOffset.applyMatrix4(m1);
            // установка позиции и направления взгляда камеры
            camera.position.copy(cameraOffset);
            camera.lookAt(controllObject.position);
            
            if (keyboard.pressed("a") || keyboard.pressed("ф")) {
                controllObject.rotateOnAxis(axisY, Math.PI / 90.0);
            }
            if (keyboard.pressed("d") || keyboard.pressed("в")) {
                controllObject.rotateOnAxis(axisY, -Math.PI / 90.0);
            }

            if (keyboard.pressed("w") || keyboard.pressed("ц")) {
                controllObject.translateZ((speed+100) * delta);
                var h = getPixel(imagedata,Math.round(controllObject.position.x),Math.round(controllObject.position.z));
                controllObject.position.y = h/5;
            }
            else if (keyboard.pressed("s") || keyboard.pressed("ы")) {
                controllObject.translateZ((speed-100) * delta);
                var h = getPixel(imagedata,Math.round(controllObject.position.x),Math.round(controllObject.position.z));
                controllObject.position.y = h/5;
            }
            else{
                controllObject.translateZ(speed * delta);
                var h = getPixel(imagedata,Math.round(controllObject.position.x),Math.round(controllObject.position.z));
                controllObject.position.y = h/5;
            }

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

function loadModel(path, oname, mname) {
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

                        for (var i = 0; i < 15; i++) {
                            var X = Math.random() * N;
                            var Z = Math.random() * N;
                            object.position.x = X;
                            object.position.z = Z;
                            var h = getPixel(imagedata,Math.round(X),Math.round(Z));
                            object.position.y = h/5;

                            object.rotation.y = Math.PI * 8;
                            var s = Math.random() * 100 + 100;
                            s /= N;
                            object.scale.set(s, s, s);

                            scene.add(object.clone());
                        }
                    },
                    onProgress,
                    onError
                );
        });
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
}

function loadAnimatedModel(path, controlled) {
    //где path – путь и название модели
    var loader = new GLTFLoader();
    loader.load(path, function (gltf) {
        var mesh = gltf.scene.children[0];
        var clip = gltf.animations[0];
        //установка параметров анимации (скорость воспроизведения и стартовый фрейм)

        for (var i = 0; i < 1; i++) {
            mesh.position.set(1 + Math.random() * N, 10, 1 + Math.random() * N);
            mesh.rotation.y = Math.PI / 8;
            var s = Math.random() * 100 + 30;
            mesh.scale.set(0.2, 0.2, 0.2);

            mesh.castShadow = true;

            var clone = mesh.clone();
            mixer.clipAction(clip, clone).setDuration(1).startAt(0).play();

            scene.add(clone);

            if (controlled == false) morphs.push(clone);
            else{
                controllObject = clone;
                var h = getPixel(imagedata,Math.round(controllObject.position.x),Math.round(controllObject.position.z));
                controllObject.position.y = h/5;
            }
        }
    });
}

function curveCreator() {
    var curve1 = new THREE.CubicBezierCurve3(
        new THREE.Vector3(100, 150, 256), //P0
        new THREE.Vector3(100, 140, 0), //P1
        new THREE.Vector3(400, 60, 0), //P2
        new THREE.Vector3(400, 50, 256) //P3
    );
    var vertices = [];
    var curve2 = new THREE.CubicBezierCurve3(
        new THREE.Vector3(400, 50, 266), //P3
        new THREE.Vector3(400, 50, 512), //P2
        new THREE.Vector3(100, 140, 512), //P1
        new THREE.Vector3(100, 150, 266) //P0
    );

    vertices = curve1.getPoints(100);
    vertices = vertices.concat(curve2.getPoints(100));

    // создание кривой по списку точек
    var path = new THREE.CatmullRomCurve3(vertices);
    // является ли кривая замкнутой (зацикленной)
    path.closed = true;

    //создание геометрии из точек кривой
    var geometry = new THREE.BufferGeometry().setFromPoints(vertices);
    var material = new THREE.LineBasicMaterial({ color: 0xaaff00 });

    //создание объекта
    var curveObject = new THREE.Line(geometry, material);
    scene.add(curveObject); //добавление объекта в сцену
    return path;
}

function keys() {
    if (keyboard.pressed("0")) {
        chase = 0;
    }

    if (keyboard.pressed("1")) {
        chase = 1;
    }
    if (keyboard.pressed("2")) {
        chase = 2;
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
