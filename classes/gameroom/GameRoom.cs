// Decompiled with JetBrains decompiler
// Type: OnyxServer.CLASSES.GAMEROOM.GameRoom
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.classes.gameroom;
using WARTLS.CLASSES.GAMEROOM.CORE;
using WARTLS.xmpp.query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.CLASSES.GAMEROOM
{
  public class GameRoom
  {
    public static long Seed = 1;
    public ClanWar ClanWar = new ClanWar();
    public WARTLS.CLASSES.GAMEROOM.CORE.Core Core = new WARTLS.CLASSES.GAMEROOM.CORE.Core();
    public Session Session = new Session();
    public RoomMaster RoomMaster = new RoomMaster();
    public PlayersReserved PlayersReserved = new PlayersReserved();
    public Mission Mission = new Mission();
    public CustomParams CustomParams = new CustomParams();
    public TeamColors TeamColors = new TeamColors();
    public Players Players = new Players();
    public string Name = "NoNameYet";
    public List<long> KickedUsers = new List<long>();
    public Client Dedicated;

    public byte MaxPlayerAtTeam
    {
      get
      {
        return Convert.ToByte((int) this.CustomParams.MaxPlayers / 2);
      }
    }

    public void SessionStarter()
    {
            if (Dedicated != null)
            {
                Dedicated.Player.RoomPlayer.Room = this;
                new MissionLoad(Dedicated).Process();
            }
        }

    public void Sync(Client NonInclude = null)
    {
            try
            {
                Client[] array = Players.Users.ToArray();
                foreach (Client client in array)
                {
                    if (NonInclude != client)
                    {
                        new GameRoom_Sync(client).Process();
                    }
                }
            }
            catch
            {
            }
        }

    public Task SyncAsync(Client NonInclude = null)
    {
      return Task.Run((Action) (() =>
      {
        foreach (Client User in this.Players.Users.ToArray())
        {
          if (NonInclude != User)
            new GameRoom_Sync(User, (XmlDocument) null).Process();
        }
      }));
    }

    public void ClanBalanceProcess()
    {
      this.Players.Users = this.Players.Users.ToList<Client>().OrderBy<Client, bool>((Func<Client, bool>) (Attribute => Attribute.Player.RoomPlayer.Status == Status.READY)).ToList<Client>();
      foreach (Client client in this.Players.Users.ToArray())
      {
        if (client.Player.Clan.Name == this.ClanWar.ClanFirst || this.ClanWar.ClanFirst == "")
        {
          this.ClanWar.ClanFirst = client.Player.Clan.Name;
          client.Player.RoomPlayer.TeamId = Teams.WARFACE;
        }
        else if (client.Player.Clan.Name == this.ClanWar.ClanSecond || this.ClanWar.ClanSecond == "")
        {
          this.ClanWar.ClanSecond = client.Player.Clan.Name;
          client.Player.RoomPlayer.TeamId = Teams.BLACKWOOD;
        }
        byte num1 = (byte) this.Players.Users.Count<Client>((Func<Client, bool>) (Attribute => Attribute.Player.RoomPlayer.TeamId == Teams.WARFACE));
        int num2 = (int) (byte) this.Players.Users.Count<Client>((Func<Client, bool>) (Attribute => Attribute.Player.RoomPlayer.TeamId == Teams.BLACKWOOD));
        if (num1 == (byte) 0)
          this.ClanWar.ClanFirst = "";
        if (num2 == 0)
          this.ClanWar.ClanSecond = "";
      }
      this.Sync((Client) null);
    }

    public void AutoBalanceProcess()
    {
      this.Players.Users = this.Players.Users.ToList<Client>().OrderBy<Client, bool>((Func<Client, bool>) (Attribute => Attribute.Player.RoomPlayer.Status == Status.READY)).ToList<Client>();
      foreach (Client client in this.Players.Users.ToArray())
      {
        byte num1 = (byte) this.Players.Users.Count<Client>((Func<Client, bool>) (Attribute => Attribute.Player.RoomPlayer.TeamId == Teams.WARFACE));
        byte num2 = (byte) this.Players.Users.Count<Client>((Func<Client, bool>) (Attribute => Attribute.Player.RoomPlayer.TeamId == Teams.BLACKWOOD));
        if ((int) num1 > (int) num2)
          client.Player.RoomPlayer.TeamId = Teams.BLACKWOOD;
        else if ((int) num2 > (int) num1)
          client.Player.RoomPlayer.TeamId = Teams.WARFACE;
      }
      this.Sync((Client) null);
    }

    public XElement Serialize(bool IncludeData = false)
    {
      XElement xelement1 = this.Core.Serialize(this.RoomMaster.UserId, this.Players);
      XElement xelement2 = new XElement((XName) "game_room");
      xelement2.Add((object) new XAttribute((XName) "room_id", (object) this.Core.RoomId));
      xelement2.Add((object) new XAttribute((XName) "room_type", (object) this.Core.RoomType));
      xelement1.Add((object) this.Players.Serialize());
      xelement1.Add((object) this.PlayersReserved.Serialize());
      xelement1.Add((object) this.TeamColors.Serialize());
      xelement2.Add((object) xelement1);
      xelement2.Add((object) this.Session.Serialize());
      xelement2.Add((object) this.Mission.Serialize(IncludeData));
      xelement2.Add((object) this.CustomParams.Serialize());
      xelement2.Add((object) this.RoomMaster.Serialize());
      if (this.Core.RoomType == (byte) 4)
        xelement2.Add((object) this.ClanWar.Serialize());
      return xelement2;
    }
  }
}
