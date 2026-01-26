using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationEventHandler : MonoBehaviour
{
    public event Action MonsterDesolve;
    public event Action<bool> OnMonsterAttack;
    public void DestroyMonster()
    {
        MonsterDesolve?.Invoke();
    }
    public void MonsterAttackStart()
    {
        OnMonsterAttack?.Invoke(true);
    }
    public void MonsterAttackEnd()
    {
        OnMonsterAttack?.Invoke(false);
    }
}
