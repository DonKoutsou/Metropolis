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
  List<LoddedCharacter> LODedChar;
  int Currentcheck = 0;
  int CurrentCharCheck = 0;

  public void UpdateLoddedObj()
  {
    LODedObj = new List<LoddedObject>();
    
    var loded = GetTree().GetNodesInGroup("LODDEDOBJ");
    foreach(LoddedObject ObjectToLodCheck in loded)
    {
        LODedObj.Add(ObjectToLodCheck);
    }
    
  }
  public void UpdateLoddedChars()
  {
    LODedChar = new List<LoddedCharacter>();
    var lodedC = GetTree().GetNodesInGroup("LODDEDCHAR");
    foreach(LoddedCharacter character in lodedC)
    {
        LODedChar.Add(character);
    }
  }
  public static void UpdateCamera(Camera cam)
  {
    CurrentCamera = cam;
  }

  public override void _EnterTree()
  {
    if(_instance != null){
       QueueFree(); // The Singletone is already loaded, kill this instance
    }
    _instance = this;

    LODedObj = new List<LoddedObject>();
    LODedChar = new List<LoddedCharacter>();
    SetProcess(false);
  }
  public override void _Process(float delta)
  {
      base._Process(delta);
      if (CurrentCamera == null || !Godot.Object.IsInstanceValid(CurrentCamera))
          return;

      if (Currentcheck >= LODedObj.Count)
      {
          Currentcheck = 0;
          UpdateLoddedObj();
      }
      else
      {
        int processed = 0;
        Player pl = Player.GetInstance();
        Vector3 campos = new Vector3(pl.GlobalTranslation.x, CurrentCamera.GlobalTranslation.y, pl.GlobalTranslation.z);
        while (processed < 10)
        {
          if (Currentcheck >= LODedObj.Count)
            break;

          LoddedObject objtocheck = LODedObj[Currentcheck];

          Currentcheck += 1;
          
          if (objtocheck != null && Godot.Object.IsInstanceValid(objtocheck) && objtocheck.IsInsideTree())
          {
            float dist = campos.DistanceTo(objtocheck.GlobalTranslation);
            float abbsize = objtocheck.abbLeangth;

            if (dist > abbsize + 1200)
              objtocheck.SwitchLod(2);
            else if (dist > objtocheck.abbLeangth + 500)
              objtocheck.SwitchLod(1);
            else
              objtocheck.SwitchLod(0);
          }
          processed += 1;

        }
      }
      if (CurrentCharCheck >= LODedChar.Count)
      {
          CurrentCharCheck = 0;
          UpdateLoddedChars();
      }
      else
      {
        Player pl = Player.GetInstance();
        Vector3 campos = new Vector3(pl.GlobalTranslation.x, CurrentCamera.GlobalTranslation.y, pl.GlobalTranslation.z);

        if (CurrentCharCheck >= LODedChar.Count)
          return;

        LoddedCharacter objtocheck = LODedChar[CurrentCharCheck];

        CurrentCharCheck += 1;
        
        if (objtocheck != null && Godot.Object.IsInstanceValid(objtocheck) && objtocheck.IsInsideTree())
        {
          float dist = campos.DistanceTo(objtocheck.GlobalTranslation);
        
          if (dist > 200)
              objtocheck.SwitchLod(true);
          else
              objtocheck.SwitchLod(false);
        }
      }
      
  }
}
