using System;
using Godot;

public static class CanMoveWithInteratia
{
    /// <summary>
    /// Moves the body with inertia.
    /// </summary>
    /// <param name="body">The body that will move.</param>
    /// <param name="delta">The time that has passed since the last frame.
    /// </param>
    /// <param name="direction">The direction in which the body will move. This
    /// value will be normalized before it is applied.</param>
    /// <param name="acceleration">The rate at which the body will speed up in
    /// pixels per second squared.</param>
    /// <param name="deceleration">The rate at which the body will slow down in
    /// pixels per second squared.</param>
    /// <param name="limit">The maximum speed at which the body can move in
    /// pixels per second.</param>
    public static void Accelerate(
        this CharacterBody2D body,
        double delta,
        float direction,
        float acceleration,
        float deceleration,
        float limit
    )
    {
        Accelerate(
            body: body,
            delta: delta,
            direction: new Vector2(direction, 0),
            acceleration: new Vector2(acceleration, 0),
            deceleration: new Vector2(deceleration, 0),
            limit: new Vector2(limit, 0)
        );
    }

    /// <summary>
    /// Moves the body with inertia.
    /// </summary>
    /// <param name="body">The body that will move.</param>
    /// <param name="delta">The time that has passed since the last frame.
    /// </param>
    /// <param name="direction">The direction in which the body will move. This
    /// value will be normalized before it is applied.</param>
    /// <param name="acceleration">The rate at which the body will speed up in
    /// pixels per second squared.</param>
    /// <param name="deceleration">The rate at which the body will slow down in
    /// pixels per second squared.</param>
    /// <param name="limit">The maximum speed at which the body can move in
    /// pixels per second.</param>
    public static void Accelerate(
        this CharacterBody2D body,
        double delta,
        Vector2 direction,
        float acceleration,
        float deceleration,
        float limit
    )
    {
        Accelerate(
            body: body,
            delta: delta,
            direction: direction,
            acceleration: new Vector2(acceleration, acceleration),
            deceleration: new Vector2(deceleration, deceleration),
            limit: new Vector2(limit, limit)
        );
    }

    /// <summary>
    /// Moves the body with inertia.
    /// </summary>
    /// <param name="body">The body that will move.</param>
    /// <param name="delta">The time that has passed since the last frame.
    /// </param>
    /// <param name="direction">The direction in which the body will move. This
    /// value will be normalized before it is applied.</param>
    /// <param name="acceleration">The rate at which the body will speed up in
    /// pixels per second squared.</param>
    /// <param name="deceleration">The rate at which the body will slow down in
    /// pixels per second squared.</param>
    /// <param name="limit">The maximum speed at which the body can move in
    /// pixels per second.</param>
    public static void Accelerate(
        this CharacterBody2D body,
        double delta,
        Vector2 direction,
        Vector2 acceleration,
        Vector2 deceleration,
        Vector2 limit
    )
    {
        Vector2 normalizedDirection = direction.Normalized();

        body.Velocity = new Vector2(
            Mathf.MoveToward(
                body.Velocity.X,
                limit.X * normalizedDirection.X,
                (
                    Mathf.Abs(body.Velocity.X)
                    < Mathf.Abs(limit.X * normalizedDirection.X)
                        ? acceleration.X
                        : deceleration.X
                ) * (float)delta
            ),
            Mathf.MoveToward(
                body.Velocity.Y,
                limit.Y * normalizedDirection.Y,
                (
                    Mathf.Abs(body.Velocity.Y)
                    < Mathf.Abs(limit.Y * normalizedDirection.Y)
                        ? acceleration.Y
                        : deceleration.Y
                ) * (float)delta
            )
        );
    }
}
