// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.SyncNotification
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class SyncNotification : Stanza
  {
    private string Channel;
    private XElement FastNotify;

    public SyncNotification(Client User, XmlDocument Packet = null)
      : base(User, Packet)
    {
      this.Process();
    }

    public SyncNotification(Client User, XElement FastNotify)
      : base(User, (XmlDocument) null)
    {
      this.FastNotify = FastNotify;
    }

    public override void Process()
    {
      if (this.Type == "result")
        return;
      XDocument xDocument = new XDocument();
      XElement xelement1 = new XElement((XName) "iq");
      xelement1.Add((object) new XAttribute((XName) "type", this.Type == "get" ? (object) "result" : (object) "get"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) "masterserver@warface/onyx"));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", this.Type == "get" ? (object) this.Id : (object) ("uid" + this.User.Player.Random.Next(9999, int.MaxValue).ToString("x8"))));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) "sync_notifications");
      if (this.FastNotify == null)
      {
        if (this.User.Player.Notifications.FirstChild.ChildNodes.Count > 0)
        {
          foreach (XmlNode childNode in this.User.Player.Notifications.FirstChild.ChildNodes)
            xelement3.Add((object) XDocument.Parse(childNode.OuterXml).Root);
        }
      }
      else
        xelement3.Add((object) this.FastNotify);
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xDocument.Add((object) xelement1);
      xelement1.Attributes().Where<XAttribute>((Func<XAttribute, bool>) (e => e.IsNamespaceDeclaration)).Remove();
      this.Compress(ref xDocument);
      this.User.Send(xDocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
