// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.GameRoom_UpdatePvP
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
  public class GameRoom_UpdatePvP : Stanza
  {
    private int ErrorId = -1;
    private string Channel;

    public GameRoom_UpdatePvP(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      GameRoom room = User.Player.RoomPlayer.Room;
      try
      {
        if (room.Mission.Map.FirstChild.Attributes["uid"].InnerText != this.Query.Attributes["mission_key"].InnerText)
        {
          XmlDocument xmlDocument = Core.GameResources.Maps.Find((Predicate<XmlDocument>) (Attribute => Attribute.FirstChild.Attributes["uid"].InnerText == this.Query.Attributes["mission_key"].InnerText));
          if (xmlDocument.FirstChild.Attributes["release_mission"].InnerText == "0")
          {
            StanzaException stanzaException = new StanzaException(User, Packet, 1);
          }
          else
          {
            room.Mission.Map = xmlDocument;
            ++room.Mission.Revision;
          }
        }
      }
      catch
      {
        this.ErrorId = 1;
        this.Process();
        return;
      }
      if (this.Query.Attributes["private"] != null)
        room.Core.Private = this.Query.Attributes["private"].InnerText == "1";
      if (this.Query.Attributes["round_limit"] != null && (this.Query.Attributes["round_limit"].InnerText == "6" || this.Query.Attributes["round_limit"].InnerText == "11" || this.Query.Attributes["round_limit"].InnerText == "16"))
        room.CustomParams.RoundLimit = Convert.ToByte(this.Query.Attributes["round_limit"].InnerText);
      if (this.Query.Attributes["friendly_fire"] != null)
        room.CustomParams.FriendlyFire = this.Query.Attributes["friendly_fire"].InnerText == "1";
      if (this.Query.Attributes["enemy_outlines"] != null)
        room.CustomParams.EmenyOutlines = this.Query.Attributes["enemy_outlines"].InnerText == "1";
      if (this.Query.Attributes["auto_team_balance"] != null)
        room.CustomParams.AutoTeamBalance = this.Query.Attributes["auto_team_balance"].InnerText == "1";
      if (this.Query.Attributes["dead_can_chat"] != null)
        room.CustomParams.DeadCanChat = this.Query.Attributes["dead_can_chat"].InnerText == "1";
      if (this.Query.Attributes["join_in_the_process"] != null)
        room.CustomParams.JoinInProcess = this.Query.Attributes["join_in_the_process"].InnerText == "1";
      if (this.Query.Attributes["max_players"] != null)
        room.CustomParams.MaxPlayers = byte.Parse(this.Query.Attributes["max_players"].InnerText);
      if (this.Query.Attributes["inventory_slot"] != null)
        room.CustomParams.InventorySlot = int.Parse(this.Query.Attributes["inventory_slot"].InnerText);
      if (this.Query.Attributes["locked_spectator_camera"] != null)
        room.CustomParams.LockedSpectatorCamera = this.Query.Attributes["locked_spectator_camera"].InnerText == "1";
      if (this.Query.Attributes["overtime_mode"] != null)
        room.CustomParams.OvertimeMode = this.Query.Attributes["overtime_mode"].InnerText == "1";
      if (this.Query["class_rifleman"] != null)
        room.CustomParams.SoldierEnabled = this.Query["class_rifleman"].Attributes["enabled"].InnerText == "1";
      if (this.Query["class_medic"] != null)
        room.CustomParams.MedicEnabled = this.Query["class_medic"].Attributes["enabled"].InnerText == "1";
      if (this.Query["class_engineer"] != null)
        room.CustomParams.EngineerEnabled = this.Query["class_engineer"].Attributes["enabled"].InnerText == "1";
      if (this.Query["class_sniper"] != null)
        room.CustomParams.SniperEnabled = this.Query["class_sniper"].Attributes["enabled"].InnerText == "1";
      ++room.CustomParams.Revision;
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
      if (room.CustomParams.AutoTeamBalance && room.Players.Users.Count != 1)
        room.AutoBalanceProcess();
      else
        room.Sync(User);
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
      XElement xelement3 = new XElement((XName) "gameroom_update_pvp");
      xelement3.Add((object) this.User.Player.RoomPlayer.Room.Serialize(false));
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xDocument.Add((object) xelement1);
      this.Compress(ref xDocument);
      this.User.Send(xDocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
