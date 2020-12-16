// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.BroadcastSessionResults
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class BroadcastSessionResults : Stanza
  {
    public List<XElement> SessionResults;

    public BroadcastSessionResults(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
    }

    public BroadcastSessionResults(Client User, List<XElement> Results)
      : base(User, (XmlDocument) null)
    {
      this.SessionResults = Results;
    }

    public override void Process()
    {
      XDocument xDocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "get"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) "k01.warface"));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.User.Player.Random.Next(999999, int.MaxValue)));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) "broadcast_session_result");
      foreach (XElement sessionResult in this.SessionResults)
        xelement3.Add((object) this.SessionResults);
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xDocument.Add((object) xelement1);
      this.Compress(ref xDocument);
      this.User.Send(xDocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
