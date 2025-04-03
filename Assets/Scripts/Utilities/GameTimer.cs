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

			if (!m_started)
			{
				m_startTime = Time.time;
			}
			else
			{
				float now = Time.time;

				if (now > (m_startTime + m_endTime))
				{
					m_started = false;
					finished = true;
				}
			}

			return finished;
		}

		public void Start()
		{
			m_started = true;

			return;
		}

		public void Reset()
		{
			m_startTime = Time.time;

			return;
		}
	}
}
