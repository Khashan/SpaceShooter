using System.Collections;
using UnityEngine;

public abstract class ScriptableBossAbility : ScriptableObject
{
    protected const string PATH_NAME = "ScriptableObject/Ability/";

    [SerializeField]
    protected float m_AbilitySpeed;
    [SerializeField]
    protected int m_AbilityDamage;
    [SerializeField]
    protected AudioClip m_AbilityAudio;

    public abstract IEnumerator CastAbility(Transform aCaster);
}
