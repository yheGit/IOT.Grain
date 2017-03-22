using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Data.Base
{
    public class SqlComm
    {
        public object ShareObject;
        public object OriginalData;
        public string CommandText;
        public DbParameter[] Parameters;
        public EffentNextType EffentNextType;
        private event EventHandler _solicitationEvent;

        public event EventHandler SolicitationEvent
        {
            add
            {
                this._solicitationEvent += value;
            }
            remove
            {
                this._solicitationEvent -= value;
            }
        }
        public void OnSolicitationEvent()
        {
            if (this._solicitationEvent != null)
            {
                this._solicitationEvent(this, new EventArgs());
            }
        }
        public SqlComm()
        {
        }
        public SqlComm(string sqlText, SqlParameter[] para)
        {
            this.CommandText = sqlText;
            this.Parameters = para;
        }
        public SqlComm(string sqlText, SqlParameter[] para, EffentNextType type)
        {
            this.CommandText = sqlText;
            this.Parameters = para;
            this.EffentNextType = type;
        }
    }


    public enum EffentNextType
    {
        None,
        WhenHaveContine,
        WhenNoHaveContine,
        ExcuteEffectRows,
        SolicitationEvent
    }
}
