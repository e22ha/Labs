using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minesweeper
{

    class logicField
    {
        /// <summary>
        /// Описание состояния элемента поля
        /// 0,1,2,3,4,5,6,7,8 - поле рядом с которым n количество мин
        /// 9 - поле с миной
        /// </summary>


        //Размер поля
        int N;

        // Создание генератора случайных чисел
        Random rng = new Random();
        static int GenerateDigit(Random rng, int k)
        {
            // Предположим, что здесь много логики
            return rng.Next(k);
        }
        //Колличество мин
        int Mine;

        //Функция создания поля
        public int[,] generateField(int N, int Mine)
        {
            int[,] filed = new int[N, N];

            while (Mine != 0)
            {
                //Строка
                int i = GenerateDigit(rng, (N * N) + 1) / N;
                //Столбец
                int j = GenerateDigit(rng, (N * N) + 1) % N;
                if (filed[i, j] != 9)
                {
                    filed[i, j] = 9;
                    Mine--;
                }
            }
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (filed[i, j] == 9)
                    {
                        if(filed[i - 1, j - 1]!=9) filed[i - 1, j - 1] += 1;//↖
                        if(filed[i - 1, j]!=9) filed[i - 1, j] += 1;//⬅
                        if(filed[i - 1, j + 1]!=9) filed[i - 1, j + 1] += 1;//↙
                        if(filed[i, j - 1]!=9) filed[i , j - 1] += 1;//⬆
                        if(filed[i, j + 1]!=9) filed[i , j + 1] += 1;//⬇
                        if(filed[i + 1, j - 1]!=9) filed[i + 1, j - 1] += 1;//↗
                        if(filed[i + 1, j]!=9) filed[i + 1, j] += 1;//➡
                        if(filed[i + 1, j + 1]!=9) filed[i + 1, j + 1] += 1;//↘
                    }
                }
            }

            return filed;
        }

    }
}
