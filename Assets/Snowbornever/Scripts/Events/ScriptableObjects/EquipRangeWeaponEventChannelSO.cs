using TopDownCharacter2D.Attacks.Range;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Equip Range Weapon Event Channel")]
public class EquipRangeWeaponEventChannelSO : DescriptionBaseSO
{
    public UnityAction<RangedAttackConfig> OnEventRaised;

    public void RaiseEvent(RangedAttackConfig value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}
