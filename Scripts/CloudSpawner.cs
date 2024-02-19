using Godot;
using System;
using System.Drawing;

public class CloudSpawner : CSGBox
{
    [Export]
    int cloudammount;

    [Export]
    PackedScene CloudScene = null;

    public override void _Ready()
    {
        SpawnClouds();
    }
    public void SpawnClouds()
    {
        Random random = new Random();
        while (cloudammount >= 0)
        {
            cloudammount -= 1;
            var x = random.Next(-(int)Math.Round(Width/2), (int)Math.Round(Width/2));
            var y = random.Next(-(int)Math.Round(Height/2), (int)Math.Round(Height/2));
            var z = random.Next(-(int)Math.Round(Depth/2), (int)Math.Round(Depth/2));

            var spawnpos = new Vector3(x, y, z);

            Spatial cloud = (Spatial)CloudScene.Instance();
            AddChild(cloud);
            cloud.GlobalTranslation = cloud.GlobalTranslation + spawnpos;
        }
    }
}
