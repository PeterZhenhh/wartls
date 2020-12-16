// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.GetAchievements
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class GetAchievements : Stanza
  {
    private string Channel;

    public GetAchievements(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      this.Process();
    }

    public override void Process()
    {
      Client client = ArrayList.OnlineUsers.Find((Predicate<Client>) (Attribute => Attribute.Player.UserID == long.Parse(this.Query.FirstChild.Attributes["profile_id"].InnerText)));
      if (client == null)
      {
        Player player = new Player()
        {
          UserID = long.Parse(this.Query.FirstChild.Attributes["profile_id"].InnerText)
        };
        if (!player.Load(false).Result)
          return;
        client = new Client() { Player = player };
      }
      XDocument xDocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) this.To));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) "get_achievements");
      XElement xelement4 = new XElement((XName) "achievement");
      xelement4.Add((object) new XAttribute((XName) "profile_id", (object) client.Player.UserID));
      if (client.Player.Achievements.FirstChild.ChildNodes.Count != 0)
      {
        foreach (XmlNode childNode in client.Player.Achievements.FirstChild.ChildNodes)
          xelement4.Add((object) XDocument.Parse(childNode.OuterXml).Root);
      }
      xelement3.Add((object) xelement4);
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xDocument.Add((object) xelement1);
      this.Compress(ref xDocument);
      this.User.Send(xDocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
