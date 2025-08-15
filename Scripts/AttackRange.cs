using Godot;
using Godot.Collections;
using Target.Scenes;
using Target.User;
using Target.Weapons;

public partial class AttackRange : Area2D
{
    const int AXE = 0;

    [Export] private Array<PackedScene> WeaponList = [];

    public Timer _attackTimer;
    private bool _canThrowAxe = false;

    public override void _Ready()
    {
        _attackTimer = GetNode<Timer>("AttackTimer");
        _attackTimer.Timeout += OnAttackTimerTimeout;

        AreaEntered += OnAreaEntered;
    }

    private void OnAreaEntered(Area2D area)
    {

        if (area is not Barrel barrel) return;

        if (!_canThrowAxe) return;

        
        //CallDeferred("SpawnAxe", barrel);

    }

    private void SpawnAxe(Barrel barrel)
    {
        Player player = GetOwner<Player>();

        var axe = WeaponList[AXE].Instantiate<Axe>();
        GetTree().CurrentScene.AddChild(axe);

        // Spawn axe at player position and find angle to the barrel
        axe.GlobalPosition = player.GlobalPosition;
        var direction = (barrel.GlobalPosition - axe.GlobalPosition).Normalized();
        axe.GlobalRotation = direction.Angle();

        _canThrowAxe = false;

    }

    private void OnAttackTimerTimeout()
    {
        _canThrowAxe = true;
    }

}
