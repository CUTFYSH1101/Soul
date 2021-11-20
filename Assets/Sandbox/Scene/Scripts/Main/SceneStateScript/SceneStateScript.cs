using System;
using Main.Entity;
using Test.Scene.Scripts.Game;
using Test.Scene.Scripts.Sub;

namespace Test.Scene.Scripts.Main.SceneStateScript
{
    public abstract class SceneStateScript : IComponent
    {
        protected GameCenter Center { get; private set; }

        public static SceneStateScript NewInstance(SceneStateScript newMap, GameCenter center)
        {
            newMap.Center = center;
            return newMap;
        }
        public static SceneStateScript NewInstance(EnumMapTag newMap, GameCenter center)
        {
            SceneStateScript _ = newMap switch
            {
                EnumMapTag.VictoryMap1 => new Victory(),
                EnumMapTag.MainMenu => new StartMenu(),
                EnumMapTag.Tutorial => new Tutorial(),
                _ => throw new ArgumentOutOfRangeException(nameof(newMap), newMap, null)
            };
            _.Center = center;
            return _;
        }

        public static SceneStateScript ChangeState(SceneStateScript oldSceneState, EnumMapTag newMap, GameCenter center)
        {
            oldSceneState?.Exit();
            var newScene = NewInstance(newMap, center);
            newScene?.Enter();
            return newScene;
        }
        public abstract void Enter();
        public abstract void Exit();
        public EnumComponentTag Tag => EnumComponentTag.SceneManagement;
        public virtual void Update(){}
    }
}