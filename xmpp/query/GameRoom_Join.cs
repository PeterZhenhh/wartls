// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.GameRoom_Join
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using WARTLS.CLASSES.GAMEROOM;
using WARTLS.EXCEPTION;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class GameRoom_Join : Stanza
  {
    private GameRoom Room;

    public GameRoom_Join(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      try
      {
        if (User.Player.RoomPlayer.Room != null)
        {
          GameRoom_Leave gameRoomLeave = new GameRoom_Leave(User, (XmlDocument) null);
        }
        this.Room = User.Channel.GameRoomList.Find((Predicate<GameRoom>) (Attribute => Attribute.Core.RoomId == long.Parse(this.Query.Attributes["room_id"].InnerText)));
        if (this.Room == null)
        {
          StanzaException stanzaException1 = new StanzaException(User, this.Packet, 10);
        }
        else if (this.Room.Core.Private )
        {
          StanzaException stanzaException2 = new StanzaException(User, this.Packet, 10);
        }
        else if (!this.Room.CustomParams.JoinInProcess && !User.Player.RoomPlayer.isInvited)
        {
          StanzaException stanzaException3 = new StanzaException(User, this.Packet, 10);
        }
        else if (this.Room.KickedUsers.Contains(User.Player.UserID))
        {
          StanzaException stanzaException4 = new StanzaException(User, this.Packet, 2);
        }
        else if (this.Room.Players.Users.Count >= (int) this.Room.CustomParams.MaxPlayers)
        {
          StanzaException stanzaException5 = new StanzaException(User, this.Packet, 4);
        }
        else if (User.Player.Clan == null && this.Room.Core.RoomType == (byte) 4)
        {
          StanzaException stanzaException6 = new StanzaException(User, this.Packet, 13);
        }
        else if (this.Room.ClanWar.ClanFirst != "" && this.Room.ClanWar.ClanFirst != User.Player.Clan.Name && (this.Room.ClanWar.ClanSecond != "" && this.Room.ClanWar.ClanSecond != User.Player.Clan.Name))
        {
          StanzaException stanzaException7 = new StanzaException(User, this.Packet, 14);
        }
        else
        {
          int num1 = (int) (byte) this.Room.Players.Users.Count<Client>((Func<Client, bool>) (Attribute => Attribute.Player.RoomPlayer.TeamId == Teams.WARFACE));
          int num2 = (int) (byte) this.Room.Players.Users.Count<Client>((Func<Client, bool>) (Attribute => Attribute.Player.RoomPlayer.TeamId == Teams.BLACKWOOD));
          User.Player.RoomPlayer.TeamId = num1 <= num2 ? Teams.WARFACE : Teams.BLACKWOOD;
          User.Player.RoomPlayer.Room = this.Room;
          this.Room.Players.Users.Add(User);
          if (!this.Room.CustomParams.SoldierEnabled || !this.Room.CustomParams.EngineerEnabled || (!this.Room.CustomParams.SniperEnabled || !this.Room.CustomParams.MedicEnabled))
          {
            foreach (Client client in this.Room.Players.Users.ToArray())
            {
              bool flag = false;
              if (client.Player.CurrentClass == (byte) 0 && !this.Room.CustomParams.SoldierEnabled)
                flag = true;
              if (client.Player.CurrentClass == (byte) 4 && !this.Room.CustomParams.EngineerEnabled)
                flag = true;
              if (client.Player.CurrentClass == (byte) 3 && !this.Room.CustomParams.MedicEnabled)
                flag = true;
              if (client.Player.CurrentClass == (byte) 2 && !this.Room.CustomParams.SniperEnabled)
                flag = true;
              if (flag)
              {
                if (this.Room.CustomParams.SoldierEnabled)
                  client.Player.CurrentClass = (byte) 0;
                if (this.Room.CustomParams.EngineerEnabled)
                  client.Player.CurrentClass = (byte) 4;
                if (this.Room.CustomParams.MedicEnabled)
                  client.Player.CurrentClass = (byte) 3;
                if (this.Room.CustomParams.SniperEnabled)
                  client.Player.CurrentClass = (byte) 2;
              }
            }
          }
        
          this.Process();
          if (this.Room.Core.RoomType == (byte) 4)
            this.Room.ClanBalanceProcess();
          else if (this.Room.CustomParams.AutoTeamBalance)
            this.Room.AutoBalanceProcess();
          else
            this.Room.Sync(User);
        }
      }
      catch
      {
      }
    }

    public override void Process()
    {
      XDocument xDocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) "k01.warface"));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) "gameroom_join");
      xelement3.Add((object) this.Room.Serialize(false));
      xelement3.Add((object) new XAttribute((XName) "room_id", (object) this.Room.Core.RoomId));
      xelement3.Add((object) new XAttribute((XName) "code", (object) 0));
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xDocument.Add((object) xelement1);
      this.Compress(ref xDocument);
      this.User.Send(xDocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
