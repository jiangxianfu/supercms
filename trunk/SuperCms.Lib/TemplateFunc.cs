using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SuperCms.Lib.VarToken;

namespace SuperCms.Lib
{
    public partial class TemplateFunc
    {
        public static string Label(string sqlfilename, string tagfilename)
        {
            StringBuilder sb = new StringBuilder();
            LabelTemplate lt = new LabelTemplate();
            DBClassesDataContext db = new DBClassesDataContext();
            string sql = FileManager.LoadContent(Utils.GetMapPath(GlobalConst.virtualFloderByLabelSql), sqlfilename);
            DataTable table = db.ExecDataTable(CommandType.Text, sql);
            foreach (DataRow item in table.Rows)
            {
                sb.Append(lt.Generated(GlobalConst.virtualFloderByLabelTag, tagfilename, 1, item));
            }
            return sb.ToString();
        }
        public static string Var(string varname)
        {
            varname = "Token_" + varname.ToLower();
            if (Token.TokenList.ContainsKey(varname))
            {
                Token token = Activator.CreateInstance(Token.TokenList[varname]) as Token;
                return token.Translate();
            }
            else
            {
                return "";
            }
        }
    }
}
