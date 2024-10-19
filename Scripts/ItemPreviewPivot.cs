using Godot;
using System;

public class ItemPreviewPivot : Spatial
{
    bool MouseInWindow = false;
    float ZoomStage = 20;
    float StartingZoom;
    Spatial ItemPreviewPivot2;
    MeshInstance Preview;
    public override void _Ready()
    {
        base._Ready();
        ItemPreviewPivot2 = GetNode<Spatial>("ItemPreviewPivot2");
        Preview = ItemPreviewPivot2.GetNode<MeshInstance>("MeshInstance");

        Stop();
    }
    public void Start(MeshInstance m)
    {
        GetParent().GetParent().GetParent().GetParent<Control>().Visible = true;
        Rotation = Vector3.Zero;
        ItemPreviewPivot2.Rotation = Vector3.Zero;
        SetProcessInput(true);

        GetNode<Spatial>("ItemPreviewPivot2").GetNode<MeshInstance>("MeshInstance").Mesh = m.Mesh;

        int amm = m.GetSurfaceMaterialCount();

        for (int i = 0; i < amm; i++)
        {
            Preview.SetSurfaceMaterial(i, m.GetSurfaceMaterial(i));
        }

        Vector3 transla = GetParent().GetNode<Camera>("Camera").Translation;
        StartingZoom = m.GetAabb().GetLongestAxisSize() * 2;
        transla.z = StartingZoom;
        GetParent().GetNode<Camera>("Camera").Translation = transla;
    }
    public void Stop()
    {
        GetParent().GetParent().GetParent().GetParent<Control>().Visible = false;
        SetProcessInput(false);
    }
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        
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
        if (MouseInWindow)
        {
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
            if (@event.IsActionPressed("ZoomOut") && ZoomStage < 20)
            {
                ZoomStage += 1;
                float fovvalue = StartingZoom * (ZoomStage / 10);
                Vector3 transla = GetParent().GetNode<Camera>("Camera").Translation;
                transla.z = fovvalue;
                GetParent().GetNode<Camera>("Camera").Translation = transla;
            }
            if (@event.IsActionPressed("ZoomIn") && ZoomStage > -20)
            {
                ZoomStage -= 1;
                float fovvalue = StartingZoom * (ZoomStage / 10);
                Vector3 transla = GetParent().GetNode<Camera>("Camera").Translation;
                transla.z = fovvalue;
                GetParent().GetNode<Camera>("Camera").Translation = transla;
            }
        }
        
    }
    private void MouseIn()
    {
        MouseInWindow = true;
    }
    private void MouseOut()
    {
        MouseInWindow = false;
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

}
