using UnityEngine;


public class GScript : MonoBehaviour
{
    public float frameLife;
    public float nextShot;
    
    
    void Start()
    {
        frameLife = 5f;
    }

    void FixedUpdate()
    {
        if (frameLife > 0f) frameLife-=1f;
        else Destroy(gameObject);
    }
}