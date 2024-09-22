using Godot;

[GlobalClass]
public partial class Day : Node
{
    public static int day;
    public static int score;
    
    public static float playerHealth;
    public static Godot.Collections.Array<InteractableObject.InteractableObjectType> tableItems {get; set;} = new Godot.Collections.Array<InteractableObject.InteractableObjectType>
    {};
}
