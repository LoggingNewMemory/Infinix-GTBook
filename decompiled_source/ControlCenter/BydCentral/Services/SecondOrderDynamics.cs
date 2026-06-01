using System;

namespace BydCentral.Services;

public class SecondOrderDynamics
{
	private double xp;

	private double y;

	private double yd;

	private double _w;

	private double _z;

	private double _d;

	private double k1;

	private double k2;

	private double k3;

	public SecondOrderDynamics(double f, double z, double r, double x0)
	{
		_w = Math.PI * 2.0 * f;
		_z = z;
		_d = _w * Math.Sqrt(Math.Abs(z * z - 1.0));
		k1 = z / (Math.PI * f);
		k2 = 1.0 / (Math.PI * 2.0 * f * (Math.PI * 2.0 * f));
		k3 = r * z / (Math.PI * 2.0 * f);
		xp = x0;
		y = x0;
		yd = 0.0;
	}

	public double Update(double deltaTime, double x)
	{
		double num = (x - xp) / deltaTime;
		double num2;
		double num3;
		if (_w * deltaTime < _z)
		{
			num2 = k1;
			num3 = Math.Max(Math.Max(k2, deltaTime * deltaTime / 2.0 + deltaTime * k1 / 2.0), deltaTime * k1);
		}
		else
		{
			double num4 = Math.Exp((0.0 - _z) * _w * deltaTime);
			double num5 = 2.0 * num4 * ((_z <= 1.0) ? Math.Cos(deltaTime * _d) : Math.Cosh(deltaTime * _d));
			double num6 = num4 * num4;
			double num7 = deltaTime / (1.0 + num6 - num5);
			num2 = (1.0 - num6) * num7;
			num3 = deltaTime * num7;
		}
		y += deltaTime * yd;
		yd += deltaTime * (x + k3 * num - y - num2 * yd) / num3;
		xp = x;
		return y;
	}
}
