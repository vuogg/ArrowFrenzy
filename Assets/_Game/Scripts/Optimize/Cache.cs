using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Cache
{
    private static Dictionary<Collider, Buff> buffs = new();
    private static Dictionary<Collider, OnHit> onHits = new();
    private static Dictionary<GameObject, Level> levels = new();
    //private static Dictionary<GameObject, Arrow> arrows = new();
    private static Dictionary<GameObject, Target> targets = new();
    //private static Dictionary<GameObject, Dictionary<System.Type, Component>> cachedComponents = new();

    private static Dictionary<float, WaitForSeconds > wfs = new();

    public static WaitForSeconds GetWFS(float time)
    {
        if (!wfs.ContainsKey(time))
        {
            wfs.Add(time, new WaitForSeconds(time));
        }
        return wfs[time];
    }

    private static Dictionary<float, WaitForSecondsRealtime> wfsRT = new();

    public static WaitForSecondsRealtime GetWFSRT(float time)
    {
        if (!wfsRT.ContainsKey(time))
        {
            wfsRT.Add(time, new WaitForSecondsRealtime(time));
        }
        return wfsRT[time];
    }

    public static Buff GetBuff(Collider collider)
    {
        if (!buffs.ContainsKey(collider))
        {
            buffs.Add(collider, collider.GetComponent<Buff>());
        }
        return buffs[collider];
    }

    public static OnHit GetOnHit(Collider collider)
    {
        if (!onHits.ContainsKey(collider))
        {
            onHits.Add(collider, collider.GetComponent<OnHit>());
        }
        return onHits[collider];
    }

    public static Target GetTarget(GameObject obj)
    {
        if (!targets.ContainsKey(obj))
        {
            targets.Add(obj, obj.GetComponent<Target>());
        }
        return targets[obj];
    }
    //public static Level GetLevel(GameObject levelObject)
    //{
    //    if (!levels.ContainsKey(levelObject))
    //    {
    //        levels.Add(levelObject, levelObject.GetComponent<Level>());
    //    }
    //    return levels[levelObject];
    //}

    //public static Arrow GetArrow(GameObject obj)
    //{
    //    if (!arrows.ContainsKey(obj))
    //    {
    //        arrows.Add(obj, obj.GetComponent<Arrow>());
    //    }
    //    return arrows[obj];
    //}

    //public static T GetCachedComponent<T>(GameObject gameObject) where T : Component
    //{
    //    if (!cachedComponents.ContainsKey(gameObject))
    //    {
    //        cachedComponents[gameObject] = new Dictionary<System.Type, Component>();
    //    }

    //    var type = typeof(T);

    //    if (!cachedComponents[gameObject].ContainsKey(type))
    //    {
    //        T component = gameObject.GetComponent<T>();
    //        cachedComponents[gameObject][type] = component;
    //    }

    //    return (T)cachedComponents[gameObject][type];
    //}

    public static void ClearCache(Collider collider)
    {
        buffs.Remove(collider);
        onHits.Remove(collider);
    }

    public static void ClearCache(GameObject gameObject)
    {
        levels.Remove(gameObject);
    }
}
