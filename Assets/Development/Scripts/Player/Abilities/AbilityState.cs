using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Abilities/Ability")]
public class AbilityState : ScriptableObject
{
    public string abilityName;
    public bool isUnlocked;
    public float cooldown;
}
