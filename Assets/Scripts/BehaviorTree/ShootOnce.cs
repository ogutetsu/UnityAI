using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;
using Pada1.BBCore.Framework;
using UnityEngine;

[Action("ShootOnce")]
[Help("Clone a bullet")]

public class ShootOnce : GOAction
{

    [InParam("shootPoint")]
    public Transform shootPoint;

    [InParam("bullet")]
    public GameObject bullet;

    [InParam("velocity", DefaultValue = 30.0f)]
    public float velocity;


    public override void OnStart()
    {
        if (shootPoint == null)
        {
            shootPoint = gameObject.transform.Find("shootPoint");
            if (shootPoint == null)
            {
                Debug.LogWarning("Shoot point not specified.");
            }
        }

        base.OnStart();
    }


    public override TaskStatus OnUpdate()
    {
        if (shootPoint == null)
        {
            return TaskStatus.FAILED;
        }

        GameObject newBullet = GameObject.Instantiate(bullet, shootPoint.position,
            shootPoint.rotation * bullet.transform.rotation) as GameObject;

        if (newBullet.GetComponent<Rigidbody>() == null)
        {
            newBullet.AddComponent<Rigidbody>();
        }

        newBullet.GetComponent<Rigidbody>().velocity = velocity * shootPoint.forward;
        
        return TaskStatus.COMPLETED;
    }


}



[Action("Shoot")]
[Help("bullet")]
public class Shoot : ShootOnce
{

    [InParam("delay", DefaultValue = 30)] public int delay;

    private int elapsed = 0;

    public override TaskStatus OnUpdate()
    {
        if (delay > 0)
        {
            ++elapsed;
            elapsed %= delay;
            if (elapsed != 0)
            {
                return TaskStatus.RUNNING;
            }
        }

        base.OnUpdate();
        return TaskStatus.RUNNING;
    }
}


[Action("SleepForever")]
public class SleepForever : BasePrimitiveAction
{
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.SUSPENDED;
    }
}


[Condition("IsNight")]
public class IsNightCondition : ConditionBase
{
    private DayNightCycle light;

    public override bool Check()
    {
        if (searchLight())
        {
            return light.isNight;
        }
        else
        {
            return false;
        }
    }

    public override TaskStatus MonitorCompleteWhenTrue()
    {
        if (Check())
        {
            return TaskStatus.COMPLETED;
        }
        else
        {
            if (light != null)
            {
                light.OnChanged += OnSunset;
            }

            return TaskStatus.SUSPENDED;
        }
    }

    public override TaskStatus MonitorFailWhenFalse()
    {
        if (!Check())
        {
            return TaskStatus.FAILED;
        }
        else
        {
            light.OnChanged += OnSunrise;
            return TaskStatus.SUSPENDED;
        }
    }

    public void OnSunset(object sender, System.EventArgs night)
    {
        light.OnChanged -= OnSunset;
        EndMonitorWithSuccess();
    }


    public void OnSunrise(object sender, System.EventArgs e)
    {
        light.OnChanged -= OnSunrise;
        EndMonitorWithFailure();
    }

    public override void OnAbort()
    {
        if (searchLight())
        {
            light.OnChanged -= OnSunrise;
            light.OnChanged -= OnSunset;
        }
        base.OnAbort();
    }

    private bool searchLight()
    {
        if (light != null)
        {
            return true;
        }

        GameObject lightGo = GameObject.FindGameObjectWithTag("MainLight");
        if (lightGo == null) return false;

        light = lightGo.GetComponent<DayNightCycle>();
        return light != null;

    }


}


