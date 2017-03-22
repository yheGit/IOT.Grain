/**  版本信息模板在安装目录下，可自行修改。
* Sensor.cs
*
* 功 能： N/A
* 类 名： Sensor
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2017/3/12 10:57:19   N/A    初版
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
	/// Sensor:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sensor
	{
		public Sensor()
		{}
		#region Model
		private int _id;
		private string _sensorid;
		private string _crc;
		private string _label;
		private int? _sequen=0;
		private string _collector;
		private int? _direction_x=0;
		private int? _direction_y=0;
		private int? _direction_z=0;
		private decimal? _standardtemp=0M;
		private decimal? _standardhum=0M;
		private int? _userid;
		private int? _isactive=0;
		/// <summary>
		/// 自动编号
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 传感器ID
		/// </summary>
		public string SensorId
		{
			set{ _sensorid=value;}
			get{return _sensorid;}
		}
		/// <summary>
		/// CRC是在网页字符串里面带来的
		/// </summary>
		public string CRC
		{
			set{ _crc=value;}
			get{return _crc;}
		}
		/// <summary>
		/// 标签
		/// </summary>
		public string Label
		{
			set{ _label=value;}
			get{return _label;}
		}
		/// <summary>
		/// 序号
		/// </summary>
		public int? Sequen
		{
			set{ _sequen=value;}
			get{return _sequen;}
		}
		/// <summary>
		/// 采集器ID
		/// </summary>
		public string Collector
		{
			set{ _collector=value;}
			get{return _collector;}
		}
		/// <summary>
		/// X坐标
		/// </summary>
		public int? Direction_X
		{
			set{ _direction_x=value;}
			get{return _direction_x;}
		}
		/// <summary>
		/// Y坐标
		/// </summary>
		public int? Direction_Y
		{
			set{ _direction_y=value;}
			get{return _direction_y;}
		}
		/// <summary>
		/// Z坐标
		/// </summary>
		public int? Direction_Z
		{
			set{ _direction_z=value;}
			get{return _direction_z;}
		}
		/// <summary>
		/// 标准温度
		/// </summary>
		public decimal? StandardTemp
		{
			set{ _standardtemp=value;}
			get{return _standardtemp;}
		}
		/// <summary>
		/// 标准湿度
		/// </summary>
		public decimal? StandardHum
		{
			set{ _standardhum=value;}
			get{return _standardhum;}
		}
		/// <summary>
		/// 用户ID
		/// </summary>
		public int? UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 0否1是
		/// </summary>
		public int? IsActive
		{
			set{ _isactive=value;}
			get{return _isactive;}
		}
		#endregion Model

	}
}

