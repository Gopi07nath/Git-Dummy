using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calibration{
    public class Args : Dictionary<String , Object>
    {
		public Args() {
		}
		public Args(String key, object value) {
			Add(key, value);
		}
		public Args(String key1, object value1, String key2, object value2) {
			Add(key1, value1);
			Add(key2, value2);
		}
		public String Trim(String field) {
			return ToString(field).Trim();
		}
		public String ToString(String field) {
			if (this[field] != null) {
				return this[field].ToString();
			} else {
				return "";
			}
		}
		public void AddLike(String key, String value) {
			Add(key, "%" + value + "%");
		}
    }
}
