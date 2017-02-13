using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RappelzSniffer.Network
{
    public enum ArObject__ObjectType : byte
    {
        STATIC_OBJECT = 0x0,
        MOVABLE_OBJECT = 0x1,
        CLIENT_OBJECT = 0x2,
    }

    public enum TS_SC_ENTER__OBJ_TYPE : byte
    {
        EOT_Player,
        EOT_NPC,
        EOT_Item,
        EOT_Monster,
        EOT_Summon,
        EOT_Skill,
        EOT_FieldProp,
        EOT_Pet
    }
    public enum ChatType : byte
    {
        Normal = 0x0,
        Yell = 0x1,
        Adv = 0x2,
        Whisper = 0x3,
        Global = 0x4,
        Emotion = 0x5,
        GM = 0x6,
        GMWhisper = 0x7,
        Party = 0xA,
        Guild = 0xB,
        AttackTeam = 0xC,
        Notice = 0x14,
        Exp = 0x1E,
        Damage = 0x1F,
        Item = 0x20,
        Battle = 0x21,
        Summon = 0x22,
        Etc = 0x23,
        NPC = 0x28,
        Debug = 0x32,
        PartySystem = 0x64,
        GuildSystem = 0x6E,
        QuestSystem = 0x78,
        RaidSystem = 0x82,
        FriendSystem = 0x8C,
        AllianceSystem = 0x96,
        HuntaholicSystem = 0xA0
    }
}
