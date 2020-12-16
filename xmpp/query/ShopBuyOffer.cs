// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.ShopBuyOffer
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class ShopBuyOffer : Stanza
  {
    private List<XElement> profileItemElements = new List<XElement>();
    private List<Item> PurchasedItems = new List<Item>();
    public static int BuyedTotally;
    private Item Buyed;
    private int OfferId;
    private int ErrId;

    public ShopBuyOffer(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      List<int> intList = new List<int>();
      if (this.Query.Name == "shop_buy_multiple_offer")
      {
        foreach (XmlElement childNode in this.Query.ChildNodes)
        {
          this.OfferId = int.Parse(childNode.Attributes["id"].InnerText);
          intList.Add(int.Parse(childNode.Attributes["id"].InnerText));
        }
      }
      else
      {
        this.OfferId = int.Parse(this.Query.Attributes["offer_id"].InnerText);
        intList.Add(this.OfferId);
      }
      foreach (int OfferId in intList)
      {
        foreach (XmlNode childNode1 in Core.GameResources.ShopOffers["items"].ChildNodes)
        {
          XmlNode Offer = childNode1;
          if (int.Parse(Offer.Attributes["id"].InnerText) == OfferId)
          {
            int num1 = int.Parse(Offer.Attributes["game_price"].InnerText);
            int num2 = int.Parse(Offer.Attributes["cry_price"].InnerText);
            int num3 = int.Parse(Offer.Attributes["crown_price"].InnerText);
            if (num3 > User.Player.CrownMoney || num1 > User.Player.GameMoney || num2 > User.Player.CryMoney)
            {
              this.ErrId = 1;
              this.Process();
              return;
            }
            User.Player.CrownMoney -= num3;
            User.Player.GameMoney -= num1;
            User.Player.CryMoney -= num2;
            if (Offer.Attributes["name"].InnerText.Contains("game_money_item_01"))
              User.Player.GameMoney += int.Parse(Offer.Attributes["quantity"].InnerText);
            else if (Offer.Attributes["name"].InnerText.Contains("box"))
              this.profileItemElements.AddRange((IEnumerable<XElement>) User.Player.GeneratePrizes(Offer.Attributes["name"].InnerText, out this.ErrId, OfferId));
            else if (Offer.Attributes["name"].InnerText.Contains("bundle_item"))
            {
              XmlDocument xmlDocument = new XmlDocument();
              xmlDocument.Load("Gamefiles/ShopItems/" + Offer.Attributes["name"].InnerText + ".xml");
              foreach (XmlNode childNode2 in xmlDocument["shop_item"]["bundle"].ChildNodes)
              {
                if (childNode2.Attributes["expiration"] != null)
                {
                  if (childNode2.Attributes["expiration"].InnerText.Contains("d"))
                  {
                    int totalHours = (int) TimeSpan.FromDays((double) int.Parse(new Regex("[0-9]*").Match(childNode2.Attributes["expiration"].InnerText).Value)).TotalHours;
                    this.PurchasedItems.Add(new Item(WARTLS.CLASSES.ItemType.TIME, User.Player.ItemSeed, childNode2.Attributes["name"].InnerText, totalHours, 0, 36000L)
                    {
                      BundleTime = childNode2.Attributes["expiration"].InnerText
                    });
                  }
                  else if (childNode2.Attributes["expiration"].InnerText.Contains("h"))
                  {
                    int Hours = int.Parse(new Regex("[0-9]*").Match(childNode2.Attributes["expiration"].InnerText).Value);
                    this.PurchasedItems.Add(new Item(WARTLS.CLASSES.ItemType.TIME, User.Player.ItemSeed, childNode2.Attributes["name"].InnerText, Hours, 0, 36000L)
                    {
                      BundleTime = childNode2.Attributes["expiration"].InnerText
                    });
                  }
                }
                else if (childNode2.Attributes["regular"] != null)
                  this.PurchasedItems.Add(new Item(WARTLS.CLASSES.ItemType.NO_REPAIR, User.Player.ItemSeed, childNode2.Attributes["name"].InnerText, 0, 0, 36000L));
                else if (childNode2.Attributes["amount"] != null)
                  this.PurchasedItems.Add(new Item(WARTLS.CLASSES.ItemType.CONSUMABLE, User.Player.ItemSeed, childNode2.Attributes["name"].InnerText, 0, int.Parse(childNode2.Attributes["amount"].InnerText), 36000L)
                  {
                    BundleQuantity = childNode2.Attributes["amount"].InnerText
                  });
                else
                  this.PurchasedItems.Add(new Item(WARTLS.CLASSES.ItemType.PERMANENT, User.Player.ItemSeed, childNode2.Attributes["name"].InnerText, 0, 0, 36000L));
              }
            }
            else if (Offer.Attributes["expirationTime"].InnerText != "0")
            {
              int totalHours = int.Parse(new Regex("[0-9]*").Match(Offer.Attributes["expirationTime"].InnerText).Value);
              if (Offer.Attributes["expirationTime"].InnerText.Contains("d"))
              {
                totalHours = (int) TimeSpan.FromDays((double) totalHours).TotalHours;
                this.PurchasedItems.Add(new Item(WARTLS.CLASSES.ItemType.TIME, User.Player.ItemSeed, Offer.Attributes["name"].InnerText, totalHours, 0, 36000L));
              }
              if (Offer.Attributes["expirationTime"].InnerText.Contains("h"))
                this.PurchasedItems.Add(new Item(WARTLS.CLASSES.ItemType.TIME, User.Player.ItemSeed, Offer.Attributes["name"].InnerText, totalHours, 0, 36000L));
            }
            else if (Offer.Attributes["durabilityPoints"].InnerText != "0")
              this.PurchasedItems.Add(new Item(WARTLS.CLASSES.ItemType.PERMANENT, User.Player.ItemSeed, Offer.Attributes["name"].InnerText, 0, 0, long.Parse(Offer.Attributes["durabilityPoints"].InnerText)));
            else if (Offer.Attributes["quantity"].InnerText != "0")
              this.PurchasedItems.Add(new Item(WARTLS.CLASSES.ItemType.CONSUMABLE, User.Player.ItemSeed, Offer.Attributes["name"].InnerText, 0, int.Parse(Offer.Attributes["quantity"].InnerText), 0L));
            else if (User.Player.Items.Find((Predicate<Item>) (Attribute => Attribute.Name == Offer.Attributes["name"].InnerText && Attribute.ItemType == WARTLS.CLASSES.ItemType.NO_REPAIR)) == null)
              this.PurchasedItems.Add(new Item(WARTLS.CLASSES.ItemType.NO_REPAIR, User.Player.ItemSeed, Offer.Attributes["name"].InnerText, 0, 0, 0L));
            else
              this.ErrId = 4;
            if (this.profileItemElements.Count == 0)
            {
              foreach (Item purchasedItem in this.PurchasedItems)
              {
                Item obj = purchasedItem;
                XElement xelement = new XElement((XName) "profile_item");
                if (purchasedItem.Name != "game_money_item_01")
                {
                  obj = User.Player.AddItem(purchasedItem);
                  xelement.Add((object) new XAttribute((XName) "name", (object) obj.Name));
                  xelement.Add((object) new XAttribute((XName) "profile_item_id", (object) obj.ID));
                  xelement.Add((object) new XAttribute((XName) "offerId", (object) OfferId));
                  xelement.Add((object) new XAttribute((XName) "added_expiration", Offer.Attributes["name"].InnerText.Contains("bundle_item") || Offer.Attributes["name"].InnerText.Contains("random_box") ? (object) purchasedItem.BundleTime : (object) Offer.Attributes["expirationTime"].InnerText));
                  xelement.Add((object) new XAttribute((XName) "added_quantity", Offer.Attributes["name"].InnerText.Contains("bundle_item") || Offer.Attributes["name"].InnerText.Contains("random_box") ? (object) purchasedItem.BundleQuantity : (object) Offer.Attributes["quantity"].InnerText));
                  xelement.Add((object) new XAttribute((XName) "error_status", (object) this.ErrId));
                  xelement.Add((object) obj.Serialize(false, 0));
                  this.profileItemElements.Add(xelement);
                }
                this.Buyed = obj;
              }
            }
            if (this.ErrId > 0)
            {
              User.Player.CrownMoney += num3;
              User.Player.GameMoney += num1;
              User.Player.CryMoney += num2;
              break;
            }
            break;
          }
        }
      }
      User.Player.Save();
      this.Process();
    }

    public override void Process()
    {
      XDocument xdocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) this.To));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) this.Query.Name);
      xelement3.Add((object) new XAttribute((XName) "error_status", (object) this.ErrId));
      if (this.Query.Name != "extend_item")
      {
        xelement3.Add((object) new XAttribute((XName) "offer_id", (object) this.OfferId));
        XElement xelement4 = new XElement((XName) "purchased_item");
        foreach (XElement profileItemElement in this.profileItemElements)
          xelement4.Add((object) profileItemElement);
        XElement xelement5 = new XElement((XName) "money");
        xelement5.Add((object) new XAttribute((XName) "cry_money", (object) this.User.Player.CryMoney));
        xelement5.Add((object) new XAttribute((XName) "crown_money", (object) this.User.Player.CrownMoney));
        xelement5.Add((object) new XAttribute((XName) "game_money", (object) this.User.Player.GameMoney));
        xelement3.Add((object) xelement4);
        xelement3.Add((object) xelement5);
      }
      else
      {
        xelement3.Add((object) new XAttribute((XName) "durability", (object) this.Buyed.DurabilityPoints));
        xelement3.Add((object) new XAttribute((XName) "total_durability", (object) this.Buyed.TotalDurabilityPoints));
        xelement3.Add((object) new XAttribute((XName) "expiration_time_utc", (object) this.Buyed.ExpirationTime));
        xelement3.Add((object) new XAttribute((XName) "seconds_left", (object) this.Buyed.SecondsLeft));
        xelement3.Add((object) new XAttribute((XName) "cry_money", (object) this.User.Player.CryMoney));
        xelement3.Add((object) new XAttribute((XName) "game_money", (object) this.User.Player.GameMoney));
        xelement3.Add((object) new XAttribute((XName) "crown_money", (object) this.User.Player.CrownMoney));
      }
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString());
    }
  }
}
