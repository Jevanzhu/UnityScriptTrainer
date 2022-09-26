﻿using Config;
using GameData.Common;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.GameDataBridge;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityGameUI;

namespace ScriptTrainer
{
    public static class Scripts
    {
        #region[全局参数]
        public static int playerId
        {
            get
            {
                return SingletonObject.getInstance<BasicGameData>().TaiwuCharId;
            }
        }
        public static int CurCharacterId
        {
            get
            {
                return SingletonObject.getInstance<UI_CharacterMenuSubPageBase>().CharacterMenu.CurCharacterId;
            }
        }
        #endregion

        #region[添加资源]
        public static void AddPlayerMoney()
        {
            UIWindows.SpawnInputDialog("您想要添加多少现金？", "添加", "1000", (string count) =>
            {
                GMFunc.GetAdvancedResource(count.ConvertToIntDef(1000), true, false, false);
            });
        }

        public static void AddPlayerAuthority()
        {
            UIWindows.SpawnInputDialog("您想要添加多少威望？", "添加", "1000", (string count) =>
            {
                GMFunc.GetAdvancedResource(count.ConvertToIntDef(1000), false, true, false);
            });
        }

        public static void AddPlayerGoldIron()
        {
            UIWindows.SpawnInputDialog("您想要添加多少金铁？", "添加", "1000", (string count) =>
            {
                AddResource(2, count);
            });
        }

        public static void AddPlayerJadeStone()
        {
            UIWindows.SpawnInputDialog("您想要添加多少玉石？", "添加", "1000", (string count) =>
            {
                AddResource(3, count);
            });
        }
        public static void AddPlayerCloth()
        {
            UIWindows.SpawnInputDialog("您想要添加多少织物？", "添加", "1000", (string count) =>
            {
                AddResource(4, count);
            });           
        }
        public static void AddPlayerMedicine()
        {
            UIWindows.SpawnInputDialog("您想要添加多少药材？", "添加", "1000", (string count) =>
            {
                AddResource(5, count);
            });
        }
        public static void AddPlayerWood()
        {
            UIWindows.SpawnInputDialog("您想要添加多少木材？", "添加", "1000", (string count) =>
            {
                AddResource(1, count);
            });
        }

        public static void AddPlayerFood()
        {
            UIWindows.SpawnInputDialog("您想要添加多少食材？", "添加", "1000", (string count) =>
            {
                AddResource(0, count);
            });
        }


        private static void  AddResource(sbyte type, string count)
        {

            GameDataBridge.AddMethodCall<sbyte, int>(-1, 5, 5, type, count.ConvertToIntDef(1000));
        }
        #endregion

        #region[玩家功能]

        public static int GetPlayerCharId()
        {
            return SingletonObject.getInstance<BasicGameData>().TaiwuCharId;
        }

        /// <summary>
        /// 设置伤势
        /// </summary>
        /// <param name="isInnerInjury">是否是内伤</param>
        /// <param name="bodyPartType">身体部位值 (0-胸背；1-腰腹；2-头颅；3-左臂；4-右臂；5-左腿；6-右腿)</param>
        /// <param name="delta">伤势值 最大6 负数为降低</param>
        public static void ChangeInjury(bool isInnerInjury, sbyte bodyPartType, sbyte delta)
        {
            int charId = GetPlayerCharId();
            GameDataBridge.AddMethodCall<int, bool, sbyte, sbyte>(-1, 4, 72, charId, isInnerInjury, bodyPartType, delta);
        }
        
        // 设置中毒
        public static void ChangePoisoned(sbyte poisonType, int changeValue)
        {
            int charId = GetPlayerCharId();
            GameDataBridge.AddMethodCall<int, sbyte, int>(-1, 4, 73, charId, poisonType, changeValue);
        }

        // 改变地区恩义?
        public static void ChangeSpiritualDebt(int areaId, int spiritualDebt)
        {
            GMFunc.ChangeSpiritualDebt(areaId, spiritualDebt);

            //GameDataBridge.AddMethodCall<short, short>(-1, 2, 4, (short)areaId, (short)spiritualDebt);
        }

        // 修改玩家年龄
        public static void ChangeAge()
        {
            UIWindows.SpawnInputDialog("您想将自己修改为多少岁？", "设置", "18", (string count) =>
            {
                int charId = GetPlayerCharId();
                GMFunc.EditActualAge(charId, count.ConvertToIntDef(18));
            });
        }
        
        public static void ChangeHp()
        {
            UIWindows.SpawnInputDialog("您想将血量设置为多少？", "设置", "200", (string count) =>
            {
                int charId = GetPlayerCharId();
                short value = (short) count.ConvertToIntDef(200);

                GameDataBridge.AddDataModification<short>(4, 0, (ulong)charId, 19U, (short)value);
                GameDataBridge.AddDataModification<short>(4, 0, (ulong)charId, 20U, (short)value);
            });
        }

        // 修改主要属性
        public static void ChangeMainAttributes(short[] attributes)
        {
            int charId = GetPlayerCharId();

            // 修改主要属性
            GameDataBridge.AddDataModification<MainAttributes>(4, 0, (ulong)charId, 18U, new MainAttributes(attributes));
            GameDataBridge.AddDataModification<MainAttributes>(4, 0, (ulong)charId, 43U, new MainAttributes(attributes));
        }

        // 编辑基础道德
        public static void ChangeBaseMorality()
        {
            UIWindows.SpawnInputDialog("您想修改道德为多少？", "设置", "18", (string count) =>
            {
                int charId = GetPlayerCharId();
                GMFunc.EditBaseMorality(charId, count.ConvertToIntDef(18));
            });
        }

        #endregion


        #region[获取资源]

        public static void GetItem(sbyte itemType, int itemId, int count)
        {
            int charId = GetPlayerCharId();

            GameDataBridge.AddMethodCall<int, sbyte, short, int>(-1, 4, 17, charId, itemType, (short)itemId, count);
        }

        #endregion




        public static void Test()
        {

            GMFunc.CricketForceWin();

            // 获取名誉相关事件
            //for (int i = 0; i < FameAction.Instance.Count; i++)
            //{
            //    Debug.Log($"{FameAction.Instance[i].Name}, {i}");
            //}

            // 事件参与者
            //for (int i = 0; i < EventActors.Instance.Count; i++)
            //{
            //    Debug.Log($"{EventActors.Instance[i].Name}, {i}");
            //}

            // CharacterPropertyReferenced
            //for (int i = 0; i < Character.Instance.Count; i++)
            //{
            //    Debug.Log($"{Character.Instance[i].GivenName}, {Character.Instance[i].TemplateId}");
            //}

            //foreach (var item in Character.Instance)
            //{
            //    Debug.Log($"{item.Surname} {item.GivenName}, {item.TemplateId}");
            //}

            //foreach (var item in Config.Armor.Instance)
            //{
            //    Debug.Log($"{item.Name}, {item.TemplateId}");
            //}

            //foreach (var item in Config.MapArea.Instance)
            //{
            //    Debug.Log($"{item.Name} = {item.TemplateId}");
            //}


            //int a =   Config.CharacterFeature.GetCharacterPropertyBonus(GetPlayerCharId(), ECharacterPropertyReferencedType.AttackSpeed);

            //Debug.Log(a.ToString());

        }
    }
}
