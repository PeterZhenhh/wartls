// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.GetProfile
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class GetProfile : Stanza
  {
    private string Channel;
    private Player player;
    private XmlDocument packet;

    public GetProfile(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      this.packet = Packet;
      this.player = User.Player;
      this.Process();
    }

    private new void Process()
    {
      foreach (Client user in this.User.Player.RoomPlayer.Room.Players.Users)
      {
        if (user.Player.UserID == long.Parse(this.packet["iq"]["query"]["getprofile"].Attributes["id"].InnerText))
        {
          XDocument xDocument = new XDocument();
          XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
          xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
          xelement1.Add((object) new XAttribute((XName) "from", (object) "k01.warface"));
          xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
          xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
          XElement xelement2 = new XElement(Stanza.NameSpace + "query");
          XElement xelement3 = new XElement((XName) "getprofile", (object) new XAttribute((XName) "id", (object) user.Player.UserID));
          XElement xelement4 = new XElement((XName) "profile");
          xelement4.Add((object) new XAttribute((XName) "nickname", (object) user.Player.Nickname));
          xelement4.Add((object) new XAttribute((XName) "user_id", (object) user.Player.UserID));
          xelement4.Add((object) new XAttribute((XName) "gender", (object) user.Player.Gender));
          xelement4.Add((object) new XAttribute((XName) "height", (object) user.Player.Height));
          xelement4.Add((object) new XAttribute((XName) "fatness", (object) user.Player.Fatness));
          xelement4.Add((object) new XAttribute((XName) "head", (object) user.Player.Head));
          xelement4.Add((object) new XAttribute((XName) "current_class", (object) user.Player.CurrentClass));
          xelement4.Add((object) new XAttribute((XName) "experience", (object) user.Player.Experience));
          xelement4.Add((object) new XAttribute((XName) "preset", (object) "DefaultPreset"));
          xelement4.Add((object) new XAttribute((XName) "clanName", (object) user.Player.Nickname));
          xelement4.Add((object) new XAttribute((XName) "unlocked_classes", (object) user.Player.UnlockedClasses));
          xelement4.Add((object) new XAttribute((XName) "group_id", (object) user.Player.RoomPlayer.GroupId));
          XElement xelement5 = new XElement((XName) "skill");
          xelement5.Add((object) new XAttribute((XName) "type", (object) "PVE"));
          xelement5.Add((object) new XAttribute((XName) "value", (object) "MasterServer.GameLogic.SkillSystem.Skill"));
          XElement xelement6 = new XElement((XName) "boosts");
          xelement6.Add((object) new XAttribute((XName) "xp_boost", (object) "0"));
          xelement6.Add((object) new XAttribute((XName) "vp_boost", (object) "0"));
          xelement6.Add((object) new XAttribute((XName) "gm_boost", (object) "0"));
          xelement6.Add((object) new XAttribute((XName) "ic_boost", (object) "0"));
          xelement6.Add((object) new XAttribute((XName) "is_vip", (object) "0"));
          XElement xelement7 = new XElement((XName) "items");
          if (this.User.Player.RoomPlayer.Room.Core.Name.Contains("МОД(1)"))
          {
            foreach (Item obj in user.Player.Items.ToArray())
            {
              if ((obj.Equipped != (byte) 0 || obj.ItemType == WARTLS.CLASSES.ItemType.DEFAULT) && (!obj.Name.StartsWith("pt") && !obj.Name.StartsWith("kn")) && (!obj.Name.Contains("explosive") && !obj.Name.StartsWith("smoke") && (!obj.Name.StartsWith("df") && !obj.Name.StartsWith("mk"))) && (!obj.Name.StartsWith("ak") && !obj.Name.StartsWith("ap") && !obj.Name.Contains("flashbang")))
              {
                ++this.User.Player.RoomPlayer.Room.Core.ItemSeed;
                xelement7.Add((object) obj.Serialize(true, this.User.Player.RoomPlayer.Room.Core.ItemSeed));
              }
            }
          }
          else
          {
            foreach (Item obj in user.Player.Items.ToArray())
            {
              ++this.User.Player.RoomPlayer.Room.Core.ItemSeed;
              if (obj.Equipped != (byte) 0 || obj.ItemType == WARTLS.CLASSES.ItemType.DEFAULT)
                xelement7.Add((object) obj.Serialize(true, this.User.Player.RoomPlayer.Room.Core.ItemSeed));
            }
          }
          xelement4.Add((object) xelement5);
          xelement4.Add((object) xelement6);
          xelement4.Add((object) xelement7);
          xelement3.Add((object) xelement4);
          xelement2.Add((object) xelement3);
          xelement1.Add((object) xelement2);
          xDocument.Add((object) xelement1);
          this.Compress(ref xDocument);
          this.User.Send(xDocument.ToString(SaveOptions.DisableFormatting));
        }
      }
    }
  }
}
