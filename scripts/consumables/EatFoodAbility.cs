using AO;

public abstract class EatFoodAbility : MyAbility
{
    public override TargettingMode TargettingMode => TargettingMode.Self;
    public override bool DrawAimingIndicators => false;
    public override Texture Icon => Assets.GetAsset<Texture>(FoodItem.Icon);

    public abstract Item_Definition FoodItem { get; }
    public abstract string FoodTrigger { get; }
    public abstract int HpAmountToRestore { get; }

    public override bool CanTarget(Player player) => true;
    public override bool CanUse()
    {
        return Player.HealthManager.Health > 0;
    }

    public override bool OnTryActivate(Player.AbilityActivationInfo info)
    {
        var player = (MyPlayer)info.TargetPlayers[0];
        if (Network.IsServer)
        {
            player.RequestRemoveItemCountFromSlot(player.CurrentHoveredSlot, 1);
        }

        player.AddEffect<EatFoodEffect>(preInit: effect =>
        {
            effect.FoodItem = FoodItem;
            effect.FoodTrigger = FoodTrigger;
            effect.FoodHealthRestore = HpAmountToRestore;
        });

        return true;
    }
}

public class EatFoodEffect : MyEffect
{
    public override bool IsActiveEffect => true;
    public override bool BlockAbilityActivation => true;
    public override float DefaultDuration => 1.4f;

    public string FoodTrigger;
    public int FoodHealthRestore;
    public Item_Definition FoodItem;
    public bool IsDrink;

    public override void OnEffectStart(bool isDropIn)
    {
        base.OnEffectStart(isDropIn);

        if (IsDrink)
        {
            SFX.Play(Assets.GetAsset<AudioAsset>("sfx/drink.wav"), new SFX.PlaySoundDesc { Volume = 0.5f, Position = Entity.Position, Positional = true });
        }
        else
        {
            SFX.Play(Assets.GetAsset<AudioAsset>("sfx/eat.wav"), new SFX.PlaySoundDesc { Volume = 0.5f, Position = Entity.Position, Positional = true });
        }

        Player.SpineAnimator.SpineInstance.StateMachine.SetTrigger(FoodTrigger);

        Player.BlockScrollReasons.Add(nameof(EatFoodEffect));
    }

    public override void OnEffectEnd(bool interrupt)
    {
        base.OnEffectEnd(interrupt);

        Player.BlockScrollReasons.Remove(nameof(EatFoodEffect));

        if (!interrupt && Network.IsServer)
        {
            var amountToRestore = FoodHealthRestore;

            Player.HealthManager.Health += amountToRestore;
        }

        // dropin case
        if (FoodItem == null)
        {
            return;
        }

        if (FoodItem.Id == "__HEALING__EnergyDrink")
        {
            Player.AddEffect<EnergyEffect>();
        }

        if (FoodItem.Id == "__HEALING__Smoothie")
        {
            Player.AddEffect<WonkyScreenEffect>();
        }
    }
}

public class EatAppleAbility : EatFoodAbility
{
    public override Item_Definition FoodItem => GameManager.Instance.GameItems.Apple.ItemDefinition;
    public override string FoodTrigger => "apple";
    public override int HpAmountToRestore => 25;
}

public class EatBurgerAbility : EatFoodAbility
{
    public override Item_Definition FoodItem => GameManager.Instance.GameItems.Burger.ItemDefinition;
    public override string FoodTrigger => "apple";
    public override int HpAmountToRestore => 55;
}

public class EatEnergyDrinkAbility : EatFoodAbility
{
    public override Item_Definition FoodItem => GameManager.Instance.GameItems.EnergyDrink.ItemDefinition;
    public override string FoodTrigger => "energy_drink";
    public override int HpAmountToRestore => 10;
}

public class EatPopcornAbility : EatFoodAbility
{
    public override Item_Definition FoodItem => GameManager.Instance.GameItems.Popcorn.ItemDefinition;
    public override string FoodTrigger => "apple";
    public override int HpAmountToRestore => 10;
}

public class EatHotdogAbility : EatFoodAbility
{
    public override Item_Definition FoodItem => GameManager.Instance.GameItems.Hotdog.ItemDefinition;
    public override string FoodTrigger => "apple";
    public override int HpAmountToRestore => 50;
}

public class EatSmoothieAbility : EatFoodAbility
{
    public override Item_Definition FoodItem => GameManager.Instance.GameItems.Smoothie.ItemDefinition;
    public override string FoodTrigger => "energy_drink";
    public override int HpAmountToRestore => 35;
}

// public class EatPorkAbility : EatFoodAbility
// {
//     public override Item_Definition FoodItem => GameManager.Instance.GameItems.Pork;
//     public override string FoodSkin => "Meat";
//     public override float HpAmountToRestore => 25.0f;
// }

// public class EatSteakAbility : EatFoodAbility
// {
//     public override Item_Definition FoodItem => GameManager.Instance.GameItems.Steak;
//     public override string FoodSkin => "Steak";
//     public override float HpAmountToRestore => 50.0f;
// }