using Godot;
using System;

public class ItemPreviewPivot : Spatial
{
    Spatial ItemPreviewPivot2;
    MeshInstance Preview;
    static ItemPreviewPivot Instance;
    public override void _Ready()
    {
        base._Ready();
        ItemPreviewPivot2 = GetNode<Spatial>("ItemPreviewPivot2");
        Preview = ItemPreviewPivot2.GetNode<MeshInstance>("MeshInstance");
        Instance = this;
        Stop();
    }
    public static ItemPreviewPivot GetInstance()
    {
        return Instance;
    }
    public void Start(MeshInstance m)
    {
        Visible = true;
        Rotation = Vector3.Zero;
        SetProcessInput(true);

        GetNode<Spatial>("ItemPreviewPivot2").GetNode<MeshInstance>("MeshInstance").Mesh = m.Mesh;

        int amm = m.GetSurfaceMaterialCount();

        for (int i = 0; i < amm; i++)
        {
            Preview.SetSurfaceMaterial(i, m.GetSurfaceMaterial(i));
        }

        Vector3 transla = GetParent().GetNode<Camera>("Camera").Translation;
        transla.z = m.GetAabb().GetLongestAxisSize();
        GetParent().GetNode<Camera>("Camera").Translation = transla;
    }
    public void Stop()
    {
        Visible = false;
        SetProcessInput(false);
    }
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventMouseMotion)
		{
            if (!Input.IsActionPressed("Select"))
				return;

            Vector2 pos = new Vector2(((InputEventMouseMotion)@event).Relative.x, ((InputEventMouseMotion)@event).Relative.y);

            Vector3 rot = ItemPreviewPivot2.Rotation;
            Vector3 myrot = Rotation;

            rot.y += pos.x / 80;

            myrot.x += pos.y / 160;

            ItemPreviewPivot2.Rotation = rot;
            Rotation = myrot;
        }
        if (@event is InputEventJoypadMotion)
        {
            Vector2 velocity = new Vector2(
            Input.GetActionStrength("CameraRight") - Input.GetActionStrength("CameraLeft"),
            Input.GetActionStrength("CameraDown") - Input.GetActionStrength("CameraUp")
            ).LimitLength(2);

            velocity = new Vector2((float)Math.Round((double)velocity.x * 10, 3), (float)Math.Round((double)velocity.y * 10, 3));

            Vector3 rot = ItemPreviewPivot2.Rotation;
            Vector3 myrot = Rotation;

            rot.y += velocity.x / 80;

            myrot.x += velocity.y / 160;

            ItemPreviewPivot2.Rotation = rot;
            Rotation = myrot;
        }
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

}
