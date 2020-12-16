// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.SetBanner
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class SetBanner : Stanza
  {
    public SetBanner(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      if (!this.CheckAchievement(this.Query.Attributes["banner_mark"].InnerText) || !this.CheckAchievement(this.Query.Attributes["banner_stripe"].InnerText) || !this.CheckAchievement(this.Query.Attributes["banner_badge"].InnerText))
      {
        User.Player.BannerMark = "4294967295";
        User.Player.BannerStripe = "4294967295";
        User.Player.BannerBadge = "4294967295";
        User.Player.Save();
        if (User.Player.RoomPlayer.Room != null)
          User.Player.RoomPlayer.Room.Sync((Client) null);
        ResyncProfile resyncProfile = new ResyncProfile(User);
      }
      else
      {
        User.Player.BannerMark = this.Query.Attributes["banner_mark"].InnerText;
        User.Player.BannerStripe = this.Query.Attributes["banner_stripe"].InnerText;
        User.Player.BannerBadge = this.Query.Attributes["banner_badge"].InnerText;
        User.Player.Save();
        if (User.Player.RoomPlayer.Room != null)
          User.Player.RoomPlayer.Room.Sync((Client) null);
        this.Process();
      }
    }

    public bool CheckAchievement(string ID)
    {
      if (ID == "4294967295")
        return true;
      foreach (XmlNode childNode in this.User.Player.Achievements["achievements"].ChildNodes)
      {
        if (childNode.Attributes["achievement_id"].InnerText == ID)
          return true;
      }
      return false;
    }

    public override void Process()
    {
      XDocument xdocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) "k01.warface"));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query", (object) new XElement((XName) "set_banner"));
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
