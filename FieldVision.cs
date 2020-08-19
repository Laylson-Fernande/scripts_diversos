using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldVision : MonoBehaviour
{
    public Transform cabecaInimigo;
    [Range(1, 50)]
    public float distanciaDeVisao = 10;
    public LayerMask layersPlayer = 2;
    [Range(5, 180)]
    public float anguloDeVisao = 120;
    LayerMask layerObstaculos;
    public bool desenharEsfera = true;
    public bool desenharFov = false;

    public List<Transform> inimigosVisiveis = new List<Transform>();
    List<Transform> listaTemporariaDeColisoes = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        layerObstaculos = ~layersPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        Vision();
       
    }

    void Vision() {
        
            Collider[] alvosNoRaioDeAlcance = Physics.OverlapSphere(cabecaInimigo.position, distanciaDeVisao, layersPlayer);
            foreach (Collider targetCollider in alvosNoRaioDeAlcance)
            {
                Transform alvo = targetCollider.transform;
                Vector3 direcaoDoAlvo = (alvo.position - cabecaInimigo.position).normalized;
                if (Vector3.Angle(cabecaInimigo.forward, direcaoDoAlvo) < (anguloDeVisao / 2.0f))
                {
                    float distanciaDoAlvo = Vector3.Distance(transform.position, alvo.position);
                    if (!Physics.Raycast(cabecaInimigo.position, direcaoDoAlvo, distanciaDoAlvo, layerObstaculos))
                    {
                        if (!alvo.transform.IsChildOf(cabecaInimigo.root))
                        {
                            if (!listaTemporariaDeColisoes.Contains(alvo))
                            {
                                listaTemporariaDeColisoes.Add(alvo);
                            }
                            if (!inimigosVisiveis.Contains(alvo))
                            {
                                inimigosVisiveis.Add(alvo);
                            }
                        }
                    }
                }
            }
            for (int x = 0; x < inimigosVisiveis.Count; x++)
            {
                //Debug.DrawLine(cabecaInimigo.position, inimigosVisiveis[x].position, Color.blue);
            }
        
        //remove da lista inimigos que não estão visiveis
        for (int x = 0; x < inimigosVisiveis.Count; x++)
        {
            if (!listaTemporariaDeColisoes.Contains(inimigosVisiveis[x]))
            {
                inimigosVisiveis.Remove(inimigosVisiveis[x]);
            }
        }
        listaTemporariaDeColisoes.Clear();
    }
    
    
}
