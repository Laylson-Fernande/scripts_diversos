using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Way_Leste_P1 : MonoBehaviour
{
    List<GameObject> bridge1= new List<GameObject>();
    List<GameObject> bridge2 = new List<GameObject>();
    bool playerIsHere;
    bool complete;
    [SerializeField]
    PressurePlate plate;
    [SerializeField]
    Lever lever;
    GameManager gameManager;
    float wait;
    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        bool thisParteisComplete = gameManager.progressData.wayLesteP1;
        if (thisParteisComplete) {
            GetObjectsOfP1();
            lever.activated = true;
        }
        /*
        partsStartPosition = new Vector4[bridgeParts.Length];
        for (int i = 0; i < bridgeParts.Length; i++)
        {
            Vector3 position = bridgeParts[i].transform.localPosition;
            float distance = Vector3.Distance(position, Vector3.zero);

            position = position.normalized;
            partsStartPosition[i] = new Vector4(position.x, position.y, position.z, distance);
            bridgeParts[i].transform.localRotation = Quaternion.Euler(new Vector3(45, 45, 45));
        }
        */
    }

    private void Start()
    {
        IfPlayerIsHere();
    }

    private void Update()
    {

        if (playerIsHere && wait < Time.time) {
            wait = Time.time + 0.5f;
            IfPlayerIsHere();
        }
        
    }
    void IfPlayerIsHere() {
        Debug.Log("ATUALIZANDO PARTE 1");
        if (!complete)
        {
            if (lever.activated) { complete = true; lever.LeverDisable(); }
            else
            {
                if (plate.activated)
                {
                    for (int i = 0; i < bridge1.Count; i++)
                    {
                        bridge1[i].GetComponent<Bridge>().Actived = true;
                    }
                }
                else
                {
                    for (int i = 0; i < bridge1.Count; i++)
                    {
                        bridge1[i].GetComponent<Bridge>().Actived = false;
                    }
                }
            }
        }
        else if (lever != null)
        {

            if (!gameManager.progressData.wayLesteP2)
            {
                gameManager.progressData.wayLesteP1 = true;
                gameManager.UpdateProgressSave();
            }
            lever = null;
            plate = null;
            for (int i = 0; i < bridge1.Count; i++)
            {
                bridge1[i].GetComponent<Bridge>().ForeverAtive = true;
                bridge2[i].GetComponent<Bridge>().enabled = true;
                bridge2[i].GetComponent<Bridge>().ForeverAtive = true;
            }

            bridge1.Clear();
            bridge2.Clear();
        }

    }

    void GetObjectsOfP1() {
        GameObject[] temp1 = GameObject.FindGameObjectsWithTag("Bridge/1");
        GameObject[] temp2 = GameObject.FindGameObjectsWithTag("Bridge/2");
        for (int i = 0; i < temp1.Length; i++)
        {
            if (!bridge1.Contains(temp1[i])) { bridge1.Add(temp1[i]); }
            if (!bridge2.Contains(temp2[i])) { bridge2.Add(temp2[i]); }
            try
            {
                temp1[i].GetComponent<Bridge>().enabled = true;
                //temp2[i].GetComponent<Bridge>().enabled = true;
            }
            catch (System.Exception)
            {

            }


        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !playerIsHere) {
            IfPlayerIsHere();
            playerIsHere = true;
            GetObjectsOfP1();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            playerIsHere = false;
            bridge1.Clear();
            bridge2.Clear();
        }
    }
}
    /*
    void PartsFlying()
    {
        for (int i = 0; i < bridgeParts.Length; i++)
        {
            Vector3 direction = new Vector3(partsStartPosition[i].x, partsStartPosition[i].y, partsStartPosition[i].z);
            float startDistance = partsStartPosition[i].w;
            float currentDistnance = Vector3.Distance(bridgeParts[i].transform.localPosition, Vector3.zero);
           
            if (currentDistnance < startDistance)
            {
                
                bridgeParts[i].transform.position += direction.normalized * 1.5f * Time.deltaTime;

                bridgeParts[i].transform.localRotation = Quaternion.Euler(new Vector3(45,45,45));

            }
            else
            {
                Vector3 rotation = bridgeParts[i].transform.localRotation.eulerAngles + new Vector3(0.5f,0,0);
                bridgeParts[i].transform.localRotation = Quaternion.Euler(rotation);
                rotation += new Vector3(0,0.5f,0);
                bridgeParts[i].transform.localRotation = Quaternion.Euler(rotation);
                rotation += new Vector3(0, 0, 0.5f);
                bridgeParts[i].transform.localRotation = Quaternion.Euler(rotation);


            }
           
        }
    }

    Vector3 RandoRotate(Vector3 vector) {
        Vector3 rotation = vector;
        int i = Random.Range(0, 2);
        switch (i) {
            case 0: rotation += Vector3.up;
                break;
            case 1: rotation += Vector3.right;
                break;
            case 2: rotation += Vector3.back;
                break;
        }
        return rotation;
    }

        void JoinParts()
        {
            for (int i = 0; i < bridgeParts.Length; i++)
            {
            
            Vector3 direction = new Vector3(partsStartPosition[i].x, partsStartPosition[i].y, partsStartPosition[i].z);
            
            float currentDistnance = Vector3.Distance(bridgeParts[i].transform.localPosition, Vector3.zero);
            if (currentDistnance > 0.5f)
            {

                bridgeParts[i].transform.localPosition -= direction.normalized * 10f * Time.deltaTime;

            }
            else { bridgeParts[i].transform.localPosition = Vector3.zero; }
                Vector3 rotation = bridgeParts[i].transform.localRotation.eulerAngles;
                if (rotation != Vector3.zero)
                {
                if (bridgeParts[i].transform.localPosition == Vector3.zero) {
                    bridgeParts[i].transform.localRotation = Quaternion.Euler(Vector3.zero);
                }
                else
                {
                    rotation = new Vector3(ToZero(rotation.x), ToZero(rotation.y), ToZero(rotation.z));
                    bridgeParts[i].transform.localRotation = Quaternion.Euler(rotation);
                }
                }
            }
        }

        float ToZero(float f)
        {
            float vetor = f;
            if (vetor > 0.5) { vetor -= 0.5f; }
            else if (vetor < -0.5) { vetor += 0.5f; }
            else if (vetor < 0.5 && vetor > -0.5) { vetor = 0; }
            return vetor;
        }
    
}
*/