/**  版本信息模板在安装目录下，可自行修改。
* Humidity.cs
*
* 功 能： N/A
* 类 名： Humidity
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2017/3/12 10:57:18   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace IOT.Grain.Entity.DR_Model
{
	/// <summary>
	/// Humidity:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Humidity
	{
		public Humidity()
		{}
		#region Model
		private int _id;
		private decimal? _humility;
		private DateTime _stamptime;
		private string _sensorid;
		/// <summary>
		/// 自动编号
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 湿度值
		/// </summary>
		public decimal? Humility
		{
			set{ _humility=value;}
			get{return _humility;}
		}
		/// <summary>
		/// 时间
		/// </summary>
		public DateTime StampTime
		{
			set{ _stamptime=value;}
			get{return _stamptime;}
		}
		/// <summary>
		/// 传感器ID
		/// </summary>
		public string SensorId
		{
			set{ _sensorid=value;}
			get{return _sensorid;}
		}
		#endregion Model

	}
}

