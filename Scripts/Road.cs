using Godot;
using System;
using System.Collections.Generic;

[Tool]
public class Road : Path
{
    [Export]
    float PieaceDistance = 1.0f;
    [Export]
    bool SnapToGround = true;
    [Export]
    bool AllignToNormal = true;

    [Export]
    bool SpawnCollisions = false;

    [Export]
    float pushtoflootam = 0.0f;

    [Export]
    Vector3 ExtraRot = Vector3.Zero;

    bool isDirty = false;

    [Export(PropertyHint.Layers3dPhysics)]
    public uint FloorLayer { get; set; }
    [Export(PropertyHint.Layers3dPhysics)]
    public uint StairLayer { get; set; }

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (!Engine.EditorHint)
		{
			SetProcess(false);
		}
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (isDirty)
        {
            GD.Print("Updating Curve");
            UpdateMultiMesh();
            isDirty = false;
        }
            
    }
    private void DeletePrevChilden()
    {
        foreach (Node shape in GetChildren())
        {
            if (shape is StaticBody)
                shape.Free();
        }
    }
    private void UpdateMultiMesh()
    {
        DeletePrevChilden();
        float pathlength = Curve.GetBakedLength();
        int count = (int)Mathf.Floor(pathlength / PieaceDistance);
        MultiMesh MultiMeshChild = GetNode<MultiMeshInstance>("MultiMeshInstance").Multimesh;
        MultiMeshChild.InstanceCount = count;
        float offset = PieaceDistance / 2.0f;

        List<StaticBody> bodies = new List<StaticBody>();
        for (int i = 0; i < count; i++)
        {
            float curveDist = offset + PieaceDistance * i;
            Vector3 pos = Curve.InterpolateBaked(curveDist, true);
            
            var spaceState = GetWorld().DirectSpaceState;
            var result = spaceState.IntersectRay(pos, pos + (Vector3.Down * 1000), new Godot.Collections.Array { this }, FloorLayer);
            

            Basis bass = Transform.basis;
            
            Vector3 up = Curve.InterpolateBakedUpVector(curveDist, true);
            Vector3 f = Curve.InterpolateBaked(curveDist + 0.1f, true);
            //Vector3 f = up * (Vector3.Forward * 2);
            f.y = pos.y;

            var forward = pos.DirectionTo(f);
            bass.y = up;
            bass.x = forward.Cross(up).Normalized();
            bass.z = -forward;

            Vector3 norm = (Vector3)result["normal"];
            var resultingBasis = new Basis(norm.Cross(bass.z), norm, bass.x.Cross(norm));

			resultingBasis = resultingBasis.Orthonormalized();
			resultingBasis.Scale = new Vector3(1, 1, 1);

            var transform = new Transform(resultingBasis, pos);
            

            //transform.(new Vector3(0,1,0), rot);
            Vector3 postouse = Vector3.Zero;
            Basis basetouse = new Basis();
            if (result.Count > 0 && SnapToGround)
            {
                if (AllignToNormal)
                {
                    postouse = (Vector3)result["position"];
                    postouse.y -= pushtoflootam;
                    basetouse = resultingBasis;
                }
                else
                {
                    postouse = (Vector3)result["position"];
                    postouse.y -= pushtoflootam;
                    basetouse = bass;
                }
            }
            else
            {
                if (AllignToNormal)
                {
                    postouse = pos;
                    basetouse = resultingBasis;
                }
                else
                {
                    postouse = pos;
                    basetouse = bass;
                }
            }
            //basetouse = basetouse.Rotated(ExtraRot.Normalized() , Mathf.Deg2Rad(rotam));
            transform = new Transform(basetouse, postouse);
                
            MultiMeshChild.SetInstanceTransform(i, transform);

            if (SpawnCollisions)
            {
                StaticBody body = new StaticBody();
                
                body.Name = "body" + i.ToString();
                bodies.Add(body);
                AddChild(body, true);
                
               
                
                CollisionShape shape = new CollisionShape();
                shape.Shape = new BoxShape();
                shape.Transform = transform;
                ((BoxShape)shape.Shape).Extents = MultiMeshChild.Mesh.GetAabb().Size / 2;
                shape.Name = "Shape" + i.ToString();
                body.AddChild(shape, true);

                
                body.Owner = Owner;
                shape.Owner = Owner;
            }
        }
        foreach (StaticBody bod in bodies)
        {
            bod.CollisionLayer = StairLayer;
            bod.CollisionMask = new uint();
            //bod.Hide();
        }
        //GetTree.CurrentScene.sav
    }
    private void OnCurveChanged()
    {
        isDirty = true;
    }
}
