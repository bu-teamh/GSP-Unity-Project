using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GSP.States
{
	public enum GlobalValue
	{
		PlayerHealth,
		PlayerCharge
	}

	public struct GlobalValueBundle
	{
		private int m_minimum;
		private int m_maximum;
		private int m_value;

		public GlobalValueBundle(int _min, int _max, int _initial)
		{
			m_minimum = _min;
			m_maximum = _max;
			m_value = _initial;
		}

		public void Add(int _value)
		{
			m_value += _value;
			m_value = Mathf.Clamp(m_value, m_minimum, m_maximum);
		}

		public int Value()
		{
			return m_value;
		}
	}
}

