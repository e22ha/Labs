using System;
using TMPro;
using UnityEngine;


public class MazeCell : MonoBehaviour
    {
        public int x;
        public int y;

        public bool left = true;
        public bool right = true;
        public bool up = true;
        public bool bottom = true;
        
        

        public bool visited = false;
        public int distance;
    }
