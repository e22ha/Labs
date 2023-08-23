using UnityEngine;

namespace Script
{
    public class DoorScript : IObject
    {
        private Animator _an;


        public Transform point1;
        public Transform point2;
        private static readonly int IsOpen = Animator.StringToHash("isOpen");

        private void Start()
        {
            _an = GetComponent<Animator>();
        }

        public override States GetAction()
        {
            return States.hit;
        }

        public override Vector3 GetPoint(Vector3 position)
        {
            var dist1 = Vector3.Distance(position, point1.position);
            var dist2 = Vector3.Distance(position, point2.position);

            if (dist1 < dist2)
                return point1.position;
            else
                return point2.position;

        }

        public override void Interact()
        {
            _an.SetBool(IsOpen, !_an.GetBool(IsOpen));
        
        }
    }
}
