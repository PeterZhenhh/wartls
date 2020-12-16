// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.MissionUnload
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using WARTLS.CLASSES.GAMEROOM;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class MissionUnload : Stanza
  {
    private string Channel;
    private GameRoom Room;

    public MissionUnload(Client User, XmlDocument Packet = null)
      : base(User, Packet)
    {
      if (this.Type == "result")
        return;
      this.Room = User.Player.RoomPlayer.Room;
      this.Room.Session.Status = (byte) 0;
      this.Room.Dedicated = (Client) null;
      User.Player.RoomPlayer.Room = (GameRoom) null;
      User.Dispose();
      User.Socket.Dispose();
      this.Room.Sync((Client) null);
      if (Packet == null)
        return;
      this.Process();
    }

    public override void Process()
    {
      if (this.Type == "result")
        return;
      XDocument xdocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "get"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) "masterserver@warface/onyx"));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.User.Player.Random.Next(99999, int.MaxValue)));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) "mission_unload");
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString());
    }
  }
}
