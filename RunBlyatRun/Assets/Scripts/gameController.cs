using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//importar atributos de UI de unity
using UnityEngine.SceneManagement;//importar atributos de Scenas de Unity
//maquina de estados
public enum GameState { idle, playing, gameOver, ready };//accesible por todos los scripts

public class gameController : MonoBehaviour
{
    [Range(0.00f, 0.15f)]//rango de valores de parallax
    public float parallaxSpeed = 0.02f;//velocidad de parallax por defecto
    public RawImage c_background;//importar imagen de background de unity
    public RawImage c_platform;//importar raw image de unity
    public GameObject UI_idle;//import UI_idle de unit
    public GameObject UI_playing;//import UI_playing de unity
    public GameObject UI_gameOver;//import UI de unity
    public Text numText;//numero de puntos en pantalla
    public Text recordText;//historial de numeros

    public GameObject player1;//importo el objeto player1 de unity
    public GameObject enemyGenerator;//impporto el enemyGenerator de Unity;

    public GameState gameSTATE = GameState.idle;//estado por defecto
    public float scaleTime = 5f;//dificultad(tiempo de activacion)
    public float scaleInc = 0.25f;//incremento

    private AudioSource musicPlayer;//audio de fondo
    private int points = 0;//puntaje
                           // Use this for initialization
    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();
        recordText.text = "Best: " + GetMaxScore().ToString();
        UI_gameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        bool userAction = Input.GetKeyDown("up") || Input.GetMouseButtonDown(0);
        //inicio de juego
        if (gameSTATE == GameState.idle && userAction)//captura mouse: Input.GetMouseButtonDown(0)
        {
            gameSTATE = GameState.playing;//cambia de estado
            UI_idle.SetActive(false);//quita los textos en pantalla cuando inicia el juego
            UI_playing.SetActive(true);//inicia los textos de puntaje
            
            player1.SendMessage("UpdateState", "player1Root");//manda  mensaje al otro script mediante el obj Player de UNITY
            enemyGenerator.SendMessage("StarGenerator");//activa la generacion de enemigos
            player1.SendMessage("DustPlay");//inicia las particulas
            musicPlayer.Play();//iniciar musica
            InvokeRepeating("GameTimeScale", scaleTime, scaleTime);//controla la incremento de dificultad
        }
        else if (gameSTATE == GameState.playing)//duarante el jeugo
        { 
            parallax();//inicia la funcion
        }
        else if (gameSTATE == GameState.ready)//juego cerca del reinicio
        {
            UI_gameOver.SetActive(true);
            if (userAction)
            {
                RestartGame();
            }
        }


    }
    //control de parallax efect
    void parallax()
    {
        float finalSpeed = parallaxSpeed * Time.deltaTime;// se multiplica el parallax con el frameTime del dispositivo
        c_background.uvRect = new Rect(c_background.uvRect.x + finalSpeed, 0f, 1f, 1f);//cambia los valores de uvRect de unity
        c_platform.uvRect = new Rect(c_platform.uvRect.x + finalSpeed * 2, 0f, 1f, 1f);//cambia los valores uvRect de platform
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("Scene1");//reinicia la escena del juego

    }

    void GameTimeScale()
    {//propiedad que controla el tiempo global del juego
        Time.timeScale += scaleInc;
        Debug.Log("Incremento de velocidad: " + Time.timeScale.ToString());
    }
    public void ResetTimeScale(float defaultTimeScale)
    {
        CancelInvoke("GameTimeScale");
        Time.timeScale = defaultTimeScale;
        Debug.Log("Incremento de velocidad: " + Time.timeScale.ToString());
    }
    //puntos aumento
    public void IncreasePoint()//aumenta points y cambia el texto
    {
        numText.text =  (++points).ToString() + "\n puntos";
        if (points >= GetMaxScore() )
        {
            recordText.text = "Best: "+ GetMaxScore().ToString();
            SaveScore(points);
        }

    }

    public int GetMaxScore()
    {//playerPrefs guarda el historial
        return PlayerPrefs.GetInt("Best Score", 0);
    }
    public void SaveScore(int currentPoints)
    { PlayerPrefs.SetInt("Best Score", currentPoints); }
}
