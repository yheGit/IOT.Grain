/**  版本信息模板在安装目录下，可自行修改。
* Collector.cs
*
* 功 能： N/A
* 类 名： Collector
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
using System.ComponentModel.DataAnnotations.Schema;
namespace IOT.Grain.Entity.DR_Model
{
	/// <summary>
	/// Collector:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
    [Table("CW_FinanceConfig", Schema = "dbo")]
	public partial class Collector
	{
		public Collector()
		{}
		#region Model
		private int _id;
		private string _cupid;
		private string _h_number;
		private string _r_number;
		private DateTime _installdate;
		private int? _userid;
		private decimal? _voltage=0M;
		private int? _isactive=0;
		/// <summary>
		/// 
		/// </summary>
        //[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// CUPID
		/// </summary>
		public string CUPId
		{
			set{ _cupid=value;}
			get{return _cupid;}
		}
		/// <summary>
		/// 堆位编号
		/// </summary>
		public string H_Number
		{
			set{ _h_number=value;}
			get{return _h_number;}
		}
		/// <summary>
		/// 手工编号
		/// </summary>
		public string R_Number
		{
			set{ _r_number=value;}
			get{return _r_number;}
		}
		/// <summary>
		/// 安装时间
		/// </summary>
		public DateTime InstallDate
		{
			set{ _installdate=value;}
			get{return _installdate;}
		}
		/// <summary>
		/// 安装人员
		/// </summary>
		public int? UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 电压
		/// </summary>
		public decimal? Voltage
		{
			set{ _voltage=value;}
			get{return _voltage;}
		}
		/// <summary>
		/// 0否1是
		/// </summary>
		public int? IsActive
		{
			set{ _isactive=value;}
			get{return _isactive;}
		}

        /// <summary>
        /// 传感器ID集合["ID1","ID2"]
        /// </summary>
        public string SensorIdArr { get; set; }
        #endregion Model

    }
}

