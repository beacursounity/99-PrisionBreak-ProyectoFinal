using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CofreScript : MonoBehaviour {

	Animator miCofre;

	[SerializeField] GameObject player;
    [SerializeField] GameObject pistola;
    [SerializeField] GameObject puerta;

    public bool recogidaPistaCofre = false;

	//
	int puntuacion = 10;

	// Use this for initialization
	void Start () {
		miCofre = transform.parent.GetComponent<Animator>();
        miCofre.SetInteger("Abrir", 0);
	}

    // AL HACER CLICK SOBRE EL GO DEL CERROJO ABRIRA EL COFRE
    private void OnMouseDown() {
        AbrirCofre();
    }


	public void AbrirCofre () {

        GetComponent<MeshRenderer>().enabled = false;    
        miCofre.SetInteger("Abrir", 1);

        // VAMOS HACER QUE SALGAN PARTICULAS 
        Invoke("LanzarParticulas", 0.5f);

        // INCREMENTAMOS PUNTOS POR COGER LA PISTA DEL BAUL !!!!!!!!!!! PROBARLO CUANDO FUNCIONE
        player.GetComponent<PlayerScript>().IncrementarPuntuacion(puntuacion);
        // EL PLAYER COGE LA PISTOLA    
        Invoke("QuitarAnimacion", 3);
        // EL PLAYER COGE LA PISTOLA    
        Invoke("CogerPistola", 3);
        // CERRAMOS EL COFRE Y YA NO SE VOLVERA ABRIR
        Invoke("CerrarCofre", 4);
      
    }

     private void QuitarAnimacion() {
        // DESACTIVA ANIMATION DE A ISTOLA
        pistola.GetComponent<PistolaScript>().QuitarAnimation();
   }

    private void CogerPistola() {
        player.GetComponent<PlayerScript>().CogerPistola();
        // ESCONDEMOS LA PISTOLA
        Invoke("EsconderPistola",0.5f);
        
    }
    private void EsconderPistola() { 
        // ESCONDEMOS LA PISTOLA
        pistola.GetComponent<PistolaScript>().EsconderPistola();
    }   

    private void LanzarParticulas() {
         //VAMOS HACER QUE SUENE
        gameObject.GetComponent<AudioSource>().Play();
        // BUSCAMOS LAS PARTICULAS QUE ESTAN COMO HIJO Y LANZA PARTICULAS
        gameObject.transform.Find("ParticulasCofre").GetComponent<ParticleSystem>().Play();
        
    }

    public void CerrarCofre () {
        puerta.GetComponent<PuertaCarcelScript>().AbrirPuerta();
        recogidaPistaCofre = true;
		miCofre.SetInteger("Abrir", 2);

      

	}

    public bool GetRecogerPista() {
        return recogidaPistaCofre;
    }

}
