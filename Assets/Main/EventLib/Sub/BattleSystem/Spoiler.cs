using System;
using JetBrains.Annotations;
using Main.Blood;
using Main.Game;
using Main.Game.Collision;
using Main.Entity;
using Main.Entity.Creature;
using Main.EventLib.Sub.CreatureEvent.Skill.Attribute;
using UnityEngine;
using static Main.EventLib.Sub.BattleSystem.Team;
using Object = UnityEngine.Object;

namespace Main.EventLib.Sub.BattleSystem
{
    public class Spoiler : MonoBehaviour, IMediator, Entity.IComponent
    {
        // 是否會受到debuff影響（例如boss免疫某些負面狀態或是完全免疫負面狀態），由builder設定
        public Creature Creature { get; private set; }
        public Team Team { get; private set; }
        public bool IsKilled => Creature.IsKilled();

        public bool IsEnemy(Team target) =>
            target switch
            {
                Evil => true,
                Peace => false,
                _ => target != Team
            };

        public static Spoiler Instance(Creature self, Team selfTeam)
        {
            var newInstance = self.Transform.GetOrAddComponent<Spoiler>();
            newInstance.Creature = self;
            newInstance.Team = selfTeam;
            return newInstance;
        }

        public Action ONHit { get; private set; }

        public Spoiler InitEvent(Action onHit)
        {
            ONHit = onHit;
            return this;
        }

        private void OnTriggerEnter2D(Collider2D injuredCollider)
        {
            if (!IsEnemy(injuredCollider)) return;
            var usingSkill = CreatureSystem.FindInUsingSkillAttr(Creature.Transform);
            SetTargetHit(injuredCollider, usingSkill);
            // LogInfo(injuredCollider, "type: " + usingSkill.BloodType.ToString());
        }

        private static bool IsCreature(Component injuredCollider, out Creature injured)
        {
            var _ = injuredCollider.CompareLayer("Creature");
            injured = _ ? CreatureSystem.FindCreature(injuredCollider.transform.root) : null;
            return _;
        }

        private bool IsEnemy(Collider2D theInjuredCollider)
        {
            // 不要自己打自己
            if (theInjuredCollider.transform.root == Creature.Transform) return false;
            var theInjuredSpoiler = (Spoiler)CreatureSystem.FindComponent(theInjuredCollider.transform,
                EnumComponentTag.BattleSpoilerSystem);
            // 敵對才能傷害到對方
            if (theInjuredSpoiler == null || !theInjuredSpoiler.IsEnemy(Team)) return false;
            return true;
        }

        /// 耗效能盡量不要用，耗能是以上任一程式碼的10倍以上
        private void LogInfo(Component theInjuredCollider)
        {
            var msg = $"[Other's Information] name:{theInjuredCollider.transform.root.name}\n" +
                      $"[My Information] team: {Team}; name:{Creature.Transform.name}; skill: {Creature.CreatureAttr.CurrentSkill.ToString()}";
            Debug.Log(msg);
        }

        public void SetTargetHit(Component injuredCollider, SkillAttr usingSkill)
        {
            if (!IsCreature(injuredCollider, out var injured)) return;

            var blood = injured.FindComponent<BloodHandler>();
            if (blood == null) return;
            if (!(usingSkill.BloodType == BloodType.Direct && blood.DiscardBlood() ||
                  blood.DiscardBlood(usingSkill.BloodType))) return;
            if (blood.IsEmpty)
            {
                UserInterface.Killed(injured);
            }
            else
            {
                SetTargetHit(injured, usingSkill);
            }
        }
        // 注意區別攻擊方與被攻擊方
        private void SetTargetHit(Creature injured, SkillAttr usingSkill)
        {
            if (injured == null) return;
            if (usingSkill == null) return;
            UserInterface.Hit(injured, usingSkill);
            ONHit?.Invoke();
        }
        public static GameObject CreateDamageBoxOnScene(Creature target, Creature attacker)
        {
            var go = new GameObject();
            go.transform.parent = attacker.Transform;
            go.transform.position = target.AbsolutePosition;
            go.layer = "Attack".GetLayer();
            go.AddComponent<CircleCollider2D>().isTrigger = true;
            Object.Destroy(go, 0.001f);
            return go;
        }

        public EnumComponentTag Tag => EnumComponentTag.BattleSpoilerSystem;

        public void Update()
        {
            // _triggerEvent.Update();
            // _triggerEvent.DuringStayOthers?.Get(s=>s.name).ArrayToString().LogLine();
        }
    }
    /*public class Spoiler : IMediator, IComponent
    {
        // 是否會受到debuff影響（例如boss免疫某些負面狀態或是完全免疫負面狀態）
        // 由builder設定
        [NotNull] private readonly SpoilerCollisionSystem _triggerEvent;

        // [NotNull] private readonly ComboUIEvent _combo;
        public Creature Creature { get; }
        public Team Team { get; }
        public bool IsKilled => Creature.IsKilled();

        public bool IsEnemy(Team target) =>
            target switch
            {
                Evil => true,
                Peace => false,
                _ => target != Team
            };

        public Spoiler(Creature self, Team selfTeam)
        {
            Creature = self;
            Team = selfTeam;

            _triggerEvent = new SpoilerCollisionSystem(self.Transform, OnTriggerEnter2D, null);
            // _combo = UserInterface.CreateComboUI();
        }

        public Action ONHit { get; private set; }

        public Spoiler InitVFXEvent(Action onHit)
        {
            ONHit = onHit;
            return this;
        }

        private void OnTriggerEnter2D(Collider2D skillObj)
        {
            /*
            if (!skillObj.CompareLayer("Attack")) return;

            if (!skillObj.transform.root.CompareLayer("Creature")) return;

            #1#
            var attacker = BattleInterface
                .FindCreature(skillObj.GetRoot()); // 無論撞到什麼，角色物件為根目錄，所以先抓根目錄

            /*
            Debug.Log($"[Other Information] name:{skillObj.GetRoot().name}\n" +
                      $"[My Information] team: {Team}; name:{Creature.Transform.name}");
                      #1#

            // 不要自己打自己
            if (attacker == Creature) return;

            // todo 根據血條決定是否能攻擊到對方

            if (attacker != null)
            {
                SetTargetHit(Creature,
                    attacker, BattleInterface // 我是攻擊方
                        .FindInUsingSkillAttr(attacker.Transform)); // 尋找我自己正在使用的技能資料，所有攻擊造成的影響都在這裡(0.0)!
                // _combo.Invoke();
                ONHit?.Invoke();
            }
            /*
            else
            {
                attacker = skillObj.transform;
                SetTargetHit(Creature, CreatureList // 我是攻擊方
                        .FindInUsingSkillAttr(attacker.Transform)); // 尋找我自己正在使用的技能資料，所有攻擊造成的影響都在這裡(0.0)!
            }
            #1#


            Debug.Log($"[Other Information] name:{skillObj.GetRoot().name}\n" +
                      $"[My Information] team: {Team}; name:{Creature.Transform.name}");

            /*
             * 1.
             * CreatureList.FindCreature
             * 透過資料庫查詢，輸入transform.root 或 creature.transform，取得self
             * 同樣透過資料庫查詢，輸入skillObj.transform.root，取得target
             * 
             * 2.攻擊對方，觸發HitEvent
             #1#
        }

        private static void SetTargetHit([CanBeNull] Creature target, [CanBeNull] SkillAttr attackerAttr)
        {
            if (attackerAttr == null) return;
            if (target == null) return;

            UserInterface.Hit(target, attackerAttr);
        }

        private static void SetTargetHit([CanBeNull] Creature target,
            [NotNull] Creature attacker, [CanBeNull] SkillAttr attackerAttr)
        {
            /*
            // 確認攻擊方的狀態，必須處於正在攻擊的狀態，才會攻擊到對方，用來區分攻擊方和被攻擊方
            if (attacker.CreatureAttr.MindState != EnumMindState.Attacking) return;
            #1#
            if (attackerAttr == null) return;
            if (target == null) return;

            UserInterface.Hit(target, attackerAttr);
        }

        public static GameObject CreateDamageBoxOnScene(Creature target, Creature attacker)
        {
            var go = new GameObject();
            go.transform.parent = attacker.Transform;
            go.transform.position = target.AbsolutePosition;
            go.layer = "Attack".GetLayer();
            go.AddComponent<CircleCollider2D>().isTrigger = true;
            Object.Destroy(go, 0.001f);
            return go;
        }

        public EnumComponentTag Tag => EnumComponentTag.BattleSpoilerSystem;

        public void Update()
        {
            _triggerEvent.Update();
            // _triggerEvent.DuringStayOthers?.Get(s=>s.name).ArrayToString().LogLine();
        }
    }*/
}