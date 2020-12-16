// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.InvitationResult
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class InvitationResult : Stanza
  {
    private InvitationTicket Ticket;

    public InvitationResult(Client User, XmlDocument Packet = null)
      : base(User, Packet)
    {
    }

    public InvitationResult(Client User, InvitationTicket Ticket)
      : base(User, (XmlDocument) null)
    {
      this.Ticket = Ticket;
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
      XElement xelement3 = new XElement((XName) "invitation_result");
      xelement3.Add((object) new XAttribute((XName) "result", (object) this.Ticket.Result));
      xelement3.Add((object) new XAttribute((XName) "user", (object) this.Ticket.Receiver.Player.Nickname));
      xelement3.Add((object) new XAttribute((XName) "is_follow", (object) (this.Ticket.IsFollow ? 1 : 0)));
      xelement3.Add((object) new XAttribute((XName) "user_id", (object) this.Ticket.Receiver.Player.TicketId));
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xDocument.Add((object) xelement1);
      this.Compress(ref xDocument);
      this.User.Send(xDocument.ToString(SaveOptions.DisableFormatting));
      this.Ticket.Receiver.InvitationTicket.Remove(this.Ticket);
      this.Ticket.Sender.InvitationTicket.Remove(this.Ticket);
    }
  }
}
