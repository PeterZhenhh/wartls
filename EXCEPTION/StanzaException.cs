// Decompiled with JetBrains decompiler
// Type: OnyxServer.EXCEPTION.StanzaException
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.EXCEPTION
{
  public class StanzaException : Exception
  {
    public StanzaException(Client User, XmlDocument Query, int ErrorId)
    {
      User.Send(new XDocument(new object[1]
      {
        (object) new XElement((XName) "iq", new object[6]
        {
          (object) new XAttribute((XName) "from", (object) "onyx@server/StanzaException"),
          (object) new XAttribute((XName) "to", (object) User.JID),
          (object) new XAttribute((XName) "id", (object) Query["iq"].Attributes["id"].InnerText),
          (object) new XAttribute((XName) "type", (object) "result"),
          (object) new XElement((XNamespace) "urn:cryonline:k01" + "query", (object) new XElement((XName) Query["iq"]["query"].FirstChild.Name)),
          (object) new XElement((XName) "error", new object[5]
          {
            (object) new XAttribute((XName) "type", (object) "continue"),
            (object) new XAttribute((XName) "code", (object) "8"),
            (object) new XAttribute((XName) "custom_code", (object) ErrorId),
            (object) new XElement((XNamespace) "urn:ietf:params:xml:ns:xmpp-stanzas" + "public-server-error"),
            (object) new XElement((XNamespace) "urn:ietf:params:xml:ns:xmpp-stanzas" + "text", (object) "Custom query error")
          })
        })
      }).ToString(SaveOptions.DisableFormatting));
    }
  }
}
