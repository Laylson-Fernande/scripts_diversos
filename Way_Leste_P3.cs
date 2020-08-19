using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Way_Leste_P3 : MonoBehaviour
{
    [SerializeField]
    List<GameObject> goMove = new List<GameObject>();
    [SerializeField]
    List<AudioSource> goAudioSource = new List<AudioSource>();
    [SerializeField]
    List<Vector3[]> goTarget = new List<Vector3[]>();
    [SerializeField]
    List<GameObject> platform = new List<GameObject>();
    [SerializeField]
    GameObject baseRotary;
    [SerializeField]
    Lever lever1;
    [SerializeField]
    Lever lever2;
    [SerializeField]
    Lever lever3;
    [SerializeField]
    PressurePlate plate1;
    [SerializeField]
    PressurePlate plate2;
    [SerializeField]
    PressurePlate plate3;

    public AudioClip[] audios;

    bool playerIsHere;
    bool allPlatformActived;
    bool plate3Active;

    GameManager gameManager;
    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        bool thisParteisComplete = gameManager.progressData.wayLesteP3;
        if (thisParteisComplete)
        {
            GetObjectsOfP3();
            lever1.activated = true;
            lever2.activated = true;
            lever3.activated = true;
        }

    }

    private void Start()
    {
        IfPlayerIsHere();
    }

    // Update is called once per frame
    private void Update()
    {

        if (playerIsHere)
        {
            IfPlayerIsHere();
        }

    }
    void IfPlayerIsHere() {
        //Alavanca 1
        if (lever1.activated) {
            lever1.LeverDisable();
            MoveGos(goMove[0], goTarget[0][0],0);
        }
        //Placa de pressão 1
        if (plate1.activated)
        {
            MoveGos(goMove[1], goTarget[1][1],1);
        }
        else {
            MoveGos(goMove[1], goTarget[1][0],1);
        }
        //Alavanca 2
        if (lever2.activated && !allPlatformActived) {
            lever2.LeverDisable();
            ActivePlatform();
        }
        if (!plate3Active)
        {
            //Peso na plataforma
            if (plate3.count == 0)
            {
                MoveGos(goMove[2], goTarget[2][0],2);
            }
            else if (plate3.count == 1)
            {
                MoveGos(goMove[2], goTarget[2][1],2);
                if (goMove[2].transform.position == goTarget[2][1] && audios[2]) {
                    GetComponent<AudioSource>().PlayOneShot(audios[3]);
                    audios[3] = null;
                }
            }
            else if (plate3.count >= 2)
            {
                plate3Active = true;
            }
        }
        else {
            MoveGos(goMove[2], goTarget[2][2],2);
            if (goMove[2].transform.position == goTarget[2][2])
            {
                MoveGos(goMove[3], goTarget[3][0],3);
                MoveGos(goMove[4], goTarget[4][0],4);
            }
        }
        //Placa de pressão 2
        if (plate2.activated)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(0, -180, 0));
            baseRotary.transform.localRotation = Quaternion.RotateTowards(baseRotary.transform.localRotation, rotation , 30 * Time.deltaTime);
        }
        else
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            baseRotary.transform.localRotation = Quaternion.RotateTowards(baseRotary.transform.localRotation, rotation, 30 * Time.deltaTime);
        }
        //Alavanca 3
        if (lever3.activated) {
            if (!gameManager.progressData.wayLesteP4) {
                gameManager.progressData.wayLesteP3 = true;
                gameManager.UpdateProgressSave();
            }
            lever3.LeverDisable();
            MoveGos(goMove[5], goTarget[5][0],5);
            MoveGos(goMove[6], goTarget[6][0],6);
        }

    }
    void MoveGos(GameObject go,Vector3 target,int i) {
        AudioSource sourceTemp = goAudioSource[i];
        try
        {
            if (!sourceTemp.isPlaying) {
                //sourceTemp.clip = audios[1];
                sourceTemp.spatialBlend = 1;
                sourceTemp.maxDistance = 2;
                sourceTemp.Play();
                Debug.Log("COMEÇOU A TOCAR EM "+go.name);
            }
            
        }
        catch (System.Exception)
        {
        }
        
        Vector3 vector = go.transform.position;
        float distance = Vector3.Distance(go.transform.position, target)*10;
        if (distance < 0.5f) {
            go.transform.position = target;
            try
            {
                if(sourceTemp.isPlaying)
                sourceTemp.Stop();
                Debug.Log("Parou de TOCAR EM " + go.name);
            }
            catch (System.Exception)
            {
            }

        }
        if (go.transform.position != target) {
            Vector3 direction = vector - target;
            go.transform.position -= direction.normalized * 0.5f * Time.deltaTime;
        }
    }
    void ActivePlatform() {
        for (int i = 0; i < platform.Count; i++) {
            platform[i].GetComponentInChildren<Platform>().startPosi = true;
        }
    }

    void GetObjectsOfP3() {
        for (int i = 1; i < 8; i++)
        {
            GameObject temp = GameObject.Find("MoveP3/" + i.ToString());
            if (!goMove.Contains(temp)) {
                goMove.Add(temp);
                goAudioSource.Add(temp.GetComponent<AudioSource>());
            }

            if (i == 3)
            {
                Vector3[] posi = new Vector3[3];
                posi[0] = temp.transform.position;
                posi[1] = GameObject.Find("TargetP3/" + i.ToString()).transform.position;
                posi[2] = GameObject.Find("TargetP3/" + (i + 1).ToString()).transform.position;
                if (!goTarget.Contains(posi)) { goTarget.Add(posi); }
            }
            else if (i > 3)
            {
                Vector3[] posi = new Vector3[1];
                posi[0] = GameObject.Find("TargetP3/" + (i + 1).ToString()).transform.position;
                if (!goTarget.Contains(posi)) { goTarget.Add(posi); }
            }
            else
            {
                if (i == 2)
                {
                    Vector3[] posi = new Vector3[2];
                    posi[0] = temp.transform.position;
                    posi[1] = GameObject.Find("TargetP3/" + i.ToString()).transform.position;
                    if (!goTarget.Contains(posi)) { goTarget.Add(posi); }
                }
                else
                {
                    Vector3[] posi = new Vector3[1];
                    posi[0] = GameObject.Find("TargetP3/" + i.ToString()).transform.position;
                    if (!goTarget.Contains(posi)) { goTarget.Add(posi); }
                }
            }

            if (i < 4)
            {
                GameObject temp3 = GameObject.Find("PlataformaMovelP3/" + i.ToString());
                if (!platform.Contains(temp3)) { platform.Add(temp3); }

            }

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player")&&!playerIsHere)
        {
            playerIsHere = true;
            GetObjectsOfP3();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            playerIsHere = false;
    }
}
