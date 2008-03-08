﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Box2DX.Common
{
	public struct Sweep
	{
		public Vector2 LocalCenter;	//local center of mass position
		public Vector2 C0, C; //local center of mass position
		public float A0, A; //world angles
		public float T0; //time interval = [T0,1], where T0 is in [0,1]

		/// <summary>
		/// Get the interpolated transform at a specific time.
		/// </summary>
		/// <param name="xf"></param>
		/// <param name="t">The normalized time in [0,1].</param>
		public void GetXForm(out XForm xf, float t)
		{
			xf = new XForm();

			// center = p + R * LocalCenter
			if (1.0f - T0 > Math.FLT_EPSILON)
			{
				float alpha = (t - T0) / (1.0f - T0);
				xf.Position = (1.0f - alpha) * C0 + alpha * C;
				float angle = (1.0f - alpha) * A0 + alpha * A;
				xf.R.Set(angle);
			}
			else
			{
				xf.Position = C;
				xf.R.Set(A);
			}

			// Shift to origin
			xf.Position -= Math.Mul(xf.R, LocalCenter);
		}

		/// <summary>
		/// Advance the sweep forward, yielding a new initial state.
		/// </summary>
		/// <param name="t">The new initial time.</param>
		public void Advance(float t)
		{
			if (T0 < t && 1.0f - T0 > Math.FLT_EPSILON)
			{
				float alpha = (t - T0) / (1.0f - T0);
				C0 = (1.0f - alpha) * C0 + alpha * C;
				A0 = (1.0f - alpha) * A0 + alpha * A;
				T0 = t;
			}
		}
	}
}