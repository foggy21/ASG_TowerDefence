using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : Tower
{
    void Start()
    {
        Enemy = null;
        OffsetAttackX = 1f;
        OffsetAttackY = 1.2f;
        DistanceAttack = 10f;
        BuildingProhibitionDistance = 5f;
        DelayAttack = 1f;
    }

    void FixedUpdate()
    {
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + OffsetAttackY), Vector2.left * BuildingProhibitionDistance);
        DisablePlayerBuildSkill();
        if (Enemy == null)
        {
            Enemy = FindEnemy();
            CurrentDelayAttack = 0;
        }

        if (Enemy != null && Vector2.Distance(transform.position, Enemy.transform.position) < DistanceAttack)
        {
            if (CurrentDelayAttack <= 0)
            {
                Attack(Enemy);
                CurrentDelayAttack = DelayAttack;
            } 
            else
            {
                CurrentDelayAttack -= Time.deltaTime;
            }
        } 
        else if (Enemy != null && Vector2.Distance(transform.position, Enemy.transform.position) >= DistanceAttack)
        {
            Enemy = null;
        }
    }

    public override void Attack(MoveableEnemy enemy)
    {
        if (enemy != null)
        {
            Fireball projectile = Resources.Load<Fireball>("Fireball");
            
            if (enemy.transform.position.x > transform.position.x)
            {
                Instantiate(projectile, new Vector2(transform.position.x + OffsetAttackX, transform.position.y + OffsetAttackY), Quaternion.identity);
            }
            else
            {
                Instantiate(projectile, new Vector2(transform.position.x - OffsetAttackX, transform.position.y + OffsetAttackY), Quaternion.identity);
            }
            GlobalEventManager.SetEnemy(enemy); 
        }
    }

    public override MoveableEnemy FindEnemy()
    {
        Collider2D enemyCollider = Physics2D.OverlapCircle(transform.position, DistanceAttack, EnemyMask);
        if (enemyCollider != null)
        {
            return enemyCollider.gameObject.GetComponent<MoveableEnemy>();
        }
        return null;
        
    }
}
