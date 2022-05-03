// Ссылка на элемент веб страницы в котором будет отображаться графика
var container;
// Переменные "камера", "сцена" и "отрисовщик"
var camera, scene, renderer;
var cameraOrtho, sceneOrtho;

var sw = window.innerWidth;
var sh = window.innerHeight;
var keyboard = new THREEx.KeyboardState();

var a;
var chase = -1;

var angle = Math.PI / 2;

var planets = [];

var clock = new THREE.Clock();

const loader = new THREE.TextureLoader();


let desc = [
    "imgs/tex/MainDesc.png",
    "imgs/tex/MercDesc.png",
    "imgs/tex/VenusDesc.png",
    "imgs/tex/EarthDesc.png",
    "imgs/tex/MarsDesc.png"
];

var info;

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
    
    ortoCamera();
    
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
            null,
            "imgs/Меркурий/mercurybump.jpg",
            null,
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
            null,
            "imgs/Венера/venusbump.jpg",
            null,
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
            addPlanet(
                0.7,
                "imgs/Земля/Луна/moonmap1k.jpg",
                6,
                0.4,
                0.1,
                null,
                "imgs/Земля/Луна/moonbump1k.jpg",
                null,
                null
            ),
            "imgs/Земля/earthbump1k.jpg",
            "imgs/Земля/earthlights1k.jpg",
            createEarthCloud()
        )
    );

    planets.push(
        addPlanet(
            3,
            "imgs/Марс/marsmap1k.jpg",
            60,
            0.8043333333,
            1.0869565217,
            null,
            "imgs/Марс/marsbump1k.jpg",
            null,
            null
        )
    );

    // planets.push(
    //   createEarthCloud(45,12,1,1)
    // );


    var spritey = makeTextSprite(" World! ", {
        fontsize:32,
        fontface: "Georgia",
        borderColor: { r: 0, g: 0, b: 255, a: 1.0 },
    });
    spritey.position.set(0, 0, 0);
    scene.add(spritey);

    info = addSprite();

    sceneOrtho.add(info.sprite);
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

        if (planets[i].clouds != null) {
            var cm = new THREE.Matrix4();
            var cm1 = new THREE.Matrix4();

            // var mm = new THREE.Matrix4();
            // mm.copyPosition(m);
            //получение позиции из матрицы позиции
            // var pos = new THREE.Vector3(0, 0, 0);
            // pos.setFromMatrixPosition(mm);
            // cm.setPosition(pos);

            // a += 2 * delta;
            // cm.makeRotationY(a);
            planets[i].clouds.matrix = cm1.multiplyMatrices(
                cm,
                planets[i].sphere.matrix
            );
            planets[i].clouds.matrixAutoUpdate = false;
        }
    }

    // Добавление функции на вызов, при перерисовки браузером страницы
    requestAnimationFrame(animate);
    render();
}
function render() {
    //функция render
    //процесс отрисовки сцены и объектов в экранных координатах
    renderer.clear();
    renderer.render(scene, camera);
    renderer.clearDepth();
    renderer.render(sceneOrtho, cameraOrtho);
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

function addPlanet(r, tname, x, s1, s2, sat, bname, sname, clouds) {
    //создание геометрии сферы
    var geometry = new THREE.SphereGeometry(r, 32, 32);
    //загрузка текстуры
    var tex = new THREE.TextureLoader().load(tname);
    tex.minFilter = THREE.NearestFilter;
    //загрузка карты высот
    var bump = new THREE.TextureLoader().load(bname);
    var spec = new THREE.TextureLoader().load(sname);
    //создание материала
    var material = new THREE.MeshPhongMaterial({
        map: tex,
        bumpMap: bump,
        specularMap: spec,
        specular: new THREE.Color("grey"),
        bumpScale: 0.05,
        side: THREE.DoubleSide,
    });

    var sphere = new THREE.Mesh(geometry, material);
    //создание объекта
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
    planet.clouds = clouds;

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
        updateSp(0);
    }
    
    if (keyboard.pressed("1")) {
        chase = 0;
        updateSp(1);
    }
    if (keyboard.pressed("2")) {
        chase = 1;
        updateSp(2);
    }
    if (keyboard.pressed("3")) {
        chase = 2;
        updateSp(3);
    }
    if (keyboard.pressed("4")) {
        chase = 3;
        updateSp(4);
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

function createEarthCloud() {
    // create destination canvas
    var canvasResult = document.createElement("canvas");
    canvasResult.width = 1024;
    canvasResult.height = 512;
    var contextResult = canvasResult.getContext("2d");
    // load earthcloudmap
    var imageMap = new Image();
    imageMap.addEventListener(
        "load",
        function () {
            // create dataMap ImageData for earthcloudmap
            var canvasMap = document.createElement("canvas");
            canvasMap.width = imageMap.width;
            canvasMap.height = imageMap.height;
            var contextMap = canvasMap.getContext("2d");
            contextMap.drawImage(imageMap, 0, 0);
            var dataMap = contextMap.getImageData(
                0,
                0,
                canvasMap.width,
                canvasMap.height
            );
            // load earthcloudmaptrans
            var imageTrans = new Image();
            imageTrans.addEventListener("load", function () {
                // create dataTrans ImageData for earthcloudmaptrans
                var canvasTrans = document.createElement("canvas");
                canvasTrans.width = imageTrans.width;
                canvasTrans.height = imageTrans.height;
                var contextTrans = canvasTrans.getContext("2d");
                contextTrans.drawImage(imageTrans, 0, 0);
                var dataTrans = contextTrans.getImageData(
                    0,
                    0,
                    canvasTrans.width,
                    canvasTrans.height
                );
                // merge dataMap + dataTrans into dataResult
                var dataResult = contextMap.createImageData(
                    canvasMap.width,
                    canvasMap.height
                );
                for (var y = 0, offset = 0; y < imageMap.height; y++)
                    for (var x = 0; x < imageMap.width; x++, offset += 4) {
                        dataResult.data[offset + 0] = dataMap.data[offset + 0];
                        dataResult.data[offset + 1] = dataMap.data[offset + 1];
                        dataResult.data[offset + 2] = dataMap.data[offset + 2];
                        dataResult.data[offset + 3] =
                            255 - dataTrans.data[offset + 0];
                    }
                // update texture with result
                contextResult.putImageData(dataResult, 0, 0);
                material.map.needsUpdate = true;
            });

            imageTrans.src = "imgs/Земля/earthcloudmaptrans.jpg";
        },
        false
    );

    imageMap.src = "imgs/Земля/earthcloudmap.jpg";
    var geometry = new THREE.SphereGeometry(4.5, 32, 32);

    var material = new THREE.MeshPhongMaterial({
        map: new THREE.Texture(canvasResult),
        side: THREE.DoubleSide,
        transparent: true,
        opacity: 0.8,
    });

    var mesh = new THREE.Mesh(geometry, material);

    //размещение объекта в сцене
    scene.add(mesh);

    return mesh;
}

function ortoCamera() {
    //создание ортогональной камеры
    width = window.innerWidth;
    height = window.innerHeight;
    cameraOrtho = new THREE.OrthographicCamera(
        -width / 2,
        width / 2,
        height / 2,
        -height / 2,
        1,
        10
    );
    cameraOrtho.position.z = 10;
    //сцена для хранения списка объектов размещаемых в экранных координатах
    sceneOrtho = new THREE.Scene();
    //отключение авто очистки рендера
    renderer.autoClear = false;

}
var sprite = new THREE.Sprite();

//функция для создания спрайта
function addSprite() {
    //загрузка текстуры спрайта
    var descSprite = {};
    descSprite.m = [];

    desc.forEach(element => {
        
        var texture = loader.load(element);
        var material = new THREE.SpriteMaterial({ map: texture });
        descSprite.m.push(material);
        
    });

    //создание спрайта
    sprite = new THREE.Sprite();
    //центр и размер спрайта
    sprite.center.set(0.0, 1.0);
    sprite.scale.set(250, 170, 1);
    //позиция спрайта (центр экрана)
    sprite.position.set(-sw/2.0, sh/2.0, 1);
    
    sprite.material = descSprite.m[0];

    descSprite.sprite = sprite;
    
    
    return descSprite;
}

function updateSp(k) {
    info.sprite.material = info.m[k];    
}
//функция для обновления позиции спрайта
function updateHUDSprite(sprite) {
    var width = window.innerWidth / 2;
    var height = window.innerHeight / 2;

    sprite.position.set(-width, height, 1); // левый верхний угол экрана
}

//sourse https://github.com/stemkoski/stemkoski.github.com/blob/master/Three.js/Sprite-Text-Labels.html
/*example use
var spritey = makeTextSprite( " World! ", 
{ fontsize: 32, fontface: "Georgia", borderColor: {r:0, g:0, b:255, a:1.0} } );
spritey.position.set(55,105,55);
scene.add( spritey );
*/

function makeTextSprite(message, parameters) {
    if (parameters === undefined) parameters = {};

    var fontface = parameters.hasOwnProperty("fontface")
        ? parameters["fontface"]
        : "Arial";

    var fontsize = parameters.hasOwnProperty("fontsize")
        ? parameters["fontsize"]
        : 18;

    var borderThickness = parameters.hasOwnProperty("borderThickness")
        ? parameters["borderThickness"]
        : 4;

    var borderColor = parameters.hasOwnProperty("borderColor")
        ? parameters["borderColor"]
        : { r: 0, g: 0, b: 0, a: 1.0 };

    var backgroundColor = parameters.hasOwnProperty("backgroundColor")
        ? parameters["backgroundColor"]
        : { r: 255, g: 255, b: 255, a: 1.0 };


    var canvas = document.createElement("canvas");
    var context = canvas.getContext("2d");
    context.font = "Bold " + fontsize + "px " + fontface;

    // get size data (height depends only on font size)
    var metrics = context.measureText(message);
    var textWidth = metrics.width;

    // background color
    context.fillStyle =
        "rgba(" +
        backgroundColor.r +
        "," +
        backgroundColor.g +
        "," +
        backgroundColor.b +
        "," +
        backgroundColor.a +
        ")";
    // border color
    context.strokeStyle =
        "rgba(" +
        borderColor.r +
        "," +
        borderColor.g +
        "," +
        borderColor.b +
        "," +
        borderColor.a +
        ")";

    context.lineWidth = borderThickness;
    roundRect(
        context,
        borderThickness / 2,
        borderThickness / 2,
        textWidth + borderThickness,
        fontsize * 1.4 + borderThickness,
        6
    );
    // 1.4 is extra height factor for text below baseline: g,j,p,q.

    // text color
    context.fillStyle = "rgba(0, 0, 0, 1.0)";

    context.fillText(message, borderThickness, fontsize + borderThickness);

    // canvas contents will be used for a texture
    var texture = new THREE.Texture(canvas);
    texture.needsUpdate = true;

    var spriteMaterial = new THREE.SpriteMaterial({
        map: texture,
        useScreenCoordinates: false,
    });
    var sprite = new THREE.Sprite(spriteMaterial);
    sprite.scale.set(100, 50, 1.0);
    return sprite;
}

// function for drawing rounded rectangles
function roundRect(ctx, x, y, w, h, r) {
    ctx.beginPath();
    ctx.moveTo(x + r, y);
    ctx.lineTo(x + w - r, y);
    ctx.quadraticCurveTo(x + w, y, x + w, y + r);
    ctx.lineTo(x + w, y + h - r);
    ctx.quadraticCurveTo(x + w, y + h, x + w - r, y + h);
    ctx.lineTo(x + r, y + h);
    ctx.quadraticCurveTo(x, y + h, x, y + h - r);
    ctx.lineTo(x, y + r);
    ctx.quadraticCurveTo(x, y, x + r, y);
    ctx.closePath();
    ctx.fill();
    ctx.stroke();
}
