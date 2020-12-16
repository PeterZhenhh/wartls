// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.ResyncProfile
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class ResyncProfile : Stanza
  {
    private string Channel;

    public ResyncProfile(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      this.Process();
    }

    public ResyncProfile(Client User)
      : base(User, (XmlDocument) null)
    {
      this.Process();
    }

    public override void Process()
    {
      if (this.Type == "result")
        return;
      XDocument xDocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", this.Type == "get" ? (object) "result" : (object) "get"));
      xelement1.Add((object) new XAttribute((XName) "from", this.Packet != null ? (object) this.To : (object) "k01.warface"));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", this.Type == "get" ? (object) this.Id : (object) ("uid" + this.User.Player.Random.Next(9999, int.MaxValue).ToString("x8"))));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) "resync_profile");
      foreach (Item obj in this.User.Player.Items)
        xelement3.Add((object) obj.Serialize(false, 0));
      XElement xelement4 = new XElement((XName) "money");
      xelement4.Add((object) new XAttribute((XName) "cry_money", (object) this.User.Player.CryMoney));
      xelement4.Add((object) new XAttribute((XName) "crown_money", (object) this.User.Player.CrownMoney));
      xelement4.Add((object) new XAttribute((XName) "game_money", (object) this.User.Player.GameMoney));
      XElement xelement5 = new XElement((XName) "character");
      xelement5.Add((object) new XAttribute((XName) "nick", (object) this.User.Player.Nickname));
      xelement5.Add((object) new XAttribute((XName) "gender", (object) this.User.Player.Gender));
      xelement5.Add((object) new XAttribute((XName) "height", (object) this.User.Player.Height));
      xelement5.Add((object) new XAttribute((XName) "fatness", (object) this.User.Player.Fatness));
      xelement5.Add((object) new XAttribute((XName) "head", (object) this.User.Player.Head));
      xelement5.Add((object) new XAttribute((XName) "current_class", (object) this.User.Player.CurrentClass));
      xelement5.Add((object) new XAttribute((XName) "experience", (object) this.User.Player.Experience));
      xelement5.Add((object) new XAttribute((XName) "pvp_rating", (object) "0"));
      xelement5.Add((object) new XAttribute((XName) "pvp_rating_points", (object) "0"));
      xelement5.Add((object) new XAttribute((XName) "banner_badge", (object) this.User.Player.BannerBadge));
      xelement5.Add((object) new XAttribute((XName) "banner_mark", (object) this.User.Player.BannerMark));
      xelement5.Add((object) new XAttribute((XName) "banner_stripe", (object) this.User.Player.BannerStripe));
      xelement5.Add((object) new XAttribute((XName) "game_money", (object) this.User.Player.GameMoney));
      xelement5.Add((object) new XAttribute((XName) "cry_money", (object) this.User.Player.CryMoney));
      xelement5.Add((object) new XAttribute((XName) "crown_money", (object) this.User.Player.CrownMoney));
      XElement xelement6 = new XElement((XName) "profile_progression_state");
      xelement6.Add((object) new XAttribute((XName) "profile_id", (object) this.User.Player.UserID));
      xelement6.Add((object) new XAttribute((XName) "mission_unlocked", (object) this.User.Player.UnlockedMissions));
      xelement6.Add((object) new XAttribute((XName) "tutorial_unlocked", (object) this.User.Player.TutorialSuggest));
      xelement6.Add((object) new XAttribute((XName) "tutorial_passed", (object) this.User.Player.TutorialPassed));
      xelement6.Add((object) new XAttribute((XName) "class_unlocked", (object) this.User.Player.UnlockedClasses));
      XElement xelement7 = new XElement((XName) "profile_progression_state");
      xelement7.Add((object) xelement6);
      xelement3.Add((object) xelement4);
      xelement3.Add((object) xelement5);
      xelement3.Add((object) xelement7);
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xDocument.Add((object) xelement1);
      this.Compress(ref xDocument);
      this.User.Send(xDocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
