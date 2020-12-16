// Decompiled with JetBrains decompiler
// Type: OnyxServer.CLASSES.InvitationTicket
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

namespace WARTLS.CLASSES
{
  public class InvitationTicket
  {
    public byte Result = byte.MaxValue;
    public string ID;
    public Client Sender;
    public Client Receiver;
    public string GroupId;
    public bool IsFollow;

    public InvitationTicket(Client Sender, Client Receiver)
    {
      this.ID = Sender.Player.Random.Next(1, int.MaxValue).ToString();
      this.Sender = Sender;
      this.Receiver = Receiver;
    }
  }
}
