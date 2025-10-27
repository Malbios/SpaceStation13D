using Godot;

namespace GodotAdapters;

public partial class TestNode : Node3D
{
	public override void _Ready()
	{
		GD.Print("Main scene ready (root node entered the tree).");
	}
}
