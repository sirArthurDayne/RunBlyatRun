using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    [Range (0f, 6f)]// rango de valores de velocidad
    public float velocity = 3f;//velocidad de enemigo por defectp
    private Rigidbody2D rBody2d;//crear atributo de Rigidboyd2d
	// Use this for initialization
	void Start () {
        rBody2d = GetComponent<Rigidbody2D>();//recuperar componente de unity
        rBody2d.velocity = Vector2.left * velocity;//Vector2: se encarga de cambiar la posicion de las coordenadas
	}
	
	// Update is called once per frame
	void Update () {
	   	
	}
    void OnTriggerEnter2D(Collider2D other)//funcion de UNITY que detecta la colision del enemigo con el Destroyer
    {
        if (other.gameObject.tag=="Destroyer")//si el enemigo choca con el colider manda un tag a la instancia
        Destroy(gameObject);//destruye la instancia del enemigo
    }

}
