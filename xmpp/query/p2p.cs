// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.p2p
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  internal class p2p : Stanza
  {
    private string Channel;
    private Client user_n;

    public p2p(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      this.user_n = ArrayList.OnlineUsers.Find((Predicate<Client>) (Attribute => Attribute.JID == this.To));
      if (this.Type == "result")
      {
        XDocument xdocument = new XDocument();
        XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
        xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
        xelement1.Add((object) new XAttribute((XName) "from", (object) this.To));
        xelement1.Add((object) new XAttribute((XName) "to", (object) User.JID));
        xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
        XElement xelement2 = new XElement(Stanza.NameSpace + "query");
        XElement xelement3 = new XElement((XName) "peer_player_info");
        xelement3.Add((object) new XAttribute((XName) "online_id", (object) User.JID));
        xelement3.Add((object) new XAttribute((XName) "nickname", (object) User.Player.Nickname));
        try
        {
          xelement3.Add((object) new XAttribute((XName) "primary_weapon", this.Query.Attributes["primary_weapon"] != null ? (object) this.Query.Attributes["primary_weapon"].Value : (object) "ar02_shop"));
          xelement3.Add((object) new XAttribute((XName) "primary_weapon_skin", this.Query.Attributes["primary_weapon_skin"] != null ? (object) this.Query.Attributes["primary_weapon_skin"].Value : (object) ""));
          xelement3.Add((object) new XAttribute((XName) "banner_badge", (object) User.Player.BannerBadge));
          xelement3.Add((object) new XAttribute((XName) "banner_mark", (object) User.Player.BannerMark));
          xelement3.Add((object) new XAttribute((XName) "banner_stripe", (object) User.Player.BannerStripe));
          xelement3.Add((object) new XAttribute((XName) "experience", (object) this.Query.Attributes["experience"].Value));
          xelement3.Add((object) new XAttribute((XName) "pvp_rating_points", (object) this.Query.Attributes["pvp_rating_points"].Value));
          xelement3.Add((object) new XAttribute((XName) "items_unlocked", (object) this.Query.Attributes["items_unlocked"].Value));
          xelement3.Add((object) new XAttribute((XName) "challenges_completed", (object) this.Query.Attributes["challenges_completed"].Value));
          xelement3.Add((object) new XAttribute((XName) "missions_completed", (object) this.Query.Attributes["missions_completed"].Value));
          xelement3.Add((object) new XAttribute((XName) "pvp_wins", (object) this.Query.Attributes["pvp_wins"].Value));
          xelement3.Add((object) new XAttribute((XName) "pvp_loses", (object) this.Query.Attributes["pvp_loses"].Value));
          xelement3.Add((object) new XAttribute((XName) "pvp_kills", (object) this.Query.Attributes["pvp_kills"].Value));
          xelement3.Add((object) new XAttribute((XName) "pvp_deaths", (object) this.Query.Attributes["pvp_deaths"].Value));
          xelement3.Add((object) new XAttribute((XName) "playtime_seconds", (object) this.Query.Attributes["playtime_seconds"].Value));
          xelement3.Add((object) new XAttribute((XName) "leavings_percentage", (object) this.Query.Attributes["leavings_percentage"].Value));
          xelement3.Add((object) new XAttribute((XName) "coop_climbs_performed", (object) this.Query.Attributes["coop_climbs_performed"].Value));
          xelement3.Add((object) new XAttribute((XName) "coop_assists_performed", (object) this.Query.Attributes["coop_assists_performed"].Value));
          xelement3.Add((object) new XAttribute((XName) "favorite_pvp_class", (object) this.Query.Attributes["favorite_pvp_class"].Value));
          xelement3.Add((object) new XAttribute((XName) "favorite_pve_class", (object) this.Query.Attributes["favorite_pve_class"].Value));
          if (User.Player.Clan.ID != 0L)
          {
            xelement3.Add((object) new XAttribute((XName) "clan_name", (object) this.Query.Attributes["clan_name"].Value));
            xelement3.Add((object) new XAttribute((XName) "clan_role", (object) this.Query.Attributes["clan_role"].Value));
            xelement3.Add((object) new XAttribute((XName) "clan_position", (object) this.Query.Attributes["clan_position"].Value));
            xelement3.Add((object) new XAttribute((XName) "clan_points", (object) this.Query.Attributes["clan_points"].Value));
            xelement3.Add((object) new XAttribute((XName) "clan_member_since", (object) this.Query.Attributes["clan_member_since"].Value));
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
        }
        xelement2.Add((object) xelement3);
        xelement1.Add((object) xelement2);
        xdocument.Add((object) xelement1);
        this.user_n.Send(xdocument.ToString());
      }
      else
        this.user_n.Send(this.Packet.InnerXml);
    }
  }
}
