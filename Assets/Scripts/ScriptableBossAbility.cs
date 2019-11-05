using UnityEngine;

public abstract class ScriptableBossAbility : ScriptableObject
{
    [SerializeField]
    protected float m_AbilitySpeed;
    [SerializeField]
    protected int m_AbilityDamage;

    public abstract void CastAbility(Transform aCaster);
}
