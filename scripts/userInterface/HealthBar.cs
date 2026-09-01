using Godot;

namespace Zeldavania.UserInterface;

[GlobalClass]
public partial class HealthBar : Control
{
    [Export]
    public Vector2 BarSize { get; set; } = new Vector2(40, 5);

    [Export]
    public Color BorderColor { get; set; } = new Color("efe7ce");

    [Export]
    public Color BackgroundColor { get; set; } = new Color("21263f");

    [Export]
    public Color FillColor { get; set; } = new Color("df2438");

    private int _currentHitPoints = 12;
    private int _maxHitPoints = 12;

    public override void _Ready()
    {
        CustomMinimumSize = BarSize;
    }

    public void UpdateHealth(int currentHitPoints, int maxHitPoints)
    {
        _currentHitPoints = Mathf.Clamp(currentHitPoints, 0, maxHitPoints);
        _maxHitPoints = Mathf.Max(1, maxHitPoints);
        QueueRedraw();
    }

    public override void _Draw()
    {
        float innerWidth = BarSize.X - 2;
        float innerHeight = BarSize.Y - 2;

        // 1. Draw border as full-sized base rect.
        DrawRect(new Rect2(0, 0, BarSize.X, BarSize.Y), BorderColor);

        // 2. Draw background inset by 1px.
        DrawRect(new Rect2(1, 1, innerWidth, innerHeight), BackgroundColor);

        // 3. Draw fill on top of background.
        if (_maxHitPoints > 0 && _currentHitPoints > 0)
        {
            float fillWidth = Mathf.Round(innerWidth * ((float)_currentHitPoints / _maxHitPoints));
            if (fillWidth > 0)
            {
                DrawRect(new Rect2(1, 1, fillWidth, innerHeight), FillColor);
            }
        }
    }
}
