// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.TelemetryStream
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class TelemetryStream : Stanza
  {
    private static Regex Regex = new Regex("{\\[*.*\\]");
    private string Channel;

    public TelemetryStream(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      if (this.Query.Attributes["finalize"].InnerText == "0")
      {
        string input = WebUtility.HtmlDecode(this.Query.InnerText);
        foreach (Capture match in TelemetryStream.Regex.Matches(input))
        {
          string key = match.Value;
          int startIndex = input.IndexOf(key) + key.Length;
          if (input.Length != 0)
          {
            string text = input.Substring(startIndex);
            int length = text.IndexOf('{');
            if (length > -1)
              text = text.Substring(0, length);
            input = input.Replace(key + text, "");
            if (text.Length > 5)
            {
              if (!User.DedicatedTelemetryes.ContainsKey(key))
                User.DedicatedTelemetryes.Add(key, (XElement) null);
              if (User.DedicatedTelemetryes[key] == null)
                User.DedicatedTelemetryes[key] = XElement.Parse(text);
              else
                User.DedicatedTelemetryes[key].Add((object) XElement.Parse(text));
            }
          }
        }
      }
      else
      {
        List<Client> clientList = new List<Client>();
        foreach (XElement xelement in User.DedicatedTelemetryes.Values)
        {
          XmlDocument xmlDocument = new XmlDocument();
          xmlDocument.Load(xelement.CreateReader());
          try
          {
            string name = xmlDocument.FirstChild.Name;
            long ProfileId;
            if (name != null)
            {
              if (!(name == "player"))
              {
                if (name == "stats_session")
                {
                  if (xmlDocument["stats_session"].Attributes["winner"] != null)
                  {
                    foreach (XmlElement childNode in xmlDocument["stats_session"]["timelines"].ChildNodes)
                    {
                      switch (childNode.Attributes["name"].InnerText)
                      {
                        case "disconnect":
                          IEnumerator enumerator = childNode.ChildNodes.GetEnumerator();
                          try
                          {
                            while (enumerator.MoveNext())
                            {
                              XmlElement current = (XmlElement) enumerator.Current;
                              ProfileId = long.Parse(current["param"].Attributes["profile_id"].InnerText);
                              string innerText = current["param"].Attributes["cause"].InnerText;
                              Client client = WARTLS.CLASSES.ArrayList.OnlineUsers.Find((Predicate<Client>) (Attribute => Attribute.Player.UserID == ProfileId));
                              if (client != null)
                              {
                                if (!clientList.Contains(client))
                                  clientList.Add(client);
                                if (innerText == "left")
                                  client.Player.StatMgr.IncrementModePlayerStat("PVP", "player_sessions_left", 1L);
                              }
                            }
                            continue;
                          }
                          finally
                          {
                            if (enumerator is IDisposable disposable)
                              disposable.Dispose();
                          }
                        default:
                          continue;
                      }
                    }
                  }
                }
              }
              else
              {
                ProfileId = long.Parse(xmlDocument["player"].Attributes["profile_id"].InnerText);
                string innerText1 = xmlDocument["player"].Attributes["character_class"].InnerText;
                uint num1 = xmlDocument["player"]["player"] != null ? (xmlDocument["player"]["player"].Attributes["lifetime_end"] == null ? (xmlDocument["player"]["player"] == null ? uint.Parse(xmlDocument["player"]["timelines"].ChildNodes[xmlDocument["player"]["timelines"].ChildNodes.Count - 1]["val"].Attributes["time"].InnerText) - uint.Parse(xmlDocument["player"].Attributes["lifetime_begin"].InnerText) : uint.Parse(xmlDocument["player"]["player"]["timelines"].ChildNodes[xmlDocument["player"]["player"]["timelines"].ChildNodes.Count - 1]["val"].Attributes["time"].InnerText) - uint.Parse(xmlDocument["player"].Attributes["lifetime_begin"].InnerText)) / 10U : uint.Parse(xmlDocument["player"]["player"].Attributes["lifetime_end"].InnerText) - uint.Parse(xmlDocument["player"].Attributes["lifetime_begin"].InnerText)) : uint.Parse(xmlDocument["player"].Attributes["lifetime_end"].InnerText) - uint.Parse(xmlDocument["player"].Attributes["lifetime_begin"].InnerText);
                Client client = WARTLS.CLASSES.ArrayList.OnlineUsers.Find((Predicate<Client>) (Attribute => Attribute.Player.UserID == ProfileId));
                if (client != null)
                {
                  if (!clientList.Contains(client))
                    clientList.Add(client);
                  Player player = client.Player;
                  player.StatMgr.IncrementClassModePlayerStat(innerText1, "PVP", "player_playtime", (long) num1);
                  foreach (XmlElement childNode in xmlDocument["player"].ChildNodes)
                  {
                    if (childNode.Name == "player" || childNode.Name == "timelines")
                    {
                      foreach (XmlElement xmlElement in !(childNode.Name == "player") ? childNode.ChildNodes : childNode["timelines"].ChildNodes)
                      {
                        string innerText2 = xmlElement.Attributes["name"].InnerText;
                        switch (innerText2)
                        {
                          case null:
                            continue;
                          default:
                            switch (innerText2)
                            {
                              case "death":
                                player.StatMgr.IncrementModePlayerStat("PVP", "player_deaths", (long) xmlElement.ChildNodes.Count);
                                continue;
                              case "hit":
                                player.StatMgr.IncrementClassModePlayerStat(innerText1, "PVP", "player_hits", (long) xmlElement.ChildNodes.Count);
                                IEnumerator enumerator1 = xmlElement.ChildNodes.GetEnumerator();
                                try
                                {
                                  while (enumerator1.MoveNext())
                                  {
                                    int num2 = int.Parse(((XmlNode) enumerator1.Current).FirstChild.Attributes["damage"].InnerText);
                                    if (player.StatMgr.GetPlayerStat("player_max_damage") < (long) num2)
                                      player.StatMgr.ResetPlayerStat("player_max_damage", (long) num2);
                                    player.StatMgr.IncrementPlayerStat("player_damage", (long) num2);
                                  }
                                  continue;
                                }
                                finally
                                {
                                  if (enumerator1 is IDisposable disposable)
                                    disposable.Dispose();
                                }
                              case "kill":
                                player.StatMgr.IncrementModePlayerStat("PVP", "player_kills_player", (long) xmlElement.ChildNodes.Count);
                                IEnumerator enumerator2 = xmlElement.ChildNodes.GetEnumerator();
                                try
                                {
                                  while (enumerator2.MoveNext())
                                  {
                                    XmlElement current = (XmlElement) enumerator2.Current;
                                    if (current.FirstChild.Attributes["hit_type"] != null)
                                    {
                                      if (current.FirstChild.Attributes["hit_type"].InnerText == "melee")
                                        player.StatMgr.IncrementModePlayerStat("PVP", "player_kills_melee", 1L);
                                      else if (current.FirstChild.Attributes["hit_type"].InnerText == "clay_more")
                                        player.StatMgr.IncrementModePlayerStat("PVP", "player_kills_claymore", 1L);
                                    }
                                  }
                                  continue;
                                }
                                finally
                                {
                                  if (enumerator2 is IDisposable disposable)
                                    disposable.Dispose();
                                }
                              case "kill_streak":
                                IEnumerator enumerator3 = xmlElement.ChildNodes.GetEnumerator();
                                try
                                {
                                  while (enumerator3.MoveNext())
                                  {
                                    int num2 = int.Parse(((XmlNode) enumerator3.Current).Attributes["prm"].InnerText);
                                    player.StatMgr.IncrementModePlayerStat("PVP", "player_kill_streak", (long) num2);
                                  }
                                  continue;
                                }
                                finally
                                {
                                  if (enumerator3 is IDisposable disposable)
                                    disposable.Dispose();
                                }
                              case "resurrent":
                                player.StatMgr.IncrementPlayerStat("player_resurrected_by_medic", 1L);
                                continue;
                              case "score":
                                IEnumerator enumerator4 = xmlElement.ChildNodes.GetEnumerator();
                                try
                                {
                                  while (enumerator4.MoveNext())
                                  {
                                    XmlElement current = (XmlElement) enumerator4.Current;
                                    string innerText3 = current["param"].Attributes["event"].InnerText;
                                    switch (innerText3)
                                    {
                                      case null:
                                        continue;
                                      default:
                                        switch (innerText3)
                                        {
                                          case "claymore_kill":
                                            player.StatMgr.IncrementPlayerStat("player_kills_claymore", 1L);
                                            continue;
                                          case "headshot_kill":
                                            player.StatMgr.IncrementClassModePlayerStat(innerText1, "PVP", "player_headshots", 1L);
                                            continue;
                                          case "sm_coop_assist":
                                            player.StatMgr.IncrementPlayerStat("player_climb_assists", 1L);
                                            continue;
                                          case "sm_coop_climb":
                                            player.StatMgr.IncrementPlayerStat("player_climb_coops", 1L);
                                            continue;
                                          case "teammate_give_ammo":
                                            player.StatMgr.IncrementPlayerStat("player_ammo_restored", 1L);
                                            continue;
                                          case "teammate_kill":
                                            player.StatMgr.IncrementModePlayerStat("PVP", "player_kills_player_friendly", 1L);
                                            continue;
                                          case "teammate_restore":
                                            if (innerText1 == "Medic")
                                            {
                                              player.StatMgr.IncrementPlayerStat("player_heal", (long) int.Parse(current["param"].Attributes["score"].InnerText));
                                              continue;
                                            }
                                            if (innerText1 == "Engineer")
                                            {
                                              player.StatMgr.IncrementPlayerStat("player_repair", (long) int.Parse(current["param"].Attributes["score"].InnerText));
                                              continue;
                                            }
                                            continue;
                                          case "teammate_resurrect":
                                            player.StatMgr.IncrementPlayerStat("player_resurrect_made", 1L);
                                            continue;
                                          default:
                                            continue;
                                        }
                                    }
                                  }
                                  continue;
                                }
                                finally
                                {
                                  if (enumerator4 is IDisposable disposable)
                                    disposable.Dispose();
                                }
                              case "shot":
                                player.StatMgr.IncrementClassModePlayerStat(innerText1, "PVP", "player_shots", (long) xmlElement.ChildNodes.Count);
                                continue;
                              default:
                                continue;
                            }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
          catch (Exception ex)
          {
           Console.WriteLine(ex.Message);
          }
        }
        foreach (Client User1 in clientList)
        {
          GetPlayerStats getPlayerStats = new GetPlayerStats(User1, (XmlDocument) null);
        }
      }
      this.Process();
    }

    public override void Process()
    {
      XDocument xdocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) this.To));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) "telemetry_stream");
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
