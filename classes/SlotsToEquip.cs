// Decompiled with JetBrains decompiler
// Type: OnyxServer.CLASSES.SlotsToEquip
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using System;

namespace WARTLS.CLASSES
{
  public class SlotsToEquip
  {
    private static int SlotToEquipped(int slot, int[] classes_slot)
    {
      if (slot == classes_slot[0])
        return 1;
      if (slot == classes_slot[1])
        return 8;
      if (slot == classes_slot[2])
        return 16;
      if (slot == classes_slot[3])
        return 4;
      if (slot == classes_slot[0] + classes_slot[1])
        return 9;
      if (slot == classes_slot[0] + classes_slot[1] + classes_slot[2])
        return 25;
      if (slot == classes_slot[0] + classes_slot[1] + classes_slot[2] + classes_slot[3])
        return 29;
      if (slot == classes_slot[1] + classes_slot[2])
        return 24;
      if (slot == classes_slot[1] + classes_slot[2] + classes_slot[3])
        return 28;
      if (slot == classes_slot[2] + classes_slot[3])
        return 20;
      if (slot == classes_slot[0] + classes_slot[3])
        return 5;
      if (slot == classes_slot[0] + classes_slot[2])
        return 17;
      if (slot == classes_slot[1] + classes_slot[3])
        return 12;
      if (slot == classes_slot[0] + classes_slot[1] + classes_slot[3])
        return 13;
      return slot == classes_slot[0] + classes_slot[2] + classes_slot[3] ? 21 : 0;
    }

    public static int GetEquipped(int slot, string item_name)
    {
      int num = 0;
      if (slot == 0)
        return num;
      if (num == 0)
        num = SlotsToEquip.SlotToEquipped(slot, new int[4]
        {
          1,
          32768,
          1048576,
          1024
        });
      if (num == 0)
        num = SlotsToEquip.SlotToEquipped(slot, new int[4]
        {
          26,
          851968,
          27262976,
          26624
        });
      if (num == 0)
        num = SlotsToEquip.SlotToEquipped(slot, new int[4]
        {
          2,
          65536,
          2097152,
          0
        });
      if (num == 0)
        num = SlotsToEquip.SlotToEquipped(slot, new int[4]
        {
          3,
          98304,
          3145728,
          3072
        });
      if (num == 0)
        num = SlotsToEquip.SlotToEquipped(slot, new int[4]
        {
          27,
          884736,
          28311552,
          27648
        });
      if (num == 0)
        num = SlotsToEquip.SlotToEquipped(slot, new int[4]
        {
          4,
          131072,
          4194304,
          4096
        });
      if (num == 0)
        num = SlotsToEquip.SlotToEquipped(slot, new int[4]
        {
          28,
          917504,
          29360128,
          28672
        });
      if (num == 0)
        num = SlotsToEquip.SlotToEquipped(slot, new int[4]
        {
          5,
          163840,
          5242880,
          5120
        });
      if (num == 0)
        num = SlotsToEquip.SlotToEquipped(slot, new int[4]
        {
          22,
          720896,
          23068672,
          22528
        });
      if (num == 0)
        num = SlotsToEquip.SlotToEquipped(slot, new int[4]
        {
          7,
          229376,
          7340032,
          7168
        });
      if (num == 0)
        num = SlotsToEquip.SlotToEquipped(slot, new int[4]
        {
          12,
          393216,
          12582912,
          12288
        });
      if (num == 0)
        num = SlotsToEquip.SlotToEquipped(slot, new int[4]
        {
          16,
          524288,
          16777216,
          16384
        });
      if (num == 0)
        num = SlotsToEquip.SlotToEquipped(slot, new int[4]
        {
          17,
          557056,
          17825792,
          17408
        });
      if (num == 0)
        num = SlotsToEquip.SlotToEquipped(slot, new int[4]
        {
          23,
          753664,
          24117248,
          23552
        });
      if (num != 0)
        return num;
            Console.WriteLine("ERR ", string.Format("СЛОТ {0} НА {1} НЕ НАЙДЕН", slot, item_name));
      return 0;
    }
  }
}
