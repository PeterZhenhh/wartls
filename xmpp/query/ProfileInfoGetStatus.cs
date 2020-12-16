// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.ProfileInfoGetStatus
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class ProfileInfoGetStatus : Stanza
  {
    private string Nickname;
    private Client OnlineUser;

    public ProfileInfoGetStatus(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      string innerText = this.Query.Attributes["nickname"].InnerText;
      this.Nickname = !innerText.Contains(" ") ? innerText : innerText.Split(' ', StringSplitOptions.None)[1];
      this.OnlineUser = ArrayList.OnlineUsers.Find((Predicate<Client>) (Attribute => Attribute.Player.Nickname == this.Nickname));
      Client onlineUser = this.OnlineUser;
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
      XElement xelement3 = new XElement((XName) "profile_info_get_status");
      xelement3.Add((object) new XAttribute((XName) "nickname", (object) this.Nickname));
      xelement3.Add((object) new XElement((XName) "profile_info", (object) new XElement((XName) "info", new object[6]
      {
        (object) new XAttribute((XName) "nickname", (object) this.Nickname),
        (object) new XAttribute((XName) "online_id", this.OnlineUser.JID != null ? (object) this.OnlineUser.JID : (object) ""),
        (object) new XAttribute((XName) "status", (object) this.OnlineUser.Status),
        (object) new XAttribute((XName) "rank", (object) this.OnlineUser.Player.Rank),
        (object) new XAttribute((XName) "user_id", (object) this.OnlineUser.Player.TicketId),
        (object) new XAttribute((XName) "profile_id", (object) this.OnlineUser.Player.UserID)
      })));
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
