using UnityEngine;

public static class TransformUtil
{
    public static Transform GetOrCreateTransform(string name)
    {
        GameObject obj = GameObject.Find(name);
        if (obj != null)
            return obj.transform;

        GameObject newObj = new GameObject(name);
        return newObj.transform;
    }
}
