﻿using UnityEngine;

namespace Event
{
    public interface IHitable
    {
        void Hit(Vector2 direction, float force,
            Transform vfx = null, Vector2 position = default);
    }
}