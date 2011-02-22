using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperCms.Lib.VarToken
{
    public abstract class Token
    {
        public abstract string Translate();

        private static Dictionary<string, Type> _tokenList = null;
        public static Dictionary<string, Type> TokenList
        {
            get
            {
                if (_tokenList == null)
                {
                    _tokenList = new Dictionary<string, Type>();
                    var x = from i in typeof(Token).Assembly.GetTypes()
                            where i.IsSubclassOf(typeof(Token)) && i.IsAbstract == false
                            orderby i.Name ascending
                            select i;
                    foreach (var item in x)
                    {
                        _tokenList.Add(item.Name, item);
                    }
                }
                return _tokenList;
            }
        }
    }
}
