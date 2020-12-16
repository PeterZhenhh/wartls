// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.ClanSetMemberRole
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using WARTLS.EXCEPTION;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class ClanSetMemberRole : Stanza
  {
    public ClanSetMemberRole(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      if (this.Type == "result")
        return;
      long TargetID = long.Parse(this.Query.Attributes["profile_id"].InnerText);
      Clan.CLAN_ROLE clanRole = (Clan.CLAN_ROLE) byte.Parse(this.Query.Attributes["role"].InnerText);
      if (!Enum.IsDefined(typeof (Clan.CLAN_ROLE), (object) clanRole) || User.Player.Clan == null || User.Player.ClanPlayer.ClanRole != Clan.CLAN_ROLE.LEADER)
        throw new StanzaException(User, Packet, 7);
      Client client = ArrayList.OnlineUsers.Find((Predicate<Client>) (Attribute => Attribute.Player.UserID == TargetID));
      client.Player.ClanPlayer.ClanRole = clanRole;
      switch (clanRole - 1)
      {
        case Clan.CLAN_ROLE.NOT_IN_CLAN:
          client.ShowMessage("@clans_you_are_promoted_to_master", false);
          break;
        case Clan.CLAN_ROLE.LEADER:
          client.ShowMessage("@clans_you_are_promoted_to_officer", false);
          break;
        case Clan.CLAN_ROLE.CO_LEADER:
          client.ShowMessage("@clans_you_are_demoted_to_regular", false);
          break;
      }
      if (clanRole == Clan.CLAN_ROLE.LEADER)
      {
        User.Player.ClanPlayer.ClanRole = Clan.CLAN_ROLE.CO_LEADER;
        User.Player.Clan.UpdatePlayer(User.Player);
      }
      User.Player.Clan.UpdatePlayer(client.Player);
      foreach (XElement xelement in User.Player.Clan.ClanMembers.Elements((XName) "clan_member_info").ToList<XElement>())
      {
        XElement clanMember = xelement;
        Client User1 = ArrayList.OnlineUsers.Find((Predicate<Client>) (x => x.Player.UserID == long.Parse(clanMember.Attribute((XName) "profile_id").Value)));
        if (User1 != null)
        {
          ClanInfo clanInfo = new ClanInfo(User1, (XmlDocument) null);
        }
      }
      this.Process();
    }

    public override void Process()
    {
      if (this.Type == "result")
        return;
      XDocument xDocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) this.To));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) "clan_set_member_role");
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xDocument.Add((object) xelement1);
      this.Compress(ref xDocument);
      this.User.Send(xDocument.ToString());
    }
  }
}
