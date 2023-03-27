#pragma once
#ifndef HORIZONTAL_MOVING_TILE_H
#define HORIZONTAL_MOVING_TILE_H

#define H_MOVING_TILE_SPEED 200
#define H_MOVING_TILE_TIME_LENGTH 3.5f
#define H_MOVING_TILE_TIME_COOLDOWN 2.0f

#include "MovingTile.h"
#include "RigidbodyComponent.h"

class HorizontalMovingTile : public MovingTile
{
public:
    HorizontalMovingTile() {}
    ~HorizontalMovingTile() {}
    void Initialize() override;
    void Update(float deltaTime) override;

private:
    Rigidbody* m_Rb;
    Vector2D m_MovingDirection = Vector2D(-1, 0);
    float m_TimeLength = 0.0f;
    float m_cooldown = 0.0f;
    bool m_checkChange = false;

    void Update_Moving(float deltaTime);
};

#endif