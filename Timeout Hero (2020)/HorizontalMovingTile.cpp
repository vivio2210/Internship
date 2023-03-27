#include "HorizontalMovingTile.h"

void HorizontalMovingTile::Initialize()
{
    m_Collider = owner->GetComponent<Collider>();
    m_Transform = owner->GetComponent<Transform>();
    m_Sprite = owner->GetComponent<Sprite>();
    m_Rb = owner->GetComponent<Rigidbody>();

    m_Rb->setGravity(0.0f);

    m_Collider->Set(m_Transform->position.x, m_Transform->position.y, m_Sprite->GetWidth(), m_Sprite->GetHeight());

    m_Sprite->SetSpriteFrame(1);
}

void HorizontalMovingTile::Update(float deltaTime)
{
    Update_Moving(deltaTime);
    MovingTile::Update(deltaTime);
}


void HorizontalMovingTile::Update_Moving(float deltaTime)
{
    if (m_TimeLength >= H_MOVING_TILE_TIME_LENGTH) {
        m_Rb->m_velocity = (0.0f, 0.0f);
        m_Transform->Translate(m_Rb->Position());

        if (m_cooldown >= H_MOVING_TILE_TIME_COOLDOWN / 2) {
            if (m_checkChange == false) {
                if (m_MovingDirection.x > 0)
                {
                    m_Sprite->SetSpriteFrame(1);
                }
                else
                {
                    m_Sprite->SetSpriteFrame(0);
                }
                m_checkChange = true;
            }

            if (m_cooldown >= H_MOVING_TILE_TIME_COOLDOWN) {
                m_checkChange = false;
                m_cooldown = 0.0f;
                m_TimeLength = 0.0f;
                m_MovingDirection = m_MovingDirection * -1;
            }
        }
        m_cooldown += deltaTime;
    }
    else {
        m_Rb->m_velocity = m_MovingDirection * H_MOVING_TILE_SPEED;
        m_Transform->Translate(m_Rb->Position());
        m_TimeLength += deltaTime;
    }
    m_Collider->Set(m_Transform->position.x, m_Transform->position.y, m_Sprite->GetWidth(), m_Sprite->GetHeight());
}
