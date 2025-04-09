using UnityEngine;

namespace GSP.Timer
{
	public class GameTimer
	{
		private float m_startTime;
		private float m_endTime;

		private bool m_started;
		private bool m_locked;

		public GameTimer(float _end)
		{
			m_startTime = 0.0f;
			m_endTime = _end;
			m_started = false;
			m_locked = false;
		}

		public GameTimer(float _end, bool _locked)
		{
			m_startTime = 0.0f;
			m_endTime = _end;
			m_started = false;
			m_locked = _locked;
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

		public float ScaleToTime(float _max, bool _inverse = false)
		{
			float progress = 0.0f;

			if (m_started)
			{
				float now = Time.fixedTime;

				progress = (now - m_startTime) / m_endTime;
				progress = Mathf.Clamp01(progress);
			}

			if (_inverse)
			{
				progress = 1.0f - progress;
			}

			return _max * progress;
		}

		public bool Start()
		{
			if (!m_started && !m_locked)
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
			if (m_started && !m_locked)
			{
				m_started = false;
			}

			return;
		}

		public void Unpause()
		{
			if (!m_started && !m_locked)
			{
				m_started = true;
			}
		}

		public void Lock()
		{
			if (!m_locked)
			{
				m_locked = true;
			}
			
			return;
		}

		public void Unlock()
		{
			if (m_locked)
			{
				m_locked = false;
			}

			return;
		}
	}
}
