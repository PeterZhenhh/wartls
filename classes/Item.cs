// Decompiled with JetBrains decompiler
// Type: OnyxServer.CLASSES.Item
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using System;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.CLASSES
{
  public class Item
  {
    public string BundleTime = "0";
    public string BundleQuantity = "0";
    public ItemType ItemType;
    public long ID;
    public string Name;
    public string Config;
    public string AttachedTo;
    public byte Equipped;
    public int Slot;
    public long BuyTime;
    public long ExpirationTime;
    public bool ExpiredConfirmed;
    public long TotalDurabilityPoints;
    public long DurabilityPoints;
    public int Quantity;
    public long RepairCost;

    public long SecondsLeft
    {
      get
      {
        if (this.ItemType == ItemType.CONSUMABLE || this.ItemType == ItemType.NO_REPAIR)
          return 0;
        long num = this.ExpirationTime - DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        if (num < 0L)
          return 0;
        this.ExpiredConfirmed = false;
        return num;
      }
    }

    public Item()
    {
    }

    public Item(
      ItemType ItemType,
      long Id,
      string Name,
      int Hours = 0,
      int Quantity = 0,
      long DurabilityPoints = 36000)
    {
      this.ItemType = ItemType;
      this.AttachedTo = "";
      this.Config = "";
      this.Name = Name;
      this.ID = Id;
      this.BuyTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
      switch (ItemType)
      {
        case ItemType.CONSUMABLE:
          this.Quantity = Quantity;
          break;
        case ItemType.PERMANENT:
          this.DurabilityPoints = DurabilityPoints;
          this.TotalDurabilityPoints = DurabilityPoints;
          break;
        case ItemType.TIME:
          this.ExpirationTime = this.BuyTime + (long) (int) TimeSpan.FromHours((double) Hours).TotalSeconds;
          break;
      }
    }

    public XElement Serialize(bool IncludeType = false, int seed = 0)
    {
      XElement xelement = new XElement((XName) "item");
      if (IncludeType)
        xelement.Add((object) new XAttribute((XName) "type", (object) (int) this.ItemType));
      if (seed == 0)
        xelement.Add((object) new XAttribute((XName) "id", (object) this.ID));
      else
        xelement.Add((object) new XAttribute((XName) "id", (object) seed));
      xelement.Add((object) new XAttribute((XName) "name", (object) this.Name));
      xelement.Add((object) new XAttribute((XName) "attached_to", (object) this.AttachedTo));
      xelement.Add((object) new XAttribute((XName) "config", (object) this.Config));
      xelement.Add((object) new XAttribute((XName) "slot", (object) this.Slot));
      xelement.Add((object) new XAttribute((XName) "equipped", (object) this.Equipped));
      xelement.Add((object) new XAttribute((XName) "default", (object) (this.ItemType == ItemType.DEFAULT ? 1 : 0)));
      xelement.Add((object) new XAttribute((XName) "permanent", (object) (this.ItemType == ItemType.PERMANENT ? 1 : 0)));
      xelement.Add((object) new XAttribute((XName) "expired_confirmed", (object) (this.ExpiredConfirmed ? 1 : 0)));
      xelement.Add((object) new XAttribute((XName) "buy_time_utc", (object) this.BuyTime));
      if ((this.Name.Contains("fbs") || this.Name.Contains("skin")) && this.ItemType != ItemType.TIME)
        this.ItemType = ItemType.NO_REPAIR;
      else if (this.ItemType == ItemType.CONSUMABLE)
        xelement.Add((object) new XAttribute((XName) "quantity", (object) this.Quantity));
      else if (this.ItemType == ItemType.DEFAULT)
        xelement.Add((object) new XAttribute((XName) "seconds_left", (object) 0));
      else if (this.ItemType == ItemType.TIME)
      {
        xelement.Add((object) new XAttribute((XName) "expiration_time_utc", (object) this.ExpirationTime));
        xelement.Add((object) new XAttribute((XName) "seconds_left", (object) this.SecondsLeft));
      }
      else if (this.ItemType == ItemType.PERMANENT)
      {
        xelement.Add((object) new XAttribute((XName) "total_durability_points", (object) this.TotalDurabilityPoints));
        xelement.Add((object) new XAttribute((XName) "durability_points", (object) this.DurabilityPoints));
      }
      return xelement;
    }

    public void Create(XmlElement Item, int id)
    {
      this.ItemType = (ItemType) int.Parse(Item.Attributes["type"].InnerText);
      this.ID = (long) id;
      this.Name = Item.Attributes["name"].InnerText;
      this.AttachedTo = Item.Attributes["attached_to"].InnerText;
      this.Config = Item.Attributes["config"].InnerText;
      this.Slot = int.Parse(Item.Attributes["slot"].InnerText);
      this.Equipped = byte.Parse(Item.Attributes["equipped"].InnerText);
      this.ExpiredConfirmed = Item.Attributes["expired_confirmed"].InnerText == "1";
      this.BuyTime = long.Parse(Item.Attributes["buy_time_utc"].InnerText);
      if (Item.Attributes["name"].InnerText.StartsWith("f_") || Item.Attributes["name"].InnerText.StartsWith("fbs_"))
        this.ItemType = ItemType.NO_REPAIR;
      else if (this.ItemType == ItemType.CONSUMABLE)
        this.Quantity = int.Parse(Item.Attributes["quantity"].InnerText);
      else if (this.ItemType == ItemType.TIME)
      {
        this.ExpirationTime = long.Parse(Item.Attributes["expiration_time_utc"].InnerText);
      }
      else
      {
        if (this.ItemType != ItemType.PERMANENT)
          return;
        this.TotalDurabilityPoints = 36000L;
        this.DurabilityPoints = 36000L;
      }
    }

    public static byte EquipperCalc(int Slot)
    {
      foreach (XElement descendant in Core.GameResources.Slots.Descendants((XName) "item"))
      {
        if (Slot.ToString() == descendant.Attribute((XName) "slot").Value)
          return Convert.ToByte(descendant.Attribute((XName) "equipped").Value);
      }
            Console.WriteLine("СЛОТ {0} НЕ НАЙДЕН", Slot);
      return 0;
    }
  }
}
