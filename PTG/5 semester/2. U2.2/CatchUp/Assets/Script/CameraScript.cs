using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player, cameraTrans;

    private void Update(){
		cameraTrans.LookAt(player);
	}
}