// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.RemoveFriend
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class RemoveFriend : Stanza
  {
    private string Target;

    public RemoveFriend(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      this.Target = this.Query.Attributes["target"].InnerText;
      Client User1 = ArrayList.OnlineUsers.Find((Predicate<Client>) (Attribute => Attribute.Player.Nickname == this.Target));
      if (User1 != null)
      {
        User.Player.RemoveFriend(User1.Player.UserID.ToString());
        User1.Player.RemoveFriend(User.Player.UserID.ToString());
        User1.Player.Save();
        new FriendList(User1, (XmlDocument) null).Process();
      }
      else
      {
        Player player = new Player()
        {
          Nickname = this.Target
        };
        if (!player.Load(true).Result)
        {
          this.Process();
          return;
        }
        User.Player.RemoveFriend(player.UserID.ToString());
        player.RemoveFriend(User.Player.UserID.ToString());
        player.Save();
      }
      new FriendList(User, (XmlDocument) null).Process();
      User.Player.Save();
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
      XElement xelement3 = new XElement((XName) "remove_friend");
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xDocument.Add((object) xelement1);
      this.Compress(ref xDocument);
      this.User.Send(xDocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
