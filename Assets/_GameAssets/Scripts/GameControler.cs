using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControler : MonoBehaviour {

    // LO QUE LE DICE AL SISTEMA OPERATIVO NO VA A OCUPAR MAS DE 4BIT
    // PRIVATE PARA QUE NO SE VEA DESDE FUERA Y CONSTANTE PARA QUE NO SE PUEDA CAMBIAR EN 
    // TIEMPO DE EJECUCION

    private const string XPOS = "xPos";
    private const string YPOS = "yPos";
    private const string ZPOS = "zPos";
    private const string XPOSPUERTACARCEL = "xPosPuertaCarcel";
    private const string YPOSPUERTACARCEL = "yPosPuertaCarcel";
    private const string ZPOSPUERTACARCEL = "zPosPuertaCarcel";
    private const string PUERTACARCEL = "puertaCarcel";
    
    private const string XPOSPUERTAINTERIOR = "xPosPuertaInterior";
    private const string YPOSPUERTAINTERIOR = "yPosPuertaInterior";
    private const string ZPOSPUERTAINTERIOR = "zPosPuertaInterior";
    private const string PUERTAINTERIOR = "puertaInterior";
    private const string PUNTOS = "puntos";
    private const string PUNTOSMAXIMOS = "puntosMaximos";
    private const string LLAVES = "llaves";
    private const string LLAVESANTERIOR = "llavesNivelAnterior";

    private const string NIVEL = "nivel";
    private const string VIDAS = "vidas";

     public static void BorrarPlayerPrefs() {

        // RECOGEMOS LOS PUNTOS MAXIMOS
         int puntosMax =  GetPuntosMaximos();
        // BORRAMOS TODO
        PlayerPrefs.DeleteAll();
        // VOLVEMOS A CARGAR LOS PUNTOS MAXIMOS AL PLAYERPREFS
        // PARA NO PERDERLOS
        StorePuntosMaximo(puntosMax);
       
    }


    // EL STATIC YO PUEDO LLAMAR A LA CLASE SIN HACER UNA INSTACIA DE LA CLASE
    public static void StorePosicion (Vector3 posicion) {
        // HARDCODE ES PONER ENTRECOMILLAS LA ETIQUETA
        PlayerPrefs.SetFloat(XPOS, posicion.x);
        PlayerPrefs.SetFloat(YPOS, posicion.y);
        PlayerPrefs.SetFloat(ZPOS, posicion.z);
        PlayerPrefs.Save();
    }

    public static Vector3 GetPosition() {

        Vector3 position = new Vector3();
        if (PlayerPrefs.HasKey(XPOS) &&
            PlayerPrefs.HasKey(YPOS)) {
            
            float x = PlayerPrefs.GetFloat(XPOS);
            float y = PlayerPrefs.GetFloat(YPOS);
            float z = PlayerPrefs.GetFloat(ZPOS);
    
            // Y PONEMOS ESAS POSICIONES AL PLAYER
            position = new Vector3(x, y, z);      
        } 
        // SI NO LO ENCUENTRA DEVUELVE UN VECTOR3 A 0
        else {
            position = Vector3.zero;
        }

        return position;
    }

    // EL STATIC YO PUEDO LLAMAR A LA CLASE SIN HACER UNA INSTACIA DE LA CLASE
    public static void StorePosicionPuertaCarcel(Vector3 posicion) {
        // HARDCODE ES PONER ENTRECOMILLAS LA ETIQUETA
        PlayerPrefs.SetFloat(XPOSPUERTACARCEL, posicion.x);
        PlayerPrefs.SetFloat(YPOSPUERTACARCEL, posicion.y);
        PlayerPrefs.SetFloat(ZPOSPUERTACARCEL, posicion.z);
        PlayerPrefs.SetString(PUERTACARCEL,"ABIERTA");
        PlayerPrefs.Save();
    }

    public static Vector3 GetPositionPuertaCarcel() {
        Vector3 position = new Vector3();
        if (PlayerPrefs.HasKey(XPOSPUERTACARCEL) &&
            PlayerPrefs.HasKey(YPOSPUERTACARCEL)) {
            float x = PlayerPrefs.GetFloat(XPOSPUERTACARCEL);
            float y = PlayerPrefs.GetFloat(YPOSPUERTACARCEL);
            float z = PlayerPrefs.GetFloat(ZPOSPUERTACARCEL);
            // Y PONEMOS ESAS POSICIONES AL PLAYER
            position = new Vector3(x, y, z);
        }
        // SI NO LO ENCUENTRA DEVUELVE UN VECTOR3 A 0
        else {
            position = Vector3.zero;
        }
        return position;
    }

    public static string GetPuertaCarcel() {
        string puertaCarcel = "CERRADA";
        if (PlayerPrefs.HasKey(PUERTACARCEL)) {
            puertaCarcel = PlayerPrefs.GetString(PUERTACARCEL);
        }
    
        return puertaCarcel;
    }

 public static void StorePosicionPuertaCarcelInterior(Vector3 posicion) {
        // HARDCODE ES PONER ENTRECOMILLAS LA ETIQUETA
        PlayerPrefs.SetFloat(XPOSPUERTAINTERIOR, posicion.x);
        PlayerPrefs.SetFloat(YPOSPUERTAINTERIOR, posicion.y);
        PlayerPrefs.SetFloat(ZPOSPUERTAINTERIOR, posicion.z);
        PlayerPrefs.SetString(PUERTAINTERIOR,"ABIERTA");
        PlayerPrefs.Save();

    }

    public static Vector3 GetPositionPuertaCarcelInterior() {
        Vector3 position = new Vector3();
        if (PlayerPrefs.HasKey(XPOSPUERTAINTERIOR) &&
            PlayerPrefs.HasKey(YPOSPUERTAINTERIOR)) {
            float x = PlayerPrefs.GetFloat(XPOSPUERTAINTERIOR);
            float y = PlayerPrefs.GetFloat(YPOSPUERTAINTERIOR);
            float z = PlayerPrefs.GetFloat(ZPOSPUERTAINTERIOR);
            // Y PONEMOS ESAS POSICIONES AL PLAYER
            position = new Vector3(x, y, z);
        }
        // SI NO LO ENCUENTRA DEVUELVE UN VECTOR3 A 0
        else {
            position = Vector3.zero;
        }
        
        return position;
    }
  public static string GetPuertaCarcelInterior() {
        string puertaInterior = "CERRADA";
        if (PlayerPrefs.HasKey(PUERTAINTERIOR)) {
            puertaInterior = PlayerPrefs.GetString(PUERTAINTERIOR);
        }
    
        return puertaInterior;
    }

    // CARGAMOS LOS PUNTOS
    public static void StorePuntos (int puntuacion) {
        PlayerPrefs.SetInt(PUNTOS, puntuacion);
        PlayerPrefs.Save();
    }

    // RECOGEMOS LOS PUNTOS
    public static int GetPuntos() {
        int puntuacion = 0;

        if (PlayerPrefs.HasKey(PUNTOS)) {
           puntuacion =  PlayerPrefs.GetInt(PUNTOS);
        } 

        return puntuacion;
    }


    // CARGAMOS LOS PUNTOS MAXIMOS
    public static void StorePuntosMaximo(int puntuacion) {
        PlayerPrefs.SetInt(PUNTOSMAXIMOS, puntuacion);
        PlayerPrefs.Save();

    }

    // RECOGEMOS LOS PUNTOS MAXIMOS
    public static int GetPuntosMaximos () {
        int puntuacionMaxima = 0;

        if (PlayerPrefs.HasKey(PUNTOSMAXIMOS)) {
            puntuacionMaxima = PlayerPrefs.GetInt(PUNTOSMAXIMOS);
        }

        return puntuacionMaxima;
    }

    // CARGAMOS LAS LLAVES
    public static void StoreLlaves (int llaves) {
        PlayerPrefs.SetInt(LLAVES, llaves);
        PlayerPrefs.Save();
    }

    // RECOGEMOS LOS LLAVES
    public static int GetLlaves() {
        int llaves = 0;

        if (PlayerPrefs.HasKey(LLAVES)) {
            llaves =  PlayerPrefs.GetInt(LLAVES);
        } 

        return llaves;
    }

    public static void StoreLlavesNiveAnterior (int llaves) {
        PlayerPrefs.SetInt(LLAVESANTERIOR, llaves);
        PlayerPrefs.Save();
    }

    // RECOGEMOS LOS LLAVES
    public static int GetLlavesNivelAnterior() {
        int llaves = 0;

        if (PlayerPrefs.HasKey(LLAVESANTERIOR)) {
            llaves =  PlayerPrefs.GetInt(LLAVESANTERIOR);
        } 

        return llaves;
    }

    // CARGAMOS EL NIVEL
    public static void StoreNivel (int nivel) {
        PlayerPrefs.SetInt(NIVEL, nivel);
        PlayerPrefs.Save();

    }

    // RECOGEMOS EL NIVEL
    public static int GetNivel() {
        int nivel = 1;

        if (PlayerPrefs.HasKey(NIVEL)) {
            nivel =  PlayerPrefs.GetInt(NIVEL);
        }
        return nivel;
    }

    // CARGAMOS LOS VIDAS
    public static void StoreVidas(int vidas) {
        PlayerPrefs.SetInt(VIDAS, vidas);
        PlayerPrefs.Save();
    }

    // RECOGEMOS LOS VIDAS
    public static int GetVidas() {
        int vidas = 5;

        if (PlayerPrefs.HasKey(VIDAS))
        {
            vidas = PlayerPrefs.GetInt(VIDAS);
        }
        return vidas;
    }

   

    // VAMOS A COGER TODAS LAS ANIMACIONES PARA PARARLAS CUANDO HA TERMINADO
    // EL TIEMPO DE JUEGO
    public static void PararAnimaciones() {
        GameObject[] animaciones = GameObject.FindGameObjectsWithTag("Animaciones");
        
        for (int i = 0; i < animaciones.Length; i++) {
            animaciones[i].SetActive(false);
        }


    }

}
