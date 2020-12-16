// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.UpdateAchievements
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class UpdateAchievements : Stanza
  {
    private string Channel;

    public UpdateAchievements(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      foreach (XmlNode childNode1 in this.Query.ChildNodes)
      {
        XmlNode achiev = childNode1;
        Client client = ArrayList.OnlineUsers.Find((Predicate<Client>) (Attribute => Attribute.Player.UserID == long.Parse(achiev.Attributes["profile_id"].InnerText)));
        if (client == null)
        {
          Player player = new Player()
          {
            UserID = long.Parse(achiev.Attributes["profile_id"].InnerText)
          };
          if (player.Load(true).Result)
            client = new Client() { Player = player };
        }
        if (client != null)
        {
          foreach (XmlNode childNode2 in achiev.ChildNodes)
            client.Player.UpdateAchievement(childNode2);
          client.Player.Save();
        }
      }
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
      XElement xelement3 = new XElement((XName) "update_achievements");
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
