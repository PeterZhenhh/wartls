// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.GameRoom_PromoteToHost
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using WARTLS.CLASSES.GAMEROOM;
using System;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class GameRoom_PromoteToHost : Stanza
  {
    private GameRoom Room;

    public GameRoom_PromoteToHost(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      this.Room = User.Player.RoomPlayer.Room;
      this.Room.RoomMaster.UserId = long.Parse(this.Query.Attributes["new_host_profile_id"].InnerText);
      User.Player.RoomPlayer.Status = Status.NOT_READY;
      this.Room.Sync(User);
    }

    public override void Process()
    {
      XDocument xdocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) "k01.warface"));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) "gameroom_promote_to_host");
      xelement3.Add((object) this.User.Player.RoomPlayer.Room.Serialize(false));
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
