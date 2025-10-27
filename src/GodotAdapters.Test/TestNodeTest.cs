using GdUnit4;
using static GdUnit4.Assertions;

namespace GodotAdapters.Test;

[TestSuite]
public class TestNodeTest
{
	[TestCase]
	public void logic_only_test()
	{
		var sum = 2 + 2;
		AssertThat(sum).IsEqual(4);
	}
	
	[TestCase, RequireGodotRuntime] // Only add this attribute if you touch Godot nodes/scenes/resources
	public void can_instantiate_node()
	{
		var n = new TestNode();
		AssertThat(n).IsNotNull();
	}
}
