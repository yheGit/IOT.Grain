/**  版本信息模板在安装目录下，可自行修改。
* Floor.cs
*
* 功 能： N/A
* 类 名： Floor
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
	/// Floor:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Floor
	{
		public Floor()
		{}
		#region Model
		private int _id;
		private string _number;
		private string _location;
		private string _wh_number;
		private int? _userid;
		private int? _isactive=0;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 手工编号(L1-F2)
		/// </summary>
		public string Number
		{
			set{ _number=value;}
			get{return _number;}
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
		/// 所属楼房编号
		/// </summary>
		public string WH_Number
		{
			set{ _wh_number=value;}
			get{return _wh_number;}
		}
		/// <summary>
		/// 保管员
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

