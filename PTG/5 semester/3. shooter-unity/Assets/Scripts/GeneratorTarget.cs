using UnityEngine;
using Random = UnityEngine.Random;

public class GeneratorTarget : MonoBehaviour
{
    public GameObject target;

    public GameObject centerStage;

    [Range(0f, 10f)] public float dist = 5f;
    [Range(0f, 10f)] public float height = 1f;

    [Range(0, 5)] public int count = 3;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        GenTarget(count, centerStage);
    }

    private void GenTarget(int i, GameObject c)
    {
        for (var j = 0; j < i; j++)
        {
            var targetClone = Instantiate(target,
                (c.transform.position),
                c.transform.rotation);
                targetClone.transform.position += new Vector3(Random.Range(-dist, dist), Random.Range(-height, height),
                Random.Range(-dist, dist));
        }
    }
}