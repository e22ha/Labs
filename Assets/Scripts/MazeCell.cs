using UnityEngine;


    public class MazeCell : MonoBehaviour
    {
        public int X;
        public int Y;

        public bool Left = true;
        public bool Right = true;
        public bool Up = true;
        public bool Bottom = true;

        public bool Visited = false;
    }
