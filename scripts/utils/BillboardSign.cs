using AO;

public partial class BillboardSign : Component
{
    [Serialized] public Texture Texture;
    [Serialized] public string Message;
    [Serialized] public float HalfWidth = 0.5f;

    public float NearSinceTime;

    public override void Update()
    {
        if (Network.IsServer) return;
        if (MyPlayer.localPlayer == null) return;

        if (false == MyPlayer.localPlayer.CurrentCameraControlWorldRect.Overlaps(new Rect(Entity.Position - new Vector2(2, 2), Entity.Position + new Vector2(2, 2)))) return;

        var localPlayer = (MyPlayer)Network.LocalPlayer;
        if ((localPlayer.Entity.Position - Entity.Position).Length > 2)
        {
            NearSinceTime = Time.TimeSinceStartup;
        }
        else
        {
            // Animate the sign so that it "projects" upward from the ground instead of jiggling.
            // Progress goes from 0 -> 1 in 0.75 s after the player comes near.
            float appearT = Ease.T(Time.TimeSinceStartup - NearSinceTime, 0.75f);
            float popT = Ease.OutQuart(appearT);

            // Vertical offset: start 1 world unit below and move up to the final position.
            float verticalOffset = AOMath.Lerp(-1.0f, 0.0f, popT);

            // Scale animation: grow from 0.8x to the original 1.5x size during the pop.
            float scaleFactor = AOMath.Lerp(0.8f, 1.5f, popT);

            UI.PushContext(UI.Context.WORLD); using var _1 = AllOut.Defer(UI.PopContext);
            UI.PushScaleFactor(scaleFactor); using var _2 = AllOut.Defer(UI.PopScaleFactor);
            UI.PushLayerRelative(2); using var _3 = AllOut.Defer(UI.PopLayer);

            var pos = Entity.Position + new Vector2(0, 1.0f + verticalOffset);
            var adjustedHalfWidth = Game.IsMobile ? HalfWidth * 3 : HalfWidth * 2;
            var rect = new Rect(pos, pos).CenterRect().OffsetUnscaled(0, 1f).Grow(0, adjustedHalfWidth, 0, adjustedHalfWidth);

            int bgSerial = IM.GetNextSerial();

            Rect contentRect;
            if (Texture != null)
            {
                contentRect = rect.Grow(1.5f);
                UI.PushLayerRelative(1); using var _4 = AllOut.Defer(UI.PopLayer); // Ensure image is on top
                UI.Image(contentRect.Offset(0, 1.4f).FitAspect(Texture.Aspect), Texture, new Vector4(1, 1, 1, 1)); // Full opacity white
            }
            else
            {
                // Display the text message
                contentRect = UI.TextSync(rect, Message, new UI.TextSettings()
                {
                    Font = UI.Fonts.Barlow,
                    Size = Game.IsMobile ? 0.3f : 0.2f,
                    Color = Vector4.White,
                    HorizontalAlignment = UI.HorizontalAlignment.Center,
                    VerticalAlignment = UI.VerticalAlignment.Center,
                    Outline = true,
                    OutlineThickness = 3,
                    WordWrap = true,
                });

                // Always draw the frame
                IM.SetNextSerial(bgSerial);
                UI.Image(contentRect.Grow(0.15f), null, new Vector4(0.1f, 0.1f, 0.1f, 1));

            }

        }
    }
}