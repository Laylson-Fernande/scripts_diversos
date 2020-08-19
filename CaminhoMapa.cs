using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaminhoMapa : MonoBehaviour
{
    public Texture2D[] sprite;
    public GameObject[] lights;
    RawImage rawImage;
    int cont = 13;
    // Start is called before the first frame update
    void Start()
    {
        rawImage = this.GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        

        
        for (int i = 12; i > -2; i--) {
            //Debug.Log(i);
            if (i == -1) { rawImage.texture = sprite[0]; break; }
            if (lights[i].GetComponent<LightTrail>().on) {
                rawImage.texture = sprite[i+1];
                //Debug.Log(sprite[i + 1].name);
                break;
            }
            
        }
    }

    
}
