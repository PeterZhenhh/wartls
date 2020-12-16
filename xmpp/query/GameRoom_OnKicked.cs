// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.GameRoom_OnKicked
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
  public class GameRoom_OnKicked : Stanza
  {
    private string Channel;
    private GameRoom Room;
    private Client Target;
    public GameRoom_OnKicked.Reason KickReason;

    public GameRoom_OnKicked(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
    }

    public GameRoom_OnKicked(Client User, GameRoom_OnKicked.Reason Reason = GameRoom_OnKicked.Reason.KickedByUser)
      : base(User, (XmlDocument) null)
    {
      this.Room = User.Player.RoomPlayer.Room;
      this.KickReason = Reason;
      this.Room.KickedUsers.Add(User.Player.UserID);
      this.Room.Players.Users.RemoveAll((Predicate<Client>) (a => a == User));
      User.Player.RoomPlayer.Room = (GameRoom) null;
      if (this.Room.Core.RoomType == (byte) 4)
        this.Room.ClanBalanceProcess();
      else if (this.Room.CustomParams.AutoTeamBalance)
        this.Room.AutoBalanceProcess();
      else
        this.Room.Sync((Client) null);
      this.Process();
    }

    public override void Process()
    {
      XDocument xdocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "get"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) "k01.warface"));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.User.Player.Random.Next(1, int.MaxValue)));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) "gameroom_on_kicked");
      xelement3.Add((object) new XAttribute((XName) "reason", (object) (byte) this.KickReason));
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
    }

    public enum Reason
    {
      KickedByUser = 1,
      NonActivity = 2,
      KickedByVoting = 3,
      KickedByRank = 6,
      KickedClan = 7,
      SystemSecurityViolation = 8,
      VersionMismatch = 9,
      NoAccessPoints = 10, // 0x0000000A
    }
  }
}
