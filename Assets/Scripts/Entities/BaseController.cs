using UnityEngine;

// Base of all controllers
// DO NOT add this to an object directly
// Use a derived class instead, if you want a controller that does nothing use DumbController
public abstract class BaseController : MonoBehaviour {
    public abstract float Horizontal();
    public abstract float Vertical();
    public abstract bool Jump();
    public abstract bool ReleaseJump();
    public abstract bool Melee();
    public abstract bool Range();
    public abstract bool Boost();
    public abstract bool Movement();
    public virtual void OnDrawGizmosSelected() { } 
}