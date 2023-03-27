#include "Monster.h"

void Monster::Initialize()
{
	// Initialize Component
	m_Collider = owner->GetComponent<Collider>();
	m_Transform = owner->GetComponent<Transform>();
	m_Sprite = owner->GetComponent<Sprite>();
	m_Anim = owner->GetComponent<SpriteAnimation>();
	m_Anim->SetProps("normal_monster_idle", 4, 100);
	m_Collider->SetBuffer(-26, -34, 52, 52);
	m_Collider->Set(m_Transform->position.x, m_Transform->position.y, m_Sprite->GetWidth(), m_Sprite->GetHeight());
}

void Monster::Update(float deltaTime)
{
	Update_Collision();
	Update_Respawn(deltaTime);
}

void Monster::Update_Collision(bool x2damage)
{
	if (CollisionTriggerred() && m_IsAlive)
	{
		//if the player is dashing
		if (m_Player->Get_IsDashing())
		{

			//set IsDashThrough of Hero
			m_Player->m_IsDashThrough = true;

			//monster does not alive
			m_IsAlive = false;
			m_Sprite->m_IsEnable = false;


			//if the monster is deadly -> destroy and no need to respawn
			if (x2damage)
			{
				//SPAWN ARCHMONSTER EXPLOSION PARTICLE
				ParticleManager::GetInstance()->Spawn("archmonster_explosion", Vector2D(m_Transform->position.x - 15, m_Transform->position.y - 18));
				owner->Destroy();
			}
			else 
			{
				//SPAWN MONSTER EXPLOSION PARTICLE
				ParticleManager::GetInstance()->Spawn("monster_explosion", Vector2D(m_Transform->position.x - 15, m_Transform->position.y - 18));
			}

		}
		//if the player is not blinking
		else if (!m_Player->Get_IsBlinking())
		{
			//is the monster is deadly
			//if the monster is deadly -> player is dead
			if (x2damage)
			{
				//the player hurts
				m_Player->m_IsHurt = true;
				m_Player->m_Damagex2 = true;
			}
			//if the monster is normal one -> player is hurt
			else 
			{
				//the player hurts
				m_Player->m_IsHurt = true;
			}
			
		}
	}
}


void Monster::Update_Respawn(float deltaTime) 
{
	//if it is not alive
	if (!m_IsAlive) 
	{
		//wait for respawning
		m_DeadTime += deltaTime;
		if (m_DeadTime >= MONSTER_DEAD_TIME) 
		{
			m_DeadTime = 0.0f;
			m_Sprite->m_IsEnable = m_IsAlive = true;
			respawn_monster = false;
		}
	}

	//FIND SOME WAY TO SPAWN MONSTER RESPAWN PARTICLE
	if (m_DeadTime >= 4.5f && respawn_monster == false) {
		ParticleManager::GetInstance()->Spawn("monster_respawn", Vector2D(m_Transform->position.x - 15, m_Transform->position.y - 18));
		respawn_monster = true;
	}
}
