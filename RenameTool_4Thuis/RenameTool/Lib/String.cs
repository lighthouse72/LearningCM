namespace RenameTool.Lib
{
    public class String
    {
        public static string Trim(string s)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(s)) return string.Empty;
                return s.Trim();
            }
            catch (System.Exception)
            {
                return string.Empty;
            }

        }
    }
}
