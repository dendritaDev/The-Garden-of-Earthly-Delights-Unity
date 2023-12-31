using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    float speed;
    int damage = 5;
    int numOfPerforationHits = 3;

    [SerializeField] float hitArea = 0.7f;
    float timeToDestroy = 6f;

    List<IDamageable> alreadyHitTargets;

    [SerializeField]
    [Tooltip("Cada cuantos frames se llama al update. Solo tocara para optimizacion. Para modificar velocidad -> cambiar speed")]
    int frameRate = 6;

    [HideInInspector] public Vector3 directionToMove;

    // Update is called once per frame
    void Update()
    {
        //Optimizar: Solo llamamos update cada 6 frames: A m�s rapido y mas peque�o sea el objeto, mas update tendremos que hacer para que no se de el caso
        //de que de frame a frame se mueve lo suficiente como para no collisionar con el enemigo aunque visualmente si que lo parezca
        if (Time.frameCount % frameRate == 0)
        {
            Move();
            HitDetection();

        }

        LifeTimer();
    }

    private void LifeTimer()
    {
        timeToDestroy -= Time.deltaTime;
        if (timeToDestroy < 0)
        {
            Destroy(gameObject);
        }
    }

    private void HitDetection()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, hitArea);

        foreach (var collider in hit)
        {
            if (numOfPerforationHits > 0)
            {
                IDamageable objectHit = collider.GetComponent<IDamageable>();
                if (objectHit != null)
                {
                    if (!CheckRepeatHit(objectHit))
                    {
                         
                        alreadyHitTargets.Add(objectHit);
                        objectHit.TakeDamage(damage);
                        numOfPerforationHits--;
                    }
                }
            }
            else
            {
                break;
            }
        }

        if (numOfPerforationHits <= 0)
            Destroy(gameObject);
    }

    private bool CheckRepeatHit(IDamageable objectHit)
    {
        if (alreadyHitTargets == null) { alreadyHitTargets = new List<IDamageable>(); }

        return alreadyHitTargets.Contains(objectHit);
    }

    /// <summary>
    /// Movimiento del proyectil.
    /// Multiplico la speed por el frameRate para compensar que si se hace el update menos veces, que se desplace m�s para que la velocidad de la piedra no dependa del framerate que le demos
    /// Asi solo tenemos que preocuparnos por modificar la velocidad
    /// </summary>
    private void Move()
    {

        transform.position += (speed * frameRate) * Time.deltaTime * directionToMove;
    }

    internal void SetStats(WeaponBase weaponBase)
    {
        damage = weaponBase.GetDamageStat();
        numOfPerforationHits = weaponBase.weaponStats.numOfPerforationHits;
        speed = weaponBase.weaponStats.projectileSpeed;
    }

}
