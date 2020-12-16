// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.RoomJoinCodes
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

namespace WARTLS.xmpp.query
{
  public enum RoomJoinCodes
  {
    MISSION_NOT_AVAILABLE = 1,
    FULL = 4,
    GAME_STARTED = 7,
    BUILD_TYPE = 8,
    PRIVATE = 9,
    MISSION_NOT_PERMITTED = 11, // 0x0000000B
    NOT_IN_CLAN = 14, // 0x0000000E
    CLAN_NOT_PLAY = 15, // 0x0000000F
    ALL_CLASS_RESTRICTED = 16, // 0x00000010
    MSGBOX_BUILD_VERSION_MISMACH = 19, // 0x00000013
    NO_ACCESS_TOKENS = 20, // 0x00000014
  }
}
