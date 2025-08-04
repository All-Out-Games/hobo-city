using AO;

namespace ReusableWeapons
{
    /// <summary>
    /// The core of the DamageNumber system is taken from Mimicer
    /// </summary>
    public class DamageNumber
    {
        public string Text;
        public Vector2 Position;
        public Vector4 Color;
        public float T;
        public UI.TextSettings TextSettings;
        public bool SpaceText;
        public bool DoingFading = false;
        public Vector2 LastPosition;
    }

    public class DamageNumberManager : Singleton<_DamageNumberManager> { }
    public class _DamageNumberManager : Component
    {
        public List<DamageNumber> ActiveDamageNumbers = new();

        public void DrawDamageNumbers()
        {
            using var _1 = UI.PUSH_CONTEXT(UI.Context.WORLD);
            using var _2 = UI.PUSH_LAYER(5);

            List<DamageNumber> numbers = ActiveDamageNumbers;
            for (int i = numbers.Count - 1; i >= 0; i -= 1)
            {
                var result = numbers[i];
                float speed = 0.5f;
                var ts = result.TextSettings;

                result.T += Time.DeltaTime * speed;
                if (result.T >= 1 && result.DoingFading)
                {
                    numbers.UnorderedRemoveAt(i);
                    continue;
                }
                if (result.T >= 1 && !result.DoingFading)
                {
                    result.T = 0.0f;
                    result.DoingFading = true;
                }

                if (!result.DoingFading)
                {
                    var pos = result.Position;
                    pos.Y += AOMath.Lerp(0, 0.5f, Ease.OutQuart(result.T));
                    var color01 = Ease.FadeInAndOut(0.1f, 1f, result.T);
                    ts.Color = Vector4.Lerp(ts.Color, result.Color, color01);
                    result.LastPosition = pos;
                }
                else
                {
                    ts.SpacingMultiplier = 1f;
                    var colorAlpha = Vector4.Zero;
                    ts.Color = Vector4.Lerp(ts.Color, colorAlpha, result.T);
                }

                var rect = new Rect(result.LastPosition, result.LastPosition);
                UI.TextAsync(rect, result.Text, ts);
            }
        }

        public void TrySpawnDamageNumber(Entity entity, int healthDiff)
        {
            if (healthDiff != 0)
            {
                Vector4 textColor = healthDiff > 0 ? Vector4.Green : Vector4.Red;
                string prefix = healthDiff > 0 ? "+" : "";

                SpawnDamageNumber(UI.Fonts.Barlow, entity.Position, textColor, $"{prefix}{healthDiff.ToString("F0")} HP");
            }
        }

        public void SpawnDamageNumber(FontAsset font, Vector2 worldPosition, Vector4 color, string text, float size = 0.3f, float slant = 0.0f, bool spaceText = false)
        {
            float randX = Random.Shared.NextFloat(-1f, 1f);
            float randY = Random.Shared.NextFloat(1f, 1.5f);
            var searchResult = new DamageNumber();
            searchResult.Text = text;
            searchResult.Position = worldPosition + new Vector2(randX, randY);
            searchResult.Color = color;
            searchResult.T = 0;
            searchResult.TextSettings = GetTextSettingsDamageNumbers(font, Game.IsMobile ? 0.5f : size, color, slant);
            searchResult.SpaceText = spaceText;
            ActiveDamageNumbers.Add(searchResult);
        }

        public UI.TextSettings GetTextSettingsDamageNumbers(FontAsset font, float size, Vector4 color, float slant)
        {
            var ts = new UI.TextSettings()
            {
                Font = font,
                Size = size,
                Color = color,
                DropShadowColor = new Vector4(0f, 0f, 0f, 1f),
                DropShadowOffset = new Vector2(0f, -3f),
                HorizontalAlignment = UI.HorizontalAlignment.Center,
                VerticalAlignment = UI.VerticalAlignment.Center,
                WordWrap = false,
                WordWrapOffset = 0,
                Outline = true,
                OutlineThickness = 3,
                Slant = slant,
            };
            return ts;
        }
    }
}