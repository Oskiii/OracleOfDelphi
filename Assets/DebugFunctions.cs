using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFunctions : MonoBehaviour
{

    public WorldspaceText GetWorldSpaceText(Object caller)
    {
        GameObject obj = Resources.Load<GameObject>("DebugPanel");
        GameObject inst = Instantiate(obj);
        inst.transform.SetParent(transform);

        WorldspaceText wst = inst.GetComponent<WorldspaceText>();
        wst.SetOwner(caller);
        return wst;
    }

}
