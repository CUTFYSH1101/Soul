using System.Collections.Generic;
using UnityEngine;

namespace Main.EventLib.Sub.BattleSystem
{
    public class GameLoop : MonoBehaviour
    {
        private AnyEnemyInView _anyEnemyInView;
        private AnyInView _anyInView;
        private Dictionary<Transform,Spoiler> _dictionary;
        public void Start()
        {
            var transform1 = transform;
            var range = new Vector2(0, 0);
            _anyInView = new AnyInView(transform1, range);
            _anyEnemyInView = new AnyEnemyInView(transform1, range, Team.Peace);
        }
    }
}