using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Way_Leste_P4 : MonoBehaviour
{
    [SerializeField]
    Camera camThisPart;
    [SerializeField]
    List<Bridge> bridge1 = new List<Bridge>();
    List<Bridge> bridge2 = new List<Bridge>();
    List<GameObject> goMove = new List<GameObject>();
    List<Vector3[]> goTarget = new List<Vector3[]>();
    [SerializeField]
    Platform platform;
    [SerializeField]
    Lever lever1;
    [SerializeField]
    Lever lever2;
    [SerializeField]
    PressurePlate plate1;
    [SerializeField]
    PressurePlate plate2;
    [SerializeField]
    PressurePlate plate3;
    [SerializeField]
    bool playerIsHere;
    GameManager gameManager;
    [SerializeField]
    GameObject[] enemys;
    public AudioClip[] audios;
    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        bool thisParteisComplete = gameManager.progressData.wayLesteP4;
        if (thisParteisComplete)
        {
            playerIsHere = true;
            GetObjectsOfP4();
            lever1.activated = true;
            lever2.activated = true;
        }
        plate1.isPressurePlate = false;
        plate2.isPressurePlate = false;
        plate3.isPressurePlate = false;

        plate1.activated = true;
        plate1.activated = true;
        plate1.activated = true;

       

    }

    private void Start()
    {
        IfPlayerIsHere();
    }

    private void Update()
    {
        if (playerIsHere) {
            IfPlayerIsHere();
        }
    }

    void IfPlayerIsHere() {

        if (enemys[0] == null)
            plate1.isPressurePlate = true;
        if (enemys[1] == null)
            plate2.isPressurePlate = true;
        if (enemys[2] == null)
            plate3.isPressurePlate = true;

        if (plate1.activated) { MoveGos(goMove[0], goTarget[0][0]); }
        else { MoveGos(goMove[0], goTarget[0][1]); }

        if (!plate2.activated)
        {
            if (platform.startPosi && !platform.moving) { platform.startPosi = false; }
            else if (!platform.startPosi && !platform.moving) { platform.startPosi = true;  }
        }
        else { platform.startPosi = true; platform.moving = false; }

        if (plate3.activated) {
            MoveGos(goMove[1], goTarget[1][0]);
            if (goMove[1].transform.position != goTarget[1][0])
            {
                camThisPart.enabled = true;    
            }
            else {
                camThisPart.enabled = false;
            }

        }
        else {
            MoveGos(goMove[1], goTarget[1][1]);
            if (goMove[1].transform.position != goTarget[1][1])
            {
                camThisPart.enabled = true;
            }
            else
            {
                camThisPart.enabled = false;
            }
        }

        if (lever1.activated && bridge1.Count != 0) {
            for (int i=0;i<bridge1.Count;i++) {
                bridge1[i].enabled = true;
                bridge1[i].ForeverAtive = true;
            }
            bridge1.Clear();
        }
        if (lever2.activated && bridge2.Count != 0) {
            if (!gameManager.progressData.wayLesteP5)
            {
                gameManager.progressData.wayLesteP4 = true;
                gameManager.UpdateProgressSave();
            }
            for (int i = 0; i < bridge2.Count; i++)
            {
                bridge2[i].enabled = true;
                bridge2[i].ForeverAtive = true;
            }
            bridge2.Clear();
        }

    }
    void MoveGos(GameObject go, Vector3 target)
    {
        AudioSource sourceTemp = new AudioSource();
        try
        {
            sourceTemp = go.GetComponent<AudioSource>();
            if (!sourceTemp.isPlaying)
                sourceTemp.Play();
            sourceTemp.spatialBlend = 1;
            sourceTemp.maxDistance = 2;
        }
        catch (System.Exception)
        {
        }
        Vector3 vector = go.transform.position;
        float distance = Vector3.Distance(go.transform.position, target) * 10;
        if (distance < 0.5f) {
            go.transform.position = target;
            if (sourceTemp.isPlaying)
                sourceTemp.Stop();
        }
        if (go.transform.position != target)
        {
            Vector3 direction = vector - target;
            go.transform.position -= direction.normalized * 0.5f * Time.deltaTime;
        }
    }
    void GetObjectsOfP4() {
        GameObject[] temp1 = GameObject.FindGameObjectsWithTag("Bridge/4");
        GameObject[] temp2 = GameObject.FindGameObjectsWithTag("Bridge/5");
        List<GameObject> temp3 = new List<GameObject>();
        for (int i = 0; i < temp1.Length; i++)
        {
            if (!temp3.Contains(temp1[i]))
            {
                temp3.Add(temp1[i]);
                bridge1.Add(temp1[i].GetComponentInChildren<Bridge>());
                //temp1[i].GetComponentInChildren<Bridge>().enabled = true;
            }
        }
        for (int i = 0; i < temp2.Length; i++)
        {
            if (!temp3.Contains(temp2[i]))
            {
                temp3.Add(temp2[i]);
                bridge2.Add(temp2[i].GetComponentInChildren<Bridge>());
                //temp2[i].GetComponentInChildren<Bridge>().enabled = true;
            }
        }
        for (int i = 1; i < 3; i++)
        {
            GameObject move = GameObject.Find("MoveP4/" + i.ToString());
            GameObject target = GameObject.Find("TargetP4/" + i.ToString());
            Vector3[] vector = new Vector3[2];
            if (!goMove.Contains(move)) { goMove.Add(move); }
            vector[0] = move.transform.position;
            vector[1] = target.transform.position;
            if (!goTarget.Contains(vector)) { goTarget.Add(vector); }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player")&&!playerIsHere) {
            playerIsHere = true;
            GetObjectsOfP4();

            try
            {
                GetComponent<AudioSource>().Play();
                Destroy(GetComponent<AudioSource>(), 8);
            }
            catch (System.Exception)
            {

                throw;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {

        }
    }
}
