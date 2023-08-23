using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;


public class Target : MonoBehaviour
{
    private static float basehp = 10f;
    public ParticleSystem explosion;
    public float hp;

    private float frameoflife;
    private float time;

    public float  Hit(float damage)
    {

        hp -= damage;
        if (hp <= 0) Death();
        return hp <= 0 ? hp+damage : damage;
    }

    [Range(0f, 10f)] public float dist = 5f;

    private Rigidbody _rb;
    private float _mSpeed = 10f;
    

    private void Start()
    {
        basehp = Random.Range(5,200);
        frameoflife = Random.Range(10f,100f);
        hp = basehp;
        _rb = GetComponent<Rigidbody>();

        gameObject.transform.localScale *= hp / 10f;
        time = 0;
    }

    private void FixedUpdate()
    {
        var xMove = Random.Range(-dist, dist);
        var zMove = Random.Range(-dist, dist);


        var dir = new Vector3(xMove, 0, zMove);

        dir.Normalize();

        var v = transform.TransformDirection(dir) * (_mSpeed * Time.fixedDeltaTime);

        v.y = _rb.velocity.y;

        _rb.velocity += v;


    }

    private void Update()
    {
        time += Time.deltaTime;
        Debug.Log(time);
        if (time >= frameoflife)
            Destroy(gameObject);
    }

    private void Death()
    {
        ParticleSystem exp = Instantiate(explosion, transform.position, transform.rotation);
        exp.Play();


        Destroy(exp.gameObject, 1f);
        Destroy(gameObject);
    }
}