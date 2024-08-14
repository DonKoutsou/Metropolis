using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class RepairUI : Control
{
    [Export]
    Color FullDamageColor = new Color(0,0,0);

    [Export]
    Color HalfDamageColor = new Color(0,0,0);

    [Export]
    Color NoDamageColor = new Color(0,0,0);
    

    public override void _Ready()
    {
        base._Ready();
        SetProcess(false);
    }
    Vehicle currentv;
    public void StartUI(Vehicle v)
    {
        currentv = v;
        SetProcess(true);
        
        bool HasWings = currentv.HasWings();

        GetNode<Control>("Panel2/ShipPartPanel/Sails").Visible = HasWings;
        GetNode<Control>("Panel2/VBoxContainer/Sails").Visible = HasWings;
    }
    public void StopUI()
    {
        SetProcess(false);
    }
    float d = 0.5f;
    public override void _Process(float delta)
    {
        base._Process(delta);
        d -= delta;
        if (d>0)
            return;
        d = 0.5f;
        UpdateUI();
    }
    public void UpdateUI()
    {
        /*VehicleDamageManager damageman = currentv.GetDamageManager();

        List<int>_EngineStates;
        damageman.GetEngineStates(out _EngineStates);
        
        for (int i = 0; i< _EngineStates.Count; i++)
        {
            Color c = GetStateColor(_EngineStates[i]);
            
            GetNode<Control>("Panel2/ShipPartPanel/Engine" + i).Modulate = c;
            GetNode<BoatPartRepairPanel>("Panel2/VBoxContainer/Engine" + i).SetData("Μηχανή" + i, _EngineStates[i]);
        }
        
        GetNode<Control>("Panel2/ShipPartPanel/Hull").Modulate = GetStateColor(damageman.GetHullState());
        GetNode<BoatPartRepairPanel>("Panel2/VBoxContainer/Hull").SetData("Κουφάρι", damageman.GetHullState());
        */
        

        /*if (v.HasWings())
        {
            List<int>_WingStates;
            damageman.GetWingStates(out _WingStates);
            for (int i = 0; i< _WingStates.Count; i++)
            {
                Color c = GetStateColor(_WingStates[i]);
                
                GetNode<Control>("Panel2/ShipPartPanel/Engine" + 1).Modulate = c;
            }
        }*/
    }
    private Color GetStateColor(int state)
    {
        Color c = new Color(0,0,0);
        if (state > 66)
            c = NoDamageColor;
        else if (state > 33)
            c = HalfDamageColor;
        else
            c = FullDamageColor;
        return c;

    }
    private void OnPartRepaired(string PName)
    {
        Player pl = Player.GetInstance();

        List<Item> toolboxes;
        ItemName[] types = {ItemName.TOOLBOX};
        pl.GetCharacterInventory().GetItemsByType(out toolboxes, types);

        if(toolboxes.Count == 0)
        {
            DialogueManager.GetInstance().ScheduleDialogue(pl, "Δέν έχω προμήθειες για να το επισκευάσω");
            //pl.GetTalkText().Talk("Δέν έχω προμήθειες για να το επισκευάσω");
        }
        bool outofsupplies = true;
        /*VehicleDamageManager damageman = currentv.GetDamageManager();
        
        if (PName.Contains("Engine"))
        {
            int engineind = PName.Substring(PName.Length - 1).ToInt();
            int ammounttofix = 100 - damageman.GetEngineState(engineind);
            for (int i = 0; i < toolboxes.Count; i++)
            {
                Toolbox t = (Toolbox)toolboxes[i];
                float sup = t.GetCurrentSupplyAmmount();
                if (sup == 0)
                {
                    continue;
                }
                else if (sup > ammounttofix / 2)
                {
                    damageman.RepairEngine(engineind, ammounttofix);
                    t.ConsumeSupplies(ammounttofix / 2);
                    return;
                }
                else
                {
                    damageman.RepairEngine(engineind, (int)sup);
                    t.ConsumeSupplies((int)sup / 2);
                    outofsupplies = false;
                }
            }
        }*/
        if (outofsupplies)
        {
            DialogueManager.GetInstance().ScheduleDialogue(pl, "Οι εργαλειοθήκες μου είναι άδειες");
            //pl.GetTalkText().Talk("Οι εργαλειοθήκες μου είναι άδειες");
        }
    }
}
