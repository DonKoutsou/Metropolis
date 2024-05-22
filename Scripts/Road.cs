using Godot;
using System;

[Tool]
public class Road : Path
{
    [Export]
    float PieaceDistance = 1.0f;

    bool isDirty = false;

    [Export(PropertyHint.Layers3dPhysics)]
    public uint FloorLayer { get; set; }

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
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
    private void UpdateMultiMesh()
    {
        float pathlength = Curve.GetBakedLength();
        int count = (int)Mathf.Floor(pathlength / PieaceDistance);
        MultiMesh MultiMeshChild = GetNode<MultiMeshInstance>("MultiMeshInstance").Multimesh;
        MultiMeshChild.InstanceCount = count;
        float offset = PieaceDistance / 2.0f;
        for (int i = 0; i < count; i++)
        {
            float curveDist = offset + PieaceDistance * i;
            Vector3 pos = Curve.InterpolateBaked(curveDist, true);

            var spaceState = GetWorld().DirectSpaceState;
            var result = spaceState.IntersectRay(pos, pos + (Vector3.Down * 1000),
                        new Godot.Collections.Array { this }, FloorLayer);
            

            Basis bass = Transform.basis;
            
            Vector3 up = Curve.InterpolateBakedUpVector(curveDist, true);
            Vector3 f = Curve.InterpolateBaked(curveDist + 0.1f, true);

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

            if (result.Count > 0)
                transform = new Transform(resultingBasis, (Vector3)result["position"]);
            else
                transform = new Transform(resultingBasis, pos);
                
            MultiMeshChild.SetInstanceTransform(i, transform);
        }
    }
    private void OnCurveChanged()
    {
        isDirty = true;
    }
}
