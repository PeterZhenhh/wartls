// Decompiled with JetBrains decompiler
// Type: OnyxServer.CLASSES.RandomExtensions
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using System;

namespace WARTLS.CLASSES
{
  public static class RandomExtensions
  {
    public static double NextDouble(this Random random, double minValue, double maxValue)
    {
      return random.NextDouble() * (maxValue - minValue) + minValue;
    }
  }
}
