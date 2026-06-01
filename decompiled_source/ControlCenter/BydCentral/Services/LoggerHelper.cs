using System;
using NLog;

namespace BydCentral.Services;

public class LoggerHelper
{
	private readonly Logger _logger = LogManager.GetCurrentClassLogger();

	private static LoggerHelper _obj;

	public static LoggerHelper _
	{
		get
		{
			return _obj ?? new LoggerHelper();
		}
		set
		{
			_obj = value;
		}
	}

	public void Debug(string msg)
	{
		_logger.Debug(msg);
	}

	public void Debug(string msg, Exception err)
	{
		_logger.Debug(err, msg);
	}

	public void Info(string msg)
	{
		_logger.Info(msg);
	}

	public void Info(string msg, Exception err)
	{
		_logger.Info(err, msg);
	}

	public void Warn(string msg)
	{
		_logger.Warn(msg);
	}

	public void Warn(string msg, Exception err)
	{
		_logger.Warn(err, msg);
	}

	public void Trace(string msg)
	{
		_logger.Trace(msg);
	}

	public void Trace(string msg, Exception err)
	{
		_logger.Trace(err, msg);
	}

	public void Error(string msg)
	{
		_logger.Error(msg);
	}

	public void Error(string msg, Exception err)
	{
		_logger.Error(err, msg);
	}

	public void Fatal(string msg)
	{
		_logger.Fatal(msg);
	}

	public void Fatal(string msg, Exception err)
	{
		_logger.Fatal(err, msg);
	}
}
