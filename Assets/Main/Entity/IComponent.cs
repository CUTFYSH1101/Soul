using System;
using System.Collections.Generic;
using Main.Util;

namespace Main.Entity
{
    public interface IComponent
    {
        int Id { get; }
        void Update();
    }
}