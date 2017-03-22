/**  版本信息模板在安装目录下，可自行修改。
* Temperature.cs
*
* 功 能： N/A
* 类 名： Temperature
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
	/// Temperature:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Temperature
	{
		public Temperature()
		{}
		#region Model
		private int _id;
		private decimal? _temp=0M;
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
		/// 温度值
		/// </summary>
		public decimal? Temp
		{
			set{ _temp=value;}
			get{return _temp;}
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

