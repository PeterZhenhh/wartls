// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.GameRoom_Leave
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
  public class GameRoom_Leave : Stanza
  {
    private GameRoom Room;

    public GameRoom_Leave(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      this.Room = User.Player.RoomPlayer.Room;
      if (this.Room != null)
      {
        this.Room.Players.Users.RemoveAll((Predicate<Client>) (a => a == User));
        User.Player.RoomPlayer.Room = (GameRoom) null;
        if (this.Room.RoomMaster.UserId == User.Player.UserID)
        {
          Client[] array = this.Room.Players.Users.ToList<Client>().OrderBy<Client, int>((Func<Client, int>) (Attribute => Attribute.Player.Experience)).ToArray<Client>();
          if (array.Length != 0)
          {
            ++this.Room.RoomMaster.Revision;
            this.Room.RoomMaster.UserId = array[0].Player.UserID;
            array[0].Player.RoomPlayer.Status = Status.NOT_READY;
          }
        }
        if (this.Room.Core.RoomType == (byte) 4)
          this.Room.ClanBalanceProcess();
        else if (this.Room.CustomParams.AutoTeamBalance)
          this.Room.AutoBalanceProcess();
        else
          this.Room.Sync(User);
      }
      if (this.Packet == null)
        return;
      this.Process();
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
      XElement xelement3 = new XElement((XName) "gameroom_leave");
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
