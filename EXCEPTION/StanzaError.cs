// Decompiled with JetBrains decompiler
// Type: OnyxServer.EXCEPTION.StanzaError
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System;
using System.Xml.Linq;

namespace WARTLS.EXCEPTION
{
  public class StanzaError : Exception
  {
    public StanzaError(Client User, Stanza Query, int CustomCode, int Code = 8)
    {
      XDocument xdocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) Query.To));
      xelement1.Add((object) new XAttribute((XName) "to", (object) User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) Query.Id));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) Query.Name);
      xelement2.Add((object) xelement3);
      XElement xelement4 = new XElement((XNamespace) "urn:ietf:params:xml:ns:xmpp-stanzas" + "error");
      xelement4.Add((object) new XAttribute((XName) "type", (object) "continue"));
      xelement4.Add((object) new XAttribute((XName) "code", (object) 8));
      xelement4.Add((object) new XAttribute((XName) "custom_code", (object) CustomCode));
      xelement1.Add((object) xelement2);
      xelement1.Add((object) xelement4);
      xdocument.Add((object) xelement1);
      User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
