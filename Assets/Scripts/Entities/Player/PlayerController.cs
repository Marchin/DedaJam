using UnityEngine;

public class PlayerController : BaseController {
    public override float Horizontal() {
        return Input.GetAxisRaw("Horizontal");
    }

    public override float Vertical() {
        return Input.GetAxisRaw("Vertical");
    }

    public override bool Jump() {
        return Input.GetButtonDown("Jump");
    }

    public override bool ReleaseJump() {
        return Input.GetButtonUp("Jump");
    }

    public override bool Melee() {
        return Input.GetButtonDown("Melee");
    }

    public override bool Range() {
        return Input.GetButtonDown("Range");
    }

    public override bool Boost() {
        return Input.GetButtonDown("Boost");
    }

    public override bool Movement() {
        return Input.GetButtonDown("Movement");
    }
}