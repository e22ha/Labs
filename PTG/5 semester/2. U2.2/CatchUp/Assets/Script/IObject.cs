using UnityEngine;

namespace Script
{
    public abstract class IObject : MonoBehaviour
    {
        public abstract void Interact();

        public abstract Vector3 GetPoint(Vector3 vector3);

        public abstract States GetAction();

    }
}
