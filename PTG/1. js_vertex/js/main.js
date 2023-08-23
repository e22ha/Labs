console.log("Hello");

// Ссылка на элемент веб страницы в котором будет отображаться графика
var container;
// Переменные "камера", "сцена" и "отрисовщик"
var camera, scene, renderer;

// Глобальная переменная для хранения карты высот
var imagedata;

// Размерность сетки
var N = 255;
var a = 0.0;

var spotlight;
var sphere;

init();
animate();

// В этой функции можно добавлять объекты и выполнять их первичную настройку
function init() {
    console.log("Hello");

    // Получение ссылки на элемент html страницы
    container = document.getElementById('container');
    // Создание "сцены"
    scene = new THREE.Scene();
    // Установка параметров камеры
    // 45 - угол обзора
    // window.innerWidth / window.innerHeight - соотношение сторон
    // 1 - 4000 - ближняя и дальняя плоскости отсечения
    camera = new THREE.PerspectiveCamera(
        45, window.innerWidth / window.innerHeight, 1, 4000);

    // Установка позиции камеры
    camera.position.set(N / 2, N / 2, N * 2);
    // Установка точки, на которую камера будет смотреть
    camera.lookAt(new THREE.Vector3(N / 2, 0.0, N / 2));

    // Создание отрисовщика
    renderer = new THREE.WebGLRenderer({
        antialias: false
    });
    renderer.setSize(window.innerWidth, window.innerHeight);
    // Закрашивание экрана синим цветом, заданным в 16ричной системе
    renderer.setClearColor(0x006600ff, 1);
    container.appendChild(renderer.domElement);
    // Добавление функции обработки события изменения размеров окна
    window.addEventListener('resize', onWindowResize, false);

    //создание точечного источника освещения заданного цвета
    spotlight = new THREE.PointLight(0xffffff);
 
    //установка позиции источника освещения
    spotlight.position.set(N*1.5, N, N/2 );
    //добавление источника в сцену
    scene.add(spotlight);

    const geometry = new THREE.SphereGeometry( 15, 32, 16 );
    const material = new THREE.MeshBasicMaterial( { color: 0xffff00 } );
    sphere = new THREE.Mesh( geometry, material );
    scene.add( sphere );

    var canvas = document.createElement('canvas');
    var context = canvas.getContext('2d');
    var img = new Image();
    img.onload = function () {
        canvas.width = img.width;
        canvas.height = img.height;
        context.drawImage(img, 0, 0);
        imagedata = context.getImageData(0, 0, img.width, img.height);
        // Пользовательская функция генерации ландшафта
        terrain();
    }
    // Загрузка изображения с картой высот
    img.src = 'img/plateau.jpg';
} 

function getPixel( imagedata, x, y )
{
    var position = ( x + imagedata.width * y ) * 4, data = imagedata.data;
    return data[ position ];;
}

function onWindowResize() {
  // Изменение соотношения сторон для виртуальной камеры
  camera.aspect = window.innerWidth / window.innerHeight;
  camera.updateProjectionMatrix();
  // Изменение соотношения сторон рендера
  renderer.setSize(window.innerWidth, window.innerHeight);
}

function animate() {
    a += 0.01;

    var x = N/2 + N * Math.cos(a);
    var y = 0 + N * Math.sin(a);

    spotlight.position.set(x, y, N/2);
    sphere.position.copy(spotlight.position);

    // Добавление функции на вызов, при перерисовки браузером страницы
    requestAnimationFrame(animate);
    render();
}

function render() {
  // Рисование кадра
  renderer.render(scene, camera);
}

function terrain() {
    var vertices = []; // Объявление массива для хранения вершин
    var faces = []; // Объявление массива для хранения индексов
    var uvs = []; // Массив для хранения текстурных координат 

    var geometry = new THREE.BufferGeometry(); // Создание структуры для хранения геометрии

    for (var i = 0; i < N; i++)
        for (var j = 0; j < N; j++) {
            //получение цвета пикселя в десятом столбце десятой строки изображения
            var h = getPixel( imagedata, i, j );

             vertices.push(i, h/5, j); // Добавление координат третьей вершины в массив вершин
            uvs.push(i / (N - 1), j / (N - 1)); // Добавление цвета для первой вершины (красный)  
        }

    for (var i = 0; i < N - 1; i++)
        for (var j = 0; j < N - 1; j++) {
            faces.push(i + j * N, (i + 1) + j * N, (i + 1) + (j + 1) * N); // Добавление индексов (порядок соединения вершин) в массив индексов
            faces.push(i + j * N, (i + 1) + (j + 1) * N, i + (j + 1) * N); // Добавление индексов (порядок соединения вершин) в массив индексов
        }

    //Добавление вершин и индексов в геометрию
    geometry.setAttribute('position', new THREE.Float32BufferAttribute(vertices, 3));
    geometry.setIndex(faces);

    //Добавление текстурных координат в геометрию
    geometry.setAttribute('uv', new THREE.Float32BufferAttribute(uvs, 2));

    geometry.computeVertexNormals(); 

    // Загрузка текстуры yachik.jpg из папки pics
    var tex = new THREE.TextureLoader().load('img/grasstile.jpg');

    var _material = new THREE.MeshLambertMaterial({
        map: tex,
        wireframe: false,
        side: THREE.DoubleSide
    });

    // Режим повторения текстуры
    tex.wrapS = tex.wrapT = THREE.RepeatWrapping;
    // Повторить текстуру 10х10 раз
    tex.repeat.set(3, 3);

    // Создание объекта и установка его в определённую позицию
    var _mesh = new THREE.Mesh(geometry, _material);
    _mesh.position.set(0.0, 0.0, 0.0);

    // Добавление объекта в сцену
    scene.add(_mesh);
}
