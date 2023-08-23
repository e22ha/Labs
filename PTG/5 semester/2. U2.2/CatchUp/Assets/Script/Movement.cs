using Script;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    [SerializeField]
    public Camera cam;

    public GameObject targetObj;

    public Animator animator;

    //private string groundTag = "Ground";

    private NavMeshAgent agent;


    public LayerMask floor;
    public LayerMask inter;
    public LayerMask bot;

    public float interactionRange = 4.0f;

    IObject obj;

    // Called when a script is enabled
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Called once every frame
    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100, inter))
            {
                Vector3 objPosition = hit.transform.GetComponent<IObject>().GetPoint(transform.position);

                double dist = Vector3.Distance(objPosition, transform.position);

                if (dist < interactionRange)
                {
                    hit.transform.GetComponent<IObject>().Interact();
                    animator.SetTrigger(States.hit.ToDescriptionString());
                }
                else
                {
                    agent.SetDestination(objPosition);
                }
            }
            else
            if (Physics.Raycast(ray, out hit, 100, bot))
            {
                Vector3 objPosition = hit.transform.GetComponent<IObject>().GetPoint(transform.position);

                double dist = Vector3.Distance(objPosition, transform.position);

                if (dist < interactionRange)
                {
                    hit.transform.GetComponent<IObject>().Interact();
                    animator.SetTrigger(States.hit.ToDescriptionString());
                }
                else
                {
                    agent.SetDestination(objPosition);
                }
            }
            else
            if (Physics.Raycast(ray, out hit, 100, floor))
            {
                targetObj.transform.position = hit.point;
                agent.SetDestination(hit.point);
            }

        }

        if (agent.velocity != Vector3.zero)
        {
            animator.SetBool("isWalking", true);
        }
        else if (agent.velocity == Vector3.zero)
        {
            animator.SetBool("isWalking", false);
        }
    }
}