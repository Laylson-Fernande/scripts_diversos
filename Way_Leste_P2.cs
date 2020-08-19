using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Way_Leste_P2 : MonoBehaviour
{
    [SerializeField]
    //List<Bridge> bridge = new List<Bridge>();
    GameObject[] bridge;
    [SerializeField]
    List<GameObject> explosions = new List<GameObject>();
    [SerializeField]
    List<GameObject> platform = new List<GameObject>();
    [SerializeField]
    List<Platform> platformScript = new List<Platform>();
    bool playerIsHere;
    bool complete;
    [SerializeField]
    Lever lever;
    GameManager gameManager;
    float wait;
    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        bool thisParteisComplete = gameManager.progressData.wayLesteP2;
        if (thisParteisComplete)
        {
            GetObjectsOfP2();
            lever.activated = true;
        }

        
    }

    private void Update()
    {

        if (playerIsHere)
        {
            IfPLayerIsHere();
        }

    }

    void IfPLayerIsHere() {
        if (lever.activated)
        {
            if (!gameManager.progressData.wayLesteP3)
            {
                gameManager.progressData.wayLesteP2 = true;
                gameManager.UpdateProgressSave();
            }
            lever.LeverDisable();
            if(bridge.Length == 0)
                bridge = GameObject.FindGameObjectsWithTag("Bridge/3");
            if (complete && bridge.Length > 1)
            {
                for (int i = 0; i < bridge.Length; i++)
                {
                    try
                    {
                        bridge[i].GetComponent<Bridge>().enabled = true;
                        bridge[i].GetComponent<Bridge>().ForeverAtive = true;
                    }
                    catch (System.Exception)
                    {

                    }
                    
                }
                for (int i = 0; i < explosions.Count; i++) {
                    Destroy(explosions[i]);
                }
                explosions.Clear();
                bridge = new GameObject[1];
            }
            else { PlatformsToStart(); }
        }
        else { ManagerPlatforms(); }
    }
    void PlatformsToStart() {
        for (int i = 0; i < platformScript.Count; i++){
            if (!platformScript[i].startPosi) { platformScript[i].startPosi = true; break; }

            if (i==platformScript.Count-1 && !platformScript[i].moving) { complete = true;}
        }
    }
    bool PlatformsStop() {
        bool b = false;
        for (int i = 0; i < platformScript.Count; i++) {
            if (!platformScript[i].startPosi)
            {
                break;
            }
            else if (platformScript[i].moving) { break; }
            if (i == platform.Count - 1) { b = true; }
        }
        return b;
    }
    void ManagerPlatforms() {

        if (platformScript[0].startPosi && !platformScript[0].moving)
        {
            StartCoroutine(ChangeBool(platformScript[0],false, 0.5f));
            StartCoroutine(ChangeBool(platformScript[1],false, 0.5f));
            StartCoroutine(ChangeBool(platformScript[2],true, 0.5f));
            //platformScript[0].startPosi = false;
            //platformScript[1].startPosi = false;
            

        }
        else if (!platformScript[0].startPosi && !platformScript[0].moving) {
            StartCoroutine(ChangeBool(platformScript[0],true, 0.5f));
            StartCoroutine(ChangeBool(platformScript[1],true, 0.5f));
            StartCoroutine(ChangeBool(platformScript[2],false, 0.5f));
           

        }

        if (platformScript[2].startPosi && !platformScript[2].moving)
        {
            //platformScript[2].startPosi = false;
            platformScript[3].startPosi = false;
        }
        else if (!platformScript[2].startPosi && !platformScript[2].moving) {
            //platformScript[2].startPosi = true;
            platformScript[3].startPosi = true;
        }

        if (platformScript[4].startPosi && !platformScript[4].moving) {
            StartCoroutine(ChangeBool(platformScript[4],false, 0.5f));
            StartCoroutine(ChangeBool(platformScript[5],false, 0.5f));
        }
        else if (!platformScript[4].startPosi && !platformScript[4].moving)
        {
            StartCoroutine(ChangeBool(platformScript[4],true, 0.5f));
            StartCoroutine(ChangeBool(platformScript[5],true, 0.5f));
        }
    }
    IEnumerator ChangeBool(Platform ps,bool b ,float wait) {
        yield return new WaitForSeconds(wait);
        if (!ps.moving) ps.startPosi = b;
        yield break;
    }

    void GetObjectsOfP2() {
        //GameObject[] temp1 = GameObject.FindGameObjectsWithTag("Bridge/3");
        //for (int i = 0; i < temp1.Length; i++)
        //{
        //    if (!bridge.Contains(temp1[i].GetComponent<Bridge>())) { bridge.Add(temp1[i].GetComponent<Bridge>()); }
        //    try
        //    {
        //        //bridge[i].enabled = true;
        //    }
        //    catch (System.Exception)
        //    {

        //    }
        //}
        for (int i = 1; i < 7; i++)
        {
            GameObject temp2 = GameObject.Find("PlataformaMovelP2/" + i.ToString());
            if (!platform.Contains(temp2))
            {
                platform.Add(temp2);
                platformScript.Insert(i - 1, temp2.GetComponentInChildren<Platform>());
            }
            try
            {
                temp2.GetComponentInChildren<Platform>().enabled = true;
                temp2.GetComponentInChildren<Platform>().startPosi = true;
            }
            catch (System.Exception)
            {

            }
        }
        platform.Clear();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player")&&!playerIsHere)
        {
            playerIsHere = true;
            GetObjectsOfP2();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsHere = false;
            platform.Clear();
        }
    }
}
