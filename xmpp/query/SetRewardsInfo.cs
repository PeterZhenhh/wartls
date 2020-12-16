// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.SetRewardsInfo
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class SetRewardsInfo : Stanza
  {
    private List<XElement> Results = new List<XElement>();
    private List<Client> BcastReceivers = new List<Client>();
    private string Channel;

    public SetRewardsInfo(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      string innerText = this.Query.Attributes["winning_team_id"].InnerText;
      int int32 = Convert.ToInt32(Math.Round(double.Parse(this.Query.Attributes["session_time"].InnerText, (IFormatProvider) CultureInfo.InvariantCulture)));
      foreach (XmlNode childNode1 in this.Query.ChildNodes)
      {
        if (childNode1.Name == "team")
        {
          double num1 = 0.0;
          foreach (XmlElement childNode2 in childNode1.ChildNodes)
          {
            XmlElement PlayerEl = childNode2;
            try
            {
              Client client = ArrayList.OnlineUsers.Find((Predicate<Client>) (Attribute => Attribute.Player != null && Attribute.Player.UserID == long.Parse(PlayerEl.Attributes["profile_id"].InnerText)));
              if (client != null)
              {
                this.BcastReceivers.Add(client);
                if (PlayerEl.Attributes["in_session_from_start"].InnerText == "0")
                  int32 /= 2;
                double num2 = 0.0;
                double num3 = 0.0;
                double num4 = 0.0;
                double num5 = ((double) int32 / (childNode1.Attributes["id"].InnerText == innerText ? 2.61 : 3.1) + num1) * 5.0 * 2.0;
                double num6 = ((double) int32 / (childNode1.Attributes["id"].InnerText == innerText ? 3.31 : 3.68) + num1) * 5.0 * 2.0;
                double num7 = ((double) int32 / (childNode1.Attributes["id"].InnerText == innerText ? 2.97 : 3.45) + num1) * 5.0 * 2.0;
                if (User.Player.RoomPlayer.Room.Mission.Mode != "ffa" && User.Player.RoomPlayer.Room.Mission.Mode != "hnt")
                {
                  if (childNode1.Attributes["id"].InnerText == innerText)
                  {
                    client.Player.StatMgr.IncrementDifficultyModePlayerStat("", "PVP", "player_sessions_won", 1L);
                    num5 *= 1.5;
                    num6 *= 1.5;
                    num7 *= 1.5;
                  }
                  else
                    client.Player.StatMgr.IncrementDifficultyModePlayerStat("", "PVP", "player_sessions_lost", 1L);
                }
                foreach (Item obj in client.Player.Items)
                {
                  if (obj.Name == "booster_02" && obj.SecondsLeft > 0L)
                  {
                    ++num3;
                    num2 += 0.5;
                    num4 += 0.75;
                  }
                  else if (obj.Name == "booster_11" && obj.SecondsLeft > 0L)
                  {
                    num3 += 1.79999995231628;
                    num2 += 1.29999995231628;
                    num4 += 1.64999997615814;
                  }
                  else if (obj.Name == "booster_01" && obj.SecondsLeft > 0L)
                    num4 += 0.150000005960464;
                  else if (obj.Name == "booster_03" && obj.SecondsLeft > 0L)
                    num3 += 0.150000005960464;
                  else if (obj.Name == "booster_04" && obj.SecondsLeft > 0L)
                    num2 += 0.150000005960464;
                }
                if (num2 != 0.0)
                  num7 *= num2;
                if (num4 != 0.0)
                  num6 *= num4;
                if (num3 != 0.0)
                  num5 *= num3;
                if (client.Player.Experience < 28856000)
                {
                  client.Player.Experience += (int) num5;
                }
                else
                {
                  num5 = 0.0;
                  client.Player.Experience = 28856000;
                }
                client.Player.GameMoney += (int) num6;
                client.Player.CrownMoney += 50;
                if (client.Player.StatMgr.GetPlayerStat("player_max_session_time") < (long) (int32 * 10))
                  client.Player.StatMgr.IncrementPlayerStat("player_max_session_time", (long) (int32 * 10));
                client.Player.StatMgr.IncrementPlayerStat("player_online_time", (long) (int32 * 10));
                client.Player.StatMgr.IncrementPlayerStat("player_gained_money", (long) (int) num6);
                XElement xelement1 = new XElement((XName) "player_result");
                xelement1.Add((object) new XAttribute((XName) "nickname", (object) client.Player.Nickname));
                xelement1.Add((object) new XAttribute((XName) "experience", (object) (int) num5));
                xelement1.Add((object) new XAttribute((XName) "sponsor_points", (object) (int) num7));
                xelement1.Add((object) new XAttribute((XName) "money", (object) (int) num6));
                xelement1.Add((object) new XAttribute((XName) "bonus_money", (object) 0));
                xelement1.Add((object) new XAttribute((XName) "gained_crown_money", (object) 0));
                xelement1.Add((object) new XAttribute((XName) "bonus_experience", (object) 0));
                xelement1.Add((object) new XAttribute((XName) "bonus_sponsor_points", (object) 0));
                xelement1.Add((object) new XAttribute((XName) "completed_stages", (object) ""));
                xelement1.Add((object) new XAttribute((XName) "money_boost", (object) 0));
                xelement1.Add((object) new XAttribute((XName) "experience_boost", (object) 0));
                xelement1.Add((object) new XAttribute((XName) "sponsor_points_boost", (object) 0));
                xelement1.Add((object) new XAttribute((XName) "experience_boost_percent", (object) num3));
                xelement1.Add((object) new XAttribute((XName) "money_boost_percent", (object) num4));
                xelement1.Add((object) new XAttribute((XName) "sponsor_points_boost_percent", (object) num2));
                xelement1.Add((object) new XAttribute((XName) "is_vip", (object) (num3 != 0.0 || num2 != 0.0 || num4 != 0.0 ? 1 : 0)));
                xelement1.Add((object) new XAttribute((XName) "score", (object) client.Player.Nickname));
                xelement1.Add((object) new XAttribute((XName) "no_crown_rewards", (object) 0));
                xelement1.Add((object) new XAttribute((XName) "dynamic_multipliers_info", (object) 0));
                this.Results.Add(xelement1);
                if (client.Player.Clan.ID != 0L)
                {
                  if (User.Player.RoomPlayer.Room.Core.RoomType == (byte) 4)
                  {
                    if (childNode1.Attributes["id"].InnerText == innerText)
                      client.Player.ClanPlayer.Points += Convert.ToInt32((double) int32 * 1.5);
                    else
                      client.Player.ClanPlayer.Points += Convert.ToInt32((double) int32 / 1.5);
                  }
                  else
                    client.Player.ClanPlayer.Points += Convert.ToInt32(int32 / 3);
                  client.Player.Clan.UpdatePlayer(client.Player);
                  foreach (XElement xelement2 in client.Player.Clan.ClanMembers.Elements((XName) "clan_member_info").ToList<XElement>())
                  {
                    XElement clanMember = xelement2;
                    Client User1 = ArrayList.OnlineUsers.Find((Predicate<Client>) (x => x.Player.UserID == long.Parse(clanMember.Attribute((XName) "profile_id").Value)));
                    if (User1 != null)
                    {
                      ClanInfo clanInfo = new ClanInfo(User1, (XmlDocument) null);
                    }
                  }
                }
                client.Player.Save();
              }
            }
            catch
            {
            }
          }
        }
      }
      foreach (Client bcastReceiver in this.BcastReceivers)
        new BroadcastSessionResults(bcastReceiver, this.Results).Process();
      this.Process();
    }

    public override void Process()
    {
      XDocument xdocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) "masterserver@warface/onyx"));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) "set_rewards_info");
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
