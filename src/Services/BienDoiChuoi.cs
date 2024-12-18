using System.Globalization;
using System.Text;

namespace WebHM.Services
{
    public static class Chuoi
    {
        // Hàm loại bỏ dấu và chuyển đổi thành slug
        public static string BienDoiChuoi(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            // Bước 1: Loại bỏ dấu
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var character in normalizedString)
            {
                // Bỏ các dấu không cần thiết (NonSpacingMark)
                if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(character);
                }
            }

            // Bước 2: Chuyển thành chữ thường và thay khoảng trắng thành dấu gạch ngang
            string result = stringBuilder.ToString().Normalize(NormalizationForm.FormC);
            result = result.ToLower();  // Chuyển tất cả thành chữ thường
            result = result.Replace(" ", "-");  // Thay khoảng trắng thành dấu gạch ngang

            return result;
        }
    }
}
