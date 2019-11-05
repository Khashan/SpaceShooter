﻿using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = PATH_NAME + "BossNova")]
public class BulletNova : ScriptableBossAbility
{
    [SerializeField]
    private GameObject m_AbilityPrefab;

    [Header("Nova Mathermatics")]
    [SerializeField]
    private int m_DegreeGaps = 15;
    [SerializeField]
    private int m_Radius;

    private Transform m_Caster;

    public override IEnumerator CastAbility(Transform aCasting)
    {
        if (m_DegreeGaps <= 0)
        {
            Debug.LogError("Bad BulletNova setting, DegreeGaps cannot be set at 0 or lower. Source: " + aCasting.name);
            m_DegreeGaps = 15;
        }

        m_Caster = aCasting;

        int rotationGaps = m_DegreeGaps;
        int rotationDone = rotationGaps;

        Quaternion holderCasterRotation = aCasting.rotation;
        m_Caster.rotation = Quaternion.identity;

        while (rotationDone <= m_Radius)
        {
            aCasting.Rotate(Vector3.up * rotationGaps);
            rotationDone += rotationGaps;
            AutoAttack();
        }

        AudioManager.Instance.PlaySFX(m_AbilityAudio, m_Caster.position);
        m_Caster.rotation = holderCasterRotation;

        yield return null;
    }

    private void AutoAttack()
    {
        Bullets projectile = PoolManager.Instance.UseObjectFromPool<Bullets>(m_AbilityPrefab, m_Caster.position, m_Caster.rotation);
        projectile.BulletInit(m_AbilityDamage, m_AbilitySpeed);
    }
}