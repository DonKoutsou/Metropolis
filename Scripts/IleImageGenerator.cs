using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
[Tool]
public class IleImageGenerator : Spatial
{
    [Export(PropertyHint.Layers3dPhysics)]
    public uint MoveLayer { get; set; }
    [Export]
    bool Update = false;
    public override void _Process(float delta)
    {
        base._Process(delta);
        if (Update)
        {
            GenerateImage();
            Update = false;
        }
    }
    public void GenerateImage()
	{
        //GD.Print(Owner.Name);
		Vector2 res = new Vector2(32, 32);
		int ilesize = 8000;
		Image im = new Image();
		im.Create((int)res.x, (int)res.y, true, Image.Format.Rgb8);
		int row = 0;
		int col = 0;
        float mult = ilesize/res.x;
		for (int i = 0; i < res.x * res.y; i++)
		{
			var spacestate = GetWorld().DirectSpaceState;

			Vector3 rayor = new Vector3((mult * col) - (ilesize / 2), 2000, (mult * row)  - (ilesize / 2));
			Vector3 rayend = rayor;
			rayend.y = -1000; 

			var rayar = spacestate.IntersectRay(rayor, rayend, new Godot.Collections.Array { this }, MoveLayer);

            if (rayar.Count == 0)
            {
                continue;
            }

            bool ItsSea = ((CollisionObject)rayar["collider"]).GetCollisionLayerBit(8);
            im.Lock();
			if (ItsSea)
			{
				im.SetPixel(col, row, Colors.Blue);
			}
            else
            {
                im.SetPixel(col, row, Colors.Brown);
            }
            im.Unlock();
			col ++;
			if (col >= 32)
			{
				col = 0;
				row ++;
				if (row >= 32)
				{
					break;
				}
			}
		}
        Island par = (Island)Owner;
        ImageTexture t = new ImageTexture();
		t.CreateFromImage(im);
        //par.Image = t;
		//im.SavePng("Ile.png");
	}
}
