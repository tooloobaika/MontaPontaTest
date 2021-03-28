using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController playerController;

    public GameObject bag;

    GameObject cTree;

    public float lookRadius;

    public NavMeshAgent player;

    Animator animator;

    Transform target;
    Transform house;

    bool treePosition;
    bool housePosition;

    bool isRun = true;
    bool isAttack;
    private void OnEnable()
    {
        playerController = this;
    }

    void Start()
    {
        house = HouseManager.instance.house.transform;
        player = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Tree").Length > 0)
        {
            target = GameObject.FindGameObjectWithTag("Tree").transform;
            
            if (!treePosition)
                MovementToTree();
        }
        else if (GameObject.FindGameObjectsWithTag("Tree").Length == 0)
        {
            //Debug.Log("Empty wood");
            animator.SetBool("Idle", true);
            if(housePosition)
                bag.SetActive(false);
        }
        else
        {
            //Debug.Log("Empty");
        }
        if (housePosition)
            housePosition = false;
    }

    private void FixedUpdate()
    {
        if (isRun)
        {
            animator.SetBool("Run", true);
        }
        if (isAttack)
        {
            isRun = false;
            animator.SetBool("Attack", true);
        }
        else
        {
            animator.SetBool("Attack", false);
        }
    }

    /// <summary>
    /// Метод для перемещения к дереву
    /// </summary>
    void MovementToTree()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Tree");
        GameObject closests = null;
        Vector3 pos = transform.position;
        float dist = Mathf.Infinity;
        foreach(GameObject g in gos)
        {
            Vector3 diff = pos - g.transform.position;
            float curDistance = diff.magnitude;
            if(curDistance < dist)
            {
                closests = g;
                cTree = g;
                dist = curDistance;
            }
        }
        bag.SetActive(false);
        if (dist <= lookRadius)
            player.SetDestination(closests.transform.position);
    }

    /// <summary>
    /// Метод для перемещения к дому
    /// </summary>
    void MovementBackToHome()
    {
        player.SetDestination(house.position);
        bag.SetActive(true);
    }

    /// <summary>
    /// Метод для проверки триггера на вход
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Tree")
        {
            treePosition = true;
            housePosition = false;
            isAttack = true;
            StartCoroutine("MoveBack");
        }
        if(other.gameObject.tag == "House")
        {
            housePosition = true;
            treePosition = false;
            WoodCounter.wood.Count();
            MovementToTree();
        }
    }
    
    /// <summary>
    /// Метод для 1.остановки у дерева, 2.уничтожения дерева, 3.возврат к дому
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveBack()
    {
        player.speed = 0;
        yield return new WaitForSeconds(3);
        cTree.gameObject.GetComponent<Animator>().Play("TreeDeath");
        Destroy(cTree.gameObject, 0.7f);
        isAttack = false;
        player.speed = 3.5f;
        MovementBackToHome();
    }

    /// <summary>
    /// Нарисовать в инспекторе радиус видимости деревьев
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }


}
