/**  版本信息模板在安装目录下，可自行修改。
* Organization.cs
*
* 功 能： N/A
* 类 名： Organization
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
	/// Organization:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Organization
	{
		public Organization()
		{}
		#region Model
		private int _id=0;
		private string _orgcode;
		private string _orgname;
		private int? _parentid;
		private int? _isactive=0;
		private DateTime _adddate;
		private DateTime _modifydate;
		/// <summary>
		/// 自动编号
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 手工编号
		/// </summary>
		public string OrgCode
		{
			set{ _orgcode=value;}
			get{return _orgcode;}
		}
		/// <summary>
		/// 组织名称
		/// </summary>
		public string OrgName
		{
			set{ _orgname=value;}
			get{return _orgname;}
		}
		/// <summary>
		/// 父ID
		/// </summary>
		public int? ParentId
		{
			set{ _parentid=value;}
			get{return _parentid;}
		}
		/// <summary>
		/// 0否、1是
		/// </summary>
		public int? IsActive
		{
			set{ _isactive=value;}
			get{return _isactive;}
		}
		/// <summary>
		/// 添加时间
		/// </summary>
		public DateTime AddDate
		{
			set{ _adddate=value;}
			get{return _adddate;}
		}
		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime ModifyDate
		{
			set{ _modifydate=value;}
			get{return _modifydate;}
		}
		#endregion Model

	}
}

