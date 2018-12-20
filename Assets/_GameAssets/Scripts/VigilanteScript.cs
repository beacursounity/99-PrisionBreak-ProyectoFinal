using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class VigilanteScript : MonoBehaviour {

    public Transform[] puntosPatrulla;

    NavMeshAgent agente;
    Animator miAnimator;

    // ESTADO DEL VIGILANTE LO HE COGIDO DEL PLAYER YA QUE HARA LO MISMO
    enum Estado { Idle, Andando, Corriendo, Saltando, Disparando, Siguiendo, Distraido, Muerto };
    // LO INICIALIZAMOS A ESTADO QUIETO
    Estado estado = Estado.Idle;

    // TIEMPO ESPERA ENTRE ASIGNACIONES DE PUNTOS DE PATRULLA
    const int TIEMPO_ESPERA = 2;

    // TEXTOS DEL CANVAS Y LOS TIENE QUE MOSTRAR CONTINUAMETNE EN EL UPDATE
    //public Text textDTP;
    //public Text textATP;

    public GameObject player;

    float anguloVision = 25;
    float distanciaVision = 6;

    // PARA SABER SI ESTA A TIRO EL PLAYER
    //public Text aTiro;

    // VIDAS POR CADA POLICIA 2
    int vidas = 2;

    // PUNTOS POR MATAR A UN POLICIA
    int puntosMatarPolicia = 10;
    AudioSource gritoVigilante;
    ParticleSystem particulasSangreVigilante;

    [SerializeField] GameObject prefabSangreSuelo;

    //bool colisionConPlayer = false;
    float numeroEspera = 0;
    float numeroMaximoEspera = 8;

    [SerializeField] AudioSource audioSirena;

    // Use this for initialization
    void Start() {
        agente = GetComponent<NavMeshAgent>();
        miAnimator = GetComponent<Animator>();
        miAnimator.SetBool("Andando", false);
        miAnimator.SetBool("Muerto", false);
        estado = Estado.Idle;

        gritoVigilante = GetComponent<AudioSource>();
        particulasSangreVigilante = GetComponent<ParticleSystem>();
        // RECOJO EL AUDIO
        audioSirena = GetComponent<AudioSource>();

        // ASIGNAMOS EL PUNTO DE PATRULLA AL ARRAY
        AsignarPuntoPatrulla();
    }


    // Update is called once per frame
    void Update() {

        // SI TIENE VIDAS
        if (estado != Estado.Muerto) {
            // SI EL VIGILANTE NO ESTA DISTRAIDO
            if (estado != Estado.Distraido) {
                VerificarObjetivo();
              }

            //EVALUAMOS LOS ESTADOS
            switch (estado) {
                case Estado.Idle:
                    // NO HAGO NADA
                    break;

                case Estado.Andando:
                    // TENEMOS QUE CONTROLAR SI CUANDO ANDAS SI HAS LLEGADO AL DESTINO
                    ComprobarDestino();
                    break;

                case Estado.Corriendo:

                    break;

                case Estado.Saltando:

                    break;

                case Estado.Disparando:

                    break;

                case Estado.Siguiendo:
                    // LE PONEMOS EL NUEVO DESTINO
                    agente.destination = player.transform.position;
                    break;

                case Estado.Distraido:
                    numeroEspera = numeroEspera + Time.deltaTime;
                    if (numeroEspera >= numeroMaximoEspera) {
                        estado = Estado.Idle;
                        numeroEspera = 0;
                        AsignarPuntoPatrulla();
                    }
                    break;
            }
        }



    }

    private void OnCollisionEnter(Collision other) {
 
        if ( estado != Estado.Muerto && other.gameObject.name == "Player") {
            // LE QUITAMOS VIDAS AL PLAYER
            // me quita 1 vidas cuando colisiono con la Bala del Enemigo Estatico
            other.gameObject.GetComponent<PlayerScript>().QuitarVidas();

            // PARA QUE SE  LE DE TIEMPO AL PLAYER A IRSE
            estado = Estado.Distraido;
            miAnimator.SetBool("Andando", false);
            numeroEspera = 0;
        }
    }


    public void QuitarVidas() {
        
        vidas = vidas - 1;
        
        // HACEMOS QUE SUENE EL AUDIOSOURCE
        gritoVigilante.Play();
        // SALGAN PARTICULAS
        particulasSangreVigilante.Play();

        // SI YA NO TENGO VIDAS SE DESTRUYE
        if (vidas == 0) {
         
            miAnimator.SetBool("Andando", false);
            estado = Estado.Muerto;
            
            // EL VIGILANTE SE CAERA CON UNA ANIMACION
            miAnimator.SetBool("Muerto", true);
         
            // Paramos el Nav Mesh Agent
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            // INSTANCIAMOS LA SANGRE EN EL SUELO
            Instantiate(prefabSangreSuelo, transform.position, transform.rotation);

            // QUITAR EL COLISIONADOR DEL VIGILANTE PARA QUE SI PASA POR ENCIMA NO SE 
            // QUEDE PARADO EL PLAYER
            GetComponent<BoxCollider>().enabled = false;

            // INCREMENTAMOS LOS PUNTOS DEL PLAYER POR MATAR AL POLICIA
            player.GetComponent<PlayerScript>().IncrementarPuntuacion(puntosMatarPolicia);
        }
        else if (vidas > 0)
        {           
            // PARA QUE SE  LE DE TIEMPO AL PLAYER A IRSE
            estado = Estado.Distraido;
            numeroEspera = 0;
            // ASIGNAMOS EL PUNTO DE PATRULLA AL ARRAY PARA QUE PUEDA ESCAPAR EL PLAYER
            AsignarPuntoPatrulla();
        }

    }

    // ASIGNAR PUNTO PATRULLA ALEATORIO
    private void AsignarPuntoPatrulla() {


        // EL DESTINO DE LA GENTE SERA EL PRIMER PUNTO DEL ARRAY
        // PERO HAREMOS UN AHORA UN RANGO
        // SI NO ESTA DISTRAIDO IRA AL PUNTO DE PATRULLA ELEGIDO ALEATORIAMENTE
        int pp = Random.Range(0, puntosPatrulla.Length);
        agente.destination = puntosPatrulla[pp].position;
        estado = Estado.Andando;
        miAnimator.SetBool("Andando", true);

    }

    // COMPROBAMOS EL DESTINO
    private void ComprobarDestino() {
        
        // SI HA CALCULADO LA RUTA, QUE ES == FALSE
        // TARDA EN CALCULAR LA RUTA
        if (!agente.pathPending) {
            // DISTANCIA QUE QUEDA POR RECORRER <= A LA DISTANCIA QUE SE PARA
            if (agente.remainingDistance <= agente.stoppingDistance) {
                miAnimator.SetBool("Andando", false);
        
                estado = Estado.Idle;

                // PARA QUE ROTE UN POCO PARA QUE QUEDE MEJOR
                transform.Rotate(0, 180, 0);
                Invoke("AsignarPuntoPatrulla", TIEMPO_ESPERA);
            }
        }
    }


    // CUANDO VA 
    public void SetDistraccion(Vector3 position) {
        agente.destination = position;
        estado = Estado.Distraido;
    }


    public void VerificarObjetivo() {
        
        // DISTANCIA ENTRE POSICION DE LA VIGILANCIA Y EL PLAYER para mi es 59
        float distancia = Vector3.Distance(transform.position, player.transform.position);
        //textDTP.text = "DTP: " + distancia;

        // ANGULO
        // HABRIA QUE COMPROBAR SI LA DISTANCIA ES LA ADECUADA HARIA NO NO EL CALCULO DEL ANGULO
        // esto esta mal ya que cojo al player en el angulo yu no tiene nada que ver
        //float angulo = Vector3.Angle(transform.position, player.transform.position);
        // DIRECCION DE MAGNITUD 1 EL VECTOR QUE ME LLEVA AL PLAYER
        // nos da igual el Normalize porque solo miramos la direccion
        Vector3 direccion = Vector3.Normalize(player.transform.position - transform.position);
        float angulo = Vector3.Angle(direccion, transform.forward);

        // SI ME ESTA VIENDO EL VIGILANTE TENDRE QUE LANZAR UN RAYCAST 
        // PARA SABER SI LE DOY O NO POR SI TENGO COLISIONADORES POR ENMEDIO
        if (distancia < distanciaVision && angulo < anguloVision) {
            //  print("verificarObjetivo");
            //Debug.DrawLine(transform.position, player.transform.position, Color.red, 2);
            RaycastHit rch;
            if (Physics.Raycast(transform.position, direccion, out rch, Mathf.Infinity)) {
                // para saber con quien me choco
                // SI ES EL PLAYER Y HE SALIDO DE LA CARCEL YA PUEDE VENIR A POR MI
                //    print(rch.transform.gameObject.name);
                if (rch.transform.gameObject.name == "Player" &&
                    player.GetComponent<PlayerScript>().GetSalirCarcel() == true) {
                    //print("PLAY SIRENA");
                    audioSirena.Play();
                    //    aTiro.text = "A tiro: SI";
                    estado = Estado.Siguiendo;
                } else {
          //          aTiro.text = "A tiro: NO";
                }
            }
        }
        // SI NO ESTA DENTRO DE LA VISION LO PONEMOS A "NO"
        else {
            //aTiro.text = "A tiro: NO";
        }

        //textATP.text = "ATP: " + angulo;
    }
}
