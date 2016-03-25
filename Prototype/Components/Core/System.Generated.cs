
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace NineByteGames.Core
{

  public delegate void SystemProcessorCallback<T1>(Entity entity, ref T1 component1);
  public delegate void SystemProcessorCallback<T1,T2>(Entity entity, ref T1 component1, ref T2 component2);
  public delegate void SystemProcessorCallback<T1,T2,T3>(Entity entity, ref T1 component1, ref T2 component2, ref T3 component3);
  public delegate void SystemProcessorCallback<T1,T2,T3,T4>(Entity entity, ref T1 component1, ref T2 component2, ref T3 component3, ref T4 component4);
  public delegate void SystemProcessorCallback<T1,T2,T3,T4,T5>(Entity entity, ref T1 component1, ref T2 component2, ref T3 component3, ref T4 component4, ref T5 component5);
  public delegate void SystemProcessorCallback<T1,T2,T3,T4,T5,T6>(Entity entity, ref T1 component1, ref T2 component2, ref T3 component3, ref T4 component4, ref T5 component5, ref T6 component6);
  public delegate void SystemProcessorCallback<T1,T2,T3,T4,T5,T6,T7>(Entity entity, ref T1 component1, ref T2 component2, ref T3 component3, ref T4 component4, ref T5 component5, ref T6 component6, ref T7 component7);



  public partial class SystemProcessor
  {
  
    private Action<Entity> _processCallback;

      
    protected void Initialize<T1>(
      SystemProcessorCallback<T1> callback, 
      ComponentId<T1> componentId1)
    {
      _processCallback = delegate(Entity entity)
      {
      
        ComponentReference<T1> component1 = entity.GetComponentReference(componentId1);

        callback.Invoke(entity, ref component1.Value);
      };
    }

      
    protected void Initialize<T1,T2>(
      SystemProcessorCallback<T1,T2> callback, 
      ComponentId<T1> componentId1, 
      ComponentId<T2> componentId2)
    {
      _processCallback = delegate(Entity entity)
      {
      
        ComponentReference<T1> component1 = entity.GetComponentReference(componentId1);
        ComponentReference<T2> component2 = entity.GetComponentReference(componentId2);

        callback.Invoke(entity, ref component1.Value, ref component2.Value);
      };
    }

      
    protected void Initialize<T1,T2,T3>(
      SystemProcessorCallback<T1,T2,T3> callback, 
      ComponentId<T1> componentId1, 
      ComponentId<T2> componentId2, 
      ComponentId<T3> componentId3)
    {
      _processCallback = delegate(Entity entity)
      {
      
        ComponentReference<T1> component1 = entity.GetComponentReference(componentId1);
        ComponentReference<T2> component2 = entity.GetComponentReference(componentId2);
        ComponentReference<T3> component3 = entity.GetComponentReference(componentId3);

        callback.Invoke(entity, ref component1.Value, ref component2.Value, ref component3.Value);
      };
    }

      
    protected void Initialize<T1,T2,T3,T4>(
      SystemProcessorCallback<T1,T2,T3,T4> callback, 
      ComponentId<T1> componentId1, 
      ComponentId<T2> componentId2, 
      ComponentId<T3> componentId3, 
      ComponentId<T4> componentId4)
    {
      _processCallback = delegate(Entity entity)
      {
      
        ComponentReference<T1> component1 = entity.GetComponentReference(componentId1);
        ComponentReference<T2> component2 = entity.GetComponentReference(componentId2);
        ComponentReference<T3> component3 = entity.GetComponentReference(componentId3);
        ComponentReference<T4> component4 = entity.GetComponentReference(componentId4);

        callback.Invoke(entity, ref component1.Value, ref component2.Value, ref component3.Value, ref component4.Value);
      };
    }

      
    protected void Initialize<T1,T2,T3,T4,T5>(
      SystemProcessorCallback<T1,T2,T3,T4,T5> callback, 
      ComponentId<T1> componentId1, 
      ComponentId<T2> componentId2, 
      ComponentId<T3> componentId3, 
      ComponentId<T4> componentId4, 
      ComponentId<T5> componentId5)
    {
      _processCallback = delegate(Entity entity)
      {
      
        ComponentReference<T1> component1 = entity.GetComponentReference(componentId1);
        ComponentReference<T2> component2 = entity.GetComponentReference(componentId2);
        ComponentReference<T3> component3 = entity.GetComponentReference(componentId3);
        ComponentReference<T4> component4 = entity.GetComponentReference(componentId4);
        ComponentReference<T5> component5 = entity.GetComponentReference(componentId5);

        callback.Invoke(entity, ref component1.Value, ref component2.Value, ref component3.Value, ref component4.Value, ref component5.Value);
      };
    }

      
    protected void Initialize<T1,T2,T3,T4,T5,T6>(
      SystemProcessorCallback<T1,T2,T3,T4,T5,T6> callback, 
      ComponentId<T1> componentId1, 
      ComponentId<T2> componentId2, 
      ComponentId<T3> componentId3, 
      ComponentId<T4> componentId4, 
      ComponentId<T5> componentId5, 
      ComponentId<T6> componentId6)
    {
      _processCallback = delegate(Entity entity)
      {
      
        ComponentReference<T1> component1 = entity.GetComponentReference(componentId1);
        ComponentReference<T2> component2 = entity.GetComponentReference(componentId2);
        ComponentReference<T3> component3 = entity.GetComponentReference(componentId3);
        ComponentReference<T4> component4 = entity.GetComponentReference(componentId4);
        ComponentReference<T5> component5 = entity.GetComponentReference(componentId5);
        ComponentReference<T6> component6 = entity.GetComponentReference(componentId6);

        callback.Invoke(entity, ref component1.Value, ref component2.Value, ref component3.Value, ref component4.Value, ref component5.Value, ref component6.Value);
      };
    }

      
    protected void Initialize<T1,T2,T3,T4,T5,T6,T7>(
      SystemProcessorCallback<T1,T2,T3,T4,T5,T6,T7> callback, 
      ComponentId<T1> componentId1, 
      ComponentId<T2> componentId2, 
      ComponentId<T3> componentId3, 
      ComponentId<T4> componentId4, 
      ComponentId<T5> componentId5, 
      ComponentId<T6> componentId6, 
      ComponentId<T7> componentId7)
    {
      _processCallback = delegate(Entity entity)
      {
      
        ComponentReference<T1> component1 = entity.GetComponentReference(componentId1);
        ComponentReference<T2> component2 = entity.GetComponentReference(componentId2);
        ComponentReference<T3> component3 = entity.GetComponentReference(componentId3);
        ComponentReference<T4> component4 = entity.GetComponentReference(componentId4);
        ComponentReference<T5> component5 = entity.GetComponentReference(componentId5);
        ComponentReference<T6> component6 = entity.GetComponentReference(componentId6);
        ComponentReference<T7> component7 = entity.GetComponentReference(componentId7);

        callback.Invoke(entity, ref component1.Value, ref component2.Value, ref component3.Value, ref component4.Value, ref component5.Value, ref component6.Value, ref component7.Value);
      };
    }

    
  }

    
  
  
  
  
  
  


}

