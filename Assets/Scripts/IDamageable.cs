using UnityEngine;

public interface IDamageable
{
    void DamageReceived(int aDamageReceived);
    void HealReceived(int aHealReceived);
}
