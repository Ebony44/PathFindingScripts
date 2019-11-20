using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    // if i need to following SRP rules, it should split up into IAttack, IHealth....
    // but let's just simplify. i don't need that many ideas for many enemies.
    // IEnemy is too wide role. if supporter type monster was in game which doesn't attack player or doesn't have HPSystem... it should be a problem.
    // ..crap don't use this.

    // IHealth
    float Health { get; set; }
    
    event System.Action<float> OnHPPtChanged;
    event System.Action OnDied;
    void TakeDamage(int amount);
    
    // IAttack
    Vector3 PlayerPosition { get; set; }
    float AttackDelay { get; set; }
    float MaxAttackDelay { get; set; }

    void Attack(int DamageAmount);

    // IAttack
    // AttackRange, AttackDelay, AttackCondition
    
    // IHealth
    // HealOverTime, DamageOverTime, HurtOnlyEnvironment, InvincibleAfterTakenDamage..etc

}
