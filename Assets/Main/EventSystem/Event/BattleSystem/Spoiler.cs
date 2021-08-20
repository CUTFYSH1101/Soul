using System.ComponentModel;
using JetBrains.Annotations;
using Main.Entity.Creature;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Attribute;
using Main.EventSystem.Event.UIEvent.Combo;
using Main.Game;
using Main.Game.Collision;
using UnityEngine;
using static Main.EventSystem.Event.BattleSystem.Team;

namespace Main.EventSystem.Event.BattleSystem
{
    public class Spoiler : MonoBehaviour, IMediator
    {
        [SerializeField, Description("是否可主動攻擊")]
        private bool enableAttack = true; // todo 區分可攻擊與否

        [SerializeField, Description("是否可受擊")] private bool enableHit = true; // todo 區分可受擊與否

        [SerializeField, Description("是否可被擊退")]
        private bool enableKnockback = true; // todo 區分受擊後可被擊退與否

        // 其他：是否會受到debuff影響（例如boss免疫某些負面狀態或是完全免疫負面狀態）
        // 修改為builder set
        private ComboUIEvent _comboUI;
        public AbstractCreature Creature { get; private set; }
        public Team Team { get; private set; }
        public bool IsKilled => Creature.IsKilled();

        public Spoiler Init(AbstractCreature self, Team team)
        {
            Creature = self;
            Team = team;
            /*if (team == Team.Player) 
                combo = new ComboEvent("UI/PanelCombo");*/
            return this;
        }

        public bool IsEnemy(Team target) =>
            target switch
            {
                Evil => true,
                Peace => false,
                _ => target != Team
            };

        public static void SetTargetHit([NotNull] AbstractCreature target, [NotNull] AbstractCreature attacker,
            [NotNull] SkillAttr attackerAttr)
        {
            /*
            if (target != null)
                if (self.CreatureAttr.MindState == EnumMindState.Attack)
                    target.Behavior.Hit(selfSkill);
        */
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Debug.Log(other.GetRoot());
            var target = CreatureList.FindCreature(other.GetRoot());
            if (target == null)
                return;
            SetTargetHit(target, Creature, CreatureList.FindSkillAttr(transform.GetRoot()));

            // Debug.Log(Team + " "+ transform.root+" ");
            // combo?.Invoke();

            /*
             * 1.
             * mono.FindAI
             * 透過資料庫查詢，輸入transform.root，取得self
             * 同樣透過資料庫查詢，輸入other.transform.root，取得target
             *
             * 2.攻擊對方
             */
        }

        public static GameObject CreateDamageBoxOnScene(AbstractCreature target, AbstractCreature attacker)
        {
            var go = new GameObject();
            go.transform.parent = attacker.Transform;
            go.transform.position = target.AbsolutePosition;
            go.layer = "Attack".GetLayer();
            go.AddComponent<CircleCollider2D>().isTrigger = true;
            Destroy(go, 0.001f);
            return go;
        }
    }
}