// Decompiled with JetBrains decompiler
// Type: OnyxServer.Core
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace WARTLS
{
  public class Core
  {
    public static X509Certificate2 Certificate;
    public static MessageFactory MessageFactory;
    public static GameResources GameResources;

    public Core()
    {
      Core.Certificate = new X509Certificate2("Cert/Server.pfx");
      Core.MessageFactory = new MessageFactory();
      Core.GameResources = new GameResources();
      new ArrayList();
      new GameResources();
      new SQL();
      ArrayList.Channels.Add(new Channel("pve_001", 1, "pve", 1, 95));
      ArrayList.Channels.Add(new Channel("pvp_skilled_1", 201, "pvp_skilled", 1, 95));
       new Gateway(5222);

        }
  }
}
