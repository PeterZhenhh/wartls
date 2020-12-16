// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.StreamStream
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System.Text.RegularExpressions;

namespace WARTLS.xmpp
{
  public class StreamStream
  {
    public string To;
    public string Xmlns;
    public string XmlnsUrl;

    public StreamStream(Client User, string Packet)
    {
      MatchCollection matchCollection = new Regex("(?:to=')([\\s\\S]+?)'|xmlns='([\\s\\S]+?)'|xmlns:stream='([\\s\\S]+?)'").Matches(Packet);
      this.To = matchCollection[0].Value;
      this.Xmlns = matchCollection[1].Value;
      this.XmlnsUrl = matchCollection[1].Value;
      User.Send("<stream:features>" + (User.SslStream != null || User.Authorized ? "" : "<starttls xmlns='urn:ietf:params:xml:ns:xmpp-tls'/>") + (!User.Authorized ? "<mechanisms xmlns='urn:ietf:params:xml:ns:xmpp-sasl'><mechanism>WARFACE</mechanism></mechanisms>" : "<bind xmlns='urn:ietf:params:xml:ns:xmpp-bind' /><session xmlns='urn:ietf:params:xml:ns:xmpp-session' />") + "</stream:features>");
    }
  }
}
