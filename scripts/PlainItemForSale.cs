using AO;
using System;

public partial class PlainItemForSale : Component
{
  [Serialized] public Interactable Interactable;

  [Serialized] public string ItemId;
  [Serialized] public int Price;
  [Serialized] public bool Consumable = false;
  [Serialized] public string SparkProductId;
  [Serialized] public bool UseBitcoin = false;
  private string currency;
  private string cachedPriceText;


  public Item_Definition ItemDef;

  // Cache for ownership check
  bool cachedOwnsItem = false;
  bool cacheInitialized = false;

  public override void Awake()
  {
    var seed = RNG.Seed(Entity.Id);

    currency = GameManager.CASH_CURRENCY;

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

    ItemDef = GameManager.Instance.GameItems.ItemPool.FirstOrDefault(item => item.ItemDefinition.Id == ItemId)?.ItemDefinition;
    Interactable.OnInteract += OnInteract;

    if (ItemDef == null)
    {
      Log.Error($"ItemStand: ItemId {ItemId} not found for {Entity.Name}");
      Entity.LocalEnabled = false;
      return;
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
    if (MyPlayer.localPlayer.Alive() == false) return;
    if (false == MyPlayer.localPlayer.CurrentCameraControlWorldRect.Overlaps(new Rect(Entity.Position - new Vector2(2, 2), Entity.Position + new Vector2(2, 2)))) return;

    // Refresh ownership status:
    //  1) Initialize cache on the first frame
    //  2) Re-evaluate while it is marked as not owned so we can detect a successful purchase
    if (!cacheInitialized || !cachedOwnsItem)
    {
      UpdateOwnershipCache();
    }

    if (!Consumable)
    {
      Interactable.Text = cachedOwnsItem
        ? $"{ItemDef.Name} (Owned)"
        : $"Buy {ItemDef.Name} - {cachedPriceText}";
    }
    else
    {
      Interactable.Text = $"Buy {ItemDef.Name} - {cachedPriceText}";
    }
  }

  public void OnInteract(Player _player)
  {
    // just assume it went thru so we don't have do a client rpc which could cause desync if it gets there late
    UpdateOwnershipCache();

    var player = (MyPlayer)_player;
    if (!player.Alive()) return;

    var ownsItem = player.DefaultInventory.Items.FirstOrDefault(item => item != null && item.Definition.Id == ItemId);

    if (SparkProductId != null)
    {
      if (Network.LocalPlayer == _player)
      {
        if (ownsItem != null)
        {
          Notifications.Show($"You already have this item!");
          return;
        }

        Purchasing.PromptPurchase(SparkProductId);
      }
      return;
    }

    if (!Network.IsServer) return;

    if (ownsItem != null && !Consumable)
    {
      GameManager.CallClient_SendTargetedMessage($"Tap the {ItemDef.Name} in your inventory to use it!", new RPCOptions(target: player));
      return;
    }

    if (Economy.GetBalance(player, currency) >= Price)
    {
      if (ItemDef.Id.StartsWith("__AMMO__"))
      {
        if (Enum.TryParse<AmmoType>(ItemDef.Id.Substring(8), out var ammoType))
        {
          player.ServerSyncAmmoAmount(ammoType, player.AmmoAmounts[ammoType].CurrentAmount + 18);
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

      Economy.WithdrawCurrency(player, currency, Price);
      player.CallClient_ThrowMoney(player);
    }
    else
    {
      GameManager.CallClient_SendTargetedMessage($"You need more {currency} (destroy objects, kill players, mine bitcoin)", new RPCOptions(target: player));
    }
  }
}
