using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    event System.Action OnDied;
    void Attack();
    void TravelToTarget(Vector3 targetPos);
    void GetAwayFromTarget(Vector3 targetPos);

}
