using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.RegularExpressions;

namespace Ecommerce.Extention
{
    public static class Extention
    {
        public static string ToVnd(this double dongia)
        {
            return dongia.ToString("#,##0")+" đ";
        }
        public static string ToTitleCase(string str)
        {
            string result = str;
            if (!string.IsNullOrEmpty(str))
            {
                var words = str.Split(' ');
                for(int index = 0; index < words.Length; index++)
                {
                    var s = words[index];
                    if (s.Length > 0)
                    {
                        words[index] = s[0].ToString().ToUpper() + s.Substring(1);
                    }
                }
                result = string.Join(" ", words);
            }
            return result;
        }
        public static string ToUrlFriendly(this string url)
        {
            var result = url.ToLower().Trim();
            result = Regex.Replace(result, @"[áàạảãâấậẩẫăắằặẳẵ]","a");
            result = Regex.Replace(result, @"[éèẹẻẽêếềệểễ]", "e");
            result = Regex.Replace(result, @"[óòọỏõôốồộổỗơớờợởỡ]","o");
            result = Regex.Replace(result, @"[úùụủũứừựửữ]","u");
            result = Regex.Replace(result, @"[íìịỉĩ]","i");
            result = Regex.Replace(result, @"[ýỳỵỷỹ]","y");
            result = Regex.Replace(result, @"[đ]","d");
            result = Regex.Replace(result.Trim(), @"[^0-9a-z-\s]", "").Trim();
            result = Regex.Replace(result.Trim(), @"\s+","-");
            result = Regex.Replace(result, @"(\s)", "-");
            while (true)
            {
                if(result.IndexOf("--")!= -1)
                {
                    result = url.Replace("--", "-");
                }
                else
                {
                    break;
                }
            }
            return result;
        }
    }
}
