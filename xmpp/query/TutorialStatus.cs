// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.TutorialStatus
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class TutorialStatus : Stanza
  {
    private byte Event;
    private string ID;

    public TutorialStatus(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      this.Event = byte.Parse(this.Query.Attributes["event"].InnerText);
      this.ID = this.Query.Attributes["id"].InnerText;
      if (this.Event != (byte) 2)
        return;
      foreach (XmlDocument map in Core.GameResources.Maps)
      {
        if (map.FirstChild.Attributes["uid"].InnerText == this.ID)
        {
          switch (map.FirstChild.Attributes["name"].InnerText)
          {
            case "@name_tutorial_soldier":
            case "@name_tutorial_medic":
            case "@name_tutorial_engineer":
              string str = map.FirstChild.Attributes["name"].InnerText == "@name_tutorial_soldier" ? "tutorial_1_completed" : (map.FirstChild.Attributes["name"].InnerText == "@name_tutorial_medic" ? "tutorial_2_completed" : "tutorial_3_completed");
              if (map.FirstChild.Attributes["name"].InnerText == "@name_tutorial_soldier" && !User.Player.SoldierPassed)
                User.Player.SoldierPassed = true;
              else if (map.FirstChild.Attributes["name"].InnerText == "@name_tutorial_medic" && !User.Player.MedicPassed)
                User.Player.MedicPassed = true;
              else if (map.FirstChild.Attributes["name"].InnerText == "@name_tutorial_engineer" && !User.Player.EngineerPassed)
                User.Player.EngineerPassed = true;
              else
                continue;
              IEnumerator enumerator = Core.GameResources.Configs["items"]["special_reward_configuration"].ChildNodes.GetEnumerator();
              try
              {
                while (enumerator.MoveNext())
                {
                  XmlNode current = (XmlNode) enumerator.Current;
                  if (current.Attributes["name"].InnerText == str)
                  {
                    foreach (XmlNode childNode in current.ChildNodes)
                    {
                      if (childNode.Name == "money")
                        User.Player.AddMoneyNotification(childNode.Attributes["currency"].InnerText, int.Parse(childNode.Attributes["amount"].InnerText), "", false);
                      if (childNode.Name == "item")
                        User.Player.AddItemNotification(childNode.Attributes["expiration"] != null ? "Expiration" : (childNode.Attributes["amount"] != null ? "Consumable" : "Permanent"), childNode.Attributes["name"].InnerText, childNode.Attributes["expiration"] != null ? int.Parse(new Regex("[0-9]*").Match(childNode.Attributes["expiration"].InnerText).Value) * (!childNode.Attributes["expiration"].InnerText.Contains("d") ? 1 : 24) : (childNode.Attributes["amount"] != null ? int.Parse(childNode.Attributes["amount"].InnerText) : 0), "", false);
                      switch (childNode.Name)
                      {
                        case "money":
                          switch (childNode.Attributes["currency"].InnerText)
                          {
                            case "game_money":
                              User.Player.GameMoney += int.Parse(childNode.Attributes["amount"].InnerText);
                              continue;
                            case "cry_money":
                              User.Player.CryMoney += int.Parse(childNode.Attributes["amount"].InnerText);
                              continue;
                            case "crown_money":
                              User.Player.CrownMoney += int.Parse(childNode.Attributes["amount"].InnerText);
                              continue;
                            default:
                              continue;
                          }
                        case "item":
                          User.Player.AddItem(new Item(childNode.Attributes["expiration"] != null ? WARTLS.CLASSES.ItemType.TIME : (childNode.Attributes["amount"] != null ? WARTLS.CLASSES.ItemType.CONSUMABLE : WARTLS.CLASSES.ItemType.PERMANENT), User.Player.ItemSeed, childNode.Attributes["name"].InnerText, childNode.Attributes["expiration"] != null ? int.Parse(new Regex("[0-9]*").Match(childNode.Attributes["expiration"].InnerText).Value) * (!childNode.Attributes["expiration"].InnerText.Contains("d") ? 1 : 24) : 0, childNode.Attributes["amount"] != null ? int.Parse(childNode.Attributes["amount"].InnerText) : 0, 36000L));
                          continue;
                        default:
                          continue;
                      }
                    }
                  }
                }
                continue;
              }
              finally
              {
                if (enumerator is IDisposable disposable)
                  disposable.Dispose();
              }
            default:
              continue;
          }
        }
      }
      this.Process();
      new SyncNotification(User, (XmlDocument) null).Process();
      User.Player.Save();
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
      XElement xelement3 = new XElement((XName) "tutorial_status");
      XElement xelement4 = new XElement((XName) "profile_progression_update");
      xelement4.Add((object) new XAttribute((XName) "profile_id", (object) this.User.Player.UserID));
      xelement4.Add((object) new XAttribute((XName) "mission_unlocked", (object) this.User.Player.UnlockedMissions));
      xelement4.Add((object) new XAttribute((XName) "tutorial_unlocked", (object) this.User.Player.TutorialSuggest));
      xelement4.Add((object) new XAttribute((XName) "tutorial_passed", (object) this.User.Player.TutorialPassed));
      xelement4.Add((object) new XAttribute((XName) "class_unlocked", (object) this.User.Player.UnlockedClasses));
      xelement3.Add((object) xelement4);
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
