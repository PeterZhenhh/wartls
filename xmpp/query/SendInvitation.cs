// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.SendInvitation
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using WARTLS.EXCEPTION;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class SendInvitation : Stanza
  {
    private SendInvitation.Error ErrorId = SendInvitation.Error.SUCCESSFILY_SENDED;
    private string Target;

    public SendInvitation(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      if (this.Query.Attributes["type"].InnerText == "64")
      {
        this.Target = this.Query.Attributes["target"].InnerText;
      }
      else
      {
        if (User.Player.ClanPlayer.ClanRole == Clan.CLAN_ROLE.DEFAULT || User.Player.ClanPlayer.ClanRole == Clan.CLAN_ROLE.NOT_IN_CLAN)
          throw new StanzaException(User, Packet, 6);
        if (User.Player.Clan.ClanMembers.Elements((XName) "clan_member_info").Count<XElement>() >= 50)
          throw new StanzaException(User, Packet, 11);
        this.Target = Client.ResolveNickname(long.Parse(this.Query.Attributes["target_id"].InnerText));
      }
      if (this.Target == User.Player.Nickname)
        return;
      Client User1 = ArrayList.OnlineUsers.Find((Predicate<Client>) (Attribute => Attribute.Player.Nickname == this.Target));
      if (User1 != null)
      {
        if (this.Query.Attributes["type"].InnerText == "16" && User1.Player.Clan.ID != 0L)
          throw new StanzaException(User, Packet, 5);
        if (this.Query.Attributes["type"].InnerText == "64")
        {
          foreach (XmlNode childNode in User.Player.friends["friends"].ChildNodes)
          {
            if (childNode.InnerText == User1.Player.UserID.ToString())
            {
              this.ErrorId = SendInvitation.Error.ALREADY_IN_FRIEND;
              this.Process();
              return;
            }
          }
          if (User.Player.friends["friends"].ChildNodes.Count >= 50)
          {
            this.ErrorId = SendInvitation.Error.LIMIT_REACHED;
            this.Process();
            return;
          }
          if (User1.Player.friends["friends"].ChildNodes.Count >= 50)
          {
            this.ErrorId = SendInvitation.Error.FRIEND_LIMIT_REACHED;
            this.Process();
            return;
          }
        }
        foreach (XmlNode childNode in User1.Player.Notifications["notifications"].ChildNodes)
        {
          if (childNode.Attributes["type"].InnerText == "64" && childNode["invitation"].Attributes["initiator"].InnerText == User.Player.Nickname)
          {
            this.ErrorId = SendInvitation.Error.IN_PROGRESS;
            this.Process();
            return;
          }
        }
        if (this.Query.Attributes["type"].InnerText == "16")
          User1.Player.AddClanNotification(User.Player.Nickname, this.Target, User.Player.UserID, User.JID, User.Player.Experience, User.Player.BannerBadge, User.Player.BannerMark, User.Player.BannerStripe, User.Player.Clan.ID, User.Player.Clan.Name);
        else
          User1.Player.AddFriendNotification(User.Player.Nickname, this.Target, User.Player.UserID, User.JID, User.Player.Experience, User.Player.BannerBadge, User.Player.BannerMark, User.Player.BannerStripe, User.Player.Clan.ID, User.Player.Clan.ID == 0L ? "" : User.Player.Clan.Name);
        User1.Player.Save();
        new SyncNotification(User1, (XmlDocument) null).Process();
      }
      else
      {
        if (this.Query.Attributes["type"].InnerText == "16")
          throw new StanzaException(User, Packet, 8);
        Player player = new Player()
        {
          Nickname = this.Target
        };
        if (!player.Load(true).Result)
        {
          this.ErrorId = SendInvitation.Error.USER_NOT_FOUND;
          this.Process();
          return;
        }
        foreach (XmlNode childNode in User.Player.friends["friends"].ChildNodes)
        {
          if (childNode.InnerText == player.UserID.ToString())
          {
            this.ErrorId = SendInvitation.Error.ALREADY_IN_FRIEND;
            this.Process();
            return;
          }
        }
        foreach (XmlNode childNode in player.Notifications["notifications"].ChildNodes)
        {
          if (childNode.Attributes["type"].InnerText == "64" && childNode["invitation"].Attributes["initiator"].InnerText == User.Player.Nickname)
          {
            this.ErrorId = SendInvitation.Error.IN_PROGRESS;
            this.Process();
            return;
          }
        }
        player.AddFriendNotification(User.Player.Nickname, this.Target, User.Player.UserID, User.JID, User.Player.Experience, User.Player.BannerBadge, User.Player.BannerMark, User.Player.BannerStripe, User.Player.Clan.ID, User.Player.Clan.ID == 0L ? "" : User.Player.Clan.Name);
        player.Save();
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
      XElement xelement3 = new XElement((XName) "send_invitation", new object[2]
      {
        (object) new XAttribute((XName) "type", (object) this.Query.Attributes["type"].InnerText),
        (object) new XAttribute((XName) "target", (object) this.Target)
      });
      xelement2.Add((object) xelement3);
      if (this.ErrorId != SendInvitation.Error.SUCCESSFILY_SENDED)
      {
        XElement xelement4 = new XElement((XNamespace) "urn:ietf:params:xml:ns:xmpp-stanzas" + "error");
        xelement4.Add((object) new XAttribute((XName) "type", (object) "continue"));
        xelement4.Add((object) new XAttribute((XName) "code", (object) 8));
        xelement4.Add((object) new XAttribute((XName) "custom_code", (object) (int) this.ErrorId));
        xelement1.Add((object) xelement4);
      }
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString());
    }

    private enum Error
    {
      REJECTED = 1,
      IN_PROGRESS = 2,
      SUCCESSFILY_SENDED = 3,
      ALREADY_IN_FRIEND = 4,
      ALREADY_IN_CLAN = 5,
      NOT_PERMISSIONS = 6,
      KICK_TIMEOUT = 7,
      USER_OFFLINE = 8,
      USER_NOT_FOUND = 9,
      USER_NOT_FOUND_IN_LOBBY = 10, // 0x0000000A
      LIMIT_REACHED = 11, // 0x0000000B
      FRIEND_LIMIT_REACHED = 12, // 0x0000000C
      TIMEOUT = 14, // 0x0000000E
      ENABLED_DND = 15, // 0x0000000F
    }
  }
}
