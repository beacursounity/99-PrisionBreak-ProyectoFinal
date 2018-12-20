using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VigilanteEstaticoScript : MonoBehaviour {

    int comienzo = 3;
    int repetircada = 6;

    Animator miAnimator;

    [SerializeField] GameObject player;

    GameObject pistola;

    // VIDAS POR CADA POLICIA 5 LUEGO HAY QUE PONERLO
    int vidas = 3;

    // BALA
    public Transform puntoGeneracionBala;
    public GameObject prefabBala;
    int fuerzaDisparo = 100;

    // PUNTOS POR MATAR A UN POLICIA
    int puntosMatarPolicia = 10;
    //
    AudioSource gritoVigilante;
    ParticleSystem particulasSangreVigilante;
    [SerializeField] GameObject prefabSangreSuelo;

    // Use this for initialization
    void Start () {
        miAnimator = GetComponent<Animator>();
        // COGEMOS LA REFERENCIA DE LA PISTOLA
        pistola = transform.Find("PistolaVigilante").gameObject;
        // RECOJO EL GRITO DEL VIGILANTE
        gritoVigilante = GetComponent<AudioSource>();
        particulasSangreVigilante = GetComponent<ParticleSystem>();
        // 
        InvokeRepeating("Disparar", comienzo , repetircada);	
    }
	
	
	void Disparar () {
        // MIRA AL PLAYER
        transform.LookAt(player.transform);
        //
        pistola.SetActive(true);
        pistola.GetComponent<AudioSource>().Play();
        miAnimator.SetBool("Disparar", true);
        // LANZAMOS LA BALA
        Invoke("LanzarBala",1);
    }


    private void LanzarBala() {
        GameObject nuevaBala = Instantiate(prefabBala, puntoGeneracionBala.position, puntoGeneracionBala.rotation);
        // TIENE QUE TENER UN RIGID PARA APLICAR UNA FUERZA
        nuevaBala.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * fuerzaDisparo);
        //
        Destroy(nuevaBala, 2);
        // PARA QUE DE TIEMPO A QUE SE DISPARE Y SE QUITE LA ANIMACION
        Invoke("PararAnimacion", 1);

    }

    private void PararAnimacion() {
        miAnimator.SetBool("Disparar", false);
        pistola.SetActive(false);
    }


    public void QuitarVidas() {
        vidas = vidas - 1;

        // HACEMOS QUE SUENE EL AUDIOSOURCE
        gritoVigilante.Play();
        // SALGAN PARTICULAS
        particulasSangreVigilante.Play();

        // SI YA NO TENGO VIDAS SE DESTRUYE
        if (vidas == 0) {

            // EL VIGILANTE SE CAERA CON UNA ANIMACION
            miAnimator.SetBool("Muerto", true);
            // INSTANCIAMOS LA SANGRE EN EL SUELO
            Instantiate(prefabSangreSuelo, transform.position, transform.rotation);

            // QUITAR EL COLISIONADOR DEL VIGILANTE PARA QUE SI PASA POR ENCIMA NO SE 
            // QUEDE PARADO EL PLAYER
            GetComponent<CapsuleCollider>().enabled = false;

            // INCREMENTAMOS LOS PUNTOS DEL PLAYER POR MATAR AL POLICIA
            player.GetComponent<PlayerScript>().IncrementarPuntuacion(puntosMatarPolicia);

        }

    }

}
