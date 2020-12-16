// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.InvitationRequest
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using WARTLS.CLASSES.GAMEROOM;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class InvitationRequest : Stanza
  {
    private GameRoom Room;
    private InvitationTicket Ticket;

    public InvitationRequest(Client User, XmlDocument Packet = null)
      : base(User, Packet)
    {
    }

    public InvitationRequest(Client User, InvitationTicket Ticket)
      : base(User, (XmlDocument) null)
    {
      if (this.Type == "result")
        return;
      this.Ticket = Ticket;
      this.Room = Ticket.Sender.Player.RoomPlayer.Room;
      this.Process();
    }

    public override void Process()
    {
      XDocument xDocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "get"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) ("masterserver@warface/" + this.User.Channel.Resource)));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.User.Player.Random.Next(1, int.MaxValue)));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) "invitation_request");
      if (this.Room != null)
      {
        XElement xelement4 = new XElement((XName) "initiator_info");
        xelement4.Add((object) new XAttribute((XName) "online_id", (object) this.Ticket.Sender.JID));
        xelement4.Add((object) new XAttribute((XName) "profile_id", (object) this.Ticket.Sender.Player.UserID));
        xelement4.Add((object) new XAttribute((XName) "is_online", (object) "1"));
        xelement4.Add((object) new XAttribute((XName) "name", (object) this.Ticket.Sender.Player.Nickname));
        xelement4.Add((object) new XAttribute((XName) "clan_name", this.Ticket.Sender.Player.Clan.ID != 0L ? (object) this.Ticket.Sender.Player.Clan.Name : (object) ""));
        xelement4.Add((object) new XAttribute((XName) "experience", (object) this.Ticket.Sender.Player.Experience));
        xelement4.Add((object) new XAttribute((XName) "badge", (object) this.Ticket.Sender.Player.BannerBadge));
        xelement4.Add((object) new XAttribute((XName) "mark", (object) this.Ticket.Sender.Player.BannerMark));
        xelement4.Add((object) new XAttribute((XName) "stripe", (object) this.Ticket.Sender.Player.BannerStripe));
        xelement3.Add((object) xelement4);
        xelement3.Add((object) new XAttribute((XName) "from", (object) this.Ticket.Sender.Player.Nickname));
        xelement3.Add((object) new XAttribute((XName) "ticket", (object) this.Ticket.ID));
        xelement3.Add((object) new XAttribute((XName) "room_id", (object) this.Room.Core.RoomId));
        xelement3.Add((object) new XAttribute((XName) "ms_resource", (object) this.Ticket.Sender.Channel.Resource));
        xelement3.Add((object) new XAttribute((XName) "is_follow", (object) (this.Ticket.IsFollow ? 1 : 0)));
        xelement3.Add((object) this.Room.Serialize(false));
      }
      else
      {
        XElement xelement4 = new XElement((XNamespace) "urn:ietf:params:xml:ns:xmpp-stanzas" + "error");
        xelement4.Add((object) new XAttribute((XName) "type", (object) "cancel"));
        xelement4.Add((object) new XAttribute((XName) "code", (object) 8));
        xelement4.Add((object) new XAttribute((XName) "custom_code", (object) 1));
        xelement1.Add((object) xelement4);
      }
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xDocument.Add((object) xelement1);
      this.Compress(ref xDocument);
      this.User.Send(xDocument.ToString());
    }
  }
}
