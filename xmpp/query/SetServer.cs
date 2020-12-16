// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.SetServer
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class SetServer : Stanza
  {
    private string Channel;

    public SetServer(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      User.DedicatedPort = ushort.Parse(this.Query.Attributes["port"].InnerText);
      User.Status = (int) byte.Parse(this.Query.Attributes["status"].InnerText);
      if (User.Status == 1)
        new MissionUpdate(User, (XmlDocument) null).Process();
      if (User.Status == 8 || User.Player.RoomPlayer.Room != null && User.Player.RoomPlayer.Room.Players.Users.Count == 0)
        new MissionUnload(User, (XmlDocument) null).Process();
      this.Process();
    }

    public override void Process()
    {
      XDocument xdocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) "masterserver@warface/onyx"));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) "setserver");
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString());
    }
  }
}
