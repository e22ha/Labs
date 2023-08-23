using Script;
using UnityEngine;
using UnityEngine.AI;

public class BotScript : IObject
{
    public Animator anim;
    NavMeshAgent _agent;

    public LayerMask player;

    public float agroRange = 10.0f;
    public float atcRng = 2.0f;

    public Transform point1;
    public Transform point2;

    bool _life = true;

    private void Start()
    {
        //anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    public override void Interact()
    {
        _life = false;
        anim.SetTrigger(States.fall.ToDescriptionString());
    }

    public override Vector3 GetPoint(Vector3 position)
    {
        var dist1 = Vector3.Distance(position, point1.position);
        var dist2 = Vector3.Distance(position, point2.position);

        return dist1 < dist2 ? point1.position : point2.position;
    }

    public override States GetAction()
    {
        return States.hit;
    }

    public void LateUpdate()
    {
        if (_life != true) return;
        Collider[] coll = Physics.OverlapSphere(transform.position, agroRange, player);

        if (_agent.velocity != Vector3.zero)
        {
            anim.SetBool("isWalking", true);
        }
        else if (_agent.velocity == Vector3.zero)
        {
            anim.SetBool("isWalking", false);
        }

        if (coll.Length <= 0) return;
        if (Vector3.Distance(transform.position, coll[0].transform.position) > atcRng)
        {
            _agent.SetDestination(coll[0].transform.position);
        }
        else
        {
            anim.SetTrigger("Hit");
        }
    }
}
