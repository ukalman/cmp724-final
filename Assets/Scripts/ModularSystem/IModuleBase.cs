using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModuleState
{
    Active,
    Paused,
    Disabled
}
public interface IModuleBase
{
    string Name { get; }
    public ModuleState State { get; }
    bool IsEnabled { get; set; }
    ModuleController Controller { get; }
    
    void Initialize();
    void OnDestroy();
    
    bool Tick();         // Called during Update
    bool FixedTick();    // Called during FixedUpdate
    bool LateTick();     // Called during LateUpdate
    
    void Activate();
    void Disable();
    void Pause();
    
    void SetController(ModuleController controller);
    
    
}