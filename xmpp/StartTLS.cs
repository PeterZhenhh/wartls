// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.StartTLS
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace WARTLS.xmpp
{
  public class StartTLS
  {
    private readonly XNamespace NameSpace = (XNamespace) "urn:ietf:params:xml:ns:xmpp-tls";

    public StartTLS(Client User)
    {
      try
      {
        XDocument xdocument = new XDocument(new object[1]
        {
          (object) new XElement(this.NameSpace + "proceed")
        });
        User.Send(xdocument.ToString());
        User.SslStream = new SslStream((Stream) new NetworkStream(User.Socket, true), true, ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback) ((_003Cp0_003E, _003Cp1_003E, _003Cp2_003E, _003Cp3_003E) => true));
        User.SslStream.AuthenticateAsServer((X509Certificate) Core.Certificate, true, SslProtocols.Tls, false);
      }
      catch
      {
      }
    }
  }
}
