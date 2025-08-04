using AO;
using ReusableWeapons;

public abstract class Food : CustomItemDefinition
{
    public Food(Item_Definition itemDef, ItemRarity rarity, ItemCategory category, int basePickupAmount = 1) : base(itemDef, rarity, category, basePickupAmount) { }

    public override void OnEquip(MyPlayer player) { }

    public override void OnUnequip(MyPlayer player) { }
}

public class Apple : Food
{
    public Apple(Item_Definition itemDef, ItemRarity rarity, ItemCategory category, int basePickupAmount = 1) : base(itemDef, rarity, category, basePickupAmount) { }

    public override Ability GetPrimaryAbility(MyPlayer player) => player.GetAbility<EatAppleAbility>();
}

public class Burger : Food
{
    public Burger(Item_Definition itemDef, ItemRarity rarity, ItemCategory category, int basePickupAmount = 1) : base(itemDef, rarity, category, basePickupAmount) { }

    public override Ability GetPrimaryAbility(MyPlayer player) => player.GetAbility<EatBurgerAbility>();
}

public class EnergyDrink : Food
{
    public EnergyDrink(Item_Definition itemDef, ItemRarity rarity, ItemCategory category, int basePickupAmount = 1) : base(itemDef, rarity, category, basePickupAmount) { }

    public override Ability GetPrimaryAbility(MyPlayer player) => player.GetAbility<EatEnergyDrinkAbility>();
}

public class Popcorn : Food
{
    public Popcorn(Item_Definition itemDef, ItemRarity rarity, ItemCategory category, int basePickupAmount = 1) : base(itemDef, rarity, category, basePickupAmount) { }

    public override Ability GetPrimaryAbility(MyPlayer player) => player.GetAbility<EatPopcornAbility>();
}

public class Hotdog : Food
{
    public Hotdog(Item_Definition itemDef, ItemRarity rarity, ItemCategory category, int basePickupAmount = 1) : base(itemDef, rarity, category, basePickupAmount) { }

    public override Ability GetPrimaryAbility(MyPlayer player) => player.GetAbility<EatHotdogAbility>();
}

public class Smoothie : Food
{
    public Smoothie(Item_Definition itemDef, ItemRarity rarity, ItemCategory category, int basePickupAmount = 1) : base(itemDef, rarity, category, basePickupAmount) { }

    public override Ability GetPrimaryAbility(MyPlayer player) => player.GetAbility<EatSmoothieAbility>();
}

