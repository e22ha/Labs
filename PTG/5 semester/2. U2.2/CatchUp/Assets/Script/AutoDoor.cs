using UnityEngine;

namespace Script
{
    public class AutoDoor : MonoBehaviour
    {
        public Animator an;

        public Collider c;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("01");
            if (other != c) return;
            Debug.Log("02");
            an.SetBool("isOpen", true);
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("03");
            if (other != c) return;
            Debug.Log("04");
            an.SetBool("isOpen", false);
        }
    }
}