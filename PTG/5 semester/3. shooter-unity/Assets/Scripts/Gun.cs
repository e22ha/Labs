using System;
using TMPro;
using UnityEngine;


public class Gun : MonoBehaviour
{
    public TMP_Text ScoreText;
    public int score = 0;

    public float damage = 10f;
    public float range = 1000f;

    public Camera cam;
    public ParticleSystem flash;
    public ParticleSystem onHit;
    public GameObject gilse;
    public GameObject ejectPoint;


    public float fireRate = 10f;
    public float nextShot;

    private void Update()
    {
        if (!Input.GetButton("Fire1") || !(Time.time >= nextShot)) return;
        nextShot = Time.time + 1 / fireRate;
        Shoot();
    }

    private void Shoot()
    {
        flash.Play();

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out var hit, range))
        {
            if (hit.transform.CompareTag("Target"))
            {
                var t = hit.transform.GetComponent<Target>();
                updatePoint(t.Hit(damage));
            }
        }

        var hitEffect = Instantiate(onHit, hit.point, Quaternion.LookRotation(hit.normal));
        hitEffect.Play();

        var gilseClone = Instantiate(gilse, ejectPoint.transform.position, Quaternion.LookRotation(Vector3.zero));
        gilseClone.GetComponent<Rigidbody>().AddForce(Vector3.forward* 100f);

        Destroy(hitEffect.gameObject, 1f);
        Destroy(gilseClone.gameObject, 1f);
    }

    private void updatePoint(float val)
    {
        score += (int)val;
        ScoreText.text = $"Score: {score:F}";
    }
}