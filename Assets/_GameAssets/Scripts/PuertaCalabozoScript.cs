using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaCalabozoScript : MonoBehaviour {

    Animator miAnimator;

    private void Start() {
        miAnimator = GetComponent<Animator>();
    }

    public void AbrirPuerta() {
        miAnimator.SetBool("Abrir",true);
        // RUIDO DE ABRIR PUERTA
        GetComponent<AudioSource>().Play();
    }
}
