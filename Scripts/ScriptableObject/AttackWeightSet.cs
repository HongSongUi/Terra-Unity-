using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewScriptableObjectScript", menuName = "Scriptable Objects/NewScriptableObjectScript")]
public class AttackWeightSet : ScriptableObject
{
    public List<int> nearWeights;
    public List<int> midWeights;
}
