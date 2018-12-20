using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaCarcelScript : MonoBehaviour {

    Animator miPuerta;
    AudioSource audioPuerta;

    private void Start() {
        miPuerta = GetComponent<Animator>();
        audioPuerta = GetComponent<AudioSource>();
    }
    public void AbrirPuerta() {
        miPuerta.SetBool("Abrir", true);
        // SONIDO PUERTA
        audioPuerta.Play();

        // DESACTVAR LA ANIMACION
        Invoke ("CargarPosicion",1); 
    }

    void CargarPosicion(){
        // CARGAMOS EN EL PLAYPREFS LA POSICION DE LA PUERTA
        GameControler.StorePosicionPuertaCarcel(gameObject.transform.localPosition);
    }
}
