// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.LobbychatGetChannelId
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class LobbychatGetChannelId : Stanza
  {
    private string Channel;

    public LobbychatGetChannelId(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      this.Channel = this.Query.Attributes["channel"].InnerText;
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
      string str = (string) null;
      try
      {
        switch (this.Channel)
        {
          case "0":
            str = "global." + this.User.Channel.Resource;
            break;
          case "1":
            str = string.Format("room.{0}", (object) this.User.Player.RoomPlayer.Room.Core.RoomId);
            break;
          case "2":
            str = string.Format("team.room.{0}", (object) this.User.Player.RoomPlayer.Room.Core.RoomId);
            break;
          case "3":
            str = string.Format("clan.{0}", (object) this.User.Player.Clan.ID);
            break;
        }
      }
      catch
      {
        XElement xelement3 = new XElement((XName) "error");
        xelement3.Add((object) new XAttribute((XName) "type", (object) "continue"));
        xelement3.Add((object) new XAttribute((XName) "code", (object) 8));
        xelement3.Add((object) new XElement((XNamespace) "urn:ietf:params:xml:ns:xmpp-stanzas" + "public-server-error"), (object) new XElement((XNamespace) "urn:ietf:params:xml:ns:xmpp-stanzas" + "text", (object) "Query processing error"));
      }
      XElement xelement4 = new XElement((XName) "lobbychat_getchannelid");
      xelement4.Add((object) new XAttribute((XName) "channel", (object) this.Channel));
      if (str != null)
        xelement4.Add((object) new XAttribute((XName) "channel_id", (object) str));
      xelement4.Add((object) new XAttribute((XName) "service_id", (object) "conference.warface"));
      xelement2.Add((object) xelement4);
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
