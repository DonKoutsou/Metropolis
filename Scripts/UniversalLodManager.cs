using Godot;
using System;
using System.Collections.Generic;

[Tool]
public partial class UniversalLodManager : Node
{
  private static UniversalLodManager _instance;
  public static UniversalLodManager Instance => _instance;
  static Camera CurrentCamera;
  List<LoddedObject> LODedObj;
  int Currentcheck = 0;

  public void UpdateLoddedObj()
  {
    LODedObj = new List<LoddedObject>();
    var loded = GetTree().GetNodesInGroup("LODDEDOBJ");
    foreach(LoddedObject mesh in loded)
    {
        LODedObj.Add(mesh);
    }
  }

  public static void UpdateCamera(Camera cam)
  {
    CurrentCamera = cam;
  }

  public override void _EnterTree()
  {
    if(_instance != null){
       this.QueueFree(); // The Singletone is already loaded, kill this instance
    }
    _instance = this;

    LODedObj = new List<LoddedObject>();
    SetProcess(false);
  }
  public override void _Process(float delta)
  {
      base._Process(delta);
      if (CurrentCamera == null || !Godot.Object.IsInstanceValid(CurrentCamera))
          return;

      if (Currentcheck == LODedObj.Count)
      {
          Currentcheck = 0;
          UpdateLoddedObj();
          return;
      }
      int processed = 0;
      Vector3 campos = CurrentCamera.GlobalTranslation;
      while (processed < 20)
      {
        if (Currentcheck == LODedObj.Count)
          return;

        LoddedObject objtocheck = LODedObj[Currentcheck];

        Currentcheck += 1;
        
        if (objtocheck != null && Godot.Object.IsInstanceValid(objtocheck) && objtocheck.IsInsideTree())
        {
          float dist = campos.DistanceTo(objtocheck.GlobalTranslation);
        
          if (dist > objtocheck.GetTransformedAabb().GetLongestAxisSize() + 500)
              objtocheck.SwitchLod(true);
          else
              objtocheck.SwitchLod(false);
        }
        processed += 1;

      }
      

      
  }
}
