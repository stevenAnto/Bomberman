using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AnimatedSpritRendered start;
    public AnimatedSpritRendered middle;
    public AnimatedSpritRendered end;

    public void SetActiveRenderer(AnimatedSpritRendered renderer)
    {
<<<<<<< HEAD:Assets/Scipts/Explosion.cs
        start.enabled = renderer == start;
        middle.enabled = renderer == middle;
        end.enabled = renderer == end;
=======
        start.enabled  = renderer == start;
        middle.enabled = renderer == middle;
        end.enabled    = renderer == end;
>>>>>>> fcc2fa7a6232512468d941a4fe347c934edaebe3:Assets/Scripts/Explosion.cs
    }

    public void SetDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void DestroyAfter(float seconds)
    {
        Destroy(gameObject, seconds);
    }
}
