// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.GameRoom_Get
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using WARTLS.CLASSES.GAMEROOM;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class GameRoom_Get : Stanza
  {
    public GameRoom_Get(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      this.Process();
    }

    public override void Process()
    {
      XDocument xDocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) this.To));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) "gameroom_get");
      foreach (GameRoom gameRoom in this.User.Channel.GameRoomList.ToArray())
      {
        try
        {
          if (gameRoom.Players.Users.Count == 0)
            this.User.Channel.GameRoomList.Remove(gameRoom);
          else
            xelement3.Add((object) gameRoom.Serialize(false));
        }
        catch
        {
          this.User.Channel.GameRoomList.Remove(gameRoom);
        }
      }
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xDocument.Add((object) xelement1);
      this.Compress(ref xDocument);
      this.User.Send(xDocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
