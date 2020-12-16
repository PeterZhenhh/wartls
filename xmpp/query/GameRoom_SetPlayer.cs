// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.GameRoom_SetPlayer
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
  public class GameRoom_SetPlayer : Stanza
  {
    private string Channel;

    public GameRoom_SetPlayer(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      GameRoom room = User.Player.RoomPlayer.Room;
      if (room == null)
        return;
      byte num1 = byte.Parse(this.Query.Attributes["team_id"].InnerText);
      if (User.Channel.ChannelType == "pve")
        User.Player.RoomPlayer.TeamId = Teams.WARFACE;
      byte num2 = (byte) room.Players.Users.Count<Client>((Func<Client, bool>) (Attribute => Attribute.Player.RoomPlayer.TeamId == Teams.WARFACE));
      byte num3 = (byte) room.Players.Users.Count<Client>((Func<Client, bool>) (Attribute => Attribute.Player.RoomPlayer.TeamId == Teams.BLACKWOOD));
      if (!room.CustomParams.AutoTeamBalance)
      {
        if ((int) num2 > (int) room.MaxPlayerAtTeam)
          User.Player.RoomPlayer.TeamId = Teams.BLACKWOOD;
        User.Player.RoomPlayer.TeamId = (int) num3 <= (int) room.MaxPlayerAtTeam ? (Teams) num1 : Teams.WARFACE;
      }
      Status status = (Status) byte.Parse(this.Query.Attributes["status"].InnerText);
      if (User.Player.RoomPlayer.Status != status && room.Dedicated != null && room.Dedicated.Status < 4)
        new MissionUpdate(room.Dedicated, (XmlDocument) null).Process();
      User.Player.RoomPlayer.Status = status;
      User.Player.CurrentClass = byte.Parse(this.Query.Attributes["class_id"].InnerText);
      if (!room.CustomParams.SoldierEnabled || !room.CustomParams.EngineerEnabled || (!room.CustomParams.SniperEnabled || !room.CustomParams.MedicEnabled))
      {
        foreach (Client client in room.Players.Users.ToArray())
        {
          bool flag = false;
          if (client.Player.CurrentClass == (byte) 0 && !room.CustomParams.SoldierEnabled)
            flag = true;
          if (client.Player.CurrentClass == (byte) 4 && !room.CustomParams.EngineerEnabled)
            flag = true;
          if (client.Player.CurrentClass == (byte) 3 && !room.CustomParams.MedicEnabled)
            flag = true;
          if (client.Player.CurrentClass == (byte) 2 && !room.CustomParams.SniperEnabled)
            flag = true;
          if (flag)
          {
            if (room.CustomParams.SoldierEnabled)
              client.Player.CurrentClass = (byte) 0;
            if (room.CustomParams.EngineerEnabled)
              client.Player.CurrentClass = (byte) 4;
            if (room.CustomParams.MedicEnabled)
              client.Player.CurrentClass = (byte) 3;
            if (room.CustomParams.SniperEnabled)
              client.Player.CurrentClass = (byte) 2;
          }
        }
      }
      this.Process();
      room.SyncAsync(User);
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
      XElement xelement3 = new XElement((XName) "gameroom_setplayer");
      xelement3.Add((object) this.User.Player.RoomPlayer.Room.Serialize(false));
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xDocument.Add((object) xelement1);
      this.Compress(ref xDocument);
      this.User.Send(xDocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
