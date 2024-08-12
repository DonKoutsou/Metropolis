using Godot;
using System;

public class InventoryItemInOutNotification : Control
{
    [Export]
    PackedScene ItemNodeScene = null;
    public void OnItemAddedToInv(Item it)
    {
        VBoxContainer cont = GetNode<VBoxContainer>("VBoxContainer");
        
        var children = cont.GetChildren();
        for (int i = 0; i < children.Count; i++)
        {
            ItemNotification notif = (ItemNotification)children[i];
            if (notif.ItName == it.GetInventoryItemName() && notif.type == NotifType.POSSETIVE)
            {
                notif.AddtoAmmount();
                GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
                return;
            }
        }

        ItemNotification newnotif = ItemNodeScene.Instance<ItemNotification>();
        newnotif.ItName = it.GetInventoryItemName();
        newnotif.Icon = it.ItemIcon;
        newnotif.ammount = 1;

        cont.AddChild(newnotif);

        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
    }
    public void OnItemRemovedFromInv(Item it)
    {
        VBoxContainer cont = GetNode<VBoxContainer>("VBoxContainer");
        
        var children = cont.GetChildren();
        for (int i = 0; i < children.Count; i++)
        {
            ItemNotification notif = (ItemNotification)children[i];
            if (notif.ItName == it.GetInventoryItemName() && notif.type == NotifType.NEGATIVE)
            {
                notif.AddtoAmmount();
                return;
            }
        }

        ItemNotification newnotif = ItemNodeScene.Instance<ItemNotification>();
        newnotif.ItName = it.GetInventoryItemName();
        newnotif.Icon = it.ItemIcon;
        newnotif.ammount = -1;

        cont.AddChild(newnotif);

        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
    }
}
