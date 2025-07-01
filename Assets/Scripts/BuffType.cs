using UnityEngine;

public enum BuffType
{
    Size,
    Speed,
    SpawnBalls,
    SlowMotion
}

public abstract class BuffBase
{
    protected GameObject target;
    protected float duration;
    protected float value;
    
    public BuffBase(GameObject target, float value, float duration)
    {
        this.target = target;
        this.value = value;
        this.duration = duration;
    }
    
    public abstract void Apply();
    public abstract void Remove();
}

public class SizeBuff : BuffBase
{
    private Vector3 originalScale;
    
    public SizeBuff(GameObject target, float value, float duration) : base(target, value, duration)
    {
        originalScale = target.transform.localScale;
    }
    
    public override void Apply()
    {
        target.transform.localScale = originalScale * value;
    }
    
    public override void Remove()
    {
        target.transform.localScale = originalScale;
    }
}

public class SpeedBuff : BuffBase
{
    public SpeedBuff(GameObject target, float value, float duration) : base(target, value, duration) { }
    
    public override void Apply()
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity *= value;
        }
    }

    public override void Remove()
    {
        // Since this prototype doesn't store original speed, we can't revert it
    }
}

public class SpawnBallsBuff : BuffBase
{
    public SpawnBallsBuff(GameObject target, float value, float duration) : base(target, value, duration) { }
    
    public override void Apply()
    {
        for (int i = 0; i < (int)value; i++)
        {
            GameObject newBall = Object.Instantiate(target);
            Vector3 spawnPos = target.transform.position + new Vector3(
                Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
            newBall.transform.position = spawnPos;
        }
    }
    
    public override void Remove()
    {
        // Keep the spawned balls; nothing to remove
    }
}

public class SlowMotionBuff : BuffBase
{
    private Rigidbody ballRb;
    private float originalDrag;
    private float originalAngularDrag;
    
    public SlowMotionBuff(GameObject target, float value, float duration) : base(target, value, duration)
    {
        ballRb = target.GetComponent<Rigidbody>();
        if (ballRb != null)
        {
            originalDrag = ballRb.linearDamping;
            originalAngularDrag = ballRb.angularDamping;
        }
    }
    
    public override void Apply()
    {
        if (ballRb != null)
        {
            // Increase drag to slow down the ball
            // TODO: Appears that other collisions affect this, so
            // the slow motion effect does not persist.
            ballRb.linearDamping = originalDrag + (1f - value) * 5f;
            ballRb.angularDamping = originalAngularDrag + (1f - value) * 5f;
        }
    }
    
    public override void Remove()
    {
        if (ballRb != null)
        {
            ballRb.linearDamping = originalDrag;
            ballRb.angularDamping = originalAngularDrag;
        }
    }
}
