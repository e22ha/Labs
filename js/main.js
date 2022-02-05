console.log("Hello");

// Ссылка на элемент веб страницы в котором будет отображаться графика
var container;
// Переменные "камера", "сцена" и "отрисовщик"
var camera, scene, renderer;

init();
//render();

function choose() {
  var rad = document.getElementsByName("selector");
  for (var i = 0; i < rad.length; i++) {
    if (rad[i].checked) {
      if (i == 0) {
        animate();
        console.log(i);
      }
      if (i == 1) {
        animate();
        terrain();
        console.log(i);
      }
      if (i == 2) {
        animate();
        regpolygrid();
        console.log(i);
      }
      if (i == 3) {
        animate();
        console.log(i);
      }
      if (i == 4) {
        animate();
        console.log(i);
      }
    }
  }
}

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
  camera.position.set(5, 5, 5);

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
  window.addEventListener("resize", onWindowResize, false);
}

function onWindowResize() {
  // Изменение соотношения сторон для виртуальной камеры
  camera.aspect = window.innerWidth / window.innerHeight;
  camera.updateProjectionMatrix();
  // Изменение соотношения сторон рендера
  renderer.setSize(window.innerWidth, window.innerHeight);
}

function animate() {
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

  var geometry = new THREE.BufferGeometry(); // Создание структуры для хранения геометрии

  vertices.push(1.0, 0.0, 3.0); // Добавление координат первой вершины в массив вершин
  vertices.push(1.0, 3.0, 0.0); // Добавление координат второй вершины в массив вершин
  vertices.push(3.0, 0.0, 1.0); // Добавление координат третьей вершины в массив вершин

  faces.push(0, 1, 2); // Добавление индексов (порядок соединения вершин) в массив индексов

  //Добавление вершин и индексов в геометрию
  geometry.setAttribute(
    "position",
    new THREE.Float32BufferAttribute(vertices, 3)
  );
  geometry.setIndex(faces);

  var colors = []; // Объявление массива для хранения цветов вершин
  colors.push(0.8, 0.0, 0.0); // Добавление цвета для первой вершины (красный)
  colors.push(0.0, 0.8, 0.0); // Добавление цвета для второй вершины (зелёный)
  colors.push(0.0, 0.0, 0.8); // Добавление цвета для третьей вершины (синий)

  //Добавление цветов вершин в геометрию
  geometry.setAttribute("color", new THREE.Float32BufferAttribute(colors, 3));

  var _material = new THREE.MeshBasicMaterial({
    vertexColors: THREE.VertexColors,
    wireframe: false,
    side: THREE.DoubleSide,
  });

  // Создание объекта и установка его в определённую позицию
  var _mesh = new THREE.Mesh(geometry, _material);
  _mesh.position.set(0.0, 0.0, 0.0);

  // Добавление объекта в сцену
  scene.add(_mesh);
}

function regpolygrid() {
  var vertices = []; // Объявление массива для хранения вершин
  var faces = []; // Объявление массива для хранения индексов
  var geometry = new THREE.BufferGeometry(); // Создание структуры для хранения геометрии

  var a = 4;

  for (y = 0; y < a; y++) {
    for (x = 0; x < a; x++) {
      vertices.push(x, 0.0, y);
    }
  }

  for (y = 0; y < a; y++) {
    for (x = 0; x < a; x++) {
      faces.push(x + y, x + y + 1, (x + 1) * a + y + 1);
      faces.push(x + y, (x + 1) * a + y + 1, (x + 1) * a + y);
    }
  }

  //Добавление вершин и индексов в геометрию
  geometry.setAttribute(
    "position",
    new THREE.Float32BufferAttribute(vertices, 3)
  );
  geometry.setIndex(faces);

  var colors = []; // Объявление массива для хранения цветов вершин
  colors.push(0.8, 0.0, 0.0); // Добавление цвета для первой вершины (красный)
  colors.push(0.8, 0.0, 0.0); // Добавление цвета для второй вершины (зелёный)
  colors.push(0.8, 0.0, 0.0); // Добавление цвета для третьей вершины (синий)

  //Добавление цветов вершин в геометрию
  geometry.setAttribute("color", new THREE.Float32BufferAttribute(colors, 3));

  var _material = new THREE.MeshBasicMaterial({
    vertexColors: THREE.VertexColors,
    wireframe: false,
    side: THREE.DoubleSide,
  });

  // Создание объекта и установка его в определённую позицию
  var _mesh = new THREE.Mesh(geometry, _material);
  _mesh.position.set(0.0, 0.0, 0.0);

  // Добавление объекта в сцену
  scene.add(_mesh);
}
