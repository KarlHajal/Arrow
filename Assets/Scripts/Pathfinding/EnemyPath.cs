using UnityEngine;
using System.Collections;

public class EnemyPath : MonoBehaviour
{

    //public Transform target;
    public GameObject target;
    public float speed = 5f;
    Vector2[] path;
    int targetIndex;
    public float pathReloadTime = 0.1f;
    private float lastReloadInstance = 0;
    Animator anim;
    bool facingRight = true;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        target = GameObject.Find("Character");
        PathRequestManager.RequestPath(transform.position, target.transform.position, OnPathFound);
    }

    void Update()
    {
            if (Time.time - lastReloadInstance >= pathReloadTime && target != null)
            {
                PathRequestManager.RequestPath(transform.position, target.transform.position, OnPathFound);
                lastReloadInstance = Time.time;
            }
             else
            {
                target = GameObject.Find("Character");
            }

        transform.position = transform.position.y * Vector3.up + transform.position.x * Vector3.right + Vector3.forward * transform.position.y;
    }

    public void OnPathFound(Vector2[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector2 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position.x == currentWaypoint.x && transform.position.y == currentWaypoint.y)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    targetIndex = 0;
                    path = new Vector2[0];
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), currentWaypoint, speed*Time.deltaTime) ;
            transform.position = transform.position.y * Vector3.up + transform.position.x * Vector3.right + Vector3.forward * transform.position.y;
            Animate(new Vector2(transform.position.x, transform.position.y), currentWaypoint);

            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if(path != null)
        {
            for(int i=targetIndex; i<path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one * (0.3f));

                if (i == targetIndex)
                    Gizmos.DrawLine(transform.position, path[i]);
                else
                    Gizmos.DrawLine(path[i - 1], path[i]);
            }
        }
    }

    void Animate(Vector2 EnemyPos, Vector2 WaypointPos)
    {
        if (Mathf.Abs(EnemyPos.x - WaypointPos.x) > Mathf.Abs(EnemyPos.y - WaypointPos.y))
        {
            anim.SetFloat("MovingX", 1);
            anim.SetFloat("MovingY", 0);
            if (EnemyPos.x - WaypointPos.x > 0 && facingRight)
                Flip();
            else if (EnemyPos.x - WaypointPos.x < 0 && !facingRight)
                Flip();
        }
        else
        {
            anim.SetFloat("MovingX", 0);
            if (EnemyPos.y - WaypointPos.y > 0)
                anim.SetFloat("MovingY", -1);
            else
                anim.SetFloat("MovingY", 1);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }
}
