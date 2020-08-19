using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Way_Leste_P5 : MonoBehaviour
{
    List<GameObject> goMove = new List<GameObject>();
    List<Vector3[]> goTarget = new List<Vector3[]>();
    [SerializeField]
    List<GameObject> destroy = new List<GameObject>();
    [SerializeField]
    GameObject scaled;
    [SerializeField]
    Platform platform1;
    [SerializeField]
    Platform platform2;
    [SerializeField]
    PressurePlate plate1;
    [SerializeField]
    PressurePlate plate2;
    [SerializeField]
    Lever lever;

    [SerializeField]
    bool playerIsHere;
    [SerializeField]
    bool spt1 = true;
    [SerializeField]
    bool spt2 = true;
    GameManager gameManager;

    public AudioClip[] audios;
    [SerializeField]
    AudioSource platformSource1, platformSource2;
    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        bool thisParteisComplete = gameManager.progressData.wayLesteP5;
        if (thisParteisComplete)
        {
            playerIsHere = true;
            GetObjectsOfP5();
            lever.activated = true;
            
        }
    }
    void Start()
    {
        StartCoroutine(Rotate());
        platform1.startPosi = true;
        platform2.startPosi = false;
        scaled.transform.localScale = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsHere) IfPlayerIsHere();


    }

    void IfPlayerIsHere() {


        if (lever.activated)
        {
            gameManager.progressData.wayLesteP5 = true;
            gameManager.UpdateProgressSave();
            for (int i = 2; i < 7; i++)
            {
                if (goMove[i].transform.position != goTarget[i][0])
                {
                    MoveGos(goMove[i], goTarget[i][0], false);
                    if (i == 5|| i==6) { RotateGos(goMove[i], new Vector3(0, 0, -90)); }
                }

            }
            if (destroy.Count != 0)
            {
                for (int i = 0; i < destroy.Count; i++)
                {
                    GameObject temp = destroy[i];
                    if (temp.transform.localScale.x >= 0f)
                    {
                        ScaleGos(temp, new Vector3(-0.5f, -0.5f, -0.5f));
                    }
                    else
                    {
                        destroy.Remove(temp);
                        Destroy(temp);
                    }
                }
            }
            if (scaled.transform.localScale.x != 1)
            {
                scaled.transform.localScale = Vector3.Lerp(scaled.transform.localScale,Vector3.one,Time.deltaTime);
            }

        }
        else {
            if (plate1.activated) platform1.startPosi = false;
            else platform1.startPosi = true;

            if (plate2.activated) platform2.startPosi = true;
            else platform2.startPosi = false;

            if (spt1)
            {
                MoveGos(goMove[0], goTarget[0][0], true);
                RotateGos(goMove[0], Vector3.zero);
                if (goMove[0].transform.position != goTarget[0][0])
                {
                    platformSource1.Play();
                }
                else if (platformSource1.isPlaying)
                {
                    if(platformSource1.clip == audios[1])
                        platformSource1.Stop();

                }

            }
            else
            {
                MoveGos(goMove[0], goTarget[0][1], true);
                RotateGos(goMove[0], new Vector3(180, 0, 0));

                if (goMove[0].transform.position != goTarget[0][0])
                {
                    platformSource1.Play();
                }
                else if (goMove[0].GetComponent<AudioSource>().isPlaying)
                {
                    if (platformSource1.clip == audios[1])
                        platformSource1.Stop();

                }
            }

            if (spt2)
            {
                MoveGos(goMove[1], goTarget[1][0], true);
                RotateGos(goMove[1], Vector3.zero);
                if (goMove[1].transform.position != goTarget[1][0])
                {
                    platformSource2.Play();
                }
                else if(platformSource2.isPlaying)
                {
                    if(platformSource2.clip == audios[1])
                    platformSource2.Stop();

                }
            }
            else
            {
                MoveGos(goMove[1], goTarget[1][1], true);
                RotateGos(goMove[1], new Vector3(180, 0, 0));
                if (goMove[1].transform.position != goTarget[1][0])
                {
                    platformSource2.Play();
                }
                else if (platformSource2.isPlaying)
                {
                    if (platformSource2.clip == audios[1])
                        platformSource2.Stop();

                }
            }
        }


    }

    void MoveGos(GameObject go, Vector3 target,bool b)
    {
        AudioSource source = new AudioSource();
        float speed = 0;
        float multi = 0;
        if (b) { speed = 0.175f; multi = 100; }
        else {
            speed = 1f;
            multi = 10;
        }
        Vector3 vector = go.transform.position;
        float distance = Vector3.Distance(go.transform.position, target) * multi;
        if (distance < 0.5f) {
            go.transform.position = target;
            if (!b)
                go.GetComponent<AudioSource>().PlayOneShot(audios[0]);
        }
        if (go.transform.position != target)
        {
            Vector3 direction = vector - target;
            go.transform.position -= direction.normalized * speed * Time.deltaTime;
        }
    }

    void RotateGos(GameObject go, Vector3 vector3) {
        Quaternion rotation = Quaternion.Euler(vector3);
        go.transform.localRotation = Quaternion.RotateTowards(go.transform.localRotation, rotation, 70 * Time.deltaTime);
    }

    void ScaleGos(GameObject go,Vector3 vector) {
        float f = 0.7f * Time.deltaTime;
        Vector3 scale = new Vector3(-f, -f, -f);
        go.transform.localScale += scale;
        if (go.transform.localScale.x <= 0) {
            destroy.Remove(go);
            Destroy(go);
        }


    }

    IEnumerator Rotate() {
        float time = 2.5f;
        yield return new WaitForSeconds(time);
        spt1 = !spt1;
        yield return new WaitForSeconds(time);
        spt2 = !spt2;
        StartCoroutine(Rotate());
        yield break;
    }

    void GetObjectsOfP5() {
        for (int i = 1; i < 9; i++)
        {
            GameObject move = GameObject.Find("MoveP5/" + i.ToString());
            GameObject target;
            Vector3[] vector;
            if (i != 8)
            {
                target = GameObject.Find("TargetP5/" + i.ToString());

                if (i == 1 || i == 2)
                {
                    vector = new Vector3[2];
                    vector[0] = move.transform.position;
                    vector[1] = target.transform.position;

                }
                else
                {
                    vector = new Vector3[1];
                    vector[0] = target.transform.position;
                }
                if (!goTarget.Contains(vector)) { goTarget.Add(vector); }
            }
            if (!goMove.Contains(move)) { goMove.Add(move); }


        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player")&&!playerIsHere) {
            playerIsHere = true;
            GetObjectsOfP5();

        }
    }
}
