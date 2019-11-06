using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = PATH_NAME + "Bullet Row")]
public class BulletRow : ScriptableBossAbility
{
    [SerializeField]
    private GameObject m_AbilityPrefab;
    [SerializeField]
    private float m_BulletDistance = 2;
    [SerializeField]
    private LayerMask m_GameBorderLayer;

    private Transform m_Caster;

    public override IEnumerator CastAbility(Transform aCaster)
    {
        RaycastHit leftBorderHit, rightBorderHit;
        m_Caster = aCaster;
        MemberVariableSecurity();

        bool hasLeftHit = Physics.Raycast(aCaster.position, GetTransformRightNoRotation(), out leftBorderHit, 100f, m_GameBorderLayer);
        bool hasRightHit = Physics.Raycast(aCaster.position, -GetTransformRightNoRotation(), out rightBorderHit, 100f, m_GameBorderLayer);

        if (hasLeftHit && hasRightHit)
        {
            Vector3 m_StartPosition = leftBorderHit.point;
            float distanceBetweenPtns = Vector3.Distance(leftBorderHit.point, rightBorderHit.point);

            while (m_StartPosition.x >= rightBorderHit.point.x)
            {
                AutoAttack(m_StartPosition);
                m_StartPosition.x -= m_BulletDistance;
            }

            AudioManager.Instance.PlaySFX(m_AbilityAudio, m_Caster.position);
        }

        yield return null;
    }

    private void MemberVariableSecurity()
    {
        if (m_BulletDistance <= 0)
        {
            Debug.LogError("BulletRow - Bullet Distance cannot be 0 or lower. Source: " + m_Caster.name);
            m_BulletDistance = 1;
        }
    }

    private Vector3 GetTransformRightNoRotation()
    {
        Vector3 right;

        Quaternion casterRotation = m_Caster.rotation;
        m_Caster.rotation = Quaternion.identity;

        right = m_Caster.right;

        m_Caster.rotation = casterRotation;
        return right;
    }

    private void AutoAttack(Vector3 aSpawnPos)
    {
        Bullets projectile = PoolManager.Instance.UseObjectFromPool<Bullets>(m_AbilityPrefab, aSpawnPos, Quaternion.identity);
        projectile.BulletInit(m_AbilityDamage, m_AbilitySpeed);
    }
}
