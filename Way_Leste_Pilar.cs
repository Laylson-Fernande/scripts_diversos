using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Way_Leste_Pilar : MonoBehaviour
{
    [SerializeField]
    Lever lever;
    [SerializeField]
    PillarLight pilar1;
    [SerializeField]
    PillarLight pilar2;
    [SerializeField]
    GameObject[] go = new GameObject[3];
    CameraLeste cam;
    [SerializeField]
    PillarLight pillarLight;

    public AudioClip[] audios;

    float cont = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraLeste>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lever.activated) {
            if (!go[0].GetComponent<AudioSource>().isPlaying)
                go[0].GetComponent<AudioSource>().Play();
            Vector3 vector = go[0].transform.position;
            Vector3 target = go[1].transform.position;
            float distance = Vector3.Distance(go[0].transform.position, target) * 10;
            if (distance < 0.5f) {
                go[0].transform.position = target;
                pilar1.activated = true;
                go[0].GetComponent<AudioSource>().Stop();
            }
            if (go[0].transform.position != target)
            {
                Vector3 direction = vector - target;
                go[0].transform.position -= direction.normalized * 0.5f * Time.deltaTime;
            }
            else if (!pillarLight.activated) { pillarLight.activated = true; }
            

        }
        if (pilar2.activated)
        {
            if (cont == 0)
                cont = Time.time + 2;
            if (cont < Time.time && go[2].active == false) {
                go[2].SetActive(true);
                cam.Posi = CameraLeste.PosiCam.Pilar2;
                GetComponent<AudioSource>().PlayOneShot(audios[0]);
            }

        }
        else {
            cont = 0;
            go[2].SetActive(false);
            if(cam.Posi == CameraLeste.PosiCam.Pilar2)
                cam.Posi = CameraLeste.PosiCam.Pilar1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(audios[1] != null)
        {

            GetComponent<AudioSource>().PlayOneShot(audios[1]);
            audios[1] = null;
        }
    }
}
