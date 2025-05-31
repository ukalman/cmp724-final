using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModuleBase : IModuleBase
{
    public string Name { get; private set; }
    public ModuleState State { get; private set; }
    public bool IsEnabled { get; set; } = true; // Member Initializer
    public ModuleController Controller { get; private set; }

    public virtual void Initialize()
    {
        // Default initialization logic
        //Disable();
    }
    
    public virtual bool Tick()
    {
        if (!IsEnabled || State != ModuleState.Active)
        {
            return false; 
        }
        return true;
    }

    public virtual bool FixedTick()
    {
        if (!IsEnabled || State != ModuleState.Active)
        {
            return false; 
        }
        return true; 
    }

    public virtual bool LateTick()
    {
        if (!IsEnabled || State != ModuleState.Active)
        {
            return false;
        }
        return true;
    }
    
   

    public virtual void OnDestroy()
    {
        // Clean up logic when module is destroyed
    }

    public void SetController(ModuleController controller)
    {
        Controller = controller;
    }

    public virtual void Activate()
    {
        State = ModuleState.Active;
    }

    public virtual void Disable()
    {
        State = ModuleState.Disabled;
    }
    
    public virtual void Pause()
    {
        State = ModuleState.Paused;
    }
    
}