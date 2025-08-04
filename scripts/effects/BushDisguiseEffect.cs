using AO;

// Effect that spawns a bush sprite which follows the player, giving the illusion of a disguise.
public class BushDisguiseEffect : MyEffect
{
    public override bool IsActiveEffect => false;
    public override bool BlockAbilityActivation => false;

    Entity bushEntity;

    public override void OnEffectStart(bool isDropIn)
    {
        // Create the bush entity and set up the sprite renderer

        // Will manually update position each frame to follow the player

        // Spawn on the server so other clients see it
        bushEntity = Entity.Create();
        bushEntity.Name = "BushDisguise";
        bushEntity.Position = Player.Entity.Position;
        bushEntity.LocalScale = new Vector2(1.2f, 1.2f);

        var sr = bushEntity.AddComponent<Sprite_Renderer>();
        sr.Texture = Assets.GetAsset<Texture>("housing/Hiding_Bushes.png");

        // Use built-in invisibility system so player sprite and animations are hidden
        Player.AddInvisibilityReason(nameof(BushDisguiseEffect));
    }

    public override void OnEffectUpdate()
    {
        // Ensure the bush tracks the player's position (extra safety in case parenting fails)
        if (bushEntity.Alive())
        {
            bushEntity.Position = Player.Entity.Position;
        }
    }

    public override void OnEffectEnd(bool interrupt)
    {
        // Restore visibility of the player
        Player.RemoveInvisibilityReason(nameof(BushDisguiseEffect));

        // Clean up the bush entity
        if (bushEntity.Alive())
        {
            bushEntity.Destroy();
        }
    }
}