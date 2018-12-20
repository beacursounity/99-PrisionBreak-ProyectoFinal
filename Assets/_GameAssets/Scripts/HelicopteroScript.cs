using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopteroScript : MonoBehaviour {

    Animator miHelicoptero;
    
    public void Escapar(){

        // DESACTIVAMOS LA ANIMATION DE LOS MOTORES
        GameObject[] Motor;
        Motor = GameObject.FindGameObjectsWithTag("Motor");
        // SI TIENE ELEMENTOS EL ARRAY
        if (Motor.Length > 0) {
            for (int i = 0; i <= Motor.Length - 1; i++) {
                Motor[i].GetComponent<Animator>().enabled = false;
            }
        }
        // ACTIVAMOS LA ANIMACION PRINCIPAL DEL HELICOPTERO
        GetComponent<Animator>().enabled = true;
		// COGEMOS EL ANIMATOR
		Animator miHelicoptero = GetComponent<Animator>();
		// EL SONIDO
		AudioSource miAudio = GetComponent<AudioSource>();
		miAudio.Play();
		// Y LA ANIMACION SE ACTIVA
		miHelicoptero.SetBool("EscaparCarcel",true);
		// FINALIZAR EL JUEGO
		Invoke("FinalizarJuego",2);
	}

	void FinalizarJuego(){	
		Application.Quit();	
	}
	
}
