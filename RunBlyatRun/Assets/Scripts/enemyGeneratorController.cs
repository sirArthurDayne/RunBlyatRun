using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyGeneratorController : MonoBehaviour
{

    public GameObject enemyPreFab;//importa el preFab de unity
    public float generatorTimer = 1.75f;//tiempo de generacion de enemigos
	// Use this for initialization
	void Start ()
    {
       
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void CreateEnemy()
    {
        Instantiate( enemyPreFab, transform.position, Quaternion.identity );/*funcion Unity, que pide el obj a clonar
        la posicion a clonar, y la propiedad de funcion Quaternion*/
    }
    public void StarGenerator()//se activa cuando cambiamos de estado
    {
        //Repetidor de eventos
        InvokeRepeating("CreateEnemy", 0f, generatorTimer);//invocar enemigos
        //parametros: "nombre de funcion", el tiempo de retraso al iniciar, tiempo de repeticion en seg.
    }
    public void CancelGenerator(bool clean = false)
    {
        CancelInvoke("CreateEnemy");//funcion UNity: hace que que el InvokeRepeating deje de funcionar
        if (clean)
        {
            Object[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy"); //arreglo que almacena las instancias para destruirlas
            foreach ( GameObject enemyGen in allEnemies)
            {
                Destroy(enemyGen);

            }//ciclo que borra todo
        }
    }
    
}
