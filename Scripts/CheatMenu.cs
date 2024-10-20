using Godot;
using System;
using System.Linq;
////////////////////////////////////////////////////////////////////////////////////
/*
 ██████╗██╗  ██╗███████╗ █████╗ ████████╗    ███╗   ███╗███████╗███╗   ██╗██╗   ██╗
██╔════╝██║  ██║██╔════╝██╔══██╗╚══██╔══╝    ████╗ ████║██╔════╝████╗  ██║██║   ██║
██║     ███████║█████╗  ███████║   ██║       ██╔████╔██║█████╗  ██╔██╗ ██║██║   ██║
██║     ██╔══██║██╔══╝  ██╔══██║   ██║       ██║╚██╔╝██║██╔══╝  ██║╚██╗██║██║   ██║
╚██████╗██║  ██║███████╗██║  ██║   ██║       ██║ ╚═╝ ██║███████╗██║ ╚████║╚██████╔╝
 ╚═════╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝   ╚═╝       ╚═╝     ╚═╝╚══════╝╚═╝  ╚═══╝ ╚═════╝                                                                                    
*/
////////////////////////////////////////////////////////////////////////////////////
#if DEBUG
public class CheatMenu : Control
{
	Player Playr;

	//CameraMovePivot CamMove;

	CameraZoomPivot CamZoom;

	bool Showing = false;

	bool HasSpeed = false;

	bool HasInfCam = false;

	bool ShowingFPS = false;
	Control Buttons;
	Control TimeProgressionSetting;
	Control TeleportPanel;

	Label FpsCounter;
	Label RainAmm;
	Label WindDir;
	Label WindAmm;
	private void On_FastRun_Button_Down()
	{
		if (HasSpeed)
		{
			Playr.SetRunSpeed(30);
			HasSpeed = false;
		}
		else
		{
			HasSpeed = true;
			Playr.SetRunSpeed(1000);
		}
		
	}
	private void On_InfCamera_Button_Down()
	{
		if (HasInfCam)
		{
			//CamMove.MaxDist = 150.0f;
			CamZoom.MaxDist = 150.0f;
			//CamMove.Frame();
			CamZoom.Frame();
			HasInfCam = false;
		}
		else
		{
			//CamMove.MaxDist = 9999999.0f;
			CamZoom.MaxDist = 9999999.0f;
			HasInfCam = true;
		}
		
	}
	private void On_FPS_Button_Down()
	{
		if (ShowingFPS)
		{
			SetProcess(false);
			ShowingFPS = false;
			FpsCounter.Hide();
		}
		else
		{
			SetProcess(true);
			ShowingFPS = true;
			FpsCounter.Show();
		}
	}
	private void On_Kill_Button_Down()
	{
		Player.GetInstance().Kill();
	}
	private void On_TP_Button_Down()
	{
		int x = TeleportPanel.GetNode<TextEdit>("X_Text").Text.ToInt();
		int y = TeleportPanel.GetNode<TextEdit>("Y_Text").Text.ToInt();
		Vector2 loc = WorldMap.GetInstance().MapToWorld( new Vector2(x,y));
		Player.GetInstance().Teleport(new Vector3 (loc.x, 5000, loc.y), Vector3.Zero);
	}
	private void On_ToLoc_Button_Down()
	{
		Player pl = Player.GetInstance();
		pl.Teleport(pl.GetMovingLocation(), Vector3.Zero);
	}
	private void IncreaseTimeProgression()
	{
		SettingsPanel.Instance.IncreaseTimeProgression();
		TimeProgressionSetting.GetNode<Label>("TimeProgText").Text = "TimeProg" + SettingsPanel.Instance.ToString();
	}
	private void DecreaseTimeProgression()
	{
		SettingsPanel.Instance.DecreaseTimeProgression();
		TimeProgressionSetting.GetNode<Label>("TimeProgText").Text = "TimeProg" + SettingsPanel.Instance.TimeProgression.ToString();
	}
	private void Recharge_Character()
	{
		Player.GetInstance().RechargeCharacter(100);
	}
	public void Start()
	{
		Show();
		Showing = true;
		Buttons.MouseFilter = MouseFilterEnum.Stop;
		SetProcess(true);
	}
	public void Stop()
	{
		Hide();
		Showing = false;
		Buttons.MouseFilter = MouseFilterEnum.Ignore;
		SetProcess(false);
	}
	public void Print_ExitDir()
	{
		GD.Print(WorldMap.GetInstance().GetExitDirection());
	}
	public void PlayerToggle(Player pl)
    {
		bool toggle = pl != null;
		SetProcessInput(toggle);
        //Visible = toggle;
		if (toggle)
		{
			Playr = pl;
			CameraPanPivot pan = Playr.GetNode<Position3D>("CameraMovePivot").GetNode<CameraPanPivot>("CameraPanPivot");
			CamZoom = pan.GetNode<SpringArm>("SpringArm").GetNode<CameraZoomPivot>("CameraZoomPivot");
		}
    }
	private void GiveItem()
	{
		int Index = GetNode<OptionButton>("Buttons/Panel/OptionButton").Selected;
		var values = Enum.GetValues(typeof(ItemName));
		ItemName it = (ItemName)values.GetValue(Index);
		Inventory inv = Player.GetInstance().GetCharacterInventory();
		inv.InsertItem(GlobalItemCatalogue.GetInstance().GetItemByType(it).Instance<Item>());
	}
	public override void _Ready()
	{
		Buttons = GetNode<Control>("Buttons");
		TimeProgressionSetting = Buttons.GetNode<Panel>("TimeProgressionSetting");
		TeleportPanel = Buttons.GetNode<Panel>("TeleportPanel");
		
		Control labelcont = (Control)GetNode("PanelContainer").GetNode("HBoxContainer").GetNode("Labels");
		FpsCounter = labelcont.GetNode<Label>("FPS_Counter");
		RainAmm = labelcont.GetNode<Label>("RainAmmount");
		WindDir = labelcont.GetNode<Label>("WindDir");
		WindAmm = labelcont.GetNode<Label>("WindAmm");
		OptionButton b = GetNode<OptionButton>("Buttons/Panel/OptionButton");
		var count = Enum.GetNames(typeof(ItemName));
		for (int i = 0; i < count.Count(); i++)
		{
			b.AddItem(count[i], i);
		}
		Visible = false;
		SetProcessInput(false);
		SetProcess(false);
	}
    public override void _Process(float delta)
    {
        base._Process(delta);
		FpsCounter.Text = Engine.GetFramesPerSecond().ToString();
		RainAmm.Text = Math.Round(CustomEnviroment.GetRainStr()).ToString();
		WindDir.Text = Math.Round(CustomEnviroment.GetWindDirection()).ToString();
		WindAmm.Text = Math.Round(CustomEnviroment.GetWindStr()).ToString();
    }

    public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("CheatMenu"))
		{
			if (!Showing)
				Start();
			else
				Stop();
		}
	}
}
#endif




