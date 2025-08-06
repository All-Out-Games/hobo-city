using AO;
using System;

public partial class ItemStand : Component
{
  [Serialized] public Interactable Interactable;
  [Serialized] public Entity Visual;


  [Serialized] public string ItemId;
  [Serialized] public int Price;
  [Serialized] public float VisualScale = 1.0f;
  [Serialized] public bool Consumable = false;
  [Serialized] public bool UseBitcoin = false;
  [Serialized] public string SparkProductId;
  [Serialized] public string DisplayName;
  private string currency;
  [Serialized] public string CosmeticId;

  float startY;
  float bobAmount = 0.2f;
  float bobSpeed = 1.75f;
  float bobPhaseOffset = 0f;

  public Item_Definition ItemDef;

  // Cache for ownership check
  bool cachedOwnsItem = false;
  bool cacheInitialized = false;

  // Cache for formatted prices to avoid expensive BBUtil.FormatNumber calls
  private string cachedFormattedPrice;
  private string cachedPriceText;

  public string OwnedNonConsumableText;
  public string UnownedNonConsumableText;
  public string BuyConsumableText;

  public override void Awake()
  {
    var seed = RNG.Seed(Entity.Id);

    currency = UseBitcoin ? GameManager.BITCOIN_CURRENCY : GameManager.CASH_CURRENCY;

    // Only try to look up an ItemDef if an ItemId was supplied
    if (!string.IsNullOrEmpty(ItemId))
    {
      ItemDef = GameManager.Instance.GameItems.ItemPool.FirstOrDefault(item => item.ItemDefinition.Id == ItemId)?.ItemDefinition;
    }
    Interactable.OnInteract += OnInteract;

    // If we have neither an ItemDef nor a CosmeticId (e.g. pets) we just rely on DisplayName for UI text.
    if (ItemDef == null && CosmeticId == null && string.IsNullOrEmpty(DisplayName))
    {
      Log.Error($"ItemStand: ItemId {ItemId} not found");
      Entity.LocalEnabled = false;
      return;
    }


    if (CosmeticId == null)
    {
      // Cache formatted price strings to avoid expensive BBUtil.FormatNumber calls every frame
      cachedFormattedPrice = BBUtil.FormatNumber(Price);
      string currencySymbol = UseBitcoin ? "BTC " : "$";
      // Cache the price text at startup
      if (SparkProductId != null)
      {
        cachedPriceText = $"{BBUtil.FormatNumber(Price)} sparks";
      }
      else if (UseBitcoin)
      {
        cachedPriceText = $"{BBUtil.FormatNumber(Price)} BTC";
      }
      else
      {
        cachedPriceText = $"${BBUtil.FormatNumber(Price)}";
      }

      // Determine a display name whether we have an ItemDef or are using a custom DisplayName
      var displayName = ItemDef != null ? ItemDef.Name : DisplayName;

      OwnedNonConsumableText = $"{displayName} (Owned)";
      UnownedNonConsumableText = $"Buy {displayName} - {cachedPriceText}";
      BuyConsumableText = $"Buy {displayName} - {cachedPriceText}";
    }


    if (Visual.Alive())
    {
      // Only set the texture if it's not a cosmetic which we have to do manually
      if (CosmeticId == null && ItemDef != null)
      {
        Visual.GetComponent<Sprite_Renderer>().Texture = Assets.GetAsset<Texture>(ItemDef.Icon);
      }

      Visual.Scale = new Vector2(VisualScale, VisualScale);

      startY = Visual.Position.Y;
      // Add slight randomness to each weapon stand's bob speed and phase
      RNG.Seed(1337);
      bobSpeed = RNG.RangeFloat(ref seed, 1.8f, 2.2f);
      bobPhaseOffset = RNG.RangeFloat(ref seed, 0f, (float)Math.PI * 2f);
    }
  }

  void UpdateOwnershipCache()
  {
    if (MyPlayer.localPlayer.Alive() && ItemDef != null)
    {
      cachedOwnsItem = MyPlayer.localPlayer.DefaultInventory.Items.FirstOrDefault(item => item != null && item.Definition.Id == ItemId) != null;
      cacheInitialized = true;
    }
  }

  public override void Update()
  {
    if (MyPlayer.localPlayer.Alive() && false == MyPlayer.localPlayer.CurrentCameraControlWorldRect.Overlaps(new Rect(Entity.Position - new Vector2(2, 2), Entity.Position + new Vector2(2, 2)))) return;

    if (MyPlayer.localPlayer.Alive())
    {
      // Initialize cache on first update if not already done
      if (!cacheInitialized)
      {
        UpdateOwnershipCache();
      }

      if (CosmeticId != null)
      {
        if (!Network.LocalPlayer.Alive()) return;

        if (Cosmetics.OwnsCosmetic(Network.LocalPlayer, CosmeticId))
        {
          Interactable.Text = "Equip Outfit";
        }
        else
        {
          Interactable.Text = "Buy This Outfit";
        }
      }
      else
      {
        if (!Consumable)
        {
          if (cachedOwnsItem)
          {
            Interactable.RequiredHoldTime = 0f;
          }
          Interactable.Text = cachedOwnsItem ? OwnedNonConsumableText : UnownedNonConsumableText;
        }
        else
        {
          Interactable.Text = BuyConsumableText;
        }
      }

    }

    if (Visual.Alive())
    {
      // Create a smooth bobbing effect using sin wave with phase offset
      float bobOffset = (float)Math.Sin((Time.TimeSinceStartup * bobSpeed) + bobPhaseOffset) * bobAmount;

      // Apply the bobbing to the visual's position
      Vector2 currentPos = Visual.Position;
      Vector2 targetPos = new Vector2(currentPos.X, startY + bobOffset);

      // Smoothly lerp to the target position
      Visual.Position = Vector2.Lerp(currentPos, targetPos, Time.DeltaTime * 3.0f);
    }
  }

  public void OnInteract(Player _player)
  {
    // Spark product purchase flow
    if (SparkProductId != null)
    {
      if (Network.LocalPlayer == _player)
      {
        var playerSpark = (MyPlayer)_player;
        // Prevent purchase if already owned and not consumable
        if (!string.IsNullOrEmpty(ItemId))
        {
          var ownsItemSpark = playerSpark.DefaultInventory.Items.FirstOrDefault(item => item != null && item.Definition.Id == ItemId);
          if (ownsItemSpark != null && !Consumable)
          {
            Notifications.Show($"You already have this item!");
            return;
          }
        }

        Purchasing.PromptPurchase(SparkProductId);
      }
      return; // Do not continue to server-side currency flow
    }

    // Cosmetic flow
    if (CosmeticId != null)
    {
      if (Network.LocalPlayer != _player) return;
      if (!Network.IsClient) return;

      if (Cosmetics.OwnsCosmetic(_player, CosmeticId))
      {
        Cosmetics.EquipCosmetic(CosmeticId);
      }
      else
      {
        Cosmetics.PromptPurchase(CosmeticId);
      }

      return;
    }
    // Regular flow

    // just assume it went thru so we don't have do a client rpc which could cause desync if it gets there late
    UpdateOwnershipCache();

    if (!Network.IsServer) return;

    var player = (MyPlayer)_player;

    var ownsItem = player.DefaultInventory.Items.FirstOrDefault(item => item != null && item.Definition.Id == ItemId);

    if (ownsItem != null && !Consumable)
    {
      GameManager.CallClient_SendTargetedMessage($"Tap the {ItemDef.Name} in your inventory to use it!", new RPCOptions(target: player));
      return;
    }

    if (Economy.GetBalance(player, currency) >= (currency == GameManager.BITCOIN_CURRENCY ? Price * 100 : Price))
    {
      if (ItemDef.Id.StartsWith("__AMMO__"))
      {
        if (Enum.TryParse<AmmoType>(ItemDef.Id.Substring(8), out var ammoType))
        {
          player.ServerSyncAmmoAmount(ammoType, player.AmmoAmounts[ammoType].CurrentAmount + 52);
          player.CallClient_ThrowMoney(player);
        }
        else
        {
          Log.Error($"Failed to parse ammo type from item: {ItemDef.Id}");
          return;
        }
      }
      else
      {
        player.ServerTryAddItem(ItemDef);
      }

      player.CallClient_ThrowMoney(player);
      Economy.WithdrawCurrency(player, currency, currency == GameManager.BITCOIN_CURRENCY ? Price * 100 : Price);
    }
    else
    {
      GameManager.CallClient_SendTargetedMessage($"You need more {currency} (destroy objects, kill players, mine bitcoin)", new RPCOptions(target: player));
    }
  }
}
