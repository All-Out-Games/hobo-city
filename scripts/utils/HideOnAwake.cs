using AO;

public class HideOnAwake : Component
{
    public override void Awake()
    {
        GetComponent<Sprite_Renderer>().LocalEnabled = false;
    }
}
