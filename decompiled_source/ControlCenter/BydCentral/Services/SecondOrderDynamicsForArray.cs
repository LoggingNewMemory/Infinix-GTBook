using System;

namespace BydCentral.Services;

public class SecondOrderDynamicsForArray
{
	private double[] xps;

	private double[] xds;

	private double[] ys;

	private double[] yds;

	private double _w;

	private double _z;

	private double _d;

	private double k1;

	private double k2;

	private double k3;

	public SecondOrderDynamicsForArray(double f, double z, double r, double x0, int size)
	{
		_w = Math.PI * 2.0 * f;
		_z = z;
		_d = _w * Math.Sqrt(Math.Abs(z * z - 1.0));
		k1 = z / (Math.PI * f);
		k2 = 1.0 / (Math.PI * 2.0 * f * (Math.PI * 2.0 * f));
		k3 = r * z / (Math.PI * 2.0 * f);
		xps = new double[size];
		ys = new double[size];
		xds = new double[size];
		yds = new double[size];
		Array.Fill(xps, x0);
		Array.Fill(ys, x0);
	}

	public double[] Update(double deltaTime, double[] xs)
	{
		if (xs.Length != xps.Length)
		{
			throw new ArgumentException();
		}
		for (int i = 0; i < xds.Length; i++)
		{
			xds[i] = (xs[i] - xps[i]) / deltaTime;
		}
		double num;
		double num2;
		if (_w * deltaTime < _z)
		{
			num = k1;
			num2 = Math.Max(Math.Max(k2, deltaTime * deltaTime / 2.0 + deltaTime * k1 / 2.0), deltaTime * k1);
		}
		else
		{
			double num3 = Math.Exp((0.0 - _z) * _w * deltaTime);
			double num4 = 2.0 * num3 * ((_z <= 1.0) ? Math.Cos(deltaTime * _d) : Math.Cosh(deltaTime * _d));
			double num5 = num3 * num3;
			double num6 = deltaTime / (1.0 + num5 - num4);
			num = (1.0 - num5) * num6;
			num2 = deltaTime * num6;
		}
		for (int j = 0; j < ys.Length; j++)
		{
			ys[j] += deltaTime * yds[j];
			yds[j] += deltaTime * (xs[j] + k3 * xds[j] - ys[j] - num * yds[j]) / num2;
		}
		for (int k = 0; k < xps.Length; k++)
		{
			xps[k] = xs[k];
		}
		return ys;
	}
}
