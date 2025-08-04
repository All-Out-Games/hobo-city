using AO;
using System;
using System.Collections.Generic;

namespace Assembly.scripts;

public partial class ViewStarterPacksEffect : MyEffect
{
    public override bool IsActiveEffect => true;
    public override bool BlockAbilityActivation => true;
    public override bool BlockInteractables => true;
    public override bool FreezePlayer => true;

    private bool RequestedClose;

    public override void OnEffectStart(bool isDropIn)
    {
        if (Player.IsLocal)
        {
            SFX.Play(Assets.GetAsset<AudioAsset>("sfx/pull_lever.wav"), new SFX.PlaySoundDesc() { EntityToFollow = Player.Entity, Volume = 0.425f });
        }
    }

    public override void OnEffectUpdate()
    {
        if (Player.IsLocal)
        {
            DrawUI(new List<StarterPackInfo>()
            {
                GetCriminalPackInfo(),
                GetPolicePackInfo(),
            }, this, UI.ScreenRect);
        }
    }

    private struct StarterPackInfo
    {
        public ItemRarity Rarity;
        public bool Owned;
        public Purchasing.Product Product;
        public Purchasing.Product LoadoutProduct;
        public Purchasing.Product ArmourProduct;
        public int Gold;
        public string GoldSprite;
        public Vector4 GoldSpriteSize;
    }

    static StarterPackInfo GetCriminalPackInfo()
    {
        return new StarterPackInfo
        {
            Rarity = ItemRarity.Epic,
            Owned = Purchasing.OwnsGamePassLocal("682bce338e06ce588ed993c5"),
            Product = new Purchasing.Product()
            {
                Id = "682bce338e06ce588ed993c5",
                Name = "Criminal Pack",
                Description = "Contains <goldcount> Cash, an Assault Rifle and the Sherrari Car!",
                Price = 199,
                IsGamePass = true,
            },
            LoadoutProduct = new Purchasing.Product() { Price = 0, Id = "682bce338e06ce588ed993c5", Icon = Assets.GetAsset<Texture>("sprites/reusable-weapons/weapon_icons/assaultrifle.png") },
            ArmourProduct = new Purchasing.Product() { Price = 0, Icon = Assets.GetAsset<Texture>("icons/car-icons/ferrari.png") },
            Gold = 10000,
            GoldSprite = "icons/cash.png",
            GoldSpriteSize = new Vector4(60, 70, 60, 70)
        };
    }

    static StarterPackInfo GetPolicePackInfo()
    {
        return new StarterPackInfo
        {
            Rarity = ItemRarity.Rare,
            Owned = Purchasing.OwnsGamePassLocal("682bce5e45e3a031bd5baeda"),
            Product = new Purchasing.Product()
            {
                Id = "682bce5e45e3a031bd5baeda",
                Name = "Police Pack",
                Description = "Contains <goldcount> Cash, a Pistol and the Police Car!",
                Price = 199,
                IsGamePass = true,
            },
            LoadoutProduct = new Purchasing.Product() { Price = 0, Id = "682bce5e45e3a031bd5baeda", Icon = Assets.GetAsset<Texture>("icons/weapons/pistol.png") },
            ArmourProduct = new Purchasing.Product() { Price = 0, Icon = Assets.GetAsset<Texture>("icons/car-icons/police.png") },
            Gold = 10000,
            GoldSprite = "icons/cash.png",
            GoldSpriteSize = new Vector4(60, 70, 60, 70)
        };
    }

    // [UIPreview]
    // private static void TestDraw(Rect r) => DrawUI(new List<StarterPackInfo>()
    // {
    //     GetCriminalPackInfo(),
    //     GetPolicePackInfo(),
    // }, null, r);

    private static void DrawUI(List<StarterPackInfo> products, ViewStarterPacksEffect effect, Rect useRect)
    {
        using var _1 = UI.PUSH_LAYER(100000000);
        UI.Image(useRect, null, new Vector4(0.0f, 0.0f, 0.0f, 0.95f));

        // Build a centered panel roughly sized for two pack cards side-by-side
        var gridRect = UI.ScreenRect.CenterRect().Grow(300, 500, 300, 500);
        var grid = UI.GridLayout.Make(gridRect, products.Count, 1, UI.GridLayout.SizeSource.ElementCount, padding: 50);

        void Entry(Rect listRect, int index, bool owned, ItemRarity rarity, StarterPackInfo packInfo)
        {
            Texture bgTexture;

            var rarityColor = GameItems.GetColorForRarity(rarity);
            var pascalRarityColor = new Vector4(MathF.Min(MathF.Max(rarityColor.X, 0.1f) * 8, 1f), MathF.Min(MathF.Max(rarityColor.Y, 0.1f) * 8, 1f), MathF.Min(MathF.Max(rarityColor.Z, 0.1f) * 8, 1f), 1f);

            var artts = new UI.TextSettings
            {
                Font = UI.Fonts.BarlowBold,
                Size = 40,
                Color = rarityColor,
                DropShadowColor = new Vector4(0.3f, 0, 0, 0.8f),
                DropShadowOffset = new Vector2(5f, -5f),
                HorizontalAlignment = UI.HorizontalAlignment.Center,
                VerticalAlignment = UI.VerticalAlignment.Top,
                Outline = true,
                WordWrap = true,
                OutlineThickness = 4
            };

            var discts = new UI.TextSettings
            {
                Font = UI.Fonts.BarlowBold,
                Size = 40,
                Color = Vector4.Green,
                DropShadowColor = new Vector4(0.3f, 0, 0, 0.8f),
                DropShadowOffset = new Vector2(5f, -5f),
                HorizontalAlignment = UI.HorizontalAlignment.Center,
                VerticalAlignment = UI.VerticalAlignment.Top,
                Outline = true,
                WordWrap = true,
                OutlineThickness = 4
            };

            var descts = new UI.TextSettings
            {
                Font = UI.Fonts.BarlowBold,
                Size = 30,
                Color = new Vector4(0.95f, 0.95f, 0.95f, 1f),
                DropShadowColor = new Vector4(0.3f, 0, 0, 0.8f),
                DropShadowOffset = new Vector2(5f, -5f),
                HorizontalAlignment = UI.HorizontalAlignment.Center,
                VerticalAlignment = UI.VerticalAlignment.Top,
                Outline = true,
                WordWrap = true,
                OutlineThickness = 4
            };

            switch (rarity)
            {
                case ItemRarity.Mythic: bgTexture = Assets.GetAsset<Texture>("$AO/new/Shop Modal/Premium_Shop/item_backs/item_back_mythic.png"); break;
                case ItemRarity.Legendary: bgTexture = Assets.GetAsset<Texture>("$AO/new/Shop Modal/Premium_Shop/item_backs/item_back_legendary.png"); break;
                case ItemRarity.Epic: bgTexture = Assets.GetAsset<Texture>("$AO/new/Shop Modal/Premium_Shop/item_backs/item_back_epic.png"); break;
                case ItemRarity.Rare: bgTexture = Assets.GetAsset<Texture>("$AO/new/Shop Modal/Premium_Shop/item_backs/item_back_rare.png"); break;
                case ItemRarity.Uncommon: bgTexture = Assets.GetAsset<Texture>("$AO/new/Shop Modal/Premium_Shop/item_backs/item_back_uncommon.png"); break;
                default: bgTexture = Assets.GetAsset<Texture>("$AO/new/Shop Modal/Premium_Shop/item_backs/item_back_neutral.png"); break;
            }

            var rect = listRect; // Use full cell area to avoid large empty margin on the left

            var artRect = rect.SubRect(0, 1, 1f, 0.435f).Inset(0, 1, 0, 1);
            var descRect = rect.SubRect(0, 0f, 1f, 0.47f);
            UI.Image(artRect, bgTexture, Vector4.White, new UI.NineSlice() { slice = new Vector4(34, 34, 34, 34), sliceScale = 0.5f });

            var maskScope = IM.CreateMaskScope(artRect);
            {
                using var _123 = IM.BUILD_MASK_SCOPE(maskScope);
                UI.Image(artRect, Assets.GetAsset<Texture>("modal-9slice.png"));
            }
            {
                using var _323 = IM.USE_MASK_SCOPE(maskScope);

                var iconRect = artRect.CenterRect().Grow(90);
                UI.Image(iconRect.Grow(200).Offset(0, MathF.Sin(Time.TimeSinceStartup * 2f - index * 1.55f) * 5 + 25), Assets.GetAsset<Texture>("Shine.png"), rotationDegrees: Time.TimeSinceStartup * 60, tint: rarityColor);
                //UI.Image(iconRect.Offset(0, MathF.Sin(Time.TimeSinceStartup * 2f - index * 1.25f) * 5 + 5), packInfo.Product.Icon);
            }

            UI.Image(descRect, Assets.GetAsset<Texture>("modal-9slice.png"), pascalRarityColor, new UI.NineSlice() { slice = new Vector4(34, 34, 34, 34), sliceScale = 0.5f });

            //Temp probably
            {
                var goldts = new UI.TextSettings
                {
                    Font = UI.Fonts.BarlowBold,
                    Size = 43,
                    Color = new Vector4(0.95f, 0.95f, 0.95f, 1f),
                    DropShadowColor = new Vector4(0.3f, 0, 0, 0.8f),
                    DropShadowOffset = new Vector2(5f, -5f),
                    HorizontalAlignment = UI.HorizontalAlignment.Center,
                    VerticalAlignment = UI.VerticalAlignment.Top,
                    Outline = true,
                    OutlineThickness = 4
                };

                float xSpacing = 130f;

                var gunRect = artRect.CenterRect().Grow(70).Offset(0, -40).Offset(0, MathF.Sin(Time.TimeSinceStartup * 2f - index * 1.25f) * 5 + 5).FitAspect(packInfo.LoadoutProduct.Icon.Aspect);
                UI.Image(gunRect, packInfo.LoadoutProduct.Icon);

                var goldRect = artRect.CenterRect().Grow(packInfo.GoldSpriteSize.X, packInfo.GoldSpriteSize.Y, packInfo.GoldSpriteSize.Z, packInfo.GoldSpriteSize.W).Offset(-xSpacing, 40).Offset(0, MathF.Sin(Time.TimeSinceStartup * 2f - index * 1.25f) * 5 + 5).FitAspect(Assets.GetAsset<Texture>(packInfo.GoldSprite).Aspect);
                UI.Image(goldRect, Assets.GetAsset<Texture>(packInfo.GoldSprite));
                UI.TextAsync(goldRect.BottomCenterRect().Offset(0, 40), BBUtil.FormatNumber(packInfo.Gold), goldts);

                if (packInfo.ArmourProduct.Icon != null)
                {
                    var armourRect = artRect.CenterRect().Grow(40, 80, 40, 80).Offset(xSpacing, 40).Offset(0, MathF.Sin(Time.TimeSinceStartup * 2f - index * 1.25f) * 5 + 5).FitAspect(packInfo.ArmourProduct.Icon.Aspect);
                    UI.Image(armourRect, packInfo.ArmourProduct.Icon);
                }
            }

            UI.TextAsync(artRect.SubRect(0, 0f, 1f, 0f).Offset(0, -5f), packInfo.Product.Name.ToUpper(), artts);

            UI.TextAsync(descRect.SubRect(0.05f, 0.5f, 0.95f, 0.95f), packInfo.Product.Description.Replace("<goldcount>", BBUtil.FormatNumber(packInfo.Gold)), descts);

            var priceRect = descRect.SubRect(0.5f, 0f, 0.5f, 0f).Grow(35, 0, 35, 110);

            if (!owned)
            {
                priceRect = priceRect.Offset(packInfo.Product.Price.ToString().Length * 15 / 2f, 92);
                var pricets = UIUtils.GetTextSettings(41);
                pricets.HorizontalAlignment = UI.HorizontalAlignment.Right;
                priceRect = UI.TextSync(priceRect, packInfo.Product.Price.ToString(), pricets);
                UI.Image(priceRect.RightCenterRect().Grow(20, 15, 20, 15).Offset(20, 0), Assets.GetAsset<Texture>("$AO/new/Shop Modal/Premium_Shop/small_spark_icon.png"));
            }

            var bs = new UI.ButtonSettings()
            {
                BackgroundColorMultiplier = owned ? new Vector4(196f / 255f, 187f / 255f, 18f / 255f, 1f) : Vector4.White,
                Sprite = Assets.GetAsset<Texture>(owned ? "$AO/new/modal/buttons/button_10.png" : "$AO/new/modal/buttons/button_2.png"),
                Slice = new UI.NineSlice() { slice = new Vector4(34, 34, 34, 34), sliceScale = 0.5f }
            };
            var buttonts = UIUtils.GetTextSettings(40);
            buttonts.Offset = new Vector2(0, 7);
            var buttonRect = descRect.SubRect(0.5f, 0f, 0.5f, 0f).Grow(35, 110, 35, 110).Offset(0, 40);
            {
                UI.PushId(packInfo.Product.Name);
                if (UI.Button(buttonRect, owned ? "Purchased!" : "Buy", bs, buttonts).JustPressed && effect != null && !effect.RequestedClose)
                {
                    Purchasing.PromptPurchase(packInfo.Product.Id);
                }

                UI.PopId();
            }
        }

        for (int i = 0; i < products.Count; i++)
        {
            var info = products[i];
            Entry(grid.Next(), 0, info.Owned, info.Rarity, info);
        }

        var buttonts = UIUtils.GetTextSettings(40);
        buttonts.Offset = new Vector2(0, 7);
        if (UI.Button(gridRect.BottomRightRect().Grow(35, 90, 35, 90).Offset(80, 30), "Close", new UI.ButtonSettings() { BackgroundColorMultiplier = Vector4.Red, Sprite = Assets.GetAsset<Texture>("$AO/new/modal/buttons/button_10.png"), Slice = new UI.NineSlice() { slice = new Vector4(34, 34, 34, 34), sliceScale = 0.5f } }, buttonts).JustPressed && effect != null)
        {
            effect.RequestedClose = true;
            CallServer_RequestCloseLoadoutStore(MyPlayer.localPlayer);
        }
    }

    [ServerRpc]
    public static void RequestCloseLoadoutStore(MyPlayer player) => CallClient_CloseLoadoutStore(player);
    [ClientRpc]
    public static void CloseLoadoutStore(MyPlayer player)
    {
        player.RemoveEffect<ViewStarterPacksEffect>(false);
    }
}