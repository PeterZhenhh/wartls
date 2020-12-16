// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.InvitationSend
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class InvitationSend : Stanza
  {
    private string Channel;
    private InvitationTicket Ticket;

    public InvitationSend(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      Client client = ArrayList.OnlineUsers.Find((Predicate<Client>) (Attribute => Attribute.Player.Nickname == this.Query.Attributes["nickname"].InnerText));
      InvitationTicket Ticket = new InvitationTicket(User, client)
      {
        GroupId = this.Query.Attributes["group_id"].InnerText
      };
      if (client != null)
      {
        this.Ticket = Ticket;
        Ticket.IsFollow = this.Query.Attributes["is_follow"].InnerText == "1";
        if ((int) User.Channel.MinRank > (int) client.Player.Rank || (int) User.Channel.MaxRank < (int) client.Player.Rank)
          Ticket.Result = (byte) 13;
        if (Ticket.Result == byte.MaxValue)
        {
          client.InvitationTicket.Add(Ticket);
          User.InvitationTicket.Add(Ticket);
          InvitationRequest invitationRequest = new InvitationRequest(client, Ticket);
        }
      }
      else
        Ticket.Result = (byte) 0;
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
      XElement xelement3 = new XElement((XName) "invitation_send");
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      if (this.Ticket.Result != byte.MaxValue)
      {
        XElement xelement4 = new XElement((XNamespace) "urn:ietf:params:xml:ns:xmpp-stanzas" + "error");
        xelement4.Add((object) new XAttribute((XName) "type", (object) "continue"));
        xelement4.Add((object) new XAttribute((XName) "code", (object) 8));
        xelement4.Add((object) new XAttribute((XName) "custom_code", (object) this.Ticket.Result));
        xelement1.Add((object) xelement4);
      }
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
    }

    private enum Results
    {
      Rejected = 1,
      AutoRejected = 2,
      MissionLocked = 12, // 0x0000000C
      RankRestricted = 13, // 0x0000000D
      FullRoom = 14, // 0x0000000E
      Banned = 15, // 0x0000000F
      BuildType = 16, // 0x00000010
      NotInClan = 18, // 0x00000012
      Participate = 19, // 0x00000013
      AllClassLocked = 20, // 0x00000014
      VersionMismatch = 21, // 0x00000015
      NoAccessTokens = 22, // 0x00000016
    }
  }
}
