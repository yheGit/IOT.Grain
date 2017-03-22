/**  版本信息模板在安装目录下，可自行修改。
* WareHouse_Log.cs
*
* 功 能： N/A
* 类 名： WareHouse_Log
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
	/// WareHouse_Log:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class WareHouse_Log
	{
		public WareHouse_Log()
		{}
		#region Model
		private int _id;
		private int? _wh_id;
		private string _wh_content;
		private string _produceplace;
		private DateTime _receiptdate;
		private DateTime _inputdate;
		private decimal? _moisture=0M;
		private int? _storetype=1;
		private decimal? _incomplete=0M;
		private int? _storelevel=1;
		private decimal? _capacity=0M;
		private decimal? _impurity=0M;
		private string _mgr;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 粮仓ID
		/// </summary>
		public int? WH_ID
		{
			set{ _wh_id=value;}
			get{return _wh_id;}
		}
		/// <summary>
		/// 粮仓内容（小麦、大米）
		/// </summary>
		public string WH_Content
		{
			set{ _wh_content=value;}
			get{return _wh_content;}
		}
		/// <summary>
		/// 产地
		/// </summary>
		public string ProducePlace
		{
			set{ _produceplace=value;}
			get{return _produceplace;}
		}
		/// <summary>
		/// 收货时间
		/// </summary>
		public DateTime ReceiptDate
		{
			set{ _receiptdate=value;}
			get{return _receiptdate;}
		}
		/// <summary>
		/// 入库时间
		/// </summary>
		public DateTime InputDate
		{
			set{ _inputdate=value;}
			get{return _inputdate;}
		}
		/// <summary>
		/// 入仓水分
		/// </summary>
		public decimal? Moisture
		{
			set{ _moisture=value;}
			get{return _moisture;}
		}
		/// <summary>
		/// 存储类型(1周转)
		/// </summary>
		public int? StoreType
		{
			set{ _storetype=value;}
			get{return _storetype;}
		}
		/// <summary>
		/// 不完善粒
		/// </summary>
		public decimal? Incomplete
		{
			set{ _incomplete=value;}
			get{return _incomplete;}
		}
		/// <summary>
		/// 仓库等级(1一等、2二等)
		/// </summary>
		public int? StoreLevel
		{
			set{ _storelevel=value;}
			get{return _storelevel;}
		}
		/// <summary>
		/// 容量(738.00g/L)
		/// </summary>
		public decimal? Capacity
		{
			set{ _capacity=value;}
			get{return _capacity;}
		}
		/// <summary>
		/// 杂质(0.90%)
		/// </summary>
		public decimal? Impurity
		{
			set{ _impurity=value;}
			get{return _impurity;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Mgr
		{
			set{ _mgr=value;}
			get{return _mgr;}
		}
		#endregion Model

	}
}

