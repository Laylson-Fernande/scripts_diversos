using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class Enemy : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField]
    bool EnemyStatic;
    NavMeshAgent agent;
    NavMeshPath path;
    NavMeshPath pathTemp;
    public float distance;
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject child;
    [SerializeField]
    Transform rotate;
    Vector3 startPosi;
    Vector3 startRotation;
    Animator anim;
    string fraqueza = "";
    float intervalo = 0;
    public bool semParticulas;
    [SerializeField]
    VisualEffect particulas;
    [SerializeField]
    SkinnedMeshRenderer skinMesh;
    [SerializeField]
    Material material;
    public int idEnemy;
    public bool morrer;
    // Start is called before the first frame update
    void Start()
    {
        idEnemy = GetInstanceID();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        anim = GetComponentInChildren<Animator>();
        if (!EnemyStatic)
        {
            startPosi = transform.position;
            startRotation = transform.rotation.eulerAngles;
            agent = GetComponent<NavMeshAgent>();
            path = agent.path;
            pathTemp = new NavMeshPath();
            player = gameManager.player;
            //player = GameObject.FindGameObjectWithTag("Player");
            anim.SetBool("Walking", false);
            agent.updateRotation = true;
        }

        Debug.Log("id do inimigo "+ GetInstanceID());
        if (gameManager.enemysDead.Contains(idEnemy)) {
            Destroy(this.gameObject);
        }

        if (!EnemyStatic)
        {

            PodeSeguirPlayer();
        }
        if (morrer) {
            Invoke("Sumir",5);
        }
    }

    void Sumir() {
        Debug.Log("Ta sumindo aquiiii");
        if (!EnemyStatic)
        {
            agent.path.ClearCorners();
            EnemyStatic = true;
        }
        anim.SetBool("Walking", false);
        Destroy(this.GetComponent<Rigidbody>());
        Destroy(this.GetComponent<CapsuleCollider>());
        StartCoroutine(Disappear());
        if (!semParticulas)
            particulas.enabled = true;
    }
    private void Update()
    {
        if(!EnemyStatic)
        PodeSeguirPlayer();
    }
    void  PodeSeguirPlayer()
    {
        
        bool podeSeguir = NavMesh.CalculatePath(this.transform.position, player.transform.position,3, pathTemp);


        if (pathTemp.status == NavMeshPathStatus.PathComplete)
        {
            agent.isStopped = false;
            agent.path.ClearCorners();
            MenorVector(player.transform.position);
            distance = Vector3.Distance(player.transform.position, transform.position);
            anim.SetBool("Walking", true);
        }
        else
        {
            distance = Vector3.Distance(startPosi, transform.position);
            Debug.Log(distance);
            if (distance < 0.1f)
            {
                //agent.isStopped = true;
                agent.path.ClearCorners();
                agent.isStopped = true;
                transform.position = startPosi;
                //transform.rotation = Quaternion.Euler(startRotation);
                child.transform.localRotation = Quaternion.RotateTowards(child.transform.localRotation, Quaternion.LookRotation(Vector3.zero), 25 * Time.deltaTime * 15);
                anim.SetBool("Walking", false);
            }
            else {
                agent.isStopped = false;
                agent.path.ClearCorners();
                MenorVector(startPosi);

                anim.SetBool("Walking", true);
            }


        }
        
    }

    void MenorVector(Vector3 vetor) {
        Vector3 target = vetor - transform.position;
        Vector3 newX = new Vector3(target.x, 0, 0);
        Vector3 newZ = new Vector3(0, 0, target.z);
        float distanceX = Vector3.Distance(newX,Vector3.zero);
        float distanceZ = Vector3.Distance(newZ, Vector3.zero);

        newX = newX.normalized * 0.05f;
        newZ = newZ.normalized * 0.05f;
        if (distanceX < 0.1) distanceX = 0;
        if (distanceZ < 0.1) distanceZ = 0;
        if (!NavMesh.CalculatePath(this.transform.position, new Vector3(vetor.x, transform.position.y, transform.position.z), 3, pathTemp))
            newX = Vector3.zero;
        if (!NavMesh.CalculatePath(this.transform.position, new Vector3(transform.position.x, transform.position.y, vetor.z), 3, pathTemp))
            newZ = Vector3.zero;


        if (distanceX < (distanceZ + 0.2) && distanceX != 0 && newX != Vector3.zero || distanceZ == 0)
        {
            target = transform.position + newX;

        }
        if (distanceZ < (distanceX + 0.2) && distanceZ != 0 && newZ != Vector3.zero || distanceX ==0)
        {
            target = transform.position + newZ;

        }
        rotate.transform.LookAt(target, Vector3.up);
        child.transform.rotation = Quaternion.RotateTowards(child.transform.rotation, rotate.transform.rotation, 50 * Time.deltaTime * 15);
        agent.SetDestination(target);

    }

    Vector3 TargetVector(Vector3 vetor) {
        

        return vetor;
    }


    float CalculateDistance()
    {
        float dist = 0;
        if ((agent.path.status != NavMeshPathStatus.PathInvalid))
        {
            for (int i = 1; i < agent.path.corners.Length; ++i)
            {
                dist += Vector3.Distance(agent.path.corners[i - 1], agent.path.corners[i]);
                Debug.DrawLine(agent.path.corners[i - 1], agent.path.corners[i]);
            }
        }
        return dist;
    }

    IEnumerator Disappear()
    {
        Debug.Log("Estou Sumindo");
        int quant = 20;
        float proporc = 0.05f;
        float alpha = 0;

        skinMesh.materials = new Material[4] { material, material, material, material };
        for (int i = 0; i < quant; i++)
        {
            alpha += proporc;
            Debug.Log(alpha);
            material.SetFloat("Amount", alpha);
            yield return new WaitForSeconds(0.05f);
        }
        gameManager.enemysDead.Add(idEnemy);
        Destroy(this.gameObject);
        yield break;
    }

    private void OnCollisionStay(Collision other)
    {
        if (this.tag == "Inimigo/Azul")
            fraqueza = "Escudo";
        else
            fraqueza = "Espada";
    }

    private void OnCollisionEnter(Collision other)
    {
        if (this.tag == "Inimigo/Azul")
            fraqueza = "Escudo";
        else
            fraqueza = "Espada";

        if (other.gameObject.tag == fraqueza) {
            if (!EnemyStatic) {
                agent.path.ClearCorners();
                EnemyStatic = true;
            }
            anim.SetBool("Walking", false);
            Destroy(this.GetComponent<Rigidbody>());
            Destroy(this.GetComponent<CapsuleCollider>());
            StartCoroutine(Disappear());
            if(!semParticulas)
            particulas.enabled = true;
            

        }
            
    }

}
