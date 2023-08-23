public class MazeCell
    {
        public int X;
        public int Y;

        public bool Left = true;
        public bool Right = true;
        public bool Up = true;
        public bool Bottom = true;
        
        

        public bool Visited = false;
        public int Distance;
    }
