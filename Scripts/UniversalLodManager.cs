using Godot;
using System;
using System.Collections.Generic;

public partial class UniversalLodManager : Node
{
  private static UniversalLodManager _instance;
  Camera CurrentCamera;
  //Player pl;
  List<LoddedObject> LODedObj;
  List<LoddedCharacter> LODedChar;
  int Currentcheck = 0;
  int CurrentCharCheck = 0;


  public static UniversalLodManager GetInstance()
  {
    return _instance;
  }
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
  public void UpdateCamera(Camera cam)
  {
      CurrentCamera = cam;
      if (cam == null)
        SetProcess(false);
      else
      {
        SetProcess(true);
        //pl = (Player)cam.Owner;
      } 
  }

  public override void _EnterTree()
  {
    _instance = this;

    LODedObj = new List<LoddedObject>();
    LODedChar = new List<LoddedCharacter>();
    SetProcess(false);
  }
    public override void _Ready()
    {
        base._Ready();
        SetProcess(false);
    }
    public override void _Process(float delta)
  {
      base._Process(delta);

      Vector3 campos = CurrentCamera.GlobalTranslation;

      if (Currentcheck >= LODedObj.Count)
      {
          Currentcheck = 0;
          UpdateLoddedObj();
      }
      else
      {
        int processed = 0;
        
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

          
            if (dist > abbsize + 5000)
              objtocheck.SwitchLod(3);
            else if (dist > abbsize + 2500)
              objtocheck.SwitchLod(2);
            else if (dist > abbsize + 500)
              objtocheck.SwitchLod(1);
            else
              objtocheck.SwitchLod(0);
            /*float DistTest = Mathf.Max(abbsize, 200);

            if (dist > DistTest * 8)
              objtocheck.SwitchLod(3);
            else if (dist > DistTest * 4)
              objtocheck.SwitchLod(2);
            else if (dist > DistTest * 2)
              objtocheck.SwitchLod(1);
            else
              objtocheck.SwitchLod(0);*/
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
        //Vector3 campos = new Vector3(pl.GlobalTranslation.x, CurrentCamera.GlobalTranslation.y, pl.GlobalTranslation.z);

        if (CurrentCharCheck >= LODedChar.Count)
          return;

        LoddedCharacter objtocheck = LODedChar[CurrentCharCheck];

        CurrentCharCheck += 1;
        
        if (objtocheck != null && Godot.Object.IsInstanceValid(objtocheck) && objtocheck.IsInsideTree())
        {
          float dist = campos.DistanceTo(objtocheck.GlobalTranslation);

          if (dist > 800)
              objtocheck.SwitchLod(2);
          else if (dist > 200)
              objtocheck.SwitchLod(1);
          else
              objtocheck.SwitchLod(0);
        }
      }
      
  }
}
