﻿using System.Collections;
using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {
    enum Estado { Idle, Andando, Corriendo, Saltando, Disparando };
    Estado estado = Estado.Idle;

    // DISTRACCION
    //public Transform puntoGeneracionPetardos;
    //public GameObject prefabPetardo;

    // RECOGEMOS EL SCRIPT DE UISCRIPT PARA PODER LLAMAR AL METODO
    [SerializeField] UIScript uiScript;
    // BALA
    public Transform puntoGeneracionBala;
    public GameObject prefabBala;
    int fuerzaDisparo = 450;

    [SerializeField] Camera playerCamara;
    GameObject playerCamaraSalir;
    [SerializeField] Camera camara;
    Animator miCamaraAnimator;

    public int fuerzaLanzamientoPetardo;

    Animator miAnimator;

    // QUE SE CORRESPONDE AL ARGUMENTO QUE HE CREADO
    float corriendo = 0.1f; // LE PONEMOS PARA QUE ME FUNCIONE BIEN LAS TRANSICIONES
                            // Y NO HAGA COSAS RARAS
    float atras = 0.1f;

   // PARA COGER LA REFERENCIA DEL COFRE
   [SerializeField] GameObject miCofre;

    
    // RECOGEMOS LAS CERRADURAS PARA HACER UN CAMBIO Y QUE SE PUEDA EL COFRE
    GameObject miCerradura;

    GameObject miCerraduraActiva ;
    bool cerraduraActiva =  false;
    public static int puntos = 0;   
    private Text txtPuntuacion;
    private Text txtLlaves;
    int numLlaves = 0;
    [SerializeField] Image pistolaCanvas;

    // NIVEL DE JUEGO
    public int nivel = 1;   
    public static int nivelActual = 1;
    public static int nivelAnterior = 1;

    int nivel2 = 2;

    // VIDAS
    int vidasMaximas = 5;
    [SerializeField] int vidas;

    [SerializeField] GameObject pistola;

    // HAY QUE DEJARLO LUEGO A FALSE Y EL PLAYER DENTRO DE LA CARCEL
    // PARA QUE COMIEMZE EL JUEGO
    public static bool salirCarcel = false; 

    int puntuacionRecogida;
    int llavesRecogida;

    //
    [SerializeField] GameObject helicoptero;
    //[SerializeField] GameObject prefabVigilante;
    [SerializeField] GameObject puertaCarcel;
    [SerializeField] GameObject puertaCarcelInterior;

    //[SerializeField] GameObject caravela;
    [SerializeField] GameObject[] Key;

    ParticleSystem particulasSangre;
    AudioSource audioGrito;

    // DONDE ESTARIA LA MIRILLA
    [SerializeField] GameObject mirilla;

    // PARA APUNTA CON MIRILLA
   // bool apuntando = false;
   // float currentFOV;
   // float minFOV = 25;
   // float maxFOV;

    private void Awake() {
        puntos = 0;
        vidas = vidasMaximas;
    }

    void Start() {
        txtPuntuacion = GameObject.Find("Puntuacion").GetComponent<Text>();
        // OBTENEMOS LA PUNTUACION
        txtPuntuacion.text = "Score: " + puntos;
        // LLAVES
        txtLlaves = GameObject.Find("NumeroLlaves").GetComponent<Text>();
        // OBTENEMOS LA PUNTUACION
        txtLlaves.text = "" + numLlaves;

        // COGEMOS EL ANIMADOR DEL PLAYER
        miAnimator = GetComponent<Animator>();
        // SI NO ES LA SALIDA AL PATIO SOLO PARA EL NIVEL 1
        if (GameControler.GetNivel() == 1 ) {
            // RECOJO EL ANIMATOR DE LA CAMARA DEL JUEGO
            miCamaraAnimator = camara.GetComponent<Animator>();
            miCamaraAnimator.SetBool("PosicionInicial", true);
            miCamaraAnimator.SetBool("NuevaPosicion", false);
        }   

        // INICIALIZAMOS LAS ANIMACIONES PARA QUE NO GIRE EL ANIMATOR
        miAnimator.SetBool("GirarDerecha", false);
        miAnimator.SetBool("GirarIzquierda", false);
        estado = Estado.Idle;

        // RECOGEMOS LOS PLAYPREFS 
        RecogerPlayPrefs();

        // SI ESTAMOS EN EL NIVEL 1 SE MUESTRA EL COFRE
        if (GameControler.GetNivel() == 1) {
            // COGEMOS LAS REFERENCIAS DE LAS DOS CERRADURAS
           miCerraduraActiva = miCofre.transform.Find("CerraduraActiva").gameObject;

        } 
        // ESTAMOS EN EL PRIMER NIVEL EN FRENTE DE LA PUERTA
        else if (GameControler.GetNivel() >= 2) {
            camara.enabled = false;
            playerCamara.enabled = true;

            if (GameControler.GetNivel() == 3 ){ // PARA CUANDO SE ESCAPE
                // POSICIONAMOS EL PLAYER AL LADO DE LA PUERTA
                // CAMBIAMOS LA POSICION DEL PLAYER AL LADO DE LA PUERTA DE SALIDA
                Vector3 posicion = GameObject.Find("PosicionSalida").transform.position;

                // LO GUARDAMOS EN EL PLAYERPREFS
                GameControler.StorePosicion(posicion);

                // ACTUALIZAMOS LA POSICION DEL PLAYER
                gameObject.transform.position = posicion;
                float rotarY = -90.0f;
                gameObject.transform.Rotate(0f, rotarY, 0f);

                // LLAMAMOS A LA ANIMACION DE LA PUERTA INTERIOR
                puertaCarcelInterior.GetComponent<PuertaInteriorScript>().AbrirPuerta();
            }
        }

        // COGEMOS LAS PARTICULAS DE SANGRE
        particulasSangre = GetComponent<ParticleSystem>();
        // COGEMOS EL AUDIO DEL GRITO DEL PLAYER
        audioGrito = GetComponent<AudioSource>();
        // ("AUDIOGRITO "+audioGrito);

        // MIRILLA
        // FIJAMOS EL MAXIMO Y EL CURRENT EN LA POSICION ACTUAL 
        //maxFOV = miCamaraMirilla.fieldOfView;
        //currentFOV = miCamaraMirilla.fieldOfView;
    }

    private void RecogerPlayPrefs() {
        // LEEMOS EL FICHERO QUE HEMOS GENERADO
        // COMPROBAMOS SI EXISTEN ANTES DE COGERLA PARA QUE NO CASQUE
        // CON QUE SOLO HAGAMOS UNA VALDRIA YA QUE 

        // RECUPERAMOS LA POSICION
        Vector3 position = GameControler.GetPosition();
        // Y SI ES DISTINTO DE ZERO ES QUE TIENE DATOS Y ME PONE LA POSICION
        if (position != Vector3.zero) {
            if (GameControler.GetNivel() == 1) {
                this.transform.position = position;
            }
            else if (GameControler.GetNivel() == 2) {
                // CARGAMOS LA POSICION DE COMIENZO
                GameControler.StorePosicion(this.transform.position);
            }

            // Y RECUPERAMOS LOS PUNTOS
            puntuacionRecogida = GameControler.GetPuntos();

            // Y SI ES DISTINTO DE ZERO ES QUE TIENE DATOS Y ME PONE LA POSICION
            if (puntuacionRecogida > 0) {
                txtPuntuacion.text = "Score: " + puntuacionRecogida;
                puntos = puntuacionRecogida;
            }

            // Y RECUPERAMOS LAS LLAVES
            llavesRecogida = GameControler.GetLlaves();

            // Y SI ES DISTINTO DE ZERO ES QUE TIENE DATOS Y ME PONE LA POSICION
            if (llavesRecogida > 0)
            {
                txtLlaves.text = "" + llavesRecogida;
                numLlaves = llavesRecogida;
            }

            // Y RECUPERAMOS LAS VIDAS
            vidas = GameControler.GetVidas();
            // RECOGEMOS LOS HIJOS DEL PANEL PARA DESACTIVAR LAS VIDAS QUE NO TIENE
            uiScript.RestarVida();

            // RECOGEMOS LA PUERTA DE LA CARCEL SU POSICION
            // NIVEL 1
            if (GameControler.GetNivel() == 1) { 
                if (GameControler.GetPuertaCarcel() == "ABIERTA") {
                    puertaCarcel.GetComponent<Animator>().enabled = false;
                }
                // OBTENEMOS LA POSICION DE LA PUERTA Y LA PONEMOS
                puertaCarcel.transform.position = GameControler.GetPositionPuertaCarcel();
            }
            // NIVEL 2
            else if (GameControler.GetNivel() == 3 &&
                    SceneManager.GetActiveScene().buildIndex == 2) { 
                // OBTENEMOS LA POSICION DE LA PUERTA Y LA PONEMOS
                puertaCarcelInterior.transform.position = GameControler.GetPositionPuertaCarcelInterior();
            }

        }
        // SI ES LA PRIMERA VEZ QUE COMIENZA EL JUEGO RECOGEMOS LA POSICION
        else {
            // RELLENAMOS ESTOS VALORES PARA QUE COMIENZE AHI Y CARGAMOS
            // LAS POSICIONES EN SU SITIO
            GameControler.StorePosicion(this.gameObject.transform.position);
            // nivel 1 CARGAMOS EL NIVEL
            GameControler.StoreNivel(nivel);
        }

        // APUNTAR CON LA MIRILLA
//        ApuntarMirilla();

    }

    // Update is called once per frame
    private void Update() {

        // CARGAMOS LA POSICION DEL PLAYER CONINUAMENTE
        GameControler.StorePosicion(this.gameObject.transform.position);
        // INICIALIZAMOS EL ANIMATOR DE GIROS
        miAnimator.SetBool("GirarDerecha", false);
        miAnimator.SetBool("GirarIzquierda", false);

        // FIRE1 CLICK iZQUIERDO RATON
        // CAMBIO DE CAMARAS
        if (Input.GetKeyDown(KeyCode.Q)) {
         if (GameControler.GetNivel() == 1)  // SOLO PARA EL NIVEL 1
            {
                camara.enabled = false;
            }
            playerCamara.enabled = true;
         }
         // SI QUEREMOS VER LA POSICION DE LA OTRA CAMARA Y
         // NO ESTAMOS SALIENDO HACIA EL HELICOPTERO, SOLO NIVEL 1
         else if (Input.GetKeyDown(KeyCode.W) && GameControler.GetNivel() == 1) {
            camara.enabled = true;
            playerCamara.enabled = false;
         }
                

        /* if (cerraduraActiva == true) {
            estado = Estado.Idle;
        }*/

        // NIVEL 1
        if (GameControler.GetNivel() == 1) {
            if (miCerraduraActiva.GetComponent<CofreScript>() != null) {

                if (miCerraduraActiva.GetComponent<CofreScript>().recogidaPistaCofre == false) {
                    // COMPROBAR SI ESTOY CERCA DEL COFRE
                    ComprobarDistanciaCofre();

                }
            }
        }
        

        // SI LE DOY ARRIBA Y LA TECLA ES EL SHIFT IZQUIERDA
        if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.LeftShift)) {
        
                // ANDA ( SI ESTA CORRIENDO DECRECE LA VELOCIDAD)
                corriendo = corriendo - 0.01f;
                // COGE EL MAXIMO PARA QUE SE VAYA ANDANDO
                corriendo = Mathf.Max(0.11f, corriendo);
                miAnimator.SetFloat("Corriendo", corriendo);
        }
        else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftShift)) {
                // CORRE
                corriendo = corriendo + 0.01f;
                // COGE EL MINIMO QUE COMO POCO SERA
                corriendo = Mathf.Min(1, corriendo);
                miAnimator.SetFloat("Corriendo", corriendo);

        }
        else if (!Input.GetKeyDown(KeyCode.UpArrow)) {
                // PARA QUE SE VAYA PARANDO
                corriendo = corriendo - 0.01f;
                // COGE EL MINIMO QUE COMO POCO SERA 1
                corriendo = Mathf.Max(0f, corriendo);
                miAnimator.SetFloat("Corriendo", corriendo);
        }

        // HACIA ATRAS
        if (Input.GetKey(KeyCode.DownArrow)) {
            // ANDA HACIA ATRAS
            // ANDA ( SI ESTA CORRIENDO DECRECE LA VELOCIDAD)
            atras = atras - 0.01f;
            // COGE EL MINIMO PARA QUE SE VAYA PARANDO POCO A POCO Y NO DE GOLPE SERA 1
            atras = Mathf.Max(0.11f, atras);
            miAnimator.SetFloat("Atras", atras);
        } 
        else if (!Input.GetKeyDown(KeyCode.DownArrow)) {
            // PARA QUE SE VAYA PARANDO
            atras = atras - 0.01f;
            // COGE EL MINIMO QUE COMO POCO SERA 1
            atras = Mathf.Max(0f, atras);
            miAnimator.SetFloat("Atras", atras);

        }

        // DISPARAR
        if (Input.GetKeyDown(KeyCode.P)) {
            estado = Estado.Disparando;
            miAnimator.SetBool("Disparar",true);
            pistola.SetActive(true);    
         }
         // DEJA DE DISPARAR
         else if (Input.GetKeyDown(KeyCode.O)) {
            miAnimator.SetBool("Disparar",false);  
            pistola.SetActive(false);   
            estado = Estado.Idle;
            float corriendo = 0.1f; // LE PONEMOS PARA QUE ME FUNCIONE BIEN LAS TRANSICIONES                    
         }

         // DISPARA CON EL CLICK DEL RATON
        if (estado == Estado.Disparando && Input.GetButtonDown("Fire1")) {
            // LLAMAR AL SONIDO DE LA PISTOLA
            pistola.GetComponent<AudioSource>().Play();
            //
            LanzarBala();
        }

        if (corriendo > 0.1f){
             transform.Rotate(0, Input.GetAxis("Horizontal"), 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            // GIRAR DERECHA
            // transform.Rotate(0, Input.GetAxis("Horizontal"), 0);
            miAnimator.SetBool("GirarDerecha",true);
            miAnimator.SetBool("GirarIzquierda",false);
        } 
        else if (Input.GetKey(KeyCode.LeftArrow))  {
            // GIRAR IZQUIERDA
            // transform.Rotate(0, Input.GetAxis("Horizontal"), 0);
             miAnimator.SetBool("GirarIzquierda",true);
             miAnimator.SetBool("GirarDerecha",false);
        }
    }

    private void OnCollisionEnter(Collision collision) {

        // SI COLISIONA CON PADRES QUE CONTENGAN EL TAG CHOCAYGIRA Y EL COFRE CON LA CERRADURA DESACTIVADA 
        if (collision.gameObject.transform.parent != null) {

            // SE PARA Y CAMBIA DE ESTADO
            estado = Estado.Idle;
            miAnimator.SetBool("GirarDerecha", false);
            
            if (collision.gameObject.transform.parent.tag == "ChocayGira") {
                miAnimator.SetBool("GirarDerecha", true);

                // CAMBIAMOS EL GIRO A FALSE Y QUE SIGAN ANDANDO
                Invoke("CambiarEstadoAndar", 1);
            }

              // COLISION CON EL COFRE TAMBIEN TENGO QUE GIRAR PERO SI NO ESTA ACTIVA LA CERRADURA PARA ABRIR
              // EL COFRE
           else if (collision.gameObject.name == "Cofre" && cerraduraActiva == false) {
                miAnimator.SetBool("GirarDerecha", true);

                // CAMBIAMOS EL GIRO A FALSE Y QUE SIGAN ANDANDO
                Invoke("CambiarEstadoAndar", 1);
            } 
            else {
                miAnimator.SetBool("GirarDerecha", false);
            }
        
        }

        if (collision.gameObject.name == "PuertaCalabozo"){
            // RECOGEMOS LA PUNTUACION MAXIMA
            RecogerPuntuacionMaxima();

            int numero = 2;
            SetNivel(numero);
        }


    }

    private void CambiarEstadoAndar() { 
        // PONEMOS EL GIRO A FALSO PARA QUE NO SIGA GIRANDO
        miAnimator.SetBool("GirarDerecha", false);

        // Y LO VOLVEMOS A PONER QUE ANDE
        estado = Estado.Andando;
    
    }


    public void CogerPistola() {
       pistolaCanvas.GetComponent<Image>().enabled = true;        
    }


    private void ComprobarDistanciaCofre() {
    
        // DISTANCIA ENTRE POSICION DEL PLAYER Y EL COFRE
        float distancia = Vector3.Distance(transform.position, miCofre.transform.position);
        
        if( distancia <= 1){
            cerraduraActiva = true;
            if (miCerradura != null){
                miCerradura.gameObject.SetActive(false);
            }

            if (miCerraduraActiva != null){
                miCerraduraActiva.SetActive(true);
            }
        }
        else // LAS DEJAMOS COMO ESTABAN
        {
        cerraduraActiva = false;
          if (miCerradura != null){
                miCerradura.gameObject.SetActive(true);
            }

            if (miCerraduraActiva != null){
                miCerraduraActiva.SetActive(false);
             }  
        }
    }

    public void IncrementarPuntuacion(int puntosAIncrementar) {    
        puntos = puntos + puntosAIncrementar;
        txtPuntuacion.text = "Score: " + puntos;
        // CARGAR PUNTOS
        GameControler.StorePuntos(puntos);
    }


    public void MostrarLlaves(int llaves) {
        txtLlaves.text = "" + llaves;
        numLlaves = llaves;
    }


    // RECOGER PUNTOS
    public int GetPuntos() {
        return puntos;
    }

    // RECOGER VIDAS
    public int GetVidas() {
        return this.vidas; 
    }

    // RECOGER NIVEL
    public int GetNivel() {
       nivel = nivelActual;
       return nivel; 
    }

    public void SetNivel(int nuevoNivel) {
        nivelActual = nuevoNivel;
        nivel = nuevoNivel;
        nivelAnterior = 1;

        // CARGAMOS EL NIVEL NUEVO QUE ES EL 2
        GameControler.StoreNivel(nuevoNivel);
        // NIVEL 2
        if ( nuevoNivel == 2) {
            // ESCENA DE LA SELECCION DE NIVELES
            SceneManager.LoadScene(1);
        }
        // ABRIMOS LA PUERTA PRINCIPAL DE LA CARCEL 
        // Y SITUAMOS AL PLAYER AL LADO DE LA PUERTA
        else if ( nuevoNivel == 3) { 
            // LLAMAMOS AL NIVEL 2
            SceneManager.LoadScene(2);
        }
       
    }
    public void SetSalirCarcel(bool carcel) {
        salirCarcel = carcel;
    }

    public bool GetSalirCarcel() {
        return salirCarcel ;
    }
    
    public void EntrarHelicoptero() {
        miAnimator.SetBool("EntrarHelicoptero",true);
        Invoke("SubirHelicoptero",1);
    }

    void SubirHelicoptero(){
        // DEJAMOS SOLO UNA CAMARA
        playerCamaraSalir = transform.Find("PlayerCameraSalir").gameObject;
        playerCamaraSalir.SetActive(true);

        GameObject posicionHelicoptero = GameObject.Find("PosicionHelicopteroPlayer");
        transform.position = posicionHelicoptero.transform.position;
        transform.Rotate(0, 180 ,0);
        miAnimator.SetBool("EntrarHelicoptero",false);
        estado = Estado.Idle;
        transform.parent = helicoptero.transform;
        helicoptero.GetComponent<HelicopteroScript>().Escapar();
    }

     public void QuitarVidas() {
       
        // SE PUEDE PONER UN SONIDO CUANDO MUERA
        vidas = vidas - 1;
        // LO ACTUALIZAMOS EN EL PLAYPREFS
        GameControler.StoreVidas(vidas);
       
        //print("QUITAVIDAS PLAY AUDIOGRITO");
        // HACEMOS QUE SUENE EL AUDIOSOURCE
        audioGrito.Play();     

        // RECOGEMOS LOS HIJOS DEL PANEL
        uiScript.RestarVida();
        // LE PONEMOS SANGRE
        particulasSangre.Play();
        
        // SI YA NO TENGO VIDAS SE VUELVE A EMPEZAR EL JUEGO
        if ( vidas == 0) {
            // RECOGEMOS LA PUNTUACION MAXIMA
            RecogerPuntuacionMaxima();

            // FINALIZAMOS EL JUEGO Y RESETEAMOS TODO EL PLAYERPREFS
            Invoke("FinalizarJuego", 2);   
        }

    }

    private void RecogerPuntuacionMaxima(){
        int puntuacionMaxima = GameControler.GetPuntosMaximos();    
        if ( puntos > puntuacionMaxima) {
            // SI ES MAYOR CARGAMOS LOS PUNTOS MAXIMOS
            GameControler.StorePuntosMaximo(puntos);
        }
    }   
    private void FinalizarJuego() {
        // BORRAMOS LOS PLAYERPREFS PERO NO LOS PUNTOS MAXIMOS
        GameControler.BorrarPlayerPrefs();

        nivel = 1;
        GameControler.StoreNivel(nivel);

        // CARGAMOS EL GAMEOVER
        SceneManager.LoadScene(4);
    }

    private void LanzarBala() {

        // LANZAMOS UN RAYO EN EL PUNTO DONDE HE HECHO EL CLICK
        /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // DEPURAMOS EL RAYO
        Debug.DrawRay(puntoGeneracionBala.position, puntoGeneracionBala.forward, Color.red, 5);

        if (Physics.Raycast(ray)) {
            //Instanciar(partícula, transformar posición, transformar rotación);
            GameObject nuevaBala = Instantiate(prefabBala, puntoGeneracionBala.position, puntoGeneracionBala.rotation);
            // TIENE QUE TENER UN RIGID PARA APLICAR UNA FUERZA
            nuevaBala.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * fuerzaDisparo);
            Destroy(nuevaBala, 2);
        }*/

        GameObject nuevaBala = Instantiate(prefabBala, puntoGeneracionBala.position, puntoGeneracionBala.rotation);
        // TIENE QUE TENER UN RIGID PARA APLICAR UNA FUERZA
        nuevaBala.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * fuerzaDisparo);
        Destroy(nuevaBala, 2);

    }

    public void MostrarPistola() {
        pistola.SetActive(true);   
    }

    /*private void ApuntarMirilla() {
        print("APUNTAR MIRILLA");
        // PULSO EL BOTON DERECHO ACERCO LA VISTA AL OBJETIVO
        if (Input.GetMouseButtonDown(1)) {
            playerCamara.enabled = false;
            miCamaraMirilla.enabled = true;

    //        Debug.Log("Boton derecho pulsado");
            apuntando = true;
            mirilla.SetActive(true);
        }
        // CUANDO LO SOLTAMOS
        if (Input.GetMouseButtonUp(1)) {
            playerCamara.enabled = true;
            miCamaraMirilla.enabled = false;
            //      Debug.Log("Boton derecho soltado");
            apuntando = false;
            mirilla.SetActive(false);
        }

        // ESTO ES EL ZOOM DE LA CAMARA
        // IRA POR FRAMES HAY QUE USAR EL DELTATIME
        if (apuntando) {
            currentFOV = currentFOV - 1;
            // QUE COJA VALOR MAXIMO DE LOS DOS PARA QUE NO SE PASE
            // DE VALOR 
            currentFOV = Mathf.Max(currentFOV, minFOV);
        }
        // SI ES FALSO ES QUE LO HEMOS SOLTADO Y 
        // DEBERIA DE VOLVER HACIA ATRAS
        else {
            currentFOV = currentFOV + 1;
            // QUE COJA VALOR MINIMO DE LOS DOS PARA QUE NO SE PASE
            // DE VALOR 
            currentFOV = Mathf.Min(currentFOV, maxFOV);

        }

        miCamaraMirilla.fieldOfView = currentFOV;
    }*/
}
