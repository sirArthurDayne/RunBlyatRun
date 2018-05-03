using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    private Animator p1_animation;//crea un Atributo de animator
    private float yGround;
    public GameObject gameControl;//importa el gameController de Unit
    public GameObject enemyGen;//importar el enemyGenerator de Unity

    public AudioClip audioJump;//efecto de saltar
    public AudioClip audioDeath;//efectio de muerte
    public AudioClip audioPoint;//efecto de punto 
    private AudioSource audioPlayer;//gestiona losefectos del personaje

    public ParticleSystem dust;//sistema de particulas
	
    
    // Use this for initialization
	void Start ()
    {//adquiere componentes
        p1_animation = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        yGround = transform.position.y;// coord, y en suelo
    }
	
	// Update is called once per frame
	void Update ()
    {
        bool ground = transform.position.y == yGround; 
        bool userAction = Input.GetKeyDown("up") || Input.GetMouseButtonDown(0);//captura de inputs
        bool gamePlaying = gameControl.GetComponent<gameController>().gameSTATE == GameState.playing;//verifica si estamos jugando
        //deteccion de salto
        if ( ground && gamePlaying && userAction)//si estamos en estado jugando y presionamos tecla up
        {
            UpdateState("player1Jump");//p1_animation.Play("player1Jump");ejecuta el animator de unity
            audioPlayer.clip = audioJump;
            audioPlayer.Play();//inicia efecto de salto
        }
	}
    public void UpdateState(string state = null)
    {
       
        if (state != null)//le pregunta si ya cambio el estado
        {
            p1_animation.Play(state);//le da paly a la animacion de corre.Play("nombre de la animacion en unity");
        }
    }
    void OnTriggerEnter2D(Collider2D other)//funcion de UNITY que detecta la colision del enemigo con el Destroyer
    {
        if (other.gameObject.tag == "Enemy")
        {     //si el enemigo choca con el colider manda un tag a la instancia
            UpdateState("player1death");//animacion de muerte
            Debug.Log("Colision detectada");//mensaje por consola
            gameControl.GetComponent<gameController>().gameSTATE = GameState.gameOver;//cambio de estado a GameOver
            enemyGen.SendMessage("CancelGenerator", true); //cancelar generacion de enemigos
            gameControl.SendMessage("ResetTimeScale", 1f);//activa el reseteo de dificultad
            DustStop();
            gameControl.GetComponent<AudioSource>().Stop();//detener musica
            audioPlayer.clip = audioDeath; //asigna el clip
            audioPlayer.Play();//inicia efecto

        } else if (other.gameObject.tag == "pointAdd")
        {
            gameControl.SendMessage("IncreasePoint");//si saltamos sobre enemigo sumaremos 1 punto.
            audioPlayer.clip = audioPoint;//activa sonido
            audioPlayer.Play();
        }
    }

    void GameReady()//cambio a estado ready
    {
        gameControl.GetComponent<gameController>().gameSTATE = GameState.ready;
    }
    //particulas
    void DustPlay()
    { dust.Play(); }
    void DustStop()
    { dust.Stop(); }
   
}
