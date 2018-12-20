using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaInteriorScript : MonoBehaviour {

    Animator miPuerta;
    AudioSource audioPuerta;
	// Use this for initialization
	void Start () {
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
        GameControler.StorePosicionPuertaCarcelInterior(gameObject.transform.localPosition);
    }
}
