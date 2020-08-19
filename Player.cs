using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    bool mobile;
    [SerializeField]
    GameObject cam;
    Animator anim;
    public GameObject body;
    Rigidbody rb;
    [SerializeField]
    LayerMask layer;
    [SerializeField]
    GameObject shieldPrefab;
    [SerializeField]
    GameObject swordPrefab;
    [SerializeField]
    GameObject shield;
    GameObject sword;
    float inputShield;
    float inputSword;

    //public GameObject ataque;
    //public GameObject defesa;

    //public GameObject btAtivar;
    //public GameObject girarR;
    //public GameObject girarL;
    //public GameObject escudo;
    //public GameObject pulo;

    public GameObject interage;
    public string nameInterage = "";

    public enum StatusPlayer { idle, walk, jump }
    public StatusPlayer status = StatusPlayer.idle;
    float timeInIdle;
    public enum PositionCam { front, back, right, left }
    public PositionCam posiCam = PositionCam.front;

    public bool onGround;
    int tempoNoChao;
    BoxHold boxHold;
    int holdingBox = 0;

    public float speed = 1.5f;

    public float distancia = 0;

    public AudioClip[] audios;
    int contDead = 3;

    float t;
    [SerializeField]
    Material[] materials;
    [SerializeField]
    public SkinnedMeshRenderer skinMesh;
    public bool dissolver;
    [Range(0f, 1f)]
    public float dissolveCount;

    public GameObject heartHands, heartBody,fxHeart;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");

        t = Time.time + 1;
        skinMesh.materials = new Material[4] { materials[1], materials[1], materials[1], materials[1] };
        
        MeshRenderer[] meshHearts = heartBody.GetComponentsInChildren<MeshRenderer>();
        meshHearts[0].material = materials[2];
        meshHearts[1].material = materials[2];
        materials[1].SetFloat("Amount", 1);
        materials[2].SetFloat("Amount", 1);
        if (gameManager.progressData.animHeart1)
        {
            Destroy(heartHands);
            heartBody.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

        gameManager.progressData.positionCam = posiCam.ToString();
        AnimationsManager();
        if (status != StatusPlayer.jump && !onGround)
        {

            //anim.SetInteger("chao", 1);
        }


    }

    public void Jump()
    {
        if (onGround)
        {
            Rigidbody rb = this.GetComponent<Rigidbody>();
            onGround = false;
            status = StatusPlayer.jump;
            anim.SetInteger("chao", 1);
            rb.AddForce(0, 3.7f * rb.mass, 0, ForceMode.Impulse);
        }
    }
    public void Shield()
    {
        if (shield == null)
            shield = Instantiate(shieldPrefab, transform.position, Quaternion.Euler(transform.eulerAngles));
    }
    public void HoldShield()
    {
        if (shield == null)
        {
            shield = Instantiate(shieldPrefab, transform.position, Quaternion.Euler(transform.eulerAngles));
            shield.GetComponent<Shield>().defense = false;

            //try
            //{
            //    GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraTutorial>().HoldShild();
            //}
            //catch (System.Exception)
            //{

            //}

        }
        else
        {
            Destroy(shield);
        }
    }
    public void MovePlayer(Vector2 input)
    {
        //Vector2 input = new Vector2(MSJoystickController.joystickInput.x, MSJoystickController.joystickInput.y);
        Vector3 movement = Vector3.zero;
        switch (posiCam)
        {
            case PositionCam.front:
                movement = new Vector3(input.x, 0, input.y);
                break;
            case PositionCam.right:
                movement = new Vector3(-input.y, 0, input.x);
                break;
            case PositionCam.left:
                movement = new Vector3(input.y, 0, -input.x);
                break;
            case PositionCam.back:
                movement = new Vector3(-input.x, 0, -input.y);
                break;
        }
        if (movement != Vector3.zero && holdingBox == 0)
        {
            if (status != StatusPlayer.jump) { status = StatusPlayer.walk; }
            transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(movement), 50 * Time.deltaTime * 15);
            body.transform.localRotation = Quaternion.Euler(Vector3.zero);
            body.transform.localPosition = Vector3.zero;
        }
        else
        {
            if (status != StatusPlayer.jump) { status = StatusPlayer.idle; }
        }
        movement = movement.normalized * speed * Time.deltaTime;
        if (holdingBox != 1)
        {
            
            transform.position += movement;

            //rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
            //transform.Translate(movement.normalized * speed * Time.deltaTime);
            if (holdingBox == 2) { boxHold.Move(movement); }
        }
    }
    public void Ativar()
    {
        interage.GetComponent<Lever>().moving = true;
    }
    public void Girar(bool b)
    {
        interage.GetComponent<MiniPilar>().Ativar(b);
    }


    void AnimationsManager()
    {

        switch (status)
        {
            case StatusPlayer.idle:
                if (timeInIdle < 15) { timeInIdle += Time.deltaTime; anim.SetInteger("status", 0); }
                else { anim.SetInteger("status", 1); }
                break;
            case StatusPlayer.walk:
                anim.SetInteger("status", 2);
                timeInIdle = 0;
                break;
            case StatusPlayer.jump:
                anim.SetInteger("status", 3);
                timeInIdle = 0;
                break;
        }
    }

    public void BateuChao()
    {
        if (status != StatusPlayer.walk)
        {
            anim.SetInteger("chao", 2);
            status = StatusPlayer.idle;
        }
    }
    public void RespawnPlayer() {
        contDead++;
        if(contDead >= 3)
        {
            contDead = 0;
            GetComponent<AudioSource>().PlayOneShot(audios[Random.Range(0, 3)]);
        }
        VoidStartCoroutines("Appear");
        transform.position = gameManager.progressData.lesteSavePositon;
    }
    public void VoidStartCoroutines(string nameCo)
    {
        switch (nameCo)
        {
            case "Disappear":
                StartCoroutine(Disappear());
                break;
            case "Appear":
                StartCoroutine(Appear());
                break;
            case "CamPlayerMoreNear":
                StartCoroutine("CamPlayerMoreNear");
                break;
            case "CamPlayerLessNear":
                StartCoroutine("CamPlayerLessNear");
                break;
        }
    }
    IEnumerator Disappear()
    {
        float contar = Time.time;
        Debug.Log("Estou Sumindo");
        int quant = 20;
        float proporc = 0.1f;
        float alpha = 0;

        skinMesh.materials = new Material[4] { materials[1], materials[1], materials[1], materials[1] };
        for (int i = 0; i < quant; i++)
        {
            alpha += proporc;
            materials[1].SetFloat("Amount", alpha);
            materials[2].SetFloat("Amount", alpha);
            yield return new WaitForSeconds(0.07f);
        }
        Debug.Log("O player demorar para sumir "+(Time.time - contar));
        yield break;
    }
    IEnumerator Appear()
    {
        int quant = 20;
        float proporc = 0.05f;
        float alpha = 1;

        for (int i = 0; i < quant; i++)
        {
            alpha -= proporc;
            materials[1].SetFloat("Amount", alpha);
            materials[2].SetFloat("Amount", alpha);
            yield return new WaitForSeconds(0.07f);
        }
        skinMesh.materials = new Material[4] { materials[0], materials[0], materials[0], materials[0] };
        
        yield break;
    }

    IEnumerator CamPlayerMoreNear()
    {
        Debug.Log("Mais near na camera agora");
        float contar = Time.time;
        float proporc = 0.15f;
        float near = 0.5f;
        Camera camPlayerTemp = gameManager.camPlayer.GetComponent<Camera>();

        while(near < 14)
        {
            near += proporc;
            camPlayerTemp.nearClipPlane = near;
            yield return new WaitForSeconds(0.01f);
        }

        Debug.Log("Tempo MoreNear camera "+(Time.time - contar));
        yield break;
    }
    IEnumerator CamPlayerLessNear()
    {
        Debug.Log("Menos near na camera agora");
        float contar = Time.time;
        float proporc = 0.1f;
        float near = 15f;
        Camera camPlayerTemp = gameManager.camPlayer.GetComponent<Camera>();
        while (near>0.3f) {
            near -= proporc;
            camPlayerTemp.nearClipPlane = near;
            yield return new WaitForSeconds(0.01f);
        }
        Debug.Log("Tempo LessNear camera " + (Time.time - contar));
        yield break;
    }

    #region COLLISION
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            gameManager.progressData.lesteSavePositon = other.gameObject.transform.position;
            Destroy(other.gameObject);
        }

        string collName = other.gameObject.name;
        if (other.gameObject.CompareTag("Limite"))
        {
            if (cam == null)
            {
                cam = GameObject.FindGameObjectWithTag("MainCamera");
            }
            cam.GetComponent<Cam>().ChangePosi(collName);

        }

        //if (other.gameObject.CompareTag("Alavanca")) {
        //    interage = other.gameObject;
        //    nameInterage = other.gameObject.tag;
        //}else if (other.gameObject.CompareTag("MiniPilar"))
        //{
        //    interage = other.gameObject;
        //    nameInterage = "MiniPilar";
        //}

    }
    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.CompareTag("Alavanca") || other.gameObject.CompareTag("MiniPilar"))
        //{ nameInterage = "";}

    }
    private void OnCollisionEnter(Collision collision)
    {
        string collTag = collision.gameObject.tag;
        //if (collision.gameObject.CompareTag("Limite")) {

        //    cam.GetComponent<Cam>().ChangePosi(collision.gameObject.name);
        //}
        if (collTag == "Inimigo/Vermelho" || collTag == "Inimigo/Azul")
        {
            VoidStartCoroutines("Disappear");
            Invoke("RespawnPlayer",0.9f);
            
        }
        else if(collision.gameObject.CompareTag("limbo"))
        {
            //cam.GetComponent<Cam>().Respawn();
            skinMesh.materials = new Material[4] { materials[1], materials[1], materials[1], materials[1] };
            RespawnPlayer();
        }
        else if (collision.gameObject.name == "CutSceneManager")
        {
            skinMesh.materials = new Material[4] { materials[1], materials[1], materials[1], materials[1] };
            transform.position = collision.gameObject.GetComponent<CutScene>().posiPlayer;
            StartCoroutine(Appear());
            //StartCoroutine(Disappear());

        }
    }
    #endregion

    #region GETS AND SETS
    #endregion
}