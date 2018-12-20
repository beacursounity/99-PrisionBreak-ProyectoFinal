using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortadaScript : MonoBehaviour {

	GameObject disparo1, disparo2, disparo3;
	AudioSource audiodisparo1, audiodisparo2, audiodisparo3;

	// Use this for initialization
	void Start () {
		// BUSCAMOS LAS REFERENCIAS DE LOS 3 DISPAROS CON SUS AUDIOSOURCE
		disparo1 = GameObject.Find("AgujeroBala1");
		disparo2 = GameObject.Find("AgujeroBala2");
		disparo3 = GameObject.Find("AgujeroBala3");

		// DISPAROS
		Invoke("DisparoSonido1",2.0f);
		Invoke("DisparoSonido3",2.5f);
		Invoke("DisparoSonido2",3.0f);

		// SELECCION DE NIVELES
		Invoke("EscenaSeleccionNiveles",4.0f);	
	}

	void DisparoSonido1(){
		audiodisparo1 = disparo1.GetComponent<AudioSource>();
		audiodisparo1.Play();
		Invoke("Disparo1",0.5f);
	}

	void DisparoSonido2(){
		audiodisparo2 = disparo2.GetComponent<AudioSource>();
		audiodisparo2.Play();
		Invoke("Disparo2",0.5f);
	}
	
	void DisparoSonido3(){
		audiodisparo3 = disparo3.GetComponent<AudioSource>();
		audiodisparo3.Play();
		Invoke("Disparo3",0.5f);
	
	}
	void Disparo1(){
		disparo1.GetComponent<SpriteRenderer>().enabled = true;
	}

	void Disparo2(){
		disparo2.GetComponent<SpriteRenderer>().enabled = true;
	}
	void Disparo3(){
		disparo3.GetComponent<SpriteRenderer>().enabled = true;
	}
	void EscenaSeleccionNiveles () {
		// SE BORRAR LOS PLAYERPREFS
		PlayerPrefs.DeleteAll(); 
		SceneManager.LoadScene(1);
	}
}
