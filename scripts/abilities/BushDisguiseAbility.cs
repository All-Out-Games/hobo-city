using AO;
using System.Collections.Generic;

// Ability that lets the player turn into a moving bush for stealth.
public class BushDisguiseAbility : MyAbility
{
    public override TargettingMode TargettingMode => TargettingMode.Self;
    public override bool DrawAimingIndicators => false;
    public override float Cooldown => 1.0f;

    // Re-use the hiding bush sprite as the icon. If it is too large the UI will scale it automatically.
    public override Texture Icon => Assets.GetAsset<Texture>("housing/Hiding_Bushes.png");

    public override bool CanTarget(Player player) => true;

    public override bool CanUse()
    {
        // Cannot use while dead, driving/flying/boating or inside a house different from outside room.
        if (Player.HealthManager.Health <= 0) return false;
        return true;
    }

    // Sparks product / gamepass id that unlocks the disguise.
    const string GAMEPASS_ID = "68409c5cac3d6521ff439755";

    public override bool OnTryActivate(Player.AbilityActivationInfo info)
    {
        var myPlayer = (MyPlayer)info.TargetPlayers[0];

        if (Network.IsClient)
        {
            if (!Purchasing.OwnsGamePassLocal(GAMEPASS_ID))
            {
                if (myPlayer.IsLocal)
                {
                    Purchasing.PromptPurchase(GAMEPASS_ID);
                }
                return false;
            }
        }
        else
        {
            if (!Purchasing.OwnsGamePass(myPlayer, GAMEPASS_ID))
            {
                return false;
            }
        }

        // Toggle the disguise effect
        if (myPlayer.HasEffect<BushDisguiseEffect>())
        {
            myPlayer.RemoveEffect<BushDisguiseEffect>(true);
        }
        else
        {
            myPlayer.AddEffect<BushDisguiseEffect>();
        }

        return true;
    }
}