public class EventService
{
    private static EventService instance;
    public static EventService Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventService();
            }
            return instance;
        }
    }

    public EventController OnLightSwitchToggled { get; private set; }
    public EventController<int> OnKeyPickedUp { get; private set; }
    public EventController OnLightsOffByGhostEvent { get; private set; }
    public EventController OnRatRush {  get; private set; }
    public EventController OnSkullShower {  get; private set; }
    public EventController<int> OnPotionDrink {  get; private set; }
    public EventController OnDollRotationStart { get; private set; }
    public EventController OnDollRotationEnded { get; private set; }
    public EventController OnWhisperingStart { get; private set; }
    public EventController OnWhisperingEnded { get; private set; }

    public EventController OnPaintingChangeEvent { get; private set; }

    public EventController PlayerEscapedEvent { get; private set; }
    public EventController PlayerDeathEvent { get; private set; }

    public EventService()
    {
        OnLightSwitchToggled = new EventController();
        OnKeyPickedUp = new EventController<int>();
        OnLightsOffByGhostEvent = new EventController();
        OnRatRush = new EventController();
        OnSkullShower = new EventController();
        OnPotionDrink = new EventController<int>();
        OnDollRotationStart = new EventController();
        OnDollRotationEnded = new EventController();
        OnPaintingChangeEvent = new EventController();
        OnWhisperingStart = new EventController();
        OnWhisperingEnded = new EventController();

        PlayerEscapedEvent = new EventController();
        PlayerDeathEvent = new EventController();
    }
}
