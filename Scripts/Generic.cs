using UnityEngine;

static public class Generic
{
    static public void LimitYVelocity(float limit, Rigidbody2D rb)
    {

        int gravityMultiplier = (int)(Mathf.Abs(rb.gravityScale)/rb.velocity.y);
        if(rb.velocity.y * -gravityMultiplier > limit)
            rb.velocity = Vector2.up * -limit * gravityMultiplier;
    }

    static public void createGameMode(Rigidbody2D rb, Movement host, bool onGroundRequired, float initialVelocity, 
    float gravityScale, bool canHold= false, bool flipOnClick = false, float rotationMod = 0, float yVelocityLimit = Mathf.Infinity)
    {
        if(!Input.GetMouseButton(0) || canHold && host.OnGround())
            host.clickProcessed = false;

        rb.gravityScale = gravityScale * host.Gravity;
        LimitYVelocity(yVelocityLimit, rb);

        if(Input.GetMouseButton(0))
        {
            if(host.OnGround() && !host.clickProcessed || !onGroundRequired && !host.clickProcessed)
            {   
                host.clickProcessed = true;
                rb.velocity = Vector2.up *initialVelocity *host.Gravity;
                host.Gravity *= flipOnClick ? -1 : 1;
            }
        }
        
        if(host.OnGround() || !onGroundRequired)
            host.Sprite.rotation = Quaternion.Euler(0,0, Mathf.Round(host.Sprite.rotation.z / 90) * 90);
        else
            host.Sprite.Rotate(Vector3.back, rotationMod * Time.deltaTime * host.Gravity);
    }
}
