// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.GetPlayerStats
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class GetPlayerStats : Stanza
  {
    private string Channel;

    public GetPlayerStats(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      try
      {
        if (this.Type == "result")
          return;
        this.Process();
      }
      catch
      {
      }
    }

    public override void Process()
    {
      XDocument xDocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", this.Type == "get" ? (object) "result" : (object) "get"));
      xelement1.Add((object) new XAttribute((XName) "from", this.Packet != null ? (object) this.To : (object) "k01.warface"));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", this.Type == "get" ? (object) this.Id : (object) ("uid" + this.User.Player.Random.Next(9999, int.MaxValue).ToString("x8"))));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) "get_player_stats");
      if (this.User.Player.Stats.FirstChild.ChildNodes.Count > 0)
      {
        foreach (XmlNode childNode in this.User.Player.Stats.FirstChild.ChildNodes)
          xelement3.Add((object) XDocument.Parse(childNode.OuterXml).Root);
      }
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xDocument.Add((object) xelement1);
      this.Compress(ref xDocument);
      this.User.Send(xDocument.ToString(SaveOptions.DisableFormatting));
      if (this.Type != null)
        return;
      this.User.Player.Save();
    }
  }
}
