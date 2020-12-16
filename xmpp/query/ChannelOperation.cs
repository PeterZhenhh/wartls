// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.ChannelOperation
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll


using MySql.Data.MySqlClient;
using WARTLS.CLASSES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class ChannelOperation : Stanza
  {
    private ChannelOperation.Error ErrorCode = ChannelOperation.Error.NO;
    private string Version;
    private string Resource;
    private string RegionId;
    private string BuildType;
    public XElement BonusStreak;

        public ChannelOperation(Client User, XmlDocument Packet)
          : base(User, Packet)
        {
            if (User.Channel != null)
            {
                ChannelLogout channelLogout = new ChannelLogout(User, (XmlDocument)null);
            }
            this.Resource = this.Query.Attributes["resource"].InnerText;
            this.BuildType = this.Query.Attributes["build_type"].InnerText;
            Version = Query.Attributes["version"].InnerText;
            if (Version != "1.15000.124.34300")
            {
                this.ErrorCode = ChannelOperation.Error.INVALID_GAMEVERSION;
            }
            User.Channel = ArrayList.Channels.Find((Predicate<Channel>)(Attribute => Attribute.Resource == this.Resource));
            if ((int)User.Channel.MinRank >= (int)User.Player.Rank && (int)User.Channel.MaxRank <= (int)User.Player.Rank)
            {
                this.ErrorCode = ChannelOperation.Error.FULL_CHANNEL;
            }
            else
            {
                User.Channel.Users.Add(User);
                if (this.Query.Name == "create_profile" && !User.Player.ProfileCreated)
                {
                    using (MySqlConnection result = SQL.GetConnection().GetAwaiter().GetResult())
                    {
                        MySqlCommand сmd = new MySqlCommand();
                        сmd.Connection = result;
                        сmd.Prepare();
                        string s = сmd.CommandText = "SELECT id FROM users WHERE nickname = @nickname";
                        сmd.Parameters.AddWithValue("@nickname", User.Player.Nickname);
                        s = сmd.ExecuteScalar().ToString();
                        сmd.CommandText = "INSERT INTO players (ID, Experience, Avatar, Items, Settings, Achievements, Notifications, Stats, Friends, RandomBox, LastActivity, PrivilegieId, BanType, UnbanTime) VALUES (" + s + ", @Experience, @Avatar, @Items, @Settings, @Achievements, @Notifications, @Stats, @Friends, @RandomBox, @LastActivity, @PrivilegieId, @BanType, @UnbanTime)";
                        сmd.Parameters.AddWithValue("@Experience", 0);
                        сmd.Parameters.AddWithValue("@Avatar", 0);
                        сmd.Parameters.AddWithValue("@Items", 0);
                        сmd.Parameters.AddWithValue("@Settings", 0);
                        сmd.Parameters.AddWithValue("@Achievements", 0);
                        сmd.Parameters.AddWithValue("@Notifications", 0);
                        сmd.Parameters.AddWithValue("@Stats", 0);
                        сmd.Parameters.AddWithValue("@Friends", 0);
                        сmd.Parameters.AddWithValue("@RandomBox", 0);
                        сmd.Parameters.AddWithValue("@LastActivity", 0);
                        сmd.Parameters.AddWithValue("@PrivilegieId", 0);
                        сmd.Parameters.AddWithValue("@BanType", 0);
                        сmd.Parameters.AddWithValue("@UnbanTime", 0);
                        сmd.ExecuteScalar();

                        User.Player = new Player()
                        {
                            TicketId = User.Player.TicketId,
                            Nickname = User.Player.Nickname,
                            UserID = long.Parse(s),
                            Head = this.Query.Attributes["head"].InnerText.StartsWith("default_head_4") ? this.Query.Attributes["head"].InnerText : "default_head_04"
                        };
                        Console.WriteLine(string.Format("{0:0000}", (object)User.Player.UserID) + " >> created profile");
                        new MySqlCommand(string.Format("UPDATE users SET profileid='{0}', nickname='{1}' WHERE id='{2}';", User.Player.UserID, User.Player.Nickname, User.Player.TicketId), result).ExecuteScalar();
                        User.Player.Save();
                        result.Close();
                    }
                }
                this.Process();
                if (this.Query.Name != "switch_channel")
                {
                    new FriendList(User, (XmlDocument)null).Process();
                    User.Player.Clan.Find(User);
                    User.Player.CheckDailyBonus();
                }
                User.Status = 3;
                User.ShowMessage("Добро пожаловать!) Приятной вам игры)", true);
    
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
      XElement xelement3 = new XElement((XName) this.Query.Name);
            if (this.ErrorCode == ChannelOperation.Error.NO)
            {
                xelement3.Add((object)new XAttribute((XName)"profile_id", (object)this.User.Player.UserID));
                XElement xelement4 = new XElement((XName)"character");
                xelement4.Add((object)new XAttribute((XName)"nick", (object)this.User.Player.Nickname));
                xelement4.Add((object)new XAttribute((XName)"gender", (object)this.User.Player.Gender));
                xelement4.Add((object)new XAttribute((XName)"height", (object)this.User.Player.Height));
                xelement4.Add((object)new XAttribute((XName)"fatness", (object)this.User.Player.Fatness));
                xelement4.Add((object)new XAttribute((XName)"head", (object)this.User.Player.Head));
                xelement4.Add((object)new XAttribute((XName)"current_class", (object)this.User.Player.CurrentClass));
                xelement4.Add((object)new XAttribute((XName)"experience", (object)this.User.Player.Experience));
                xelement4.Add((object)new XAttribute((XName)"pvp_rating", (object)"0"));
                xelement4.Add((object)new XAttribute((XName)"pvp_rating_points", (object)"0"));
                xelement4.Add((object)new XAttribute((XName)"banner_badge", (object)this.User.Player.BannerBadge));
                xelement4.Add((object)new XAttribute((XName)"banner_mark", (object)this.User.Player.BannerMark));
                xelement4.Add((object)new XAttribute((XName)"banner_stripe", (object)this.User.Player.BannerStripe));
                xelement4.Add((object)new XAttribute((XName)"game_money", (object)this.User.Player.GameMoney));
                xelement4.Add((object)new XAttribute((XName)"cry_money", (object)this.User.Player.CryMoney));
                xelement4.Add((object)new XAttribute((XName)"crown_money", (object)this.User.Player.CrownMoney));
                xelement4.Add((new XElement("login_bonus", new XAttribute("current_streak", User.Player.CurrentBonusStreak))));
                xelement4.Add((new XAttribute("current_reward", User.Player.CurrentBonusPos)));

                if (this.User.Player.Notifications.FirstChild.ChildNodes.Count > 0)
                {
                    foreach (XmlNode childNode in this.User.Player.Notifications.FirstChild.ChildNodes)
                        xelement4.Add((object)XDocument.Parse(childNode.OuterXml).Root);

                }
                XElement xelement5 = new XElement((XName)"profile_progression_state");
                xelement5.Add((object)new XAttribute((XName)"profile_id", (object)this.User.Player.UserID));
                xelement5.Add((object)new XAttribute((XName)"mission_unlocked", (object)this.User.Player.UnlockedMissions));
                xelement5.Add((object)new XAttribute((XName)"tutorial_unlocked", (object)this.User.Player.TutorialSuggest));
                xelement5.Add((object)new XAttribute((XName)"tutorial_passed", (object)this.User.Player.TutorialPassed));
                xelement5.Add((object)new XAttribute((XName)"class_unlocked", (object)this.User.Player.UnlockedClasses));
                XElement xelement6 = new XElement((XName)"chat_channels");
                XElement xelement7 = new XElement((XName)"chat");
                xelement7.Add((object)new XAttribute((XName)"channel", (object)0));
                xelement7.Add((object)new XAttribute((XName)"channel_id", (object)this.User.Channel.JID));
                xelement7.Add((object)new XAttribute((XName)"service_id", (object)"conference.warface"));
                xelement6.Add((object)xelement7);
                if (this.Query.Name != "switch_channel")
                {
                    XElement xelement8 = new XElement((XName)"sponsor_info");
                    xelement8.Add((object)new XElement((XName)"sponsor", new object[3]
                    {
            (object) new XAttribute((XName) "sponsor_id", (object) "0"),
            (object) new XAttribute((XName) "sponsor_points", (object) "0"),
            (object) new XAttribute((XName) "next_unlock_item", (object) "")
                    }));
                    xelement8.Add((object)new XElement((XName)"sponsor", new object[3]
                    {
            (object) new XAttribute((XName) "sponsor_id", (object) "1"),
            (object) new XAttribute((XName) "sponsor_points", (object) "0"),
            (object) new XAttribute((XName) "next_unlock_item", (object) "")
                    }));
                    xelement8.Add((object)new XElement((XName)"sponsor", new object[3]
                    {
            (object) new XAttribute((XName) "sponsor_id", (object) "2"),
            (object) new XAttribute((XName) "sponsor_points", (object) "0"),
            (object) new XAttribute((XName) "next_unlock_item", (object) "")
                    }));

                    foreach (Item obj in this.User.Player.Items)
                    {
                        if (obj.ItemType == WARTLS.CLASSES.ItemType.TIME && obj.SecondsLeft <= 0L && !obj.ExpiredConfirmed)
                        {
                            xelement4.Add((object)new XElement((XName)"expired_item", new object[3]
                            {
                (object) new XAttribute((XName) "id", (object) obj.ID),
                (object) new XAttribute((XName) "name", (object) obj.Name),
                (object) new XAttribute((XName) "slot_ids", (object) obj.Slot)
                            }));
                            obj.ExpiredConfirmed = true;
                        }
                        xelement4.Add((object)obj.Serialize(false, 0));
                    }
                    


                   
                   
                };
        xelement4.Add((object) xelement6);
        xelement4.Add((object) xelement5);
        xelement4.Add((object) Core.GameResources.OnlineVariables.ToXDocument().FirstNode);
        xelement3.Add((object) xelement4);
        xelement2.Add((object) xelement3);
        xelement1.Add((object) xelement2);
        Console.WriteLine((object) xelement1);
      }
      else
      {
        XElement xelement4 = new XElement((XNamespace) "urn:ietf:params:xml:ns:xmpp-stanzas" + "error");
        xelement4.Add((object) new XAttribute((XName) "type", (object) "cancel"));
        xelement4.Add((object) new XAttribute((XName) "code", (object) 8));
        xelement4.Add((object) new XAttribute((XName) "custom_code", (object) (int) this.ErrorCode));
        xelement2.Add((object) xelement3);
        xelement1.Add((object) xelement2);
        xelement1.Add((object) xelement4);
      }
      xDocument.Add((object) xelement1);
      this.Compress(ref xDocument);
      this.User.Player.Save();
      this.User.Send(xDocument.ToString(SaveOptions.DisableFormatting));
   
    }

    private enum Error
    {
      NO = -1, // 0xFFFFFFFF
      PROFILE_NOT_EXIST = 1,
      INVALID_GAMEVERSION = 2,
      BANNED = 3,
      FULL_CHANNEL = 5,
    }
  }
}
