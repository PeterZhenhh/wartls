// Decompiled with JetBrains decompiler
// Type: OnyxServer.CLASSES.GAMEROOM.CORE.CustomParams
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using System.Xml.Linq;

namespace WARTLS.CLASSES.GAMEROOM.CORE
{
  public class CustomParams
  {
    public bool AutoTeamBalance = true;
    public bool DeadCanChat = true;
    public bool JoinInProcess = true;
    public byte MaxPlayers = 16;
    public byte RoundLimit = 11;
    public bool SoldierEnabled = true;
    public bool MedicEnabled = true;
    public bool EngineerEnabled = true;
    public bool SniperEnabled = true;
    public int Revision = 2;
    public bool FriendlyFire;
    public bool EmenyOutlines;
    public int InventorySlot;
    public bool LockedSpectatorCamera;
    public bool HighLatencyAutokick;
    public bool OvertimeMode;

    public byte ClassRestriction
    {
      get
      {
        byte num = 224;
        if (this.SoldierEnabled)
          ++num;
        if (this.MedicEnabled)
          num += (byte) 8;
        if (this.EngineerEnabled)
          num += (byte) 16;
        if (this.SniperEnabled)
          num += (byte) 4;
        return num;
      }
    }

    public XElement Serialize()
    {
      XElement xelement = new XElement((XName) "custom_params");
      xelement.Add((object) new XAttribute((XName) "friendly_fire", (object) (this.FriendlyFire ? 1 : 0)));
      xelement.Add((object) new XAttribute((XName) "enemy_outlines", (object) (this.EmenyOutlines ? 1 : 0)));
      xelement.Add((object) new XAttribute((XName) "auto_team_balance", (object) (this.AutoTeamBalance ? 1 : 0)));
      xelement.Add((object) new XAttribute((XName) "dead_can_chat", (object) (this.DeadCanChat ? 1 : 0)));
      xelement.Add((object) new XAttribute((XName) "join_in_the_process", (object) (this.JoinInProcess ? 1 : 0)));
      xelement.Add((object) new XAttribute((XName) "max_players", (object) this.MaxPlayers));
      xelement.Add((object) new XAttribute((XName) "round_limit", (object) this.RoundLimit));
      xelement.Add((object) new XAttribute((XName) "inventory_slot", (object) this.InventorySlot));
      xelement.Add((object) new XAttribute((XName) "locked_spectator_camera", (object) (this.LockedSpectatorCamera ? 1 : 0)));
      xelement.Add((object) new XAttribute((XName) "high_latency_autokick", (object) (this.HighLatencyAutokick ? 1 : 0)));
      xelement.Add((object) new XAttribute((XName) "overtime_mode", (object) (this.OvertimeMode ? 1 : 0)));
      xelement.Add((object) new XAttribute((XName) "class_restriction", (object) this.ClassRestriction));
      xelement.Add((object) new XAttribute((XName) "revision", (object) this.Revision));
      return xelement;
    }
  }
}
