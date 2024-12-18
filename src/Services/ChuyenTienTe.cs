namespace WebHM.Services
{
    public static class ChuyenTienTe
    {
        public static string Dec2VND(decimal amount)
        {
            return string.Format("{0:#,0 VND}", amount);
        }
    }
}
