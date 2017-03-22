/**  版本信息模板在安装目录下，可自行修改。
* WareHouse.cs
*
* 功 能： N/A
* 类 名： WareHouse
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
	/// WareHouse:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class WareHouse
	{
		public WareHouse()
		{}
		#region Model
		private int _id;
		private string _number;
		private string _name;
		private string _location;
		private int? _type=1;
		private string _userid;
		private decimal? _averagetemperature=0M;
		private decimal? _maximumemperature=0M;
		private decimal? _minimumtemperature=0M;
		private DateTime? _stamptime;
		private int? _isactive=0;
		/// <summary>
		/// 自动编号ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 手工编号
		/// </summary>
		public string Number
		{
			set{ _number=value;}
			get{return _number;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 详细位置
		/// </summary>
		public string Location
		{
			set{ _location=value;}
			get{return _location;}
		}
		/// <summary>
		/// 粮仓类型(1楼房、2平方、3筒仓)
		/// </summary>
		public int? Type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 保管员
		/// </summary>
		public string UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 平均温度
		/// </summary>
		public decimal? AverageTemperature
		{
			set{ _averagetemperature=value;}
			get{return _averagetemperature;}
		}
		/// <summary>
		/// 最高温度
		/// </summary>
		public decimal? Maximumemperature
		{
			set{ _maximumemperature=value;}
			get{return _maximumemperature;}
		}
		/// <summary>
		/// 最低温度
		/// </summary>
		public decimal? MinimumTemperature
		{
			set{ _minimumtemperature=value;}
			get{return _minimumtemperature;}
		}
		/// <summary>
		/// 时间戳
		/// </summary>
		public DateTime? StampTime
		{
			set{ _stamptime=value;}
			get{return _stamptime;}
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

