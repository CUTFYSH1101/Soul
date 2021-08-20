using System.Collections.Generic;
using System.Linq;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Common;
using Main.Util;

namespace Main.EventSystem.Event.CreatureEventSystem.Skill.Attribute
{
    public class SkillAttrList
    {
        private readonly Dictionary<EnumSkillTag, SkillAttr> _list = new Dictionary<EnumSkillTag, SkillAttr>();
        public void Remove(EnumSkillTag tag) => _list.Remove(tag);
        public void Append(SkillAttr newAttr)
        {
            if (!_list.ContainsKey(newAttr.SkillTag))
                _list.Add(newAttr.SkillTag, newAttr);
            else
                "資料庫有同名資料！".LogLine();
        }
        public SkillAttr Find(EnumSkillTag tag) => 
            _list.ContainsKey(tag) ? _list[tag] : null;
        
        public override string ToString() =>
            $"--【{GetType().Name}】--\n" +
            $"{_list?.Select(pp => $"名稱: {pp.Key}\n內容:\n{pp.Value}").ToArray().ArrayToString('\n', false)}";
    }
}