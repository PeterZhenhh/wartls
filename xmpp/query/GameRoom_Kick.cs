// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.GameRoom_Kick
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using WARTLS.CLASSES.GAMEROOM;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class GameRoom_Kick : Stanza
  {
    private string Channel;
    private GameRoom Room;
    private Client Target;

    public GameRoom_Kick(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
            Room = User.Player.RoomPlayer.Room;
            long TargetId = long.Parse(Query.Attributes["target_id"].InnerText);
            Target = Room.Players.Users.ToList().Find((Client Attribute) => Attribute.Player.UserID == TargetId);
            if (Room != null && Room.RoomMaster.UserId == User.Player.UserID)
            {
                new GameRoom_OnKicked(Target);
                Process();
            }
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
      XElement xelement3 = new XElement((XName) "gameroom_kick");
      xelement3.Add((object) this.Room.Serialize(false));
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xDocument.Add((object) xelement1);
      this.Compress(ref xDocument);
      this.User.Send(xDocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
