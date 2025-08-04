using AO;
using ReusableWeapons;

public class Fists : CustomItemDefinition
{
    public Fists(Item_Definition itemDef, ItemRarity rarity, ItemCategory category, int basePickupAmount = 1) : base(itemDef, rarity, category, basePickupAmount) { }

    public override void OnEquip(MyPlayer player) { }

    public override void OnUnequip(MyPlayer player) { }

    public override Ability GetPrimaryAbility(MyPlayer player) => player.GetAbility<PunchAbility>();

}
