#pragma once
#ifndef MONSTER_H
#define MONSTER_H

#define MONSTER_DEAD_TIME 5.0f

#include "LevelObject.h"
#include "ParticleManager.h"

class Monster : public LevelObject
{
	public:
		Monster() {}
		virtual ~Monster() {}
		void Initialize() override;
		void Update(float deltaTime) override;

	protected:
		bool m_IsAlive = true;
		void Update_Collision(bool isDeadly = false);
		void Update_Respawn(float deltaTime);

	private:
		float m_DeadTime = 0.0f;
		bool respawn_monster = false;
};

#endif