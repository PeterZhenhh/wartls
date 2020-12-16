// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.StreamError
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;

namespace WARTLS.xmpp
{
  public class StreamError
  {
    public string To;
    public string Xmlns;
    public string XmlnsUrl;

    public StreamError(Client User, string Error)
    {
      User.Send("<stream:error><" + Error + " xmlns='urn:ietf:params:xml:ns:xmpp-streams'/></stream:error></stream:stream>");
      User.Socket.Dispose();
      if (User.SslStream == null)
        return;
      User.SslStream.Dispose();
    }
  }
}
