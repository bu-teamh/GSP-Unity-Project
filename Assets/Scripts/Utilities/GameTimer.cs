using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GSP.Timer
{
	public class GameTimer
	{
		private float m_startTime;
		private float m_endTime;

		private bool m_started;

		public GameTimer(float _end)
		{
			m_startTime = 0.0f;
			m_endTime = _end;
			m_started = false;
		}

		public bool Update()
		{
			bool finished = false;

			if (m_started)
			{
				float now = Time.time;

				if (now > (m_startTime + m_endTime))
				{
					m_startTime = now;
					m_started = false;
					finished = true;
				}
			}

			return finished;
		}

		public bool Start()
		{
			if (!m_started)
			{
				m_started = true;
			}

			return m_started;
		}

		public bool Interrupt()
		{
			bool interrupt = false;

			if (m_started)
			{
				m_startTime = Time.time;
				m_started = false;

				interrupt = true;
			}

			return interrupt;
		}
	}
}
