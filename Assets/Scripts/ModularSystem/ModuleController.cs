using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleController : MonoBehaviour
{
    [SerializeField]
    private Dictionary<Type, IModuleBase> _modules = new Dictionary<Type, IModuleBase>();
    
    void Update()
    {
        foreach (var module in _modules.Values)
        {
            module.Tick();
        }
    }

    void FixedUpdate()
    {
        foreach (var module in _modules.Values)
        {
            module.FixedTick();
        }
    }

    void LateUpdate()
    {
        foreach (var module in _modules.Values)
        {
            module.LateTick();
        }
    }

    void OnDestroy()
    {
        foreach (var module in _modules.Values)
        {
            module.OnDestroy();
        }
    }
    
    protected void AddModule(IModuleBase module)
    {
        var type = module.GetType();
        if (_modules.TryAdd(type, module))
        {
            module.SetController(this);
            module.Initialize();
        }
        else
        {
            Debug.LogWarning($"Module of type {type} already exists in the controller.");
        }
    }
    
    public T GetModule<T>() where T : class, IModuleBase
    {
        if (_modules.TryGetValue(typeof(T), out IModuleBase module))
        {
            return module as T;
        }

        return null;
    }
    
    public void RemoveModule<T>() where T : IModuleBase
    {
        Type type = typeof(T);
        if (_modules.ContainsKey(type))
        {
            _modules[type].OnDestroy();
            _modules.Remove(type);
        }
    }
    
    protected void ActivateModules()
    {
        foreach (var module in _modules.Values)
        {
            module.Activate();
        }
    }
    
    public void DisableModules()
    {
        foreach (var module in _modules.Values)
        {
            module.Disable();
        }
    }

}