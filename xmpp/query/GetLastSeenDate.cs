// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.GetLastSeenDate
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class GetLastSeenDate : Stanza
  {
    private long LastSeen = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    private string ProfileId;

    public GetLastSeenDate(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      this.ProfileId = this.Query.Attributes["profile_id"].InnerText;
      long num = long.Parse(this.ProfileId);
      Player player1;
      if (num > 0L)
      {
        player1 = new Player() { UserID = num };
      }
      else
      {
        player1 = new Player();
        player1.Nickname = this.ProfileId;
      }
      Player player2 = player1;
      if (!player2.Load(false).Result)
      {
        this.Process();
      }
      else
      {
        this.LastSeen = player2.LastSeen;
        this.Process();
      }
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
      XElement xelement3 = new XElement((XName) "get_last_seen_date");
      xelement3.Add((object) new XAttribute((XName) "profile_id", (object) this.ProfileId));
      xelement3.Add((object) new XAttribute((XName) "last_seen", (object) this.LastSeen));
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
