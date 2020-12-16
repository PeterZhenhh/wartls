// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.GameRoom_Open
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using WARTLS.CLASSES.GAMEROOM;
using WARTLS.EXCEPTION;
using System;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class GameRoom_Open : Stanza
  {
    private string Channel;
    private int Code;

    public GameRoom_Open(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      if (User.Channel.ChannelType == "pve")
      {
        this.Query.Attributes["max_players"].InnerText = "5";
      }
      else
      {
        if (byte.Parse(this.Query.Attributes["max_players"].InnerText) > (byte) 16)
          this.Code = 3;
        if (byte.Parse(this.Query.Attributes["max_players"].InnerText) < (byte) 4)
          this.Code = 3;
        if ((int) byte.Parse(this.Query.Attributes["max_players"].InnerText) % 2 != 0)
          this.Code = 3;
      }
      if (this.Code != 0)
      {
        this.Process();
      }
      else
      {
        string Uid = this.Query.Attributes["mission"].InnerText;
        if (User.Player.RoomPlayer.Room != null)
        {
          GameRoom_Leave gameRoomLeave = new GameRoom_Leave(User, (XmlDocument) null);
        }
        GameRoom gameRoom = new GameRoom()
        {
          Core = {
            RoomId = GameRoom.Seed
          }
        };
        ++GameRoom.Seed;
        gameRoom.RoomMaster.UserId = User.Player.UserID;
        if (User.Channel.ChannelType != "pve")
        {
          XmlDocument xmlDocument = Core.GameResources.Maps.Find((Predicate<XmlDocument>) (Attribute => Attribute.FirstChild.Attributes["uid"].InnerText == Uid));
                    if (xmlDocument.FirstChild.Attributes["release_mission"].InnerText == "0")
                    {
                        StanzaException stanzaException = new StanzaException(User, Packet, 1);
                    }
                    else
                    {
                        gameRoom.Mission.Map = xmlDocument;
                    }
        }
                if (User.Channel.ChannelType == "pve")
                {
                    XmlDocument xmlDocument = Core.GameResources.Maps.Find((XmlDocument Attribute) => Attribute.FirstChild.Attributes["uid"].InnerText == Uid);
                    if (xmlDocument.FirstChild.Attributes["release_mission"].InnerText == "0")
                    {
                        new StanzaException(User, Packet, 1);
                    }
                    else
                    {
                        gameRoom.Mission.Map = xmlDocument;
                    }
                }
                gameRoom.Core.Name = this.Query.Attributes["room_name"].InnerText;
        gameRoom.Core.RoomType = User.Channel.ChannelType == "pve" ? (byte) 1 : (byte) 2;
        if (this.Query.Attributes["group_id"] != null)
          User.Player.RoomPlayer.GroupId = this.Query.Attributes["group_id"].InnerText;
        if (this.Query.Attributes["private"] != null)
          gameRoom.Core.Private = this.Query.Attributes["private"].InnerText == "1";
        if (this.Query.Attributes["round_limit"] != null && (this.Query.Attributes["round_limit"].InnerText == "6" || this.Query.Attributes["round_limit"].InnerText == "11" || this.Query.Attributes["round_limit"].InnerText == "16"))
          gameRoom.CustomParams.RoundLimit = Convert.ToByte(this.Query.Attributes["round_limit"].InnerText);
        if (this.Query.Attributes["room_type"] != null)
        {
          gameRoom.Core.RoomType = byte.Parse(this.Query.Attributes["room_type"].InnerText);
          if (gameRoom.Core.RoomType == (byte) 4 && User.Player.Clan == null)
          {
            StanzaException stanzaException = new StanzaException(User, Packet, 1);
          }
        }
        if (this.Query.Attributes["friendly_fire"] != null)
          gameRoom.CustomParams.FriendlyFire = this.Query.Attributes["friendly_fire"].InnerText == "1";
        if (this.Query.Attributes["enemy_outlines"] != null)
          gameRoom.CustomParams.EmenyOutlines = this.Query.Attributes["enemy_outlines"].InnerText == "1";
        if (this.Query.Attributes["auto_team_balance"] != null)
          gameRoom.CustomParams.AutoTeamBalance = this.Query.Attributes["auto_team_balance"].InnerText == "1";
        if (this.Query.Attributes["dead_can_chat"] != null)
          gameRoom.CustomParams.DeadCanChat = this.Query.Attributes["dead_can_chat"].InnerText == "1";
        if (this.Query.Attributes["join_in_the_process"] != null)
          gameRoom.CustomParams.JoinInProcess = this.Query.Attributes["join_in_the_process"].InnerText == "1";
        if (this.Query.Attributes["max_players"] != null)
          gameRoom.CustomParams.MaxPlayers = User.Channel.ChannelType == "pve" ? (byte) 5 : byte.Parse(this.Query.Attributes["max_players"].InnerText);
        if (this.Query.Attributes["inventory_slot"] != null)
          gameRoom.CustomParams.InventorySlot = int.Parse(this.Query.Attributes["inventory_slot"].InnerText);
        if (this.Query.Attributes["locked_spectator_camera"] != null)
          gameRoom.CustomParams.LockedSpectatorCamera = this.Query.Attributes["locked_spectator_camera"].InnerText == "1";
        if (this.Query.Attributes["overtime_mode"] != null)
          gameRoom.CustomParams.OvertimeMode = this.Query.Attributes["overtime_mode"].InnerText == "1";
        if (this.Query.Attributes["class_rifleman"] != null)
          gameRoom.CustomParams.SoldierEnabled = this.Query["class_rifleman"].Attributes["enabled"].InnerText == "1";
        if (this.Query.Attributes["class_medic"] != null)
          gameRoom.CustomParams.MedicEnabled = this.Query["class_medic"].Attributes["enabled"].InnerText == "1";
        if (this.Query.Attributes["class_engineer"] != null)
          gameRoom.CustomParams.EngineerEnabled = this.Query["class_engineer"].Attributes["enabled"].InnerText == "1";
        if (this.Query.Attributes["class_sniper"] != null)
          gameRoom.CustomParams.SniperEnabled = this.Query["class_sniper"].Attributes["enabled"].InnerText == "1";
        User.Player.RoomPlayer.Room = gameRoom;
        User.Player.RoomPlayer.TeamId = Teams.WARFACE;
        User.Channel.GameRoomList.Add(gameRoom);
        gameRoom.Players.Users.Add(User);
        if (gameRoom.Core.RoomType == (byte) 4)
          gameRoom.ClanWar.ClanFirst = User.Player.Clan.Name;
          Program.WriteLine($"Игрок {User.Player.Nickname} создал комнату. Карта: {gameRoom.Mission.Map}, ID: {gameRoom.Core.RoomId}", ConsoleColor.Yellow);
                this.Process();
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
      XElement xelement3 = new XElement((XName) "gameroom_open");
      if (this.Code != 0)
      {
        XElement xelement4 = new XElement((XNamespace) "urn:ietf:params:xml:ns:xmpp-stanzas" + "error");
        xelement4.Add((object) new XAttribute((XName) "type", (object) "continue"));
        xelement4.Add((object) new XAttribute((XName) "code", (object) 8));
        xelement4.Add((object) new XAttribute((XName) "custom_code", (object) this.Code));
        xelement1.Add((object) xelement4);
      }
      else
        xelement3.Add((object) this.User.Player.RoomPlayer.Room.Serialize(false));
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xDocument.Add((object) xelement1);
      if (this.Code == 0)
        this.Compress(ref xDocument);
      this.User.Send(xDocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
