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

		public bool Check()
		{
			bool finished = false;

			if (m_started)
			{
				float now = Time.fixedTime;

				if (now > (m_startTime + m_endTime))
				{
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
				m_startTime = Time.fixedTime;
				m_started = true;
			}

			return m_started;
		}

		public bool Interrupt()
		{
			bool interrupt = false;

			if (m_started)
			{
				m_startTime = Time.fixedTime;
				m_started = false;

				interrupt = true;
			}

			return interrupt;
		}

		public void Pause()
		{
			if (m_started)
			{
				m_started = false;
			}

			return;
		}
	}
}
