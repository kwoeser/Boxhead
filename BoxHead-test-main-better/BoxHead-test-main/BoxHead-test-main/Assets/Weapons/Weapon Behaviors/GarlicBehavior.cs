using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicBehavior : WeaponMeleeBehavior
{
    List<GameObject> markedEnemies;
    protected override void Start()
    {
        base.Start();
        markedEnemies = new List<GameObject>();
    }
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") && !markedEnemies.Contains(col.gameObject))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage);

            markedEnemies.Add(col.gameObject);  //Mark the enemy
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps breakable) && !markedEnemies.Contains(col.gameObject))
            {
                breakable.TakeDamage(currentDamage);

                markedEnemies.Add(col.gameObject);
            }
        }
        
    }


}
